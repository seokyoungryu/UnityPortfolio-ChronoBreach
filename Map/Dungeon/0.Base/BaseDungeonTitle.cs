using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum DrawEnemyType
{
    NORAML = 0,
    TARGET = 1,
    PLAYABLE = 2,
}
public abstract class BaseDungeonTitle : BaseScriptableObject
{
    [SerializeField] protected SoundList baseBGM;
    [SerializeField] protected SoundList bossBGM;

    [SerializeField] private bool isDungeonClear = false;
    [SerializeField] protected string targetString = string.Empty;
    [SerializeField] protected string initGlobalNotifier = string.Empty;
    [SerializeField] protected StringTaskTarget taskTarget;   
    [SerializeField] protected BaseDungeonCateogry dungeonCateogry;
    [SerializeField] protected NormalDungeonCondition dungeonCondition;
    [SerializeField] protected DungeonReward dungeonReward;
    [SerializeField] protected BaseDungeonMapData dungeonMapData;
    [SerializeField] protected DungeonSpawnPositionList dungeonSpawnPosition;

    protected PlayerStateController originController = null;
    protected PlayerStateController excuteController = null;
    [HideInInspector] public CoroutineForDungeon dungeonCoroutine;
    private int entryCount = 0;
    private int completeCount = 0;
    private Vector3 drawBarrierPos;
    private Vector3 drawSpawnTriggerPos;

    public PlayerStateController OriginController => originController;
    public PlayerStateController ExcuteController => excuteController;
    public BaseDungeonMapData DungeonMapData => dungeonMapData;
    public DungeonSpawnPositionList DungeonSpawnPosition => dungeonSpawnPosition;
    public BaseDungeonCateogry DungeonCateogry => dungeonCateogry;
    public StringTaskTarget TaskTarget => taskTarget;
    public DungeonReward DungeonReward => dungeonReward;
    public NormalDungeonCondition DungeonCondition => dungeonCondition;
    public bool IsDungeonClear => isDungeonClear;
    public SoundList BaseBGM => baseBGM;
    public SoundList BossBGM => bossBGM;
    public string TargetString => targetString;
    public string InitGlobalNotifier => initGlobalNotifier;

    //던전 Entry시 컨트롤러 세팅.
    public virtual void SettingControllers(PlayerStateController controller)
    {
        originController = controller;
        excuteController = dungeonCateogry.InitControllerSetting(this);

    }

    public virtual void ClearObj()
    {
       

    }

    public void SetDungeonCoroutine(CoroutineForDungeon co) => dungeonCoroutine = co;

    public abstract void Excute();

    public abstract BaseDungeonTitle GetClone();

    public virtual void ExcuteDrawCurrentWave(int currentIndex) { }


    public virtual void ExcuteDrawAllWave() { }


    public virtual void DrawSpawnTriggerABarrier(SpawnTriggerInfo[] triggerInfos, SpawnBarrierInfo[] barrierInfos, SpawnBarrierInfo[] existBarrierInfos, CheckBossBGMInfo info)
    {
        DrawTrigger(triggerInfos);
        DrawBarrier(barrierInfos);
        DrawExistBarrier(existBarrierInfos);
        DrawCheckBossBGM(info);
    }

    public void DungeonClear() => isDungeonClear = true;
    public void DungeonReset() => isDungeonClear = false;




    protected void DrawEnemySpawnPos(BaseDungeonEnemyInfo[] infos, int wave, int round, DrawEnemyType type)
    {
        for (int i = 0; i < infos.Length; i++)
        {
            Vector3 pos = dungeonSpawnPosition.GetSpawnPosition(infos[i].SpawnPositionIndex);
#if UNITY_EDITOR

            if (type == DrawEnemyType.PLAYABLE)
            {
                Handles.color = Color.white;
                Handles.Label(pos + Vector3.up * 1f, "Wave" + wave + "-" + round + $" 아군({i}) 생성 위치");
            }
            else if (type == DrawEnemyType.NORAML)
            {
                Handles.color = Color.red;
                Handles.Label(pos + Vector3.up * 1f, "Wave" + wave + "-" + round + $" 적({i}) 생성 위치");
            }
            else if (type == DrawEnemyType.TARGET)
            {
                if (infos[i] is TargetDungeonEnemyInfo)
                {
                    if ((infos[i] as TargetDungeonEnemyInfo).IsTarget)
                    {
                        Handles.color = Color.magenta;
                        Handles.Label(pos + Vector3.up * 1f, "Wave" + wave + "-" + round + $" 타겟({i}) 생성 위치");
                    }
                    else
                    {
                        Handles.color = Color.red;
                        Handles.Label(pos + Vector3.up * 1f, "Wave" + wave + "-" + round + $" 적({i}) 생성 위치");
                    }
                }
            }

            Handles.DrawWireCube(pos + Vector3.up * 1f, Vector3.one * 2f);
            Handles.color = Color.green;
            if (infos[i].SpawnRotation == -Vector3.one)
            {
                Transform tr = dungeonSpawnPosition.GetSpawnTransform(infos[i].SpawnPositionIndex);
                Handles.DrawLine(pos + Vector3.up * 1, pos + Vector3.up * 1 + tr.forward * 3f);
            }
            else
            {
                Quaternion rot = Quaternion.Euler(infos[i].SpawnRotation);
                Handles.DrawLine(pos + Vector3.up * 1, pos + Vector3.up * 1 + (rot * Vector3.forward) * 3f);
            }
#endif
        }
    }

