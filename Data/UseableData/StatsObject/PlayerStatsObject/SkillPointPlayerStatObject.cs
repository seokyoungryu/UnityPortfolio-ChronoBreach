using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Skill Point Object", fileName = "Stats_SkillPoint")]
public class SkillPointPlayerStatObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        playerController.playerStats.AddSkillPoint((int)value);
        playerController.playerStats.UpdateStats();
    }

}

