using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public static class UIHelper
{

    public static void AddEventTrigger(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (trigger == null)
          trigger = go.AddComponent<EventTrigger>();

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
}
