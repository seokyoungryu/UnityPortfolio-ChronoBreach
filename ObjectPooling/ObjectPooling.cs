using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.SceneManagement;


public class ObpInfo
{
    public string obpName;
    public GameObject obpGo;

    public ObpInfo() { }
    public ObpInfo(string name, GameObject go)
    {
        obpName = name;
        obpGo = go;
    }
}

public class ObjectPooling : Singleton<ObjectPooling>
{
    [SerializeField] private PoolDataContainer[] poolDataContainers;

    [SerializeField] private List<EffectPoolData> effectPoolData = new List<EffectPoolData>();

    [SerializeField] private Transform rootObject = null;

    private List<ObpInfo> activeObps = new List<ObpInfo>();
    private Dictionary<string, Queue<GameObject>> dataDictionary = new Dictionary<string, Queue<GameObject>>();
    private HashSet<GameObject> useObjects = new HashSet<GameObject>();

    #region Events
    public delegate void OnExcuteAfterCreate();
    public event OnExcuteAfterCreate onExcuteAfterCreate;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        SetEffectDataToList();
        InitPoolCreateAndDisable();
        onExcuteAfterCreate?.Invoke();
        Debug.Log("OBP 실행 Awake");

    }
 

    public void DisableAllActive()
    {
        if (activeObps.Count <= 0) return;
        for (int i = 0; i < activeObps.Count; i++)
        {
            Debug.Log("Set - " + activeObps[i].obpName);
            SetOBP(activeObps[i].obpName, activeObps[i].obpGo);
        }

        activeObps.Clear();

    }


    private void SetEffectDataToList() //툴 오브젝트 풀링은 이런식으로 더하면 될듯?  밑에보다 최적화하기 (캐싱이나 그런거(
    {
        effectPoolData = new List<EffectPoolData>();
        for (int i = 0; i < EffectManager.Instance.GetEffectData().effectClips.Length; i++)
        {
            EffectPoolData effectPool = new EffectPoolData();
            effectPool.idForEffect = i;
            effectPool.name = EffectManager.Instance.GetEffectData().effectClips[i].effectName;
            effectPool.count = 5;
            effectPool.prefab = EffectManager.Instance.GetEffectData().effectClips[i].effectPrefab;
            effectPoolData.Add(effectPool);
        }
    }


    private void InitPoolCreateAndDisable()
    {
        if (poolDataContainers != null)
        {
            foreach (PoolDataContainer container in poolDataContainers)
            {
                foreach (PoolData data in container.Pools)
                {
                    Queue<GameObject> dataQueue = new Queue<GameObject>();
                    int count = (data.count > 0) ? data.count : 0;
                    CreateOBP(data, dataQueue, count);
                    dataDictionary.Add(data.name, dataQueue);
                }
            }
        }


        if (effectPoolData != null)
        {
            foreach (PoolData effectData in effectPoolData)
            {
                Queue<GameObject> effectDataQueue = new Queue<GameObject>();
                int count = (effectData.count > 0) ? effectData.count : 0;
                CreateOBP(effectData, effectDataQueue, count);
                dataDictionary.Add(effectData.name, effectDataQueue);
            }
        }

    }


    private void CreateOBP(PoolData data, Queue<GameObject> dataQueue, int count = 0)
    {
        Transform parent;
        if (data.parent == null)
        {
            parent = new GameObject(data.name).transform;
            parent.SetParent(rootObject);
            data.parent = parent;
        }

        for (int i = 0; i < count; i++)
        {
            if (data.prefab == null)
                return;

            GameObject dataPrefab = Instantiate(data.prefab, Vector3.zero, Quaternion.identity);
            dataPrefab.transform.SetParent(data.parent);
            dataPrefab.name = data.name;
            dataPrefab.SetActive(false);
            QuestTargetMarker marker = dataPrefab.GetComponentInChildren<QuestTargetMarker>();
            if (marker != null)
                marker.CheckIsTarget();
            dataQueue.Enqueue(dataPrefab);
        }
    }

    public GameObject GetOBP(string obpName)
    {
        foreach (PoolDataContainer container in poolDataContainers)
        {
            foreach (PoolData data in container.Pools)
            {
                if (data.name == obpName)
                {
                    // 1. 풀에 아무것도 없으면 생성
                    if (dataDictionary[data.name].Count <= 0)
                    {
                        CreateOBP(data, dataDictionary[data.name]);
                    }

                    // 2. 모든 오브젝트가 사용 중이면 추가 생성
                    bool isAllActiveOrUsed = true;
                    foreach (GameObject go in dataDictionary[data.name])
                    {
                        if (go == null || go.activeInHierarchy || useObjects.Contains(go)) continue;
                        isAllActiveOrUsed = false;
                        break;
                    }

                    if (isAllActiveOrUsed)
                    {
                        CreateOBP(data, dataDictionary[data.name], 1);
                    }

                    // 3. 꺼낼 수 있는 오브젝트 찾기
                    int loopCount = dataDictionary[data.name].Count;
                    GameObject retGo = null;

                    for (int i = 0; i < loopCount; i++)
                    {
                        GameObject go = dataDictionary[data.name].Dequeue();

                        if (go == null)
                        {
                            continue;
                        }

                        if (go.activeInHierarchy || useObjects.Contains(go))
                        {
                            dataDictionary[data.name].Enqueue(go); // 다시 큐에 넣기
                            continue;
                        }

                        retGo = go;
                        break;
                    }

                    // 4. 적당한 오브젝트 없으면 생성 후 다시 꺼내기
                    if (retGo == null)
                    {
                        CreateOBP(data, dataDictionary[data.name], 1);
                        retGo = dataDictionary[data.name].Dequeue();
                    }

                    retGo.SetActive(true);
                    useObjects.Add(retGo); // 사용 중으로 등록

                    if (data.includeAllDisable)
                        activeObps.Add(new ObpInfo(data.name, retGo));

                    return retGo;
                }
            }
        }

       // Debug.Log($"<color=red> {obpName} (OBPManager) NULL </color>");
        return null;
    }


    public GameObject GetEffectOBP(int index)
    {
        foreach (EffectPoolData effectData in effectPoolData)
        {
            if (effectData.idForEffect == index)
            {
                if (dataDictionary[effectData.name].Count <= 0)
                    CreateOBP(effectData, dataDictionary[effectData.name], 1);
                int loopCount = dataDictionary[effectData.name].Count;
                GameObject retGo = null;

                for (int i = 0; i < loopCount; i++)
                {
                    GameObject go = dataDictionary[effectData.name].Dequeue();

                    if (go == null)
                    {
                        continue;
                    }

                    if (go.activeInHierarchy || useObjects.Contains(go))
                    {
                        dataDictionary[effectData.name].Enqueue(go); // 다시 큐에 넣기
                        continue;
                    }

                    retGo = go;
                    break;
                }

                if (retGo == null)
                {
                    CreateOBP(effectData, dataDictionary[effectData.name], 1);
                    retGo = dataDictionary[effectData.name].Dequeue();
                }

                retGo.SetActive(true);
                activeObps.Add(new ObpInfo(effectData.name, retGo));
                useObjects.Add(retGo);
                return retGo;
            }
        }
        return null;
    }



    public GameObject GetOBPParentContainer(string obpName)
    {
        foreach (PoolDataContainer container in poolDataContainers)
        {
            foreach (PoolData data in container.Pools)
            {
                if (data.name == obpName)
                    return data.parent.gameObject;
            }
        }

        return null;
    }
    public GameObject GetEffectOBPParentContainer(string obpName)
    {
        foreach (PoolData data in effectPoolData)
        {
            if (data.name == obpName)
                return data.parent.gameObject;
        }
        return null;
    }


    public void SetOBP(string obpName, GameObject go)
    {
        if (go == null)
        {
            return;
        }
        if (dataDictionary.ContainsKey(obpName) == false)
        {
            return;
        }

        if (useObjects.Contains(go))
            useObjects.Remove(go);

        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(false);
        dataDictionary[obpName].Enqueue(go);
    }

#if UNITY_EDITOR
    [ContextMenu("Make To List")]
    public void CreateEnumList()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();
        int index = 0;

        for (int i = 0; i < poolDataContainers.Length; i++)
        {
            for (int j = 0; j < poolDataContainers[i].Pools.Count; j++)
            {
                sb.AppendLine("    " + GetInspectorName(poolDataContainers[i], poolDataContainers[i].Pools[j]));
                sb.AppendLine("    " + poolDataContainers[i].Pools[j].name + " = " + index + ",");
                Debug.Log(poolDataContainers[i].Pools[j].name);
                index++;
            }
        }
        EditorHelper.CreateEnumList("ObjectPoolingList", sb);
    }

    private string GetInspectorName(PoolDataContainer container, PoolData pool)
    {
        string retName = "[InspectorName(|*|)]";
        retName = retName.Replace("*", container.PoolZipName + "/" + pool.name);
        return retName.Replace('|', '"');
    }
#endif
}
