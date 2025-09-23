using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HpCheckConditionType { TARGET = 0, CONTROLELR_OWN = 1, PLAYER = 2,}

[CreateAssetMenu(menuName = "Data/Condition Data/Hp Check", fileName = "Condition_HpCheck_")]
public class HPCheckCondition : BaseConditionInfo
{
    [SerializeField] private HpCheckConditionType conditionTarget = HpCheckConditionType.CONTROLELR_OWN;
    [Header("controller의 Hp가 HpPercentage보다 작을 True (0% ~ 100%)")]
    [SerializeField] private float hpPercentage = 100f;
    private float percentage = 0f;

    public override bool CanExcuteCondition(BaseController controller)
    {
        if (controller == null) return false;

        switch (conditionTarget)
        {
            case HpCheckConditionType.TARGET:
                return TargetCondition(controller);
            case HpCheckConditionType.CONTROLELR_OWN:
                return OwnCondition(controller);
            case HpCheckConditionType.PLAYER:
                return PlayerCondition(controller);
        }

        return false;
    }


    private bool TargetCondition(BaseController controller)
    {
        AIController aiContr = controller.GetComponent<AIController>();
        if (aiContr == null || aiContr.aIVariables.target == null) return false;
        BaseStatus stats = aiContr.aIVariables.target.GetBaseStatus();
        if (stats == null) return false;

        if (GetPercentage(stats.GetCurrentHPValue(), stats.GetTotalHPValue()) <= hpPercentage)
            return true;
        return false;
    }
    private bool OwnCondition(BaseController controller)
    {
        BaseStatus stats = controller.GetComponent<BaseStatus>();
        if (stats == null) return false;

        if (GetPercentage(stats.GetCurrentHPValue(), stats.GetTotalHPValue()) <= hpPercentage)
            return true;
        return false;
    }
    private bool PlayerCondition(BaseController controller)
    {
        BaseStatus stats = GameManager.Instance.Player?.GetComponent<BaseStatus>();
        if (stats == null) return false;

        if (GetPercentage(stats.GetCurrentHPValue(), stats.GetTotalHPValue()) <= hpPercentage)
            return true;
        return false;
    }


    private float GetPercentage(float currentHp, float maxHp)
    {
        percentage = (currentHp / maxHp) * 100f;
        return percentage;
    }

}
