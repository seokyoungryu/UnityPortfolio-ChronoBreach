using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Relaxation")]
public class RelaxationAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        controller.aIFSMVariabls.currentIdleTimer = 0f;
        controller.SetNavSpeed(0f);
        controller.nav.velocity = Vector3.zero;

        SetRelaxationChangeTime(controller, controller.aIFSMVariabls.minChangeTime, controller.aIFSMVariabls.maxChangeTime);
        if (controller.aIFSMVariabls.relaxationCount == 1 )
            controller.aiAnim.CrossFade(controller.aIFSMVariabls.relaxationAnimationClipName[0], 0.2f);
    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.aIFSMVariabls.relaxationCount <= 1) return;

        controller.aIFSMVariabls.currentIdleTimer += deltaTime;
        if (controller.aIFSMVariabls.currentIdleTimer >= controller.aIFSMVariabls.relaxationChangeTime)
        {
            int randomRelaxation = Random.Range(0, controller.aIFSMVariabls.relaxationCount);
            controller.aiAnim.CrossFade(controller.aIFSMVariabls.relaxationAnimationClipName[randomRelaxation], 0.2f);
            SetRelaxationChangeTime(controller,controller.aIFSMVariabls.minChangeTime, controller.aIFSMVariabls.maxChangeTime);
        }
    }

    private void SetRelaxationChangeTime(AIController controller,float minTime, float maxTime)
    {
        controller.aIFSMVariabls.relaxationChangeTime = Random.Range(minTime, maxTime);
        controller.aIFSMVariabls.currentIdleTimer = 0;
    }
}

