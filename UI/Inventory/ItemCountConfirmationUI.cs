using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCountConfirmationUI : UIRoot
{
    [SerializeField] private ItemCountConfirmCategory currentConfirmType = ItemCountConfirmCategory.BUY;
    [SerializeField] private StoreItem selectItem = null;
    [SerializeField] private int currentCount = 0;
    [SerializeField] private int maxCount = 0;
    [SerializeField] private int finalPrice = 0;

    [SerializeField] private TMP_InputField inputField = null;
    [SerializeField] private TextMeshProUGUI description_Text = null;
    [SerializeField] private TextMeshProUGUI finalPrice_Text = null;
    private int npcID = 0;

    public StoreItem SelectItem => selectItem;
    public int CurrentCount => currentCount;
    public int MaxCount
    {
        get { return maxCount; }
        set { maxCount = value; }
    }
    public int FinalPrice => finalPrice;
    public int NpcID => npcID;
    ///이 ㅅ스크립트는 max 등 아이템 사고 팔때 이 count UI 프리팹이 뜨는데 
    #region Event
    public delegate void OnInit(ItemCountConfirmationUI storeConfirmationUI);
    public delegate bool OnCheckConfirm(Item item, int amount, bool isRepurchase);
    public delegate void OnConfirm(ItemCountConfirmationUI storeConfirmationUI);
    public delegate void OnDropConfirm(Item item, int amount);

    public event OnInit onBuyItemInit;
    public event OnCheckConfirm onBuyCheckConfirm;
    public event OnConfirm onBuyConfirm;

    public event OnInit onSellItemInit;
    public event OnCheckConfirm onSellCheckConfirm;
    public event OnConfirm onSellConfirm;

    public event OnInit onRepurchaseItemInit;
    public event OnCheckConfirm onRepurchaseCheckConfirm;
    public event OnConfirm onRepurchaseConfirm;

    public event OnDropConfirm onDropConfirm;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public override void OpenUIWindow()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        base.OpenUIWindow();

    }

    public override void CloseUIWindow()
    {
        CloseUIWindowData();
    }


    public void SettingWindow(StoreUI storeUI)
    {
        OpenUIWindow();
        currentConfirmType = storeUI.CurrentCategory;
        selectItem = storeUI.SelectItem;
        npcID = storeUI.NpcID;
        description_Text.text = storeUI.SelectItem.GetItem.objectName;
        currentCount = 1;
        maxCount = 0;
        finalPrice = 0;

        if (currentConfirmType == ItemCountConfirmCategory.BUY)
        {
            onBuyItemInit?.Invoke(this);
            currentCount = maxCount <= 0 ? 0 : 1;
        }
        else if (currentConfirmType == ItemCountConfirmCategory.REPURCHASE)
        {
            onRepurchaseItemInit?.Invoke(this);
            currentCount = maxCount;
        }
        else
        {
            onSellItemInit?.Invoke(this);
            currentCount = CommonUIManager.Instance.playerInventory.FindInstanceIDItem(selectItem.GetItem, selectItem.GetItem.itemClip.instanceID).amount;

        }

        UpdateValues();
        inputField.Select();
    }

    public void SettingDrop(Item item, int amount)
    {
        selectItem = new StoreItem(item, maxCount);
        if(!item.itemClip.isOverlap)
        {
            maxCount = 1;
            currentCount = 1;
            DropConfirm();
            return;
        }

        OpenUIWindow();
        currentConfirmType = ItemCountConfirmCategory.DROP;

        maxCount = CommonUIManager.Instance.playerInventory.GetHaveItemCount(item);
        finalPrice = 0;
        currentCount = amount;
        Debug.Log("SettingDrop : " + item.itemClip.uiItemName);

        UpdateValues();
        inputField.Select();
    }


    #region UI Button
    public void CountUp_Btn()
    {
        currentCount = int.Parse(inputField.text);
        if (currentCount > maxCount) currentCount = maxCount;

        if (currentConfirmType == ItemCountConfirmCategory.BUY && CheckBuyConfirm(false) && currentCount < maxCount)
            currentCount++;
        else if (currentConfirmType == ItemCountConfirmCategory.REPURCHASE && CheckBuyConfirm(true) && currentCount < maxCount)
            currentCount++;
        else if (currentConfirmType == ItemCountConfirmCategory.SELL && currentCount < maxCount)
            currentCount++;
        else if (currentConfirmType == ItemCountConfirmCategory.DROP && currentCount < maxCount)
            currentCount++;
        UpdateValues();
    }


    public void CountDown_Btn()
    {
        currentCount = int.Parse(inputField.text);
        if (currentCount > maxCount) currentCount = maxCount;


        if (currentCount > 1)
            currentCount--;
        UpdateValues();
    }
    public void Max_Btn()
    {
        currentCount = maxCount <= 0 ? 0 : maxCount;
        UpdateValues();
    }

    public void Confirm_Btn()
    {
        if(currentCount <= 0)
        {
            CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("갯수가 0개 입니다.");
            return;
        }
        currentCount = int.Parse(inputField.text);
        if (currentCount > maxCount) currentCount = maxCount;

        if (currentConfirmType == ItemCountConfirmCategory.BUY)
            BuyConfirm();
        else if (currentConfirmType == ItemCountConfirmCategory.REPURCHASE)
            RepurchaseConfirm();
        else if (currentConfirmType == ItemCountConfirmCategory.SELL)
            SellConfirm();
        else if (currentConfirmType == ItemCountConfirmCategory.DROP)
            DropConfirm();
    }

    private void BuyConfirm()
    {
        if (!CheckBuyConfirm(false))
            return;

        onBuyConfirm?.Invoke(this);
        CommonUIManager.Instance.ExcuteGlobalNotifer(selectItem.GetItem.objectName + "을 " + currentCount + "개 구매하셨습니다");
        CloseUIWindow();
    }

    private void RepurchaseConfirm()
    {
        if (!CheckBuyConfirm(true))
            return;

        onRepurchaseConfirm?.Invoke(this);
        CloseUIWindow();
    }

    private void SellConfirm()
    {
        onSellConfirm?.Invoke(this);
        CloseUIWindow();
    }

    private void DropConfirm()
    {
        Debug.Log("Confirm");
        onDropConfirm?.Invoke(selectItem.GetItem, currentCount);
        CloseUIWindow();
    }

    public void Exit_Btn()
    {
        CloseUIWindow();
    }

    public void UpdateInputField()
    {
        currentCount = int.Parse(inputField.text);
        if (currentCount > maxCount) currentCount = maxCount;
        UpdateValues();
    }

    #endregion

    private bool CheckBuyConfirm(bool isRepurchase)
    {
        UpdateValues();
        if (GameManager.Instance.OwnMoney < finalPrice) return false;

        foreach (OnCheckConfirm check in onBuyCheckConfirm.GetInvocationList())
        {
            if (!check.Invoke(selectItem.GetItem, currentCount,isRepurchase))
                return false;
        }
        return true;
    }


    private void UpdateValues()
    {
        if (currentConfirmType == ItemCountConfirmCategory.BUY)
            finalPrice = currentCount * selectItem.GetItem.itemClip.buyCost;
        else if (currentConfirmType == ItemCountConfirmCategory.SELL)
            finalPrice = currentCount * selectItem.GetItem.itemClip.sellCost;
        else if (currentConfirmType == ItemCountConfirmCategory.REPURCHASE)
            finalPrice = currentCount * selectItem.GetItem.itemClip.repurchaseCost;
        inputField.text = currentCount.ToString();


        if (currentConfirmType == ItemCountConfirmCategory.BUY)
            description_Text.text = selectItem.GetItem.objectName + " " + currentCount + "개를 구매하시겠습니까?";
        else if (currentConfirmType == ItemCountConfirmCategory.SELL)
            description_Text.text = selectItem.GetItem.objectName + " " + currentCount + "개를 파시겠습니까?";
        else if (currentConfirmType == ItemCountConfirmCategory.REPURCHASE)
            description_Text.text = selectItem.GetItem.objectName + " " + currentCount + "개를 재구매하시겠습니까?";
        else if (currentConfirmType == ItemCountConfirmCategory.DROP)
            description_Text.text = selectItem.GetItem.objectName + " " + currentCount + "개를 버리시겠습니까?";


        if (currentConfirmType == ItemCountConfirmCategory.DROP)
            finalPrice_Text.text = "버릴 아이템 개수 : " + currentCount;
        else finalPrice_Text.text = "최종 금액 : " + finalPrice;

    }
}
