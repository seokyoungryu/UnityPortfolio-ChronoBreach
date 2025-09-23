using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Ciritical Chance Object", fileName = "Stats_CriticalChance")]
public class CriticalChancePlayerStatObject : BaseStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
          if (applyOriginStat)
            controller.GetBaseStatus().OriginCriticalChance += (int)value;
        else
            controller.GetBaseStatus().ExtraCriticalChance += (int)value;
        controller.GetBaseStatus().UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            controller.GetBaseStatus().OriginCriticalChance -= (int)value;
        else
            controller.GetBaseStatus().ExtraCriticalChance -= (int)value;
        controller.GetBaseStatus().UpdateStats();
    }
}
