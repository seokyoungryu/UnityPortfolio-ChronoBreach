using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NormalRushSpawnInfos : RushSpawnInfo<NormalRushDungeonEnemyInfo>
{
      
}

[System.Serializable]
public class NormalRushDungeonEnemyInfo : BaseDungeonEnemyInfo
{
    [SerializeField] private float delaySpawnTime = 0f;
    [SerializeField] private float runSpeed = -1f;

    public float DelaySpawnTime { get { return delaySpawnTime; } set { delaySpawnTime = value; } }

    public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
}
