using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Can Patroll", fileName = "CanPatrollDecision")]
public class CanPatrollDecision : Decision
{

    public override bool Decide(AIController controller)
    {
        if (controller.wayPointInfos.Count <= 0)
            return false;

        return true;
    }
}
