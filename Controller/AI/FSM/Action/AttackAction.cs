using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/Attack")]
public class AttackAction : Action
{
   //스탠딩의 attack count일 경우 
   //AttackAction을 재 진입해서 완료시키기.
   //만역 finalAttack이 아니면 재진입, 다음 공격이 존재하면 다음공격 standing attack하다가 
   //standing count가 남으면 재진입해서 채우기.


    public override void OnEnterAction(AIController controller)
    {
        controller.aiAnim.Play("Reset");
        controller.nav.updatePosition = false;
        controller.nav.updateRotation = true;
        controller.aIFSMVariabls.meleeClip = null;
        controller.aIFSMVariabls.pervMeleeClip = null;
        controller.aIFSMVariabls.rangeClip = null;
        controller.aIFSMVariabls.isFinalClip = false;
        controller.aIFSMVariabls.attackComboCount = 0;

        controller.aiStatus.UpdateStats();
        SettingRandomStartAttack(controller);

        if (controller.aIFSMVariabls.meleeClip != null)
            SettingMeleeAttack(controller);
        else if(controller.aIFSMVariabls.rangeClip != null)
            SettingRangeAttack(controller, controller.aiStatus.CurrentAtkSpeed);

        controller.aiAnim.SetFloat(AnimatorKey.AttackSpeed, controller.aiStatus.CurrentAtkSpeed);

        controller.aiConditions.detectedOn = false;
        controller.aiAnim.applyRootMotion = true;
        controller.aiConditions.IsAttacking = false;
        controller.aiConditions.IsEndAttacking = false;
        controller.SetNavSpeed(0f);
        controller.nav.velocity = Vector3.zero;
        controller.myRigid.velocity = Vector3.zero;
      
    }

    bool isStart = false;
    float timer = 0f;
    public override void Act(AIController controller, float deltaTime)
    {
        if (isStart)
            timer += Time.deltaTime;

        if (controller.aiConditions.IsChangingState)
            return;
        if (!controller.aiConditions.CanAttacking || controller.aiConditions.IsEndAttacking || controller.aiAttackClips.Length <= 0) 
            return;

        if (controller.aiAttackType == AIController.AIAttackType.MELEE)
        {
            if (controller.aIFSMVariabls.isFinalClip == false && controller.aiConditions.IsAttacking == false)
                controller.StartCoroutine(MeleeAttackProcess(controller));
        }
        else if (controller.aiAttackType == AIController.AIAttackType.RANGE)
            if (controller.aiConditions.IsAttacking == false)
                controller.StartCoroutine(RangeAttackProcess(controller));
    }


    public override void OnExitAction(AIController controller)
    {
        controller.aiAnim.applyRootMotion = false;
        controller.nav.updatePosition = true;
        controller.nav.updateRotation = true;
        controller.nav.Warp(controller.transform.position);

        controller.aIFSMVariabls.meleeClip = null;
        controller.aiConditions.IsEndAttacking = false;
        controller.aiConditions.currentCombatType = CurrentCombatType.NONE;
        controller.aiConditions.IsAttacking = false;
        controller.aiConditions.detectedOn = false;
        controller.aIFSMVariabls.currentAttackRange = 0f;
        controller.StopAllCoroutines();
        //Debug.Log("공격 상태 OUT");
    }

