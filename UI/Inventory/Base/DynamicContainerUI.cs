using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DynamicContainerUI : BaseInventoryUI
{
    public GameObject slot_Prefab;
    public InventoryObject inventory = null;

    [SerializeField] protected Vector2 startPosition = Vector2.zero;
    [SerializeField] protected Vector2 size = Vector2.zero;
    [SerializeField] protected Vector2 space = Vector2.zero;
    [SerializeField] protected int colum = 0;

  
    protected override void Start()
    {
        UpdateInventorySlots(inventory);
    }


    protected virtual void SettingDelegateSlots(InventorySlot slot) { }

    protected override void CreateSlotUIs()
    {
        Debug.Log("CreateSlotUIs  : " + ScenesManager.Instance.ChangeCount);

        if (ScenesManager.Instance.ChangeCount != 1)
       {
           Debug.Log(gameObject.name + "CreateSlotUIs NOT!");
           return;
       }

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            inventory.slots[i].parent = inventory;
            inventory.slots[i].OnPostUpdate += OnPostUpdate;
          
            GameObject go = Instantiate(slot_Prefab, Vector3.zero, Quaternion.identity, transform);
            go.GetComponent<RectTransform>().anchoredPosition = CalculateRectPosition(i, startPosition, size, space, colum);

            UIHelper.AddEventTrigger(go, EventTriggerType.PointerEnter, delegate { OnPointEnter(go); });
            UIHelper.AddEventTrigger(go, EventTriggerType.PointerExit, delegate { OnPointExit(go); });

            UIHelper.AddEventTrigger(go, EventTriggerType.PointerEnter, delegate { OnItemInformationPointerEnter(slotUIs[go].item); });
            UIHelper.AddEventTrigger(go, EventTriggerType.PointerExit, delegate { OnItemInformationPointerExit(slotUIs[go].item); });
            UIHelper.AddEventTrigger(go, EventTriggerType.BeginDrag, delegate { OnItemInformationPointerExit(slotUIs[go].item); });
            UIHelper.AddEventTrigger(go, EventTriggerType.PointerUp , delegate { OnItemInformationPointerEnter(slotUIs[go].item); });

            SlotFunction(go);


            inventory.slots[i].slotUI = go;
            slotUIs.Add(go, inventory.slots[i]);
            go.name = gameObject.name + "_" + i;

        }
    }



    protected override void OnEndDrag(GameObject go, InventoryObject inventory)
    {
        base.OnEndDrag(go, inventory);
    }
}
