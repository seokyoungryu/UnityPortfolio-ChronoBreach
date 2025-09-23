using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Spawn Data/Target Spawn Data", fileName = "TargetSpawnData_")]
public class TargetSpawnData : TriggerSpawnData<TargetSpawnInfos>
{

    [Header("Setting")]
    [SerializeField] private bool resetTarget = false;
    private int targetCount = 0;


    [ContextMenu("타겟들 초기화")]
    public void ResetTargets()
    {
        for (int i = 0; i < waves.Length; i++)
            for (int j = 0; j < waves[i].RoundInfo.Length; j++)
                for (int k = 0; k < waves[i].RoundInfo[j].EnemyInfos.Count; k++)
                    waves[i].RoundInfo[j].EnemyInfos[k].IsTarget = false;
    }


    protected override void SetCompleteAllWave()
    {
        for (int i = 0; i < waves.Length; i++)
            if (IsCompleteWave(waves, i))
                waves[i].CurrentProcess = SpawnProcessType.COMPLETE;
    }

    protected override bool CheckCompleteDungeon()
    {
        TargetDungeonEnemyInfo[] allTargets = GetAllTargetInfos();

        if (CheckAllTargetsDead(allTargets)) return true;
        else if (CheckCompleteImmediate()) return true;
        return false;
    }

    public override bool CheckCompleteImmediate() => CheckCompleteImmediate(waves);
    public override void StartImmediate() => StartImmediate(waves);
    public override void StartWave() => StartWave(CurrentWave);
    public override void StartRound() => dungeon.StartCoroutine(StartRound(CurrentWave));
    public override void NextStartWave() => NextStartWave(NextWave);
    public override void NextStartRound() => dungeon.StartCoroutine(NextStartRound(CurrentWave));

    public override bool IsEndRound()
    {
        TargetDungeonEnemyInfo[] infos = GetRoundTargetInfo(currentWaveIndex, CurrentWave.CurrentRoundIndex);
        if (infos.Length <= 0)
        {
            infos = CurrentWave.GetRoundEnemy(CurrentWave.CurrentEntryRound);
            return CurrentWave.IsEndRoundCheckAllEnemy(infos);
        }
        else
            return CurrentWave.IsEndRoundCheckOnlyTargets(CurrentWave.GetRoundEnemy(CurrentWave.CurrentEntryRound));
    }
    public override void CompleteCurrentRoundState() => CompleteCurrentRoundState(CurrentWave);
    public override bool HaveNextRound() => HaveNextRound(CurrentWave);

    public override async Task<AIController> SpawnEnemy(BaseDungeonEnemyInfo info)
    {
        AIController contr = await base.SpawnEnemy(info);
        if (info is TargetDungeonEnemyInfo)
        {
            if ((info as TargetDungeonEnemyInfo).IsTarget)
            {
                contr.ActiveDungeonTargetMarker(true);
                contr.onExtraDead += () => contr.ActiveDungeonTargetMarker(false);
                contr.onExtraDead += () => targetCount--;
                contr.onExtraDead += () => MapManager.Instance.DungeonNotifierUI.SetText("타겟 수 : " + targetCount);

                targetCount++;
                MapManager.Instance.DungeonNotifierUI.SetText("타겟 수 : " + targetCount);
            }
        }

        return contr;
    }

    public override void SpawnRoundEnemy(int currentWaveIndex, int roundIndex)
    {
        BaseDungeonEnemyInfo[] roundInfos = waves[currentWaveIndex].GetRoundEnemy(roundIndex + 1);
        for (int i = 0; i < roundInfos.Length; i++)
            SpawnEnemy(roundInfos[i]);
    }

    public override void OnDeadTrigger()
    {
        base.OnDeadTrigger();
        AfterClearRoundEnemy();
    }

    public override void OnDeadImmediate()
    {
        base.OnDeadImmediate();
        SetAllRoundComplete();
        SetCompleteAllWave();
        AfterClearRoundEnemy();
    }

    protected void AfterClearRoundEnemy()
    {
        for (int i = 0; i < waves.Length; i++)
            for (int x = 0; x < waves[i].RoundInfo.Length; x++)
                if (waves[i].RoundInfo[x].RoundState == SpawnProcessType.COMPLETE && !waves[i].RoundInfo[x].IsDoneOtherState
                    && waves[i].RoundInfo[x].CompleteState == DungeonCompleteState.KILL)
                    waves[i].RoundInfo[x].OtherStateKill();
    }


