using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Have Target")]
public class HaveTargetDecision : Decision
{
    [SerializeField] private bool isCheckFollowTarget = false;

    public override bool Decide(AIController controller)
    {
        if (isCheckFollowTarget)
            return controller.aIVariables.followTarget;
        else
            return controller.aIVariables.Target != null;
    }

}
