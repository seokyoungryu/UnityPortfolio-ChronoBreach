using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CounterAttackType
{
    NONE = -1,
    MELEE = 0,
    RANGE = 1,
}

public class CounterAttackState : GenealState
{
    private const string meleeCounterAttackName = "MeleeCounterAttack";
    private const string rangeCounterAttackName = "RangeCounterAttack";


    [Header("Reference")]
    [SerializeField] private CounterSkillClip counterSkillClip = null;
    private AttackState attackState;

    [Header("Variables")]
    [SerializeField] private BaseController counterSuccessTargetAI = null;
    private CounterAttackType currentSuccessCounterType = CounterAttackType.NONE;
    public LayerMask detectEnemyLayer = TagAndLayerDefine.LayersIndex.Enemy;
    public Transform reflectShootTransform = null;
    [SerializeField] private RandomEffectInfo counterSuccessEffect;
    [SerializeField] private RandomEffectInfo counterParryEffect;
    [SerializeField] private Transform counterEffectTr;


    [Header("Colliders")]
    private Collider[] detectEnemyColliders = new Collider[10];

    [Header("Settings")]
    [SerializeField] private float counterComboResetTime = 3f;
    [SerializeField, Tooltip("Counter 성공 후 어택 안하면 moveState 이동까지 시간.")]
    private float successExitTime = 0.5f;
    [SerializeField, Tooltip("CounterAttack할 경우. 종료 후 공격 가능 시간")]
    private float waitCanAttackTime = 0f;
    [SerializeField, Tooltip("Counter 종료 후 다시 재진입 가능 시간")]
    private float delayAfterReEnterTime = 0f;

    [Header("DefenseParray")]
    [SerializeField, Tooltip("Denfense 시 Parrying Detect 유지 할 시간")]
    private float defenseDetectParryTime = 0.5f;
    [Header("AfterCounterAttack")]
    [SerializeField, Tooltip("CounterAttack 후 Parrying Detect시 유지 할 시간")]
    private float afterDetectTime = 0.5f;
    [SerializeField, Tooltip("CounterAttack 후 최대 Parrying 수")]
    private int afterMaxParryCount = 2;
    private float attackEndTime = 0f;
    private bool excuteParrying = false;    //CounterAttack 후 Parry Detect 실행.
    private int currentParryCount = 0;

    private float currentTimer = 0f;
    private bool isDefense = false;
    private bool canCounterAttack = false;
    private bool isCounterAttack = false;
    private bool isCounterSucess = false;
    private bool excuteFailReEnterCoolTime = false;
    private bool excuteSuccessComboResetTime = false;
    private bool excuteWaitEnterAttackCoolTime = false;
    private bool canReEnterState = false;
    private float currentReEnterTimer = 0f;
    private float currentComboResetTimer = 0f;
    private float currentWaitCanAttackTimer = 0f;
    private int successSoundCount;
    private float startTime;
    private bool canParry = false;
    private bool defenseSucess = false;

   [Header("Animation Frame Variables")]
    private float counterDetectOffTiming = 0f;
    private float counterAttackTiming = 0f;
    private float counterDefenseEndTiming = 0f;

    private IEnumerator defense_Co = null;
    private IEnumerator counterAttack_Co = null;


    public CounterSkillClip CounterSkillClip { get { return counterSkillClip; } set { counterSkillClip = value; } }
    public CounterAttackType CurrentSuccessCounterType { get { return currentSuccessCounterType; } set { currentSuccessCounterType = value; } }

    protected override void Awake()
    {
        base.Awake();
        attackState = GetComponent<AttackState>();
        controller.AddState(this, ref controller.counterAttackStateHash, hashCode);
    }

    private void Start()
    {
        attackState = controller.GetState<AttackState>();
        SettingClip();
    }

