using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/Str Buff Object", fileName = "Buff_Str")]
public class StrBuffObject : BuffStatsObject
{
   
    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraStrength += (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraStrength -= (int)value;
        playerController.playerStats.UpdateStats();

    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraStrength -= (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraStrength += (int)value;
        playerController.playerStats.UpdateStats();

    }
}
