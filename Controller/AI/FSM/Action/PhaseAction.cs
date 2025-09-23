using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Phase")]
public class PhaseAction : Action
{
 
    public override void OnEnterAction(AIController controller)
    {
        // Debug.Log("------------------------------------------------------------");
        // Debug.Log("∆‰¿Ã¡Ó In : " + controller.aIFSMVariabls.phaseData.phaseName);
        controller.aiAnim.Play("Reset", 1, 0f);
        controller.aIFSMVariabls.isPhaseDone = false;
        controller.StartCoroutine(PhaseProcess(controller));
    }

    public override void Act(AIController controller, float deltaTime)
    {
    }

    public override void OnExitAction(AIController controller)
    {
        controller.aiConditions.IsDamaged = false;
        controller.aiConditions.IsForcedDamage = false;

        controller.aiConditions.CanGroggy = false;
        controller.aIFSMVariabls.currentCumulativeGroggyDamage = 0f;
        controller.aIFSMVariabls.currentCumulativeTimer = 0f;
    }

    IEnumerator PhaseProcess(AIController controller)
    {
        AIPhaseAttackData phase = controller.aIFSMVariabls.phaseData;
        controller.aiAnim.SetFloat(AnimatorKey.PhaseSpeed, phase.animationSpeed);
        CommonUIManager.Instance.ExcuteGlobalBattleNotifer(phase.globalNotifier, 5f);
        controller.aiAnim.Play(phase.phaseAnimationClipName);
        controller.skillController.UnlockPhaseSkill(phase.phaseCount);
        float phaseTime = controller.skillController.GetPhaseData(phase.phaseCount).phaseAnimationEndFrame * (1f / (phase .animClip.frameRate * phase.animationSpeed));
        controller.skillController.ApplyPhaseUseableObjs(phase.phaseCount);
        Debug.Log("PH : " + phase.phaseName + " , " + phase.waitEndTime);

        yield return new WaitForSeconds(phaseTime + phase.waitEndTime);
        Debug.Log("PH Time : " + phaseTime + phase.waitEndTime);

        controller.aIFSMVariabls.currentPhaseCount = phase.phaseCount;
        controller.aIFSMVariabls.phaseData = null;
        controller.aIFSMVariabls.isPhaseDone = true;
    }
}
