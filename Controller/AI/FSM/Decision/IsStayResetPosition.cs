using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Decisions/Is Stay Reset Position",fileName = "IsStayResetPosition")]
public class IsStayResetPosition : Decision
{
    public override bool Decide(AIController controller)
    {
        if (!controller.aiConditions.CanResetPosition) return true;

        if (Vector3.Distance(controller.transform.position, controller.aIFSMVariabls.resetPos) <= controller.aIFSMVariabls.stayPosOffset)
        {
            return true;
        }    

        return false;
    }
}
