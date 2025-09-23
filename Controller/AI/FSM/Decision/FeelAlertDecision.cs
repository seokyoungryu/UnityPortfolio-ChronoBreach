using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Feel Alert")]
public class FeelAlertDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return controller.aiConditions.IsFeelAlert;
    }
}
