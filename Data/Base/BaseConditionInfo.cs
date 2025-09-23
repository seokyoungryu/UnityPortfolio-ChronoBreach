using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseConditionInfo : ScriptableObject
{

    protected AIController aiController;


    public abstract bool CanExcuteCondition(BaseController controller);


    protected bool CanSetAIController(BaseController controller)
    {
        if (controller == null) return false;
        if (controller is AIController) aiController = controller as AIController;
        if (aiController) return true;

        return false;
    }


}
