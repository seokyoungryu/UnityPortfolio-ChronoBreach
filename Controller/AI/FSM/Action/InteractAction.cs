using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/Interact", fileName ="InteractAction")]
public class InteractAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        if (controller.npcController == null)
        {
            controller.aiConditions.IsInteract = false;
            return;
        }

        controller.SetNavSpeed(0f);
        controller.aIFSMVariabls.currentCumulativeGroggyDamage = 0f;
        controller.aIFSMVariabls.interactOriginRotation = controller.transform.eulerAngles;
        controller.aIFSMVariabls.canExitInteractAction = false;

        if (controller.npcController.NpcFunction.interactByTargetTr != null)
        {
            Vector3 dir = controller.npcController.NpcFunction.interactByTargetTr.position - controller.transform.position;
            dir.y = 0f;
            dir.Normalize();
            controller.aIFSMVariabls.interactTargetRotation = Quaternion.LookRotation(dir).eulerAngles;
        }

    }


    public override void Act(AIController controller, float deltaTime)
    {
        if(controller.aiConditions.IsInteract)
        {
            SmoothRotateTarget(controller,controller.aIFSMVariabls.interactTargetRotation, true);
        }
        else if(!controller.aiConditions.IsInteract && !controller.aIFSMVariabls.canExitInteractAction)
        {
            if (SmoothRotateTarget(controller, controller.aIFSMVariabls.interactOriginRotation, false))
                controller.aIFSMVariabls.canExitInteractAction = true;
        }
    }


    public override void OnExitAction(AIController controller)
    {
        controller.aIFSMVariabls.canExitInteractAction = false;
    }

    private bool SmoothRotateTarget(AIController controller ,Vector3 targetRot, bool isFastSmoothValue )
    {
        // controller.transform.eulerAngles = Vector3.Lerp(controller.transform.eulerAngles, targetRot, (isFastSmoothValue ? 7f : 3f) * Time.deltaTime);
        controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, Quaternion.Euler(targetRot), (isFastSmoothValue ? 7f : 4f) * Time.deltaTime);


        if (Quaternion.Angle(controller.transform.rotation, Quaternion.Euler(targetRot)) <= 0.3f)
            return true;
        return false;
    }


}
