using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseDungeonMapData : ScriptableObject
{
    [Header("(SpawnPosition)")]
    [SerializeField] private int playerSpawnPositionIndex = -1;
    [SerializeField] private Vector3 playerRotation = -Vector3.one;


    public int SpawnIndex => playerSpawnPositionIndex;
    public Vector3 PlayerRotation => playerRotation;
    /// <summary>
    /// 설정한 map으로 이동.
    /// </summary>
    public abstract void ExcuteTeleportMap();

    public virtual void ExcuteTeleportController(PlayerStateController controller, DungeonSpawnPositionList dungeonSpawnPosition)
    {
        Transform refTrans = dungeonSpawnPosition.GetSpawnTransform(playerSpawnPositionIndex);
        controller.TranslatePosition(refTrans.localPosition);
        if (playerRotation == -Vector3.one)
            controller.transform.rotation = refTrans.rotation;
        else if (playerRotation != -Vector3.one)
            controller.transform.rotation = Quaternion.Euler(playerRotation);
        Debug.Log("playerSpawnPositionIndex : " + playerSpawnPositionIndex);
        Debug.Log("refTrans : " + refTrans.position + " ||  localPosition : " + refTrans.localPosition+ " || 현 위치 : " + controller.transform.position);
    }


}
