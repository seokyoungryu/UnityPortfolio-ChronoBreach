using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StaticContainertUI : BaseInventoryUI
{
    public InventoryObject inventoryObject = null;
    public GameObject[] staticSlots = null;




  // public  void Update()
  // {
  //     if (Input.GetKeyDown(KeyCode.M))
  //         inventoryObject.SaveLoadData.SaveInventoryData(inventoryObject);
  //     
  //
  //     if (Input.GetKeyDown(KeyCode.C))
  //     {
  //         inventoryObject.Clear();
  //     }
  //
  //     if (Input.GetKeyDown(KeyCode.B))
  //     {
  //         inventoryObject.AddItem(ItemManager.Instance.GenerateRandomItem(), 1);
  //         UpdateInventorySlots(inventoryObject);
  //     }
  //
  // }


    protected override void CreateSlotUIs()
    {
        if (ScenesManager.Instance.ChangeCount != 1 )
        {
            Debug.Log(gameObject.name + "CreateSlotUIs NOT!");
            return;
        }

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            inventoryObject.slots[i].parent = inventoryObject;
            inventoryObject.slots[i].OnPostUpdate += OnPostUpdate;

            GameObject go = staticSlots[i];

            UIHelper.AddEventTrigger(go, EventTriggerType.PointerEnter, delegate { OnPointEnter(go); });
            UIHelper.AddEventTrigger(go, EventTriggerType.PointerExit, delegate { OnPointExit(go); });

            UIHelper.AddEventTrigger(go, EventTriggerType.PointerEnter, delegate { OnItemInformationPointerEnter(slotUIs[go].item); });
            UIHelper.AddEventTrigger(go, EventTriggerType.PointerExit, delegate { OnItemInformationPointerExit(slotUIs[go].item); });
            UIHelper.AddEventTrigger(go, EventTriggerType.BeginDrag, delegate { OnItemInformationPointerExit(slotUIs[go].item); });
            UIHelper.AddEventTrigger(go, EventTriggerType.PointerUp, delegate { OnItemInformationPointerEnter(slotUIs[go].item); });

            SlotFunction(go);
            
            inventoryObject.slots[i].slotUI = go;
            slotUIs.Add(go, inventoryObject.slots[i]);
            inventoryObject.slots[i].SetCoolTimeUI(go.GetComponent<ImageCoolTimeUI>());
            // go.name = gameObject.name + "_" + i;

        }
    }

    protected override void OnEndDrag(GameObject go, InventoryObject inventory)
    {
        base.OnEndDrag(go, inventory);
    }
}
