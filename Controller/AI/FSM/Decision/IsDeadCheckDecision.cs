using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Dead Check")]
public class IsDeadCheckDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        if (controller.aiStatus.Immortality)
            return false;

        if (controller.aiConditions.IsDead || (controller.aiStatus.CurrentHealth <= 0f))
            return true;

        return false;
    }
}
