using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/HP Object", fileName = "Stats_HP")]
public class HPPlayerStatsObject : BaseStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            controller.GetBaseStatus().OriginHealth += (int)value;
        else
            controller.GetBaseStatus().ExtraHealth += (int)value;

        controller.GetBaseStatus().AddCurrentHealth((int)value);
        controller.GetBaseStatus().UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            controller.GetBaseStatus().OriginHealth -= (int)value;
        else
            controller.GetBaseStatus().ExtraHealth -= (int)value;

        controller.GetBaseStatus().UpdateStats();
    }
}

