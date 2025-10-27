using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public enum CreateAIType
{
    ENEMY ,
    PLAYABLEAI,
}





/// <summary>
/// SpawnData의 기준은 Map Data. Map의 위치를 기반으로 작성.
/// </summary>
public abstract class BaseSpawnData : ScriptableObject
{
    [SerializeField] private List<BaseDungeonEnemyInfo> initPlayerableAIInfos = new List<BaseDungeonEnemyInfo>();
    [HideInInspector] public CoroutineForDungeon dungeon;
    protected DungeonSpawnPositionList dungeonPositinList = null;
    protected bool isCompleteDungeon = false;
    protected bool isFailDungeon = false;
    [Header("Task.Delay (1000 -> 1초)")]
    [SerializeField] protected int spawnDeleyTime = 800;
    [SerializeField] protected int eachSpawnDelayTime = 200;
    protected BaseController playerController = null;
    protected BaseController followTarget = null;
    protected ProtectDungeonPlayableMoveType followType;
    protected List<BaseDungeonEnemyInfo> playableAIInfos = new List<BaseDungeonEnemyInfo>();
    protected bool isExcuteBossBGM = false;
    private Collider[] checkBossEntryColl;
    private List<ObpInfo> obpObjs = new List<ObpInfo>();

    [Header("계속 존재할 벽")]
    [SerializeField] protected SpawnBarrierInfo[] existBarriers;

    [Header("Boss BGM 실행 위해")]
    public LayerMask checkLayer = TagAndLayerDefine.LayersIndex.Player;
    [SerializeField] protected CheckBossBGMInfo checkBossBgmInfo;

    #region Event
    public delegate void OnCompleteDungeon();

    public event OnCompleteDungeon onExcuteBoss;
    public event OnCompleteDungeon onCompleteDungeon;
    public event OnCompleteDungeon onFailDungeon;

    #endregion

    public virtual void ClearObjs()
    {
        foreach (ObpInfo info in obpObjs)
            ObjectPooling.Instance.SetOBP(info.obpName, info.obpGo);
        Debug.Log("Clear 실행!");
    }

    public virtual void ExcuteEndProcess()
    {
        SoundManager.Instance.PlayUISound(UISoundType.DUNGEON_CLEAR);
        CommonUIManager.Instance.successScoreUI.ExcuteScoreUI(MapManager.Instance.CurrentDungeonTitle);
        onCompleteDungeon?.Invoke();
        MapManager.Instance.CurrentSelectedDungeonTitle.DungeonClear();
        
    }
    public virtual void ExcuteFailProcess()
    {
        Debug.Log("Spawn Excute Fail Process");

        SoundManager.Instance.PlayUISound(UISoundType.DUNGEON_FAIL);
        CommonUIManager.Instance.failScoreUI.ExcuteScoreUI(MapManager.Instance.CurrentDungeonTitle);
        onFailDungeon?.Invoke();
    }

    public SpawnBarrierInfo[] ExistBarriers => existBarriers;

    public CheckBossBGMInfo CheckBossBgmInfo => checkBossBgmInfo;
    public void SettingSpawnPositionList(DungeonSpawnPositionList list) => dungeonPositinList = list;
    public List<BaseDungeonEnemyInfo> InitPlayerableAIInfos => initPlayerableAIInfos;

    protected async Task<AIController> CreateAI(BaseDungeonEnemyInfo info , CreateAIType type)
    {
        info.SetSpawnPositions(dungeonPositinList.GetSpawnPosition(info.SpawnPositionIndex));
        EffectManager.Instance.GetEffectObjectInfo(info.SpawnAppearEffect, info.SpawnPosition, Vector3.zero, Vector3.zero);

        AIController enemy = AIManager.Instance.CreateAI(info.EnemyObpName, info.EnemyInfoList);

        if (enemy == null)
            Debug.Log("<color=yellow> 이거 NULL </color>");

        enemy.gameObject.SetActive(false);

        await System.Threading.Tasks.Task.Delay(spawnDeleyTime);

        enemy.gameObject.SetActive(true);


        enemy.IsPlayableObject = false;
        enemy.ResetAI();
        SetEnemyWayPoints(info, enemy);
        if (info.SpawnRotation == -Vector3.one)
            info.SetSpawnRositions(dungeonPositinList.GetSpawnRotation(info.SpawnPositionIndex).eulerAngles);

        enemy.TranslatePosition(info.SpawnPosition);
        enemy.RotateByVector(info.SpawnRotation);

        if (info.IsForceRunning)
            enemy.aiConditions.IsForceRunning = info.IsForceRunning;

        enemy.transform.localScale = info.SpawnSize;
        enemy.aIFSMVariabls.resetPos = info.SpawnPosition;
        enemy.targetLayer = info.TargetLayer;
        enemy.aiStatus.ExcuteOnHPHUD();
        obpObjs.Add(new ObpInfo(enemy.OBPName, enemy.obpGo));
        return enemy;
    }