    public override void Enter(PlayerStateController stateController, int enumType = -1)
    {
        if (attackState == null)
            attackState = stateController.GetState<AttackState>();

        defenseSucess = false;
        isCounterSucess = false;
        EnterInitCounterSetting();
        counterAttack_Co = CounterAttackProcess();
        defense_Co = CounterDefenseProcess();
        StartCoroutine(defense_Co);
        attackState.AttackIndex = 0;
        attackState.SetComboClip(counterSkillClip.counterAttackClip);
    }

    public override void UpdateAction(PlayerStateController stateController)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"<color=green> 카운터 시도 isCounterAttack {isCounterAttack} canReEnterState {canReEnterState}</color>");
            if (isCounterAttack && canReEnterState)
            {
                Debug.Log($"<color=yellow> 카운터 재진입</color>");
                stateController.ChangeState(stateController.counterAttackStateHash, 0);
            }
            else if (defenseSucess)
                stateController.ChangeState(stateController.counterAttackStateHash, 0);
        }


        if (controller.IsMove() == true && Input.GetKeyDown(KeyCode.Space) && controller.Conditions.CanRoll)
        {
            if (controller.playerStats.CurrentStamina < controller.playerStats.RollSpCost)
            {
                CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("스태미나가 부족합니다.");
            }
            else
                stateController.ChangeState(controller.rollStateHash);
        }

        if (currentSuccessCounterType != CounterAttackType.NONE && isCounterSucess && canCounterAttack) //카운터 성공시.
        {
            currentTimer += Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && !isCounterAttack)   //카운터 성공시 마우스 클릭시 -> 카운터 어택.
            {
                currentTimer = 0f;
                if (counterAttack_Co != null)
                    StopCoroutine(counterAttack_Co);
                counterAttack_Co = CounterAttackProcess();
                StartCoroutine(counterAttack_Co);
            }
            else if (currentTimer >= successExitTime && !isCounterAttack && !canParry)  // 카운터 성공했는데 마우스 클릭안하면  exitTime 후 자동 moveState로.
            {
                stateController.ChangeState(stateController.moveStateHash);
            }
        }


    }

    public override void AlwaysCheckUpdate(PlayerStateController stateController)
    {
        if (excuteFailReEnterCoolTime && !defenseSucess && !isCounterAttack) CounterFailReEnterCoolTime();
        if (excuteWaitEnterAttackCoolTime && !defenseSucess) WaitAfterCounterAttacking();
        if (excuteSuccessComboResetTime) CheckSuccessComboReset();
    }


    public bool exitStop = false;

    public override void Exit(PlayerStateController stateController)
    {
        TimeManager.Instance.TimeReset();
        stateController.Conditions.CanGetDamage = true;
        ResetConditionTimer();

        if (isCounterAttack) excuteWaitEnterAttackCoolTime = true;                                              //카운터 어택시.
        if (!isCounterAttack) excuteFailReEnterCoolTime = true;        //카운터 실패시.
        else controller.Conditions.CanCounter = true;

        excuteSuccessComboResetTime = true;
        ExitInitCounterSetting();
        Debug.Log($"<color=black> 카운터 상태 EXIT </color> IsDetectParry {controller.Conditions.IsDetectParry}");
    }


    public void SettingClip()
    {
        PlayerSkillController skillController = controller.skillController;
        counterSkillClip = skillController.GetUseSkillClip<CounterSkillClip>();
        if (skillController != null && counterSkillClip == null)
        {
            CounterSkillClip tmpClip = skillController.OwnSkillDatabase.FindSkillType<CounterSkillClip>();
            counterSkillClip = skillController.GetOwnSkillClip<CounterSkillClip>(tmpClip.ID);
            counterSkillClip?.UseRequipedSkill(skillController);
        }
    }

    /// <summary>
    /// AI 카운터 성공 시 
    /// </summary>
    public void CounterSuccess(AIController aicontroller)
    {
       // Debug.Log("카운터 성공");
        currentSuccessCounterType = (aicontroller.aiAttackType == AIController.AIAttackType.MELEE)
                                     ? CounterAttackType.MELEE : CounterAttackType.RANGE;
        counterSuccessTargetAI = aicontroller;
        CommonCounterSuccess();
    }

    /// <summary>
    /// Base(Shoot Obstacle) 카운터 성공 시 
    /// </summary>
    public void CounterSuccess(BaseController baseController, CounterAttackType type)
    {
        //Debug.Log("카운터 성공");
        currentSuccessCounterType = type;
        counterSuccessTargetAI = baseController;
        CommonCounterSuccess();
    }


    private void CommonCounterSuccess()
    {
        successSoundCount = counterSkillClip.counterDefenseClip.successSound.Length;
        SoundManager.Instance.PlayExtraSound(counterSkillClip.counterDefenseClip.successSound[Random.Range(0, successSoundCount)]);

        if (excuteParrying && currentParryCount >= afterMaxParryCount)
        {
            controller.ChangeState(controller.damagedStateHash, 2);
            return;
        }

        controller.Conditions.AddSuccessCounterCount(1);

        if (counterSuccessTargetAI != null)  controller.RotateToTarget(counterSuccessTargetAI.transform.position);

        if (isDefense)
        {
            if (controller.Conditions.SuccessCounter % 2 == 0) controller.myAnimator.Play("CounterSuccess1");
            else                                               controller.myAnimator.Play("CounterSuccess2"); 
        }

        controller.playerStats.AddCurrentStamina(15);
        defenseSucess = true;

        if (!isCounterSucess)
        {
            GameManager.Instance.Cam.ShakeCamera(counterSkillClip.counterDefenseClip.SuccessDefenseShakeInfo);
            EffectManager.Instance.GetEffectObjectRandom(counterSuccessEffect, counterEffectTr.position, Vector3.zero, Vector3.zero);
            if (counterSkillClip.counterDefenseClip.GetTimeData != null)
                TimeManager.Instance.ExcuteTimeData(counterSkillClip.counterDefenseClip.GetTimeData);
            else
                TimeManager.Instance.ExcuteBaseTimeData(TimeInfoType.COUNTER_SUCCESS);
           // Debug.Log("<color=yellow> 카운터 성공 </color>");

        }
        else
        {
            if (!isCounterAttack)
                canParry = true;

            GameManager.Instance.Cam.ShakeCamera(counterSkillClip.counterDefenseClip.ParryingShakeInfo);
            EffectManager.Instance.GetEffectObjectRandom(counterParryEffect, counterEffectTr.position, Vector3.zero, Vector3.zero);
            TimeManager.Instance.ExcuteBaseTimeData(TimeInfoType.COUNTER_PARRYING);
           // Debug.Log("<color=blue> 패링 성공 </color>");
        }

        if (isCounterAttack && !controller.Conditions.IsCounting)
        {
            currentParryCount++;
            if (!excuteParrying)
                StartCoroutine(Parrying());
        }
         else
             StartCoroutine(WaitCounterAttack(0.1f));

        if (!isDefense) isDefense = true;
        isCounterSucess = true;

    }



    private IEnumerator CounterDefenseProcess()
    {
        SoundManager.Instance.PlayExtraSound(counterSkillClip.entrySound);
        controller.myAnimator.Play(counterSkillClip.counterDefenseClip.animationName, 2, 0f);

        yield return new WaitForSeconds(counterDetectOffTiming);

        if (!isCounterSucess)
        {
            controller.Conditions.IsCounting = false;
            yield return new WaitForSeconds(counterDefenseEndTiming - counterDetectOffTiming);
           // Debug.Log("<color=white> CounterDefenseProcess !isCounterSucess </color>");

            if (!isCounterAttack)
            {
                controller.ChangeState(controller.moveStateHash, 0);
            }
        }
        else if (isCounterSucess)
        {
            canParry = true;
            if (!isCounterAttack)
                controller.Conditions.IsDetectParry = true;

            yield return new WaitForSeconds(defenseDetectParryTime);
            if (!isCounterAttack)
            {
                controller.ChangeState(controller.moveStateHash, 0);
            }
        }

    }

    private IEnumerator CounterAttackProcess()
    {
        if (defense_Co != null) StopCoroutine(defense_Co);

        startTime = Time.time;
        canCounterAttack = false;
        controller.Conditions.IsCounting = true;
        controller.Conditions.IsDetectParry = false;
        isCounterAttack = true;
        defenseSucess = false;
        canParry = false;
        TimeManager.Instance.TimeReset();
        attackState.RotateToTarget(counterSuccessTargetAI.transform);

        //데미지 처리 
        if (currentSuccessCounterType == CounterAttackType.MELEE)
        {
            controller.myAnimator.Play(counterSkillClip.counterAttackClip.animationName);

            float endTiming = 0f;
            float[] timing = attackState.SetMultiAttackTimings(counterSkillClip.counterAttackClip.attackTimingFrame
                                                              , counterSkillClip.meleeAnimationClip
                                                              , counterSkillClip.meleeAnimSpeed);
            attackState.SpawnAttackEffects(counterSkillClip.counterAttackClip, counterSkillClip.meleeAnimSpeed);

            for (int i = 0; i < timing.Length; i++)
            {
                endTiming += timing[i];
                yield return new WaitForSeconds(timing[i]);
                attackState.FindCanAttackEnemy(counterSkillClip.counterAttackClip, ref detectEnemyColliders, detectEnemyLayer);

                RandomEffectInfo effect = counterSkillClip.counterAttackClip.hitEffectList.Length > i ? counterSkillClip.counterAttackClip.hitEffectList[i] : null;
                SoundManager.Instance.PlayEffect(counterSkillClip.counterAttackClip.hitEffectList[i].effectSound);
                attackState.DamageEnemy(effect,CounterSkillClip.counterAttackClip.hitEffectList[i].effectSound,
                                        counterSkillClip.counterAttackClip.attackStrengthType[i],
                                        counterSkillClip.counterAttackClip.attackShakeCam[i],
                                        i, counterSkillClip.counterAttackClip.damage[i],
                                        counterSkillClip.counterAttackClip.GetTimeData);
            }
            attackEndTime = counterSkillClip.counterAttackClip.GetAttactEndAnimationFrameToTime(counterSkillClip.meleeAnimSpeed) - endTiming;
        }
        else if (currentSuccessCounterType == CounterAttackType.RANGE)
        {
            controller.myAnimator.Play(counterSkillClip.counterRangeReflectClip.AnimName);
            float rangeTiming = counterSkillClip.GetFrameToTime(counterSkillClip.counterRangeReflectClip.ReflectTimingFrame
                                                                           , counterSkillClip.rangeAnimationClip.frameRate
                                                                           , counterSkillClip.rangeAnimSpeed);
            float endTime = counterSkillClip.GetFrameToTime(counterSkillClip.counterRangeReflectClip.EndAnimFrame
                                                                           , counterSkillClip.rangeAnimationClip.frameRate
                                                                           , counterSkillClip.rangeAnimSpeed);
            yield return new WaitForSeconds(rangeTiming);

            Debug.Log("<color=yellow> Counter Range ! </color>");
            counterSkillClip.counterRangeReflectClip.Info.ExcuteCreate(controller, counterSuccessTargetAI?.transform, this);
            SoundManager.Instance.PlayEffect(counterSkillClip.counterAttackClip.hitEffectList[0].effectSound);
            attackEndTime = endTime - rangeTiming;

        }
        canReEnterState = true;
        Debug.Log($"<color=green> 공격 재진입 허용! {canReEnterState} </color>");
        controller.Conditions.IsCounting = false;
        controller.Conditions.IsDetectParry = true;


        yield return new WaitUntil(() => (Time.time - startTime) >= attackEndTime || excuteParrying);
        Debug.Log("<color=green> 공격 종료! </color>");

        if (!excuteParrying)
        {
            Debug.Log("종료1");
            controller.ChangeState(controller.moveStateHash,0);
        }
    }

    private IEnumerator Parrying()
    {
        excuteParrying = true;
        canParry = true; 
        controller.Conditions.IsDetectParry = true;

        yield return new WaitForSeconds(afterDetectTime);

        controller.Conditions.IsDetectParry = false;
        Debug.Log("종료2");
        controller.ChangeState(controller.moveStateHash, 0);

    }

    private void WaitAfterCounterAttacking()
    {
        controller.Conditions.CanAttack = false;
        currentWaitCanAttackTimer += Time.deltaTime;
        if (currentWaitCanAttackTimer >= waitCanAttackTime)
        {
            excuteWaitEnterAttackCoolTime = false;
            controller.Conditions.CanAttack = true;
        }
    }


    private void CounterFailReEnterCoolTime()
    {
        Debug.Log($"<color=red> 재엔터 시작.. </color> ");

        controller.Conditions.CanCounter = false;
        currentReEnterTimer += Time.deltaTime;
        if (currentReEnterTimer >= delayAfterReEnterTime)
        {
            excuteFailReEnterCoolTime = false;
            controller.Conditions.CanCounter = true;
        }
    }


    private void CheckSuccessComboReset()
    {
        currentComboResetTimer += Time.deltaTime;
        if (currentComboResetTimer >= counterComboResetTime)
        {
            excuteSuccessComboResetTime = false;
            controller.Conditions.ResetSuccessCounterCount();
        }
    }

    IEnumerator WaitCounterAttack(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        canCounterAttack = true;
    }

    private void EnterInitCounterSetting()
    {
        controller.myAnimator.SetFloat("MeleeCounterAttackSpeed", counterSkillClip.meleeAnimSpeed);
        controller.myAnimator.SetFloat("RangeCounterAttackSpeed", counterSkillClip.rangeAnimSpeed);
        controller.Conditions.IsCounting = true;
        counterDefenseEndTiming = counterSkillClip.counterDefenseClip.GetEndAnimationFrameToTime();
        counterDetectOffTiming = counterSkillClip.counterDefenseClip.GetDetectOffFrameToTime();
        ResetConditionTimer();
        controller.Conditions.CanAttack = true;
        controller.Conditions.CanCounter = true;
        canParry = false;
        CommonInitSetting();
    }

    private void ExitInitCounterSetting()
    {
        controller.Conditions.IsCounting = false;
        controller.Conditions.IsDetectParry = false;
        counterDefenseEndTiming = 0f;
        counterDetectOffTiming = 0f;
        controller.Conditions.IsDamaged = false;
        attackState.SetComboData(null);
        controller.Conditions.CanGetDamage = true;
        canParry = false;
        CommonInitSetting();
    }

    private void CommonInitSetting()
    {
        currentParryCount = 0;
        counterSuccessTargetAI = null;
        counterAttackTiming = 0f;
        currentSuccessCounterType = CounterAttackType.NONE;
        isCounterAttack = false;
        currentTimer = 0f;
        canReEnterState = false;
        excuteParrying = false;
        canCounterAttack = false;
        isDefense = false;
        attackEndTime = 0f;
        controller.Conditions.IsDetectParry = false;

        if (defense_Co != null) StopCoroutine(defense_Co);
        defense_Co = null;
        if (counterAttack_Co != null) StopCoroutine(counterAttack_Co);
        counterAttack_Co = null;

    }


    private void ResetConditionTimer()
    {
        excuteFailReEnterCoolTime = false;
        excuteSuccessComboResetTime = false;
        excuteWaitEnterAttackCoolTime = false;

        currentReEnterTimer = 0f;
        currentComboResetTimer = 0f;
        currentWaitCanAttackTimer = 0f;
    }


}
