using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Ciritical Damage Object", fileName = "Stats_CriticalDamage")]
public class CriticalDamagePlayerStatsObject : BaseStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            controller.GetBaseStatus().OriginCriticalDmg += (int)value;
        else
            controller.GetBaseStatus().ExtraCriticalDmg += (int)value;
        controller.GetBaseStatus().UpdateStats();
    }


    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            controller.GetBaseStatus().OriginCriticalDmg -= (int)value;
        else
            controller.GetBaseStatus().ExtraCriticalDmg -= (int)value;
        controller.GetBaseStatus().UpdateStats();
    }
}
