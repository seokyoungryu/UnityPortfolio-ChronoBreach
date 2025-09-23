using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDungeonCondition : ScriptableObject
{


    public abstract DungeonDetailConditionTask[] CreateDungeonConditionUIs(string conditionOBP);
    
}
