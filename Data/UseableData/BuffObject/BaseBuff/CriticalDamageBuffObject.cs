using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/CriticalDamage Buff Object", fileName = "Buff_CriticalDamage")]
public class CriticalDamageBuffObject : BuffStatsObject
{

    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraCriticalDmg += value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraCriticalDmg -= value;
        playerController.playerStats.UpdateStats();
    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraCriticalDmg -= (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraCriticalDmg += (int)value;
        playerController.playerStats.UpdateStats();
    }
    protected override void SetAIBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraCriticalDmg += value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraCriticalDmg -= value;
        aIController.aiStatus.UpdateStats();
    }

    protected override void SetAIDeBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraCriticalDmg -= value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraCriticalDmg += value;
        aIController.aiStatus.UpdateStats();
    }
}