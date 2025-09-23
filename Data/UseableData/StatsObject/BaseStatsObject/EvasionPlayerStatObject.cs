using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Evasion Object", fileName = "Stats_Evasion")]
public class EvasionPlayerStatObject : BaseStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            controller.GetBaseStatus().OriginEvasion += (int)value;
        else
            controller.GetBaseStatus().ExtraEvasion += (int)value;
        controller.GetBaseStatus().UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            controller.GetBaseStatus().OriginEvasion -= (int)value;
        else
            controller.GetBaseStatus().ExtraEvasion -= (int)value;
        controller.GetBaseStatus().UpdateStats();
    }
}