using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Function/Normal Function ", fileName = "NormalFunction")]
public class NormalDungeonFunction : BaseDungeonFunction
{

    public void ExcuteProcess(NormalDungeonTitle title)
    {
        //title.SpawnData.dungeon = title.dungeonCoroutine;
        //title.DungeonMapData.ExcuteTeleportMap();
        //title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        //GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        //title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        //title.SpawnData.StartWave();

        SoundManager.Instance.PlayBGM_CrossFade(title.BaseBGM, 4f);
        title.SpawnData.dungeon = title.dungeonCoroutine;
        title.DungeonMapData.ExcuteTeleportMap();
        title.SpawnData.onExcuteBoss += () => { SoundManager.Instance.PlayBGM_CrossFade(title.BossBGM, 3f); };
        ScenesManager.Instance.OnExcuteAfterLoading = () => title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.StartWave();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.CreateExistBarrier();
        ScenesManager.Instance.OnExcuteAfterLoading += () => CommonUIManager.Instance.ExcuteGlobalNotifer(title.InitGlobalNotifier);

        title.SpawnData.onCompleteDungeon += () => QuestManager.Instance.ReceiveReport(QuestCategoryDefines.COMPLETE_DUNGEON, title.TaskTarget, 1);
        GameManager.Instance.Player.playerStats.OnDead_ += () => title?.SpawnData?.ExcuteFailProcess();


        // 1. 던전 entry시 controller 세팅해줌. newController는 생성하고 category에 excuteController로 받아옴.
        // 2. mapData 세팅해두기. 이거 -> 세팅이 해당 맵으로 이동해주기. 
        // 3. controller 위치 이동.
        // spawnData 실행. 1. tRigger, 2. Immediate 
        // spawnData에서 생성할때 각각 onDead에 해당 info의 state 변경하게하기?.



    }

}


//음..  생각해보는거.

//타이틀 - 기본
//카테고리 - 기본으로 사용할지, 새로운 모델 사용할지, 총 상태 사용할지등 설정함.
//맵 데이터 - 스폰할 맵 데이터, 맵 위치. 
//스폰데이터 - 맵에 생성할 위치, 생성 종류 등.
//기능 - (기본) -> normal spawn data 생성. ( trigger, 반복생성)  


//트리거
//트리거 검사 실행. 
//플레이어 검출시 -> 1웨이브의 1라운드 생성.
//생성할때 죽을경우 state상태 바꾸기 등록.
//생성할때 죽을경우 spawnData의 함수 등록. 내용은 현재 라운드 다 죽었는지 검사.다 죽었을 경우 다음 라운드 프로세스.

//즉시
//한번에 다 생성. 
//트리거 검사 x .
//죽을 경우 현재 상태들 검사.