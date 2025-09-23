using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetSpawnInfos : TriggerSpawnInfo <TargetDungeonRound, TargetDungeonEnemyInfo>
{

    public bool IsEndRoundCheckOnlyTargets(TargetDungeonEnemyInfo[] infos)
    {
        TargetDungeonEnemyInfo[] retInfos = infos;
        for (int i = 0; i < infos.Length; i++)
            if (infos[i].IsTarget && infos[i].EnemyState != EnemyState.DEAD)
                return false;
        return true;
    }

    public bool IsEndRoundCheckAllEnemy(TargetDungeonEnemyInfo[] infos)
    {
        TargetDungeonEnemyInfo[] retInfos = infos;
        for (int i = 0; i < infos.Length; i++)
            if (infos[i].EnemyState == EnemyState.ACTIVE)
                return false;
        return true;
    }
}

[System.Serializable]
public class TargetDungeonRound : TriggerDungeonRound<TargetDungeonEnemyInfo>
{
    [SerializeField] private DungeonCompleteState completeState = DungeonCompleteState.ALIVE;
    private bool isDoneOtherState = false;

    public DungeonCompleteState CompleteState => completeState;
    public bool IsDoneOtherState => isDoneOtherState;

    public void OtherStateKill()
    {
        isDoneOtherState = true;
        if (completeState != DungeonCompleteState.KILL) return;

        for (int i = 0; i < enemyInfos.Count; i++)
            if (enemyInfos[i].EnemyState == EnemyState.ACTIVE)
                enemyInfos[i].Kill();
    }

    public override bool IsCompleteRound() => IsCompleteRound(enemyInfos.ToArray());

}

[System.Serializable]
public class TargetDungeonEnemyInfo : BaseDungeonEnemyInfo
{
    [SerializeField] private bool isTarget = false;

    public bool IsTarget { get { return isTarget; } set { isTarget = value; } }
}


/// <summary>
/// 해당 라운드의 타겟들이 죽었을 경우 다른 기본 몬스터 상태
/// </summary>
public enum DungeonCompleteState
{
    ALIVE = 0,
    KILL = 1,
}