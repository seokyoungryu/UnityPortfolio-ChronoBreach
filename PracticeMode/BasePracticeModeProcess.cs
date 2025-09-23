using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BasePracticeModeProcess : MonoBehaviour
{
    private PlayerStateController player;
    [SerializeField] private OtherSceneMapData practiveMapData;
    [SerializeField] private DungeonSpawnPositionList practiceSpawnList;

    [SerializeField] private int maxScareCrowCount;
    [SerializeField] private int maxEnemyAICount;
    [SerializeField] private int maxStandingBossCount;
    [SerializeField] private int maxShooterCount;
    [SerializeField] private int levelUpValue;
    [SerializeField] private int getGoldValue;


    [SerializeField] private BaseDungeonEnemyInfo scareCrowObp ;
    [SerializeField] private BaseDungeonEnemyInfo enemyAIObp;
    [SerializeField] private BaseDungeonEnemyInfo standingBossObp;
    [SerializeField] private BaseDungeonEnemyInfo shooterObp;



    [SerializeField] private int spawnEnemyAiIndex = 1;
    [SerializeField] private float spawnAIRandomRadius = 10f;
    [SerializeField] private List<int> spawnScareCrowIndexs = new List<int>();
    [SerializeField] private List<int> spawnShooterIndexs = new List<int>();
    [SerializeField] private int spawnStandingBossIndex = 1;
    [SerializeField] private float spawnStandingBossRandomRadius = 10f;


    private int currentEnemyAiCount = 0;
    private int currentStandingBossCount = 0;
    private int currentScareCrowsCount = 0;
    private int currentShooterCount = 0;
    private int currentPlayerLevelUpValue = 1;
    private int currentGoldValue = 5000;

    private List<AIController> enemyAIs = new List<AIController>();
    private List<AIController> scareCrows = new List<AIController>();
    private List<BaseController> shooters = new List<BaseController>();
    private List<AIController> standingBosses = new List<AIController>();

    public int CurrentEnemyAiCount => currentEnemyAiCount;
    public int CurrentScareCrowsCount => currentScareCrowsCount;
    public int CurrentStandingBossCount => currentStandingBossCount;
    public int CurrentPlayerLevelUpValue => currentPlayerLevelUpValue;
    public int CurrentGoldValue => currentGoldValue;
    public int CurrentShooterCount => currentShooterCount;

    #region Events
    public delegate void Update(BasePracticeModeProcess process);

    public event Update onUpdate;
    #endregion

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    private void Start()
    {
        AllReset();
    }


    public void SetPlayerLv(bool add)
    {
        if (add)
            currentPlayerLevelUpValue += levelUpValue;
        else
        {
            currentPlayerLevelUpValue -= levelUpValue;
            if (currentPlayerLevelUpValue <= levelUpValue)
                currentPlayerLevelUpValue = levelUpValue;
        }
        onUpdate?.Invoke(this);

    }


    public void SetGold(bool add)
    {
        if (add)
            currentGoldValue += getGoldValue;
        else
        {
            currentGoldValue -= getGoldValue;
            if (currentGoldValue <= getGoldValue)
                currentGoldValue = getGoldValue;
        }
        onUpdate?.Invoke(this);

    }
   
    public void AllReset()
    {
        currentEnemyAiCount = 0;
        currentScareCrowsCount = 0;
        currentShooterCount = 0;
        currentStandingBossCount = 0;
        currentGoldValue = getGoldValue;
        currentPlayerLevelUpValue = levelUpValue;
        for (int i = 0; i < scareCrows.Count; i++)
            ObjectPooling.Instance.SetOBP(scareCrows[i].OBPName, scareCrows[i].gameObject);
        for (int i = 0; i < enemyAIs.Count; i++)
            ObjectPooling.Instance.SetOBP(enemyAIs[i].OBPName, enemyAIs[i].gameObject);
        for (int i = 0; i < shooters.Count; i++)
            ObjectPooling.Instance.SetOBP(shooterObp.EnemyObplist.ToString(), shooters[i].gameObject);
        for (int i = 0; i < standingBosses.Count; i++)
            ObjectPooling.Instance.SetOBP(standingBosses[i].OBPName, standingBosses[i].gameObject);

        enemyAIs.Clear();
        scareCrows.Clear();
        shooters.Clear();
        standingBosses.Clear();
        ResetPlayerPosition();
        onUpdate?.Invoke(this);

    }

    public void ResetSkillCoolTime()
    {
        player.skillController.ResetSkillCoolTime();
    }

    public void ResetPlayerPosition()
    {
        if (player.IsDead())
            player.Resurrection();

        player.TranslatePosition(practiceSpawnList.GetSpawnPosition(practiveMapData.SpawnIndex));
        player.transform.rotation = practiceSpawnList.GetSpawnRotation(practiveMapData.SpawnIndex);
        GameManager.Instance.Cam.ResetRotation();
    }

    public void PlayerLevelUp()
    {
        GameManager.Instance.Player.playerStats.AddLevel(currentPlayerLevelUpValue);
    }

    public void Resurrection()
    {
        player.Resurrection();
        GameManager.Instance.Cam.ResetRotation();
        
    }
    public async void SpawnShooterCrow()
    {
        if (currentShooterCount >= maxShooterCount) return;

        Vector3 pos = practiceSpawnList.GetSpawnPosition(spawnShooterIndexs[currentShooterCount]);
        EffectManager.Instance.GetEffectObjectInfo(shooterObp.SpawnAppearEffect, pos, Vector3.zero, Vector3.zero);
        currentShooterCount += 1;
        onUpdate?.Invoke(this);

        await System.Threading.Tasks.Task.Delay(300);


        BaseController ai = ObjectPooling.Instance.GetOBP(shooterObp.EnemyObplist.ToString()).GetComponent<BaseController>();
        shooters.Add(ai);
        Vector3 rot = Vector3.zero;
        rot = practiceSpawnList.GetSpawnRotation(spawnShooterIndexs[shooters.Count - 1]).eulerAngles;
        ai.transform.localScale = shooterObp.SpawnSize;
        ai.transform.rotation = Quaternion.Euler(rot);
        ai.transform.position = pos;

    }

    public void DeleteShooter()
    {
        if (currentShooterCount <= 0) return;

        BaseController ai = shooters[shooters.Count - 1];
        ObjectPooling.Instance.SetOBP(shooterObp.EnemyObplist.ToString(), ai.gameObject);
        shooters.RemoveAt(shooters.Count - 1);
        currentShooterCount -= 1;
        onUpdate?.Invoke(this);

    }

    public void DeleteAllShooter()
    {
        for (int i = 0; i < shooters.Count; i++)
            ObjectPooling.Instance.SetOBP(shooterObp.EnemyObplist.ToString(), shooters[i].gameObject);

        currentShooterCount = 0;
        shooters.Clear();
        onUpdate?.Invoke(this);
    }
    public async void SpawnStandingBoss()
    {
        if (currentStandingBossCount >= maxStandingBossCount) return;

        Vector3 pos = practiceSpawnList.GetSpawnPosition(spawnStandingBossIndex);
        float randomX = Random.Range(-spawnStandingBossRandomRadius, spawnStandingBossRandomRadius);
        float randomZ = Random.Range(-spawnStandingBossRandomRadius, spawnStandingBossRandomRadius);
        Vector3 retPos = new Vector3(pos.x + randomX, 0, pos.z + randomZ);
        EffectManager.Instance.GetEffectObjectInfo(standingBossObp.SpawnAppearEffect, retPos, Vector3.zero, Vector3.zero);

        currentStandingBossCount += 1;
        onUpdate?.Invoke(this);

        await System.Threading.Tasks.Task.Delay(300);

        AIController ai = AIManager.Instance.CreateAI(standingBossObp.EnemyObplist.ToString(), standingBossObp.EnemyInfoList);
        ai.TranslatePosition(retPos);
        ai.transform.localScale = standingBossObp.SpawnSize;
        Vector3 rot = Vector3.zero;
        rot = practiceSpawnList.GetSpawnRotation(spawnStandingBossIndex).eulerAngles;
        ai.transform.localScale = standingBossObp.SpawnSize;
        ai.RotateByVector(rot);
        standingBosses.Add(ai);

    }

    public void DeleteStandingBoss()
    {
        if (currentStandingBossCount <= 0) return;

        AIController ai = standingBosses[standingBosses.Count - 1];
        ObjectPooling.Instance.SetOBP(ai.OBPName, ai.obpGo);
        standingBosses.RemoveAt(standingBosses.Count - 1);
        currentStandingBossCount -= 1;
        onUpdate?.Invoke(this);

    }

    public void DeleteAllStandingBoss()
    {
        for (int i = 0; i < standingBosses.Count; i++)
            ObjectPooling.Instance.SetOBP(standingBosses[i].OBPName, standingBosses[i].obpGo);

        currentStandingBossCount = 0;
        standingBosses.Clear();
        onUpdate?.Invoke(this);
    }


    public async void SpawnScareCrow()
    {
        if (currentScareCrowsCount >= maxScareCrowCount) return;

        Vector3 pos = practiceSpawnList.GetSpawnPosition(spawnScareCrowIndexs[currentScareCrowsCount]);
        EffectManager.Instance.GetEffectObjectInfo(scareCrowObp.SpawnAppearEffect, pos, Vector3.zero, Vector3.zero);
        currentScareCrowsCount += 1;
        onUpdate?.Invoke(this);

        await System.Threading.Tasks.Task.Delay(300);


        AIController ai = AIManager.Instance.CreateAI(scareCrowObp.EnemyObplist.ToString(), scareCrowObp.EnemyInfoList);
        scareCrows.Add(ai);
        Vector3 rot = Vector3.zero;
        rot = practiceSpawnList.GetSpawnRotation(spawnScareCrowIndexs[scareCrows.Count - 1]).eulerAngles;
        ai.transform.localScale = scareCrowObp.SpawnSize;
        ai.RotateByVector(rot);
        ai.TranslatePosition(pos);

    }

    public void DeleteScareCrow()
    {
        if (currentScareCrowsCount <= 0) return;

        AIController ai = scareCrows[scareCrows.Count - 1];
        ObjectPooling.Instance.SetOBP(ai.OBPName, ai.obpGo);
        scareCrows.RemoveAt(scareCrows.Count - 1);
        currentScareCrowsCount -= 1;
        onUpdate?.Invoke(this);

    }

    public void DeleteAllScareCrow()
    {
        for (int i = 0; i < scareCrows.Count; i++)
            ObjectPooling.Instance.SetOBP(scareCrows[i].OBPName, scareCrows[i].obpGo);

        currentScareCrowsCount = 0;
        scareCrows.Clear();
        onUpdate?.Invoke(this);
    }

    public async void SpawnEnemyAI()
    {
        if (currentEnemyAiCount >= maxEnemyAICount) return;
        Vector3 pos = practiceSpawnList.GetSpawnPosition(spawnEnemyAiIndex);
        float randomX = Random.Range(-spawnAIRandomRadius, spawnAIRandomRadius);
        float randomZ = Random.Range(-spawnAIRandomRadius, spawnAIRandomRadius);
        Vector3 retPos = new Vector3(pos.x + randomX, 0, pos.z + randomZ);
        EffectManager.Instance.GetEffectObjectInfo(enemyAIObp.SpawnAppearEffect, retPos, Vector3.zero, Vector3.zero);

        currentEnemyAiCount += 1;
        onUpdate?.Invoke(this);

        await System.Threading.Tasks.Task.Delay(300);

        AIController ai = AIManager.Instance.CreateAI(enemyAIObp.EnemyObplist.ToString(), enemyAIObp.EnemyInfoList);
        enemyAIs.Add(ai);
        ai.TranslatePosition(retPos);
        ai.transform.localScale = enemyAIObp.SpawnSize;


    }
    public void DeleteEnemyAI()
    {
        if (currentEnemyAiCount <= 0) return;

        AIController ai = enemyAIs[enemyAIs.Count - 1];
        ObjectPooling.Instance.SetOBP(ai.OBPName, ai.obpGo);
        enemyAIs.RemoveAt(enemyAIs.Count - 1);
        currentEnemyAiCount -= 1;
        onUpdate?.Invoke(this);

    }

    public void DeleteAllEnemyAI()
    {
        for (int i = 0; i < enemyAIs.Count; i++)
            ObjectPooling.Instance.SetOBP(enemyAIs[i].OBPName, enemyAIs[i].obpGo);

        enemyAIs.Clear();
        currentEnemyAiCount = 0;
        onUpdate?.Invoke(this);

    }


    public void Immortal(bool active)
    {
        GameManager.Instance.Player.Conditions.IsImmortal = active;
    }
    public void InfinityStamina(bool active)
    {
        GameManager.Instance.Player.Conditions.InfinityStamina = active;
    }
    public void IgnoreDamage(bool active)
    {
        GameManager.Instance.Player.Conditions.IgnoreDamaged = active;
    }
    public void GetGold()
    {
        GameManager.Instance.SetPlusOwnMoney(currentGoldValue);
    }

    public void GetSkillPoint(RewardSkillPoint reward)
    {
        reward.Giver(null);
    }
}
