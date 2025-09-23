using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Can Skill")]
public class CanSkillDecision : Decision
{
    public override void OnInitDecide(AIController controller)
    {
        controller.aIFSMVariabls.skillTargetAngle = 0f;
        controller.aIFSMVariabls.skillDetectTargetCount = 0;
        controller.aIFSMVariabls.skillTargetColliders = new Collider[1];
    }

    public override bool Decide(AIController controller)
    {
        if (controller == null) return false;
        if (controller.aIFSMVariabls.currentSkillData == null || controller.aIFSMVariabls.currentSkillData.skillClip == null) return false;
        if (controller.aiConditions.currentCombatType != CurrentCombatType.SKILL || controller.skillController.IsAllSkillCoolTime())
            return false;

        Debug.Log("CanSkillDec : " + controller.aIFSMVariabls.detectSkillRadius);

        if (CheckTargetInAttackRange(controller,ref controller.aIFSMVariabls.skillDetectTargetCount,
                                                    controller.aIFSMVariabls.detectSkillRadius,
                                                    controller.aIFSMVariabls.skillTargetColliders))
        {
            if (controller.aIVariables.Target != null)
                return true;

            for (int i = 0; i < controller.aIFSMVariabls.skillTargetColliders.Length; i++)
                if (controller.aIFSMVariabls.skillTargetColliders[i] != null && controller.aIFSMVariabls.skillTargetColliders[i].transform != null)
                    if (CheckTargetInAngle(controller, controller.aIFSMVariabls.skillTargetColliders[i].transform))
                        return true;
        }
        return false;
    }


}
