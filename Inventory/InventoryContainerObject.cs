using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryCategory
{
    EQUIPMENT = 0,
    CONSUMABLE = 1,
    MATERIAL = 2,
    QUESTITEM = 3,
}

[CreateAssetMenu(menuName = "Inventory/Inventory Container Object")]
public class InventoryContainerObject : ScriptableObject
{
    [SerializeField] private InventoryContainerInfo[] containers;


    public InventoryContainerObject(InventoryContainerObject clone)
    {
        containers = new InventoryContainerInfo[clone.containers.Length];
        for (int i = 0; i < containers.Length; i++)
        {
            if (clone.containers[i] != null)
            {
                containers[i] = new InventoryContainerInfo();
                containers[i].inventoryObj = Instantiate(clone.containers[i].inventoryObj);
            }
            else
                Debug.Log(i + " NULL");
        }
    }

    public void ClearAllInventory()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].inventoryObj.Clear();
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < containers.Length; i++)
        {
            for (int x = 0; x < containers[i].inventoryObj.slots.Length; x++)
            {
                InventorySlot slot = containers[i].inventoryObj.slots[x];
                slot.UpdateSlot(slot.item, slot.amount);
            }
        }
    }


    public bool CheckCanAddItems(ItemContainer itemContainer) => CheckCanAddItems(new ItemContainer[]{itemContainer});

    public bool CheckCanAddItems(ItemContainer[] itemContainers)
    {
        InventoryContainerObject tmpContainer = new InventoryContainerObject(this);

        bool check = true;
        for (int i = 0; i < itemContainers.Length; i++)
        {
            check = CheckCanAddItems(itemContainers[i], tmpContainer);
            if (!check)
                return false;
        }

        return true;
    }

    public bool CheckCanAddItems(ItemContainer itemContainer, InventoryContainerObject inventoryContainerObject)
    {
        if(!inventoryContainerObject.AddItem(itemContainer.Item, itemContainer.Amount, true))
        {
            ItemCategoryType itemType = itemContainer.Item.itemClip.itemCategoryType;
            if (itemType == ItemCategoryType.EQUIPMENT)
                CommonUIManager.Instance.ExcuteGlobalNotifer("장비창 공간을 비워주세요.");
            else if(itemType == ItemCategoryType.CONSUMABLE)
                CommonUIManager.Instance.ExcuteGlobalNotifer("소비창 공간을 비워주세요.");
            else if (itemType == ItemCategoryType.MATERIAL)
                CommonUIManager.Instance.ExcuteGlobalNotifer("재료창 공간을 비워주세요.");
            else if (itemType == ItemCategoryType.QUESTITEM)
                CommonUIManager.Instance.ExcuteGlobalNotifer("퀘스트아이템 공간을 비워주세요.");
            return false;
        }

        return true;
    }

    public bool AddItem(Item item, int amount, bool isChecking = false)
    {
        if (!isChecking)
            CommonUIManager.Instance.ExcuteItemGainNotifier(item, amount);

        switch (item.itemClip.itemCategoryType)
        {
            case ItemCategoryType.EQUIPMENT:
               return containers[0].inventoryObj.AddItem(item, amount);
            case ItemCategoryType.CONSUMABLE:
                return containers[1].inventoryObj.AddItem(item, amount);
            case ItemCategoryType.MATERIAL:
                return containers[2].inventoryObj.AddItem(item, amount);
            case ItemCategoryType.QUESTITEM:
                return containers[3].inventoryObj.AddItem(item, amount);
        }
      
        return false;
    }

    public void RemoveItemOne(Item item)
    {
        RemoveItem(item, 1);
    }

    /// <summary>
    /// 고유 아이템 삭제
    /// </summary>
    public void RemoveItemByInstanceID(Item item, int instanceID)
    {
        RemoveItem(item, 1, instanceID);
    }

    public void RemoveItem(Item item, int amount, int removeByinstanceID = -1)
    {
        switch (item.itemClip.itemCategoryType)
        {
            case ItemCategoryType.EQUIPMENT:
                if (removeByinstanceID == -1) containers[0].inventoryObj.RemoveItem(item, amount);
                else containers[0].inventoryObj.RemoveItem(item, amount, removeByinstanceID);
                break;
            case ItemCategoryType.CONSUMABLE:
                if (removeByinstanceID == -1) containers[1].inventoryObj.RemoveItem(item, amount);
                else containers[1].inventoryObj.RemoveItem(item, amount, removeByinstanceID);
                break;
            case ItemCategoryType.MATERIAL:
                if (removeByinstanceID == -1) containers[2].inventoryObj.RemoveItem(item, amount);
                else containers[2].inventoryObj.RemoveItem(item, amount, removeByinstanceID);
                break;
            case ItemCategoryType.QUESTITEM:
                if (removeByinstanceID == -1) containers[3].inventoryObj.RemoveItem(item, amount);
                else containers[3].inventoryObj.RemoveItem(item, amount, removeByinstanceID);
                break;
        }
    }

    public int GetRemainingItemCount(Item item)
    {
        switch (item.itemClip.itemCategoryType)
        {
            case ItemCategoryType.EQUIPMENT:
                return containers[0].inventoryObj.GetRemainingCount(item);
            case ItemCategoryType.CONSUMABLE:
                return containers[1].inventoryObj.GetRemainingCount(item);
            case ItemCategoryType.MATERIAL:
                return containers[2].inventoryObj.GetRemainingCount(item);
            case ItemCategoryType.QUESTITEM:
                return containers[3].inventoryObj.GetRemainingCount(item);
        }
        return 0;
    }

    public InventorySlot FindInstanceIDItem(Item item, int instanceId)
    {
        switch (item.itemClip.itemCategoryType)
        {
            case ItemCategoryType.EQUIPMENT:
                return containers[0].inventoryObj.FindInstanceID(item, instanceId);
            case ItemCategoryType.CONSUMABLE:
                return containers[1].inventoryObj.FindInstanceID(item,instanceId);
            case ItemCategoryType.MATERIAL:
                return containers[2].inventoryObj.FindInstanceID(item,instanceId);
            case ItemCategoryType.QUESTITEM:
                return containers[3].inventoryObj.FindInstanceID(item,instanceId);
        }
        return null;
    }

    public int GetEmptySlotCount(Item item)
    {
        switch (item.itemClip.itemCategoryType)
        {
            case ItemCategoryType.EQUIPMENT:
                return containers[0].inventoryObj.GetEmptySlotCount();
            case ItemCategoryType.CONSUMABLE:
                return containers[1].inventoryObj.GetEmptySlotCount();
            case ItemCategoryType.MATERIAL:
                return containers[2].inventoryObj.GetEmptySlotCount();
            case ItemCategoryType.QUESTITEM:
                return containers[3].inventoryObj.GetEmptySlotCount();
        }
        return 0;
    }

    public int GetHaveItemCount(Item item)
    {
        switch (item.itemClip.itemCategoryType)
        {
            case ItemCategoryType.EQUIPMENT:
                return containers[0].inventoryObj.GetHaveItemCount(item);
            case ItemCategoryType.CONSUMABLE:
                return containers[1].inventoryObj.GetHaveItemCount(item);
            case ItemCategoryType.MATERIAL:
                return containers[2].inventoryObj.GetHaveItemCount(item);
            case ItemCategoryType.QUESTITEM:
                return containers[3].inventoryObj.GetHaveItemCount(item);
        }
        return 0;
    }
}


[System.Serializable]
public class InventoryContainerInfo
{
    public InventoryCategory category;
    public InventoryObject inventoryObj;

}
