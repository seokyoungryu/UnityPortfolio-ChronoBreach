using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Function/Target Function ", fileName = "TargetFunction_")]
public class TargetDungeonFunction : BaseDungeonFunction
{
    private TargetSpawnData spawnData = null;

    private List<TargetDungeonEnemyInfo> targets = new List<TargetDungeonEnemyInfo>();
    //TargetEnemyInfos list 등록하기.  이거는 trigger, round, 한번에 에 따라서 달라.
    // trigger1번일 경우, 라운드 시작시 targets에 해당 라운드의 타겟들 저장. 해당 타겟들이 죽어야됨.
    // 다음 라운드 시작시 targets 초기화하고 , 해당 라운드 타겟 등록하는 방식.

    // 2번 같은 경우도 해당 웨이브(라운드1개임)의 타겟들 저장하고 죽으면 다음 웨이브 이런식.
    // 한번에 같은 경우는 전체 생성하니까 -> allTargets 로 등록하기.

    public void SettingSpawnData(TargetSpawnData spawnData)
    {
        this.spawnData = spawnData;
    }

    public void ExcuteProcess(TargetDungeonTitle title)
    {
        SoundManager.Instance.PlayBGM_CrossFade(title.BaseBGM,4f);
        title.SpawnData.onCompleteDungeon += () => QuestManager.Instance.ReceiveReport(QuestCategoryDefines.COMPLETE_DUNGEON, title.TaskTarget, 1);
        title.SpawnData.dungeon = title.dungeonCoroutine;
        title.SpawnData.onExcuteBoss += () => { SoundManager.Instance.PlayBGM_CrossFade(title.BossBGM, 3f); };

        title.DungeonMapData.ExcuteTeleportMap();
        ScenesManager.Instance.OnExcuteAfterLoading = () => title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.CreateExistBarrier();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.StartWave();
        ScenesManager.Instance.OnExcuteAfterLoading += () => CommonUIManager.Instance.ExcuteGlobalNotifer(title.InitGlobalNotifier);

        GameManager.Instance.Player.playerStats.OnDead_ += () => title?.SpawnData?.ExcuteFailProcess();

        // 1. mapData 세팅해두기. 
        // 2. 던전 entry시 controller 세팅해줌. newController는 생성하고 category에 excuteController로 받아옴.
        // 3. controller 위치 이동.
        // spawnData 실행. 1. tRigger, 2. Immediate 
        // spawnData에서 생성할때 각각 onDead에 해당 info의 state 변경하게하기?.

        //타겟은 trigger일 경우, 해당 웨이브의 타겟을 죽일 경우 다음 라운드 실행?
        //정하기. 웨이브1 -> 각 라운드 존재.
        //Trigger일 경우 
        // 1. 각 라운드마다 타겟을 잡아야지 다음 라운드로 이동. ( 타겟 아닌것이 살아있다면 죽임 ), 모든 라운드 타겟 잡으면, 
        //     다음 웨이브로 이동. 
        // 2. 라운드가 1라운드바께없고, 웨이브1에 trigger 되면  해당 타겟만 죽이면 아음 웨이브2로 이동 가능하게. 
        //근데 1,2번 중복아님? 결국엔 라운드에서 타겟을 잡아야지 다음 웨이브로 가는거잖아.

        //타겟이 immediate일 경우
        // 그냥 모든 웨이브 enemy 전부 생성.
    }





}
