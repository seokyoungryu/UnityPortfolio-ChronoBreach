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
            //Gizmos.DrawWireCube(drawSpawnTriggerPos + Vector3.up * triggerInfos[i].extend.y / 2f, triggerInfos[i].extend);
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
            //Gizmos.DrawWireCube(drawSpawnTriggerPos + Vector3.up * triggerInfos[i].extend.y / 2f, triggerInfos[i].extend);
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
            // Gizmos.DrawWireCube(drawBarrierPos, barrierInfos[i].spawnSize);
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
            // Gizmos.DrawWireCube(drawBarrierPos, barrierInfos[i].spawnSize);
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

// 타이틀에서 일단 기본적인거 세팅해줌. 
// 플레이어가 UI로 던전 Entry 하면 실행할거.
// 1. GameManager에 현재 타이틀 넣기. 
// 2. 일단 Title의 MapData를 실행. (맵 이동 or 씬이동 )
// 3. SettingsController 해서 플레이어 다 세팅해둠. (newController는 이제 생성해주면됨)
//         여기서부터는 Function에서 해줌. (밑에는 Normal기능임)
// 4. 플레이어 초기 위치 이동. 
// 5. SpawnData는 종류가 다르니까. 함수로 통일? 예를들어 abstract StartSpawn(); 으로?

//기능의 구별 기준은... 완료조건?임 
//기능을 너무 상세하게 구별하지 않기. 예로 완료조건 (특정 개체 처치) 
//기능 종류
//1. 기본, 던전 몬스터, 보스 처치 (즉 전체 AI 처치) ( 트리거이 트리거는 spawnData니까 상관없을듯?.. ) ( 던전 클리어 )
//2. 특정 타겟만 처치. (즉 생성된 AI들중에서 특정 개체만 죽일 경우) (타겟을 기능에서 ? 데이터에서? )  (암살 or 특정 보스 처치 등 )
//3. 버티기 (시간 버티기?, spawnData는 생성될 시간, 변수로 죽었을 경우만할지, 계속 소환할지)   (기능에 정해진 시간만큼 버티기.)  (5분 버티기, 10분 버티기 등) 
//4. 레이싱 (1등 하기?) (레이싱 or 아이템전 or 차에서 총쏘기? )

//음.. 즉 기능 - spawnData가 같아야뎀. 엄밀히 말하자면 정확한 map,category,spawnData,function이 장착되야함.
//예를들어  map - 얼음동굴 , category - 기본, 총(이것도 가능하긴함), 로봇 (가능하긴함) , 레이싱 x 
//       spawnData - 트리거 or All or 버티기? , 기능 - 기본, 특정타겟, 버티기. 음 ... ㅇ

//정리해보자면, spawnData = function 호환되야함. 
//즉 레이싱이면 레이싱spawnData,  기본이면 트리거spawnData,  버티기면 버티기spawnData, 특정타겟이면 특정타겟spawnData