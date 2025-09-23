using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "Map/Dungeon Spawn Data/Rescuer Spawn Data", fileName = "RescuerSpawnData_")]
public class RescuerSpawnData : TriggerSpawnData<RescuerSpawnInfos>
{

    [Header("(true) 모든 적을 쓰려트려야 구출 위치 생성")]
    [SerializeField] private bool allEnemyKill = true;
    [SerializeField] private EffectInfo reachEffect = null;
    [Header("최소 구출 인원")]
    [SerializeField] private int minRescuerCount = 0;
    [Header("구출 목표 위치")]
    [SerializeField] private LayerMask reachLayer;
    [SerializeField] private SpawnTriggerInfo rescueTargetPosition;
    [SerializeField] private RescuerDungeonEnemyInfo[] rescuersInfos;
    [Header("보스전 트리거")]
    [SerializeField] private SpawnTriggerInfo bossWaveTriggerPos;
    [Header("보스전 시 이동해 있을 위치(TriggerIndex)")]
    [SerializeField] private int resucerBossWaveIndex;
    private List<AIController> rescuersControllers = new List<AIController>();
    private List<AIController> activeRescuerContrs = new List<AIController>();
    private bool isEnterTargetPosition = false;
    [SerializeField] private int currentRescuerdCount = 0; //현재 구출된 인원.
    private int countReachedTargetPosition = 0; // 구출 위치 검사 
    private Transform rescuerTarget = null;
    private bool isStartCheckRescuerCount = false;
    private bool isStartCheckBossTrigger = false;


    public int RescuerCount => rescuersInfos.Length;
    public int MinRescuerCount { get { return minRescuerCount; } set { minRescuerCount = value; } }
    public int RescuerInfosCount => rescuersInfos.Length;
    public SpawnTriggerInfo RescueTargetPosition => rescueTargetPosition;
    public RescuerDungeonEnemyInfo[] RescuerInfos => rescuersInfos;
    public SpawnTriggerInfo BossWaveTriggerPos => bossWaveTriggerPos;
    public int ResucerBossWaveIndex => resucerBossWaveIndex;

    private async void SpawnAllRescuers()
    {
        rescuersControllers = new List<AIController>();
        for (int i = 0; i < rescuersInfos.Length; i++)
            rescuersControllers.Add(await SpawnRescuer(rescuersInfos[i]));
    }

    private async Task<AIController> SpawnRescuer(RescuerDungeonEnemyInfo info)
    {
        AIController rescuer = await CreateAI(info, CreateAIType.PLAYABLEAI);
        rescuer.aiConditions.CanState = false;
        rescuer.aiConditions.CanDetect = false;
        rescuer.aIVariables.followDistance = info.FollowDistance;

        if (info.FollowWalkSpeed > 0)
            rescuer.aIVariables.followWalkSpeed = info.FollowWalkSpeed;
        if (info.FollowRunSpeed > 0)
            rescuer.aIVariables.followRunSpeed = info.FollowRunSpeed;

        rescuer.onExtraDead += info.Dead;
        rescuer.onExtraDead += DeadRescuer;
        rescuer.onExtraDead += () => currentRescuerdCount--;
        rescuer.onExtraDead += () => RescuerNotifierText();

        if (activeRescuerContrs == null) activeRescuerContrs = new List<AIController>();
        RescuerCheck check = rescuer.GetComponentInChildren<RescuerCheck>();
        check.onActiveRescuer += info.Rescue;
        check.onActiveRescuer += () => currentRescuerdCount++;
        check.onActiveRescuer += () => rescuer.gameObject.layer = LayerMask.NameToLayer(TagAndLayerDefine.LayersString.ProtectAI);
        check.onActiveRescuer += StartCheckTargetPosition;
        check.onActiveRescuer += () => rescuer.aiConditions.CanDetect = true;
        check.onActiveRescuer += () => activeRescuerContrs.Add(rescuer);
        check.onActiveRescuer += () => QuestManager.Instance.ReceiveReport(QuestCategoryDefines.RESUER_COUNT, rescuer.questReporter.Target, 1);
        check.onActiveRescuer += () => CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("시민 구출 !");
        check.onActiveRescuer += () => RescuerNotifierText();


        info.Active();
        info.SetController(rescuer);
        rescuer.aIVariables.followTarget = rescuerTarget;
        return rescuer;
    }
    

    public void RescuerNotifierText()
    {
        Color green = new Color(39, 152, 43, 1);
        string colorgreenCode = ColorUtility.ToHtmlStringRGB(green);
        string colorwhileCode = ColorUtility.ToHtmlStringRGB(Color.white);

        if (currentRescuerdCount >= minRescuerCount)
            MapManager.Instance.DungeonNotifierUI.SetText($"<color=#{colorgreenCode}>인질 {currentRescuerdCount} / {minRescuerCount} </color>");
        else
            MapManager.Instance.DungeonNotifierUI.SetText($"<color=#{colorwhileCode}>인질 {currentRescuerdCount} / {minRescuerCount} </color>");
    }


    public void RescuerSetTartget(Transform target)
    {
        rescuerTarget = target;
    }

    protected override bool CheckCompleteDungeon()
    {
        if (isEnterTargetPosition && currentRescuerdCount >= minRescuerCount)
            return true;
        return false;
    }

    private void DeadRescuer()
    {
        if (GetCurrentAliveRescuer() < minRescuerCount)
            ExcuteFailProcess();
    }
   
    private void StartCheckTargetPosition()
    {
        if (!isStartCheckRescuerCount  && currentRescuerdCount >= minRescuerCount)
            dungeon.StartCoroutine(CheckTargetPosition());

        if (!isStartCheckBossTrigger)
            dungeon.StartCoroutine(CheckBossTrigger());
    }

