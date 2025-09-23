using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using UnityEngine.EventSystems;

public enum ItemCountConfirmCategory
{
    BUY =0,
    SELL =1,
    REPURCHASE =2,
    DROP = 3,
}

public class StoreUI : UIRoot 
{
    [Header("Sound")]
    [SerializeField] private SoundList clickSound;

    [Header("[0] : BuyCategory, [1] SellCategory")]
    [SerializeField] private ItemCountConfirmCategory currentCategory = ItemCountConfirmCategory.BUY;
    [SerializeField] private Transform[] category = null;

    [Header("Setting")]
    [SerializeField] private StoreItem selectedItem = null;
    [SerializeField] private RectTransform content = null;
    [SerializeField] private RectTransform buyItemList_Tr = null;
    [SerializeField] private RectTransform repurchaseItemList_Tr = null;
    [SerializeField] private StoreItem storeItemUI_Prefab;
    [SerializeField] private float mouseClickLimitTimer = 0;
   
    [SerializeField] private int npcID = -1;
    private bool isClickCoroutineStart = false;
    private int mouseClickCount = 0;
    private float mouseClickTimer = 0;
    private float scrollCurrentValue = 0f; //마우스 휠 스크롤 Height범위.
    private float scrollMaxValue = 0f; //마우스 휠 스크롤 Height범위.

    [Header("Scroll View Settings")]
    [SerializeField] private float scrollIntensity = 200f;
    [SerializeField] private float smoothSpeed = 4f;

    [SerializeField] private List<StoreItem> childBuyItem = new List<StoreItem>();
    [SerializeField] private List<StoreItem> childRepurchaseItem = new List<StoreItem>();


    public int NpcID => npcID;
    public StoreItem SelectItem => selectedItem;
    public RectTransform RepurchaceItemList_Tr => repurchaseItemList_Tr;
    public ItemCountConfirmCategory CurrentCategory => currentCategory;

    #region Event
    public delegate void OnDoubleClick(StoreUI storeUI);
    public delegate void OnSellItem(StoreUI storeUI);

    public event OnDoubleClick onDoubleClick;
    public event OnSellItem onSellItem;
    #endregion




    protected override void Start()
    {
        UIHelper.AddEventTrigger(category[0].gameObject, EventTriggerType.PointerClick, delegate { OnBuyCategoryPointerClick(category[0].gameObject); });
        UIHelper.AddEventTrigger(category[1].gameObject, EventTriggerType.PointerClick, delegate { OnSellCategoryPointerClick(category[1].gameObject); });
        UIHelper.AddEventTrigger(repurchaseItemList_Tr.gameObject, EventTriggerType.PointerEnter, delegate { OnUIRootEnter(repurchaseItemList_Tr.gameObject); });
        UIHelper.AddEventTrigger(repurchaseItemList_Tr.gameObject, EventTriggerType.PointerExit, delegate { OnUIRootExit(repurchaseItemList_Tr.gameObject); });
        gameObject.SetActive(false);
    }


    private void Update()
    {
        if (currentCategory == ItemCountConfirmCategory.BUY)
        {
            scrollMaxValue = buyItemList_Tr.rect.height - content.rect.height;
            if(buyItemList_Tr.rect.height < content.rect.height)
                scrollMaxValue = 0f;
        }
        else if (currentCategory == ItemCountConfirmCategory.SELL)
        {
            scrollMaxValue = repurchaseItemList_Tr.rect.height - content.rect.height;
            if (repurchaseItemList_Tr.rect.height < content.rect.height)
                scrollMaxValue = 0f;
        }

        scrollCurrentValue -= Input.GetAxisRaw("Mouse ScrollWheel") * scrollIntensity;
        scrollCurrentValue = Mathf.Clamp(scrollCurrentValue, 0, scrollMaxValue);

        if (currentCategory == ItemCountConfirmCategory.BUY)
            buyItemList_Tr.anchoredPosition 
                = Vector3.Lerp(buyItemList_Tr.anchoredPosition, Vector3.up * scrollCurrentValue, Time.deltaTime * smoothSpeed);
        else if (currentCategory == ItemCountConfirmCategory.SELL)
            repurchaseItemList_Tr.anchoredPosition 
                = Vector3.Lerp(repurchaseItemList_Tr.anchoredPosition, Vector3.up * scrollCurrentValue, Time.deltaTime * smoothSpeed);
    }

    public void ScrollReset()
    {
        Debug.Log("스크롤 :" + scrollCurrentValue);
        scrollCurrentValue = 0;
    }



