using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is End Attacking")]
public class IsEndAttackingDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return controller.aiConditions.IsEndAttacking;
    }
}
