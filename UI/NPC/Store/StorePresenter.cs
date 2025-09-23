using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePresenter : MonoBehaviour
{
    [SerializeField] private StoreUI storeUI;
    [SerializeField] private ItemCountConfirmationUI itemCountConfirmationUI;
    [SerializeField] private ReConfirmDropUI reConfirmUI;
    [SerializeField] private StoreNpcFunction tempStoreNpcFunctions;


    private void Awake()
    {
        if (storeUI == null) storeUI = FindObjectOfType<StoreUI>();
        if (itemCountConfirmationUI == null) itemCountConfirmationUI = FindObjectOfType<ItemCountConfirmationUI>();

        QuestManager.Instance.onSetNewNpcStoreFuncCallback += SetNewStoreFunction;

        itemCountConfirmationUI.onBuyCheckConfirm += tempStoreNpcFunctions.CheckBuyItem_Inventory;
        itemCountConfirmationUI.onBuyCheckConfirm += tempStoreNpcFunctions.CheckBuyItem_Money;
        itemCountConfirmationUI.onBuyItemInit += tempStoreNpcFunctions.MaxCount_BuyItemInit;
        itemCountConfirmationUI.onBuyConfirm += tempStoreNpcFunctions.BuyItem;

        itemCountConfirmationUI.onRepurchaseCheckConfirm += tempStoreNpcFunctions.CheckBuyItem_Inventory;
        itemCountConfirmationUI.onRepurchaseCheckConfirm += tempStoreNpcFunctions.CheckBuyItem_Money;
        itemCountConfirmationUI.onRepurchaseItemInit += tempStoreNpcFunctions.MaxCount_RepurchaseInit;
        itemCountConfirmationUI.onRepurchaseConfirm += InvokeRepurchaseItem;

        itemCountConfirmationUI.onSellItemInit += tempStoreNpcFunctions.MaxCount_SellItemInit;
        itemCountConfirmationUI.onSellConfirm += InvokeSellItem;
        itemCountConfirmationUI.onDropConfirm += reConfirmUI.Setting;


        storeUI.onSellItem += itemCountConfirmationUI.SettingWindow;
        storeUI.onDoubleClick += itemCountConfirmationUI.SettingWindow;
    }

    private void OnDestroy()
    {
        QuestManager.Instance.onSetNewNpcStoreFuncCallback -= SetNewStoreFunction;
        RemoveStoreFunction();

        itemCountConfirmationUI.onBuyCheckConfirm -= tempStoreNpcFunctions.CheckBuyItem_Inventory;
        itemCountConfirmationUI.onBuyCheckConfirm -= tempStoreNpcFunctions.CheckBuyItem_Money;
        itemCountConfirmationUI.onBuyItemInit -= tempStoreNpcFunctions.MaxCount_BuyItemInit;
        itemCountConfirmationUI.onBuyConfirm -= tempStoreNpcFunctions.BuyItem;

        itemCountConfirmationUI.onRepurchaseCheckConfirm -= tempStoreNpcFunctions.CheckBuyItem_Inventory;
        itemCountConfirmationUI.onRepurchaseCheckConfirm -= tempStoreNpcFunctions.CheckBuyItem_Money;
        itemCountConfirmationUI.onRepurchaseItemInit -= tempStoreNpcFunctions.MaxCount_RepurchaseInit;
        itemCountConfirmationUI.onRepurchaseConfirm -= InvokeRepurchaseItem;

        itemCountConfirmationUI.onSellItemInit -= tempStoreNpcFunctions.MaxCount_SellItemInit;
        itemCountConfirmationUI.onSellConfirm -= InvokeSellItem;
        itemCountConfirmationUI.onDropConfirm -= reConfirmUI.Setting;


        storeUI.onSellItem -= itemCountConfirmationUI.SettingWindow;
        storeUI.onDoubleClick -= itemCountConfirmationUI.SettingWindow;

    }


    public void SetNewStoreFunction(StoreNpcFunction func)
    {
        func.OnInteract_ += storeUI.SettingStoreUI;
        func.OnExit_ += storeUI.ResetData;
        func.OnCompletedSell_ += storeUI.RepurchaseItemListSetting;
        func.OnCompletedRepurchase_ += storeUI.RepurchaseItemListSetting;
    }
    public void RemoveStoreFunction()
    {
        for (int i = 0; i < QuestManager.Instance.NpcStoreFunction.Count; i++)
        {
            QuestManager.Instance.NpcStoreFunction[i].OnInteract_ -= storeUI.SettingStoreUI;
            QuestManager.Instance.NpcStoreFunction[i].OnExit_ -= storeUI.ResetData;
            QuestManager.Instance.NpcStoreFunction[i].OnCompletedSell_ -= storeUI.RepurchaseItemListSetting;
            QuestManager.Instance.NpcStoreFunction[i].OnCompletedRepurchase_ -= storeUI.RepurchaseItemListSetting;
        }

    }

    public void InvokeSellItem(ItemCountConfirmationUI storeConfirmationUI)
    {
        int npcID = storeConfirmationUI.NpcID;
        StoreNpcFunction npcFunction = null;
        List<StoreNpcFunction> getStoreFuncs = QuestManager.Instance.NpcStoreFunction;
        for (int i = 0; i < getStoreFuncs.Count; i++)
            if (npcID == getStoreFuncs[i].NpcController.ID)
                npcFunction = getStoreFuncs[i];

        npcFunction.SellItem(storeConfirmationUI.SelectItem, storeConfirmationUI.CurrentCount, storeConfirmationUI.FinalPrice);
    }

    public void InvokeRepurchaseItem(ItemCountConfirmationUI storeConfirmationUI)
    {
        int npcID = storeConfirmationUI.NpcID;
        StoreNpcFunction npcFunction = null;
        List<StoreNpcFunction> getStoreFuncs = QuestManager.Instance.NpcStoreFunction;
        for (int i = 0; i < getStoreFuncs.Count; i++)
            if (npcID == getStoreFuncs[i].NpcController.ID)
                npcFunction = getStoreFuncs[i];

        npcFunction.RepurchaseItem(storeConfirmationUI);   
        storeUI.RepurchaceItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.Do();
    }
}
