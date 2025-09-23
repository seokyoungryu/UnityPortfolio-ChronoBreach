using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is Done Dash")]
public class IsDoneDashDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return controller.aIFSMVariabls.isDoneDash;
    }

   
}
