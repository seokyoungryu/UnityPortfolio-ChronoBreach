using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// 포션 사용 여기서 적용하기. / 무기 등 아이템도 / 버프도 / 디버프도 
public class PlayerStatus : BaseStatus, IDamageable 
{
    [SerializeField] private EffectInfo levelUpEffect;

    [SerializeField] private float rollSpCost = 10f;
    #region  Origin Stats
    //플레이어 원본 스탯은 저장됨. 
    [Header("Player Origin Stats Settings")]        
    [SerializeField] private ExpData expData = null;
    [SerializeField] private ExpContainer nextExp = null;
    [SerializeField] private long currentExp = 0;
    [SerializeField] private int remainingStatPoint = 0;
    [SerializeField] private int remainingSkillPoint = 0;
    [SerializeField] private int stamina = 100;
    [SerializeField] private int allStats = 0;
    [SerializeField] private int strength = 5;
    [SerializeField] private int dexterity = 5;
    [SerializeField] private int luck = 5;
    [SerializeField] private int intelligence = 5;
    [SerializeField] private float hpRegenerationPerSecond = 0f;
    [SerializeField] private float staminaRegenationPerSecond = 0f;
    [SerializeField] private float skillIncreaseDamagePercentage = 0f;
    [SerializeField] private float hitPerHp = 0f;
    [SerializeField] private float healthRegenTime = 1.5f;
    [SerializeField] private float staminaRegenTime = 1.5f;

    [SerializeField] private float originWalkSpeed = 3f;
    [SerializeField] private float originSprintSpeed = 6f;
    [SerializeField] private float originLimitBreakSpeed = 8f;
    #endregion

    #region Extra Stats
    [Header("Extra Stats")]
    [SerializeField] private int extraStamina = 0;
    [SerializeField] private int extraAllStats = 0;
    [SerializeField] private int extraStrength = 0;
    [SerializeField] private int extraDexterity = 0;
    [SerializeField] private int extraLuck = 0;
    [SerializeField] private int extraIntelligence = 0;
    [SerializeField] private float extraHpRegenPerSec = 0f;
    [SerializeField] private float extraStaminaRegenPerSec = 0f;
   [SerializeField] private float extraSkillIncreaseDmgPercentage = 0f;
    [SerializeField] private float extraHitPerHp = 0f;
    #endregion

    #region Percentage
    [Header("Percentage [ ex) 1 == 1% ]")]
    [SerializeField] private float maxHpPercentage = 0f;
    [SerializeField] private float allStatsPercentage = 0f;
    [SerializeField] private float strPercentage = 0f;
    [SerializeField] private float luckPercentage = 0f;
    [SerializeField] private float dexPercentage = 0f;
    [SerializeField] private float intPercentage = 0f;
    [SerializeField] private float damagePercentage = 0f; // 최종 데미지 줄떄 1000 입혀야되면 1000 * damagePercentage
    [SerializeField] private float atkPercentage = 0f;
    [SerializeField] private float expPercentage = 0f;
    #endregion

    #region Temp Stats
    private int tempRemainingStatPoint = 0;
    private int tempStr = 0;
    private int tempDex = 0;
    private int tempLuc = 0;
    private int tempInt = 0;

    public int TempRemainingStatPoint { get { return tempRemainingStatPoint; } set { tempRemainingStatPoint = value; } }
    public int TempSTR => tempStr;
    public int TempDex => tempDex;
    public int TempLuc => tempLuc;
    public int TempInt => tempInt;
    #endregion

    #region EditStat
    [SerializeField] private int editHealth = 0;
    [SerializeField] private int editStamina = 0;
    [SerializeField] private float editEvasion = 0;
    [SerializeField] private int editAtk = 0;
    [SerializeField] private float editAtkSpeed = 0;
    [SerializeField] private int editDefense = 0;
    [SerializeField] private int editMagicDefense = 0;
    [SerializeField] private float editCriChance = 0;
    [SerializeField] private float editCriDmg = 0;
    [SerializeField] private float editAllMoveSpeed = 0;
    [SerializeField] private float editWalkSpeed = 0;
    [SerializeField] private float editSprintSpeed = 0;
    [SerializeField] private float editLimitBreakSpeed = 0;



