using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/HitPerHpHeal Buff Object", fileName = "Buff_HitPerHpHeal")]
public class HitPerHpHealBuffObject : BuffStatsObject
{

    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraHitPerHp += (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraHitPerHp -= (int)value;
        playerController.playerStats.UpdateStats();
    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraHitPerHp -= (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraHitPerHp += (int)value;
        playerController.playerStats.UpdateStats();
    }
}

