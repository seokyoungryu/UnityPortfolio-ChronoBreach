using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is Damaged ")]
public class IsDamagedDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        if (controller.aiConditions.IsForcedDamage)
            return true;
        if (CheckDefensing(controller.aiConditions) || CheckStanding(controller.aiConditions) || !CheckIsCommonType(controller))
        {
            controller.aiConditions.IsDamaged = false;
            return false;
        }

        return controller.aiConditions.IsDamaged;
    }

    private bool CheckDefensing(AIConditions condition)
    {
        if (condition.CanDefense || condition.IsDefensing)
            return true;
        return false;
    }
    private bool CheckStanding(AIConditions condition)
    {
        if (condition.CanStanding || condition.IsStanding)
            return true;
        return false;
    }
    private bool CheckIsCommonType(AIController controller)
    {
        if (controller.aiStatus.IgnoreDamageState)
            return false;
        if (controller.aiStatus.AIType == AIType.BOSS || controller.aiStatus.AIType == AIType.ELITE)
            return false;

        return true;
    }
}
