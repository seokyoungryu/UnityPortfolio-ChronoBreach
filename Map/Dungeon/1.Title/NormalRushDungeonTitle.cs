using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Map/Dungeon Title/Rush Title ", fileName = "RushTitle_")]
public class NormalRushDungeonTitle : BaseDungeonTitle
{
    [SerializeField] protected NormalRushSpawnData spawnData;
    [SerializeField] protected NormalRushDungeonFunction function;

    public NormalRushSpawnData SpawnData => spawnData;
    public NormalRushDungeonFunction Function => function;
    public override void ClearObj()
    {
        base.ClearObj();
        spawnData.ClearObjs();

    }
    public override BaseDungeonTitle GetClone()
    {
        return Clone();
    }
    public NormalRushDungeonTitle Clone()
    {
        NormalRushDungeonTitle cloneTitle = Instantiate(this);
        cloneTitle.id = id;
        cloneTitle.taskTarget = Instantiate(taskTarget);
        cloneTitle.dungeonSpawnPosition = Instantiate(dungeonSpawnPosition);
        cloneTitle.dungeonReward = Instantiate(dungeonReward);
        cloneTitle.dungeonCondition = Instantiate(dungeonCondition);

        cloneTitle.dungeonCateogry = Instantiate(dungeonCateogry);
        cloneTitle.dungeonMapData = Instantiate(dungeonMapData);
        cloneTitle.spawnData = Instantiate(spawnData);
        cloneTitle.function = Instantiate(function);
        cloneTitle.dungeonCoroutine = dungeonCoroutine;
        cloneTitle.originController = originController;
        cloneTitle.excuteController = excuteController;
        return cloneTitle;
    }

    public override void Excute()
    {
        function.ExcuteProcess(this);
    }

    public override void ExcuteDrawAllWave()
    {
        base.ExcuteDrawAllWave();

        for (int i = 0; i < spawnData.Rushs.Length; i++)
        {
            DrawEnemySpawnPos(spawnData.Rushs[i].RushEnemyInfos, i + 1, 0, DrawEnemyType.NORAML);
            DrawEnemySpawnPos(spawnData.Rushs[i].PlayableAIInfos, i + 1, 0, DrawEnemyType.PLAYABLE);
        }

        DrawCommon();
    }

    public override void ExcuteDrawCurrentWave(int currentIndex)
    {
       int index = currentIndex == -1 ? spawnData.CurrentRushIndex : currentIndex;
       base.ExcuteDrawCurrentWave(index);
        DrawEnemySpawnPos(spawnData.Rushs[index].RushEnemyInfos, index + 1, 0, DrawEnemyType.NORAML);
        DrawEnemySpawnPos(spawnData.Rushs[index].PlayableAIInfos, index + 1, 0, DrawEnemyType.PLAYABLE);
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
        DrawBarrier(spawnData.ExistBarriers);
        DrawCheckBossBGM(spawnData.CheckBossBgmInfo);

#endif
    }

}