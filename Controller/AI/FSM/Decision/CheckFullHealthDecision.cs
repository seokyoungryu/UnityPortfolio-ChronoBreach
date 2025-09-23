using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Check Full Health")]
public class CheckFullHealthDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return controller.aiStatus.CurrentHealth >= controller.aiStatus.CurrentMaxHealth;
    }
}
