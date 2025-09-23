using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/CriticalChance Buff Object", fileName = "Buff_CriticalChance")]
public class CriticalChanceBuffObject : BuffStatsObject
{

    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraCriticalChance += value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraCriticalChance -= value;
        playerController.playerStats.UpdateStats();
    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraCriticalChance -= (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraCriticalChance += (int)value;
        playerController.playerStats.UpdateStats();
    }

    protected override void SetAIBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraCriticalChance += value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraCriticalChance -= value;
        aIController.aiStatus.UpdateStats();
    }

    protected override void SetAIDeBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraCriticalChance -= value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraCriticalChance += value;
        aIController.aiStatus.UpdateStats();
    }
}