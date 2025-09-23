using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TriggerSpawnInfo<T , I>: BaseSpawnInfos  where T : TriggerDungeonRound<I>
{
    [SerializeField] protected SpawnTriggerInfo[] spawnTrigger;
    [SerializeField] protected SpawnBarrierInfo[] spawnBarriers; //트리거 할 경우 -> 벽 생성.
    [SerializeField] protected T[] roundInfo;
    public T[] RoundInfo => roundInfo;
    public T CurrentRound => roundInfo[currentRound];


    protected int currentRound = 0;
    protected bool isDone = false;

    public int CurrentRoundIndex { get { return currentRound; } set { currentRound = value; } }
    public int CurrentEntryRound => currentRound + 1;
    
    public SpawnTriggerInfo[] SpawnTrigger => spawnTrigger;
    public SpawnBarrierInfo[] SpawnBarriers => spawnBarriers;


    public bool HaveNextRound(int index)
    {     
        if (index > currentRound + 1) 
            return true;
        return false;
    }

    public virtual I[] GetRoundEnemy(int currentEntryRound)
    {
        List<I> retInfo = new List<I>();
        foreach (T info in roundInfo)
            if (info.EntryRound == currentEntryRound)
                retInfo = info.EnemyInfos;
        return retInfo.ToArray();
    }
}

[System.Serializable]
public abstract class TriggerDungeonRound<T> 
{
    [SerializeField] protected string roundName = string.Empty;
    [SerializeField] protected int entryRound = 0;
    [SerializeField] protected List<T> enemyInfos; //entryRound에 등장할 몬스터 리스트
    [SerializeField] protected BaseDungeonEnemyInfo[] playableAIInfos;
    [SerializeField] protected SpawnProcessType roundState = SpawnProcessType.WAIT;
    public string RoundName { get { return roundName; } set { roundName = value; } }
    public int EntryRound { get { return entryRound; } set { entryRound = value; } }
    public SpawnProcessType RoundState { get { return roundState; } set { roundState = value; } }

    public BaseDungeonEnemyInfo[] PlayableAIInfos => playableAIInfos;
    public List<T> EnemyInfos => enemyInfos;

    public abstract bool IsCompleteRound();
    public bool IsCompleteRound(BaseDungeonEnemyInfo[] infos)
    {
        for (int i = 0; i < infos.Length; i++)
        {
            if (infos[i].EnemyState != EnemyState.DEAD)
                return false;
        }
        return true;
    }
}

[System.Serializable]
public class SpawnTriggerInfo
{
    [Header("(TriggerIndex)")]
    public int positionIndex = -1;
    [HideInInspector] public Vector3 triggerPosition = Vector3.zero;
    [Header("(-1,-1,-1)일 경우 triggerIndex의 scale을 사용.")]
    public Vector3 extend = -Vector3.one;

    public void SettingValue(Transform trigger)
    {
        Debug.Log("Wall : " + trigger.position + " :: " + trigger.localPosition);
        if (positionIndex != -1)
            triggerPosition = trigger.localPosition;
        if (extend == -Vector3.one)
            extend = trigger.localScale;
    }
}

[System.Serializable]
public class SpawnBarrierInfo
{
    public ObjectPoolingList obpList;
    [Header("(TriggerIndex)")]
    public int positionIndex = -1;
    [HideInInspector] public Vector3 spawnPosition = Vector3.zero;
    [Header("(-1,-1,-1)일 경우 triggerIndex의 값을 사용.")]
    public Vector3 spawnRotation = -Vector3.one;
    public Vector3 spawnSize = -Vector3.one;

    public void SettingTransform(Transform trigger)
    {
        if (positionIndex != -1)
            spawnPosition = trigger.localPosition;
        if (spawnRotation == -Vector3.one)
            spawnRotation = trigger.rotation.eulerAngles;
        if (spawnSize == -Vector3.one)
            spawnSize = trigger.localScale;
    }
}


[System.Serializable]
public class CheckBossBGMInfo
{
    [Header("Delay Check Time ")]
    public float delayTime = 0f;
    [Header("(TriggerIndex)")]
    public int positionIndex = -1;
    [HideInInspector] public Vector3 spawnPosition = Vector3.zero;
    [Header("(-1,-1,-1)일 경우 triggerIndex의 값을 사용.")]
    public Vector3 spawnRotation = -Vector3.one;
    public Vector3 spawnSize = -Vector3.one;

    public void SettingTransform(Transform trigger)
    {
        if (positionIndex != -1)
            spawnPosition = trigger.localPosition;
        if (spawnRotation == -Vector3.one)
            spawnRotation = trigger.rotation.eulerAngles;
        if (spawnSize == -Vector3.one)
            spawnSize = trigger.localScale;
    }
}


