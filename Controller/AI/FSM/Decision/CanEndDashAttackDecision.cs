using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "AI/Decisions/Can End Dash Attack", fileName ="CanEndDashAttackDecision")]
public class CanEndDashAttackDecision : Decision
{
    public CanAttackDecision canAttackDecision;
    public CanDashDecision canDashDecision;
    public override bool Decide(AIController controller)
    {
        if (canAttackDecision.Decide(controller) && canDashDecision.Decide(controller))
            return true;

        return false;
    }

   
}