    public int EditHealth { get { return editHealth; } set { editHealth = value; } }
    public int EditStamina { get { return editStamina; } set { editStamina = value; } }
    public float EditEvasion { get { return editEvasion; } set { editEvasion = value; } }
    public int EditAtk { get { return editAtk; } set { editAtk = value; } }
    public float EditAtkSpeed { get { return editAtkSpeed; } set { editAtkSpeed = value; } }
    public int EditDefense { get { return editDefense; } set { editDefense = value; } }
    public int EditMagicDefense { get { return editMagicDefense; } set { editMagicDefense = value; } }
    public float EditCriChance { get { return editCriChance; } set { editCriChance = value; } }
    public float EditCriDmg { get { return editCriDmg; } set { editCriDmg = value; } }
    public float EditAllMoveSpeed { get { return editAllMoveSpeed; } set { editAllMoveSpeed = value; } }
    public float EditWalkSpeed { get { return editWalkSpeed; } set { editWalkSpeed = value; } }
    public float EditSprintSpeed { get { return editSprintSpeed; } set { editSprintSpeed = value; } }
    public float EditLimitBreakSpeed { get { return editLimitBreakSpeed; } set { editLimitBreakSpeed = value; } }

    #endregion

    #region Current Stats
    [Header("Current Stats")]
    [SerializeField] private int maxStamina = 0;
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private int currentStamina = 0;
    [SerializeField] private int currentAllStats = 0;
    [SerializeField] private int currentStrength = 0;
    [SerializeField] private int currentDexterity = 0;
    [SerializeField] private int currentLuck = 0;
    [SerializeField] private int currentIntelligence = 0;
    [SerializeField] private float currentHpRegenPerSec = 0f;
    [SerializeField] private float currentStaminaRegenPerSec = 0f;
 
    [SerializeField] private float currentSkillIncreaseDmgPercentage = 0f;
    [SerializeField] private float currentHitPerHp = 0f;
    [SerializeField] private float currentDamage = 0f;
    #endregion

    private bool isRegenHp = false;
    private bool isRegenStamina = false;

    #region Getter Setter Variables
    //Origin Stats
    public float RollSpCost => rollSpCost;
    public int Level { get { return level; } set { level = value; } }
    public long CurrentExp { get { return currentExp; } set { currentExp = value; } }
    public ExpData ExpData { get { return expData; }}

    public ExpContainer NextExp { get { return nextExp; } set { nextExp = value; } }
    public int OriginStamina { get { return stamina; } set { stamina =  value; } }
    public int OriginAllStats { get { return allStats; } set { allStats = value; } }
    public int OriginStrength { get { return strength; } set { strength =  value; } }
    public int OriginDexterity { get { return dexterity; }  set { dexterity =  value; } }
    public int OriginLuck { get { return luck; } set { luck =  value;} }
    public int OriginIntelligence { get { return intelligence; }  set { intelligence =  value; } }
    public float OriginHpRegenPerSec { get { return hpRegenerationPerSecond; }  set { hpRegenerationPerSecond =  value;} }
    public float OriginStaminaRegenPerSec { get { return staminaRegenationPerSecond; }  set { staminaRegenationPerSecond =  value; } }
   public float OriginSkillIncreaseDmgPercentage { get { return skillIncreaseDamagePercentage; }  set { skillIncreaseDamagePercentage =  value; } }
    public float OriginHitPerHp { get { return hitPerHp; }  set { hitPerHp =  value; } }
    public int RemainingSkillPoint { get { return remainingSkillPoint; } set { remainingSkillPoint = value; } }
    public int RemainingStatPoint { get { return remainingStatPoint; } set { remainingStatPoint = value; } }

    public float HealthRegenTime { get { return healthRegenTime; } set { healthRegenTime = value; } }
    public float StaminaRegenTime { get { return staminaRegenTime; } set { staminaRegenTime = value; } }

    public float OriginWalkSpeed { get { return originWalkSpeed; } set { originWalkSpeed = value; } }
    public float OriginSprintSpeed { get { return originSprintSpeed; } set { originSprintSpeed = value; } }
    public float OriginLimitBreakSpeed { get { return originLimitBreakSpeed; } set { originLimitBreakSpeed = value; } }

