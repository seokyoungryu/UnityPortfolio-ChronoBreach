using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatus : MonoBehaviour, IDamageable
{                                
    [SerializeField] protected int level = 0;
    [SerializeField] protected int health = 0;                                        //Base
    [SerializeField] protected float atk = 0;                                         //Base
    [SerializeField] protected float atkSpeed = 0;                                       //Base
    [SerializeField] protected float criticalChance = 0;                                     //Base
    [SerializeField] protected float criticalDmg = 0;                                        //Base
    [SerializeField] protected float magicDefense = 0;                                       //Base  
    [SerializeField] protected float defense = 0;                  //Base
    [SerializeField] protected float evasion = 0f;

    [SerializeField] protected int extraHealth = 0;
    [SerializeField] protected float extraAtk = 0;
    [SerializeField] protected float extraAtkSpeed = 0f;
    [SerializeField] protected float extraCriChance = 0f;
    [SerializeField] protected float extraCriDmg = 0f;
    [SerializeField] protected float extraDefense = 0f;
    [SerializeField] protected float extraMagicDefense = 0f;
    [SerializeField] protected float extraEvasion = 0f;
    [SerializeField] protected float extraMoveSpeed = 1f;

    [SerializeField] protected int maxHealth = 0;
    [SerializeField] protected float currentAtk = 0;
    [SerializeField] protected float currentAtkSpeed = 0f;
    [SerializeField] protected float currentCriticalChance = 0f;
    [SerializeField] protected float currentCriticalDmg = 0f;
    [SerializeField] protected float currentDefense = 0f;
    [SerializeField] protected float currentMagicDefense = 0f;
    [SerializeField] protected float currentEvasion = 0f;
    [SerializeField] protected float currentMoveSpeed = 0f;


    #region Getter Setter
    #region Origin Stats
    public int OriginHealth { get { return health;  } set { health =  value;} }
    public float OriginAtk { get { return atk; } set { atk = value; } }
    public float OriginAtkSpeed { get { return atkSpeed; }  set { atkSpeed =  value; } }
    public float OriginCriticalChance { get { return criticalChance; }  set { criticalChance =  value; } }
    public float OriginCriticalDmg { get { return criticalDmg; } set { criticalDmg = value; } }
    public float OriginDefense { get { return defense; } set { defense = value; } }
    public float OriginMagicDefense { get { return magicDefense; } set { magicDefense = value; } }
    public float OriginEvasion { get { return evasion; } set { evasion = value; } }
    #endregion

    #region Extra Stats
   public int ExtraHealth { get { return extraHealth; }  set { extraHealth =  value;} }
   public float ExtraAtk { get { return extraAtk; } set { extraAtk = value; } }
   public float ExtraAtkSpeed { get {return  extraAtkSpeed;}  set { extraAtkSpeed =  value; } }
   public float ExtraCriticalChance { get {return extraCriChance; }  set { extraCriChance =  value;} }
   public float ExtraCriticalDmg { get { return extraCriDmg; } set { extraCriDmg = value; } }
   public float ExtraDefense { get { return defense; } set { defense = value; } }
   public float ExtraMagicDefense { get { return magicDefense; } set { magicDefense = value; } }
   public float ExtraEvasion { get { return extraEvasion; } set { extraEvasion = value; } }
    public float ExtraMoveSpeed { get { return extraMoveSpeed; } set { extraMoveSpeed = value; } }

    #endregion

    #region Current Stats           
    public int CurrentMaxHealth { get { return maxHealth; } }
    public float CurrentAtk { get { return currentAtk; } }
    public float CurrentAtkSpeed { get { return currentAtkSpeed; } }
    public float CurrentCriChance { get { return currentCriticalChance; } }
    public float CurrentCriDmg { get { return currentCriticalDmg; } }
    public float CurrentDefense { get { return currentDefense; } }
    public float CurrentMagicDefense { get { return currentMagicDefense; } }
    public float CurrentEvasion { get { return currentEvasion; } }
    public float CurrentMoveSpeed { get { return currentMoveSpeed; } set { currentMoveSpeed = value; } }

    #endregion
    #endregion

    public abstract bool IsFullHP();
    public abstract void SetCurrentHP(int value);
    public abstract void AddCurrentHealth(float value);
    public abstract bool IsEvasion();

    public abstract float GetTotalHPValue();
    public abstract float GetCurrentHPValue();


    public bool IsCritical(float criChance)
    {
        float getPercent = MathHelper.RandomPercentage0To100();
        if (getPercent <= criChance)
            return true;
        return false;
    }

    public virtual void UpdateStats() { }

    public virtual void Damaged(float damage, BaseController attacker, bool isCritical, bool isSkill, AttackStrengthType attackStrengthType, bool isForceDmg = false)
    {
    }
}





