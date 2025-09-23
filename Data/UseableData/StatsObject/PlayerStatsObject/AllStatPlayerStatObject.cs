using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/All Stat Object", fileName = "Stats_AllStats")]
public class AllStatPlayerStatObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        playerController.playerStats.OriginAllStats += (int)value;
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        playerController.playerStats.OriginAllStats -= (int)value;
        playerController.playerStats.UpdateStats();
    }
}
