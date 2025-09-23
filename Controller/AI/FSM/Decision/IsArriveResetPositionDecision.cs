using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is Arrive ResetPosition")]
public class IsArriveResetPositionDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return controller.aIFSMVariabls.isArrivePosition;
    }
}
