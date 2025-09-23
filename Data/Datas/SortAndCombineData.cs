using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChildWords
{
    public string[] keywords = new string[0];

    public ChildWords(int index)
    {
        keywords = new string[index];
    }
}

public class SortAndCombineData : MonoBehaviour
{
    public GameObject rootObject = null;
    public List<GameObject> objectHierarchy = new List<GameObject>();
    public string combineMeshFolderPath = "Assets/10.Resources/Environment_CombineTest/CombineMeshs/";
    public string originCombineMeshFolderPath = "Assets/10.Resources/Environment_CombineTest/CombineMeshs/";
    private  List<Transform> sortedTr = new List<Transform>();

    #region Sort
    public GameObject SortGameObject(GameObject root, List<string> childParentName, List<ChildWords> childWords, List<SortKeyWordType> sortkeyword, bool onlyUnderChild)
    {
        rootObject = root;
        objectHierarchy.Clear();
        SetHierarchyObject(root, onlyUnderChild);
        if (!onlyUnderChild)
            SetParentNull();

        SortObject(childParentName, childWords, sortkeyword);

        return rootObject;
    }

    public GameObject SortOnlyTarget(GameObject root, List<string> childParentName, List<ChildWords> childWords, List<SortKeyWordType> sortkeyword, bool onlyUnderChild) //자식들 중에 해당 키워드인 오브젝트만 생성 폴더로 이동.
    {
        rootObject = root;
        sortedTr.Clear();
        for (int i = 0; i < childParentName.Count; i++)
        {
            objectHierarchy.Clear();
            GameObject parent = new GameObject(childParentName[i]);
            parent.transform.parent = root.transform;
            sortedTr.Add(parent.transform);
            SetOnlyTargetHierarchyObject(root, parent.transform, childWords[i], sortkeyword[i], onlyUnderChild);

            foreach (GameObject obj in objectHierarchy.ToArray())
            {
                obj.transform.parent = parent.transform;
            }
        }

        return rootObject;

    }

    private void SetHierarchyObject(GameObject root, bool onlyUnderChild)
    {
        if (root.transform.childCount <= 0) return;

        foreach (Transform obj in root.transform)
        {
            objectHierarchy.Add(obj.gameObject);
            if (!onlyUnderChild)
                SetHierarchyObject(obj.gameObject, onlyUnderChild);

        }
    }

    private void SetOnlyTargetHierarchyObject(GameObject root, Transform parent, ChildWords childWords, SortKeyWordType sortKeyWord, bool onlyUnderChild)
    {
        if (root.transform.childCount <= 0) return;

        if (sortKeyWord == SortKeyWordType.ANY)
        {
            foreach (Transform obj in root.transform)
                if (!sortedTr.Contains(obj))
                    objectHierarchy.Add(obj.gameObject);
        }
        else
        {
            foreach (Transform obj in root.transform)
            {
                if (CanSortKeyWord(obj.gameObject, childWords, sortKeyWord))
                    objectHierarchy.Add(obj.gameObject);

                if (!onlyUnderChild)
                    SetOnlyTargetHierarchyObject(obj.gameObject, parent, childWords, sortKeyWord, onlyUnderChild);
            }
        }

    }


    private void SetParentNull()
    {
        foreach (GameObject go in objectHierarchy)
        {
            go.transform.parent = rootObject.transform;
        }
    }

    private void SortObject(List<string> childParentName, List<ChildWords> childWords, List<SortKeyWordType> sortkeyword)
    {
        for (int i = 0; i < childParentName.Count; i++)
        {
            GameObject parent = new GameObject(childParentName[i]);
            parent.transform.SetParent(rootObject.transform);

            List<GameObject> childs = SortKeyword(childWords[i], sortkeyword[i]);

            for (int j = 0; j < childs.Count; j++)
                childs[j].transform.SetParent(parent.transform);
        }
    }


    private List<GameObject> SortKeyword(ChildWords childWord, SortKeyWordType sortKeyWord)
    {
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < objectHierarchy.Count; i++)
            if (CanSortKeyWord(objectHierarchy[i], childWord, sortKeyWord))
                result.Add(objectHierarchy[i]);

        foreach (GameObject go in objectHierarchy.ToArray())
        {
            for (int i = 0; i < result.Count; i++)
                if (go == result[i])
                    objectHierarchy.Remove(go);
        }

