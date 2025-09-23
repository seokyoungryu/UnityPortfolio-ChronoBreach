using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is Detect Obstacle To Target", fileName ="IsDetectObstacleToTargetDecision")]
public class IsDetectObstacleToTargetDecision : Decision
{
    //ai와 타겟 사이에 장애물이 검출되면 timer를 함. 
    //특정 시간이 지나면 resetPosition으로.

    //그리고 ResetPosition에서 만약 reset가능할 경우 바로 돌아간느게 아니라 변수의 waitTime만큼 그 자리에서 Find하다가 돌아가기??

    public override void OnInitDecide(AIController controller)
    {
    }

    public override bool Decide(AIController controller)
    {
        if(controller.aIVariables.target != null)
        {
            if (controller.IsDetectObstacle(controller.damagedPosition, controller.aIVariables.target.damagedPosition))
            {
                controller.aIFSMVariabls.currentDetectObstacleTimer += Time.deltaTime;
                if (controller.aIFSMVariabls.currentDetectObstacleTimer >= controller.aIFSMVariabls.detectObstacleTime)
                {
                    controller.aIFSMVariabls.isDetectObstacleToTarget = true;
                    if (controller.aIVariables.target != null)
                        controller.aIVariables.target = null;
                }

            }
            else
            {
                controller.aIFSMVariabls.currentDetectObstacleTimer =0f;
                controller.aIFSMVariabls.isDetectObstacleToTarget = false;
            }
            return controller.aIFSMVariabls.isDetectObstacleToTarget;
        }
        return false;
    }
}
