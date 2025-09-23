using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionObject : UseableObject
{
    protected PlayerStateController playerController = null;

    public override void Apply(BaseController controller)
    {
        if (controller is PlayerStateController)
            playerController = controller as PlayerStateController;

    }
}
