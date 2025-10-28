using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Map/Dungeon Title/Normal Title ", fileName = "NormalTitle_")]
public class NormalDungeonTitle : BaseDungeonTitle
{
    [SerializeField] protected NormalSpawnData normalSpawnData;
    [SerializeField] protected NormalDungeonFunction normalFunction;


    public NormalSpawnData SpawnData => normalSpawnData;
    public NormalDungeonFunction Function => normalFunction;


    public override void ClearObj()
    {
        base.ClearObj();
        normalSpawnData.ClearObjs();
    }

    public override BaseDungeonTitle GetClone()
    {
        return Clone();
    }

    public NormalDungeonTitle Clone()
    {
        NormalDungeonTitle cloneTitle = Instantiate(this);
        cloneTitle.id = id;
        cloneTitle.taskTarget = Instantiate(taskTarget);
        cloneTitle.dungeonCateogry = Instantiate(dungeonCateogry);
        cloneTitle.dungeonMapData = Instantiate(dungeonMapData);
        cloneTitle.dungeonSpawnPosition = Instantiate(dungeonSpawnPosition);
        cloneTitle.dungeonReward = Instantiate(dungeonReward);
        cloneTitle.dungeonCondition = Instantiate(dungeonCondition);
        cloneTitle.normalSpawnData = Instantiate(normalSpawnData);
        cloneTitle.normalFunction = Instantiate(normalFunction);
        cloneTitle.dungeonCoroutine = dungeonCoroutine;
        cloneTitle.originController = originController;
        cloneTitle.excuteController = excuteController;
        return cloneTitle;
    }

    public override void Excute()
    {
        normalFunction.ExcuteProcess(this);
    }
    public override void ExcuteDrawCurrentWave(int currentIndex)
    {
        int index = currentIndex == -1 ? normalSpawnData.CurrentWaveIndex : currentIndex;
        base.ExcuteDrawCurrentWave(index);
        DrawSpawnTriggerABarrier(normalSpawnData.CurrentWave.SpawnTrigger, normalSpawnData.CurrentWave.SpawnBarriers, normalSpawnData.ExistBarriers, normalSpawnData.CheckBossBgmInfo);
        for (int i = 0; i < normalSpawnData.CurrentWave.RoundInfo.Length; i++)
        {
            DrawEnemySpawnPos(normalSpawnData.CurrentWave.RoundInfo[i].EnemyInfos.ToArray(), normalSpawnData.CurrentWaveIndex + 1, normalSpawnData.CurrentWave.RoundInfo[i].EntryRound, DrawEnemyType.NORAML);
            DrawEnemySpawnPos(normalSpawnData.CurrentWave.RoundInfo[i].PlayableAIInfos, normalSpawnData.CurrentWaveIndex + 1, normalSpawnData.CurrentWave.RoundInfo[i].EntryRound, DrawEnemyType.PLAYABLE);
        }
        DrawCommon();
    }

    public override void ExcuteDrawAllWave()
    {
        base.ExcuteDrawAllWave();
        for (int i = 0; i < normalSpawnData.Waves.Length; i++)
        {
            DrawSpawnTriggerABarrier(normalSpawnData.Waves[i].SpawnTrigger, normalSpawnData.Waves[i].SpawnBarriers, normalSpawnData.ExistBarriers, normalSpawnData.CheckBossBgmInfo);
        }
        for (int i = 0; i < normalSpawnData.Waves.Length; i++)
        {
            for (int j = 0; j < normalSpawnData.Waves[i].RoundInfo.Length; j++)
            {
                DrawEnemySpawnPos(normalSpawnData.Waves[i].RoundInfo[j].EnemyInfos.ToArray(), i + 1, normalSpawnData.Waves[i].RoundInfo[j].EntryRound, DrawEnemyType.NORAML);
                DrawEnemySpawnPos(normalSpawnData.Waves[i].RoundInfo[j].PlayableAIInfos, i + 1, normalSpawnData.Waves[i].RoundInfo[j].EntryRound, DrawEnemyType.PLAYABLE);
            }
        }

        DrawCommon();
    }

    private void DrawCommon()
    {
        DrawPlayerSpawnPos();

    }



}
