using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Decision : ScriptableObject
{
    public virtual void OnInitDecide(AIController controller)
    {
    }

    public abstract bool Decide(AIController controller);

    public bool CheckTargetInSight(AIController controller)
    {
        controller.ResetCheckTargetInSight();
        controller.checkInSightTargetCount = Physics.OverlapSphereNonAlloc(controller.transform.position, 
                                                                           controller.aIVariables.currentSightRange, 
                                                                           controller.checkTargetColliders,
                                                                           controller.targetLayer);

        return controller.checkInSightTargetCount > 0;
    }

    public bool CheckTargetInAttackRange(AIController controller,ref int dectCount , float range, Collider[] retColliders)
    {
        if (range <= 0f)
            return false;

        dectCount = Physics.OverlapSphereNonAlloc(controller.transform.position, range, retColliders, controller.targetLayer);
       
        if (dectCount > 0)
            return true;
   
        return false;
    }

     public bool CheckTargetInAngle(AIController controller,Transform target)
     {
        BaseController baseController = target.GetComponent<BaseController>();

        if (baseController.CheckControllerIsDead(baseController) || !baseController.CanDetect()) return false;
        if (controller.gameObject == target.gameObject)
        {
            controller.aIVariables.target = null;
            return false;
        }

        Vector3 targetDir = (target.position - controller.transform.position).normalized;
        controller.aIFSMVariabls.targetAngle = Vector3.Angle(controller.transform.forward, targetDir);

        if (Mathf.Abs(controller.aIFSMVariabls.targetAngle) <= controller.aIVariables.sightAngle / 2f)
        {
            controller.aIVariables.SetTarget(baseController);
            return true;
         }

        return false;
     }


    public virtual void DrawDecisionGizmos(AIController controller) { }

}
