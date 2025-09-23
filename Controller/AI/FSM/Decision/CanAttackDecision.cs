using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Can Attack")]
public class CanAttackDecision : Decision
{
    public AttackAction attackAction;
    public override void OnInitDecide(AIController controller)
    {
        controller.aIFSMVariabls.targetAngle = 0f;
        controller.aIFSMVariabls.detectTargetCount = 0;
        if (controller.aIFSMVariabls.targetColliders.Length <= 0 || controller.aIFSMVariabls.targetColliders == null)
            controller.aIFSMVariabls.targetColliders = new Collider[10];

        attackAction.SettingRandomStartAttack(controller);
        if (controller.aiAttackType == AIController.AIAttackType.MELEE)
        {
            controller.aIFSMVariabls.detectAttackRadius = controller.aIFSMVariabls.meleeClip.detectRange;
            controller.aIFSMVariabls.attackRange = controller.aIFSMVariabls.meleeClip.attackRange;
            controller.aIFSMVariabls.attackAngle = controller.aIFSMVariabls.meleeClip.attackAngle;
        }
        else if (controller.aiAttackType == AIController.AIAttackType.RANGE)
        {
            controller.aIFSMVariabls.detectAttackRadius = controller.aIFSMVariabls.rangeClip.canExcuteDistance;
            //controller.aIFSMVariabls.attackRange = controller.aIFSMVariabls.rangeClip.Infos.DamageRange;
            //controller.aIFSMVariabls.attackAngle = controller.aIFSMVariabls.rangeClip.Infos.ExplosionAngle;
        }
    }


    public override bool Decide(AIController controller)
    {
        if (controller.aiConditions.currentCombatType != CurrentCombatType.ATTACK ||
            controller.aiConditions.IsDamaged  || !controller.aiConditions.CanAttacking) return false;

        if (controller.aIVariables.target)
        {
            if (controller.IsDetectObstacle(controller.damagedPosition, controller.aIVariables.target.damagedPosition))
                return false;
        }

        if (CheckTargetInAttackRange(controller, ref controller.aIFSMVariabls.detectTargetCount,
                                                     controller.aIFSMVariabls.detectAttackRadius,
                                                     controller.aIFSMVariabls.targetColliders))
        {
            for (int i = 0; i < controller.aIFSMVariabls.targetColliders.Length; i++)
            {
                if (controller.aIFSMVariabls.targetColliders[i] == null) continue;
                if (controller.aIFSMVariabls.targetColliders[i].gameObject == controller.gameObject)
                    continue;

                if (controller.aIFSMVariabls.targetColliders[i] != null )
                {
                    if (CheckTargetInAngle(controller, controller.aIFSMVariabls.targetColliders[i].transform))
                        return true;
                }
            }
        }
        return false;
    }



    public Vector3 TargetDirection(AIController controller, float angle)
    {
        angle += controller.transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }


}
