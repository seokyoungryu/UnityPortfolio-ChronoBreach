using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DashType
{
    NONE = -1,
    ACCELERATE_TO_FRONT = 0,
    TELEPORT_TO_FRONT = 1,
    TELEPORT_TO_BACK = 2,
    TELEPORT_TO_RANDOMANGLE = 3,
}

[CreateAssetMenu(menuName = "AI/Actions/Dash")]
public class DashAction : Action
{

    public override void OnEnterAction(AIController controller)
    {
        controller.aIFSMVariabls.isDoneDash = false;
        controller.aiAnim.Play("Reset");
        controller.aIFSMVariabls.currentDashCoolTimer = 0f;
        controller.aIFSMVariabls.currentTotalDashCount++;
        controller.aIFSMVariabls.currentDashCount++;

        if(controller.aIVariables.useRandomCoolTime)
        controller.aIVariables.dashCoolTime = Random.Range(controller.aIVariables.randomRangeDashCoolTime.x
                                                          ,controller.aIVariables.randomRangeDashCoolTime.y);
        // Debug.Log("Action Can Dash Distance : " + controller.aIFSMVariabls.randomDashDistance);
        // Debug.Log("Action Angle : " + controller.aIFSMVariabls.randomDirAngle);

        // controller.nav.stoppingDistance = controller.aIVariables.dashStoppingDistance;
        controller.SetNavSpeed(controller.aIVariables.accelerateDashSpeed);

        if (controller.aIVariables.target == null || controller.aIVariables.target.IsDead())
        {
            controller.aiConditions.CanDash = false;
            controller.aIFSMVariabls.isDoneDash = true;
            return;
        }

       if (controller.aIVariables.dashType == DashType.ACCELERATE_TO_FRONT)
        {
            controller.StartCoroutine(AccelerateProcess(controller));
        }
        else if (controller.aIVariables.dashType == DashType.TELEPORT_TO_FRONT)
        {
            controller.StartCoroutine(TeleportProcess(controller, DashType.TELEPORT_TO_FRONT));
        }
        else if (controller.aIVariables.dashType == DashType.TELEPORT_TO_BACK)
        {
            controller.StartCoroutine(TeleportProcess(controller, DashType.TELEPORT_TO_BACK));
        }
        else if (controller.aIVariables.dashType == DashType.TELEPORT_TO_RANDOMANGLE)
        {
            controller.StartCoroutine(TeleportProcess(controller, DashType.TELEPORT_TO_RANDOMANGLE));
        }

    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.aIVariables.dashType != DashType.ACCELERATE_TO_FRONT) return;

        if(controller.nav.remainingDistance <= controller.nav.stoppingDistance)
        {
            controller.nav.SetDestination(controller.transform.position);
            controller.aIFSMVariabls.isDoneDash = true;
            controller.aiConditions.CanDash = false;
        }
    }


    public override void OnExitAction(AIController controller)
    {
        controller.nav.stoppingDistance = controller.aIVariables.stopDistance;
        controller.SetNavSpeed(controller.aIVariables.walkSpeed);
        controller.aIFSMVariabls.isDoneDash = false;
    }

    private IEnumerator AccelerateProcess(AIController controller)
    {
        if (controller.aIVariables.haveDashReadyMotion)
        {
            controller.aiAnim.CrossFade(controller.aIVariables.dashReadyMotionAnimationName, 0.2f);
            yield return new WaitForSeconds(controller.aIVariables.dashReadyMotionEndFrame * (1f / 30f));
        }

        controller.nav.SetDestination(controller.aIVariables.Target.transform.position);

    }

    private IEnumerator TeleportProcess(AIController controller, DashType dashType)
    {
        if (controller.aIVariables.haveDashReadyMotion)
        {
            controller.aiAnim.CrossFade(controller.aIVariables.dashReadyMotionAnimationName, 0.2f);
            yield return new WaitForSeconds(controller.aIVariables.dashReadyMotionEndFrame * (1f / 30f));
        }
        EffectManager.Instance.GetEffectObjectInfo(controller.DashEffect, controller.DashTargetMaskTr.position, Vector3.zero, Vector3.zero);
        controller.EnemyDetect(false);

        if (dashType == DashType.TELEPORT_TO_FRONT)
        {
            Vector3 dir = (controller.aIVariables.target.transform.position - controller.transform.position);
            dir.y = 0f;
            dir.Normalize();
            controller.aIFSMVariabls.dashPosition = controller.aIVariables.Target.transform.position + (-dir * controller.aIFSMVariabls.randomDashDistance);
        }
        else if (dashType == DashType.TELEPORT_TO_BACK)
            controller.aIFSMVariabls.dashPosition = (controller.aIVariables.Target.transform.position + (-controller.aIVariables.target.transform.forward * controller.aIFSMVariabls.randomDashDistance));
        else if (dashType == DashType.TELEPORT_TO_RANDOMANGLE)
            controller.aIFSMVariabls.dashPosition = (controller.aIVariables.Target.transform.position + (controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance));

        float height = Mathf.Abs(controller.aIVariables.rangeDashHeight.x) + controller.aIVariables.rangeDashHeight.y;
        Ray ray = new Ray(controller.aIFSMVariabls.dashPosition + Vector3.up * height, Vector3.down);

        //레이를 쏴서 땅 위치로 이동하기.
        if (Physics.Raycast(ray, out controller.aIFSMVariabls.groundHit, height, controller.groundLayer))
            controller.aIFSMVariabls.dashPosition = controller.aIFSMVariabls.groundHit.point;

        controller.TranslatePosition(controller.aIFSMVariabls.dashPosition);

        Vector3 direction = (controller.aIVariables.Target.transform.position - controller.transform.position).normalized;
        direction = controller.aIVariables.Target.transform.position - controller.transform.position;
        direction.y = 0f;
        direction.Normalize();

        Quaternion lookDirection = Quaternion.LookRotation(direction);
        controller.transform.rotation = lookDirection;
        float delayTeleportTime = Random.Range(controller.aIVariables.rangeDelayTeleportTime.x, controller.aIVariables.rangeDelayTeleportTime.y);

                                                                                
        yield return new WaitForSeconds(delayTeleportTime);

        EffectManager.Instance.GetEffectObjectInfo(controller.DashEffect, controller.DashTargetMaskTr.position, Vector3.zero, Vector3.zero);

        controller.EnemyDetect(true);
        controller.aiConditions.CanDash = false;
        controller.aIFSMVariabls.isDoneDash = true;
    }




}
