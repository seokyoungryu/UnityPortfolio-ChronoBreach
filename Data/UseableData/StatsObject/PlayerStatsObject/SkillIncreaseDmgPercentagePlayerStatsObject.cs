using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Skill Increase Damage % Object", fileName = "Stats_SkillDamagePercent")]
public class SkillIncreaseDmgPercentagePlayerStatsObject : PlayerStatsObject
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
