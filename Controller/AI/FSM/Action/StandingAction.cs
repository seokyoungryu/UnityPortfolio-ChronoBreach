using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Standing", fileName = "StandingAction")]
public class StandingAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        controller.nav.velocity = Vector3.zero;
        controller.aiAnim.Play("Reset", 1, 0f);
        controller.aiAnim.Play("Reset",3,0f);
        controller.aiConditions.IsStanding = true;
        controller.aiConditions.CanStanding = true;
        controller.aiConditions.currentCombatType = CurrentCombatType.ATTACK;
        controller.aiConditions.CanAttacking = false;
        controller.HitEffect.ExcuteStandinfColor();
        controller.aiConditions.CanDefense = false;
        controller.aiConditions.IsDefensing = false;

        controller.StartCoroutine(StandingAnimTime_Co(controller));
        controller.aiStatus.ExtraAtkSpeed += controller.aiStatus.IncreaseStandingAttackSpeed;
        controller.aiStatus.UpdateStats();
        SoundManager.Instance.PlayUISound(controller.standingSound[Random.Range(0, controller.standingSound.Length)]);
        Debug.Log("½ºÅÄµù ENter");

    }


    public override void Act(AIController controller, float deltaTime)
    {
        if (controller.aiStatus.StandingType == AIStandingType.Full_COMBO|| !controller.aiConditions.IsStanding) return;

        if(controller.aiStatus.StandingType == AIStandingType.ATTACK_COUNT)
        {
            if(controller.aIFSMVariabls.attackComboCount >= controller.aiStatus.StandingAttackCount)
            {
                ResetDatas(controller);
                Debug.Log("¿©±â¿È1 : " + controller.aiConditions.IsStanding);
            }
        }
    }


    public override void OnExitAction(AIController controller)
    {
        controller.aiAnim.Play("Reset",3);
        if (controller.aiConditions.IsStanding)
            ResetDatas(controller);

        controller.aIFSMVariabls.IsStandingCoolTime = true;
        controller.aiConditions.IsDamaged = false;
        controller.aiConditions.ResetDefenseBool();
        controller.aiConditions.ResetStandingBool();
        controller.aiStatus.StandingCoolTime();
        Debug.Log("½ºÅÄµù »óÅÂ OUT");
    }


    private void ResetDatas(AIController controller)
    {
        controller.aiConditions.IsStanding = false;
        controller.aiStatus.ExtraAtkSpeed -= controller.aiStatus.IncreaseStandingAttackSpeed;
        controller.aiStatus.UpdateStats();
    }


    private IEnumerator StandingAnimTime_Co(AIController controller)
    {
        Debug.Log("½ºÅÄµù!");
        controller.aiAnim.Play("Standing",3 , 0f);
        float time = controller.aIVariables.StandingAnimFrame * (1f / (30f * controller.aIVariables.StandingAnimSpeed));

        yield return new WaitForSeconds(time);

        controller.aiAnim.Play("Reset", 3, 0f);
        controller.aiConditions.CanAttacking = true;
        Debug.Log("½ºÅÄµù °ø°Ý!");

    }
}
