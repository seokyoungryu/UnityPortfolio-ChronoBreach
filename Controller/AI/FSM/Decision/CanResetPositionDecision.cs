using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Can ResetPosition")]
public class CanResetPositionDecision : Decision
{
    public bool checkTarget = true;

    public override void OnInitDecide(AIController controller)
    {
        controller.aIFSMVariabls.currentResetTimer = 0f;
    }

    public override bool Decide(AIController controller)
    {
        if (!controller.aiConditions.CanResetPosition || !CheckDistanceToResetPosition(controller)) return false;
        if (checkTarget && controller.aIVariables.target)
            controller.aIFSMVariabls.targetDistance = Vector3.Distance(controller.transform.position, controller.aIVariables.Target.transform.position);
        if (!checkTarget)
            controller.aIFSMVariabls.targetDistance = Vector3.Distance(controller.transform.position, controller.aIFSMVariabls.resetPos);


        CanReset(controller, Time.deltaTime);
        if (controller.aIVariables.target && controller.aIFSMVariabls.currentResetTimer >= controller.aIFSMVariabls.resetTimer)
        {
            controller.aIVariables.target = null;
          return true;
        }
        else
            return false;
    }


    private bool CanReset(AIController controller, float time)
    {
        if (controller.aiConditions.IsAttacking || controller.aiConditions.IsSkilling) return false;

        if (controller.aIFSMVariabls.targetDistance >= controller.aIFSMVariabls.resetMinDistance)
        {
            controller.aIFSMVariabls.currentResetTimer += time;
        }
        else
            controller.aIFSMVariabls.currentResetTimer = 0f;
       // Debug.Log("check Reset : " + controller.aIFSMVariabls.currentResetTimer);
        return false;
    }

    private bool CheckDistanceToResetPosition(AIController controller)
    {
        float distance = (controller.transform.position - controller.aIFSMVariabls.resetPos).magnitude;
        if (Mathf.Abs( distance) > 0.3f)
            return true;

        return false;
    }

}
