using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/HP Regeneration Buff Object", fileName = "Buff_HPRegeneration")]
public class HPRegenerationBuffObject : BuffStatsObject
{
    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraHpRegenPerSec += value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraHpRegenPerSec -= value;

        playerController.playerStats.UpdateStats();

    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraHpRegenPerSec -= (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraHpRegenPerSec += (int)value;
        playerController.playerStats.UpdateStats();

    }

}
