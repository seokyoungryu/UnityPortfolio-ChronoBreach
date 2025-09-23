using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Condition Data/Target Far", fileName = "Condition_TargetFar_")]
public class TargetFarCondition : BaseConditionInfo
{
    [Header("Target이 Far보다 멀 경우 True")]
    [SerializeField] private float farDistance = 6f;
    private float distance = 0f;

    public override bool CanExcuteCondition(BaseController controller)
    {
        if (!CanSetAIController(controller)) return false;
        if (aiController == null || aiController.IsDead()) return false;
        if (aiController.aIVariables.target == null) return false;

        distance = (aiController.aIVariables.target.transform.position - controller.transform.position).magnitude;
        if (distance >= farDistance)
            return true;

        return false;
    }
}
