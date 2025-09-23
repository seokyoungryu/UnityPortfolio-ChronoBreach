using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is End Groggy", fileName ="IsEndGroggyDecision")]
public class IsEndGroggyDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        if (controller.aIFSMVariabls.isEndGroggy)
            return true;
        return false;
    }
}

