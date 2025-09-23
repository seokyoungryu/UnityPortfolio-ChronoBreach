using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Rescyr Action")]
public class RescurAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        controller.aiAnim.SetBool("IsRescur", true);
        controller.aiConditions.CanDamaged = false;
    }

    public override void Act(AIController controller, float deltaTime)
    {
        //타겟 범위만큼 체ㅋ하다가 발견시 타겟 등록후 rescurAction Exit하기

    }

    public override void OnExitAction(AIController controller)
    {
        controller.aiAnim.SetBool("IsRescur", false);
        controller.aiConditions.CanDamaged = true;
    }
}
