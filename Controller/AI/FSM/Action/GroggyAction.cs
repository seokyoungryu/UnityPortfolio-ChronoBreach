using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Groggy")]
public class GroggyAction : Action
{

    public override void OnEnterAction(AIController controller)
    {
        controller.aiAnim.Play("Reset", 1, 0f);
        controller.SetNavSpeed(0f);
        controller.aIFSMVariabls.isEndGroggy = false;
        controller.aIFSMVariabls.currentCumulativeGroggyDamage = 0f;
        controller.aIFSMVariabls.groggyInAnimFrameTime = GetAnimClipTime(controller.aIVariables.groggyInAnimFullFrame);
        controller.aIFSMVariabls.groggyOutAnimFrameTime = GetAnimClipTime(controller.aIVariables.groggyOutAnimFullFrame);
        controller.aIFSMVariabls.currentGroggyingCount += 1;
        controller.aiConditions.IsGroggying = true;

        controller.StartCoroutine(GroggyProcess_Co(controller));
    }


    public override void Act(AIController controller, float deltaTime)
    {
    }
    public override void OnExitAction(AIController controller)
    {
        controller.aiConditions.IsGroggying = false;
        controller.aiConditions.IsDamaged = false;
        controller.aiConditions.IsForcedDamage = false;
        controller.aIFSMVariabls.currentCumulativeGroggyDamage = 0f;
        controller.aiConditions.CanDefense = false;
        controller.aiConditions.CanStanding = false;
        controller.aiConditions.CanGroggy = false;
        controller.aIFSMVariabls.currentGroggyCoolTimer = 0f;
    }


    private IEnumerator GroggyProcess_Co(AIController controller)
    {
        Debug.Log("½ÇÇà1");
        controller.aiAnim.Play("Groggy_In",3, 0f);

        yield return new WaitForSeconds(controller.aIFSMVariabls.groggyInAnimFrameTime);
        yield return new WaitForSeconds(controller.aIVariables.groggyDuration);

        controller.aiAnim.CrossFade("Groggy_Out", 0.1f);

        yield return new WaitForSeconds(controller.aIFSMVariabls.groggyOutAnimFrameTime);

        controller.aIFSMVariabls.isEndGroggy = true;
        Debug.Log("¾Æ¿ô");

    }

    private float GetAnimClipTime(int animFrame)
    {
        return animFrame * (1f/30f);
    }



}
