using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Position", fileName = "DungeonPosition_")]
public class DungeonSpawnPositionList : ScriptableObject
{
    [SerializeField] private MapDrawPositions scenePosition_Prefab = null;

    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private List<Transform> triggerPosition = new List<Transform>();
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();

    public List<Transform> SpawnPositions => spawnPositions;
    public List<Transform> WayPoints => wayPoints;
    public List<Transform> TriggerPosition => triggerPosition;

    public Vector3 GetSpawnPosition(int index) => index >= 0 ? spawnPositions[index].localPosition : Vector3.zero;
    public Vector3 GetTriggerPosition(int index) => index >= 0 ? triggerPosition[index].localPosition : Vector3.zero;
    public Quaternion GetSpawnRotation(int index) => index >= 0 ? spawnPositions[index].rotation : Quaternion.identity;

    public Transform GetSpawnTransform(int index) => index >= 0 ? spawnPositions[index] : null;
    public Transform GetTriggerTransform(int index) => index >= 0 ? triggerPosition[index] : null;
    public Transform GetWayPointsTransform(int index) => index >= 0 ? wayPoints[index] : null;


    private void OnValidate()
    {
        if(scenePosition_Prefab != null)
        {
            for (int i = 0; i < scenePosition_Prefab.Positions.Length; i++)
            {
                if (scenePosition_Prefab.Positions[i].DisplayNameType == MapPositionDlayName.SPAWN)
                    spawnPositions = scenePosition_Prefab.Positions[i].Positions;
                if (scenePosition_Prefab.Positions[i].DisplayNameType == MapPositionDlayName.WAYPOINT)
                    wayPoints = scenePosition_Prefab.Positions[i].Positions;
                if (scenePosition_Prefab.Positions[i].DisplayNameType == MapPositionDlayName.TRIGGER)
                    triggerPosition = scenePosition_Prefab.Positions[i].Positions;
            }
        }
    }


    [ContextMenu("Setting")]
    private void Setting()
    {
        if (scenePosition_Prefab != null)
        {
            for (int i = 0; i < scenePosition_Prefab.Positions.Length; i++)
            {
                if (scenePosition_Prefab.Positions[i].DisplayNameType == MapPositionDlayName.SPAWN)
                    spawnPositions = scenePosition_Prefab.Positions[i].Positions;
                if (scenePosition_Prefab.Positions[i].DisplayNameType == MapPositionDlayName.WAYPOINT)
                    wayPoints = scenePosition_Prefab.Positions[i].Positions;
                if (scenePosition_Prefab.Positions[i].DisplayNameType == MapPositionDlayName.TRIGGER)
                    triggerPosition = scenePosition_Prefab.Positions[i].Positions;
            }
        }
    }
}