    private IEnumerator MeleeAttackProcess(AIController controller)
    {
        //이부분 따로 뺴기.

        isStart = true;  //DE
        timer = 0f;     //DE
                        //Debug.Log("S : " + timer);
        controller.aiConditions.IsAttacking = true;
        controller.aiConditions.detectedOn = false;
        SettingMeleeAttackClip(controller);

        for (int i = 0; i < controller.aIFSMVariabls.meleeClip.attackEffect.Length; i++)
            CreateAttackEffect(controller, controller.aIFSMVariabls.meleeClip.attackEffect[i], controller);

        controller.aIFSMVariabls.attackWaitTime = 0f;
        controller.StartCoroutine(RotateToTarget(controller, FollowDirection.STATIC_DIRECTION));
        controller.aiAnim.SetFloat(AnimatorKey.AttackSpeed, controller.aiStatus.CurrentAtkSpeed * controller.aIFSMVariabls.meleeClip.baseAnimSpeed);
        controller.aiAnim.CrossFade(controller.aIFSMVariabls.meleeClip.animationClipName, 0.2f, 1);

        if(controller.aIFSMVariabls.meleeClip.randomAttackVoiceSound != null && controller.aIFSMVariabls.meleeClip.randomAttackVoiceSound.Length > 0)
        SoundManager.Instance.PlayEffect(controller.aIFSMVariabls.meleeClip.randomAttackVoiceSound);

        int currentTarget = 0;
        for (int i = 0; i < controller.aIFSMVariabls.meleeClip.attackTimingFrame.Length; i++)
        {
            controller.aIFSMVariabls.attackWaitTime += controller.aIFSMVariabls.meleeAttackTimingTime[i];
            controller.aIFSMVariabls.currentAttackRange = controller.aIFSMVariabls.attackRange[i];
            currentTarget = 0;

           yield return new WaitForSeconds(controller.aIFSMVariabls.meleeAttackTimingTime[i]);
            //Debug.Log("데미지! : "+ timer);
            if (FindCanDamagedToTarget(controller, false, i))
            {
                for (int x = 0; x < controller.aIFSMVariabls.canDamageEnemy.Count; x++)
                {
                    if (currentTarget >= controller.aIFSMVariabls.maxTargetCount) break;
                    if (controller.IsDetectObstacle(controller.transform, controller.aIFSMVariabls.canDamageEnemy[x].GetComponent<BaseController>()?.damagedPosition)) 
                        continue;

                    EffectManager.Instance.GetEffectObjectRandom(controller.aIFSMVariabls.meleeClip.hitEffect[i],
                                                           controller.aIFSMVariabls.canDamageEnemy[x].damagedPosition.position,
                                                           Vector3.zero, Vector3.zero);
                    SoundManager.Instance.PlayOneShot(controller.aIFSMVariabls.meleeClip.hitEffect[i].effectSound);
                    //SoundManager.Instance.PlaySoundAtPosition(controller.aIFSMVariabls.meleeClip.hitEffect[i].effectSound, controller.damagedPosition);
                    Damage(controller, controller.aIFSMVariabls.canDamageEnemy[x], false, x, controller.aIFSMVariabls.attackDamage[i], controller.aIFSMVariabls.meleeClip.attackStrengthType[i]);
                    currentTarget++;
                }
            }
        }

        //Debug.Log("M : " + timer);
        yield return new WaitForSeconds((controller.aIFSMVariabls.attackEndTime - controller.aIFSMVariabls.attackWaitTime) + controller.aIFSMVariabls.meleeClip.waitAttackEndTime);

        if (controller.aIFSMVariabls.meleeClip.isFinalClip || controller.aIFSMVariabls.meleeClip.nextAttackClip == null)
        {
            controller.aIFSMVariabls.isFinalClip = true;
            controller.aiConditions.IsEndAttacking = true;
        }
        else
        {
            controller.aIFSMVariabls.pervMeleeClip = controller.aIFSMVariabls.meleeClip;
            controller.aIFSMVariabls.meleeClip = controller.aIFSMVariabls.meleeClip.nextAttackClip;
            SettingMeleeAttack(controller);
        }


        controller.aIFSMVariabls.attackComboCount++;

        controller.aiConditions.IsAttacking = false;
        controller.aiConditions.detectedOn = false;
        //Debug.Log("E : " + timer);
        isStart = false;  //DE


    }

    private IEnumerator RangeAttackProcess(AIController controller)
    {
        controller.StartCoroutine(RotateToTarget(controller, controller.aIFSMVariabls.rangeClip.followDirectionType, controller.aIFSMVariabls.rangeAttackTimingTime));
        controller.aiConditions.IsAttacking = true;
        controller.aiAnim.SetFloat(AnimatorKey.AttackSpeed, controller.aiStatus.CurrentAtkSpeed * controller.aIFSMVariabls.rangeClip.animationSpeed);
        controller.aiAnim.CrossFade(controller.aIFSMVariabls.rangeClip.animationClipName, 0.15f);
        FollowDirection followType = controller.aIFSMVariabls.rangeClip.followDirectionType;
       
        yield return new WaitForSeconds(controller.aIFSMVariabls.rangeAttackTimingTime);

        controller.aIFSMVariabls.rangeClip.ProjectileCreator.ExcuteCreate(controller, controller.aIVariables.target?.transform, controller);

        yield return new WaitForSeconds(controller.aIFSMVariabls.attackEndTime - controller.aIFSMVariabls.rangeAttackTimingTime);


        yield return new WaitForSeconds(controller.aIFSMVariabls.rangeClip.waitAttackEndTime);
        //끝
        controller.aIFSMVariabls.attackComboCount++;
        controller.aiConditions.IsAttacking = false;
        controller.aiConditions.IsEndAttacking = true;
    }


