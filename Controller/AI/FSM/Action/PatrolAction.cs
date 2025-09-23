using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Patrol")]
public class PatrolAction : Action
{

    public override void OnEnterAction(AIController controller)
    {
        if (controller.wayPointInfos.Count <= 0) return;

        //처음 시작엔 바로 첫번째 웨이포인트로 이동.
        controller.aIVariables.currentNextWayTimer = controller.aIVariables.nextWayWaitTime;
        controller.SetNavSpeed(controller.aIVariables.patrolSpeed);
        controller.nav.stoppingDistance = 2f;
        controller.nav.destination = controller.wayPointInfos[controller.currentWaypointIndex].WayPointTr.position;
    }

    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.wayPointInfos.Count <= 0) return;

        if(controller.nav.remainingDistance <= controller.nav.stoppingDistance && controller.nav.pathPending == false)
        {
            controller.aIVariables.currentNextWayTimer += Time.deltaTime;
            if(controller.aIVariables.currentNextWayTimer >= controller.aIVariables.nextWayWaitTime)
            {
                controller.aIVariables.currentThisWayWaitTimer += Time.deltaTime;
                if (controller.aIVariables.currentThisWayWaitTimer >= controller.wayPointInfos[controller.currentWaypointIndex].ThisPointWaitTime)
                    controller.aIVariables.canNextWayPointTimer = true;

                if (controller.aIVariables.canNextWayPointTimer)
                {
                    controller.aIVariables.canNextWayPointTimer = false;              
                    controller.currentWaypointIndex = (controller.currentWaypointIndex + 1) % controller.wayPointInfos.Count;
                    controller.nav.destination = controller.wayPointInfos[controller.currentWaypointIndex].WayPointTr.position;
                    controller.aIVariables.currentNextWayTimer = 0;
                    controller.aIVariables.currentThisWayWaitTimer = 0f;
                }

            }
        }
    }
}
