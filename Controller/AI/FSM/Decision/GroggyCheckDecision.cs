using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is Groggy Check")]
public class GroggyCheckDecision : Decision
{
    public override void OnInitDecide(AIController controller)
    {
        controller.aIFSMVariabls.groggyCumulativeHpValue = controller.aiStatus.CurrentMaxHealth * (controller.aIVariables.groggyingHpPercent * 0.01f);

    }

    public override bool Decide(AIController controller)
    {
        CheckGroggyCoolTime(controller);
        if (GroggyCumulativeCheck(controller) && controller.aiConditions.CanGroggy)
            return true;
        return false;
    }


    private void CheckGroggyCoolTime(AIController controller)
    {
        if (controller.aiConditions.CanGroggy) return;

        controller.aIFSMVariabls.currentGroggyCoolTimer += Time.deltaTime;
        if (controller.aIFSMVariabls.currentGroggyCoolTimer >= controller.aIVariables.groggyCoolTime)
            controller.aiConditions.CanGroggy = true;
    }


    private bool GroggyCumulativeCheck(AIController controller)
    {
        if (controller.aIFSMVariabls.currentGroggyingCount >= controller.aIVariables.maxGroggyingCount) return false;

        CheckResetCumulativeDamage(controller);
        if(controller.aIFSMVariabls.currentCumulativeGroggyDamage >= controller.aIFSMVariabls.groggyCumulativeHpValue)
            return true;
        else
            return false;
    }

    private void CheckResetCumulativeDamage(AIController controller)
    {
        controller.aIFSMVariabls.currentCumulativeTimer += Time.deltaTime;
        if( controller.aIFSMVariabls.currentCumulativeTimer >= controller.aIVariables.groggyingResetTime)
        {
            controller.aIFSMVariabls.currentCumulativeGroggyDamage = 0f;
            controller.aIFSMVariabls.currentCumulativeTimer = 0f;
        }
    }
}
