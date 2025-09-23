using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Limit UI", fileName = "LimitUIs_")]
public class DungeonLimitUI : ScriptableObject
{
    [SerializeField] private LimitUIDefine.UIDefine[] limitUIs;

    public LimitUIDefine.UIDefine[] LimitUIs => limitUIs;
}
