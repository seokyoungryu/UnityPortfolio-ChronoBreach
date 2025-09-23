using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Str Object", fileName = "Stats_Str")]
public class StrPlayerStatObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            playerController.playerStats.OriginStrength += (int)value;
        else
            playerController.playerStats.ExtraStrength += (int)value;
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            playerController.playerStats.OriginStrength -= (int)value;
        else
            playerController.playerStats.ExtraStrength -= (int)value;
        playerController.playerStats.UpdateStats();
    }
}
