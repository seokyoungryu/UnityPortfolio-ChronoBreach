using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Item Reward", fileName = "ItemReward_")]
public class RewardItem : Reward
{
    [SerializeField] private ItemList itemList = ItemList.None;
    [SerializeField] private int count = 0;

    public Item rewardItem => ItemManager.Instance.GenerateItem((int)itemList);
    public Sprite Icon => rewardItem.itemClip.itemTexture;
    public ItemList ItemList { get { return itemList; } set { itemList = value; } }
    public int Count { get { return count; } set { count = value; } }

    public override void Giver(Quest quest)
    {
        Debug.Log("Give : PlayerInven : " + CommonUIManager.Instance.playerInventory);
        BaseItemClip clip = ItemManager.Instance.GetItemClip((int)itemList);
        if (clip.isOverlap)
        {
            Item item = ItemManager.Instance.GenerateItem((int)itemList);
            CommonUIManager.Instance.playerInventory.AddItem(new ItemContainer(item, count));
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                Item item = ItemManager.Instance.GenerateItem((int)itemList);
                CommonUIManager.Instance.playerInventory.AddItem(new ItemContainer(item, 1));
            }
        }   


    }
    public override int GetIntValue()
    {
        return count;
    }

    public override void Remove()
    {
        BaseItemClip clip = ItemManager.Instance.GetItemClip((int)itemList);
        if (clip != null)
        {
            Item item = ItemManager.Instance.GenerateItem((int)itemList);
            CommonUIManager.Instance.playerInventory.RemoveItem(item, count);
        }
    }


}
