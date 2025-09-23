using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIStatus : BaseStatus, IDamageable
{
    private AIController controller = null;
    private AIConditions aIConditions = null;
    private AIVariables aIVariables = null;
    private AIFSMVariabls aIFSMVariabls = null;

    [Header("AI Origin Stat Infomation")]
    [SerializeField] private string aiName = string.Empty;
    [SerializeField] private string aiNameUI = string.Empty;
    [SerializeField] private string aiCharacteristicNameUI = string.Empty;
    [SerializeField] private AIType aiType = AIType.COMMON;
    [SerializeField] private int healthBarCount = 0;
   [SerializeField] private float atkMinPercent = 0; // CurrentAtk * atkMinPercent.  ex) atkMinPer = 0.3이면 Random(CuAtk *atkMin , CuAtk) 
 
    [Header("Standing")]
    [SerializeField] private bool useStanding = false;
    [SerializeField] private float standingPercent = 0f;
    [SerializeField] private AIStandingType standingType = AIStandingType.ATTACK_COUNT;
    [SerializeField] private int standingAttackCount = 1;
    [SerializeField] private float increaseStandingAttackSpeed = 0f;
    private float targetDistance = 0f;
    [SerializeField] private bool ignoreDamageState = false;

    [Header("Defensing")]
    [SerializeField] private bool useDefense = false;
    [SerializeField] private float defensePercent = 0f;
    [SerializeField] private AIDefenseType defenseType = AIDefenseType.NONE;
    [SerializeField] private int defensePerCount = 0;
    [SerializeField] private float defensingTime = 0f;

    [Header("Etc")]
    [SerializeField] private bool immortality = false;
    [SerializeField] private bool recycleHpReset = false;
    [SerializeField] private float recycleHpResetTime = 0f;

    [Header("Rewards")]
    [SerializeField] private float exp = 0;
    [SerializeField] private DropItem[] dropItems = new DropItem[0];

    [Header("Extra Stats")]
    [SerializeField] private int extraLevel = 0;
    [SerializeField] private float extraExp = 0;

    [Header("Current Stats")]
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private float currentExp = 0;

    private IEnumerator recycleHpReset_Co = null;

    #region Gettter Setter Variables
    //Origin Stats
    public string AINameUI { get { return aiNameUI; }  }
    public string AICharacteristicNameUI { get { return aiCharacteristicNameUI; } }
    public int OriginLevel { get { return level; } }
    public AIType AIType { get { return aiType; } }
    public int OriginHealthBarCount { get { return healthBarCount; } }
    public float OriginExp { get { return exp; } }
    public DropItem[] DropItems { get { return dropItems; } }

    public bool UseStanding { get { return useStanding; } }
    public float StandingPercent { get { return standingPercent; } }
    public AIStandingType StandingType { get { return standingType; } }
    public int StandingAttackCount { get { return standingAttackCount; } }
    public float IncreaseStandingAttackSpeed { get { return increaseStandingAttackSpeed; } }
    public bool IgnoreDamageState { get { return ignoreDamageState; } }
    public bool Immortality { get { return immortality; } }
    public bool RecycleHpReset { get { return recycleHpReset; } }

    public bool UseDefense { get { return useDefense; } }
    public float DefensePercent { get { return defensePercent; } }
    public AIDefenseType DefenseType { get { return defenseType; } }
    public int DefensePerCount { get { return defensePerCount; } }
    public float DefensingTime { get { return defensingTime; } }


    //Extra Stats
    public int ExtraLevel { get { return extraLevel; }   set { extraLevel = value; } }
    public float ExtraExp { get{return extraExp; }   set{ extraExp = value; } }

    //Current Stats
    public float CurrentExp { get { return currentExp; } }
    public int CurrentLevel { get { return currentLevel;  } }                                                                  
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    public int TotalHealth => health + extraHealth;
    #endregion

    #region Events
    public delegate void OnChangedHP(AIStatus aIStatus);

    private event OnChangedHP onChangedHp;
    public event OnChangedHP OnChangedHPUI
    {
        add
        {
            if (onChangedHp == null || !onChangedHp.GetInvocationList().Contains(value))
                onChangedHp += value;
        }
        remove
        {
            onChangedHp -= value;
        }
    }
    #endregion

    void Awake()
    {
        controller = GetComponent<AIController>();
        aIConditions = GetComponent<AIConditions>();
        aIVariables = GetComponent<AIVariables>();
        aIFSMVariabls = GetComponent<AIFSMVariabls>();
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void InitInfoData(AIInfoList enemyInfoList)
    {
        AIInfoClip aiData = AIManager.Instance.GetAIInfoClip(enemyInfoList);
        aiName = aiData.aiNameEnum;
        aiCharacteristicNameUI = aiData.characteristicsDisplayName;
        aiNameUI = aiData.originDisplayName;
        aiType = aiData.aiType;
        level = aiData.level;
        healthBarCount = aiData.healthBarCount;
        health = aiData.health;
        currentHealth = health + extraHealth;
        atk = aiData.atk;
        evasion = aiData.evasion;
        atkMinPercent = aiData.minAtkPercent;
        criticalChance = aiData.criticalChance;
        criticalDmg = aiData.criticalDmg;
        ignoreDamageState = aiData.ignoreDamageState;
        useStanding = aiData.useStanding;
        standingPercent = aiData.standingPercent;
        standingType = aiData.standingType;
        standingAttackCount = aiData.standingAttackCount;
        increaseStandingAttackSpeed = aiData.increaseStandingAttackSpeed;
        immortality = aiData.immortality;
        recycleHpReset = aiData.recycleHpReset;
        recycleHpResetTime = aiData.recycleHpResetTime;
        useDefense = aiData.useDefense;
        defensePercent = aiData.defensePercent;
        defenseType = aiData.defenseType;
        defensePerCount = aiData.defenseCount;
        defensingTime = aiData.defensingTime;
        atkSpeed = aiData.atkSpeed;
        defense = aiData.defense;
        magicDefense = aiData.magicDefense;
        exp = aiData.exp;
        dropItems = aiData.dropItems;
        aIVariables.defenseCoolTime = aiData.defenseCoolTime;
        aIVariables.StandingCoolTime = aiData.standingCoolTime;
        UpdateStats();
    }

    public override void UpdateStats()
    {
        currentLevel = level + extraLevel;
        maxHealth = health + extraHealth;
        currentDefense = defense + extraDefense;
        currentAtk = atk + extraAtk;
        currentAtkSpeed = atkSpeed + extraAtkSpeed;
        currentMagicDefense = magicDefense + extraMagicDefense;
        currentExp = exp + extraExp;
        currentEvasion = evasion + extraEvasion;
        currentCriticalChance = criticalChance + extraCriChance;
        if (currentCriticalChance > 100) currentCriticalChance = 100;
        currentCriticalDmg = criticalDmg + extraCriDmg;
        controller.aIFSMVariabls.currentHpPercentage = ((float)controller.aiStatus.CurrentHealth / (float)controller.aiStatus.CurrentMaxHealth) * 100f;
        // Debug.Log("C P% : " + controller.aIFSMVariabls.currentHpPercentage);
        if (aiType == AIType.COMMON)
            onChangedHp?.Invoke(this);
    }

    public void ExcuteOnHPHUD()
    {
        onChangedHp?.Invoke(this);
    }

    public override bool IsFullHP()
    {
        if (currentHealth >= TotalHealth)
            return true;
        return false;
    }

    public override bool IsEvasion()
    {
        float value = Random.Range(0f, 100f);
        if (value <= CurrentEvasion)
            return true;
        return false;
    }


    public override void SetCurrentHP(int value)
    {
        currentHealth = value;
        UpdateStats();
    }

    public override void AddCurrentHealth(float value)
    {
        currentHealth += (int)value;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (!immortality)
            {
                aIConditions.IsDead = true;
                controller.myColl.enabled = false;
            }
        }
        UpdateStats();
        if ((aiType == AIType.BOSS || aiType == AIType.ELITE) && !controller.IsPlayableObject)
        {
            if (value >= 0)
            {
                CommonUIManager.Instance.ExcuteFloatingDamagedText((int)value, FloatingType.HEAL, controller.damagedPosition);
                AIManager.Instance.UpdateIncreaseAppearBossEvent(controller);
            }
            else if (value < 0)
                AIManager.Instance.UpdateReduceAppearBossEvent(controller);
        }

        if (recycleHpReset)
        {
            if (recycleHpReset_Co != null)
                StopCoroutine(recycleHpReset_Co);

            recycleHpReset_Co = RecycleHpReset_Co();
            StartCoroutine(recycleHpReset_Co);
        }

    }


    public void ResetRecycle_Co()
    {
        if (recycleHpReset_Co != null)
            StopCoroutine(recycleHpReset_Co);
        recycleHpReset_Co = null;
    }


    private IEnumerator RecycleHpReset_Co()
    {
        float currTimer = 0f;
        while(controller.gameObject.activeInHierarchy)
        {
            currTimer += Time.deltaTime;
            yield return null;

            if(currTimer >= recycleHpResetTime)
            {
                currTimer = 0f;
                if (currentHealth < TotalHealth)
                    SetCurrentHP(TotalHealth);
            }
        }
    }


    public override void Damaged(float damage, BaseController attacker, bool isCritical,bool isSkill , AttackStrengthType attackStrengthType, bool isForceDmg = false)
    {
        if (controller.IsDead()) return;
        if (!aIConditions.CanDamaged) return;
        if (aIConditions.CanDmgRegisterTarget && attacker != null) aIVariables.SetTarget(attacker);
        if(IsEvasion())
        {
            CommonUIManager.Instance.ExcuteFloatingDamagedText((int)damage,FloatingType.MISS, controller.damagedPosition);
            controller.HitEffect.ExcuteEvasionColor();
            return;
        }
        aIFSMVariabls.DamagedStrengthType = attackStrengthType;
        aIConditions.IsForcedDamage = isForceDmg;
        // aIConditions.CanDamaged = true;

        ExcuteDamageProcess(damage, isForceDmg);
        ExcuteDamagedEvents(damage, false, isSkill, isCritical);

        if (attacker != null)
        {
            if (aIConditions.IsDead && attacker is AIController) attacker.GetComponent<AIVariables>().target = null;
            if (aIConditions.IsDead && attacker is PlayerStateController) (attacker as PlayerStateController).playerStats.AddExp(exp);
        }

        if ((aiType == AIType.BOSS || aiType == AIType.ELITE) && !controller.IsPlayableObject)
            AIManager.Instance.UpdateReduceAppearBossEvent(controller);
        if (!aIConditions.IsDead && !aIConditions.IsDefensing) controller.HitEffect.ExcuteDamagedColor();

        UpdateStats();
        //if (aIConditions.CanDamaged) aIConditions.IsDamaged = true;
        //else aIConditions.IsDamaged = false;
        aIConditions.IsDamaged = true;

    }



    private bool ExcuteDefense()
    {
        if (!useDefense || aIConditions.IsSkilling || aIFSMVariabls.IsDefenseCoolTime || aIConditions.IsGroggying || aIConditions.IsStanding) return false;
        //if (aIConditions.IsDefensing && aIConditions.CanDefense) return true;

        if (CheckIsDefensing())
        {
            aIFSMVariabls.CurrentDefenseCount += 1;
            controller.aIFSMVariabls.isStartBlockDefense = true;
            controller.aiAnim.Play("ExcuteDefense", 3, 0f);
            SoundManager.Instance.PlayUISound(SoundList.None);
            controller.HitEffect.ExcuteDefenseColor();
            Debug.Log("디펜스 : " + aIFSMVariabls.CurrentDefenseCount);
            return true;
        }
        else
        {
            if (useDefense && !aIConditions.CanDefense && ComparePercent(defensePercent))
            {
                SoundManager.Instance.PlayUISound(SoundList.None);
                aIConditions.CanDefense = true;
                return true;
            }
        }

        return false;
    }

    private bool CheckCanStanding()
    {
        if (!useStanding || aIConditions.IsSkilling || aIFSMVariabls.IsStandingCoolTime || aIConditions.IsGroggying) return false;
        if (controller.aIVariables.target == null) return false;
        if (aIConditions.IsStanding && aIConditions.CanStanding)
        {
            //aIConditions.CanDamaged = false;
            return false;
        }

        targetDistance = (controller.aIVariables.target.transform.position - controller.transform.position).magnitude;
        if (targetDistance > controller.aIFSMVariabls.detectAttackRadius + 0.3f) 
            return false;

        if (useStanding && ComparePercent(standingPercent))
            return true;

        return false;
    }


    public void StandingCoolTime() => StartCoroutine(StandingCoolTime_Co());
    public void DefenseCoolTime() => StartCoroutine(DefenseCoolTime_Co());


    private IEnumerator StandingCoolTime_Co()
    {
        yield return new WaitForSeconds(aIVariables.StandingCoolTime);
        Debug.Log("쿨타임 초기화!");

        aIFSMVariabls.IsStandingCoolTime = false;
    }
    private IEnumerator DefenseCoolTime_Co()
    {
        yield return new WaitForSeconds(aIVariables.defenseCoolTime);

        aIFSMVariabls.IsDefenseCoolTime = false;

    }

    private void ExcuteDamageProcess(float damage, bool isForceDmg)
    {
        if (isForceDmg)
        {
            AddCurrentHealth(-damage);
            GroggyCumulative(damage);
            return;
        }

        if (!ExcuteDefense())
        {
            if (CheckCanStanding())
            {
                if (!aIConditions.CanStanding)
                {
                    //aIConditions.CanDamaged = false;
                    aIConditions.CanStanding = true;
                }
            }
            AddCurrentHealth(-damage);
            GroggyCumulative(damage);
        }
    }
                               
    private void ExcuteDamagedEvents(float damage,bool isEvasion, bool isSkill , bool isCritical)
    {
        if (damage > 0 )
            CommonUIManager.Instance.ExcuteFloatingDamagedText((int)damage, GetFloatingType(isSkill,isEvasion, isCritical), controller.damagedPosition);

        if ((aiType == AIType.BOSS || aiType == AIType.ELITE) && !controller.IsPlayableObject)
            AIManager.Instance.SettingAppearBossEvent(controller);
    }

    private void GroggyCumulative(float damage)
    {
        if (controller.aiConditions.IsStanding || controller.aiConditions.IsDefensing || controller.aiConditions.IsSkilling) return;

        if (!controller.aiConditions.CanGroggy) controller.aIFSMVariabls.currentCumulativeGroggyDamage = 0f;
        else controller.aIFSMVariabls.currentCumulativeGroggyDamage += damage;
    }               

    private FloatingType GetFloatingType(bool isSkill,bool isEvasion, bool isCritical)
    {
        if (aIConditions.CanDefense || aIConditions.IsDefensing) return FloatingType.BLOCK;
        else if (isEvasion) return FloatingType.MISS;
        else if (isCritical) return FloatingType.CRITICAL;
        else if (isSkill) return FloatingType.SKILL;
        else return FloatingType.ATTACK;
    }

    private bool ComparePercent(float percent)
    {
        float retPercent = Random.Range(0f, 100f);
        if (retPercent <= percent)
            return true;

        return false;
    }


    private bool CheckIsDefensing()
    {
        if (aIConditions.IsDefensing && defenseType == AIDefenseType.COUNT )
        {
            if (aIFSMVariabls.CurrentDefenseCount < defensePerCount && aIFSMVariabls.CurrentDefenseTimer < defensingTime)
                return true;
        }
        else if(aIConditions.IsDefensing && defenseType == AIDefenseType.TIME)
        {
            if (aIFSMVariabls.CurrentDefenseTimer < defensingTime)
                return true;
        }

        return false;
    }

    public float GetDamage(bool isCritical)
    {
        float dmg = Random.Range(CurrentAtk * atkMinPercent, CurrentAtk);
        if (isCritical)
            dmg = dmg * (1 + currentCriticalDmg);
        return dmg;
    }

    public override float GetTotalHPValue()
    {
        UpdateStats();
        return TotalHealth;
    }

    public override float GetCurrentHPValue()
    {
        UpdateStats();
        return CurrentHealth;

    }
}
