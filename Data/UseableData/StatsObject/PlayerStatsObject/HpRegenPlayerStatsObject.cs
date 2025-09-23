using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Hp RegenPerSec Object", fileName = "Stats_HpRegen")]
public class HpRegenPlayerStatsObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            playerController.playerStats.OriginHpRegenPerSec += (int)value;
        else
            playerController.playerStats.ExtraHpRegenPerSec += (int)value;

        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            playerController.playerStats.OriginHpRegenPerSec -= (int)value;
        else
            playerController.playerStats.ExtraHpRegenPerSec -= (int)value;
        playerController.playerStats.UpdateStats();
    }
}