    //Extra Stats

    public int ExtraStamina { get {return extraStamina; }  set { extraStamina =  value;} }
    public int ExtraAllStats { get { return extraAllStats; } set { extraAllStats = value; } }
    public int ExtraStrength { get {return  extraStrength;}  set { extraStrength =  value;} }
    public int ExtraDexterity { get {return extraDexterity ;}  set { extraDexterity =  value;} }
    public int ExtraLuck { get {return extraLuck; }  set { extraLuck =  value;} }
    public int ExtraIntelligence { get {return extraIntelligence; }  set { extraIntelligence =  value;} }
    public float ExtraHpRegenPerSec { get {return extraHpRegenPerSec; }  set { extraHpRegenPerSec =  value;} }
    public float ExtraStaminaRegenPerSec { get {return extraStaminaRegenPerSec; }  set { extraStaminaRegenPerSec =  value;} }
    public float ExtraSkillIncreaseDmgPercentage { get {return extraSkillIncreaseDmgPercentage; }  set { extraSkillIncreaseDmgPercentage =  value; } }
    public float ExtraHitPerHp { get {return extraHitPerHp; }  set { extraHitPerHp =  value; } }


    // Stats Percentage
    public float MaxHpPercentage { get { return maxHpPercentage; } set { maxHpPercentage = value; } }
    public float AllStatsPercentage { get { return allStatsPercentage; }  set { allStatsPercentage =  value; }}
    public float StrPercentage { get { return strPercentage; }  set { strPercentage =  value; }}
    public float LuckPercentage { get { return luckPercentage; }  set { luckPercentage =  value; }}
    public float DexPercentage { get { return dexPercentage; } set { dexPercentage =  value; } }
    public float IntPercentage { get { return intPercentage; }  set { intPercentage =  value; }}
    public float DamagePercentage { get { return damagePercentage; } set { damagePercentage =  value; } }
    public float AtkPercentage { get { return atkPercentage; }  set { atkPercentage =  value; }}
    public float ExpPercentage { get { return expPercentage; } set { expPercentage = value; } }

    //Current Stats
    public int MaxStamina { get { return maxStamina; } }
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    public int CurrentStamina { get { return currentStamina; } set { currentStamina = value; } }
    public int CurrentAllStats { get { return currentAllStats ; } }
    public int CurrentStrength { get { return currentStrength; } }
    public int CurrentDexterity { get { return currentDexterity; } }
    public int CurrentLuck { get { return currentLuck; } }
    public int CurrentIntelligence { get { return currentIntelligence; } }
    public float CurrentHpRegenPerSec { get { return currentHpRegenPerSec; } }
    public float CurrentStaminaRegenPerSec { get { return currentStaminaRegenPerSec; } }
    public float CurrentSkillIncreaseDmgPercentage { get { return currentSkillIncreaseDmgPercentage; } }
    public float CurrentHitPerHp { get { return currentHitPerHp; } }

    //Total Stats
    public int TotalHealth { get { return (health + extraHealth + editHealth + (TotalDexterity * 4)) + (int)((health + extraHealth + editHealth + (TotalDexterity * 4)) * (MaxHpPercentage == 0 ? 0f : (MaxHpPercentage / 100)) ) ; } }
    public int TotalStamina { get { return stamina + extraStamina + editStamina; } }
    public float TotalAtk { get { return (atk + extraAtk + editAtk ) + (int)((atk + extraAtk + editAtk) * (atkPercentage == 0 ? 0f : (atkPercentage / 100))); } }
    public int TotalAllStats { get { return (allStats + extraAllStats) + (int)((allStats + extraAllStats) * (allStatsPercentage == 0 ? 0f : (allStatsPercentage / 100))); } }
    public int TotalStrength { get { return (strength + extraStrength + TotalAllStats ) + (int)((strength + extraStrength + TotalAllStats ) *  (strPercentage == 0 ? 0f :  (strPercentage/100))); } }
    public int TotalDexterity { get { return (dexterity + extraDexterity + TotalAllStats) + (int)((dexterity + extraDexterity + TotalAllStats ) * (dexPercentage == 0 ? 0f : (dexPercentage / 100)));} }
    public int TotalLuck { get { return (luck + extraLuck + TotalAllStats) + (int)((luck + extraLuck + TotalAllStats) * (luckPercentage == 0 ? 0f : (luckPercentage / 100))); } }
    public int TotalIntelligence { get { return (intelligence + extraIntelligence + TotalAllStats ) + (int)((intelligence + extraIntelligence + TotalAllStats ) * (intPercentage == 0 ? 0f : (intPercentage / 100))); } }