    private IEnumerator RotateToTarget(AIController controller, FollowDirection followDirectionType, float followTimer = 0f)
    {
        if (controller.aIVariables.target == null) yield break;

        if (followDirectionType == FollowDirection.STATIC_DIRECTION)
            controller.RotateTarget(controller.aIVariables.Target.transform);
        else if (followDirectionType == FollowDirection.FOLLOW_DIRECTION)
        {
            float timer = 0f;
            while (timer < followTimer)
            {
                timer += Time.deltaTime;
                controller.RotateTarget(controller.aIVariables.Target.transform);
                yield return null;
            }
        }
    }


    private void CreateAttackEffect(AIController controller ,ControllerEffectInfo effectInfo, BaseController target)
    {
        float framerate = 1f / (30f * (controller.aiStatus.CurrentAtkSpeed * controller.aIFSMVariabls.meleeClip.baseAnimSpeed));
        controller.StartCoroutine(controller.CreateAttackEffect(effectInfo.effectFrame * framerate, effectInfo, target));
    }

    public void SettingRandomStartAttack(AIController controller)
    {
        int index = Random.Range(0, controller.aiAttackClips.Length);
        if (controller.aiAttackClips[index] is AIMeleeAttackClip)
        {
            controller.aIFSMVariabls.meleeClip = controller.aiAttackClips[index] as AIMeleeAttackClip;
            SettingMeleeAttack(controller);
        }
        else
            controller.aIFSMVariabls.rangeClip = controller.aiAttackClips[index] as AIRangeAttackClip;
    }

    private void SettingMeleeAttackClip(AIController controller)
    {
        float atkSpeed = controller.aiStatus.CurrentAtkSpeed * controller.aIFSMVariabls.meleeClip.baseAnimSpeed;
        float framerate = 1f / (controller.aIFSMVariabls.meleeClip.attackClip.frameRate * atkSpeed);
        controller.aIFSMVariabls.meleeAttackTimingTime = new float[controller.aIFSMVariabls.meleeClip.attackTimingFrame.Length];
        controller.aIFSMVariabls.meleeAttackTimingFrame = controller.aIFSMVariabls.meleeClip.GetTimingFrameToTime(atkSpeed);
        controller.aIFSMVariabls.attackEndTime = controller.aIFSMVariabls.meleeClip.attackEndAnimationFrame * framerate;

        if (controller.aIFSMVariabls.meleeAttackTimingFrame.Length <= 0) return;

        float beforeTime = 0f;
        for (int i = 0; i < controller.aIFSMVariabls.meleeAttackTimingFrame.Length; i++)
        {
            if (i == 0)
            {
                controller.aIFSMVariabls.meleeAttackTimingTime[i] = controller.aIFSMVariabls.meleeAttackTimingFrame[i];
                beforeTime = controller.aIFSMVariabls.meleeAttackTimingTime[i];
            }
            else
            {
                controller.aIFSMVariabls.meleeAttackTimingTime[i] = controller.aIFSMVariabls.meleeAttackTimingFrame[i] - beforeTime;
                beforeTime = controller.aIFSMVariabls.meleeAttackTimingFrame[i];
            }
        }
    }


    private void SettingRangeAttack(AIController controller, float atkSpeed)
    {
        AIRangeAttackClip rangeClip = controller.aIFSMVariabls.rangeClip;
        float frame = (rangeClip.attackClip.frameRate * (rangeClip.animationSpeed * atkSpeed));
        float rate = 1f / frame;
        controller.aIFSMVariabls.rangeAttackTimingTime = rangeClip.shootTimingFrame * rate;
        controller.aIFSMVariabls.attackEndTime = rangeClip.attackEndAnimationFrame * rate;
        controller.aIFSMVariabls.currentAttackRange = rangeClip.canExcuteDistance;
    }


    private void SettingMeleeAttack(AIController controller)
    {
        controller.aIFSMVariabls.maxTargetCount = controller.aIFSMVariabls.meleeClip.maxTargetCount;
        controller.aIFSMVariabls.attackDamage = controller.aIFSMVariabls.meleeClip.damage;
        controller.aIFSMVariabls.attackAngle = controller.aIFSMVariabls.meleeClip.attackAngle;
        controller.aIFSMVariabls.attackRange = controller.aIFSMVariabls.meleeClip.attackRange;
        controller.aIFSMVariabls.currentAttackRange = controller.aIFSMVariabls.meleeClip.attackRange[0];
    }
}

