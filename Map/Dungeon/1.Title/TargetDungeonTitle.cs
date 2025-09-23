using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Map/Dungeon Title/Target Title ", fileName = "TargetTitle_")]
public class TargetDungeonTitle : BaseDungeonTitle
{
    [SerializeField] protected TargetSpawnData targetSpawnData;
    [SerializeField] protected TargetDungeonFunction targetFunction;

    public TargetSpawnData SpawnData => targetSpawnData;
    public TargetDungeonFunction Function => targetFunction;
    public override void ClearObj()
    {
        base.ClearObj();
        targetSpawnData.ClearObjs();

    }
    public override BaseDungeonTitle GetClone()
    {
        return Clone();
    }
    public TargetDungeonTitle Clone()
    {
        TargetDungeonTitle cloneTitle = Instantiate(this);
        cloneTitle.id = id;
        cloneTitle.taskTarget = Instantiate(taskTarget);
        cloneTitle.dungeonSpawnPosition = Instantiate(dungeonSpawnPosition);
        cloneTitle.dungeonReward = Instantiate(dungeonReward);
        cloneTitle.dungeonCondition = Instantiate(dungeonCondition);
        cloneTitle.dungeonCateogry = Instantiate(dungeonCateogry);
        cloneTitle.dungeonMapData = Instantiate(dungeonMapData);
        cloneTitle.targetSpawnData = Instantiate(targetSpawnData);
        cloneTitle.targetFunction = Instantiate(targetFunction);
        cloneTitle.dungeonCoroutine = dungeonCoroutine;
        cloneTitle.originController = originController;
        cloneTitle.excuteController = excuteController;
        return cloneTitle;
    }


    public override void Excute()
    {
        targetFunction.ExcuteProcess(this);
    }

#if UNITY_EDITOR
    public override void ExcuteDrawCurrentWave(int currentIndex)
    {
        int index = currentIndex == -1 ? targetSpawnData.CurrentWaveIndex : currentIndex;
        base.ExcuteDrawCurrentWave(index);
        DrawSpawnTriggerABarrier(targetSpawnData.CurrentWave.SpawnTrigger, targetSpawnData.CurrentWave.SpawnBarriers, targetSpawnData.ExistBarriers, targetSpawnData.CheckBossBgmInfo);
        for (int i = 0; i < targetSpawnData.CurrentWave.RoundInfo.Length; i++)
        {
            DrawEnemySpawnPos(targetSpawnData.CurrentWave.RoundInfo[i].EnemyInfos.ToArray(), targetSpawnData.CurrentWaveIndex + 1, targetSpawnData.CurrentWave.RoundInfo[i].EntryRound, DrawEnemyType.TARGET);
            DrawEnemySpawnPos(targetSpawnData.CurrentWave.RoundInfo[i].PlayableAIInfos, targetSpawnData.CurrentWaveIndex + 1, targetSpawnData.CurrentWave.RoundInfo[i].EntryRound, DrawEnemyType.PLAYABLE);
        }
        DrawCommon();
    }

    public override void ExcuteDrawAllWave()
    {
        base.ExcuteDrawAllWave();
        for (int i = 0; i < targetSpawnData.Waves.Length; i++)
        {
            DrawSpawnTriggerABarrier(targetSpawnData.Waves[i].SpawnTrigger, targetSpawnData.Waves[i].SpawnBarriers, targetSpawnData.ExistBarriers, targetSpawnData.CheckBossBgmInfo);
        }
        for (int i = 0; i < targetSpawnData.Waves.Length; i++)
        {
            for (int j = 0; j < targetSpawnData.Waves[i].RoundInfo.Length; j++)
            {
                DrawEnemySpawnPos(targetSpawnData.Waves[i].RoundInfo[j].EnemyInfos.ToArray(), i + 1, targetSpawnData.Waves[i].RoundInfo[j].EntryRound, DrawEnemyType.TARGET);
                DrawEnemySpawnPos(targetSpawnData.Waves[i].RoundInfo[j].PlayableAIInfos, i + 1, targetSpawnData.Waves[i].RoundInfo[j].EntryRound, DrawEnemyType.PLAYABLE);
            }
        }

        DrawCommon();
    }

    private void DrawCommon()
    {
        DrawPlayerSpawnPos();

    }
#endif
}

