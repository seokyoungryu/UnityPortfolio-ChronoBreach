using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Current Combat Type Detect")]
public class CurrentCombatTypeDetectDecision : Decision
{
    public bool isNoneTpye = false;
    public CurrentCombatType detectCurrentCombatType = CurrentCombatType.NONE;

    public override bool Decide(AIController controller)
    {
        if(isNoneTpye)
        {
            if (controller.aiConditions.currentCombatType == CurrentCombatType.NONE)
                return true;
            else
                return false;
        }

        return controller.aiConditions.currentCombatType == detectCurrentCombatType;
    }
}
