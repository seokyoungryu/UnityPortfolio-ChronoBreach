using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [SerializeField] private InventoryContainterUI inventoryContainter;
    [SerializeField] private QuickSlotContainer quickSlotContainer;
    [SerializeField] private InventoryUI[] inventoryUIs;

    private void Awake()
    {
        QuestManager.Instance.onRegister += inventoryContainter.RegisterReceiveHaveItemCount;
    }

    private void Start()
    {
        inventoryContainter.OnUpdateQuickSlot += UpdateItemCountInQuick;
        for (int i = 0; i < inventoryUIs.Length; i++)
            SetSlotOnItemUse(inventoryUIs[i].inventory);
        for (int i = 0; i < quickSlotContainer.QuickSlots.Length; i++)
            SetSlotOnItemUse(quickSlotContainer.QuickSlots[i].inventoryObject);
    }

    private void OnDestroy()
    {
        inventoryContainter.OnUpdateQuickSlot -= UpdateItemCountInQuick;
        for (int i = 0; i < inventoryUIs.Length; i++)
            RemoveSlotOnItemUse(inventoryUIs[i].inventory);
        for (int i = 0; i < quickSlotContainer.QuickSlots.Length; i++)
            RemoveSlotOnItemUse(quickSlotContainer.QuickSlots[i].inventoryObject);
    }


    private void LoadData()
    {
        for (int i = 0; i < inventoryUIs.Length; i++)
            SetSlotOnItemUse(inventoryUIs[i].inventory);
        for (int i = 0; i < quickSlotContainer.QuickSlots.Length; i++)
            SetSlotOnItemUse(quickSlotContainer.QuickSlots[i].inventoryObject);
    }


    private void UpdateItemCountInQuick(InventoryContainterUI inventoryContainterUI, Item item)
    {
        if (quickSlotContainer == null)
            quickSlotContainer = MainCanvas.Instance.QuickSlotContainer;

        quickSlotContainer.UpdateItemCountInQuick(inventoryContainter, item);
    }
    private void SetSlotOnItemUse(InventoryObject inventory)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            inventory.slots[i].OnItemUse_ += inventoryContainter.RemoveItemOne;
            //inventory.slots[i].onItemUse += quickSlotContainer.OnItemUses;
        }
    }

    private void RemoveSlotOnItemUse(InventoryObject inventory)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            inventory.slots[i].OnItemUse_ -= inventoryContainter.RemoveItemOne;
            //inventory.slots[i].onItemUse += quickSlotContainer.OnItemUses;
        }
    }
}