        return result;
    }


    private bool CanSortKeyWord(GameObject go ,ChildWords childWord, SortKeyWordType sortKeyWord)
    {
        if (sortKeyWord == SortKeyWordType.AND)
        {
            if (CheckANDKeyword(go, childWord.keywords))
                return true;
        }
        else if (sortKeyWord == SortKeyWordType.OR)
        {
            if (CheckORKeyword(go, childWord.keywords))
                return true;
        }
        else if (sortKeyWord == SortKeyWordType.PERFECT)
        {
            if (!CheckPerfectKeyword(go, childWord.keywords))
                return true;
        }
        else if (sortKeyWord == SortKeyWordType.ANY)
            return true;

        return false;
    }


    private bool CheckANDKeyword(GameObject go, string[] keyword)
    {
        bool isKey = true;
        foreach (string key in keyword)
        {
            if (!go.name.Contains(key) ||  key == "")
                isKey = false;
        }

        return isKey;
    }

    private bool CheckORKeyword(GameObject go, string[] keyword)
    {
        foreach (string key in keyword)
        {
            if (go.name.Contains(key) && key != "")
                return true;
        }

        return false;
    }

    private bool CheckPerfectKeyword(GameObject go, string[] keyword)
    {
        foreach (string key in keyword)
        {
            if (go.name == key && key != "")
                return true;
        }

        return false;
    }
    #endregion

    #region Combine 
#if UNITY_EDITOR
    //우선 원본 Root Obnject에 저장함. 그리고 작업은 매개변수로 변환 이름, 변환할 자식의 root 오브젝트,
    //그래서 변환할 자식의 오브젝트의 자식들을 CombineObject()로 하나로 합치고 root오브젝트에 넣음,
    //즉 -> COmbine()으로 원본 object 와 작업 List를 받아오고 작업 하나당 combine 실행함.
    public GameObject Combine(GameObject root, List<GameObject> childObject, List<string> parentName, ChildWords exceptWords, SortKeyWordType exceptType)
    {
        rootObject = root;
        GameObject[] applyExceptObject;
        if (childObject == null)
        {
            List<GameObject> childObjectTemp = new List<GameObject>();
            foreach (Transform tr in rootObject.transform)
                childObjectTemp.Add(tr.gameObject);
            applyExceptObject = ExceptSortKeyword(childObjectTemp, exceptWords, exceptType);
        }
        else
            applyExceptObject = ExceptSortKeyword(childObject, exceptWords, exceptType);

        //키워드 제외된 작업 목록
        for (int i = 0; i < applyExceptObject.Length; i++)
        {
            GameObject[] childGo = new GameObject[applyExceptObject[i].transform.childCount];
            for (int x = 0; x < applyExceptObject[i].transform.childCount; x++)
                childGo[x] = applyExceptObject[i].transform.GetChild(x).gameObject;

            if (parentName.Count <= 0 || (parentName.Count - 1) < i)
                CombineChildObject(childGo, rootObject, applyExceptObject[i].transform.localPosition, "Combine_" + i.ToString());
            else
                CombineChildObject(childGo, rootObject, applyExceptObject[i].transform.localPosition, parentName[i]);

        }

        foreach (GameObject go in applyExceptObject)
            DestroyImmediate(go);

        return rootObject;
    }
#endif

