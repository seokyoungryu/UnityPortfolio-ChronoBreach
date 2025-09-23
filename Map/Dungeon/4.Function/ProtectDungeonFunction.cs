using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Function/Protect Function ", fileName = "ProtectFunction")]
public class ProtectDungeonFunction : BaseDungeonFunction
{
    [SerializeField] private float startWaveTime = 2f;

    public IEnumerator ExcuteProcess(ProtectedDungeonTitle title)
    {
        //title.SpawnData.dungeon = title.dungeonCoroutine;
        // title.DungeonMapData.ExcuteTeleportMap();
        // title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        // GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        // title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        //title.SpawnData.SpawnProtectAI();
        // CommonUIManager.Instance.ExcuteGlobalNotifer("AI를 보호하며 던전 끝까지 도달하세요.");
        // if (title.SpawnData.IsFollowing)
        //     title.SpawnData.ProtectAIController.aIVariables.followTarget = title.ExcuteController.transform;
        SoundManager.Instance.PlayBGM_CrossFade(title.BaseBGM, 4f);
        title.SpawnData.onCompleteDungeon += () => QuestManager.Instance.ReceiveReport(QuestCategoryDefines.COMPLETE_DUNGEON, title.TaskTarget, 1);
        title.SpawnData.onExcuteBoss += () => { SoundManager.Instance.PlayBGM_CrossFade(title.BossBGM, 3f); };

        title.SpawnData.dungeon = title.dungeonCoroutine;
        title.DungeonMapData.ExcuteTeleportMap();
        ScenesManager.Instance.OnExcuteAfterLoading = () => title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.SettingPlayerController(title.ExcuteController);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.SpawnProtectAI();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.CreateExistBarrier();
        ScenesManager.Instance.OnExcuteAfterLoading += () => CommonUIManager.Instance.ExcuteGlobalNotifer(title.InitGlobalNotifier);

        GameManager.Instance.Player.playerStats.OnDead_ += () => title?.SpawnData?.ExcuteFailProcess();

        //ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.StartWave();

        yield return new WaitUntil(() => title.SpawnData.isCreateProectedAI);


        yield return new WaitForSeconds(startWaveTime);

        title.SpawnData.StartWave();

        // 1. 던전 entry시 controller 세팅해줌. newController는 생성하고 category에 excuteController로 받아옴.
        // 2. mapData 세팅해두기. 이거 -> 세팅이 해당 맵으로 이동해주기. 
        // 3. controller 위치 이동.
        // spawnData 실행. 1. tRigger, 2. Immediate 
        // spawnData에서 생성할때 각각 onDead에 해당 info의 state 변경하게하기?.
    }

}
