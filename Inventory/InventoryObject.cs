using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Inventory/Inventory Object")]
public class InventoryObject : ScriptableObject
{
    public ContainerType type;
    public InventorySaveLoad SaveLoadData;
    public InventorySlot[] slots;


    public int GetEmptySlotCount()
    {
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item.id < 0)
                count++;
        }

        return count;
    }

    public bool CanJoinAmount(Item itemTo, Item targetJoinItem)
    {
        if (itemTo.itemClip.id != targetJoinItem.itemClip.id) return false;

        InventorySlot slot1 = FindInstanceID(itemTo, itemTo.itemClip.instanceID);
        InventorySlot slot2 = FindInstanceID(targetJoinItem, targetJoinItem.itemClip.instanceID);

        if (slot2.item.itemClip.maxOverlapCount >= (slot1.amount + slot2.amount))
        {
            Debug.Log("캔 조인 : " + (slot1.amount + slot2.amount));
            return true;
        }
        Debug.Log($"cant 조인 {slot2.item.itemClip.maxOverlapCount} : " + (slot1.amount + slot2.amount));

        return false;
    }

    public void JoinAmount(Item itemTo, Item targetJoinItem)
    {
        if (!CanJoinAmount(itemTo, targetJoinItem)) return;

        InventorySlot slot1 = FindInstanceID(itemTo, itemTo.itemClip.instanceID);
        InventorySlot slot2 = FindInstanceID(targetJoinItem, targetJoinItem.itemClip.instanceID);

        slot2.AddAmount(slot1.amount);
        slot1.RemoveItem();

    }


    public bool AddItem(Item item, int amount)
    {
        if (GetEmptySlotCount() <= 0)
            return false;

        if(!item.itemClip.isOverlap)  // 단일 아이템일 경우.
        {
            for (int i = 0; i < amount; i++)
            {
                InventorySlot slot = GetEmptySlot();
                if (slot == null)
                    return false;
                else
                {
                    Debug.Log("item Add - " + item.itemClip.uiItemName);
                    slot?.AddItem(item, 1);
                }
            }
      
            return true;
        }
      
        //밑에는 IsOverlap일 경우
        List<InventorySlot> findSlots = FindItemInInventorys(item, true);
        if (findSlots == null || !item.itemClip.isOverlap || findSlots.Count <= 0)
        {
            float division = amount / item.itemClip.maxOverlapCount;
            if (division <= 0 || amount == item.itemClip.maxOverlapCount)
                GetEmptySlot().AddItem(item, amount);
            else
            {
                int remainingAmount = amount;
                while (remainingAmount > 0)
                {
                    if (remainingAmount >= item.itemClip.maxOverlapCount)
                    {
                        Item newItem = ItemManager.Instance.GenerateItem(item);
                        GetEmptySlot().AddItem(newItem, item.itemClip.maxOverlapCount);
                        remainingAmount -= item.itemClip.maxOverlapCount;
                    }
                    else
                    {
                        Item newItem = ItemManager.Instance.GenerateItem(item);
                        GetEmptySlot().AddItem(newItem, remainingAmount);
                        remainingAmount = 0;
                    }
                }
            }
        }
        else  //여기선 item슬롯에 amount를 maxOverlap까지 더하고 그래도 자리가 남으면 남은 갯수를   새로운 슬롯에 저장하는식.
        {
            int remainingAmount = amount;
            for (int i = 0; i < findSlots.Count; i++)
            {
                if (remainingAmount <= 0) continue;
                if (item.itemClip.maxOverlapCount > findSlots[i].amount)
                {
                    int leftCount = item.itemClip.maxOverlapCount - findSlots[i].amount;
                    if (remainingAmount >= leftCount)
                    {
                        findSlots[i].amount += leftCount;
                        remainingAmount -= leftCount;
                    }
                    else
                    {
                        findSlots[i].amount += remainingAmount;
                        remainingAmount = 0;
                    }
                }
            }

            while (remainingAmount > 0)
            {
                if (GetEmptySlot() == null) return false;

                if (remainingAmount >= item.itemClip.maxOverlapCount)
                {
                    Item newItem = ItemManager.Instance.GenerateItem(item);
                    GetEmptySlot().AddItem(newItem, item.itemClip.maxOverlapCount);
                    remainingAmount -= item.itemClip.maxOverlapCount;
                }
                else
                {
                    Item newItem = ItemManager.Instance.GenerateItem(item);
                    GetEmptySlot().AddItem(newItem, remainingAmount);
                    remainingAmount = 0;
                }
            }

        }


        return true;
    }

    /// <summary>
    /// 인스턴스 ID 를 찾아서 해당 슬롯부터 삭제.
    /// </summary>
    public bool RemoveItem(Item item, int amount, int instanceID)
    {
        Debug.Log("0이거 실행 instance버림 : " + amount);

        int remainingAmount = amount;
        InventorySlot findSlot = FindInstanceID(item, instanceID);
       // if (findSlot != null && findSlot.amount < amount) amount = findSlot.amount; 

        if (findSlot != null && findSlot.amount >= amount)
        {
            Debug.Log("1이거 실행 instance버림 : " + amount);
            findSlot.amount -= amount;
            remainingAmount = 0;
            if (findSlot.amount <= 0) findSlot.RemoveItem();
            return true;
        }
        else if (findSlot != null && findSlot.amount < amount)
        {
            int deleteCount = findSlot.amount <= remainingAmount ? findSlot.amount : remainingAmount;
            remainingAmount -= deleteCount;
            findSlot.amount -= deleteCount;

            if (findSlot.amount <= 0)
                findSlot.RemoveItem();
            Debug.Log("2이거 실행 instance버림 : " + amount);
        }

        if (remainingAmount > 0)
        {
            Debug.Log("이거 실행 버림 : " + remainingAmount);
            RemoveItem(item, remainingAmount);
        }

        return true;
    }

    /// <summary>
    ///  첫번째 Item 슬롯부터 순서대로 삭제.
    /// </summary>
    public bool RemoveItem(Item item, int amount)
    {
        if (FindItemInInventorys(item, false).Count <= 0) return false;
        int remainingCount = amount;
        foreach (InventorySlot slot in FindItemInInventorys(item,false))
        {
            int deleteCount = slot.amount <= remainingCount ? slot.amount : remainingCount;
            remainingCount -= deleteCount;
            slot.amount -= deleteCount;

            if (slot.amount <= 0)
                slot.RemoveItem();
        }                                                            
        return true;
    }


    public InventorySlot FindInstanceID(Item item, int instanceID)
    {
        foreach (InventorySlot slot in slots)
            if (slot.item.itemClip != null && slot.item.itemClip.instanceID == instanceID && slot.item.id == item.id)
                return slot;
        return null;
    }



    public int GetRemainingCount(Item item)
    {
        if (item.itemClip.isOverlap)
            return GetRemainingCount_Overlap(item);
        else
            return GetRemainingCount_OnlyOne(item);
    }

    private int GetRemainingCount_Overlap(Item item)
    {
        if (item == null)
            return -1;

        int emptySlotCount = GetEmptySlotCount();
        List<InventorySlot> itemSlots = FindItemInInventorys(item, true);
        int resultRemainingCount = (emptySlotCount * item.itemClip.maxOverlapCount);

        if (itemSlots != null)
            foreach (InventorySlot slot in itemSlots)
                if (slot != null)
                    resultRemainingCount += slot.item.itemClip.maxOverlapCount - slot.amount;

        return resultRemainingCount;
    }

    private int GetRemainingCount_OnlyOne(Item item)
    {
        if (item == null)
            return -1;
        return GetEmptySlotCount();
    }

    private InventorySlot FindItemInInventory(Item item , bool remainingSlot = false)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!remainingSlot && slot.item.id == item.id)
                return slot;
            else if(remainingSlot && slot.item.id == item.id && slot.amount < item.itemClip.maxOverlapCount)
                return slot;
        }
        return null;
    }

    private List<InventorySlot> FindItemInInventorys(Item item, bool findRemainingItem = false)
    {
        List<InventorySlot> findInvens = new List<InventorySlot>();
        foreach (InventorySlot slot in slots)
        {
            if (!findRemainingItem && slot.item.id == item.id)
                findInvens.Add(slot);
            else if (findRemainingItem && slot.item.id == item.id && slot.amount < item.itemClip.maxOverlapCount)
                findInvens.Add(slot);
        }
        return findInvens;
    }

    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
            if (slots[i].item.id  < 0)
                return slots[i];
        return null;
    }

    public int GetHaveItemCount(Item item)
    {
        int haveCount = 0;
        foreach (InventorySlot findItem in FindItemInInventorys(item, false))
            haveCount += findItem.amount;

        return haveCount;
    }

    public bool SwapItemSlot(InventorySlot slotA, InventorySlot slotB)
    {
        if (slotA == slotB) return false;

        if (slotA.CanPlaceInSlot(slotB.item) && slotB.CanPlaceInSlot(slotA.item))
        {
            InventorySlot tempSlot = new InventorySlot(slotB.item, slotB.amount);
            slotB.UpdateSlot(slotA.item, slotA.amount);
            slotA.UpdateSlot(tempSlot.item, tempSlot.amount);
            return true;
        }

        return false;
    }


    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].UpdateSlot(slots[i].item, slots[i].amount);
        }
    }


    [ContextMenu("저장 경로 업데이트")]
    public void SavePathUpdate()
    {
        SaveLoadData.fullPath = Application.persistentDataPath + SaveLoadData.fileName;
    }


}
