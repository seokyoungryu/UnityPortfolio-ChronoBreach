using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Spawn Data/Normal Spawn Data", fileName = "NormalSpawnData_")]
public class NormalSpawnData : TriggerSpawnData<NormalSpawnInfos>
{

    public override bool CheckCompleteImmediate()
    {
        return CheckCompleteImmediate(waves);
    }


    protected override void SetCompleteAllWave()
    {
        for (int i = 0; i < waves.Length; i++)
            if (IsCompleteWave(waves,i))
                waves[i].CurrentProcess = SpawnProcessType.COMPLETE;
    }
    public override void StartImmediate() => StartImmediate(waves);
    public override void StartWave() => StartWave(CurrentWave);
    public override void StartRound() => dungeon.StartCoroutine( StartRound(CurrentWave));
    public override void NextStartWave() => NextStartWave(NextWave);
    public override void NextStartRound() => dungeon.StartCoroutine(NextStartRound(CurrentWave));

    public override bool IsEndRound() => CurrentWave.IsEndRound(CurrentWave.GetRoundEnemy(CurrentWave.CurrentEntryRound));
    public override void CompleteCurrentRoundState() => CompleteCurrentRoundState(CurrentWave);
    public override bool HaveNextRound() => HaveNextRound(CurrentWave);

    protected bool IsCompleteImmediateWave(int waveIndex)
    {
        for (int i = 0; i < waves[waveIndex].RoundInfo.Length; i++)
            for (int x = 0; x < waves[waveIndex].RoundInfo[i].EnemyInfos.Count; x++)
                if (waves[waveIndex].RoundInfo[i].EnemyInfos[x].EnemyState != EnemyState.DEAD)
                    return false;
        return true;
    }

   
    public async override void SpawnRoundEnemy(int currentWaveIndex,int roundIndex)
    {
        BaseDungeonEnemyInfo[] roundInfos = waves[currentWaveIndex].GetRoundEnemy(roundIndex + 1);
        for (int i = 0; i < roundInfos.Length; i++)
        {
            SpawnEnemy(roundInfos[i]);
            await System.Threading.Tasks.Task.Delay(eachSpawnDelayTime);
        }
    }


    public override void ExcuteEndProcess()
    {
        base.ExcuteEndProcess();

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




