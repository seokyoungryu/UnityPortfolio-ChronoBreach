using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/AtkSpeed Buff Object", fileName = "Buff_AtkSpeed")]
public class AtkSpeedBuffObject : BuffStatsObject
{
    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraAtkSpeed += value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraAtkSpeed -= value;
        playerController.playerStats.UpdateStats();
    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraAtkSpeed -= value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraAtkSpeed += value;
        playerController.playerStats.UpdateStats();
    }

    protected override void SetAIBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraAtkSpeed += value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraAtkSpeed -= value;

        aIController.aiStatus.UpdateStats();
    }

    protected override void SetAIDeBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraAtkSpeed -= value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraAtkSpeed += value;
        aIController.aiStatus.UpdateStats();
    }
}