    public float TotalHpRegenPerSec { get { return (hpRegenerationPerSecond + extraHpRegenPerSec) <= 0 ? 0 : hpRegenerationPerSecond + extraHpRegenPerSec; } }
    public float TotalStaminaRegenPerSec { get { return (staminaRegenationPerSecond + extraStaminaRegenPerSec) <= 0 ? 0 : staminaRegenationPerSecond + extraStaminaRegenPerSec; } }
    public float TotalAtkSpeed { get { return atkSpeed + extraAtkSpeed + editAtkSpeed; } }
    public float TotalCriChance { get { return criticalChance + extraCriChance +editCriChance + (TotalLuck * 0.06f); } }
    public float TotalCriDmg { get { return criticalDmg + extraCriDmg + editCriDmg; } }
    public float TotalDefense { get { return defense + extraDefense + TotalDexterity + editDefense; } }
    public float TotalMagicDefense{ get { return magicDefense + extraMagicDefense + editMagicDefense+ (TotalLuck * 0.7f); } }
    public float TotalEvasion { get { return evasion + extraEvasion+editEvasion + (TotalLuck * 0.04f); } }
    public float TotalSkillIncreaseDmgPercentage { get { return skillIncreaseDamagePercentage + extraSkillIncreaseDmgPercentage; } }
    public float TotalHitPerHp { get { return hitPerHp + extraHitPerHp; } }

    #endregion

    private IEnumerator regenStamina_Co;
    private IEnumerator regenHealth_Co;

    private PlayerStateController controller;

    #region Evenets
    public delegate void OnHpChanged(PlayerStatus playerStatus);
    public delegate void OnSpChanged(PlayerStatus playerStatus);
    public delegate void OnUpdateStatInformations(PlayerStatus playerStatus);
    public delegate void OnInit(PlayerStatus playerStatus);
    public delegate void OnExpUpdate();
    public delegate void OnDead();

    private event OnInit onInit;
    public event OnInit OnInit_
    {
        add
        {
            if (onInit == null || !onInit.GetInvocationList().Contains(value))
                onInit += value;
        }
        remove
        {
            onInit -= value;
        }
    }
    private event OnUpdateStatInformations onUpdateFunctionUIs;
    public event OnUpdateStatInformations OnUpdateFunctionUIs_
    {
        add
        {
            if (onUpdateFunctionUIs == null || !onUpdateFunctionUIs.GetInvocationList().Contains(value))
                onUpdateFunctionUIs += value;
        }
        remove
        {
            onUpdateFunctionUIs -= value;
        }
    }
    private event OnUpdateStatInformations onUpdateStatInfos;
    public event OnUpdateStatInformations OnUpdateStatInfos_
    {
        add
        {
            if (onUpdateStatInfos == null || !onUpdateStatInfos.GetInvocationList().Contains(value))
                onUpdateStatInfos += value;
        }
        remove
        {
            onUpdateStatInfos -= value;
        }
    }
    private event OnUpdateStatInformations onExpUpdate;
    public event OnUpdateStatInformations OnExpUpdate_
    {
        add
        {
            if (onExpUpdate == null || !onExpUpdate.GetInvocationList().Contains(value))
                onExpUpdate += value;
        }
        remove
        {
            onExpUpdate -= value;
        }
    }
    private event OnUpdateStatInformations onLevelUpInit;
    public event OnUpdateStatInformations OnLevelUpInit_
    {
        add
        {
            if (onLevelUpInit == null || !onLevelUpInit.GetInvocationList().Contains(value))
                onLevelUpInit += value;
        }
        remove
        {
            onLevelUpInit -= value;
        }
    }

