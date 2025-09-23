using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Check Is End Damaged")]
public class CheckIsEndDamagedDecision : Decision
{
    public override bool Decide(AIController controller)
    {
        return controller.aIFSMVariabls.IsEndDamagedAnimation;
    }

}
