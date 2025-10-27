using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDungeonCateogry : ScriptableObject
{
    public abstract PlayerStateController InitControllerSetting(BaseDungeonTitle title);
}
