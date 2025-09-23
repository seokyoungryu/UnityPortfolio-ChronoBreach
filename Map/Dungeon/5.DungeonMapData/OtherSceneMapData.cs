using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Map/Dungeon Map Data/Other Scene Map Data", fileName = "OtherMapdata_")]
public class OtherSceneMapData : BaseDungeonMapData
{
    [SerializeField] private string sceneName = string.Empty;

    public override void ExcuteTeleportMap()
    {
        Debug.Log("OtherSceneMapData ½ÇÇà");
        ScenesManager.Instance.ChangeScene(sceneName);

        //SceneManager.LoadScene(sceneName , LoadSceneMode.Single);
    }

}