    private IEnumerator CheckTargetPosition()
    {
        Transform reachTr = dungeonPositinList.GetTriggerTransform(rescueTargetPosition.positionIndex);
        rescueTargetPosition.triggerPosition = dungeonPositinList.GetTriggerPosition(rescueTargetPosition.positionIndex);
        isStartCheckRescuerCount = true;
        countReachedTargetPosition = 0;

        if (allEnemyKill)
            yield return new WaitUntil(() => IsAllWavesComplete());

        EffectManager.Instance.GetEffectObjectInfo(reachEffect, rescueTargetPosition.triggerPosition + Vector3.up * rescueTargetPosition.extend.y / 2f, Vector3.zero, Vector3.zero);
        for (int i = 0; i < activeRescuerContrs.Count; i++)
        {
            float followDistance = Random.Range(1f, 3f);
            activeRescuerContrs[i].aIVariables.followTarget = reachTr;
            activeRescuerContrs[i].aIVariables.followDistance = followDistance;
            activeRescuerContrs[i].nav.stoppingDistance = followDistance;
        }

        while (!isCompleteDungeon)
        {
            countReachedTargetPosition = BoxCast(rescueTargetPosition , reachLayer);
            if (countReachedTargetPosition > 0)
            {
                isEnterTargetPosition = true;
                if (CheckCompleteDungeon())
                    ExcuteEndProcess();
            }
            else
                isEnterTargetPosition = false;

            yield return new WaitForSeconds(0.2f);
        }
    }


    private IEnumerator CheckBossTrigger()
    {
        Debug.Log("보스 체크 IN!");

        int detectCount = 0;
        bool isEnterBossTrigger = false;
        isStartCheckBossTrigger = true;
        bossWaveTriggerPos.triggerPosition = dungeonPositinList.GetTriggerPosition(bossWaveTriggerPos.positionIndex);
        Transform moveTr = dungeonPositinList.GetTriggerTransform(resucerBossWaveIndex);

        while (!isEnterBossTrigger)
        {
            detectCount = BoxCast(bossWaveTriggerPos, reachLayer);
            Debug.Log("Detect Count :" + detectCount);

            if (detectCount > 0)
            {

                isEnterBossTrigger = true;
                for (int i = 0; i < activeRescuerContrs.Count; i++)
                {
                    activeRescuerContrs[i].aIVariables.followTarget = moveTr;

                    float followDistance = Random.Range(1f, 3f);
                    activeRescuerContrs[i].aIVariables.followDistance = followDistance;
                    activeRescuerContrs[i].nav.stoppingDistance = followDistance;
                }
            }
            yield return new WaitForSeconds(0.3f);

            Debug.Log("Active Resucer : " + activeRescuerContrs.Count);
        }

    }

    private int GetCurrentAliveRescuer()
    {
        int retCurrentAlive = 0;
        for (int i = 0; i < rescuersInfos.Length; i++)
            if (rescuersInfos[i].EnemyState == EnemyState.ACTIVE)
                retCurrentAlive++;

        return retCurrentAlive;
    }

    protected override void SetCompleteAllWave()
    {
        for (int i = 0; i < waves.Length; i++)
            if (IsCompleteWave(waves, i))
                waves[i].CurrentProcess = SpawnProcessType.COMPLETE;
    }
    public override void StartImmediate() => StartImmediate(waves);
    public override void StartWave()
    {
        //rescuer 생성.
        RescuerNotifierText();
        SpawnAllRescuers();
        StartWave(CurrentWave);
    }
    public override void StartRound() => dungeon.StartCoroutine(StartRound(CurrentWave));
    public override void NextStartWave() => NextStartWave(NextWave);
    public override void NextStartRound() => dungeon.StartCoroutine(NextStartRound(CurrentWave));

    public override bool IsEndRound() => CurrentWave.IsEndRound(CurrentWave.GetRoundEnemy(CurrentWave.CurrentEntryRound));
    public override void CompleteCurrentRoundState() => CompleteCurrentRoundState(CurrentWave);
    public override bool HaveNextRound() => HaveNextRound(CurrentWave);
    public override bool CheckCompleteImmediate() => CheckCompleteImmediate(waves);


    public async override void SpawnRoundEnemy(int currentWaveIndex, int roundIndex)
    {
        BaseDungeonEnemyInfo[] roundInfos = waves[currentWaveIndex].GetRoundEnemy(roundIndex + 1);
        for (int i = 0; i < roundInfos.Length; i++)
        {
            SpawnEnemy(roundInfos[i]);
            await System.Threading.Tasks.Task.Delay(eachSpawnDelayTime);
        }
    }

    public override void ExcuteFailProcess()
    {
        if (isCompleteDungeon) return;
        base.ExcuteFailProcess();

        isFailDungeon = true;
    }

    public override void ExcuteEndProcess()
    {
        if (isCompleteDungeon || isFailDungeon) return;
        base.ExcuteEndProcess();
        isCompleteDungeon = true;

    }

    private void OnValidate()
    {
        if (waves.Length <= 0) return;
        for (int i = 0; i < waves.Length; i++)
            if (waves.Length >= i)
                waves[i].InfoName = "Wave " + (i + 1);

        for (int i = 0; i < waves.Length; i++)
            for (int x = 0; x < waves[i].RoundInfo.Length; x++)
            {
                waves[i].RoundInfo[x].RoundName = "라운드" + (x + 1);
                waves[i].RoundInfo[x].EntryRound = (x + 1);
            }
    }

}