    private event OnHpChanged onHpChanged;
    public event OnHpChanged OnHpChanged_
    {
        add
        {
            if (onHpChanged == null || !onHpChanged.GetInvocationList().Contains(value))
                onHpChanged += value;
        }
        remove
        {
            onHpChanged -= value;
        }
    }
    private event OnSpChanged onSpChanged;
    public event OnSpChanged OnSpChanged_
    {
        add
        {
            if (onSpChanged == null || !onSpChanged.GetInvocationList().Contains(value))
                onSpChanged += value;
        }
        remove
        {
            onSpChanged -= value;
        }
    }
    private event OnDead onDead;
    public event OnDead OnDead_
    {
        add
        {
            if (onDead == null || !onDead.GetInvocationList().Contains(value))
                onDead += value;
        }
        remove
        {
            onDead -= value;
        }
    }
    #endregion

    public void Awake()
    {
        controller = GetComponent<PlayerStateController>();
        UpdateStats();
        SetCurrentHP(TotalHealth);
        SetCurrentStamina(TotalStamina);

        nextExp = expData.GetExpContainer(level + 1);
        onInit?.Invoke(this);
        onUpdateFunctionUIs?.Invoke(this);
        onUpdateStatInfos?.Invoke(this);
        regenStamina_Co = RegenerateStamina();
        regenHealth_Co = RegenerateHealth();
    }

    void OnEnable()
    {
        UpdateStats();
        SetCurrentHP(TotalHealth);
        SetCurrentStamina(TotalStamina);

        onInit?.Invoke(this);
        onUpdateFunctionUIs?.Invoke(this);
        onUpdateStatInfos?.Invoke(this);
        regenStamina_Co = RegenerateStamina();
        regenHealth_Co = RegenerateHealth();
    }

    public void Resurrection()
    {
        SetCurrentHP(TotalHealth);
        SetCurrentStamina(TotalStamina);
        UpdateStats();
    }

    [ContextMenu("TT")]
    public void Test()
    {
        UpdateStats();
        Debug.Log("TotalAllStats : " + TotalAllStats);
        Debug.Log("TotalStrength : " + TotalStrength);
        Debug.Log("strength : " + strength);
        Debug.Log("extraStrength : " + extraStrength);
        Debug.Log("(strength + extraStrength + TotalAllStats) : " + (strength + extraStrength + TotalAllStats));
        Debug.Log("(strPercentage == 0 ? 1f : (strPercentage / 100)) : " + (strPercentage == 0 ? 1f : (strPercentage / 100)));

    }



    public float GetCalculateDefense(float dmg, bool isSkill)
    {
        if (dmg <= 0) return 0f;

        if (isSkill)
        {
            dmg -= TotalMagicDefense;
            if (dmg <= 0)
                return 1f;
        }
        else
        {
            dmg -= TotalDefense;
            if (dmg <= 0)
                return 1f;
        }

        return dmg;
    }

    public override void Damaged(float damage, BaseController attacker, bool isCritical, bool isSkill, AttackStrengthType attackStrengthType, bool isForceDmg = false)
    {
        if (!controller.Conditions.CanGetDamage || controller.Conditions.IgnoreDamaged) return;
        if (IsEvasion())
        {
            SoundManager.Instance.PlayExtraSound(SoundList.Cast_02);
            return;
        }

        UseCurrentHealth(GetCalculateDefense(damage, isSkill));
        controller.Conditions.ResetSuccessCounterCount();
        controller.Attacker = attacker;
        if(currentHealth <= 0 && !controller.Conditions.IsImmortal)
        {
            onDead?.Invoke();
            StopAllCoroutines();
            controller.ChangeState(controller.deadStateHash);
            if (attacker is AIController)
                attacker.GetComponent<AIVariables>().target = null;
            return;
        }

        if (!controller.Conditions.IsSkilling)
        {
            controller.Conditions.IsDamaged = true;
            controller.ChangeState(controller.damagedStateHash, (int)attackStrengthType);
        }
    }

