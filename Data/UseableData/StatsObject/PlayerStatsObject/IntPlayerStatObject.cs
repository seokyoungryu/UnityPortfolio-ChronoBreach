using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Int Object", fileName = "Stats_Int")]
public class IntPlayerStatObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            playerController.playerStats.OriginIntelligence += (int)value;
        else
            playerController.playerStats.ExtraIntelligence += (int)value;
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            playerController.playerStats.OriginIntelligence -= (int)value;
        else
            playerController.playerStats.ExtraIntelligence -= (int)value;
        playerController.playerStats.UpdateStats();
    }
}


