using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Hit Per Hp Heal Object", fileName = "Stats_HitPerHpHeal")]
public class HitPerHpHealPlayerStatObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            playerController.playerStats.OriginHitPerHp += (int)value;
        else
            playerController.playerStats.ExtraHitPerHp += (int)value;
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            playerController.playerStats.OriginHitPerHp -= (int)value;
        else
            playerController.playerStats.ExtraHitPerHp -= (int)value;
        playerController.playerStats.UpdateStats();
    }
}
