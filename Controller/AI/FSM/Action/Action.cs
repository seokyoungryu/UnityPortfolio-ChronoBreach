using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    protected float damageAngle = 0f;
    protected float damageRange = 0f;


    public virtual void OnEnterAction(AIController controller)
    {

    }

    public abstract void Act(AIController controller, float deltaTime);

    public virtual void OnExitAction(AIController controller)
    {

    }

    protected BaseController FindNearEnemy(AIController controller,float range ,ref int count, ref Collider[] colliders)
    {
        count = Physics.OverlapSphereNonAlloc(controller.transform.position, range, colliders, controller.targetLayer);

        if (count > 0)
        {
            controller.SortFindEmenyByNearDistance(controller.transform, ref colliders);
            for (int i = 0; i < count; i++)
            {
                if (colliders[i].gameObject == controller.gameObject) continue;
                if (CheckTargetInAngle(controller, colliders[i].transform, controller.aIFSMVariabls.DetectNearAngle))
                {
                    BaseController retC = GetController(colliders[i]);
                   
                    if (retC != null && !retC.CheckControllerIsDead(retC))
                            return retC;
                }    
            }
        }

        return null;
    }

    protected BaseController GetController(Collider collider)
    {
        if (collider.GetComponent<BaseController>() != null)
            return collider.GetComponent<BaseController>();
        return null;
    }

    protected void Damage(AIController attacker , BaseController targetController,bool isSkill,int index ,float dmg, AttackStrengthType atkStrengthType)
    {
        (bool, float) dmgValue = attacker.GetDamageValue(true);

        if (targetController is PlayerStateController)
        {
            PlayerStateController player = targetController as PlayerStateController;
            if (player.Conditions.IsCounting || player.Conditions.IsDetectParry)
                player.GetState<CounterAttackState>().CounterSuccess(attacker);
            else if (player.Conditions.CanDamaged())
                player.Damaged(dmgValue.Item2 * dmg , attacker, dmgValue.Item1, isSkill, atkStrengthType);
        }
        else if (targetController is AIController)
            targetController.Damaged(dmgValue.Item2 * dmg, attacker, dmgValue.Item1, isSkill, atkStrengthType);
    }


    public bool CheckTargetInHeight(AIController owner, Transform target)
    {
        float height = target.position.y - owner.transform.position.y;
        if (height >= owner.aIFSMVariabls.MinLimitHeight && height <= owner.aIFSMVariabls.MaxLimitHeight)
            return true;
        return false;
    }


    protected bool FindCanDamagedToTarget(AIController controller, bool isSkill, int index)
    {
        controller.ResetCheckTargetInAttackDamaged();
        controller.aIFSMVariabls.canDamageEnemy.Clear();
        float attackRange = isSkill ? controller.aIFSMVariabls.skillRange[index] : controller.aIFSMVariabls.attackRange[index];

        FindNearEnemy(controller, attackRange, ref controller.checkDetectAttackTargetCount, ref controller.checkDetectAttackTargetColliders);

        if (controller.checkDetectAttackTargetCount > 0)
        {
            SetCanDamagedSetting(controller, isSkill, index);
            for (int i = 0; i < controller.checkDetectAttackTargetCount; i++)
            {
                if (controller.checkDetectAttackTargetColliders[i] == null) continue;
                if (controller.checkDetectAttackTargetColliders[i].gameObject == controller.gameObject)
                    continue;

                if(Vector3.Distance(controller.transform.position, controller.checkDetectAttackTargetColliders[i].transform.position) <= damageRange)
                {
                    if (CheckTargetInAngle(controller, controller.checkDetectAttackTargetColliders[i].transform, damageAngle))
                    {
                        BaseController canDamageObject = controller.checkDetectAttackTargetColliders[i].GetComponent<BaseController>();
                        if (canDamageObject != null)
                            controller.aIFSMVariabls.canDamageEnemy.Add(canDamageObject);
                    }
                }
            }

            if (controller.aIFSMVariabls.canDamageEnemy.Count > 0)
                return true;
        }
        return false;
    }


    public bool CheckTargetInAngle(AIController owner, Transform target, float angle)
    {
         Vector3 dir = target.position - owner.transform.position;
        if (dir.y < owner.aIFSMVariabls.MinLimitHeight || dir.y > owner.aIFSMVariabls.MaxLimitHeight)
            return false;

         dir.y = 0f;
         dir.Normalize();

        if (Mathf.Abs(Vector3.Angle(dir, owner.transform.forward)) <= angle / 2f)
            return true;

        return false;
    }

 

    private void SetCanDamagedSetting(AIController controller ,bool isSkill, int index)
    {
        if (isSkill == false)
        {
             damageAngle = (controller.aIFSMVariabls.attackAngle.Count < index) ? controller.aIFSMVariabls.attackAngle[0]
                : controller.aIFSMVariabls.attackAngle[index] == 0 ? controller.aIFSMVariabls.attackAngle[0] : controller.aIFSMVariabls.attackAngle[index];
             damageRange = (controller.aIFSMVariabls.attackRange.Count < index) ? controller.aIFSMVariabls.attackRange[0]
                 : controller.aIFSMVariabls.attackRange[index] == 0 ? controller.aIFSMVariabls.attackRange[0] : controller.aIFSMVariabls.attackRange[index];
        }
        else
        {
            damageAngle = (controller.aIFSMVariabls.skillAngle.Count < index) ? controller.aIFSMVariabls.skillAngle[0]
                : controller.aIFSMVariabls.skillAngle[index] == 0 ? controller.aIFSMVariabls.skillAngle[0] : controller.aIFSMVariabls.skillAngle[index];
            damageRange = (controller.aIFSMVariabls.skillRange.Count < index) ? controller.aIFSMVariabls.skillRange[0] 
                : controller.aIFSMVariabls.skillRange[index] == 0 ? controller.aIFSMVariabls.skillRange[0] : controller.aIFSMVariabls.skillRange[index];
        }

    }

}
