using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "AI/Decisions/Can Move To Target", fileName = "CanMoveToTargetDecision")]
public class CanMoveToTargetDecision : Decision
{
    enum MoveToTargetCheckType
    {
        ONLY_TARGET = 0,
        ONLY_TARGET_VECTOR =1,
        ONLY_FOLLOW_TARGET =2,
        BOTH_TARGET_AND_VECTOR =3,
    }

    [SerializeField] private MoveToTargetCheckType checkType;

    public override bool Decide(AIController controller)
    {
        Debug.Log("DIs :" + Vector3.Distance(controller.transform.position, controller.aIVariables.targetVector));

         if (checkType == MoveToTargetCheckType.ONLY_TARGET_VECTOR && controller.aIVariables.targetVector != Vector3.zero &&
            Vector3.Distance(controller.transform.position, controller.aIVariables.targetVector) > 1f)
            return true;
        else if(checkType == MoveToTargetCheckType.ONLY_TARGET && controller.aIVariables.Target != null)
            return true;
        else if(checkType == MoveToTargetCheckType.ONLY_FOLLOW_TARGET&& controller.aIVariables.followTarget != null)
            return true;
        else if (checkType == MoveToTargetCheckType.BOTH_TARGET_AND_VECTOR)
        {

            return true;
        }

        return false;

    }
}


//정리하기.
//1. bool 형태로 강제 이동 해야됨.음..
//2. 강제이동이 필요할때 MoveToTarget variables에 강제이동 = flase하고.
//3. decision으로 강제이동이 있는지 검사. 있다면 -> 강제이동음....