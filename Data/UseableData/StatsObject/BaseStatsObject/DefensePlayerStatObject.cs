using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Defense Object", fileName = "Stats_Defense")]
public class DefensePlayerStatObject : BaseStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            controller.GetBaseStatus().OriginDefense += (int)value;
        else
            controller.GetBaseStatus().ExtraDefense += (int)value;
        controller.GetBaseStatus().UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            controller.GetBaseStatus().OriginDefense -= (int)value;
        else
            controller.GetBaseStatus().ExtraDefense -= (int)value;
        controller.GetBaseStatus().UpdateStats();
    }
}
