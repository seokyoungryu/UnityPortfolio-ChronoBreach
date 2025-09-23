using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Dex Object", fileName = "Stats_Dex")]
public class DexPlayerStatObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            playerController.playerStats.OriginDexterity += (int)value;
        else
            playerController.playerStats.ExtraDexterity += (int)value;
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            playerController.playerStats.OriginDexterity -= (int)value;
        else
            playerController.playerStats.ExtraDexterity -= (int)value;
        playerController.playerStats.UpdateStats();
    }
}
