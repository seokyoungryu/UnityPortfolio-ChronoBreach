using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "AI/Decisions/Is Interact", fileName = "IsInteractDecision")]
public class IsInteractDecision : Decision
{
    [SerializeField] private InteractType interactType = InteractType.TRUE_INTERACT;
    enum InteractType
    {
        TRUE_INTERACT = 0,
        FALASE_INTERACT = 1,
        END_INTERACT =2,
    }

    public override bool Decide(AIController controller)
    {
        if (interactType == InteractType.TRUE_INTERACT)
        {
            if (controller.aiConditions.IsInteract)
                return true;
        }
        else if (interactType == InteractType.FALASE_INTERACT)
        {
            if (!controller.aiConditions.IsInteract)
                return true;
        }
        else if (interactType == InteractType.END_INTERACT)
        {
            if (controller.aIFSMVariabls.canExitInteractAction)
                return true;
        }


        return false;
    }


}