    public override void UpdateStats()
    {
        currentStrength = TotalStrength + tempStr;
        currentDexterity = TotalDexterity + tempDex;
        currentLuck = TotalLuck + tempLuc;
        currentIntelligence = TotalIntelligence + tempInt;
        currentHpRegenPerSec = Mathf.Round((hpRegenerationPerSecond + extraHpRegenPerSec) * 100f) / 100f;
        currentStaminaRegenPerSec = Mathf.Round((staminaRegenationPerSecond + extraStaminaRegenPerSec) * 100f) / 100f;
        currentAtk = (int)TotalAtk;
        currentAtkSpeed = Mathf.Round((TotalAtkSpeed) * 100f) / 100f;
        currentCriticalChance = TotalCriChance;
        currentCriticalDmg = TotalCriDmg;
        currentSkillIncreaseDmgPercentage = TotalSkillIncreaseDmgPercentage;
        currentHitPerHp = TotalHitPerHp;
        currentDamage = GetDamage();
        currentMagicDefense = TotalMagicDefense;
        currentDefense = TotalDefense;
        currentEvasion = TotalEvasion;
        maxHealth = TotalHealth;
        maxStamina = TotalStamina;

        onHpChanged?.Invoke(this);
        onSpChanged?.Invoke(this);
        onUpdateStatInfos?.Invoke(this);
        onExpUpdate?.Invoke(this);
        GameManager.Instance.UpdateSkillInfo();
        ResetCheckRegens();
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

        if (value <= TotalEvasion)
            return true;
        return false;
    }

    public void AllResetEditStats()
    {
        ResetEditMaxHealth();
        ResetEditMaxStamina();
        editEvasion = 0;
        editAtk = 0;
        editAtkSpeed = 0;
        editDefense = 0;
        editMagicDefense = 0;
        editCriChance = 0;
        editCriDmg = 0;
        editAllMoveSpeed = 0;
        editWalkSpeed = 0;
        editSprintSpeed = 0;
        editLimitBreakSpeed = 0;
        UpdateStats();
    }

    public void AddEditMaxHealth(int value)
    {
        editHealth += value;
        UpdateStats();
        SetCurrentHP(CurrentHealth + value);
    }
    public void RemoveEditMaxHealth(int value)
    {
        editHealth -= value;
        UpdateStats();
        SetCurrentHP(CurrentHealth - value);
    }
    public void ResetEditMaxHealth()
    {
        float retHp = CurrentHealth - editHealth;
        editHealth = 0;
        SetCurrentHP((int)retHp);
        UpdateStats();
    }
    public void AddEditMaxStamina(int value)
    {
        editStamina += value;
        UpdateStats();
        SetCurrentStamina(CurrentStamina + value);
    }
    public void RemoveEditMaxStamina(int value)
    {
        editStamina -= value;
        UpdateStats();
        SetCurrentStamina(CurrentStamina - value);
    }
    public void ResetEditMaxStamina()
    {
        float retSta = CurrentStamina - editStamina;
        editStamina = 0;
        SetCurrentStamina((int)retSta);
        UpdateStats();
    }


    public void SetFullHP()
    {
        currentHealth = TotalHealth;
        UpdateStats();
    }
    public override void SetCurrentHP(int value)
    {
        currentHealth = value;
        UpdateStats();
    }
    public void SetCurrentStamina(int value)
    {
        currentStamina = value;
        UpdateStats();
    }
    public override void AddCurrentHealth(float value)
    {
        currentHealth += (int)value;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        else if (currentHealth <= 0)
            currentHealth = 0;
        UpdateStats();
    }

    public void UseCurrentHealth(float value)
    {
        currentHealth -= (int)value;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
        else if (currentHealth <= 0)
            currentHealth = 0;
        UpdateStats();
        StopCoroutine(regenHealth_Co);
        regenHealth_Co = RegenerateHealth();
        StartCoroutine(regenHealth_Co);
    }


    public void AddCurrentStamina(int value)
    {
        currentStamina += (int)value;

        if (currentStamina >= maxStamina)
            currentStamina = maxStamina;
        else if (currentStamina <= 0)
            currentStamina = 0;
        UpdateStats();
      
    }

    public void UseCurrentStamina(float value)
    {
        isRegenStamina = false;
        currentStamina -= (int)value;

        if (currentStamina >= maxStamina)
            currentStamina = maxStamina;
        else if (currentStamina <= 0)
            currentStamina = 0;
        UpdateStats();
        StopCoroutine(regenStamina_Co);
        regenStamina_Co = RegenerateStamina();
        StartCoroutine(regenStamina_Co);
    }

