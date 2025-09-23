using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Stamina Object", fileName = "Stats_Stamina")]
public class StatminaPlayerStatsObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            playerController.playerStats.OriginStamina += (int)value;
        else
            playerController.playerStats.ExtraStamina += (int)value;
        playerController.playerStats.AddCurrentStamina((int)value);
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            playerController.playerStats.OriginStamina -= (int)value;
        else
            playerController.playerStats.ExtraStamina -= (int)value;
        playerController.playerStats.AddCurrentStamina(-(int)value);
        playerController.playerStats.UpdateStats();
    }
}

