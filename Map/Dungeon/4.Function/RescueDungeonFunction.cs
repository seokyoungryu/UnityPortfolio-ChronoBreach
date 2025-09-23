using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Function/Rescue Function ", fileName = "RescueFunction_")]
public class RescueDungeonFunction : BaseDungeonFunction
{
    private RescuerSpawnData spawnData = null;

    public void SettingSpawnData(RescuerSpawnData spawnData)
    {
        this.spawnData = spawnData;
    }

    public void ExcuteProcess(RescueDungeonTitle title)
    {
        // title.SpawnData.dungeon = title.dungeonCoroutine;
        // title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        //  title.DungeonMapData.ExcuteTeleportMap();
        //  GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        // if (title.SpawnData.MinRescuerCount > title.SpawnData.RescuerInfosCount)
        //     title.SpawnData.MinRescuerCount = title.SpawnData.RescuerInfosCount;
        // title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        // title.SpawnData.StartWave();
        // title.SpawnData.RescuerSetTartget(title.ExcuteController.transform);

        SoundManager.Instance.PlayBGM_CrossFade(title.BaseBGM, 4f);
        title.SpawnData.onCompleteDungeon += () =>
        {
            Debug.Log("완료 실행! : " + title.TaskTarget.name);
            QuestManager.Instance.ReceiveReport(QuestCategoryDefines.COMPLETE_DUNGEON, title.TaskTarget, 1);
        };
        title.SpawnData.onExcuteBoss += () => { SoundManager.Instance.PlayBGM_CrossFade(title.BossBGM, 3f); };

        title.SpawnData.dungeon = title.dungeonCoroutine;
        title.DungeonMapData.ExcuteTeleportMap();
        ScenesManager.Instance.OnExcuteAfterLoading = () => title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        ScenesManager.Instance.OnExcuteAfterLoading += () =>
        {
            if (title.SpawnData.MinRescuerCount > title.SpawnData.RescuerInfosCount)
                title.SpawnData.MinRescuerCount = title.SpawnData.RescuerInfosCount;

        };
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.CreateExistBarrier();

        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.RescuerSetTartget(title.ExcuteController.transform);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.StartWave();
        ScenesManager.Instance.OnExcuteAfterLoading += () => CommonUIManager.Instance.ExcuteGlobalNotifer(title.InitGlobalNotifier);

        GameManager.Instance.Player.playerStats.OnDead_ += () => title?.SpawnData?.ExcuteFailProcess();

    }





}