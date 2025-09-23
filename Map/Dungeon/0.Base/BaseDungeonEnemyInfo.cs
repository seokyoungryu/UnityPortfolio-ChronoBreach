using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseDungeonEnemyInfo
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private ObjectPoolingList enemyObpName;
    [SerializeField] private AIInfoList enemyInfo = AIInfoList.None;
    [SerializeField] private int positionIndex = -1;
    private Vector3 position = Vector3.zero;
    [SerializeField] private Vector3 rotation = -Vector3.one;
    [SerializeField] private Vector3 size = -Vector3.one;
    [SerializeField] private bool isForceRunning = false;
    [SerializeField] private EnemyState enemyState = EnemyState.DISABLE;
    [SerializeField] private EffectInfo spawnAppearEffect = new EffectInfo() { 
                                                                             effect = EffectList.EnemyAppearEffect_Black,
                                                                             spawnScale = Vector3.one * 1.5f
                                                                             };

    [SerializeField] private WayPointInfo[] wayPointInfos;
    private AIController enemyController = null;


    public AIController AIController => enemyController;
    public LayerMask TargetLayer { get { return targetLayer; } set { targetLayer = value; } }
    public ObjectPoolingList EnemyObplist { get { return enemyObpName; } set { enemyObpName = value; } }

    public string EnemyObpName => enemyObpName.ToString();
    public AIInfoList EnemyInfoList { get { return enemyInfo; } set { enemyInfo = value; } }
    public int SpawnPositionIndex { get { return positionIndex; } set { positionIndex = value; } }
    public bool IsForceRunning { get { return isForceRunning; } set { isForceRunning = value; } }

    public Vector3 SpawnPosition => position;
    public Vector3 SpawnRotation => rotation;
    public Vector3 SpawnSize => size;
    public EnemyState EnemyState => enemyState;
    public EffectInfo SpawnAppearEffect { get { return spawnAppearEffect; } set { spawnAppearEffect = value; } }

    public WayPointInfo[] WayPointInfos => wayPointInfos;

    public void SetController(AIController controller) => enemyController = controller;
    public void SetSpawnPositions(Vector3 spawnPosition) => this.position = spawnPosition;
    public void SetSpawnRositions(Vector3 spawnRosition) => this.rotation = spawnRosition;
    public void SetSpawnScales(Vector3 spawnScale) => this.size = spawnScale;

    public void Dead()
    {
        enemyState = EnemyState.DEAD;
        enemyController = null;
    }
    public void Active() => enemyState = EnemyState.ACTIVE;
    public void Kill(bool canDropItem = true, bool excuteOnDead = true)
    {
        if (enemyController == null || enemyState == EnemyState.DEAD) return;

        if (!canDropItem) enemyController.aiConditions.CanDropItem = false;
        else enemyController.aiConditions.CanDropItem = true;
        if (!excuteOnDead) enemyController.ClearOnDead();
        enemyController.Kill();
        Dead();
    }
}


public enum EnemyState
{
    DISABLE = 0,
    ACTIVE =1,
    DEAD = 2,
}