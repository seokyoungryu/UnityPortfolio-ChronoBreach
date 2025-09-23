using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/ Check Exist Target")]
public class CheckExistTargetDecision : Decision
{
  
    public override bool Decide(AIController controller)
    {
        return controller.aIVariables.Target != null;
    }

}
