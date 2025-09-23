using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Hp Regen Time Object", fileName = "Stats_HpRegenTime")]
public class HPRegenTimePlayerStatsObject : PlayerStatsObject
{
    [Tooltip("Value만큼 현재 리젠 시간에서 - 한다. ")]

    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        playerController.playerStats.HealthRegenTime -= (int)value;
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        playerController.playerStats.HealthRegenTime += (int)value;
        playerController.playerStats.UpdateStats();
    }

}
