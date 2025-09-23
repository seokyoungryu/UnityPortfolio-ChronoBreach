using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NormalSpawnInfos : TriggerSpawnInfo<NormalDungeonRound, BaseDungeonEnemyInfo>
{
    public bool IsEndRound(BaseDungeonEnemyInfo[] infos)
    {
        BaseDungeonEnemyInfo[] retInfos = infos;
        for (int i = 0; i < infos.Length; i++)
            if (infos[i].EnemyState == EnemyState.ACTIVE)
                return false;
        return true;
    }

}


[System.Serializable]
public class NormalDungeonRound : TriggerDungeonRound<BaseDungeonEnemyInfo>
{
    public override bool IsCompleteRound()  => IsCompleteRound(enemyInfos.ToArray());

}