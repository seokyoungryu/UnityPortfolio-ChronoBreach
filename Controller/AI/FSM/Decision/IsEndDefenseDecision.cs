using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is End Defense", fileName = "IsEndDefenseDecision")]
public class IsEndDefenseDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        if (!controller.aiConditions.IsDefensing && controller.aIFSMVariabls.isEndBlockDefense)
            return true;

        return false;
    }
}
