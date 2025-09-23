using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/Defense Buff Object", fileName = "Buff_Defense")]
public class DefenseBuffObject : BuffStatsObject
{

    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraDefense += value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraDefense -= value;

    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
        {
            playerController.playerStats.ExtraDefense -= value;
            playerController.skillController.RegisterBuff(this);
        }
        else
            playerController.playerStats.ExtraDefense += value;
    }

    protected override void SetAIBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraDefense += value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraDefense -= value;
        aIController.aiStatus.UpdateStats();
    }

    protected override void SetAIDeBuff(bool isStart)
    {
        if (isStart)
        {
            aIController.aiStatus.ExtraDefense -= value;
            aIController.skillController.RegisterBuff(this);
        }
        else
            aIController.aiStatus.ExtraDefense += value;
        aIController.aiStatus.UpdateStats();
    }

}
