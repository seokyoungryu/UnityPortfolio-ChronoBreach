using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Find Target")]
public class FindTargetDecision : Decision
{

    public override void OnInitDecide(AIController controller)
    {
        controller.aIVariables.currentSightRange = controller.aIVariables.sightRange;
    }

    public override bool Decide(AIController controller)
    {
        if (CheckTargetInSight(controller))
        {
            if (controller.aIVariables.target != null)
            {
                BaseController currentTarget = controller.aIVariables.target;
                if (!currentTarget.CheckControllerIsDead(currentTarget) && currentTarget.CanDetect())
                {
                    if (!controller.IsDetectObstacle(controller.damagedPosition, currentTarget.damagedPosition))
                    {
                        Vector3 dirToCurrent = currentTarget.transform.position - controller.transform.position;
                        float distanceToCurrent = dirToCurrent.magnitude;

                        if (dirToCurrent.y >= controller.aIFSMVariabls.MinLimitHeight && dirToCurrent.y <= controller.aIFSMVariabls.MaxLimitHeight)
                        {
                            dirToCurrent.y = 0f;
                            dirToCurrent.Normalize();

                            float angleToCurrent = Vector3.Angle(controller.transform.forward, dirToCurrent);
                            if (Mathf.Abs(angleToCurrent) <= (controller.aIVariables.sightAngle / 2f) && distanceToCurrent <= 6f)
                                return true;
                        }
                    }
                }
            }

            float nearDistance = Mathf.Infinity;
            foreach (Collider target in controller.checkTargetColliders)
            {
                if (target == null) continue;
                if (target.gameObject == controller.gameObject) continue;
                BaseController baseController = target.GetComponent<BaseController>();
                if (baseController == null) continue;
                if (baseController.CheckControllerIsDead(baseController) || !baseController.CanDetect())
                    continue;
                if (controller.IsDetectObstacle(controller.damagedPosition, baseController.damagedPosition))
                    continue;

                Vector3 dir = target.transform.position - controller.transform.position;
                if (dir.y < controller.aIFSMVariabls.MinLimitHeight || dir.y > controller.aIFSMVariabls.MaxLimitHeight)
                    continue;

                dir.y = 0f;
                dir.Normalize();

                float angle = Vector3.Angle(controller.transform.forward, dir);
                //float sightAngle = controller.aIVariables.Target == null ? 360f : (controller.aIVariables.sightAngle / 2f);
                if (Mathf.Abs(angle) <= (controller.aIVariables.sightAngle / 2f))
                {
                    float distance = Vector3.Distance(controller.transform.position, target.transform.position);
                    if (distance < nearDistance)
                    {
                        // if (controller.IsDetectObstacle(controller.damagedPosition, target.GetComponent<BaseController>().damagedPosition))
                        //     continue;
                        nearDistance = distance;
                        controller.aIVariables.SetTarget(target.GetComponent<BaseController>());
                        //Debug.Log("Find Decision Target : " + controller.aIVariables.target);

                    }
                }
            }

            if (controller.aIVariables.target)
                return true;
        }
        return false;
    }
}
