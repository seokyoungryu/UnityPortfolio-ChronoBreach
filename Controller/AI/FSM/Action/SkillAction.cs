using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Skill")]
public class SkillAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
      //  Debug.Log("스킬 상태 Enter");
        controller.nav.Warp(controller.transform.position);
        controller.aiAnim.applyRootMotion = true;
        controller.nav.updatePosition = false;
        controller.nav.updateRotation = true;

        controller.aiConditions.IsSkilling = true;
        controller.aiConditions.IsEndSkilling = false;
        controller.myRigid.velocity = Vector3.zero;
        controller.nav.velocity = Vector3.zero;
        controller.SetNavSpeed(0f);

        //이부분 combat 설정하는 Action에다가
        //하기. 즉. 여기서 랜덤으로 정하고 스킬을 시작하는게 아니라, combatType이 Skill이 되었을때, 스킬클립을 세팅하기.
        //그리고 여기서는 단순히 지금 등록된 스킬을 실행하는 역활임.
        //여기서는 조건으로 만약 해당 클립이 쿨타임중이면 다른 클립을 검색, 전부 쿨타임일경우 attack으로.
        if (controller.aIFSMVariabls.currentSkillData == null || controller.aIFSMVariabls.currentSkillData.skillClip == null)
        {
            controller.aiConditions.currentCombatType = CurrentCombatType.ATTACK;
            controller.aiConditions.IsEndSkilling = true;
            return;
        }

        controller.aiAnim.SetFloat(AnimatorKey.BeforeSkillMotionSpeed, controller.aIFSMVariabls.currentSkillData.skillClip.beforeSkillMotionInfo.animationSpeed);

        Debug.Log("-------------------");
        Debug.Log("스킬 실행!");
        Debug.Log("스킬 이름 : " + controller.aIFSMVariabls.currentSkillData.skillName_Kor);
        Debug.Log("스킬 타입 : " + controller.aIFSMVariabls.currentSkillData.skillClip.skillType);

        SoundManager.Instance.PlayEffect(controller.aIFSMVariabls.currentSkillData.skillClip.randomVoiceSound);

        if (controller.SkillNotifierEffectTr != null)
        {
            SoundManager.Instance.PlayEffect(controller.SkillNotifierEffect.effectSound);
            GameObject go = EffectManager.Instance.GetEffectObjectInfo(controller.SkillNotifierEffect, Vector3.zero, Vector3.zero, Vector3.zero);
            go.transform.parent = controller.SkillNotifierEffectTr;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
        }


        if (controller.aIFSMVariabls.currentSkillType == SkillType.ATTACK)
        {
            controller.StartCoroutine(AttackSkillProcess(controller));
        }
        else if (controller.aIFSMVariabls.currentSkillType == SkillType.MAGIC)
        {
            controller.StartCoroutine(MagicSkillProcess(controller));
        }
        else if (controller.aIFSMVariabls.currentSkillType == SkillType.BUFF)
        {
            controller.StartCoroutine(BuffSkillProcess(controller));
        }

        Debug.Log("-------------------");

    }


    public override void Act(AIController controller, float deltaTime)
    {
        if(controller.aIFSMVariabls.isSkillSmoothRotate)
        {
            controller.transform.rotation 
                = Quaternion.Lerp(controller.transform.rotation, controller.aIFSMVariabls.targetRotation, controller.aIVariables.skillSmoothRotateAnguler* Time.deltaTime);
            if (Quaternion.Angle(controller.transform.rotation, controller.aIFSMVariabls.targetRotation) <= 5f)
            {
                controller.aIFSMVariabls.isSkillSmoothRotate = false;
                controller.transform.rotation = controller.aIFSMVariabls.targetRotation;
            }
        }

     //   Debug.Log("스킬중..");

    }

    public override void OnExitAction(AIController controller)
    {
        controller.StopAllCoroutines();
        controller.aiConditions.IsSkilling = false;
        controller.aiConditions.IsEndSkilling = true;
        controller.aIFSMVariabls.DamagedStrengthType = AttackStrengthType.NONE;
        controller.aiConditions.IsDamaged = false;
        controller.aiConditions.currentCombatType = CurrentCombatType.NONE;
        controller.aIFSMVariabls.attackTiming = null;
        controller.aIFSMVariabls.attackRotateTiming = null;
        controller.aiAnim.applyRootMotion = false;
        controller.nav.updatePosition = true;
        controller.nav.updateRotation = true;
        controller.aiConditions.IgnoreDetectCollider = false;

        controller.nav.Warp(controller.transform.position);

    }



    private IEnumerator AttackSkillProcess(AIController controller)
    {
        //Before Skill Motion Process
        if (controller.aIFSMVariabls.currentAttackClip.beforeSkillMotionInfo.beforeMotionClip != null)
            yield return controller.StartCoroutine(ExcuteBeforeSkillMotion_Co(controller.aIFSMVariabls.currentAttackClip.beforeSkillMotionInfo, controller));

        controller.aiConditions.detectedOn = false;
        controller.aiAnim.SetFloat(AnimatorKey.SkillSpeed, controller.aIFSMVariabls.currentAttackClip.SkillAnimSpeed);
       // Debug.Log("스킬 속도 : "  + controller.aIFSMVariabls.currentAttackClip.SkillAnimSpeed);

        controller.skillController.AddSkillCoolTimeList(controller.aIFSMVariabls.currentSkillData);
        if (controller.aIFSMVariabls.currentSkillData.skillClip.startRotateToTarget)
            RotateToTarget(controller);

        controller.aiAnim.Play(controller.aIFSMVariabls.currentAttackClip.AnimationClipName);
        for (int i = 0; i < controller.aIFSMVariabls.currentAttackClip.AttackEffects.Count; i++)
            CreateAttackEffect(controller, controller.aIFSMVariabls.currentAttackClip.AttackEffects[i]);

        controller.StartCoroutine(AttackRotates(controller));
        controller.StartCoroutine(AttackTimings(controller));
        controller.StartCoroutine(AttackEndTime(controller));

    }

    private IEnumerator AttackRotates(AIController controller)
    {
        if (controller.aIFSMVariabls.currentAttackClip.AnimationRotateFrames.Count > 0)
        {
            SetMultiFrameToTime(controller, ref controller.aIFSMVariabls.attackRotateTiming, controller.aIFSMVariabls.currentAttackClip.AnimationRotateFrames.ToArray()
                    , controller.aIFSMVariabls.currentAttackClip.AnimationClip.frameRate);
            for (int i = 0; i < controller.aIFSMVariabls.currentAttackClip.AnimationRotateFrames.Count; i++)
            {
                yield return new WaitForSeconds(controller.aIFSMVariabls.attackRotateTiming[i]);
                controller.aiConditions.detectedOn = false;
                RotateToTarget(controller);
               // Debug.Log(controller.aIFSMVariabls.attackRotateTiming[i]);
            }
        }

    }

    private IEnumerator AttackTimings(AIController controller)
    {
        if (controller.aIFSMVariabls.currentAttackClip.AnimationAttackTimingFrame.Count > 0)
        {
            SetMultiFrameToTime(controller,ref controller.aIFSMVariabls.attackTiming, controller.aIFSMVariabls.currentAttackClip.AnimationAttackTimingFrame.ToArray()
                ,controller.aIFSMVariabls.currentAttackClip.AnimationClip.frameRate);
            for (int i = 0; i < controller.aIFSMVariabls.currentAttackClip.AnimationAttackTimingFrame.Count; i++)
            {
                if (controller.aiConditions.IsDead) continue;
                controller.aIFSMVariabls.currentAttackRange = controller.aIFSMVariabls.skillRange[i];

                yield return new WaitForSeconds(controller.aIFSMVariabls.attackTiming[i]);
                if (FindCanDamagedToTarget(controller, true, i))
                {
                    for (int x = 0; x < controller.aIFSMVariabls.canDamageEnemy.Count; x++)
                    {
                        if (controller.aIFSMVariabls.canDamageEnemy[x] == null || controller.aIFSMVariabls.canDamageEnemy[x].GetComponent<BaseController>() == null)
                            continue;
                        if (controller.IsDetectObstacle(controller.damagedPosition, controller.aIFSMVariabls.canDamageEnemy[x].GetComponent<BaseController>().damagedPosition)) 
                            continue;

                        if (controller.aIFSMVariabls.skillDamage.Count < i)
                            controller.aIFSMVariabls.skillDamage[i] = controller.aIFSMVariabls.skillDamage[0];

                        Damage(controller, controller.aIFSMVariabls.canDamageEnemy[x], true, i, controller.aIFSMVariabls.skillDamage[i],controller.aIFSMVariabls.currentAttackClip.StrengthType[i]);
                        EffectManager.Instance.GetEffectObjectRandom(controller.aIFSMVariabls.currentAttackClip.HitEffects[i],
                                                                controller.aIFSMVariabls.canDamageEnemy[x].transform.position,
                                                               Vector3.zero,
                                                               controller.transform.localScale);

                    }
                }

            }
        }
    }

    private IEnumerator AttackEndTime(AIController controller)
    {

        yield return new WaitForSeconds(controller.aIFSMVariabls.currentAttackClip.GetFrameToTime(controller.aIFSMVariabls.currentAttackClip.AnimationEndFrame
                                                                                                  ,controller.aIFSMVariabls.currentAttackClip.AnimationClip.frameRate
                                                                                                  , controller.aIFSMVariabls.skillSpeed));
        controller.aiConditions.IsEndSkilling = true;
        controller.aiConditions.IsSkilling = false;
        controller.aiConditions.detectedOn = false;
    }



    private IEnumerator MagicSkillProcess(AIController controller)
    {
        controller.skillController.AddSkillCoolTimeList(controller.aIFSMVariabls.currentSkillData);
        controller.aiAnim.SetFloat(AnimatorKey.SkillSpeed, controller.aIFSMVariabls.currentMagicClip.animationSpeed);

        if (controller.aIFSMVariabls.currentMagicClip.beforeSkillMotionInfo.beforeMotionClip != null)
            yield return controller.StartCoroutine(ExcuteBeforeSkillMotion_Co(controller.aIFSMVariabls.currentMagicClip.beforeSkillMotionInfo, controller));

        if(controller.aIFSMVariabls.currentMagicClip.startRotateToTarget)
            controller.StartCoroutine(SmoothRotateToTargeting(controller, controller.aIFSMVariabls.currentMagicClip.GetRotatingWhenCastTime()));

        controller.aiAnim.CrossFade(controller.aIFSMVariabls.currentMagicClip.skillAnimationName, 0.2f);

        for (int i = 0; i < controller.aIFSMVariabls.currentMagicClip.createProjectileInfos.Count; i++)
            controller.StartCoroutine(CreateMagic_Co(controller, controller.aIFSMVariabls.currentMagicClip.createProjectileInfos[i].createFrameTiming, i));

        yield return new WaitForSeconds(controller.aIFSMVariabls.currentMagicClip.GetEndTime());
        controller.aiConditions.IsEndSkilling = true;
        Debug.Log("종료");
    }





    private IEnumerator BuffSkillProcess(AIController controller)
    {
        BaseController[] targets = controller.GetBuffTargetList(controller, controller.aIFSMVariabls.currentBuffClip);
        if(targets == null || targets.Length <= 0)
        {
            controller.aiConditions.currentCombatType = CurrentCombatType.ATTACK;
            controller.aiConditions.IsEndSkilling = true;
            yield break;
        }

        if (controller.aIFSMVariabls.currentBuffClip.beforeSkillMotionInfo.beforeMotionClip != null)
            yield return controller.StartCoroutine(ExcuteBeforeSkillMotion_Co(controller.aIFSMVariabls.currentBuffClip.beforeSkillMotionInfo, controller));

        float buffTiming = controller.aIFSMVariabls.currentBuffClip.GetBuffTimingTime();
        controller.skillController.AddSkillCoolTimeList(controller.aIFSMVariabls.currentSkillData);
        controller.aiAnim.SetFloat(AnimatorKey.SkillSpeed, controller.aIFSMVariabls.currentBuffClip.animationSpeed);

        for (int i = 0; i < targets.Length; i++)
            CreateBuffEffects(controller, targets[i], controller.aIFSMVariabls.currentBuffClip);

        if (controller.aIFSMVariabls.currentSkillData.skillClip.startRotateToTarget)
            RotateToTarget(controller);

        controller.aiAnim.CrossFade(controller.aIFSMVariabls.currentBuffClip.animationClipName, 0.2f);

        yield return new WaitForSeconds(buffTiming);

        ApplyBuff(controller, targets);
     
        yield return new WaitForSeconds(controller.aIFSMVariabls.currentBuffClip.GetBuffTimingTime() - buffTiming);
        controller.aiConditions.IsEndSkilling = true;
    }


    private IEnumerator ExcuteBeforeSkillMotion_Co(BeforeSkillMotionInfo info , AIController controller)
    {
        if (info == null || controller == null)
            yield break;

        if (info.allowRootMotion)
            controller.aiConditions.IgnoreDetectCollider = true;

        if (info.useBeforeMRotateToTarget)
            yield return controller.StartCoroutine(RotateTarget_SmoothRotate(controller));

        yield return controller.StartCoroutine(info.ExcuteBeforeMotion_Co(controller.aiAnim, controller.transform, controller.aIVariables.target.transform));

        controller.aiConditions.IgnoreDetectCollider = false;
    }


    private void CreateBuffEffects(AIController controller, BaseController target, BuffSkillClip buffClip)
    {
        for (int i = 0; i < buffClip.castingEffect.Length; i++)
        {
            float delayTime = buffClip.GetFrameToTime(buffClip.castingEffect[i].effectFrame, buffClip.animationClip.frameRate, buffClip.animationSpeed);
            controller.StartCoroutine(controller.CreateAttackEffect(delayTime, buffClip.castingEffect[i], target));
        }
        for (int i = 0; i < buffClip.effectInfos.Length; i++)
        {
            float delayTime = buffClip.GetFrameToTime(buffClip.effectInfos[i].effectFrame, buffClip.animationClip.frameRate, buffClip.animationSpeed);
            controller.StartCoroutine(controller.CreateTimeParticleEffect(delayTime, buffClip.effectInfos[i], target,buffClip.particleTimeType, buffClip.stayTime, buffClip.durationTime, buffClip.returnObpTime));
        }
    }

    private IEnumerator SmoothRotateToTargeting(AIController controller, float followTimer = 0f)
    {
        if (controller.aIVariables.target == null || controller == null) yield break;

        float timer = 0f;
        while (timer < followTimer)
        {
            timer += Time.deltaTime;
            if (controller.aIVariables.target != null && controller != null)
                controller.RotateTarget(controller.aIVariables.Target.transform);
            yield return null;
        }
    }


    private IEnumerator RotateTarget_SmoothRotate(AIController controller)
    {
        if (controller.aIVariables.Target == null) yield break;

        controller.aIFSMVariabls.isSkillSmoothRotate = true;
        Vector3 direction = controller.aIVariables.Target.transform.position - controller.transform.position;
        direction.y = 0f;
        direction.Normalize();

        controller.aIFSMVariabls.targetRotation = Quaternion.LookRotation(direction);
        while (controller.aIFSMVariabls.isSkillSmoothRotate)
        {
            yield return null;
        }
    }

    private void RotateToTarget(AIController controller)
    {
        if(controller.aIVariables.Target != null)
        {
             Vector3 direction = controller.aIVariables.Target.transform.position - controller.transform.position;
            direction.y = 0f;
            direction.Normalize();

            Quaternion targetRotate = Quaternion.LookRotation(direction);
            controller.transform.rotation = targetRotate;
        }

    }
    private void CreateAttackEffect(AIController controller, ControllerEffectInfo effectInfo)
    {
        float framerate = 1f / (controller.aIFSMVariabls.currentAttackClip.AnimationClip.frameRate * (controller.aIFSMVariabls.currentAttackClip.SkillAnimSpeed));
        controller.StartCoroutine(controller.CreateAttackEffect(effectInfo.effectFrame * framerate, effectInfo, controller));
    }
    private void SetMultiFrameToTime(AIController controller,ref float[] retArray, int[] skillTimingFrames, float frameRate= 30f)
    {
        retArray = new float[skillTimingFrames.Length];
        float framerate = 1f / (frameRate * controller.aIFSMVariabls.skillSpeed);

        if (skillTimingFrames.Length > 0)
        {
            float beforeFrameTime = 0f;
            for (int i = 0; i < skillTimingFrames.Length; i++)
            {
                if (i >= 1)
                {
                    retArray[i] = (skillTimingFrames[i] * framerate) - beforeFrameTime;
                    beforeFrameTime = skillTimingFrames[i] * framerate;
                }
                else
                {
                    retArray[i] = skillTimingFrames[i] * framerate;
                    beforeFrameTime = retArray[i];
                }
            }
        }
    }

   

    private void ApplyBuff(AIController own,BaseController[]  targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            Debug.Log("Apply Buff : " + targets[i].name);
            for (int j = 0; j < own.aIFSMVariabls.currentBuffClip.buffObjects.Length; j++)
                own.aIFSMVariabls.currentBuffClip.buffObjects[j].Apply(targets[i]);
        }
    }

    private IEnumerator CreateMagic_Co(AIController controller, float frame, int index)
    {
        float delayTime = frame * (1f / (controller.aIFSMVariabls.currentMagicClip.skillAnimationClip.frameRate * controller.aIFSMVariabls.skillSpeed));
        yield return new WaitForSeconds(delayTime);
        CreateMagicAndSetting(controller, index);
    }

    private void CreateMagicAndSetting(AIController controller, int index)
    {
        controller.aIFSMVariabls.currentMagicClip.createProjectileInfos[index]?.projectileCreator?.ExcuteCreate(controller, controller.aIVariables.Target?.transform, controller);
    }


  //  private Vector3 SetSkillPosition(AIController controller)
  //  {
  //      Vector3 retPosition = Vector3.zero;
  //
  //      switch(controller.aIFSMVariabls.currentMagicClip.Infos.RangeMoveType)
  //      {
  //         case RangeMoveType.THROWING:
  //              retPosition = controller.skillShootingPosition.position;
  //             break;
  //         case RangeMoveType.POINT:
  //             retPosition = controller.aIVariables.Target.transform.root.position;
  //              retPosition.y += 0.15f;
  //             break;
  //       
  //         case RangeMoveType.HOMING:
  //             retPosition = controller.aIVariables.Target.transform.root.position;
  //              retPosition.y += 0.15f;
  //             break;
  //      }
  //
  //      return retPosition;
  //  }
  //
  //
  //  private Vector3 GetSkillDirection(AIController controller)
  //  {
  //      Vector3 retDirection = Vector3.zero;
  //      switch (controller.aIFSMVariabls.currentMagicClip.Infos.RangeMoveType)
  //      {
  //          case RangeMoveType.THROWING:
  //              retDirection = controller.aIVariables.Target.transform.position - controller.transform.position;
  //              retDirection.y = 0f;
  //              retDirection.Normalize();
  //              break;
  //          case RangeMoveType.POINT:
  //              break;
  //
  //          case RangeMoveType.HOMING:
  //              break;
  //      }
  //      return retDirection;
  //  }

}


