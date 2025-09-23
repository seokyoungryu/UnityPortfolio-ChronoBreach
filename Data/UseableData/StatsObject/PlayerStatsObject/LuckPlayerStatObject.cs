using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Luck Object", fileName = "Stats_Luck")]
public class LuckPlayerStatObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            playerController.playerStats.OriginLuck += (int)value;
        else
            playerController.playerStats.ExtraLuck += (int)value;
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            playerController.playerStats.OriginLuck -= (int)value;
        else
            playerController.playerStats.ExtraLuck -= (int)value;
        playerController.playerStats.UpdateStats();
    }
}
