using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RescuerSpawnInfos : TriggerSpawnInfo<RescuerDungeonRound, BaseDungeonEnemyInfo>
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
public class RescuerDungeonRound : TriggerDungeonRound<BaseDungeonEnemyInfo>
{
    public override bool IsCompleteRound() => IsCompleteRound(enemyInfos.ToArray());

}

[System.Serializable]
public class RescuerDungeonEnemyInfo : BaseDungeonEnemyInfo
{
    [SerializeField] private float followDistance = 2f;
    [SerializeField] private float followWalkSpeed = -1f;
    [SerializeField] private float followRunSpeed = -1f;
    //[SerializeField] private bool isEntryRotateToPlayer = true;
    private bool isRescued = false;                        


    public float FollowWalkSpeed => followWalkSpeed;
    public float FollowRunSpeed => followRunSpeed;
    public float FollowDistance => followDistance;
    public bool IsResuced => isRescued;
   // public bool IsEntryRotateToPlayer => isEntryRotateToPlayer;


    public void Rescue() => isRescued = true;
}