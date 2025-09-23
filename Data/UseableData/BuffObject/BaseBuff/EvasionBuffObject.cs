using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/Evasion Buff Object", fileName = "Buff_Evasion")]
public class EvasionBuffObject : BuffStatsObject
{
    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraEvasion += (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraEvasion -= (int)value;
        playerController.playerStats.UpdateStats();
    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraEvasion -= (int)value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraEvasion += (int)value;
        playerController.playerStats.UpdateStats();
    }
    protected override void SetAIBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraEvasion += value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraEvasion -= value;
        aIController.aiStatus.UpdateStats();
    }

    protected override void SetAIDeBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraEvasion -= value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraEvasion += value;
        aIController.aiStatus.UpdateStats();
    }
}
