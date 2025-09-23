using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Stamina Regen Time Object", fileName = "Stats_StaminaRegenTime")]
public class StaminaRegenTimePlayerStatsObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (applyOriginStat)
            playerController.playerStats.OriginSkillIncreaseDmgPercentage += (int)value;
        else
            playerController.playerStats.ExtraSkillIncreaseDmgPercentage += (int)value;
        playerController.playerStats.UpdateStats();
    }

    public override void RemoveApplyValue(BaseController controller)
    {
        if (applyOriginStat)
            playerController.playerStats.OriginSkillIncreaseDmgPercentage -= (int)value;
        else
            playerController.playerStats.ExtraSkillIncreaseDmgPercentage -= (int)value;
        playerController.playerStats.UpdateStats();
    }
}
