using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Is Reached Target Position", fileName = "IsReachedTargetPositionDecision")]
public class IsReachedTargetPositionDecision : Decision
{
    [SerializeField] private bool ifReachedInitTargetVector = false;

    private float distance = 0f;
    public override bool Decide(AIController controller)
    {
        if (ifReachedInitTargetVector)
        {
            distance = (controller.aIVariables.targetVector - controller.transform.position).magnitude;
            if ((controller.nav.remainingDistance <= 0.35f && distance <= 0.05f) || distance <= 1f)
            {
               //controller.aIVariables.targetVector = Vector3.zero;
                return true;
            }
        }
        else
        {
            if (controller.nav.remainingDistance <= 0.1f)
                return true;
        }

        return false;

    }

}
