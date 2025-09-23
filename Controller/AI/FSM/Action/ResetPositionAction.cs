using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Reset Position")]
public class ResetPositionAction : Action
{
    public ResetType resetType = ResetType.WALK;
  
    public enum ResetType
    {
        WALK = 0,
        RUN = 1,
        TELEPOTATION = 2,
    }

    public override void OnEnterAction(AIController controller)
    {
        if (controller.gameObject.layer == 11)
        {
            Debug.Log("ResetPosition In!");
        }
        controller.aIFSMVariabls.isArrivePosition = false;
        controller.myRigid.velocity = Vector3.zero;
        controller.nav.stoppingDistance = 0f;
        SetResetTypeSpeed(controller);
    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (resetType == ResetType.WALK || resetType == ResetType.RUN)
        {
            controller.nav.SetDestination(controller.aIFSMVariabls.resetPos);
            if (controller.aIFSMVariabls.isArrivePosition == false)
            {
                if (Vector3.Distance(controller.transform.position, controller.aIFSMVariabls.resetPos) <= 0.5f )
                {
                    Debug.Log("µµÂø");
                    controller.aIFSMVariabls.isArrivePosition = true;
                }
            }
        }
        else
        {
            controller.transform.position = controller.aIFSMVariabls.resetPos;
            controller.aIFSMVariabls.isArrivePosition = true;
        }
    }

    public override void OnExitAction(AIController controller)
    {
        controller.nav.stoppingDistance = controller.aIVariables.stopDistance;
    }

    private void SetResetTypeSpeed(AIController controller)
    {
        switch (resetType)
        {
            case ResetType.WALK:
                controller.SetNavSpeed(controller.aIVariables.walkSpeed);
                break;
            case ResetType.RUN:
                controller.SetNavSpeed(controller.aIVariables.runSpeed);
                break;
            case ResetType.TELEPOTATION:
                controller.SetNavSpeed(0f);
                break;
        }


    }
}
