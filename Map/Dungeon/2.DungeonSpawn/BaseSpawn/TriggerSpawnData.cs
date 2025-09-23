using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class TriggerSpawnData<T> : BaseSpawnData where T : BaseSpawnInfos
{
    [SerializeField] protected TriggerSpawnType spawnType = TriggerSpawnType.TRIGGER;
    [SerializeField] protected T[] waves;
    protected int currentWaveIndex = 0;
    protected bool isStartRound = false;
    protected Collider[] entryTriggerCollider = new Collider[3];
    protected ReturnObjectToObjectPooling[] allBarriers;

    private int entryCount = 0;
    private RaycastHit[] castHit =  new RaycastHit[10];
    protected LayerMask triggerLayerMask = TagAndLayerDefine.LayersIndex.Player;

    

    public int EntryCount => entryCount;
    public TriggerSpawnType SpawnType => spawnType;
     public T CurrentWave => waves[currentWaveIndex];
     public T NextWave => waves.Length > currentWaveIndex ? waves[currentWaveIndex + 1] : null;

    public int CurrentWaveIndex => currentWaveIndex;
    public T[] Waves => waves;
    #region Events

    #endregion


    public override void ClearObjs()
    {
        base.ClearObjs();

    }


    /// <summary>
    /// 최종 게임 종료 조건.
    /// </summary>
    protected virtual bool CheckCompleteDungeon()
    {
        if (IsAllWavesComplete()) return true;
        else if (CheckCompleteImmediate()) return true;
        return false;
    }

    public virtual bool IsCompleteWave<I, R>(TriggerSpawnInfo<I,R>[] waves, int waveIndex) where I : TriggerDungeonRound<R>
    {
        for (int i = 0; i < waves[waveIndex].RoundInfo.Length; i++)
            if (waves[waveIndex].RoundInfo[i].RoundState != SpawnProcessType.COMPLETE)
                return false;
        return true;
    }

    protected abstract void SetCompleteAllWave();
    public abstract bool CheckCompleteImmediate();
    public abstract void StartWave();
    public abstract void NextStartWave();
    public abstract void StartRound();
    public abstract void NextStartRound();
    public abstract void SpawnRoundEnemy(int currentWaveIndex, int roundIndex);
    public abstract void StartImmediate();
    public abstract bool IsEndRound();
    public abstract void CompleteCurrentRoundState();
    public abstract bool HaveNextRound();

    public override void ExcuteFailProcess()
    {
        base.ExcuteFailProcess();
        isFailDungeon = true;
        Debug.Log("아웃");
    }

    
    protected IEnumerator BoxcastEntryTriggerBox(SpawnTriggerInfo trigger)
    {
        if (dungeonPositinList == null)
        {
            Debug.Log("NULL DungeonPosiList1");
            yield return new WaitForSeconds(.5f);
        }
        if (dungeonPositinList == null)
        {
            Debug.Log("NULL DungeonPosiList2");
            yield return new WaitForSeconds(.5f);
        }
        if (dungeonPositinList == null)
        {
            dungeonPositinList = MapManager.Instance.CurrentDungeonTitle.DungeonSpawnPosition;
            Debug.Log("dungeonPositinList NUll Setting : " + dungeonPositinList);
        }

        entryCount = 0;
        isStartRound = false;
        trigger.SettingValue(dungeonPositinList.GetTriggerTransform(trigger.positionIndex));
        while (!isStartRound)
        {
            entryCount = BoxCast(trigger, triggerLayerMask);
            if (entryCount > 0)
            {
                StartRound();
                isStartRound = true;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    protected int BoxCast(SpawnTriggerInfo trigger , LayerMask targetLayer)
    {
        return Physics.BoxCastNonAlloc(trigger.triggerPosition + Vector3.up * trigger.extend.y / 2f, trigger.extend * 0.5f, Vector3.up, castHit, Quaternion.identity, 0, targetLayer); 
    }
  

    public virtual void OnDeadTrigger()
    {
        if (isCompleteDungeon) return;

        if (IsEndRound())
        {
            CompleteCurrentRoundState();
            if (HaveNextRound())
                NextStartRound();
            else if (HaveNextWave(waves.Length))
            {
                if (allBarriers != null)
                    for (int i = 0; i < allBarriers.Length; i++)   //없앨때 없어지는 effect그자리에 생성?
                        allBarriers[i].SetOBP();
                NextStartWave();
            }
        }

        SetCompleteAllWave();
        if (CheckCompleteDungeon() && !isCompleteDungeon)
        {
            Debug.Log("여기서 끝!!!!!!");
            ExcuteEndProcess(); 
        }
    }

    public virtual void OnDeadImmediate()
    {
        if (CheckCompleteDungeon())
            ExcuteEndProcess();
    }


    public virtual async Task<AIController> SpawnEnemy(BaseDungeonEnemyInfo info)
    {
        AIController enemy = await CreateAI(info, CreateAIType.ENEMY);
        Debug.Log("생성 : " + info.EnemyObpName);

        enemy.ClearOnDead();
        enemy.onExtraDead += info.Dead;
        if (spawnType == TriggerSpawnType.IMMEDIATE)
            enemy.onExtraDead += OnDeadImmediate;
        else if (spawnType == TriggerSpawnType.TRIGGER)
            enemy.onExtraDead += OnDeadTrigger;

        info.Active();
        info.SetController(enemy);

        return enemy;
    }

    
    public virtual void StartRoundProcess() { }


    public void StartWave<I,R>(TriggerSpawnInfo<I,R> currentWave) where I : TriggerDungeonRound<R>
    {
        dungeon.StartCoroutine(CheckBossBGM());
        if (currentWave.GlobalNotifier != string.Empty)
            CommonUIManager.Instance.ExcuteGlobalBattleNotifer(currentWave.GlobalNotifier, 8f);
        currentWaveIndex = 0;
        currentWave.CurrentProcess = SpawnProcessType.RUNNING;
        if (spawnType == TriggerSpawnType.TRIGGER)
            for (int i = 0; i < currentWave.SpawnTrigger.Length; i++)
            {
                Debug.Log("BoxcastEntryTriggerBox : " + i);
                dungeon.StartCoroutine(BoxcastEntryTriggerBox(currentWave.SpawnTrigger[i]));
            }
        else if (spawnType == TriggerSpawnType.IMMEDIATE)
            StartImmediate();

        //init 플레이어블 AI 생성하기.
        SpawnAllPlayableAI(InitPlayerableAIInfos.ToArray());

    }

    public virtual void NextStartWave<I, R>(TriggerSpawnInfo<I, R> nextWave) where I : TriggerDungeonRound<R>
    {
        if (nextWave == null) return;

        currentWaveIndex += 1;
        if (nextWave.GlobalNotifier != string.Empty)
            CommonUIManager.Instance.ExcuteGlobalBattleNotifer(nextWave.GlobalNotifier, 8f);


        // CommonUIManager.Instance.ExcuteGlobalNotifer($"Next Wave {currentWaveIndex} 시작!");
        nextWave.CurrentProcess = SpawnProcessType.RUNNING;
        if (spawnType == TriggerSpawnType.TRIGGER)
            for (int i = 0; i < nextWave.SpawnTrigger.Length; i++)
            {
                Debug.Log("NextStartWave : " + i);
                dungeon.StartCoroutine(BoxcastEntryTriggerBox(nextWave.SpawnTrigger[i]));
            }
    }

    public virtual IEnumerator StartRound<I, R>(TriggerSpawnInfo<I, R> currentWave) where I : TriggerDungeonRound<R>
    {
        yield return dungeon.StartCoroutine(WaitTime(currentWave.WaitNextRound));

       // CommonUIManager.Instance.ExcuteGlobalNotifer($"Start Round");
       // CommonUIManager.Instance.ExcuteGlobalNotifer($"라운드 {currentWave.CurrentEntryRound} 시작!");
        currentWave.CurrentRoundIndex = 0;
        currentWave.CurrentRound.RoundState = SpawnProcessType.RUNNING;
        SpawnRoundEnemy(currentWaveIndex, currentWave.CurrentRoundIndex);
        //Round 플레이블  AI 생성.

        if (currentWave.SpawnBarriers.Length > 0)
        {
            allBarriers = new ReturnObjectToObjectPooling[currentWave.SpawnBarriers.Length];
            for (int i = 0; i < currentWave.SpawnBarriers.Length; i++)
                allBarriers[i] = SetBarrier(currentWave.SpawnBarriers[i]);
        }
        StartRoundProcess();
    }


    public virtual IEnumerator NextStartRound<I, R>(TriggerSpawnInfo<I, R> currentWave) where I : TriggerDungeonRound<R>
    {
        currentWave.CurrentRoundIndex++;
        yield return dungeon.StartCoroutine(WaitTime(currentWave.WaitNextRound));
        //CommonUIManager.Instance.ExcuteGlobalNotifer($"라운드 {currentWave.CurrentEntryRound} 시작!");
        currentWave.CurrentRound.RoundState = SpawnProcessType.RUNNING;
        SpawnRoundEnemy(currentWaveIndex, currentWave.CurrentRoundIndex);
        //Round 플레이블  AI 생성.

    }

    public virtual void StartImmediate<I, R>(TriggerSpawnInfo<I, R>[] waves) where I : TriggerDungeonRound<R>
    {
       // CommonUIManager.Instance.ExcuteGlobalNotifer("즉시 실행됨");

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].CurrentProcess = SpawnProcessType.RUNNING;
            for (int x = 0; x < waves[i].RoundInfo.Length; x++)
            {
                waves[i].RoundInfo[x].RoundState = SpawnProcessType.RUNNING;
                SpawnRoundEnemy(i, x);
            }
        }

    }

    protected bool IsCompleteImmediateWave<I, R>(TriggerSpawnInfo<I, R>[] waves,int waveIndex) where I : TriggerDungeonRound<R>
    {
        for (int i = 0; i < waves[waveIndex].RoundInfo.Length; i++)
            for (int x = 0; x < waves[waveIndex].RoundInfo[i].EnemyInfos.Count; x++)
                if (!waves[waveIndex].RoundInfo[i].IsCompleteRound())
                    return false;
        return true;
    }

    public bool CheckCompleteImmediate<I, R>(TriggerSpawnInfo<I, R>[] waves) where I : TriggerDungeonRound<R>
    {
        if (spawnType != TriggerSpawnType.IMMEDIATE) return false;

        for (int i = 0; i < waves.Length; i++)
            if (!IsCompleteImmediateWave<I,R>(waves,i))
                return false;
        return true;
    }

    public void CompleteCurrentRoundState<I, R>(TriggerSpawnInfo<I, R> currentWave) where I : TriggerDungeonRound<R>
      => currentWave.RoundInfo[currentWave.CurrentRoundIndex].RoundState = SpawnProcessType.COMPLETE;
     public bool HaveNextRound<I, R>(TriggerSpawnInfo<I, R> currentWave) where I : TriggerDungeonRound<R>
        => currentWave.HaveNextRound(currentWave.RoundInfo.Length);
    protected bool HaveNextWave(int waveLength)
    {
        if (waves.Length > currentWaveIndex + 1)
            return true;
        return false;
    }


    public virtual bool IsAllWavesComplete()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            if (waves[i].CurrentProcess != SpawnProcessType.COMPLETE)
            {
                Debug.Log(i+" Wave Not Complete");
                return false;
            }
        }
        return true;
    }

}


public enum TriggerSpawnType
{
    TRIGGER = 0,
    IMMEDIATE = 1,
}
