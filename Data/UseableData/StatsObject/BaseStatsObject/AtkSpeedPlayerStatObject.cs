using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Atk Speed Object", fileName = "Stats_AtkSpeed")]
public class AtkSpeedPlayerStatObject : BaseStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            controller.GetBaseStatus().OriginAtkSpeed += (int)value;
        else
            controller.GetBaseStatus().ExtraAtkSpeed += (int)value;
        controller.GetBaseStatus().UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            controller.GetBaseStatus().OriginAtkSpeed -= (int)value;
        else
            controller.GetBaseStatus().ExtraAtkSpeed -= (int)value;
        controller.GetBaseStatus().UpdateStats();
    }
}
