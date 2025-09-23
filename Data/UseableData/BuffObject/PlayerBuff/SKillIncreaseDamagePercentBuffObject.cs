using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/SKillIncreaseDamage Buff Object", fileName = "Buff_SKillIncreaseDamagePercent")]
public class SKillIncreaseDamagePercentBuffObject : BuffStatsObject
{
    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraSkillIncreaseDmgPercentage += value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraSkillIncreaseDmgPercentage -= value;
        playerController.playerStats.UpdateStats();

    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraSkillIncreaseDmgPercentage -= (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraSkillIncreaseDmgPercentage += (int)value;
        playerController.playerStats.UpdateStats();

    }
}
