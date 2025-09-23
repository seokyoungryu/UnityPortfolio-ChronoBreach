using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Damaged")]
public class DamagedAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        controller.aiAnim.Play("Reset",1,0f);

        controller.nav.updatePosition = true;
        controller.nav.updateRotation = true;

        DamagedClip damagedClip;
        if (controller.aiConditions.DamagedStanding)
            damagedClip = GetDamagedClip(controller, AttackStrengthType.WEAK);
        else
            damagedClip = GetDamagedClip(controller, controller.aIFSMVariabls.DamagedStrengthType);

        if (damagedClip != null) 
            controller.StopAllCoroutines();
        if (damagedClip != null && controller.aIVariables.Target != null) 
            controller.RotateTarget(controller.aIVariables.Target.transform);

        if (damagedClip != null && !damagedClip.IsFlyDown)
        {
            controller.aiAnim.Play(damagedClip.AniamtionName,4 ,0f);
            controller.aiConditions.IsDamaged = false;
            controller.aIFSMVariabls.DamagedTimer = damagedClip.EndAnimationFrameToTime();
        }
        else if (damagedClip != null && damagedClip.IsFlyDown)
        {
            controller.aiConditions.IsDamaged = false;
            controller.StartCoroutine(FlyDownProcess(controller, damagedClip));
        }

        controller.SetNavSpeed(0f);

        controller.myRigid.velocity = Vector3.zero;
        controller.aiConditions.CanAttacking = false;
        controller.aIFSMVariabls.CurrentDamagedTimer = 0f;
        controller.aIFSMVariabls.IsEndDamagedAnimation = false;
        controller.aiConditions.IsDamageState = true;
        controller.aiConditions.IsForcedDamage = false;
        controller.aIFSMVariabls.DamagedStrengthType = AttackStrengthType.NONE;
    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.aiConditions.IsDown) return;

        controller.aIFSMVariabls.CurrentDamagedTimer += Time.deltaTime;
        if (controller.aIFSMVariabls.CurrentDamagedTimer >= controller.aIFSMVariabls.DamagedTimer + 0.4f)
        {
            controller.aIFSMVariabls.IsEndDamagedAnimation = true;
        }
    }

    public override void OnExitAction(AIController controller)
    {
        controller.aiConditions.CanAttacking = true;
        controller.aiConditions.IsDown = false;
        controller.aiConditions.IsDamageState = false;
        controller.aIFSMVariabls.IsEndDamagedAnimation = false;
        controller.StopAllCoroutines();

        controller.aiAnim.applyRootMotion = false;
        controller.nav.updatePosition = true;
        controller.nav.updateRotation = true;
        controller.nav.Warp(controller.transform.position);

    }

    private IEnumerator FlyDownProcess(AIController controller, DamagedClip damagedClip)
    {
        controller.aiConditions.IsDown = true;
        controller.aiAnim.Play(damagedClip.AniamtionName, 4,0f);

        yield return new WaitForSeconds(damagedClip.FrameToTime(damagedClip.EndAnimationFrame) + damagedClip.RiseTime);

        controller.aiConditions.IsDamaged = false;
        DamagedClip riseClip = controller.riseClip;
        controller.aiAnim.CrossFade(riseClip.AniamtionName, 0.2f);

        yield return new WaitForSeconds(riseClip.FrameToTime(riseClip.EndAnimationFrame) + 0.4f);
        controller.aiConditions.IsDown = false;
        controller.aIFSMVariabls.IsEndDamagedAnimation = true;
    }


    private DamagedClip GetDamagedClip(AIController controller, AttackStrengthType attackStrengthType)
    {
        foreach (DamagedClip clip in controller.damagedClips)
            if (clip.StrengthType == attackStrengthType)
                return clip;
        return null;
    }
}
