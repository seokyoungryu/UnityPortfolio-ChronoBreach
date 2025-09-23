using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Map/Dungeon Title/Protected Title ", fileName = "ProtectedTitle_")]
public class ProtectedDungeonTitle : BaseDungeonTitle
{
    [SerializeField] protected ProtectSpawnData protectSpawnData;
    [SerializeField] protected ProtectDungeonFunction protectFunction;

    public ProtectSpawnData SpawnData => protectSpawnData;
    public ProtectDungeonFunction Function => protectFunction;
    public override void ClearObj()
    {
        base.ClearObj();
        protectSpawnData.ClearObjs();

    }
    public override BaseDungeonTitle GetClone()
    {
        return Clone();
    }
    public ProtectedDungeonTitle Clone()
    {
        ProtectedDungeonTitle cloneTitle = Instantiate(this);
        cloneTitle.id = id;
        cloneTitle.taskTarget = Instantiate(taskTarget);
        cloneTitle.dungeonSpawnPosition = Instantiate(dungeonSpawnPosition);
        cloneTitle.dungeonReward = Instantiate(dungeonReward);
        cloneTitle.dungeonCondition = Instantiate(dungeonCondition);

        cloneTitle.dungeonCateogry = Instantiate(dungeonCateogry);
        cloneTitle.dungeonMapData = Instantiate(dungeonMapData);
        cloneTitle.protectSpawnData = Instantiate(protectSpawnData);
        cloneTitle.protectFunction = Instantiate(protectFunction);
        cloneTitle.dungeonCoroutine = dungeonCoroutine;
        cloneTitle.originController = originController;
        cloneTitle.excuteController = excuteController;
        return cloneTitle;
    }

    public override void Excute()
    {
        dungeonCoroutine.StartCoroutine(protectFunction.ExcuteProcess(this));
    }

    private void DrawCommon()
    {
#if UNITY_EDITOR
        DrawPlayerSpawnPos();
        protectSpawnData.FinalReachedPosition.triggerPosition = dungeonSpawnPosition.GetTriggerPosition(protectSpawnData.FinalReachedPosition.positionIndex);
        Handles.color = Color.red;
        Handles.Label(protectSpawnData.FinalReachedPosition.triggerPosition, "√÷¡æ µµ¬¯¡°");
        Handles.DrawWireCube(protectSpawnData.FinalReachedPosition.triggerPosition + Vector3.up * protectSpawnData.FinalReachedPosition.extend.y / 2f
                           , protectSpawnData.FinalReachedPosition.extend);

        Vector3 protectAIPos = dungeonSpawnPosition.GetSpawnPosition(protectSpawnData.ProtectAIInfo.SpawnPositionIndex);
        Handles.color = Color.red;
        Handles.Label(protectAIPos + Vector3.up * protectSpawnData.ProtectAIInfo.SpawnSize.y/2f, "ProtectAI Spawn");
        Handles.DrawWireCube(protectAIPos + Vector3.up * protectSpawnData.ProtectAIInfo.SpawnSize.y / 2f, protectSpawnData.ProtectAIInfo.SpawnSize);

#endif

    }

    public override void ExcuteDrawAllWave()
    {
#if UNITY_EDITOR
        base.ExcuteDrawAllWave();
        for (int i = 0; i < protectSpawnData.Waves.Length; i++)
        {
            DrawSpawnTriggerABarrier(protectSpawnData.Waves[i].SpawnTrigger, protectSpawnData.Waves[i].SpawnBarriers, protectSpawnData.ExistBarriers, protectSpawnData.CheckBossBgmInfo);
        }
        for (int i = 0; i < protectSpawnData.Waves.Length; i++)
        {

            for (int j = 0; j < protectSpawnData.Waves[i].RoundInfo.Length; j++)
            {
                DrawEnemySpawnPos(protectSpawnData.Waves[i].RoundInfo[j].EnemyInfos.ToArray(), i + 1, protectSpawnData.Waves[i].RoundInfo[j].EntryRound, DrawEnemyType.NORAML);
                DrawEnemySpawnPos(protectSpawnData.Waves[i].RoundInfo[j].PlayableAIInfos, protectSpawnData.CurrentWaveIndex + 1, protectSpawnData.Waves[i].RoundInfo[j].EntryRound, DrawEnemyType.PLAYABLE);
            }
        }

        for (int i = 0; i < protectSpawnData.Waves.Length; i++)
        {
            Vector3 protectAIEnterPos = dungeonSpawnPosition.GetTriggerPosition(protectSpawnData.Waves[i].AiEnterWayPointIndex);
            Vector3 protectAIEntryPos = dungeonSpawnPosition.GetTriggerPosition(protectSpawnData.Waves[i].ProtectAIEntryStopPositionIndex);

            Handles.Label(protectAIEntryPos + Vector3.up * 1.3f, "ProtectAI Entry Position");
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(protectAIEntryPos, 1f);

            Handles.Label(protectAIEnterPos + Vector3.up * 1.3f, "ProtectAI Enter Trigger Position");
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(protectAIEnterPos, 1f);
        }

        DrawCommon();

#endif
    }

    public override void ExcuteDrawCurrentWave(int currentIndex)
    {
        int index = currentIndex == -1 ? protectSpawnData.CurrentWaveIndex : currentIndex;
#if UNITY_EDITOR
        base.ExcuteDrawCurrentWave(index);
        DrawSpawnTriggerABarrier(protectSpawnData.Waves[index].SpawnTrigger, protectSpawnData.Waves[index].SpawnBarriers, protectSpawnData.ExistBarriers, protectSpawnData.CheckBossBgmInfo);
        DrawCommon();

        for (int i = 0; i < protectSpawnData.Waves[index].RoundInfo.Length; i++)
        {
            DrawEnemySpawnPos(protectSpawnData.Waves[index].RoundInfo[i].EnemyInfos.ToArray(), index + 1, protectSpawnData.Waves[index].RoundInfo[i].EntryRound, DrawEnemyType.NORAML);
            DrawEnemySpawnPos(protectSpawnData.Waves[index].RoundInfo[i].PlayableAIInfos, index + 1, protectSpawnData.Waves[index].RoundInfo[i].EntryRound, DrawEnemyType.PLAYABLE);
        }

        Vector3 protectAIEnterPos = dungeonSpawnPosition.GetTriggerPosition(protectSpawnData.Waves[index].AiEnterWayPointIndex);
        Vector3 protectAIEntryPos = dungeonSpawnPosition.GetTriggerPosition(protectSpawnData.Waves[index].ProtectAIEntryStopPositionIndex);

        Handles.Label(protectAIEntryPos + Vector3.up * 1.3f, "ProtectAI Entry Position");
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(protectAIEntryPos, 1f);

        Handles.Label(protectAIEnterPos + Vector3.up * 1.3f, "ProtectAI Enter Trigger Position");
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(protectAIEnterPos, 1f);
#endif

    }



}