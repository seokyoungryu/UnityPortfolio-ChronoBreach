using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum RushRandomMode { ADDITIVE = 0, CLEAR_ADD= 1,}

[System.Serializable]
public class SetRushRandomEnemyInfo
{
    public RushRandomMode mode;
    public LayerMask targerLayer;
    public ObjectPoolingList[] randomEnemys;
    public AIInfoList aiInfo;
    public int createCount = 1;
    public int setRushIndex = 0;
    public Vector2 randomSpawnIndex = Vector2.zero;
    public float startSumDelayTime = 0f;
    public Vector2 randomSpawnDelayTime = Vector2.zero;
    public Vector2 randomRunSpeed = Vector2.zero;
    public Vector3 rotation = Vector3.zero;
    public Vector2 randomScale = Vector2.zero;

}


[CreateAssetMenu(menuName = "Map/Dungeon Spawn Data/Normal Rush Spawn Data", fileName = "NormalRushSpawnData_")]
public class NormalRushSpawnData : RushSpawnData<NormalRushSpawnInfos>
{
    [Header("Apply Data¿ë")]
    [SerializeField] private EffectInfo appearEffect;
    [SerializeField] private SetRushRandomEnemyInfo setRandomInfo;
    protected bool printEnemyCount = false;

    private int enemyCount = 0;
    [SerializeField] private AIInfoList[] setInfo;
    public override void StartRush()
    {
        printEnemyCount = true;
        MapManager.Instance.DungeonNotifierUI.SetText("¸Á·É ¼ö : " + enemyCount);
        StartRush(CurrentRush);
    }
    public override void NextStartRush() => NextStartRush(NextRush);
    public override void StartRound() => dungeon.StartCoroutine(StartRound(CurrentRush));


    [ContextMenu("Apply AI Info")]
    public void ApplyAllAppear()
    {
        for (int i = 0; i < rushs.Length; i++)
            for (int j = 0; j < rushs[i].RushEnemyInfos.Length; j++)
                rushs[i].RushEnemyInfos[j].SpawnAppearEffect = appearEffect;
    }

    [ContextMenu("Print Count")]
    public void ApplyAllAppear1()
    {
        int[,] count = new int[30, 1];

        for (int i = 0; i < rushs.Length; i++)
            for (int j = 0; j < rushs[i].RushEnemyInfos.Length; j++)
                count[((int)rushs[i].RushEnemyInfos[j].EnemyObplist - 36), 0] += 1;

        for (int i = 0; i < count.Length; i++)
            if (count[i, 0] != 0)
                Debug.Log(Enum.GetName(typeof(ObjectPoolingList), (36 + i)) + "=" + count[i, 0]);

    }

    [ContextMenu("Apply Setting Only Info")]
    public void ApplySetInfo()
    {
        for (int i = 0; i < rushs[setRandomInfo.setRushIndex].RushEnemyInfos.Length; i++)
        {
            rushs[setRandomInfo.setRushIndex].RushEnemyInfos[i].EnemyInfoList = setInfo[UnityEngine.Random.Range(0,setInfo.Length)];
        }
    }


    [ContextMenu("Setting Random Enemys")]
    public void SettingEnemyForRandom()
    {
        List<NormalRushDungeonEnemyInfo> enemys = new List<NormalRushDungeonEnemyInfo>();
        float sumDelayTime = setRandomInfo.startSumDelayTime;
        for (int i = 0; i < setRandomInfo.createCount; i++)
        {
            NormalRushDungeonEnemyInfo enemy = new NormalRushDungeonEnemyInfo();
            int random = UnityEngine.Random.Range(0, setRandomInfo.randomEnemys.Length - 1);
            enemy.TargetLayer = setRandomInfo.targerLayer;
            enemy.EnemyInfoList = setRandomInfo.aiInfo;
            enemy.EnemyObplist = setRandomInfo.randomEnemys[random];
            enemy.SpawnPositionIndex = (int)UnityEngine.Random.Range(setRandomInfo.randomSpawnIndex.x, setRandomInfo.randomSpawnIndex.y);
            float delayTime = UnityEngine.Random.Range(setRandomInfo.randomSpawnDelayTime.x, setRandomInfo.randomSpawnDelayTime.y);
            sumDelayTime += delayTime;
            enemy.DelaySpawnTime = sumDelayTime;
            enemy.RunSpeed = UnityEngine.Random.Range(setRandomInfo.randomRunSpeed.x, setRandomInfo.randomRunSpeed.y);
            enemy.SetSpawnRositions(setRandomInfo.rotation);
            enemy.SetSpawnScales(Vector3.one * UnityEngine.Random.Range(setRandomInfo.randomScale.x, setRandomInfo.randomScale.y));
            enemy.IsForceRunning = true;
            enemys.Add(enemy);
        }

        if (setRandomInfo.mode == RushRandomMode.CLEAR_ADD)
        {
            rushs[setRandomInfo.setRushIndex].RushEnemyInfos = new NormalRushDungeonEnemyInfo[0];
            rushs[setRandomInfo.setRushIndex].RushEnemyInfos = enemys.ToArray();
        }   
        else if( setRandomInfo.mode == RushRandomMode.ADDITIVE)
        {
            rushs[setRandomInfo.setRushIndex].RushEnemyInfos = ArrayHelper.CombineTwoArray(rushs[setRandomInfo.setRushIndex].RushEnemyInfos, enemys.ToArray());
        }
    }


    [ContextMenu("Setting Enemys Info")]
    public void SettingEnemyAIInfo()
    {
        float sumDelayTime = setRandomInfo.startSumDelayTime;
        for (int i = 0; i < rushs[setRandomInfo.setRushIndex].RushEnemyInfos.Length; i++)
        {
            rushs[setRandomInfo.setRushIndex].RushEnemyInfos[i].EnemyInfoList = setInfo[0];
        }
    }



    public override void EnemySettings(AIController controller, BaseDungeonEnemyInfo info)
    {
        NormalRushDungeonEnemyInfo rushInfo = info as NormalRushDungeonEnemyInfo;
        if (rushInfo.RunSpeed != -1f)
            controller.aIVariables.runSpeed = rushInfo.RunSpeed;

        controller.onExtraDead += () => { enemyCount--; };
        controller.onExtraDead += () => { MapManager.Instance.DungeonNotifierUI.SetText("¸Á·É ¼ö : " + enemyCount); };
        controller.aIVariables.target = playerController;
        enemyCount++;
        if (printEnemyCount)
            MapManager.Instance.DungeonNotifierUI.SetText("¸Á·É ¼ö : " + enemyCount);

    }

    public override void AIDead()
    {
        SetRushCompleteProcess();

        if (CurrentRush.CurrentProcess == SpawnProcessType.COMPLETE && HaveNextRush())
            NextStartRush(NextRush);

        if (CheckDungeonComplete())
            ExcuteEndProcess();
    }

    private void SetRushCompleteProcess()
    {
        for (int i = 0; i < rushs.Length; i++)
            if (rushs[i].IsAllEnemyDead())
                rushs[i].CurrentProcess = SpawnProcessType.COMPLETE;
    }

    protected override bool CheckAllRushComplete()
    {
        for (int i = 0; i < rushs.Length; i++)
            if (rushs[i].CurrentProcess != SpawnProcessType.COMPLETE)
                return false;
        return true;
    }


    private void OnValidate()
    {
        if (rushs.Length <= 0) return;
        for (int i = 0; i < rushs.Length; i++)
            if (rushs.Length >= i)
                rushs[i].InfoName = "Rush " + (i + 1);


    }

}
