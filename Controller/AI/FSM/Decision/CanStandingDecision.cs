using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIStandingType { ATTACK_COUNT = 0, Full_COMBO = 1}

[CreateAssetMenu(menuName = "AI/Decisions/Can Standing")]
public class CanStandingDecision : Decision
{
    //스탠딩 타입.
    //1. 공격 한번.
    //2. 1 콤보.

    public override bool Decide(AIController controller)
    {
        if (controller.aiConditions.IsGroggying) return false;
        if (controller.aiConditions.CanStanding && !controller.aiConditions.IsStanding)
            return true;

        return false;
    }

}
