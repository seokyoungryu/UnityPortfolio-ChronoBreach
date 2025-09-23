using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Stamina Regen Object", fileName = "Stats_StaminaRegen")]
public class StaminaRegenPlayerStatObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            playerController.playerStats.OriginStaminaRegenPerSec += (int)value;
        else
            playerController.playerStats.ExtraStaminaRegenPerSec += (int)value;
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            playerController.playerStats.OriginStaminaRegenPerSec -= (int)value;
        else
            playerController.playerStats.ExtraStaminaRegenPerSec -= (int)value;
        playerController.playerStats.UpdateStats();
    }
}

