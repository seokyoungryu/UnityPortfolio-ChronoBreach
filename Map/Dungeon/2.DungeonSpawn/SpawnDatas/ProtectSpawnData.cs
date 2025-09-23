using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProtectAIType { STAND = 0, FOLLOWING=1, WAYPOINT = 2,}

[CreateAssetMenu(menuName = "Map/Dungeon Spawn Data/Protect Spawn Data", fileName = "ProtectSpawnData_")]
public class ProtectSpawnData : TriggerSpawnData<ProtectSpawnInfos>
{
    [SerializeField] private BaseDungeonEnemyInfo protectAIInfo;
    [SerializeField] private SpawnTriggerInfo finalReachedPosition;
    [SerializeField] private EffectInfo finalEffect;

    [Header("Protect AI가 Follow상태면 체크.")]
    [SerializeField] private ProtectAIType protectType = ProtectAIType.STAND;
    [Header("Follow Setting(기본세팅 사용할 경우 -1)")]
    [SerializeField] private float followDistance = -1f;
    [SerializeField] private float followSpeed = -1f;

    private AIController protectAIController = null;
    private bool isReachedFinalPosition = false;
    private int reachCount = 0;
    private ProtectSpawnData tempData = null;
    private bool isExcuteFinalEffect = false;
    public bool isCreateProectedAI = false;

    public int ReachCount => reachCount;

    public AIController ProtectAIController => protectAIController;
    public ProtectAIType ProtectType => protectType;
    public SpawnTriggerInfo FinalReachedPosition => finalReachedPosition;
    public BaseDungeonEnemyInfo ProtectAIInfo => protectAIInfo;

    public override void ClearObjs()
    {
        base.ClearObjs();

    }

    public async void SpawnProtectAI()
    {
        AIController protectAI = await CreateAI(protectAIInfo, CreateAIType.PLAYABLEAI);
        protectAI.aIVariables.targetVector = Vector3.zero;
        protectAI.aiConditions.CanDmgRegisterTarget = false;

        if(protectType == ProtectAIType.FOLLOWING)
        {
            protectAI.aIVariables.followDistance = followDistance;
            protectAI.aIVariables.followWalkSpeed = followSpeed;
            protectAI.aIVariables.stopDistance = followDistance;
            protectAI.aIVariables.followTarget = playerController.transform;
        }

        protectAI.ClearOnDead();
        protectAI.onExtraDead += protectAIInfo.Dead;
        protectAI.onExtraDead += ExcuteFailProcess;

        protectAIInfo.Active();
        protectAIInfo.SetController(protectAI);
        protectAIController = protectAI;
        isCreateProectedAI = true;

    }

    public override void ExcuteEndProcess()
    {
        if (isCompleteDungeon || isFailDungeon) return;
        base.ExcuteEndProcess();

        isCompleteDungeon = true;
        Debug.Log("★☆★☆★ 던전 클리어 ★☆★☆★");
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
        SettingFollowTarget(CurrentWave.PlayableAIFollowType);

        triggerLayerMask = CurrentWave.TriggerLayerMask;
        StartWave(CurrentWave);

        if (protectAIController == null)
            Debug.Log("프로텍트 AI NULL");

        if (protectType == ProtectAIType.WAYPOINT)
        {
            if (CurrentWave.AIMoveSpeed != 0)
                protectAIController.aIVariables.walkSpeed = CurrentWave.AIMoveSpeed;

            if (CurrentWave.AiEnterWayPointIndex != -1 && protectAIController != null)
            {
                CurrentWave.AIWayPoint = dungeonPositinList.GetTriggerPosition(CurrentWave.AiEnterWayPointIndex);
                protectAIController.aIVariables.targetVector = CurrentWave.AIWayPoint;
            }
        }
    }
    public override void StartRound()
    {
        dungeon.StartCoroutine(StartRound(CurrentWave));
    }

