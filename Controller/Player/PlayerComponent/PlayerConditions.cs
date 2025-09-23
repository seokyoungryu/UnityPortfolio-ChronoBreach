using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerConditions : MonoBehaviour
{
    [Header("Player Conditions")]
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isSprint = false;
    [SerializeField] private bool isCantMove = false;

    [SerializeField] private bool isGround = false;
    [SerializeField] private bool isSlope = false;
    [SerializeField] private bool isLimitedSlope = false;

    [SerializeField] private bool isDown = false;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isDamaged = false;
    [SerializeField] private bool isSkilling = false;
    [SerializeField] private bool isLessHp = false;
    [SerializeField] private bool isRoll = false;

    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool canGetDamage = true;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canSkill = true;
    [SerializeField] private bool canCounter = true;
    [SerializeField] private bool canRoll = true;
    [SerializeField] private bool canApplyAnimator= true;
    [SerializeField] private bool isCounting = false;
    [SerializeField] private bool isDetectParry = false;
    [SerializeField] private bool canDetect = true;

    [Header("Edit")]
    [SerializeField] private bool ignoreDamaged = false;
    [SerializeField] private bool isImmortal = false;
    [SerializeField] private bool infinityStamina = false;
    [SerializeField] private bool skillNoCooltime = false;


    private bool isMoveStop = false;

    [Header("Variables")]
    [SerializeField] private int successDefenseCount = 0;
    [SerializeField] private int currentNeedDashCount = 0;


    #region Getter Setter Variables
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool CanSkill { get { return canSkill; } set { canSkill = value; } }
    public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
    public bool IsSprint { get { return isSprint; } set { isSprint = value; } }
    public bool IsCantMove { get { return isCantMove; } set { isCantMove = value; } }

    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    public bool IsCounting { get { return isCounting; } set { isCounting = value; } }
    public bool IsDown { get { return isDown; } set { isDown = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }
    public bool IsDamaged { get { return isDamaged; } set { isDamaged = value; } }
    public bool IsSkilling { get { return isSkilling; } set { isSkilling = value; } }
    public bool IsRoll { get { return isRoll; } set { isRoll = value; } }
    public bool CanCounter { get { return canCounter; } set { canCounter = value; } }
    public bool CanRoll { get { return canRoll; } set { canRoll = value; } }
    public bool CanGetDamage { get { return canGetDamage; } set { canGetDamage = value; } }
    public bool IsGround { get { return isGround; } set { isGround = value; } }
    public bool IsSlope { get { return isSlope; } set { isSlope = value; } }
    public bool IsLimitedSlope { get { return isLimitedSlope; } set { isLimitedSlope = value; } }
    public bool CanDash { get { return canDash; } set { canDash = value; } }
    public bool CanApplyAnimator { get { return canApplyAnimator; } set { canApplyAnimator = value; } }
    public bool IsDetectParry { get { return isDetectParry; } set { isDetectParry = value; } }
    public bool CanDetect { get { return canDetect; } set { canDetect = value; } }
    public bool IsMoveStop { get { return isMoveStop; } set { isMoveStop = value; } }

    public bool IgnoreDamaged { get { return ignoreDamaged; } set { ignoreDamaged = value; } }
    public bool IsImmortal { get { return isImmortal; } set { isImmortal = value; } }
    public bool InfinityStamina { get { return infinityStamina; } set { infinityStamina = value; } }
    public bool SkillNoCooltime { get { return skillNoCooltime; } set { skillNoCooltime = value; } }


    public int SuccessCounter => successDefenseCount;
    public int CurrentNeedDashCount { get { return currentNeedDashCount; } set { currentNeedDashCount = value; } }

    #endregion


    #region Events
    public delegate void OnSuccessUpdate(int count);

    private event OnSuccessUpdate onSuccessCounterUpdate;
    public event OnSuccessUpdate OnSuccessCounterUpdate_
    {
        add
        {
            if (onSuccessCounterUpdate == null || !onSuccessCounterUpdate.GetInvocationList().Contains(value))
                onSuccessCounterUpdate += value;
        }
        remove
        {
            onSuccessCounterUpdate -= value;
        }
    }
    private event OnSuccessUpdate onSuccessDashUpdate;
    public event OnSuccessUpdate OnSuccessDashUpdate_
    {
        add
        {
            if (onSuccessDashUpdate == null || !onSuccessDashUpdate.GetInvocationList().Contains(value))
                onSuccessDashUpdate += value;
        }
        remove
        {
            onSuccessDashUpdate -= value;
        }
    }
    #endregion


    public void AddSuccessCounterCount(int count)
    {
        successDefenseCount += count;
        onSuccessCounterUpdate?.Invoke(successDefenseCount);
        AddSuccessDashCount(1);
    }

    public void ResetSuccessCounterCount()
    {
        successDefenseCount = 0;
        onSuccessCounterUpdate?.Invoke(successDefenseCount);
    }


    public void AddSuccessDashCount(int count)
    {
        currentNeedDashCount += 1;
        onSuccessDashUpdate?.Invoke(currentNeedDashCount);
    }

    public void SuccessCountClear() => currentNeedDashCount = 0;

    public  bool CanDamaged()
    {
        if (!isDead && !isDown && CanGetDamage && !isCounting && !isDamaged && !isRoll)
            return true;
        else
            return false;
    }

    public bool CanChangeCountAttackState()
    {
        if (!isMoving && canCounter)
            return true;

        return false;
    }
    
    public void Resurrection()
    {
        isMoving = false;
        isSprint = false;
        isGround = false;
        isDown = false;
        isDead = false;
        isDamaged = false;
        isSkilling = false;
        isLessHp = false;
        isRoll = false;

        canGetDamage = true;
        canAttack = true;
        canSkill = true;
        canCounter = true;
        canRoll = true;
        isCounting = false;
        canMove = true;
        isDetectParry = false;
    }

    public void DeadSettings()
    {
        isDead = true;
        isMoving = false;
        isSprint = false;
        isDown = false;
        isDamaged = false;
        isLessHp = false;

        canGetDamage = false;
        canAttack = false;
        canSkill = false;
        isCounting = false;
        isDetectParry = false;

    }

    public void ResetConditions()
    {
        isMoving = false;
        isDown = false;
        isDetectParry = false;

    }


}
