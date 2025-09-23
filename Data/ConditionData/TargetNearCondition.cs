using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Condition Data/Target Near", fileName = "Condition_TargetNear_")]
public class TargetNearCondition : BaseConditionInfo
{
    [Header("Target이 Near 안에 있을 경우 True")]
    [SerializeField] private float nearDistance = 6f;
    private float distance = 0f;

    public override bool CanExcuteCondition(BaseController controller)
    {
        if (!CanSetAIController(controller)) return false;
        if (aiController.aIVariables.target == null || aiController.aIVariables.target.IsDead()) return false;

        distance = (aiController.aIVariables.target.transform.position - controller.transform.position).magnitude;
        if (distance <= nearDistance)
            return true;

        return false;
    }
}
