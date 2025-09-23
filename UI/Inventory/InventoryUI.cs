using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : DynamicContainerUI
{
    public PlayerEquipment playerEquipment = null;


    protected override void SlotFunction(GameObject slot)
    {
        if (uIType != ContainerType.INVENTORY) return;

        UIHelper.AddEventTrigger(slot, EventTriggerType.BeginDrag, delegate { OnStartDrag(slot); });
        UIHelper.AddEventTrigger(slot, EventTriggerType.Drag, delegate { OnDraging(slot); });
        UIHelper.AddEventTrigger(slot, EventTriggerType.EndDrag, delegate { OnEndDrag(slot, inventory); });
        UIHelper.AddEventTrigger(slot, EventTriggerType.PointerUp, delegate { PointerUp(slot, inventory); });

    }

    private void PointerUp(GameObject go, InventoryObject inventory)
    {
        if (MouseUIData.tempDraggingImage != null)
            Destroy(MouseUIData.tempDraggingImage);
    }


    protected override void OnEndDrag(GameObject go, InventoryObject inventory)
    {
        base.OnEndDrag(go, inventory);
        if (MouseUIData.tempDraggingImage == null) return;

        Destroy(MouseUIData.tempDraggingImage);

        if (MouseUIData.enterUIRoot == null)   //버림.
        {
            if(slotUIs[go].item.itemClip.isUnbreakable)
            {
                CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("파괴불가 아이템입니다.");
                return;
            }
            Debug.Log(slotUIs[go].item.itemClip.uiItemName + " Item 버림 ");
            CommonUIManager.Instance.DropItem(slotUIs[go].item, slotUIs[go].amount);
            CommonUIManager.Instance.playerInventory.UpdateQuickSlots(slotUIs[go].item);
        }
        else if (MouseUIData.enterUIRoot.uIType == ContainerType.INVENTORY && MouseUIData.enterSlot && slotUIs[go].item.HaveItem() 
            && slotUIs[MouseUIData.enterSlot].item.HaveItem() && inventory.CanJoinAmount(slotUIs[go].item, slotUIs[MouseUIData.enterSlot].item))
        {
            inventory.JoinAmount(slotUIs[go].item, slotUIs[MouseUIData.enterSlot].item);
        }
        else if (MouseUIData.enterUIRoot.uIType == ContainerType.INVENTORY && MouseUIData.enterSlot != null)
        {
            if (slotUIs[go].CanPlaceInSlot(slotUIs[MouseUIData.enterSlot].item))
            {
                inventory.SwapItemSlot(slotUIs[go], slotUIs[MouseUIData.enterSlot]);
                Debug.Log("아이템 스왑");
            }
        }
        else if (MouseUIData.enterUIRoot.uIType == ContainerType.EQUIPMNET && MouseUIData.enterSlot != null)
        {
            InventorySlot tmpEnterSlot = MouseUIData.enterUIRoot.GetComponent<EquipmentUI>().slotUIs[MouseUIData.enterSlot];

            if (playerEquipment == null)
                playerEquipment = GameManager.Instance.Player.playerEquipment;

            if (playerEquipment.CanEquipment(slotUIs[go].item) || GameManager.Instance.ignoreItemEquipCondition)
            {
                if (inventory.SwapItemSlot(slotUIs[go], tmpEnterSlot))
                {
                    if (slotUIs[go].item.itemClip != null && slotUIs[go].item.itemClip.equipmentTpye == EquipmentTpye.ACCESSORIES)
                        playerEquipment.UnEquipItem(slotUIs[go].item, false);

                    playerEquipment.EquipmentItem(tmpEnterSlot.item);
                }
            }
            else
                CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("아이템을 장착 할 수 없습니다!", SoundList.UIClick_NeutralButton19_KRST_NONE);
        }
        else if (MouseUIData.enterUIRoot != null && MouseUIData.enterUIRoot.uIType == ContainerType.QUICK
                    && MouseUIData.enterSlot != null)
        {
            QuickUI quickUI = MouseUIData.enterUIRoot.GetComponent<QuickUI>();
            if (quickUI.slotUIs[MouseUIData.enterSlot].CanPlaceInSlot(slotUIs[go].item))
                quickUI.slotUIs[MouseUIData.enterSlot].UpdateSlot(slotUIs[go].item, GetHaveCount(slotUIs[go].item));

        }
        else if (MouseUIData.enterSlot != null)
        {
            InventorySlot tmpEnterSlot = MouseUIData.enterUIRoot.GetComponent<InventoryContainterUI>().GetCurrentInventoryCategory().slotUIs[MouseUIData.enterSlot];
            inventory.SwapItemSlot(slotUIs[go], tmpEnterSlot);
            Debug.Log("아이템 스왑?2");
        }
        else if (MouseUIData.enterUIRoot.GetComponent<StoreUI>() != null)
        {
            StoreUI ui = MouseUIData.enterUIRoot.GetComponent<StoreUI>();
            if (ui.CurrentCategory == ItemCountConfirmCategory.SELL)
                ui?.SellItem(slotUIs[go].item);

            Debug.Log("아이템 판매");
        }
        else
            Debug.Log("아무것도 해당없음 - EndDrag");

        MouseUIData.dragSlot = null;
        OnItemInformationPointerEnter(slotUIs[go].item);
    }

    private int GetHaveCount(Item item)
    {
        return GetComponentInParent<InventoryContainterUI>().GetHaveItemCount(item);
    }

}
