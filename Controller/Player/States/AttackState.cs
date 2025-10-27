using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class AttackState : GenealState
{
    [Header("Reference")]
    private ComboController comboController = null;
    private ComboData currentCombo = null;
    private List<int> inputs = new List<int>();
    private Collider[] detectEnemyColliders = new Collider[10];
    private List<AIController> canDamageEnemy = new List<AIController>();

    [Header("Attack Variables")]
    public float attackRange = 5f;
    public Collider nearCollider = null;
    private const int LEFTINPUT = 0;
    private const int RIGHTINPUT = 1;
    private float attackTimeLimit = 0f;
    private float[] attackTiming;
    private bool isAttacking = false;
    private bool canInputCombo = false;
    private bool hasInput = false;
    private float waitTimeSum = 0f;
    private int attackIndex = 0;
    private bool isAttack = false;

    public bool isinput = false;
    public float limitInputTime = 0.5f;
    public float inputTimer = 0f;

    [Header("Coroutine")]
    private IEnumerator attackProcessCoroutine;
    private IEnumerator attackEndCheckCoroutine;

    [Header("Animator Hash")]
    private int attackSpeedFloatHash = 0;

    public int AttackIndex { get { return attackIndex; } set { attackIndex = value; } }
    public List<AIController> CanDamageEnemy { get { return canDamageEnemy; }}

    public enum AttackInputType
    {
        NONE = -1,
        LEFT = 0,
        RIGHT = 1,
    }

    protected override void Awake()
    {
        base.Awake();
        controller.AddState(this, ref controller.attackStateHash, this.hashCode);
        comboController = GetComponentInParent<ComboController>();
        attackSpeedFloatHash = Animator.StringToHash(AnimatorKey.AttackSpeed);

    }

    public override void Enter(PlayerStateController stateController, int enumType = -1)
    {
        StopAllCoroutines();
        attackProcessCoroutine = MultiAttackingProcess(currentCombo);
        attackEndCheckCoroutine = AttackEndCheck(currentCombo);
        canDamageEnemy.Clear();
        currentCombo = null;
        attackTimeLimit = 0f;
        attackTiming = null;
        attackIndex = 0;
        isAttack = false;
        canInputCombo = false;
        hasInput = false;
        isinput = false;
        inputs.Clear();
        inputs.Add(enumType);
        InitComboSetting(stateController.comboController.GetComboData(inputs));
        Attack(currentCombo);
    }

    public override void UpdateAction(PlayerStateController stateController)
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (stateController.IsMove() == false && stateController.Conditions.CanChangeCountAttackState())
                stateController.ChangeState(stateController.counterAttackStateHash);
            else if(stateController.IsMove() == true && stateController.Conditions.CanRoll)
            {
                if (controller.playerStats.CurrentStamina < controller.playerStats.RollSpCost)
                {
                    CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("스태미나가 부족합니다.");
                }
                else
                    stateController.ChangeState(controller.rollStateHash);

            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (stateController.Conditions.CanDash)
                stateController.ChangeState(stateController.dashStateHash);
        }


        if (canInputCombo && !hasInput)
        {
            if (Input.GetMouseButtonDown(0))  // L
            {
                inputs.Add(LEFTINPUT);
                if (stateController.comboController.FindHaveCombo(inputs))
                {
                    hasInput = true;
                    InitComboSetting(stateController.comboController.GetComboData(inputs));
                    Attack(currentCombo);
                }
                else
                    inputs.RemoveAt(inputs.Count - 1);

            }
            if (Input.GetMouseButtonDown(1)) // R
            {
                inputs.Add(RIGHTINPUT);
                if (stateController.comboController.FindHaveCombo(inputs))
                {
                    hasInput = true;
                    InitComboSetting(stateController.comboController.GetComboData(inputs));
                    Attack(currentCombo);
                }
                else
                    inputs.RemoveAt(inputs.Count - 1);
            }
        }

        if(nearCollider != null)
            Debug.DrawLine(stateController.transform.position, nearCollider.transform.position, Color.blue);

    }

    public override void Exit(PlayerStateController stateController)
    {
        StopAllCoroutines();
        currentCombo = null;

        isinput = false;
        inputTimer = 0f;
        if (isAttack)
            stateController.myAnimator.CrossFade("Reset", 0.4f);

        isAttack = false;
    }


    private void Attack(ComboData comboData)
    {
        if (!UseStamina(comboData))
        {
            controller.ChangeState(controller.moveStateHash);
            return;
        }

        canInputCombo = false;
        hasInput = false;
        isAttack = true;
        controller.myAnimator.SetFloat(attackSpeedFloatHash, controller.playerStats.CurrentAtkSpeed);
        attackTiming = SetMultiAttackTimings(comboData.comboClip.attackTimingFrame, comboData.comboClip.animationClip, controller.playerStats.CurrentAtkSpeed);
        CoroutineSetting(comboData);
        StartCoroutine(attackProcessCoroutine);
        StartCoroutine(attackEndCheckCoroutine);
    }

    private void CoroutineSetting(ComboData comboData)
    {
        StopCoroutine(attackProcessCoroutine);
        StopCoroutine(attackEndCheckCoroutine);

        attackProcessCoroutine = null;
        attackEndCheckCoroutine = null;

        attackProcessCoroutine = MultiAttackingProcess(comboData);
        attackEndCheckCoroutine = AttackEndCheck(comboData);
    }

    private IEnumerator AttackEndCheck(ComboData comboData)
    {
        float atkSpeed = controller.playerStats.TotalAtkSpeed;
        float canInputTime = comboData.comboClip.GetCanComboInputTime(atkSpeed);
        float endTime = comboData.comboClip.endForWaitTime + comboData.comboClip.GetAttactEndAnimationFrameToTime(atkSpeed);

        yield return new WaitForSeconds(canInputTime);
        canInputCombo = true;

        yield return new WaitForSeconds(endTime - canInputTime);
        controller.ChangeState(controller.moveStateHash);
    }

    private bool UseStamina(ComboData comboData)
    {
        if (controller.Conditions.InfinityStamina) return true;

        if (controller.playerStats.CurrentStamina < comboData.comboClip.staminaCost)
        {
            CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("스태미나가 부족합니다.");
            return false;
        }
        else
        {
            controller.playerStats.UseCurrentStamina((int)comboData.comboClip.staminaCost);
            return true;
        }

    }

    private IEnumerator WaitForTime(float maxtime)
    {
        float timer = 0;
        while (timer <= maxtime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }


    /// <summary>
    ///  입력 값이 없고 적이 공격 범위 안에  있으면 적쪽으로 회전, 없으면 입력 방향으로 회전.
    /// </summary>
    public void RotateAttackDirection(float range, ref Collider nearColl)
    {
        if(controller.IsMove())
            controller.RotateToDirection();
        else
        {
            FindNearEnemy(ref nearColl, detectEnemyColliders, controller.targetLayer, range);
            RotateToTarget(nearColl);
            if (nearColl == null)
                controller.RotateToDirection();

        }
    }


    /// <summary>
    /// 멀티 데미지 게산 코루틴 
    /// </summary>
    private IEnumerator MultiAttackingProcess(ComboData comboData)
    {
        RotateAttackDirection(attackRange,ref nearCollider);
        controller.myAnimator.CrossFade(comboData.comboClip.animationName, 0.1f);
        SpawnAttackEffects(comboData.comboClip, controller.playerStats.CurrentAtkSpeed);

        waitTimeSum = 0;
        for (int i = 0; i < comboData.comboClip.attackTimingFrame.Length; i++)
        {
            waitTimeSum += attackTiming[i];
            attackIndex = i;
            yield return new WaitForSeconds(attackTiming[i]);
            FindCanAttackEnemy(comboData.comboClip, ref detectEnemyColliders, controller.targetLayer);
            RandomEffectInfo effect = comboData.comboClip.hitEffectList.Length > i ? comboData.comboClip.hitEffectList[i] : null;
            DamageEnemy(effect, comboData.comboClip.hitEffectList[i].effectSound,comboData.comboClip.attackStrengthType[i], comboData.comboClip.attackShakeCam[i], i, comboData.comboClip.damage[i], comboData.comboClip.GetTimeData);
        }

        yield return new WaitForSeconds(attackTimeLimit - waitTimeSum);
    }


    public void SpawnAttackEffects(ComboClip clip, float animSpeed)
    {
        for (int i = 0; i < clip.DamageEffects.Length; i++)
        {
            float delayTime = clip.GetFrameToTime(animSpeed, clip.DamageEffects[i].effectFrame);
            StartCoroutine(controller.CreateAttackEffect(delayTime, clip.DamageEffects[i], controller));
        }
    }

 


    /// <summary>
    ///  공격 타이밍 계산.
    /// </summary>
    public float[] SetMultiAttackTimings(int[] timingFrames,AnimationClip animClip,float timingSpeed = 1)
   {
        float[] tempAttackTiming = new float[timingFrames.Length];
   
       float atkSpeed = timingSpeed;
       float framerate = 1f / (animClip.frameRate * atkSpeed);
   
       if (timingFrames.Length > 0)
       {
           float beforeFrameTime = 0f;
           for (int i = 0; i < timingFrames.Length; i++)
           {
               if (i >= 1)
               {
                   tempAttackTiming[i] = (timingFrames[i] * framerate) - beforeFrameTime;
                   beforeFrameTime = timingFrames[i] * framerate;
               }
               else
               {
                   tempAttackTiming[i] = timingFrames[i] * framerate;
                   beforeFrameTime = tempAttackTiming[i];
               }
           }
       }
        return tempAttackTiming;
   }


    public bool FindCanAttackEnemy(ComboClip comboClip, ref Collider[] detectColliders, LayerMask detectLayer)
     => FindEnemy(comboClip.attackAngle, comboClip.attackRange, comboClip.maxTargetCount, ref detectEnemyColliders, controller.targetLayer);

    public bool FindCanAttackEnemy(float attackAngle, float attackRange, int maxTargetCount, ref Collider[] detectColliders, LayerMask detectLayer)
   => FindEnemy(attackAngle, attackRange, maxTargetCount, ref detectColliders, detectLayer);


    public float GetExistValue(float[] values, int index)
    {
        if (values.Length <= 0) return 0;

        float retValue = (values.Length > index && values[index] != 0) ? values[index] : values[0];
        return retValue;
    }

    /// <summary>
    /// 공격 가능한 Enemy를 찾아서 canDamageEnemy 리스트에 추가.
    /// </summary>
    private bool FindEnemy(float attackAngle, float attackRange, int maxTargetCount, ref Collider[] detectColliders, LayerMask detectLayer)
    {
        ResetFindEnemysCollider(detectColliders);
        canDamageEnemy.Clear();
        int enemysCount = Physics.OverlapSphereNonAlloc(controller.transform.position, attackRange, detectColliders, detectLayer);
        int attackCount = 0;

        controller.SortFindEmenyByNearDistance(controller.transform, ref detectColliders);
        if (enemysCount > 0)
        {
            for (int i = 0; i < enemysCount; i++)
            {
                if (attackCount >= maxTargetCount) break;
                Vector3 dirEnemy = detectColliders[i].transform.position - controller.transform.position;
                dirEnemy.y = 0;
                dirEnemy.Normalize();

                if (Mathf.Abs(Vector3.Angle(controller.myTrans.forward, dirEnemy)) <= attackAngle / 2f)
                {
                    if (Vector3.Distance(controller.myTrans.position, detectColliders[i].transform.position) <= attackRange)
                    {
                        attackCount++;
                        canDamageEnemy.Add(detectColliders[i].GetComponent<AIController>());
                    }
                }
            }
        }

        return false;
    }

  

    /// <summary>
    /// canDamageEnemy에 추가된 enemy를 데미지.
    /// </summary>
    public void DamageEnemy(RandomEffectInfo effectList,SoundList hitSound,AttackStrengthType attackStrengthType,CameraShakeInfo shakeInfo ,int index, float dmg,TimeData timeData ,AttackSkillClip attackSkillClip = null)
    {

        for (int i = 0; i < canDamageEnemy.Count; i++)
        {
            if (canDamageEnemy[i] != null && canDamageEnemy[i].aiConditions.IsDead) continue;
            if (controller.IsDetectObstacle(controller.damagedPosition, canDamageEnemy[i].GetComponent<BaseController>().damagedPosition))
                continue;
            if (timeData != null)
                TimeManager.Instance.ExcuteTimeData(timeData);
            else
            {
                if (attackSkillClip == null) TimeManager.Instance.ExcuteBaseTimeData(TimeInfoType.ATTACK);
                else                         TimeManager.Instance.ExcuteBaseTimeData(TimeInfoType.SKILL);
            }
            (bool, float) dmgValue = controller.GetDamageValue(attackSkillClip != null ? true : false);
            canDamageEnemy[i]?.Damaged(dmg * dmgValue.Item2, controller, dmgValue.Item1, attackSkillClip != null ? true : false, attackStrengthType);
            SoundManager.Instance.PlayEffect(hitSound);
            GameManager.Instance.Cam.ShakeCamera(shakeInfo);
            if (effectList != null)
                CreateHitEffect(i, effectList, attackSkillClip, canDamageEnemy[i].GetComponent<BaseController>().damagedPosition);
        }
    }

   
    public void FindNearEnemy(ref Collider nearEnemyCollider, Collider[] detectEnemtColliders, LayerMask detectlayer, float range)
    {
        nearEnemyCollider = null;
        ResetFindEnemysCollider(detectEnemtColliders);
        int enemys = Physics.OverlapSphereNonAlloc(controller.transform.position, range, detectEnemtColliders, detectlayer);
        float nearDistance = Mathf.Infinity;

        if (enemys > 0)
        {
            for (int i = 0; i < enemys; i++)
            {
                if (detectEnemtColliders[i] == null || detectEnemtColliders[i].GetComponent<BaseController>() == null)
                    continue;
                if (controller.IsDetectObstacle(controller.damagedPosition, detectEnemtColliders[i].GetComponent<BaseController>().damagedPosition))
                    continue;

                float distance = Vector3.Distance(controller.transform.position, detectEnemtColliders[i].transform.position);
                if (distance < nearDistance)
                {
                    nearDistance = distance;
                    nearEnemyCollider = detectEnemtColliders[i];
                }
            }
        }
    }

    private void CreateHitEffect(int index ,RandomEffectInfo info, AttackSkillClip skillClip, Transform target)
    {
        if (info != null)
            EffectManager.Instance.GetEffectObjectRandom(info, target.position, Vector3.zero, Vector3.zero);
        else if (skillClip != null && skillClip.HitEffects.Count > index)
        {
            if (skillClip.IsHitRandom[index])
                EffectManager.Instance.GetEffectObjectRandom(skillClip.HitEffects[index], target.position, Vector3.zero, Vector3.zero);
            else
                EffectManager.Instance.GetEffectObjectRandom(skillClip.HitEffects[index], target.position, Vector3.zero, Vector3.zero);
        }


    }

    public void RotateToTarget(Collider nearEnemyCollider)
    {
        if (nearEnemyCollider != null)
            Rotate(nearEnemyCollider.transform);
    }

    public void RotateToTarget(Transform enemyTransform) => Rotate(enemyTransform);


    private void Rotate(Transform targetTr)
    {
        Vector3 enemyDirection = targetTr.position - controller.transform.position;
        enemyDirection.y = 0f;
        enemyDirection.Normalize();

        Quaternion lookEnemy = Quaternion.LookRotation(enemyDirection);
        lookEnemy = Quaternion.Slerp(controller.transform.rotation, lookEnemy, 5f);
        controller.transform.rotation = lookEnemy;
    }


    private void ResetFindEnemysCollider(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
            colliders[i] = null;
    }

    private void InitComboSetting(ComboData comboData)
    {
        currentCombo = comboData;
        attackRange = currentCombo.comboClip.attackRange;
    }

   

    public void SetComboData(ComboData comboData) => currentCombo = comboData;

    public void SetComboClip(ComboClip comboclip) => currentCombo = new ComboData(comboclip);



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

 
}
