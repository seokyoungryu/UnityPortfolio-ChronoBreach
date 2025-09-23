using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is End Phase", fileName = "IsEndPhaseDecision")]
public class IsEndPhaseDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        if (controller.aIFSMVariabls.isPhaseDone)
            return true;
        return false;
    }
}
