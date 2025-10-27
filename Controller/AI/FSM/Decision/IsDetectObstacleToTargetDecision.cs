using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is Detect Obstacle To Target", fileName ="IsDetectObstacleToTargetDecision")]
public class IsDetectObstacleToTargetDecision : Decision
{

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
