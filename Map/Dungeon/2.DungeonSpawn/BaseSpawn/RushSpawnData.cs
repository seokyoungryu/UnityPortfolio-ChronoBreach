using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class RushSpawnData<T> : BaseSpawnData where T : BaseSpawnInfos
{
    [SerializeField] protected DungeonCompleteState completeEnemyState = DungeonCompleteState.ALIVE;
    [SerializeField] protected DungeonCompleteState completePlayableState = DungeonCompleteState.ALIVE;

    private List<BaseDungeonEnemyInfo> spawnedEnemyInfos = new List<BaseDungeonEnemyInfo>();
    private List<BaseDungeonEnemyInfo> spawnedPlayerableInfos = new List<BaseDungeonEnemyInfo>();

    [Header("(TriggerIndex) (-1)일 경우 rush -> 플레이어 Tranform")]
    [SerializeField] private int enemyRushPositionIndex = -1;
    [SerializeField] protected T[] rushs;
    protected int currentRushIndex = 0;
    protected T CurrentRush => rushs[currentRushIndex];
    protected T NextRush => rushs[currentRushIndex + 1];
    public T[] Rushs => rushs;

    public int CurrentRushIndex => currentRushIndex;


    public override void ClearObjs()
    {
        base.ClearObjs();

    }
    public override void ExcuteEndProcess()
    {
        base.ExcuteEndProcess();
        isCompleteDungeon = true;
        if (completeEnemyState == DungeonCompleteState.KILL)
            ExcuteAIAllDead(spawnedEnemyInfos);
        if (completePlayableState == DungeonCompleteState.KILL)
            ExcuteAIAllDead(spawnedPlayerableInfos);
    }

    public override void ExcuteFailProcess()
    {
        base.ExcuteFailProcess();

    }

    protected virtual bool CheckDungeonComplete()
    {
        if (CheckAllRushComplete())
            return true;

        return false;
    }

    protected void ExcuteAIAllDead(List<BaseDungeonEnemyInfo> infos)
    {
        for (int i = 0; i < infos.Count; i++)
            infos[i].Kill(false,false);
    }

    public abstract void StartRush();
    public abstract void NextStartRush();
    public abstract void StartRound();
    protected abstract bool CheckAllRushComplete();
    public abstract void AIDead();
    public virtual void EnemySettings(AIController controller, BaseDungeonEnemyInfo info) { }

    protected bool HaveNextRush()
    {
       // if (NextRush != null)
       if(rushs.Length > (currentRushIndex +1))
            return true;
        return false;
    }

    protected virtual async void SpawnRushEemeys<I>(RushSpawnInfo<I> currentRush) where I : BaseDungeonEnemyInfo
    {
        for (int i = 0; i < currentRush.RushEnemyInfos.Length; i++)
        {
            if (isCompleteDungeon) return;
            if (currentRush.RushEnemyInfos[i] is NormalRushDungeonEnemyInfo)
                dungeon.StartCoroutine(DelaySpawnEnemy(currentRush.RushEnemyInfos[i] as NormalRushDungeonEnemyInfo));
            await System.Threading.Tasks.Task.Delay(eachSpawnDelayTime);
        }

        for (int i = 0; i < currentRush.PlayableAIInfos.Length; i++)
           await SpawnPlayableAI(currentRush.PlayableAIInfos[i]);
    }

    protected async Task<AIController> SpawnEnemy(BaseDungeonEnemyInfo info)
    {
        AIController controller = await CreateAI(info, CreateAIType.ENEMY);

        if (enemyRushPositionIndex != -1)
            controller.aIVariables.targetVector = dungeonPositinList.GetTriggerPosition(enemyRushPositionIndex);
        else
            controller.aIVariables.target = playerController;
        controller.ClearOnDead();
        controller.onExtraDead += info.Dead;
        controller.onExtraDead += AIDead;

        EnemySettings(controller, info);
        controller.TransitionToState(controller.currentState);
        info.Active();
        info.SetController(controller);
        spawnedEnemyInfos.Add(info);

        return controller;
    }

    private IEnumerator DelaySpawnEnemy(NormalRushDungeonEnemyInfo info)
    {
        yield return new WaitForSeconds(info.DelaySpawnTime);
        SpawnEnemy(info);
    }


    protected virtual void StartRush<I>(RushSpawnInfo<I> currentRush) where I : BaseDungeonEnemyInfo
    {
        if (currentRush == null) return;

        if (currentRush.GlobalNotifier != string.Empty)
            CommonUIManager.Instance.ExcuteGlobalBattleNotifer(currentRush.GlobalNotifier, 8f);
        dungeon.StartCoroutine(CheckBossBGM());
     //  CommonUIManager.Instance.ExcuteGlobalNotifer($"Start Rush");
        currentRushIndex = 0;
        spawnedEnemyInfos.Clear();
        currentRush.CurrentProcess = SpawnProcessType.RUNNING;
        StartRound();
    }

    protected virtual void NextStartRush<I>(RushSpawnInfo<I> currentRush) where I : BaseDungeonEnemyInfo
    {
        if (currentRush == null) return;
        if (currentRush.GlobalNotifier != string.Empty)
            CommonUIManager.Instance.ExcuteGlobalBattleNotifer(currentRush.GlobalNotifier, 8f);

        //CommonUIManager.Instance.ExcuteGlobalNotifer($"Next Rush");
        currentRushIndex++;
        currentRush.CurrentProcess = SpawnProcessType.RUNNING;
        StartRound();
    }


    protected virtual IEnumerator StartRound<I>(RushSpawnInfo<I> currentRush) where I : BaseDungeonEnemyInfo
    {
        if (currentRush == null) yield break;

       // CommonUIManager.Instance.ExcuteGlobalNotifer($"Start Round");
        currentRush.CurrentRoundIndex = 0;
        yield return WaitTime(currentRush.WaitNextRound);
        SpawnRushEemeys(currentRush);
    }



}
