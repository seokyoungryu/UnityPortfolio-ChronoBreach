using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Alert")]
public class AlertAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        controller.aIVariables.currentSightRange = controller.aIVariables.alertSightRange;
        controller.aIFSMVariabls.maxAlertTime = Random.Range(controller.aIVariables.minAlertTime, controller.aIVariables.maxAlertTime);
    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.aIVariables.currentAlertTimer < controller.aIFSMVariabls.maxAlertTime)
            controller.aIVariables.currentAlertTimer += deltaTime;
        else
            controller.aIFSMVariabls.isReachedMaxAlertTime = true;
    }



    public override void OnExitAction(AIController controller)
    {
        controller.aIVariables.currentSightRange = controller.aIVariables.sightRange;
    }
}