    private void SetAllRoundComplete()
    {
        for (int i = 0; i < waves.Length; i++)
            SetRoundComplete(i);
    }

    private void SetRoundComplete(int waveIndex)
    {
        for (int i = 0; i < waves[waveIndex].RoundInfo.Length; i++)
        {
            TargetDungeonEnemyInfo[] infos = GetRoundTargetInfo(waveIndex, i);
            if (infos.Length <= 0 || infos == null)
                infos = waves[waveIndex].RoundInfo[i].EnemyInfos.ToArray();
            bool isCompleteRound = true;
            for (int x = 0; x < infos.Length; x++)
                if (infos[x].EnemyState != EnemyState.DEAD)
                    isCompleteRound = false;

            if (isCompleteRound) 
                waves[waveIndex].RoundInfo[i].RoundState = SpawnProcessType.COMPLETE;
        }
    }


    public override void ExcuteEndProcess()
    {
        if (isCompleteDungeon) return;
        base.ExcuteEndProcess();

        isCompleteDungeon = true;
        //End 프로세스. 
        //End를 음.. 타겟에는 처리한 타겟 수 
        //레이싱에는 등수 등 여러 항목이 추가되거나 삭제될텐데.
        //이부분은 음 아이템 정보창처럼 생성하느식으로해주기.
        //고려할점은 각 항목들이 floating처럼 올라가는 거 ?
       // CommonUIManager.Instance.ExcuteGlobalNotifer("★☆★☆★ 던전 클리어 ★☆★☆★");

    }

  

    public TargetDungeonEnemyInfo[] GetAllTargetInfos()
    {
        List<TargetDungeonEnemyInfo> retInfo = new List<TargetDungeonEnemyInfo>();

        for (int i = 0; i < waves.Length; i++)
        {
            TargetDungeonEnemyInfo[] infos = GetWaveTargetInfo(i);
            if (infos.Length <= 0) continue;

            for (int x = 0; x < infos.Length; x++)
                retInfo.Add(infos[x]);
        }

        return retInfo.ToArray();
    }


    public TargetDungeonEnemyInfo[] GetWaveTargetInfo(int index)
    {
        if (waves[index].RoundInfo.Length <= 0) return null;

        List<TargetDungeonEnemyInfo> retInfos = new List<TargetDungeonEnemyInfo>();

        for (int i = 0; i < waves[index].RoundInfo.Length; i++)
        {
            TargetDungeonEnemyInfo[] infos = waves[index].GetRoundEnemy(i + 1);
            if (infos.Length <= 0) continue;
            for (int x = 0; x < infos.Length; x++)
                if (infos[x].IsTarget)
                    retInfos.Add(infos[x]);
        }

        return retInfos.ToArray();
    }



    public TargetDungeonEnemyInfo[] GetRoundTargetInfo(int waveIndex, int roundIndex)
    {
        if (waves[waveIndex].RoundInfo[roundIndex].EnemyInfos.Count <= 0) return null;

        List<TargetDungeonEnemyInfo> retInfos = new List<TargetDungeonEnemyInfo>();
        TargetDungeonEnemyInfo[] infos = waves[waveIndex].GetRoundEnemy(roundIndex + 1);

        if (infos.Length <= 0) return null;

        for (int i = 0; i < infos.Length; i++)
            if (infos[i].IsTarget)
                retInfos.Add(infos[i]);

        return retInfos.ToArray();
    }


    public bool CheckAllTargetsDead(TargetDungeonEnemyInfo[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
            if (targets[i].IsTarget && targets[i].EnemyState != EnemyState.DEAD)
                return false;

        return true;
    }


    private void OnValidate()
    {
        if (waves.Length <= 0) return;
        for (int i = 0; i < waves.Length; i++)
            if (waves.Length >= i)
                waves[i].InfoName = "Wave " + (i + 1);

        for (int i = 0; i < waves.Length; i++)
            for (int x = 0; x < waves[i].RoundInfo.Length; x++)
            {
                waves[i].RoundInfo[x].RoundName = "라운드" + (x + 1);
                waves[i].RoundInfo[x].EntryRound = (x + 1);
            }
    }

  
}

