using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is End Find Enemy", fileName = "IsEndFindEnemyDecision")]
public class IsEndFindEnemyDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return controller.aIFSMVariabls.IsEndNearAction;
    }

}
