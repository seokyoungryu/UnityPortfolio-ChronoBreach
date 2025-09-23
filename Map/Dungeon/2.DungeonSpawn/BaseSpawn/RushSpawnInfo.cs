using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RushSpawnInfo<T> : BaseSpawnInfos where T : BaseDungeonEnemyInfo
{
    [SerializeField] private T[] rushEnemyInfos;
    [SerializeField] protected BaseDungeonEnemyInfo[] playableAIInfos;

    protected int currentRoundIndex = 0;
    public T CurrentRound => rushEnemyInfos[currentRoundIndex];
    public T NextRound => rushEnemyInfos[currentRoundIndex +1];

    public int CurrentRoundIndex { get { return currentRoundIndex; } set { currentRoundIndex = value; } }
    public T[] RushEnemyInfos { get { return rushEnemyInfos; } set { rushEnemyInfos = value; } }
    public BaseDungeonEnemyInfo[] PlayableAIInfos => playableAIInfos;

    public bool IsAllEnemyDead()
    {
        for (int i = 0; i < rushEnemyInfos.Length; i++)
            if (rushEnemyInfos[i].EnemyState != EnemyState.DEAD)
                return false;
        return true;
    }

    public void KillAllEnemy()
    {
        for (int i = 0; i < rushEnemyInfos.Length; i++)
                rushEnemyInfos[i].Kill(false, false);
    }
}

