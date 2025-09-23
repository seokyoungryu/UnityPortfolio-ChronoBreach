using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsObject : BaseStatsObject
{
    protected PlayerStateController playerController;


    public override void Apply(BaseController controller)
    {
        if (!(controller is PlayerStateController)) return;
        playerController = controller as PlayerStateController;
    }
}
