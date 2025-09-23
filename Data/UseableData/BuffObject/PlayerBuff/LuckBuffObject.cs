using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/Luck Buff Object", fileName = "Buff_Luck")]
public class LuckBuffObject : BuffStatsObject
{
    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraLuck += (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraLuck -= (int)value;
        playerController.playerStats.UpdateStats();

    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraLuck -= (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraLuck += (int)value;
        playerController.playerStats.UpdateStats();

    }
}

