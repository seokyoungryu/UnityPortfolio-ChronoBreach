using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Magic Defense Object", fileName = "Stats_MagicDefense")]
public class MagicDefensePlayerStatsObject : BaseStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            controller.GetBaseStatus().OriginMagicDefense += (int)value;
        else
            controller.GetBaseStatus().ExtraMagicDefense += (int)value;
        controller.GetBaseStatus().UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            controller.GetBaseStatus().OriginMagicDefense -= (int)value;
        else
            controller.GetBaseStatus().ExtraMagicDefense -= (int)value;
        controller.GetBaseStatus().UpdateStats();
    }
}
