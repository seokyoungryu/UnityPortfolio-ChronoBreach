using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is End Skill")]
public class IsEndSkillDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return controller.aiConditions.IsEndSkilling;
    }
}
