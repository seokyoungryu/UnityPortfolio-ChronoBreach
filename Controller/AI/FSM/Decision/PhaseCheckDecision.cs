using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Phase Check")]
public class PhaseCheckDecision : Decision
{
    public override void OnInitDecide(AIController controller)
    {
        controller.aIFSMVariabls.phaseData = controller.skillController.GetPhaseData(controller.aIFSMVariabls.currentPhaseCount + 1);
    }

    public override bool Decide(AIController controller)
    {
        if (controller.IsDead()) return false;
        if (controller.aIFSMVariabls.phaseData == null) return false;
        if (controller.aIFSMVariabls.currentPhaseCount >= controller.aIVariables.phaseLimit || controller.aIFSMVariabls.phaseData == null ) 
            return false;

        if(controller.aIFSMVariabls.currentHpPercentage <= controller.aIFSMVariabls.phaseData.phasePercent)
        {
            Debug.Log("Check True PhasePercent : " + controller.aIFSMVariabls.currentHpPercentage + "/ " + controller.aIFSMVariabls.phaseData.phasePercent);
            return true;
        }
        return false;
    }
}
