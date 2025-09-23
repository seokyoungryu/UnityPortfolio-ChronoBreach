using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 이거 머임 헷갈림.
/// </summary>
[CreateAssetMenu(menuName = "AI/Actions/Rest")]
public class RestAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        controller.aIFSMVariabls.currentRestAwaitTimer = 0f;
        controller.aIFSMVariabls.currentRestTimer = 0f;
        controller.SetNavSpeed(0f);
        controller.myRigid.velocity = Vector3.zero;
        controller.aiConditions.CanRest = false;
        controller.aiAnim.CrossFade("Rest", 0.2f);
        controller.aiAnim.SetBool("Rest", true);

        Debug.Log("Rest Enter");

    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.aiStatus.IsFullHP()) return;

        controller.aIFSMVariabls.currentRestTimer += deltaTime;
        if (controller.aIFSMVariabls.currentRestTimer >= controller.aIVariables.restWaitPerTime)
        {
            controller.aIFSMVariabls.currentRestTimer = 0;
            controller.aiStatus.AddCurrentHealth((int)controller.aIVariables.restHealHealthPerValue);
        }

    }


    public override void OnExitAction(AIController controller)
    {
        controller.aiConditions.CanRest = true;
        controller.aIFSMVariabls.currentRestTimer = 0f;
        controller.aiAnim.SetBool("Rest", false);

    }


}
