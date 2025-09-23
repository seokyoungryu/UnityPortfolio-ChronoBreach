using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Useable/Buff/Move Speed Buff Object", fileName = "Buff_MoveSpeed_")]
public class MoveSpeddBuffObject : BuffStatsObject
{

    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        if (playerController != null)
            playerController.skillController.RegisterBuff(this);
        else if (aIController != null)
            aIController.skillController.RegisterBuff(this);

    }

    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)  playerController.playerStats.ExtraMoveSpeed = value;
        else playerController.playerStats.ExtraMoveSpeed = 1f;

    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart) playerController.playerStats.ExtraMoveSpeed = value;
        else playerController.playerStats.ExtraMoveSpeed = 1f;
    }

    protected override void SetAIBuff(bool isStart)
    {
        if (isStart) aIController.aiStatus.ExtraMoveSpeed = value;
        else aIController.aiStatus.ExtraMoveSpeed = 1f;
        aIController.UpdateNavSpeed();
    }

    protected override void SetAIDeBuff(bool isStart)
    {
        if (isStart) aIController.aiStatus.ExtraMoveSpeed = value;
        else aIController.aiStatus.ExtraMoveSpeed = 1f;
        aIController.UpdateNavSpeed();

    }
}
