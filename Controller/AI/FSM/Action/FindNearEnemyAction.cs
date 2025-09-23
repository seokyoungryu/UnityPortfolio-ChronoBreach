using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Find Near Enemy", fileName = "FindNearEnemyAction")]
public class FindNearEnemyAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        if (controller.gameObject.layer == 11)
        {
            Debug.Log("FindNear In!");
        }
        controller.SetNavSpeed(0f);
        controller.nav.velocity = Vector3.zero;
        controller.myRigid.velocity = Vector3.zero;

        controller.aIFSMVariabls.CanFindNearEnemy = true;
        controller.aIFSMVariabls.IsEndNearAction = false;
        controller.aiAnim.SetBool("IsFindNearEnemy", true);
        controller.aIFSMVariabls.CurrentNearDetectTimer = 0f;
        controller.aiConditions.CanRest = false;

        if (controller.aIFSMVariabls.WaitCoroutine != null)
            controller.StopCoroutine(controller.aIFSMVariabls.WaitCoroutine);

        controller.aIFSMVariabls.WaitCoroutine = WaitTime(controller);
        controller.StartCoroutine(controller.aIFSMVariabls.WaitCoroutine);
    }

    public override void Act(AIController controller, float deltaTime)
    {
        //|| controller.aIVariables.Target != null
        if (!controller.aIFSMVariabls.CanFindNearEnemy || controller.aIVariables.Target != null || controller.aIFSMVariabls.IsEndNearAction)
            return ;

        controller.aIFSMVariabls.CurrentNearDetectTimer += Time.deltaTime ;
        if(controller.aIFSMVariabls.CurrentNearDetectTimer >= controller.aIFSMVariabls.DetectNearTime || controller.aIVariables.Target != null)
            controller.aIFSMVariabls.IsEndNearAction = true;

        BaseController target = FindNearEnemy(controller, controller.aIFSMVariabls.DetectNearRange, ref controller.aIFSMVariabls.nearDetectCount, ref controller.aIFSMVariabls.nearTargetColliders);
        if (target != null && target.CanDetect())
        {
            if (CheckTargetInHeight(controller, target.transform) && !controller.IsDetectObstacle(controller.damagedPosition, target.damagedPosition))
            {
                controller.aIVariables.SetTarget(target);
            }

        }
        
    }

    public override void OnExitAction(AIController controller)
    {
        controller.aiAnim.SetBool("IsFindNearEnemy", false);
        controller.aIFSMVariabls.IsEndNearAction = true;
        controller.aIFSMVariabls.CanFindNearEnemy = false;
        controller.StopCoroutine(controller.aIFSMVariabls.WaitCoroutine);
        controller.aIFSMVariabls.WaitCoroutine = null;
        controller.aiConditions.CanRest = true;
        Debug.Log("FindNear Target : " + controller.aIVariables.target);
    }


    private IEnumerator WaitTime(AIController controller)
    {
        Debug.Log("Wait Ω√¿€!");

        yield return new WaitForSeconds(controller.aIFSMVariabls.FindNearWaitTime);
        controller.aIFSMVariabls.CanFindNearEnemy = true;
        Debug.Log("Wait ≥°!!!");

    }
}