    public override void StartRoundProcess()
    {
        base.StartRoundProcess();
        CurrentWave.ProtectAIStopPosition = dungeonPositinList.GetTriggerPosition(CurrentWave.ProtectAIEntryStopPositionIndex);
        //protectAIController.aIVariables.targetVector = Vector3.zero;
        if (protectType == ProtectAIType.WAYPOINT && CurrentWave.ProtectAIStopPosition != Vector3.zero && protectAIController != null)
            protectAIController.aIVariables.targetVector = CurrentWave.ProtectAIStopPosition;
        else if (protectType == ProtectAIType.FOLLOWING)
        {
            if (CurrentWave.IsMoveEntryPosition)
            {
                CurrentWave.AIWayPoint = dungeonPositinList.GetTriggerPosition(CurrentWave.ProtectAIEntryStopPositionIndex);
                protectAIController.aIVariables.targetVector = CurrentWave.AIWayPoint;
            }
            else
            {
                protectAIController.aIVariables.targetVector = Vector3.zero;
            }
        }
    }
    public override void NextStartWave()
    {
        if (NextWave != null)
            SettingFollowTarget(NextWave.PlayableAIFollowType);

        triggerLayerMask = NextWave.TriggerLayerMask;
        NextStartWave(NextWave);
        CurrentWave.AIWayPoint = dungeonPositinList.GetTriggerPosition(CurrentWave.AiEnterWayPointIndex);

        if (protectType == ProtectAIType.WAYPOINT)
        {
            if (CurrentWave.AIMoveSpeed != 0)
                protectAIController.aIVariables.walkSpeed = CurrentWave.AIMoveSpeed;

            if (CurrentWave.AIWayPoint != Vector3.zero && protectAIController != null)
                protectAIController.aIVariables.targetVector = CurrentWave.AIWayPoint;
        }
      

    }
    public override void NextStartRound() => dungeon.StartCoroutine(NextStartRound(CurrentWave));
    public override bool IsEndRound() => CurrentWave.IsEndRound(CurrentWave.GetRoundEnemy(CurrentWave.CurrentEntryRound));
    public override void CompleteCurrentRoundState() => CompleteCurrentRoundState(CurrentWave);
    public override bool HaveNextRound() => HaveNextRound(CurrentWave);

    public void SettingFollowTarget(ProtectDungeonPlayableMoveType type)
    {
        followType = type;
        if (type == ProtectDungeonPlayableMoveType.NONE)
        {
            followTarget = null;
            return;
        }

        if (type == ProtectDungeonPlayableMoveType.ONCE_FOLLOW_PLAYER || type == ProtectDungeonPlayableMoveType.ONLY_FOLLOW_PLAYER  || type == ProtectDungeonPlayableMoveType.ALL_PLAYABLE_F_PLAYER)
            followTarget = playerController;
        else if (type == ProtectDungeonPlayableMoveType.ONCE_FOLLOW_PROTECTAI || type == ProtectDungeonPlayableMoveType.ONLY_FOLLOW_PROTECTAI || type == ProtectDungeonPlayableMoveType.ALL_PLAYABLE_F_PROTECTAI)
            followTarget = protectAIController;

        if (type == ProtectDungeonPlayableMoveType.ALL_PLAYABLE_F_PLAYER || type == ProtectDungeonPlayableMoveType.ALL_PLAYABLE_F_PROTECTAI)
        {
            for (int i = 0; i < playableAIInfos.Count; i++)
            {
                if (playableAIInfos[i].AIController.aIFSMVariabls.followType == ProtectDungeonPlayableMoveType.ONLY_FOLLOW_PLAYER ||
                    playableAIInfos[i].AIController.aIFSMVariabls.followType == ProtectDungeonPlayableMoveType.ONLY_FOLLOW_PROTECTAI)
                    continue;

                playableAIInfos[i].AIController.aIFSMVariabls.followType = type;
                if (type == ProtectDungeonPlayableMoveType.ALL_PLAYABLE_F_PLAYER)
                    playableAIInfos[i].AIController.aIVariables.followTarget = playerController.transform;
                else if(type == ProtectDungeonPlayableMoveType.ALL_PLAYABLE_F_PROTECTAI)
                    playableAIInfos[i].AIController.aIVariables.followTarget = protectAIController.transform;
            }
        }
    }

    protected override bool CheckCompleteDungeon()
    {
        if (!isReachedFinalPosition) return false;

        return base.CheckCompleteDungeon();
    }

    public override bool CheckCompleteImmediate()
    {
        return CheckCompleteImmediate(waves);
    }

    private IEnumerator CheckEntryReachPosition(SpawnTriggerInfo triggerInfo)
    {
        isReachedFinalPosition = false;
        reachCount = 0;
      
        while (!isReachedFinalPosition)
        {
            reachCount = BoxCast(triggerInfo,triggerLayerMask);
            if (reachCount > 0)
                isReachedFinalPosition = true;
            yield return null;
        }

        if (isReachedFinalPosition)
        {
            ExcuteEndProcess();
        }
    }

