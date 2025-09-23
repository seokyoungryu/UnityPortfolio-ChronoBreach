using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitUIDefine : MonoBehaviour
{
    [SerializeField] private UIDefine define = UIDefine.NONE;

    public UIDefine Define => define;

    public enum UIDefine
    {
        NONE = -1,
        INVENTORY = 0,
        SKILL_TREE = 1,
        QUICKSLOT = 2,
        EQUIPMENT = 3,
        QUEST_DISPLAY =4,
        QUEST_WINDOW = 5,
        STATUS = 6,
    } 
  
}


