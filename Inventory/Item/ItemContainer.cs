using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemContainer
{
    [SerializeField] private bool isMoney = false;
    [SerializeField] private Item item = null;
    [SerializeField] private int amount = 0;
    [SerializeField] private Reward reward;

    public bool IsMoney => isMoney;
    public Item Item => item;
    public int Amount => amount;
    public Reward Reward => reward;
    public ItemContainer(int moneyAmount)
    {
        this.isMoney = true;
        this.amount = moneyAmount;
    }

    public ItemContainer(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public ItemContainer(RewardItem item)
    {
        this.item = item.rewardItem;
        this.amount = item.Count;
    }
    public ItemContainer(ItemRewardData reward)
    {
        this.item = ItemManager.Instance.GenerateItem((int)reward.Item);
        this.amount = reward.ItemCount;

    }

    public ItemContainer(Reward reward)
    {
        this.reward = reward;
    }
}