    private void ResetCheckRegens()
    {
        if (regenHealth_Co != null)
            StopCoroutine(regenHealth_Co);
        regenHealth_Co = RegenerateHealth();
        StartCoroutine(regenHealth_Co);

        if (regenStamina_Co != null)
            StopCoroutine(regenStamina_Co);
        regenStamina_Co = RegenerateStamina();
        StartCoroutine(regenStamina_Co);
    }

    private IEnumerator RegenerateHealth()
    {
        if (TotalHpRegenPerSec <= 0 || controller.IsDead()) yield break;

        yield return new WaitForSeconds(healthRegenTime);
        isRegenHp = true;

        while (isRegenHp && currentHealth < TotalHealth )
        {
            currentHealth += (int)TotalHpRegenPerSec;
            if (currentHealth >= TotalHealth) currentHealth = TotalHealth;
            onHpChanged?.Invoke(this);
            yield return new WaitForSeconds(3f);
        }
        isRegenHp = false;
    }

    private IEnumerator RegenerateStamina()
    {
        if (TotalStaminaRegenPerSec <= 0 || controller.IsDead()) yield break;

        yield return new WaitForSeconds(staminaRegenTime);
        isRegenStamina = true;

        while (isRegenStamina && currentStamina < TotalStamina)
        {
            currentStamina += (int)TotalStaminaRegenPerSec;
            if (currentStamina >= TotalStamina) currentStamina = TotalStamina;
            onSpChanged?.Invoke(this);
            yield return new WaitForSeconds(0.2f);
        }
        isRegenStamina = false;
    }



    public void AddLevel(int value)
    {
        int beforeLv = level;
        int currLb = level + value;
        level += value;
        LevelUpUI ui = EffectManager.Instance.GetEffectObject(EffectList.LevelUpEffectUI_Image, Vector3.zero, Vector3.zero, Vector3.one * 1.3f, true).GetComponent<LevelUpUI>();
        ui.SettingText(beforeLv, currLb);
        CommonUIManager.Instance.LevelUpUI.RegisterUI(ui);
        EffectManager.Instance.GetEffectObjectInfo(levelUpEffect, Vector3.zero, Vector3.zero, Vector3.zero);


        AddStatsPoint(5 * value);
        AddSkillPoint(5 * value);
        nextExp = expData.GetExpContainer(currLb);
        onLevelUpInit?.Invoke(this);
        onUpdateStatInfos?.Invoke(this);
        UpdateStats();
        //레벨업 사운드, 레벨업 이펙트 주기.
        onExpUpdate?.Invoke(this);
    }

    public void AddExp(double value, RewardExp reward = null)
    {
        double resultExp = value + (value * expPercentage);

        if (currentExp + resultExp >= nextExp.RequiredExp)
        {
            double exceptExp = nextExp.RequiredExp - currentExp;
            double remainingExp = resultExp - exceptExp;
            currentExp = 0;
            AddLevel(1);
            if (remainingExp > 0)
                AddExp(remainingExp);
        }
        else
            currentExp += (int)resultExp;

        if (reward != null && value > 0)
            CommonUIManager.Instance.ExcuteItemGainNotifier(reward);
        else if (reward == null && value > 0)
        {
            RewardExp tempReward = new RewardExp();
            tempReward.Icon = GameManager.Instance.StandardExp.Icon;
            tempReward.ExpValue = (int)value;
            tempReward.RewardName = $"경험치 +{value}";
            tempReward.Description = $"경험치 {value} 획득";
        }

        UpdateStats();
        onExpUpdate?.Invoke(this);
    }

    public void RemoveExp(int value)
    {
        if (currentExp + value <= 0)
        {
            currentExp = 0;
            return;
        }
        currentExp -= value;
        UpdateStats();
        onExpUpdate?.Invoke(this);
    }

    public void AddStatsPoint(int value, RewardStatPoint reward = null)
    {
        remainingStatPoint += value;
        if (remainingStatPoint < 0) remainingSkillPoint = 0;
        if (reward != null && value > 0)
            CommonUIManager.Instance.ExcuteItemGainNotifier(reward);
        else if (reward == null && value > 0)
        {
            RewardStatPoint tempReward = new RewardStatPoint();
            tempReward.Icon = GameManager.Instance.StandardStatpoint.Icon;
            tempReward.StatPoint = (int)value;
            tempReward.RewardName = $"스탯포인트 +{value}";
            tempReward.Description = $"스탯포인트 {value} 획득";
        }
        onUpdateFunctionUIs?.Invoke(this);
        UpdateStats();
    }

