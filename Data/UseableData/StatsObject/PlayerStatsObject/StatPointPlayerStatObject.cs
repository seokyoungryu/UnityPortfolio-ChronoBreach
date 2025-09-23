using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Stats Point Object", fileName = "Stats_StatsPoint")]
public class StatPointPlayerStatObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        playerController.playerStats.AddStatsPoint((int)value);
        playerController.playerStats.UpdateStats();
    }


}