    public void SettingStoreUI(StoreNpcFunction storeNpcFunction)
    {
        OpenUIWindow();
        CommonUIManager.Instance.playerInventory.OpenUIWindow();
        CategorySetting();

        foreach (ItemList item in storeNpcFunction.SellItemList)
        {
            StoreItem storeItem = Instantiate(storeItemUI_Prefab, buyItemList_Tr);
            storeItem.Setting(ItemManager.Instance.GenerateItem((int)item),-1,ItemCountConfirmCategory.BUY);
            UIHelper.AddEventTrigger(storeItem.ItemImg.gameObject, EventTriggerType.PointerEnter, delegate { OnItemInformationPointerEnter(storeItem.GetItem); });
            UIHelper.AddEventTrigger(storeItem.ItemImg.gameObject, EventTriggerType.PointerExit, delegate { OnItemInformationPointerExit(storeItem.GetItem); });
            UIHelper.AddEventTrigger(storeItem.ItemImg.gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(storeItem.gameObject, false); });
            UIHelper.AddEventTrigger(storeItem.gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(storeItem.gameObject, false); });
            Debug.Log(item.ToString() + " - 생성");
            childBuyItem.Add(storeItem);
        }

        ResetSellList(storeNpcFunction.NpcController.ID);
        buyItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.Do();
        repurchaseItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.Do();
        ScrollReset();
    }


    public void ResetData()                          
    {
        selectedItem = null;
        mouseStayInIconTimer = 0f; 
        mouseClickCount = 0;
        currentCategory = ItemCountConfirmCategory.BUY;

        foreach (StoreItem tr in childBuyItem)
        {
            tr.gameObject.SetActive(false);
            Destroy(tr.gameObject);
        }

        buyItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.ResetData();
        childBuyItem.Clear();

        buyItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.Do();
        CloseUIWindow();
    }

    //이건 아이콘 클릭시 재구매할지 물어보는것.
    public void RepurchaseItemListSetting(StoreNpcFunction storeNpcFunction)
    {
        childRepurchaseItem.Clear();
        foreach (Transform child in repurchaseItemList_Tr.transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        foreach (StoreItem item in storeNpcFunction.RepurchaseItemList)
        {
            StoreItem storeItem = Instantiate(storeItemUI_Prefab, repurchaseItemList_Tr);
            storeItem.Setting(item.GetItem,item.Amount,ItemCountConfirmCategory.REPURCHASE);
            UIHelper.AddEventTrigger(storeItem.ItemImg.gameObject, EventTriggerType.PointerEnter, delegate { OnItemInformationPointerEnter(storeItem.GetItem); });
            UIHelper.AddEventTrigger(storeItem.ItemImg.gameObject, EventTriggerType.PointerExit, delegate { OnItemInformationPointerExit(storeItem.GetItem); });
            UIHelper.AddEventTrigger(storeItem.ItemImg.gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(storeItem.gameObject, true); });
            UIHelper.AddEventTrigger(storeItem.gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(storeItem.gameObject, true); });

            childRepurchaseItem.Add(storeItem);
        }


        repurchaseItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.Do();
        //OnSellCategoryPointerClick(null);   //이거하니까 무시안됨 머지.
        currentCategory = ItemCountConfirmCategory.SELL;
        Debug.Log("이거 클릭됨");

    }

    /// <summary>
    /// 인벤토리에서 호출함.
    /// 여기엔 StoreConfirm의 init 호출
    /// -> Init에서 관련 Text, Item 셋팅하고 
    /// </summary>
    public void SellItem(Item item)
    {
        currentCategory = ItemCountConfirmCategory.SELL;
        StoreItem tmpItem = new StoreItem(item);
        selectedItem = tmpItem;
        onSellItem?.Invoke(this);
    }

    private void ResetSellList(int newNpcId)
    {
        if (npcID == newNpcId) return;

        npcID = newNpcId;
        foreach (Transform child in repurchaseItemList_Tr.transform)
           Destroy(child.gameObject);

        childRepurchaseItem.Clear();

    }

    private void CategorySetting()
    {
        switch (currentCategory)
        {
            case ItemCountConfirmCategory.BUY:
                buyItemList_Tr.gameObject.SetActive(true);
                buyItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.Do();
                repurchaseItemList_Tr.gameObject.SetActive(false);
                break;
            case ItemCountConfirmCategory.SELL:
                buyItemList_Tr.gameObject.SetActive(false);
                repurchaseItemList_Tr.gameObject.SetActive(true);
                repurchaseItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.Do();
                break;
        }
        buyItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.Do();
        repurchaseItemList_Tr.GetComponent<CustomGridLayoutGroup>()?.Do();
        ScrollReset();
    }

    #region Pointer

    private void OnBuyCategoryPointerClick(GameObject go)
    {
        if (currentCategory == ItemCountConfirmCategory.BUY)
            return;

        currentCategory = ItemCountConfirmCategory.BUY;
        scrollCurrentValue = 0f;
        buyItemList_Tr.anchoredPosition = Vector3.up * scrollCurrentValue;

        CategorySetting();
    }

    private void OnSellCategoryPointerClick(GameObject go)
    {
        if (currentCategory == ItemCountConfirmCategory.SELL)
            return;

        currentCategory = ItemCountConfirmCategory.SELL;
        scrollCurrentValue = 0f;
        repurchaseItemList_Tr.anchoredPosition = Vector3.up * scrollCurrentValue;

        CategorySetting();
    }

    
    private void OnPointerClick(GameObject go, bool isRepurchase)
    {
        if (!isClickCoroutineStart)
            StartCoroutine(CheckDoubleClick());
        if (isRepurchase)
            currentCategory = ItemCountConfirmCategory.REPURCHASE;

        selectedItem = go.GetComponent<StoreItem>();
        int cost = isRepurchase ? selectedItem.GetItem.itemClip.repurchaseCost : selectedItem.GetItem.itemClip.buyCost;
        mouseClickCount++;


        if (mouseClickCount >= 2)
        {
            if (GameManager.Instance.OwnMoney < cost)
                CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("돈이 부족합니다");
            else if (CommonUIManager.Instance.playerInventory.CheckCanAddItems(selectedItem.GetItem, selectedItem.Amount))
                onDoubleClick?.Invoke(this);
        }
    }



    #endregion

    private IEnumerator CheckDoubleClick()
    {
      
        isClickCoroutineStart = true;
        while (mouseClickTimer <= mouseClickLimitTimer)
        {
            mouseClickTimer += Time.deltaTime;
            yield return null;
        }
        mouseClickCount = 0;
        mouseClickTimer = 0;
        isClickCoroutineStart = false;

    }

}
