using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/MoveToTarget")]
public class MoveToTargetAction : Action
{
    enum AIMoveTargetType
    {
        TARGET =0,
        FOLLOWTARGET= 1,
        TARGET_AND_VECTOR = 2,
        TARGET_AND_FOLLOWTARGET =3,
        ONLY_TARGET_VECTOR = 4,
        VECTOR_AND_FOLLOWTARET = 5,
    }

    [SerializeField] private AIMoveTargetType targetMoveType;
    public MoveToTargetType moveType = MoveToTargetType.WALK;

    public enum MoveToTargetType
    {
        WALK = 0,
        RUN = 1,
        BOTH = 2,
    }

    public override void OnEnterAction(AIController controller)
    {
        controller.nav.updatePosition = true;
        controller.nav.updateRotation = true;
        SettingNavSpeed(controller, moveType);
        controller.aIFSMVariabls.transitionTime = 0.5f;
        controller.nav.velocity = Vector3.zero;
        controller.nav.stoppingDistance = controller.aIVariables.stopDistance;
    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.aIVariables.Target == null && controller.aIVariables.targetVector == Vector3.zero 
            && controller.aIVariables.followTarget == null) return;

        controller.aIFSMVariabls.currentTransitionTImer += deltaTime;

        if (controller.aIFSMVariabls.currentTransitionTImer >= controller.aIFSMVariabls.transitionTime)
        {
            controller.aIFSMVariabls.currentTransitionTImer = 0f;
            MovingAction(controller, moveType);
        }

        if (targetMoveType == AIMoveTargetType.TARGET)
        {
            if (controller.aIVariables.Target != null && !controller.aIVariables.target.IsDead())
                controller.nav.destination = controller.aIVariables.Target.transform.position;
        }
        else if (targetMoveType == AIMoveTargetType.TARGET_AND_VECTOR )
        {
            if (controller.aIVariables.target != null)
            {
                controller.nav.destination = controller.aIVariables.target.transform.position;
                Debug.Log("여기 1");
            }
            else  if(controller.aIVariables.targetVector != Vector3.zero)
            {
                Debug.Log("여기2 ");
                controller.nav.destination = controller.aIVariables.targetVector;
            }
            Debug.Log("여기3 ");

        }
        else if (targetMoveType == AIMoveTargetType.FOLLOWTARGET)
        {
            if (controller.aIVariables.followTarget != null)
                controller.nav.destination = controller.aIVariables.followTarget.position;
        }
        else if (targetMoveType == AIMoveTargetType.TARGET_AND_FOLLOWTARGET)
        {
            if (controller.aIVariables.Target != null && !controller.aIVariables.target.IsDead())
                controller.nav.destination = controller.aIVariables.target.transform.position;
            else
            {
                if (controller.aIVariables.followTarget != null)
                    controller.nav.destination = controller.aIVariables.followTarget.position;
            }
        }
        else if(targetMoveType == AIMoveTargetType.ONLY_TARGET_VECTOR && controller.aIVariables.targetVector != Vector3.zero)
        {
            controller.nav.destination = controller.aIVariables.targetVector;
        }
        else if (targetMoveType == AIMoveTargetType.VECTOR_AND_FOLLOWTARET)
        {
            if (controller.aIVariables.targetVector != Vector3.zero)
                controller.nav.destination = controller.aIVariables.targetVector;
            else if (controller.aIVariables.followTarget != null)
                controller.nav.destination = controller.aIVariables.followTarget.position;
        }

    }

    public override void OnExitAction(AIController controller)
    {
        controller.nav.updatePosition = true;
        controller.nav.updateRotation = true;

    }

    private void SettingNavSpeed(AIController controller ,MoveToTargetType moveType)
    {
        if(controller.aiConditions.IsForceRunning)
        {
            controller.SetNavSpeed(controller.aIVariables.runSpeed);
            return;
        }

        switch (moveType)
        {
            case MoveToTargetType.WALK:
                controller.SetNavSpeed(controller.aIVariables.walkSpeed);
                break;
            case MoveToTargetType.RUN:
                controller.SetNavSpeed(controller.aIVariables.runSpeed);
                break;
            case MoveToTargetType.BOTH:
                controller.SetNavSpeed(controller.aIVariables.walkSpeed);
                break;
        }
    }


    private void MovingAction(AIController controller, MoveToTargetType moveType)
    {
        if (controller.aIVariables.Target == null && controller.aIVariables.followTarget == null) return;
        if (controller.aiConditions.IsForceRunning)
        {
            controller.SetNavSpeed(controller.aIVariables.runSpeed);
            return;
        }
        if (moveType == MoveToTargetType.BOTH)
        {
            if (targetMoveType == AIMoveTargetType.TARGET)
                controller.aIFSMVariabls.distance = (controller.aIVariables.Target.transform.position - controller.transform.position).magnitude;
            else if (targetMoveType == AIMoveTargetType.ONLY_TARGET_VECTOR)
                controller.aIFSMVariabls.distance = (controller.aIVariables.targetVector - controller.transform.position).magnitude;
            else if (targetMoveType == AIMoveTargetType.FOLLOWTARGET)
                controller.aIFSMVariabls.distance = (controller.aIVariables.followTarget.position - controller.transform.position).magnitude;
            else if (targetMoveType == AIMoveTargetType.TARGET_AND_FOLLOWTARGET)
            {
                if (controller.aIVariables.target != null) controller.aIFSMVariabls.distance = (controller.aIVariables.Target.transform.position - controller.transform.position).magnitude;
                else if (controller.aIVariables.followTarget != null) controller.aIFSMVariabls.distance = (controller.aIVariables.followTarget.position - controller.transform.position).magnitude;
            }
            else if (targetMoveType == AIMoveTargetType.TARGET_AND_VECTOR)
            {
                if (controller.aIVariables.target != null)
                {
                    controller.aIFSMVariabls.distance = (controller.aIVariables.Target.transform.position - controller.transform.position).magnitude;
                }
                else if (controller.aIVariables.targetVector != Vector3.zero) controller.aIFSMVariabls.distance = (controller.aIVariables.targetVector - controller.transform.position).magnitude;

            }
            else if (targetMoveType == AIMoveTargetType.VECTOR_AND_FOLLOWTARET)
            {
                if (controller.aIVariables.targetVector != Vector3.zero) controller.aIFSMVariabls.distance = (controller.aIVariables.targetVector - controller.transform.position).magnitude;
                else if (controller.aIVariables.followTarget != null) controller.aIFSMVariabls.distance = (controller.aIVariables.followTarget.position - controller.transform.position).magnitude;

            }

            if (controller.aIFSMVariabls.distance >= controller.aIVariables.runSightRange)
                controller.SetNavSpeed(controller.aIVariables.runSpeed);
            else
                controller.SetNavSpeed(controller.aIVariables.walkSpeed);
        }


    }


}
