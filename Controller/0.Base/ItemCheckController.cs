using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCheckInfo
{
    public int itemID = -1;
    public string itemName = string.Empty;
    public float itemCoolTime = -1f;
    public float currentTimer = 0f;

    public ItemCheckInfo(Item item)
    {
        itemID = item.id;
        itemName = item.objectName;
        itemCoolTime = item.itemClip.itemCoolTime;
        currentTimer = itemCoolTime;
    }
}

public class ItemCheckController : MonoBehaviour
{
    [SerializeField] private List<ItemCheckInfo> itemCoolTimeList = new List<ItemCheckInfo>();

    public List<ItemCheckInfo> ItemCoolTimeList => ItemCoolTimeList;


    public void CheckItemCoolTime()
    {
        if (itemCoolTimeList.Count <= 0) return;

        for (int i = 0; i < itemCoolTimeList.Count; i++)
        {
            itemCoolTimeList[i].currentTimer -= Time.deltaTime;
            if (itemCoolTimeList[i].currentTimer < 0f)
                itemCoolTimeList.Remove(itemCoolTimeList[i]);
        }
    }

    public void AddCoolTimeList(Item item)
    {
        ItemCheckInfo info = new ItemCheckInfo(item);
        itemCoolTimeList.Add(info);
    }

    public bool CanAddItemToList(Item item)
    {
        for (int i = 0; i < itemCoolTimeList.Count; i++)
        {
            if (item.id == itemCoolTimeList[i].itemID)
                return false;
        }
        return true;
    }

    public bool ItemIsCoolTime(Item item) => ItemIsCoolTime(item.id);

    public bool ItemIsCoolTime(int id)
    {
        for (int i = 0; i < itemCoolTimeList.Count; i++)
        {
            if (id == itemCoolTimeList[i].itemID)
                return true;
        }
        return false;
    }

    public ItemCheckInfo GetCoolTimeItemInfo(int id)
    {
        foreach (ItemCheckInfo info in itemCoolTimeList)
            if (info.itemID == id)
                return info;

        return null;
    }
}
