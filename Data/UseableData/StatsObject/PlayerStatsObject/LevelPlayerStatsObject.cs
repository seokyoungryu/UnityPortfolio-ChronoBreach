using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Player Stats Object/Level Object", fileName = "Stats_Level")]
public class LevelPlayerStatsObject : PlayerStatsObject
{
    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        playerController.playerStats.AddLevel((int)value);
        playerController.playerStats.UpdateStats();
    }

}

