using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UISoundType
{
    OPEN_WINDOW,
    CLOSE_WINDOW,
    START_DRAG,
    END_DRAG,
    ORDER_BY_WINDOW,
    ITEMDETAIL_WINDOW,
    PLAYER_LEVELUP,
    ITEM_COLLECT,
    INTERACT ,
    PRINT_DIALOG_ALL,
    PRINT_DIALOG_ONEBYONE,
    DONE_DIALOG,
    DIALOG_ACCEPT,
    DIALOG_REJECT,
    DIALOG_CLOSE,
    QUEST_REGISTER,
    QUEST_COMPLETE,
    QUEST_TASKREGISER,
    QUEST_TASKCOMPLETE,
    QUEST_CANCEL,
    SKILL_UPGRADE,
    STATS_BTN,
    STATS_CENCEL,
    STATS_APPLY,
    SKILL_LOCK_ON,

    DUNGEON_CLEAR,
    DUNGEON_FAIL,

    VICTORY_SCORE_ENTER,
    FAIL_SCORE_ENTER,
    SCORE_TASK_ENTER,
    SCORE_COUNTING,
    SPAWN_ENEMY,
    SPAWN_PLAYABLEAI,

    TITLE_POINTER_ENTER = 33,
    TITLE_POINTER_ENTER2 = 34,
    TITLE_POINTER_ENTER3 = 35,

    NEGATIVE_NOTIFIER,
    POSITIVE_NOTIFIER,
    CLICK,
    QUEST_COMPLETE_DIALOG,

    EQUIPMENT,

}

[System.Serializable]
public class UISoundSetting
{
    [SerializeField] private UISoundType info;
    [SerializeField] private EventTriggerType type;
    [SerializeField] private GameObject[] targetGos;

    public UISoundType UISoundType => info;
    public EventTriggerType Type => type;
    public GameObject[] TargetGos => targetGos;
}


public class UISounds : MonoBehaviour
{
    [SerializeField] private List<UISoundInfo> infos = new List<UISoundInfo>();



    public UISoundInfo GetInfo(UISoundType type)
    {
        for (int i = 0; i < infos.Count; i++)
            if (infos[i].type == type)
                return infos[i];

        return null;
    }


    private void OnValidate()
    {
        if(infos.Count >0)
        {
            for (int i = 0; i < infos.Count; i++)
                infos[i].name = infos[i].type.ToString();
        }
    }

}


[System.Serializable]
public class UISoundInfo
{
    public string name;
    public UISoundType type;
    public SoundList sound;
}



