using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIConditions : MonoBehaviour
{
    public CurrentCombatType currentCombatType = CurrentCombatType.NONE;

    [Header("Doing Bools")]
    [SerializeField] private bool damagedStanding = false;
    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isTargetInSight = false;
    [SerializeField] private bool isFeelAlert = false;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isSkilling = false;
    [SerializeField] private bool isEndAttacking = false;
    [SerializeField] private bool isEndSkilling = false;
    [SerializeField] private bool isDamageState = false;
    [SerializeField] private bool isGroggying = false;
    [SerializeField] private bool isResting = false;
    [SerializeField] private bool isWaitTime = false;
    [SerializeField] private bool isDamaged = false;
    [SerializeField] private bool isDown = false;
    [SerializeField] private bool isDefensing = false;
    [SerializeField] private bool isStanding = false;
    [SerializeField] private bool isForcedDamage = false;
    [SerializeField] private bool ignoreDetectCollider = false;
    [SerializeField] private bool isForceRunning = false;
    [SerializeField] private bool isInteract = false;
    [SerializeField] private bool isChangingState = false;

    [Header("Can Bools")]
    [SerializeField] private bool canResetPosition = true;
    [SerializeField] private bool canState = true;
    [SerializeField] private bool canAttacking = true;
    [SerializeField] private bool canMeleeAttack = false;
    [SerializeField] private bool canRangeAttack = false;
    [SerializeField] private bool canDash = false;
    [SerializeField] private bool canDamaged = true;
    [SerializeField] private bool canGroggy = false;
    [SerializeField] private bool canRest = true;
    [SerializeField] private bool canDropItem = true;
    [SerializeField] private bool canDefense = false;
    [SerializeField] private bool canStanding = false;
    [SerializeField] private bool canDetect = true;
    [SerializeField] private bool canDmgRegisterTarget = true;



    public bool detectedOn = false;

    #region Getter Setter Variables
    public bool DamagedStanding { get { return damagedStanding; } set { damagedStanding = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }
    public bool IsTargetInSight { get { return isTargetInSight; } set { isTargetInSight = value; } }
    public bool IsFeelAlert { get { return isFeelAlert; } set { isFeelAlert = value; } }
    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
    public bool IsSkilling { get { return isSkilling; } set { isSkilling = value; } }
    public bool IsDamageState { get { return isDamageState; } set { isDamageState = value; } }
    public bool IsEndAttacking { get { return isEndAttacking; } set { isEndAttacking = value; } }
    public bool IsEndSkilling { get { return isEndSkilling; } set { isEndSkilling = value; } }
    public bool IsGroggying { get { return isGroggying; } set { isGroggying = value; } }
    public bool IsResting { get { return isResting; } set { isResting = value; } }
    public bool IsWaitTime { get { return isWaitTime; } set { isWaitTime = value; } }
    public bool IsDamaged { get { return isDamaged; } set { isDamaged = value; } }
    public bool IsDown { get { return isDown; } set { isDown = value; } }
    public bool IsDefensing { get { return isDefensing; } set { isDefensing = value; } }
    public bool IsStanding { get { return isStanding; } set { isStanding = value; } }
    public bool IsForcedDamage { get { return isForcedDamage; } set { isForcedDamage = value; } }
    public bool IgnoreDetectCollider { get { return ignoreDetectCollider; } set { ignoreDetectCollider = value; } }
    public bool IsForceRunning { get { return isForceRunning; } set { isForceRunning = value; } }
    public bool IsInteract { get { return isInteract; } set { isInteract = value; } }
    public bool IsChangingState { get { return isChangingState; } set { isChangingState = value; } }


    public bool CanResetPosition { get { return canResetPosition; } set { canResetPosition = value; } }
    public bool CanState { get { return canState; } set { canState = value; } }
    public bool CanAttacking { get { return canAttacking; } set { canAttacking = value; } }
    public bool CanDash { get { return canDash; } set { canDash = value; } }
    public bool CanGroggy { get { return canGroggy; } set { canGroggy = value; } }
    public bool CanDamaged { get { return canDamaged; } set { canDamaged = value; } }
    public bool CanRest { get { return canRest; } set { canRest = value; } }
    public bool CanDropItem { get { return canDropItem; } set { canDropItem = value; } }
    public bool CanDefense { get { return canDefense; } set { canDefense = value; } }
    public bool CanStanding { get { return canStanding; } set { canStanding = value; } }
    public bool CanDetect { get { return canDetect; } set { canDetect = value; } }
    public bool CanDmgRegisterTarget { get { return canDmgRegisterTarget; } set { canDmgRegisterTarget = value; } }

    #endregion


    public void ResetCondition()
    {
        isChangingState = false;
        isDead = false;
        isTargetInSight = false;
        isFeelAlert = false;
        isAttacking = false;
        isSkilling = false;
        isEndAttacking = false;
        isEndSkilling = false;
        isGroggying = false;
        isResting = false;
        isWaitTime = false;
        isDamaged = false;
        ignoreDetectCollider = false;
        isForceRunning = false;

       canAttacking = true;
        canMeleeAttack = false;
        canRangeAttack = false;
        canDash = false;
        canGroggy = true;
        canRest = true;
        canDropItem = true;
        canDefense = false;
        canStanding = false;
        canDetect = true;
        isInteract = false;
        canDmgRegisterTarget = true;
    }


    public void ResetDefenseBool()
    {
        isDefensing = false;
        canDefense = false;
    }

    public void ResetStandingBool()
    {
        isStanding = false;
        canStanding = false;
    }
}

