using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustEnemySpawn : MonoBehaviour
{
    public Vector3 rushTargetPoint;
    public DungeonSpawnPositionList map;
    [SerializeField] private EnemyInfo[] info;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            for (int i = 0; i < info.Length; i++)
            {
                AIController enemy = AIManager.Instance.CreateAI(info[i].obpList.ToString(), info[i].aiInfoList);
                enemy.ResetAI();
                enemy.TranslatePosition(info[i].enemySpawnPosition);
                enemy.RotateByVector(info[i].enemyRotation);
                enemy.ClearWayPoints();
                enemy.aIVariables.targetVector = rushTargetPoint;
                enemy.aiConditions.IsForceRunning = info[i].isForceRunning;

                for (int x = 0; x < info[i].waypoints.Length; x++)
                {
                    enemy.SetWayPoints(info[i].waypoints[x], map.GetWayPointsTransform(info[i].waypoints[x].WayIndex));
                }

                enemy.aiStatus.ExcuteOnHPHUD();
                enemy.transform.localScale = info[i].scale;
                enemy.aIFSMVariabls.resetPos = info[i].enemySpawnPosition;
                enemy.targetLayer = info[i].targetLayer;
            }
        }

    }

}