#if UNITY_EDITOR
    private void CombineChildObject(GameObject[] gos, GameObject parent, Vector3 parentPosition, string parentName)
    {
        if (gos.Length <= 0) return;
        #region position Setting
        Vector3 rootOriginPosition = rootObject.transform.parent == null ? rootObject.transform.position : rootObject.transform.localPosition;
        rootObject.transform.position = Vector3.zero;
        #endregion

        List<MeshFilter> filters = new List<MeshFilter>();
        int filterIndex = 0;
        for (int i = 0; i < gos.Length; i++)
        {
            if (gos[i].GetComponent<MeshFilter>() != null)
            {
                filters.Add(new MeshFilter());
                filters[filterIndex] = gos[filterIndex].GetComponent<MeshFilter>();
                filterIndex++;
            }
        }

        int totalVertexs = 0;
        CombineInstance[] combines = new CombineInstance[filters.Count];
        for (int i = 0; i < filters.Count; i++)
        {
            combines[i].mesh = filters[i].sharedMesh;
            combines[i].transform = filters[i].transform.localToWorldMatrix;
            if (filters[i] != null && filters[i].sharedMesh != null)
                totalVertexs += filters[i].sharedMesh.vertexCount;
        }

        Mesh mesh = new Mesh();

        if (totalVertexs < ushort.MaxValue)
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
        else
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.CombineMeshes(combines, true);

        mesh.Optimize();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        MeshUtility.Optimize(mesh);
        MeshUtility.SetMeshCompression(mesh, ModelImporterMeshCompression.High);
        mesh.UploadMeshData(true);
        RemoveUnusedChannels(mesh);

        GameObject newGo = new GameObject(parentName);
        newGo.AddComponent<MeshFilter>().sharedMesh = mesh;
        newGo.AddComponent<MeshRenderer>().sharedMaterial = filters[0].GetComponent<MeshRenderer>().sharedMaterial;
        newGo.transform.parent = parent.transform;

        if (!System.IO.Directory.Exists(combineMeshFolderPath))
        {
            System.IO.Directory.CreateDirectory(combineMeshFolderPath);
        }

        int fileIndex = 1;
        string fileName = combineMeshFolderPath + newGo.transform.parent.name + "_" + parentName;
        string meshPath = fileName + ".asset";
        while (System.IO.File.Exists(meshPath))
        {
            fileName += fileIndex;
            meshPath = fileName + ".asset";
            fileIndex++;
        }

        AssetDatabase.CreateAsset(mesh, meshPath);
        AssetDatabase.SaveAssets();


        if (rootObject.transform.parent == null)
            rootObject.transform.position = rootOriginPosition;
        else
            rootObject.transform.localPosition = rootOriginPosition;

        Debug.Log("Combine_" + fileName + newGo.name);
    }

    private GameObject[] ExceptSortKeyword(List<GameObject> gos, ChildWords childWords, SortKeyWordType sortKeyWordType)
    {
        if (childWords.keywords.Length <= 0 || sortKeyWordType == SortKeyWordType.ANY) return gos.ToArray();

        List<GameObject> resultGameobject = new List<GameObject>();
        foreach (GameObject go in gos)
        {
            if (sortKeyWordType == SortKeyWordType.AND)
            {
                if (!CheckANDKeyword(go, childWords.keywords))
                    resultGameobject.Add(go);
            }
            else if (sortKeyWordType == SortKeyWordType.OR)
            {
                if (!CheckORKeyword(go, childWords.keywords))
                    resultGameobject.Add(go);
            }
        }
        return resultGameobject.ToArray();
    }
#endif

    #endregion


    #region Sort & Combine

    public void SetLodComponent(LODGroup lodRoot, List<GameObject> lodChilds, float[] screenTransition)
    {
        if (lodRoot == null || lodChilds.Count <= 0) return;

        lodRoot.fadeMode = LODFadeMode.CrossFade;

        LOD[] lod = lodRoot.GetLODs();
        for (int i = 0; i < 3; i++)
        {
            if (lodChilds.Count <= i)       // 0  ,  1  ,  2     || 
            {                               // 0     1     2
                Debug.Log("SetLodComponent Legnth 0 : " + lodRoot.name);
                continue;
            }

            Renderer[] renderers = lodChilds[i]?.GetComponentsInChildren<Renderer>();
            lod[i].renderers = renderers;
            lod[i].screenRelativeTransitionHeight = screenTransition[i];
        }

        lodRoot.SetLODs(lod);
        lodRoot.RecalculateBounds();

    }


    #endregion


    private void RemoveUnusedChannels(Mesh mesh)
    {
        if (mesh == null)
        {
            Debug.Log("Remove : " + mesh.name);
            return;
        }
        //if (!mesh.isReadable)
        //{
        //    Debug.Log("isReadable : " + mesh.name);
        //    return;
        //}

        // 제거할 UV 및 컬러 채널을 설정합니다.
        bool[] uvChannelUsed = new bool[8];
        bool colorChannelUsed = false;

        for (int i = 0; i < mesh.vertexCount; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (mesh.HasVertexAttribute((UnityEngine.Rendering.VertexAttribute.TexCoord0 + j)))
                {
                    uvChannelUsed[j] = true;
                }
            }
            if (mesh.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                colorChannelUsed = true;
            }
        }

        for (int i = 0; i < 8; i++)
        {
            if (!uvChannelUsed[i])
            {
                if (mesh.isReadable)
                    mesh.SetUVs(i, new List<Vector4>());
            }
        }

        if (!colorChannelUsed)
        {
            if (mesh.isReadable)
                mesh.colors = new Color[0];
        }
    }

    public bool HaveAnyKeyword(SortKeyWordType[] types)
    {
        for (int i = 0; i < types.Length; i++)
            if (types[i] == SortKeyWordType.ANY)
                return true;

        return false;
    }
}

