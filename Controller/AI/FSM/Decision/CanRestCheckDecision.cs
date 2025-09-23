using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Can Rest Check")]
public class CanRestCheckDecision : Decision
{

    public override bool Decide(AIController controller)
    {
        if (controller.aiConditions.CanRest && !controller.IsMove() && controller.aIVariables.target == null && !controller.aiStatus.IsFullHP())
        {
            controller.aIFSMVariabls.currentRestAwaitTimer += Time.deltaTime;
            //Debug.Log("레스팅중..");
            if (controller.aIFSMVariabls.currentRestAwaitTimer >= controller.aIVariables.requireEnterRestTime)
            {
                //Debug.Log("캔 레스팅");
                return true;
            }
        }
        else
            controller.aIFSMVariabls.currentRestAwaitTimer = 0f;

        return false;
    }

}
