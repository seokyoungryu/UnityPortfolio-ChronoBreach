using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InteractMouseInUI : MonoBehaviour
{
    [SerializeField] private List<EnterMouseUIInfo> uis = new List<EnterMouseUIInfo>();


    private void Awake()
    {
        for (int i = 0; i < uis.Count; i++)
        {
            UnityEvent action = uis[i].Action;
            for (int x = 0; x < uis[i].UIs.Count; x++)
                UIHelper.AddEventTrigger(uis[i].UIs[x], uis[i].TriggerType, delegate { action?.Invoke(); });

        }
    }

    public void DontAttack()
    {
        GameManager.Instance.Player.Conditions.CanAttack = false;
    }

    public void CanAttack()
    {
        GameManager.Instance.Player.Conditions.CanAttack = true;
    }

}


[System.Serializable]
public class EnterMouseUIInfo
{
    [SerializeField] private List<GameObject> uis;
    [SerializeField] private EventTriggerType triggerType;
    [SerializeField] private UnityEvent action;

    public List<GameObject> UIs => uis;
    public EventTriggerType TriggerType => triggerType;
    public UnityEvent Action => action;
}




