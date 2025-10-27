using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Follow")]
public class FollowTargetAction : Action
{
    public FollowType followType = FollowType.NONE;
    public MoveToTargetType moveType = MoveToTargetType.WALK;

    public enum MoveToTargetType
    {
        WALK = 0,
        RUN = 1,
        BOTH = 2,
    }
    public enum FollowType
    {
        NONE = -1,
        UPDATED_FOLLOW =0,
        DELAY_FOLLOW = 1,
    }

    public override void OnEnterAction(AIController controller)
    {
       controller.aIFSMVariabls.currentFollowTimer = 0f;

        controller.nav.velocity = Vector3.zero;
        controller.nav.stoppingDistance = controller.aIVariables.followDistance;
        SettingNavSpeed(controller,moveType);
        SetFollowTime(controller,controller.aIVariables);

    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.aIVariables.followTarget == null) return;

        if (followType == FollowType.DELAY_FOLLOW)
        {
            if (Vector3.Distance(controller.transform.position, controller.aIVariables.followTarget.position) > controller.aIVariables.followDistance)
                controller.aIFSMVariabls.currentFollowTimer += deltaTime;

            if (controller.aIFSMVariabls.currentFollowTimer >= controller.aIFSMVariabls.delayTime)
            {
                controller.aIFSMVariabls.currentFollowTimer = 0f;
                MovingAction(controller, moveType);
                controller.nav.SetDestination(controller.aIVariables.followTarget.position);
            }
        }
        else if(followType == FollowType.UPDATED_FOLLOW)
        {
            controller.aIFSMVariabls.currentFollowTimer += deltaTime;
            if (controller.aIFSMVariabls.currentFollowTimer >= controller.aIFSMVariabls.delayTime)
                if (controller.aIFSMVariabls.currentFollowTimer >= controller.aIFSMVariabls.delayTime)
                {
                    controller.aIFSMVariabls.currentFollowTimer = 0f;
                    MovingAction(controller, moveType);
                    controller.nav.SetDestination(controller.aIVariables.followTarget.position);
                }
        }

    }


    private void SetFollowTime(AIController controller,AIVariables aIVariables)
    {
        switch(followType)
        {
            case FollowType.UPDATED_FOLLOW:
                controller.aIFSMVariabls.delayTime = aIVariables.followUpdateTime;
                break;
            case FollowType.DELAY_FOLLOW:
                controller.aIFSMVariabls.delayTime = aIVariables.followDeleyTime;
                break;
        }
    }
    private void SettingNavSpeed(AIController controller, MoveToTargetType moveType)
    {
        switch (moveType)
        {
            case MoveToTargetType.WALK:
                controller.SetNavSpeed(controller.aIVariables.followWalkSpeed);
                break;
            case MoveToTargetType.RUN:
                controller.SetNavSpeed(controller.aIVariables.followRunSpeed);
                break;
            case MoveToTargetType.BOTH:
                controller.SetNavSpeed(controller.aIVariables.followWalkSpeed);
                break;
        }
    }



    private void MovingAction(AIController controller, MoveToTargetType moveType)
    {
        if (moveType == MoveToTargetType.BOTH)
        {
            if (controller.aIVariables.followTarget == null) return;

            controller.aIFSMVariabls.distance = (controller.transform.position - controller.aIVariables.followTarget.position).magnitude;
            if (controller.aIFSMVariabls.distance >= controller.aIVariables.followRunSight)
                controller.SetNavSpeed(controller.aIVariables.followRunSpeed);
            else controller.SetNavSpeed(controller.aIVariables.followWalkSpeed);
        }
    }
}
