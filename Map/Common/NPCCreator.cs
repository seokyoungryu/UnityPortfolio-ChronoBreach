using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPCCreator : MonoBehaviour
{
    enum SpawnNPCType
    {
        START_PLAY = 0,
        TRIGGER = 1,
    }
    [SerializeField] private SpawnNPCType spawnNPCType;
    [SerializeField] private DungeonSpawnPositionList spawnList;
    [SerializeField] private List<SpawnNPCInfo> infos = new List<SpawnNPCInfo>();
    [SerializeField] private LayerMask target;
    [SerializeField] private Vector3 triggerPos;
    [SerializeField] private Vector3 triggerExtend;

    private bool isCreated = false;
    private List<AIController>  controllers = new List<AIController>();


    private Vector3 gizmoPos;

    private void Awake()
    {
        if (spawnNPCType == SpawnNPCType.START_PLAY)
            Create(infos);
        else if (spawnNPCType == SpawnNPCType.TRIGGER)
            StartCoroutine(Check());

        ScenesManager.Instance.onExucteInit += Clear;
    }

    private void Start()
    {
        
    }

    private IEnumerator Check()
    {
        if (isCreated) yield break;

        bool isTigger = false;
        Collider[] colls;
        while (!isTigger)
        {
            colls = Physics.OverlapBox(triggerPos, triggerExtend / 2f, Quaternion.identity, target);
            if(colls.Length > 0)
            {
                isTigger = true;
                Create(infos);
                isCreated = true;
            }
            yield return null;

        }
    }

    [ContextMenu("积己1")]

    public void CreateTest()
    {
        Create(infos);
    }

    private void Create(List<SpawnNPCInfo> infos)
    {
        controllers.Clear();
        for (int i = 0; i < infos.Count; i++)
        {
            CreateAI(infos[i], i + 0000);
        }
    }

    [ContextMenu("昏力1")]
    private void Clear()
    {
        Debug.Log($"<color=purple> Clear !</color>");

        for (int i = 0; i < controllers.Count; i++)
        {
            if (controllers[i] == null || controllers[i].gameObject == null)
            {
                Debug.Log($"<color=red> {i} - {controllers[i]} Clear NULL </color>");
                continue;
            }
            ObjectPooling.Instance.SetOBP(controllers[i].OBPName, controllers[i].obpGo);
        }
        controllers.Clear();
    }

    private void CreateAI(SpawnNPCInfo info, int id)
    {
       Vector3 spawnPos = spawnList.GetSpawnPosition(info.AiInfo.SpawnPositionIndex);

        AIController controller = AIManager.Instance.CreateAI(info.AiInfo.EnemyObpName);

        if(controller == null)
        {
            Debug.Log($"<color=red> {info.AiInfo.EnemyInfoList.ToString()} CreateAI NULL </color>");
        }


        for (int i = 0; i < info.AiInfo.WayPointInfos.Length; i++)
            controller.SetWayPoints(info.AiInfo.WayPointInfos[i], spawnList.GetWayPointsTransform(info.AiInfo.WayPointInfos[i].WayIndex));

        Vector3 rotation;
        if (info.AiInfo.SpawnRotation == -Vector3.one)
            rotation = spawnList.GetSpawnRotation(info.AiInfo.SpawnPositionIndex).eulerAngles;
        else rotation = info.AiInfo.SpawnRotation;

        controller.TranslatePosition(spawnPos);
        controller.RotateByVector(rotation);
        controller.aIVariables.patrolSpeed = info.PatrollSpeed;
        controller.SetNavSpeed(info.PatrollSpeed);

        NpcController npcController = controller.GetComponentInChildren<NpcController>();
        if(npcController != null)
        {
            npcController.ID = id;
            QuestManager.Instance.SetNewNpcController(npcController);
            npcController.QuestReporter?.SetTarget(info.TaskTarget);
            if (info.QuestList != null) npcController.NpcQuestGiver.QuestList = info.QuestList;
            if (info.DialogFile != null) npcController.NpcFunction.interacDialog = info.DialogFile;
            Debug.Log("NPC 积己!");
        }
        StoreNpcFunction storeFunc = controller.GetComponentInChildren<StoreNpcFunction>();
        if(storeFunc != null)
            QuestManager.Instance.SetNewNpcStoreFunction(storeFunc);

        if (info.TaskTarget != null)
        {
            npcController?.TargetMarker.SetTaskTarget(info.TaskTarget);
            controller.GetComponentInChildren<SimpleNPCWorldCanvas>()?.SettingName(info.TaskTarget.DisplayName);
            controller.questReporter?.SetTarget(info.TaskTarget);
        }


        if (info.AiInfo.IsForceRunning)
            controller.aiConditions.IsForceRunning = info.AiInfo.IsForceRunning;

        controller.transform.localScale = info.AiInfo.SpawnSize;
        controller.aIFSMVariabls.resetPos = info.AiInfo.SpawnPosition;
        controller.targetLayer = info.AiInfo.TargetLayer;
        controller.aiStatus.ExcuteOnHPHUD();

        controllers.Add(controller);
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (infos == null || infos.Count <= 0 || spawnList == null)
            return;

        if(spawnNPCType == SpawnNPCType.TRIGGER)
        {
            Handles.DrawWireCube(triggerPos + Vector3.up * triggerExtend.y/2f, triggerExtend);
        }


        for (int i = 0; i < infos.Count; i++)
        {
            if (infos[i].AiInfo.SpawnPositionIndex == -1) continue;

            gizmoPos = spawnList.GetSpawnPosition(infos[i].AiInfo.SpawnPositionIndex);
            Handles.Label(gizmoPos + Vector3.up * 1.5f, infos[i].NPCGizmosName);
            Handles.color = Color.green;
            Handles.DrawWireCube(gizmoPos, Vector3.one * 2f);

            Handles.color = Color.blue;
            if (infos[i].AiInfo.SpawnRotation == -Vector3.one)
            {
                Transform tr = spawnList.GetSpawnTransform(infos[i].AiInfo.SpawnPositionIndex);
                Handles.DrawLine(gizmoPos , gizmoPos + tr.forward * 2f);
            }
            else
            {
                Quaternion rot = Quaternion.Euler(infos[i].AiInfo.SpawnRotation);
                Handles.DrawLine(gizmoPos , gizmoPos + (rot * Vector3.forward) * 2f);
            }
        }
    }

#endif
}


[System.Serializable]
public class SpawnNPCInfo
{
    [SerializeField] private string npcGizmosName = string.Empty;
    [SerializeField] private float patrollSpeed = 0.55f;
    [SerializeField] private StringTaskTarget taskTarget = null;
    [SerializeField] private QuestList questList = null;
    [SerializeField] private DialogFile dialogFile = null;
    [SerializeField] private BaseDungeonEnemyInfo aiInfo;

    public string NPCGizmosName => npcGizmosName;
    public float PatrollSpeed => patrollSpeed;
    public StringTaskTarget TaskTarget => taskTarget;
    public QuestList QuestList => questList;
    public DialogFile DialogFile => dialogFile;

    public BaseDungeonEnemyInfo AiInfo => aiInfo;
}
