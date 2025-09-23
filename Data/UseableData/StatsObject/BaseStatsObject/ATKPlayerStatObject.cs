using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Atk Object", fileName = "Stats_Atk")]
public class ATKPlayerStatObject : BaseStatsObject
{

    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            controller.GetBaseStatus().OriginAtk += (int)value;
        else
            controller.GetBaseStatus().ExtraAtk += (int)value;

        controller.GetBaseStatus().UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            controller.GetBaseStatus().OriginAtk -= (int)value;
        else
            controller.GetBaseStatus().ExtraAtk -= (int)value;

        controller.GetBaseStatus().UpdateStats();
    }
}
