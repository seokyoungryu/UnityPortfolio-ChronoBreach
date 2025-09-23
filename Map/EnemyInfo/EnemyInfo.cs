using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public bool isForceRunning = false;
    public LayerMask targetLayer;
    public ObjectPoolingList obpList; 
    public AIInfoList aiInfoList;
    public Vector3 enemySpawnPosition;
    public Vector3 enemyRotation;
    public Vector3 scale;
    public WayPointInfo[] waypoints;
}