    public void AddSkillPoint(int value, RewardSkillPoint reward = null)
    {
        remainingSkillPoint += value;
        if (remainingSkillPoint < 0) remainingSkillPoint = 0;
        if (reward != null && value > 0)
            CommonUIManager.Instance.ExcuteItemGainNotifier(reward);
        else if (reward == null && value > 0)
        {
            RewardSkillPoint tempReward = new RewardSkillPoint();
            tempReward.Icon = GameManager.Instance.StandardSkillpoint.Icon;
            tempReward.SkillPoint = (int)value;
            tempReward.RewardName = $"스킬포인트 +{value}";
            tempReward.Description = $"스킬포인트 {value} 획득";
        }

        UpdateStats();
    }

    public void UseSkillPoint(int value) => AddSkillPoint(-value);
    public void UseStatsPoint(int value) =>AddStatsPoint(-value);


    public void AddTempStr(int value) => tempStr+= value;
    public void AddTempDex(int value) => tempDex += value;
    public void AddTempLuc(int value) => tempLuc += value;
    public void AddTempInt(int value) => tempInt += value;


    public int GetTempStatsCount()
    {
        return tempStr + tempDex + tempLuc + tempInt;
    }

    public void ApplyTempStats()
    {
        tempRemainingStatPoint = remainingStatPoint;
        strength += tempStr;
        dexterity += tempDex;
        luck += tempLuc;
        intelligence += tempInt;

        ResetTempStats();
        UpdateStats();
    }

    public void ResetTempStats()
    {
        remainingStatPoint = tempRemainingStatPoint;
        tempStr = 0;
        tempDex = 0;
        tempLuc = 0;
        tempInt = 0;
    }


    public float GetDamage() => Random.Range(GetMinDamage(false), GetMaxDamage(false));

    public float GetCriticalDamage()
    {
        float criticalDamagePercent = ReturnIf0To1(currentCriticalDmg);
        return GetDamage() * (1 + criticalDamagePercent);
    }

    public float GetMaxDamage(bool includeTempStat)
    {
        float damagePercent = ReturnIf0To1(damagePercentage);
        float totalSTR = includeTempStat ? TotalStrength + tempStr : TotalStrength;
        int totalTempStat = includeTempStat ? tempDex + tempLuc + tempInt : 0;

        float retDamage = ((totalSTR * 0.8f) + (TotalDexterity + TotalLuck + TotalIntelligence + totalTempStat) * 0.8f) * (TotalAtk * 0.7f) * damagePercent;
        return retDamage;
    }

    public float GetMinDamage(bool includeTempStat)
    {
        return GetMaxDamage(includeTempStat) * 0.6f;
    }

    public float GetSkillDamage() => Random.Range(GetMinSkillDamage(false), GetMaxSkillDamage(false));

    public float GetMaxSkillDamage(bool includeTempStat)
    {
        float TotalSkillIncreaseDmgPercent = ReturnIf0To1(TotalSkillIncreaseDmgPercentage);
        float totalINT = includeTempStat ? TotalIntelligence + tempInt : TotalIntelligence;
        int totalTempStat = includeTempStat ? tempDex + tempLuc + tempStr : 0;

        float retDamage = ((totalINT * 1.5f) + (TotalDexterity + TotalLuck + TotalStrength + totalTempStat) * 0.8f) * ((TotalAtk * 0.75f)* TotalSkillIncreaseDmgPercent);
        return retDamage;
    }

    public float GetMinSkillDamage(bool includeTempStat)
    {
        return GetMaxSkillDamage(includeTempStat) * 0.6f;
    }

    public float GetCriticalSkillDamage()
    {
        float criticalDamagePercent = ReturnIf0To1(currentCriticalDmg);
        float retDamage = GetSkillDamage() * (1 + criticalDamagePercent);
        return retDamage;
    }

    private float ReturnIf0To1(float value)
    {
        return value == 0 ? 1 : value;
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
