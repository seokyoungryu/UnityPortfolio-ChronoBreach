using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/Stamina Regeneration Buff Object", fileName = "Buff_StaminaRegeneration")]
public class StaminaRegenerationBuffObject : BuffStatsObject
{
    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraStaminaRegenPerSec += value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraStaminaRegenPerSec -= value;
        playerController.playerStats.UpdateStats();

    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraStaminaRegenPerSec -= (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraStaminaRegenPerSec += (int)value;
        playerController.playerStats.UpdateStats();

    }
}