    protected void DrawPlayerSpawnPos()
    {
        Vector3 playerPos = dungeonSpawnPosition.GetSpawnPosition(dungeonMapData.SpawnIndex);
#if UNITY_EDITOR
        Handles.color = Color.blue;
        Handles.Label(playerPos + Vector3.up * 2.5f, "플레이어 생성 위치") ;
        Handles.DrawWireCube(playerPos + Vector3.up * 1f, Vector3.one * 2f);
        if (dungeonMapData.PlayerRotation == -Vector3.one)
        {
            Transform tr = dungeonSpawnPosition.GetSpawnTransform(dungeonMapData.SpawnIndex);
            Handles.DrawLine(playerPos + Vector3.up * 1, playerPos + Vector3.up * 1 + tr.forward * 3f);
        }
        else
        {
            Quaternion rot = Quaternion.Euler(dungeonMapData.PlayerRotation);
            Handles.DrawLine(playerPos + Vector3.up * 1, playerPos + Vector3.up * 1 + (rot * Vector3.forward) * 3f);
        }
#endif
    }

    protected void DrawTrigger(SpawnTriggerInfo[] triggerInfos)
    {
        if (triggerInfos.Length <= 0) return;

        for (int i = 0; i < triggerInfos.Length; i++)
        {
            drawSpawnTriggerPos = dungeonSpawnPosition.GetTriggerTransform(triggerInfos[i].positionIndex).localPosition;
#if UNITY_EDITOR
            Handles.color = Color.red;
            Handles.Label(drawSpawnTriggerPos + Vector3.up * 2f, "Wave Entry Trigger");
            Handles.color = Color.green;
            Handles.DrawWireCube(drawSpawnTriggerPos + Vector3.up * triggerInfos[i].extend.y / 2f, triggerInfos[i].extend);
#endif
        }
    }

    protected void DrawCheckBossBGM(CheckBossBGMInfo info)
    {
        if (info == null || info.positionIndex == -1) return;
        
            drawSpawnTriggerPos = dungeonSpawnPosition.GetTriggerTransform(info.positionIndex).localPosition;
#if UNITY_EDITOR
            Handles.color = Color.red;
            Handles.Label(drawSpawnTriggerPos + Vector3.up * 2f, "Check Boss BGM");
            Handles.color = Color.green;
            Handles.DrawWireCube(drawSpawnTriggerPos + Vector3.up * info.spawnSize.y / 2f, info.spawnSize);
#endif
    }

    protected void DrawBarrier(SpawnBarrierInfo[] barrierInfos)
    {
        if (barrierInfos.Length <= 0) return;

        for (int i = 0; i < barrierInfos.Length; i++)
        {
            drawBarrierPos = dungeonSpawnPosition.GetTriggerTransform(barrierInfos[i].positionIndex).localPosition;
#if UNITY_EDITOR
            Handles.color = Color.white;
            Handles.Label(drawBarrierPos + Vector3.up * 2f, "Barrier");
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(drawBarrierPos + Vector3.up * barrierInfos[i].spawnSize.y/2f, Quaternion.Euler(barrierInfos[i].spawnRotation), Vector3.one);
            Gizmos.matrix = rotationMatrix;
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(Vector3.zero, barrierInfos[i].spawnSize);
#endif
        }
    }
    private void DrawExistBarrier(SpawnBarrierInfo[] barrierInfos)
    {
        if (barrierInfos.Length <= 0) return;

        for (int i = 0; i < barrierInfos.Length; i++)
        {
            drawBarrierPos = dungeonSpawnPosition.GetTriggerTransform(barrierInfos[i].positionIndex).localPosition;
#if UNITY_EDITOR
            Handles.color = Color.white;
            Handles.Label(drawBarrierPos + Vector3.up * 2f, "Exist Barrier");
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(drawBarrierPos + Vector3.up * barrierInfos[i].spawnSize.y / 2f, Quaternion.Euler(barrierInfos[i].spawnRotation), Vector3.one);
            Gizmos.matrix = rotationMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero, barrierInfos[i].spawnSize);
#endif
        }
    }



#if UNITY_EDITOR
    [ContextMenu("Save")]
    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
