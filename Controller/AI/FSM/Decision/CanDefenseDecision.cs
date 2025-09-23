using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIDefenseType { NONE = -1, COUNT = 0, TIME = 1 }


[CreateAssetMenu(menuName = "AI/Decisions/Can Defense")]
public class CanDefenseDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        if (controller.aiConditions.IsGroggying) return false;
        if (controller.aiConditions.IsDown) return false;

        if (controller.aiConditions.CanDefense && !controller.aiConditions.IsDefensing)
            return true;

        return false;
    }
}

