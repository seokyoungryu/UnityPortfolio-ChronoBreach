using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Can Dash")]
public class CanDashDecision : Decision
{
    //액셀레이트 같은 경우는 SetDestination으로 하기떄문에 검사 안해도될듯?
    //텔포만 검사.
    //max can dash distance도 추가해서 검사.
    public override bool Decide(AIController controller)
    {
        if (controller.aIVariables.target == null) return false;

        if (controller.aIFSMVariabls.currentTotalDashCount >= controller.aIVariables.limitDashCount 
            && !CanDashTargetDistance(controller) && controller.IsDetectObstacle(controller.damagedPosition, controller.aIVariables.target.damagedPosition))
        {
            return false;
        }

        if (controller.aiConditions.CanDash && CanTeleport(controller, controller.aIVariables.target, controller.aIVariables.dashType))
            return true;

        return false;
    }

    private bool CanDashHeight(AIController controller)
    {
        float yHeight = controller.aIVariables.target.transform.position.y - controller.transform.position.y;
        //Debug.Log("H : " + yHeight);
        if (yHeight >= controller.aIVariables.rangeDashHeight.x && yHeight <= controller.aIVariables.rangeDashHeight.y)
            return true;

        return false;
    }

    private bool CanDashTargetDistance(AIController controller)
    {
        if (controller.aIVariables.target == null) return false;

        Vector3 dir = controller.aIVariables.target.transform.position - controller.transform.position;
        dir.y = 0f;
        float distance = dir.magnitude;
        if (distance >= controller.aIVariables.rangeCanDashTargetDistance.x & distance <= controller.aIVariables.rangeCanDashTargetDistance.y)
            return true;

        return false;
    }

    private bool CanTeleport(AIController controller,BaseController target , DashType dashType)
    {
        if (target == null) return false;

        controller.aIFSMVariabls.randomDashDistance = Random.Range(controller.aIVariables.rangeDashStopDistance.x, controller.aIVariables.rangeDashStopDistance.y);
        controller.aIFSMVariabls.dashDirection = GetDirectionDashType(dashType, controller,target);
        controller.aIFSMVariabls.dashDirection.Normalize();
        Ray ray = new Ray(target.damagedPosition.position, controller.aIFSMVariabls.dashDirection);

        //1차 타겟과 텔포 위치에 장애물 있나 판단.
        if (Physics.Raycast(ray, out controller.aIFSMVariabls.dashHit, controller.aIFSMVariabls.randomDashDistance, controller.obstacleLayer + controller.limitDashObstacleLayer))
            return false;

        int count = Physics.OverlapSphereNonAlloc(target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance
                                                  , 5f, controller.aIFSMVariabls.limitDashColl, controller.limitDashObstacleLayer);
        if (count > 0)
            return false;

        //2차 해당 위치에서 땅이 있나 판단.
        float height = Mathf.Abs(controller.aIVariables.rangeDashHeight.x) + controller.aIVariables.rangeDashHeight.y;
        ray = new Ray(target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * height, Vector3.down);
        if (!Physics.SphereCast(ray, 0.5f, out controller.aIFSMVariabls.dashHit, height, controller.groundLayer))
            return false;

        //3차 높이가 되는지 확인
        if (!CanDashHeight(controller))
            return false;

        // Debug.Log("Decision Can Dash Distance : " + controller.aIFSMVariabls.randomDashDistance);

        return true;
    }


    private Vector3 GetDirectionDashType(DashType dashType,  AIController controller, BaseController target)
    {
        if (dashType == DashType.TELEPORT_TO_BACK)
            return -target.transform.forward;
        else if (dashType == DashType.TELEPORT_TO_FRONT)
            return target.transform.forward;
        else if (dashType == DashType.TELEPORT_TO_RANDOMANGLE)
            return GetRandomDirection(controller, target);

        return Vector3.zero;
    }

    private Vector3 GetRandomDirection(AIController controller, BaseController target)
    {
        controller.aIFSMVariabls.randomDirAngle = Random.Range(0f, 360f);

        Vector3 dir = Quaternion.Euler(0,controller.aIFSMVariabls.randomDirAngle,0) * target.transform.forward;
        dir.Normalize();
        //Debug.Log("Decision Angle : " + controller.aIFSMVariabls.randomDirAngle);

        return dir;
    }


    public override void DrawDecisionGizmos(AIController controller)
    {
        BaseController target = controller.aIVariables.target;
        if (target == null) return;

        Ray ray = new Ray(target.damagedPosition.position, controller.aIFSMVariabls.dashDirection);

        //1차 타겟과 텔포 위치에 장애물 있나 판단.
        if (Physics.Raycast(ray, out controller.aIFSMVariabls.dashHit, controller.aIFSMVariabls.randomDashDistance, controller.obstacleLayer))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(target.damagedPosition.position, target.damagedPosition.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(target.damagedPosition.position, target.damagedPosition.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance);
        }

        //2차 해당 위치에서 땅이 있나 판단.
        float height = Mathf.Abs(controller.aIVariables.rangeDashHeight.x) + controller.aIVariables.rangeDashHeight.y;
        ray = new Ray(target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * height, Vector3.down);
        if (!Physics.SphereCast(ray, 0.5f, out controller.aIFSMVariabls.dashHit, height, controller.groundLayer))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * 5f, 0.5f);
            Gizmos.DrawWireSphere(target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * 5f + Vector3.down * height, 0.5f);
            Gizmos.DrawLine(target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * 5f
                , target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * 5f + Vector3.down * height);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * 5f, 0.5f);
            Gizmos.DrawWireSphere(target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * 5f + Vector3.down * height, 0.5f);
            Gizmos.DrawLine(target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * 5f
                , target.transform.position + controller.aIFSMVariabls.dashDirection * controller.aIFSMVariabls.randomDashDistance + Vector3.up * 5f + Vector3.down * height);

        }

        //3차 높이가 되는지 확인
        if (!CanDashHeight(controller))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(target.transform.position + Vector3.up * controller.aIVariables.rangeDashHeight.x
                , target.transform.position + Vector3.up * controller.aIVariables.rangeDashHeight.y);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(target.transform.position + Vector3.up * controller.aIVariables.rangeDashHeight.x
                , target.transform.position + Vector3.up * controller.aIVariables.rangeDashHeight.y);

        }



    }

}
