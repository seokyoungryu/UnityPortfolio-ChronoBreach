using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentUI : StaticContainertUI
{
    public PlayerEquipment playerEquipment = null;

   
    protected override void Awake()
    {
        base.Awake(); 
        LoadEquipmentUI();
   }

    protected override void Start()
    {
        if (playerEquipment == null)
            playerEquipment = GameManager.Instance.Player.playerEquipment;
        base.Start();
        gameObject.SetActive(false);

    }

    public void LoadEquipmentUI()
    {
        inventoryObject.SaveLoadData.LoadInventoryData(inventoryObject);
        UpdateInventorySlots(inventoryObject);
    }
    public void SaveEquipmentUI()
    {
        inventoryObject.SaveLoadData.SaveInventoryData(inventoryObject);
    }

    public InventorySlot[] GetEquipmentSlots()
    {
        InventorySlot[] slots = new InventorySlot[staticSlots.Length];

        for (int i = 0; i < staticSlots.Length; i++)
        {
            slots[i] = slotUIs[staticSlots[i]];
        }

        return slots;
    }

    protected override void SlotFunction(GameObject slot)
    {
        if (uIType != ContainerType.EQUIPMNET) return;

        UIHelper.AddEventTrigger(slot, EventTriggerType.BeginDrag, delegate { OnStartDrag(slot); });
        UIHelper.AddEventTrigger(slot, EventTriggerType.Drag, delegate { OnDraging(slot); });
        UIHelper.AddEventTrigger(slot, EventTriggerType.EndDrag, delegate { OnEndDrag(slot, inventoryObject); });
    }


    protected override void OnEndDrag(GameObject go, InventoryObject inventory)
    {
        base.OnEndDrag(go, inventory);
        if (MouseUIData.tempDraggingImage == null) return;

        Destroy(MouseUIData.tempDraggingImage);

        if (MouseUIData.enterUIRoot == null && MouseUIData.dragSlot.GetComponentInParent<EquipmentUI>()) //착용제거 & 버림
        {
           // playerEquipment.UnEquipItem(slotUIs[go].item);
           // slotUIs[go].RemoveItem();
           // Debug.Log("1");
           //
        }
        else if(MouseUIData.enterUIRoot.uIType == ContainerType.INVENTORY)
        {
            Debug.Log("NO");
        }
        else if (MouseUIData.enterUIRoot == null)   //버림.
        {
            Debug.Log("2");
            // playerEquipment.UnEquipItem(slotUIs[go].item);
            slotUIs[go].RemoveItem();
        }

        else
            Debug.Log("아무것도 해당없음 - EndDrag");

        MouseUIData.dragSlot = null;
    }
}
