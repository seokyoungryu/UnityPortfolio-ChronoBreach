using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Set Current Combat Tpye")]
public class SetCurrentCombatTypeAction : Action
{

    public override void OnEnterAction(AIController controller)
    {
        controller.aIFSMVariabls.percentage = Random.Range(0, 100f);
        controller.aIFSMVariabls.attackPercentage = 0f;
        controller.aIFSMVariabls.skillPercentage = 0f;

        if (controller.skillController.GetOwnSkillCount() <= 0 || controller.skillController.IsAllSkillCoolTime())
        {
            controller.aIFSMVariabls.attackPercentage = 100f;
            controller.aIFSMVariabls.skillPercentage = 0f;
        }
        else
        {
            controller.aIFSMVariabls.attackPercentage = controller.aIVariables.attackCombatPercentage;
            controller.aIFSMVariabls.skillPercentage = controller.aIVariables.skillCombatPercentage;
        }
        SetCurrentCombatType(controller);
        if (controller.aiConditions.currentCombatType == CurrentCombatType.SKILL)
        {
            controller.aIFSMVariabls.skillCount = controller.skillController.GetOwnSkillCount();
            controller.aIFSMVariabls.currentSkillData = controller.skillController.GetCanUseSkillData();
            if (controller.aIFSMVariabls.currentSkillData?.skillClip == null)
                controller.aiConditions.currentCombatType = CurrentCombatType.ATTACK;
            SettingClips(controller);
        }
    }

    public override void Act(AIController controller, float deltaTime)
    {
        
    }



    private void SetCurrentCombatType(AIController controller)
    {
        if (controller.aIFSMVariabls.percentage <= controller.aIFSMVariabls.attackPercentage)
            controller.aiConditions.currentCombatType = CurrentCombatType.ATTACK;
        else
            controller.aiConditions.currentCombatType = CurrentCombatType.SKILL;
        
    }

    private void SettingClips(AIController controller)
    {
        if (controller.aIFSMVariabls.currentSkillData?.skillClip == null) return;

        if (controller.aIFSMVariabls.currentSkillData.skillClip is AttackSkillClip)
        {
            controller.aIFSMVariabls.currentSkillType = SkillType.ATTACK;
            controller.aIFSMVariabls.currentAttackClip = controller.aIFSMVariabls.currentSkillData.skillClip as AttackSkillClip;
            controller.aIFSMVariabls.detectSkillRadius = controller.aIFSMVariabls.currentAttackClip.TargetDetectRange;
            controller.aIFSMVariabls.skillDamage = controller.aIFSMVariabls.currentAttackClip.AttackDamage;
            controller.aIFSMVariabls.skillAngle = controller.aIFSMVariabls.currentAttackClip.AttackAngle;
            controller.aIFSMVariabls.skillRange = controller.aIFSMVariabls.currentAttackClip.AttackRange;
            controller.aIFSMVariabls.skillSpeed = controller.aIFSMVariabls.currentAttackClip.SkillAnimSpeed;
           // Debug.Log("스킬 스피드 : " + controller.aIFSMVariabls.currentAttackClip.SkillAnimSpeed);

        }
        else if (controller.aIFSMVariabls.currentSkillData.skillClip is MagicSkillClip)
        {      
            controller.aIFSMVariabls.currentSkillType = SkillType.MAGIC;
            controller.aIFSMVariabls.currentMagicClip = controller.aIFSMVariabls.currentSkillData.skillClip as MagicSkillClip;
            controller.aIFSMVariabls.detectSkillRadius = controller.aIFSMVariabls.currentMagicClip.canExcuteDistance;
            controller.aIFSMVariabls.skillSpeed = controller.aIFSMVariabls.currentMagicClip.animationSpeed;
            // controller.aIFSMVariabls.skillDamage = controller.aIFSMVariabls.currentMagicClip.Infos.Damage;
            //controller.aIFSMVariabls.skillAngle = controller.aIFSMVariabls.currentMagicClip.Infos.ExplosionAngle;
            //controller.aIFSMVariabls.skillRange = controller.aIFSMVariabls.currentMagicClip.canExcuteDistance;

        }
        else if (controller.aIFSMVariabls.currentSkillData.skillClip is BuffSkillClip)
        {
            controller.aIFSMVariabls.currentBuffClip = controller.aIFSMVariabls.currentSkillData.skillClip as BuffSkillClip;
            controller.aIFSMVariabls.currentSkillType = SkillType.BUFF;
            controller.aIFSMVariabls.detectSkillRadius = controller.aIFSMVariabls.currentBuffClip.detectRange;
        }
    }


}
