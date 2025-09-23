using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Wait")]
public class WaitAction : Action
{
    public float waitTime = 0f;

    public override void OnEnterAction(AIController controller)
    {
        controller.aIFSMVariabls. timer = 0f;
        controller.SetNavSpeed(0f);
        controller.myRigid.velocity = Vector3.zero;
        controller.aiConditions.IsWaitTime = false;

    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.aiConditions.IsWaitTime) return;

        controller.aIFSMVariabls.timer += Time.deltaTime;
        if(controller.aIFSMVariabls.timer >= waitTime)
        {
            controller.aiConditions.IsWaitTime = true;
        }
    }

    
}