    public void SettingPlayerController(BaseController controller) => playerController = controller;


    public async Task<AIController> SpawnPlayableAI(BaseDungeonEnemyInfo info)
    {
        AIController playableAI = await CreateAI(info, CreateAIType.PLAYABLEAI);
        playableAI.IsPlayableObject = true;
        playableAI.ClearOnDead();
        info.Active();
        info.SetController(playableAI);
        if (followTarget != null)
        {
            playableAI.aIFSMVariabls.followType = followType;
            playableAI.aIVariables.followTarget = followTarget.transform;
        }

        Debug.Log("Follow Target : " + followTarget.name);
       
        playableAIInfos.Add(info);
        return playableAI;
    }


    public async void SpawnAllPlayableAI(BaseDungeonEnemyInfo[] info)
    {
        Debug.Log("플레이어블 생성 : " + info.Length);

        for (int i = 0; i < info.Length; i++)
        {
            SpawnPlayableAI(info[i]);
            await System.Threading.Tasks.Task.Delay(eachSpawnDelayTime);
        }
    }

    protected IEnumerator WaitTime(float waitTime)
    {
        float currentTimer = 0f;
        while (currentTimer < waitTime)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
    }


    public void CreateExistBarrier()
    {
        for (int i = 0; i < existBarriers.Length; i++)
            SetBarrier(existBarriers[i]);
    }
                 

    protected IEnumerator CheckBossBGM()
    {
        if (checkBossBgmInfo.positionIndex == -1) yield break;

        if (dungeonPositinList == null)
        {
            dungeonPositinList = MapManager.Instance.CurrentDungeonTitle.DungeonSpawnPosition;
            Debug.Log("dungeonPositinList NUll Setting : " + dungeonPositinList);
        }

        Debug.Log("체크 BOSS BGM 코루틴 실행..");
        yield return new WaitForSeconds(checkBossBgmInfo.delayTime);

        checkBossBgmInfo.spawnPosition = dungeonPositinList.GetTriggerTransform(checkBossBgmInfo.positionIndex).localPosition;

        while (!isExcuteBossBGM)
        {
            checkBossEntryColl = Physics.OverlapBox(checkBossBgmInfo.spawnPosition + Vector3.up * checkBossBgmInfo.spawnSize.y / 2f, checkBossBgmInfo.spawnSize / 2f, Quaternion.identity, checkLayer);

            if (checkBossEntryColl?.Length > 0)
            {
                Debug.Log("탐지!!!!!!!!!!!!!!!!!!!!!!!!");
                isExcuteBossBGM = true;
                onExcuteBoss?.Invoke();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    protected ReturnObjectToObjectPooling SetBarrier(SpawnBarrierInfo info)
    {
        ReturnObjectToObjectPooling barrier = ObjectPooling.Instance.GetOBP(info.obpList.ToString()).GetComponent<ReturnObjectToObjectPooling>();
        if (barrier == null) return null;

        info.SettingTransform(dungeonPositinList.GetTriggerTransform(info.positionIndex));
        barrier.transform.rotation = Quaternion.Euler(info.spawnRotation);
        barrier.transform.localScale = info.spawnSize;
        barrier.transform.position = info.spawnPosition;
        obpObjs.Add(new ObpInfo(barrier.objectPoolName, barrier.gameObject));
        return barrier;
    }

    private void SetEnemyWayPoints(BaseDungeonEnemyInfo info, AIController enemy)
    {
        for (int i = 0; i < info.WayPointInfos.Length; i++)
            enemy.SetWayPoints(info.WayPointInfos[i], dungeonPositinList.GetWayPointsTransform(info.WayPointInfos[i].WayIndex));

    }
}


