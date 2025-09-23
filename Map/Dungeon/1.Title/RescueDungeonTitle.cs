using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Map/Dungeon Title/Rescue Title ", fileName = "RescueTitle_")]
public class RescueDungeonTitle : BaseDungeonTitle
{
    [SerializeField] protected RescuerSpawnData rescueSpawnData;
    [SerializeField] protected RescueDungeonFunction rescueFunction;

    public RescuerSpawnData SpawnData => rescueSpawnData;
    public RescueDungeonFunction Function => rescueFunction;
    public override void ClearObj()
    {
        base.ClearObj();
        rescueSpawnData.ClearObjs();

    }
    public override BaseDungeonTitle GetClone()
    {
        return Clone();
    }
    public RescueDungeonTitle Clone()
    {
        RescueDungeonTitle cloneTitle = Instantiate(this);
        cloneTitle.id = id;
        cloneTitle.taskTarget = Instantiate(taskTarget);
        cloneTitle.dungeonSpawnPosition = Instantiate(dungeonSpawnPosition);
        cloneTitle.dungeonReward = Instantiate(dungeonReward);
        cloneTitle.dungeonCondition = Instantiate(dungeonCondition);
        cloneTitle.dungeonCateogry = Instantiate(dungeonCateogry);
        cloneTitle.dungeonMapData = Instantiate(dungeonMapData);
        cloneTitle.rescueSpawnData = Instantiate(rescueSpawnData);
        cloneTitle.rescueFunction = Instantiate(rescueFunction);
        cloneTitle.dungeonCoroutine = dungeonCoroutine;
        cloneTitle.originController = originController;
        cloneTitle.excuteController = excuteController;
        return cloneTitle;
    }

    public override void Excute()
    {
        rescueFunction.ExcuteProcess(this);
    }


    public override void ExcuteDrawAllWave()
    {
        base.ExcuteDrawAllWave();
        for (int i = 0; i < rescueSpawnData.Waves.Length; i++)
        {
            DrawSpawnTriggerABarrier(rescueSpawnData.Waves[i].SpawnTrigger, rescueSpawnData.Waves[i].SpawnBarriers, rescueSpawnData.ExistBarriers, rescueSpawnData.CheckBossBgmInfo);
        }

        for (int i = 0; i < rescueSpawnData.Waves.Length; i++)
        {
            for (int j = 0; j < rescueSpawnData.Waves[i].RoundInfo.Length; j++)
            {
                DrawEnemySpawnPos(rescueSpawnData.Waves[i].RoundInfo[j].EnemyInfos.ToArray(), i + 1, rescueSpawnData.Waves[i].RoundInfo[j].EntryRound, DrawEnemyType.NORAML);
                DrawEnemySpawnPos(rescueSpawnData.Waves[i].RoundInfo[j].PlayableAIInfos, i + 1, rescueSpawnData.Waves[i].RoundInfo[j].EntryRound, DrawEnemyType.PLAYABLE);
            }
        }

        DrawCommon();
    }

    public override void ExcuteDrawCurrentWave(int currentIndex)
    {
        int index = currentIndex == -1 ? rescueSpawnData.CurrentWaveIndex : currentIndex;
        base.ExcuteDrawCurrentWave(index);
        DrawSpawnTriggerABarrier(rescueSpawnData.Waves[index].SpawnTrigger, rescueSpawnData.Waves[index].SpawnBarriers, rescueSpawnData.ExistBarriers, rescueSpawnData.CheckBossBgmInfo);

        for (int i = 0; i < rescueSpawnData.Waves[index].RoundInfo.Length; i++)
        {
            DrawEnemySpawnPos(rescueSpawnData.Waves[index].RoundInfo[i].EnemyInfos.ToArray(), index + 1, rescueSpawnData.Waves[index].RoundInfo[i].EntryRound, DrawEnemyType.NORAML);
            DrawEnemySpawnPos(rescueSpawnData.Waves[index].RoundInfo[i].PlayableAIInfos, index + 1, rescueSpawnData.Waves[index].RoundInfo[i].EntryRound, DrawEnemyType.PLAYABLE);
        }
        //구출 위치


        DrawCommon();

#if UNITY_EDITOR
        //Handles.color = Color.red;
        //Handles.Label(rescueSpawnData.CurrentWave.SpawnTrigger.triggerPosition, "스폰 트리거 ");
        //Handles.color = Color.red;
        //Handles.Label(rescueSpawnData.FinalReachedPosition.triggerPosition, "최종 도착점 ");
        //
#endif
    }

    private void DrawCommon()
    {
#if UNITY_EDITOR
        DrawPlayerSpawnPos();
        Vector3 rescuerReachPos = dungeonSpawnPosition.GetTriggerPosition(rescueSpawnData.RescueTargetPosition.positionIndex);
        Handles.color = Color.magenta;
        Handles.Label(rescuerReachPos + Vector3.up * 2.5f,"인질 구출 위치"); ;
        Handles.DrawWireCube(rescuerReachPos + Vector3.up * rescueSpawnData.RescueTargetPosition.extend.y /2f, rescueSpawnData.RescueTargetPosition.extend);

        //보스전 인질 이동 트리거
        Vector3 rescuerBossPos = dungeonSpawnPosition.GetTriggerPosition(rescueSpawnData.BossWaveTriggerPos.positionIndex);
        Handles.color = Color.magenta;
        Handles.Label(rescuerBossPos + Vector3.up * 2.5f, "보스전 인질 이동 트리거"); ;
        Handles.DrawWireCube(rescuerBossPos + Vector3.up * rescueSpawnData.BossWaveTriggerPos.extend.y / 2f, rescueSpawnData.BossWaveTriggerPos.extend);

        Vector3 rescuerBossMovePos = dungeonSpawnPosition.GetTriggerPosition(rescueSpawnData.ResucerBossWaveIndex);
        Handles.color = Color.magenta;
        Handles.Label(rescuerBossMovePos + Vector3.up * 2.5f, "보스전 인질 이동 위치"); ;
        Handles.DrawWireCube(rescuerBossMovePos + Vector3.up * 1 , Vector3.one * 3f);


        for (int i = 0; i < rescueSpawnData.RescuerInfos.Length; i++)
        {
            Vector3 pos = dungeonSpawnPosition.GetSpawnPosition(rescueSpawnData.RescuerInfos[i].SpawnPositionIndex);
            Handles.color = Color.yellow;
            Handles.Label(pos + Vector3.up * 2.5f, (i+1) +"번 인질 생성 위치"); ;
            Handles.DrawWireCube(pos + Vector3.up * 1f, Vector3.one * 2f);
        }
#endif
    }

}