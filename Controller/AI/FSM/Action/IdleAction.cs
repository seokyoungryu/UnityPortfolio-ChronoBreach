using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Idle")]
public class IdleAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
       
        controller.aIFSMVariabls.currentIdleTimer = 0f;
        controller.SetNavSpeed(0f);

        controller.nav.velocity = Vector3.zero;
        controller.nav.stoppingDistance = controller.aIVariables.stopDistance;
        controller.aiAnim.Play("Idle");
    }

    public override void Act(AIController controller, float deltaTime)
    {
    }

}