    public async override void SpawnRoundEnemy(int currentWaveIndex, int roundIndex)
    {
        BaseDungeonEnemyInfo[] playableAis = waves[currentWaveIndex].RoundInfo[roundIndex].PlayableAIInfos;
        SpawnAllPlayableAI(playableAis);
        Debug.Log("여기서 생성");

        BaseDungeonEnemyInfo[] roundInfos = waves[currentWaveIndex].GetRoundEnemy(roundIndex + 1);
        Debug.Log("Count : " + roundInfos.Length);
        for (int i = 0; i < roundInfos.Length; i++)
        {
            SpawnEnemy(roundInfos[i]);
            await System.Threading.Tasks.Task.Delay(eachSpawnDelayTime);
        }

    }

    public override void OnDeadTrigger()
    {
        if (CurrentWave.IsMoveEntryPosition)
        {
            if(IsEndRound())
            {
                Debug.Log("IsEndRound IsMoveEntry !!! ");
                protectAIController.aIVariables.targetVector = Vector3.zero;
            }

        }

        finalReachedPosition.SettingValue(dungeonPositinList.GetTriggerTransform(finalReachedPosition.positionIndex));
        base.OnDeadTrigger();

        if (IsAllWavesComplete() && !isExcuteFinalEffect)
        {
            isExcuteFinalEffect = true;
            if (protectType == ProtectAIType.FOLLOWING || protectType == ProtectAIType.WAYPOINT)
            {
                Vector3 pos = dungeonPositinList.GetTriggerPosition(finalReachedPosition.positionIndex);
                EffectManager.Instance.GetEffectObjectInfo(finalEffect, pos + Vector3.up * finalEffect.spawnScale.y / 2f, Vector3.zero, Vector3.zero);
            }
        }

        if (IsAllWavesComplete() && protectType == ProtectAIType.STAND)
        {
            ExcuteEndProcess();
            return;
        }
        else if(protectType != ProtectAIType.STAND)
        {
            dungeon.StartCoroutine(CheckEntryReachPosition(finalReachedPosition));
            if (IsAllWavesComplete())
            {
                if (protectType == ProtectAIType.WAYPOINT)
                {
                    if (finalReachedPosition.triggerPosition != Vector3.zero && protectAIController != null)
                    {
                        if (allBarriers != null && allBarriers.Length > 0)
                            for (int i = 0; i < allBarriers.Length; i++)
                                allBarriers[i].SetOBP();

                        protectAIController.aIVariables.targetVector = finalReachedPosition.triggerPosition + (Vector3.forward * 2f);
                    }
                    else if (finalReachedPosition.triggerPosition == Vector3.zero)
                    {
                        ExcuteEndProcess();
                    }
                }
                else if (protectType == ProtectAIType.FOLLOWING)
                {
                    Debug.Log("AllWaveCom");
                    protectAIController.aIVariables.targetVector = Vector3.zero;
                }
            }
        }

        
    }

    public override void ExcuteFailProcess()
    {
        base.ExcuteFailProcess();

    }

    public override void OnDeadImmediate()
    {
        base.OnDeadImmediate();
    }

    private void OnValidate()
    {
        if (spawnType != TriggerSpawnType.TRIGGER)
            spawnType = TriggerSpawnType.TRIGGER;

        if (waves.Length <= 0) return;
        for (int i = 0; i < waves.Length; i++)
            if (waves.Length >= i)
                waves[i].InfoName = "Wave " + (i + 1);

        for (int i = 0; i < waves.Length; i++)
        {
            for (int x = 0; x < waves[i].RoundInfo.Length; x++)
            {
                waves[i].RoundInfo[x].RoundName = "라운드" + (x + 1);
                waves[i].RoundInfo[x].EntryRound = (x + 1);
            }
        }
    }


    [ContextMenu("데이터 임시 저장")]
    private void SaveTempSpawnData()
    {
        tempData = Instantiate(this);
    }


    [ContextMenu("임시 데이터 불러오기")]
    private void LoadTempSpawnData()
    {
        protectAIInfo = tempData.protectAIInfo;
        finalReachedPosition = tempData.finalReachedPosition;
        spawnType = tempData.spawnType;
        waves = tempData.waves;
        allBarriers = tempData.allBarriers;
        triggerLayerMask = tempData.triggerLayerMask;
    }

   
}
