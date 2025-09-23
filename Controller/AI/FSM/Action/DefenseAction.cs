using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/Defense", fileName ="DefenseAction")]
public class DefenseAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        controller.aiAnim.Play("Reset", 1, 0f);
        controller.aiAnim.SetFloat("ExcuteBlockDefenseSpeed", controller.aIVariables.blockDefenseAnimSpeed);
        controller.nav.velocity = Vector3.zero;
        controller.aIFSMVariabls.blockDefenseAnimTime = GetBlockDefenseFrameToTime(controller);
        controller.aiConditions.IsDefensing = true;
        controller.aiConditions.CanDefense = true;
        controller.HitEffect.ExcuteDefenseColor();

        controller.aIFSMVariabls.CurrentDefenseTimer = 0f;
        controller.aIFSMVariabls.CurrentDefenseCount = 1;
        controller.aIFSMVariabls.isStartBlockDefense = true;
        controller.aIFSMVariabls.isEndBlockDefense = false;
        controller.aiAnim.SetBool("CanDefense", true);
        controller.aiAnim.Play("ExcuteDefense", 3, 0f);
        SoundManager.Instance.PlayUISound(controller.defenseSound[Random.Range(0,controller.defenseSound.Length)]);
        Debug.Log("µðÆæ½º : " + controller.aIFSMVariabls.CurrentDefenseCount);
    }


    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.nav.speed > 0)
            controller.SetNavSpeed(0f);

        controller.aIFSMVariabls.CurrentDefenseTimer += Time.deltaTime;

        if (controller.aIFSMVariabls.CurrentDefenseTimer >= controller.aiStatus.DefensingTime)
        {
            controller.aiConditions.IsDefensing = false;
        }
        
        if (controller.aiStatus.DefenseType == AIDefenseType.COUNT && controller.aIFSMVariabls.CurrentDefenseCount >= controller.aiStatus.DefensePerCount)
        {
            controller.aiConditions.IsDefensing = false;
            //controller.aIFSMVariabls.isEndBlockDefense = true;
        }

        CheckExcuteBlockDefenseTime(controller);
    }


    public override void OnExitAction(AIController controller)
    {
        
        controller.aiAnim.SetBool("CanDefense", false);

        controller.aiConditions.ResetDefenseBool();
        controller.aiConditions.ResetStandingBool();
        controller.aiConditions.IsDamaged = false;
        controller.aIFSMVariabls.IsDefenseCoolTime = true;
        controller.aIFSMVariabls.CurrentDefenseCount = 0;
        controller.aIFSMVariabls.CurrentDefenseTimer = 0f;

        controller.aiStatus.DefenseCoolTime();
    }


    private void CheckExcuteBlockDefenseTime(AIController controller)
    {
        if (controller.aIFSMVariabls.isStartBlockDefense)
        {
            controller.aIFSMVariabls.currentBlockTimer = 0f;
            controller.aIFSMVariabls.isStartBlockDefense = false;
            controller.aIFSMVariabls.isEndBlockDefense = false;
        }

        controller.aIFSMVariabls.currentBlockTimer += Time.deltaTime;
        if (controller.aIFSMVariabls.currentBlockTimer >= controller.aIFSMVariabls.blockDefenseAnimTime)
            controller.aIFSMVariabls.isEndBlockDefense = true;
    }



    private float GetBlockDefenseFrameToTime(AIController controller)
    {
        return controller.aIVariables.blockDefenseAnimFrame * (1f / (30f * controller.aIVariables.blockDefenseAnimSpeed));
    }



}
