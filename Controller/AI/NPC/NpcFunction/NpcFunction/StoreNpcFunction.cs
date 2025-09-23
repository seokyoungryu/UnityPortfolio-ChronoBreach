using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoreNpcFunction : BaseNpcFunction
{
    [SerializeField] private List<ItemList> sellItemList;
    [SerializeField] private List<StoreItem> repurchaseItemList = new List<StoreItem>();
    [SerializeField] private QuestCategory category = null;

    public IReadOnlyList<ItemList> SellItemList => sellItemList;
    public List<StoreItem> RepurchaseItemList => repurchaseItemList;

    #region Events
    public delegate void OnInteracter(StoreNpcFunction storeNpcFunction);
    public delegate void OnExit();
    public delegate void OnCompleted(StoreNpcFunction storeNpcFunction);

    private event OnInteracter onInteract;
    public event OnInteracter OnInteract_
    {
        add
        {
            if (onInteract == null || !onInteract.GetInvocationList().Contains(value))
                onInteract += value;
        }
        remove
        {
            onInteract -= value;
        }
    }
    private event OnExit onExit;
    public event OnExit OnExit_
    {
        add
        {
            if (onExit == null || !onExit.GetInvocationList().Contains(value))
                onExit += value;
        }
        remove
        {
            onExit -= value;
        }
    }
    private event OnCompleted onCompletedBuy;
    public event OnCompleted OnCompletedBuy_
    {
        add
        {
            if (onCompletedBuy == null || !onCompletedBuy.GetInvocationList().Contains(value))
                onCompletedBuy += value;
        }
        remove
        {
            onCompletedBuy -= value;
        }
    }
    private event OnCompleted onCompletedSell;
    public event OnCompleted OnCompletedSell_
    {
        add
        {
            if (onCompletedSell == null || !onCompletedSell.GetInvocationList().Contains(value))
                onCompletedSell += value;
        }
        remove
        {
            onCompletedSell -= value;
        }
    }
    private event OnCompleted onCompletedRepurchase;
    public event OnCompleted OnCompletedRepurchase_
    {
        add
        {
            if (onCompletedRepurchase == null || !onCompletedRepurchase.GetInvocationList().Contains(value))
                onCompletedRepurchase += value;
        }
        remove
        {
            onCompletedRepurchase -= value;
        }
    }
    #endregion


    /// <summary>
    /// 플레이어가 interact 할시 실행될 프로세스.
    /// </summary>
    public override void ExcuteInteract()
    {
        base.ExcuteInteract();
        SettingManager.Instance.UseScreenTouch = false;
        SettingManager.Instance.CanExcuteESC = false;

        if (npcQuestGiver?.WaitForCompleteQuests.Count > 0)
        {
            npcController.onCompletedQuest?.Invoke(npcController);
            return;
        }
        else if (npcQuestGiver?.canGiveQuestCount > 0)
        {
            npcController.onRegisterSelectQuest?.Invoke(npcController);
            return;
        }

        if (category != null && QuestManager.Instance.TargetTaskInActiveQuests(category, NpcController.QuestReporter?.Target)) return;

        Debug.Log("♣♣♣♣♣♣ onExit : " + onExit?.GetInvocationList().Length);
        Debug.Log("♣♣♣♣♣♣ onInteract : " + onInteract?.GetInvocationList().Length);

        onExit?.Invoke();
        onInteract?.Invoke(this);
        npcController.QuestReporter?.ReceiveReport(category.CodeName);
    }

    public override void ExitInteract()
    {
        GameManager.Instance.canUseCamera = true;
        SettingManager.Instance.UseScreenTouch = true;
        SettingManager.Instance.CanExcuteESC = true;
        onExit?.Invoke();
    }


    #region UI Process

    public bool CheckBuyItem_Inventory(Item item, int amount, bool isRepurchase)
    {
        InventoryContainterUI inventory = CommonUIManager.Instance.playerInventory;
        if (inventory.GetRemainingItemCount(item) >= amount && inventory.GetEmptySlotCount(item) > 0)
            return true;

        //이 경우 Instantiate나 objectpool로   알림창 생성. 이 알림창은 몇초뒤 알아서 들어감.
        Debug.Log("CheckBuyItem_Inventory  - 실행안됨");
        return false;
    }

    public bool CheckBuyItem_Money(Item item, int amount, bool isRepurchase)
    {
        int price = isRepurchase ? item.itemClip.repurchaseCost : item.itemClip.buyCost;
        int finalPrice = price * amount;
        if (finalPrice <= GameManager.Instance.OwnMoney)
            return true;

        CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("돈이 부족합니다.");
        return false;
    }


    public void BuyItem(ItemCountConfirmationUI confirm)
    {
        Item buyItem = confirm.SelectItem.GetItem;
        ItemContainer itemContainer = new ItemContainer(buyItem, confirm.CurrentCount);
        if (!CommonUIManager.Instance.playerInventory.CheckCanAddItems(itemContainer)) return;

        GameManager.Instance.SetMinusOwnMoney(confirm.FinalPrice);
        if (buyItem.itemClip.itemCategoryType == ItemCategoryType.EQUIPMENT)
        {
            for (int i = 0; i < confirm.CurrentCount; i++)
            {
                Item equipmentItem = ItemManager.Instance.GenerateItem(buyItem);
                equipmentItem.itemClip.SetItemInstanceID();
                CommonUIManager.Instance.playerInventory.AddItem(equipmentItem, 1);
            }
        }
        else
            CommonUIManager.Instance.playerInventory.AddItem(buyItem, confirm.CurrentCount);

        QuestManager.Instance.ReceiveReport(QuestCategoryDefines.ONLY_ITEM_BUY, (int)buyItem.itemClip.id, confirm.CurrentCount);

        onCompletedBuy?.Invoke(this);
    }

    public void RepurchaseItem(ItemCountConfirmationUI confirm)
    {
        Item buyItem = confirm.SelectItem.GetItem;
        ItemContainer itemContainer = new ItemContainer(buyItem, confirm.CurrentCount);
        if (!CommonUIManager.Instance.playerInventory.CheckCanAddItems(itemContainer)) return;

        GameManager.Instance.SetMinusOwnMoney(confirm.FinalPrice);
        CommonUIManager.Instance.playerInventory.AddItem(buyItem, confirm.CurrentCount);

        for (int i = 0; i < repurchaseItemList.ToArray().Length; i++)
        {
            if (repurchaseItemList[i].GetItem.itemClip.id == buyItem.itemClip.id)
            {
                repurchaseItemList[i].Amount -= confirm.CurrentCount;
                if (repurchaseItemList[i].Amount <= 0)
                    repurchaseItemList.RemoveAt(i);
            }
        }

        onCompletedRepurchase?.Invoke(this);
    }

    public void MaxCount_RepurchaseInit(ItemCountConfirmationUI storeConfirmationUI)
    {
        storeConfirmationUI.MaxCount = MaxCanRepurchaseItem(storeConfirmationUI.SelectItem);
    }

    private int MaxCanRepurchaseItem(StoreItem item)
    {
        InventoryContainterUI inventory = CommonUIManager.Instance.playerInventory;
        int canBuyCount;
        int remainingItemCount = inventory.GetRemainingItemCount(item.GetItem);
        int amount = item.Amount;
        if (item.GetItem.itemClip.repurchaseCost <= 0) canBuyCount = amount;
        else canBuyCount =  GameManager.Instance.OwnMoney / (int)(item.GetItem.itemClip.repurchaseCost);

        if (amount <= remainingItemCount && amount <= canBuyCount)
            return amount;
        else if (amount >= remainingItemCount)
            return remainingItemCount;
        else if (amount >= canBuyCount)
            return canBuyCount;

        return 0;
    }


    public void MaxCount_BuyItemInit(ItemCountConfirmationUI storeConfirmationUI)
    {
        storeConfirmationUI.MaxCount = MaxCanBuyItem(storeConfirmationUI.SelectItem.GetItem);
    }

    private int MaxCanBuyItem(Item item)
    {
        InventoryContainterUI inventory = CommonUIManager.Instance.playerInventory;
        int remainingItemCount = inventory.GetRemainingItemCount(item);
        int canBuyCount = GameManager.Instance.OwnMoney / item.itemClip.buyCost;

        Debug.Log("remainingItemCount  " + remainingItemCount + "canBuyCount  " + canBuyCount);
        if (canBuyCount <= remainingItemCount)
            return canBuyCount;
        else if (canBuyCount > remainingItemCount)
            return remainingItemCount;

        return 0;
    }


    public void MaxCount_SellItemInit(ItemCountConfirmationUI storeConfirmationUI)
    {
        storeConfirmationUI.MaxCount = MaxCanSellItem(storeConfirmationUI.SelectItem.GetItem);
    }

    private int MaxCanSellItem(Item item)
    {
        return CommonUIManager.Instance.playerInventory.GetHaveItemCount(item);
    }

    public void SellItem(StoreItem item, int amount, int finalPrice)
    {
        item.Amount = amount;
        GameManager.Instance.SetPlusOwnMoney(finalPrice);
        CommonUIManager.Instance.playerInventory.RemoveItemInstanceID(item.GetItem, amount, item.GetItem.itemClip.instanceID);

        bool isHave = false;
        foreach (StoreItem sellItem in repurchaseItemList)
        {
            if (sellItem.GetItem.itemClip.id == item.GetItem.itemClip.id && item.GetItem.itemClip.itemCategoryType != ItemCategoryType.EQUIPMENT)
            {
                sellItem.Amount += item.Amount;
                isHave = true;
            }
        }

        if (!isHave)
            repurchaseItemList.Add(new StoreItem(item));

        onCompletedSell?.Invoke(this);
    }
    #endregion

}
