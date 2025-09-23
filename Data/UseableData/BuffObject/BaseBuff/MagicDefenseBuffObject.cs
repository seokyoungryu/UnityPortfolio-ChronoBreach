using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/MagicDefense Buff Object", fileName = "Buff_MagicDefense")]
public class MagicDefenseBuffObject : BuffStatsObject
{
    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraMagicDefense += value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraMagicDefense -= value;
        playerController.playerStats.UpdateStats();

    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraMagicDefense -= value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraMagicDefense += value;
        playerController.playerStats.UpdateStats();

    }

    protected override void SetAIBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraMagicDefense += value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraMagicDefense -= value;
        aIController.aiStatus.UpdateStats();
    }

    protected override void SetAIDeBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraAtk -= value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraAtk += value;
        aIController.aiStatus.UpdateStats();
    }

}


