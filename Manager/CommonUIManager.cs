using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CommonUIManager : Singleton<CommonUIManager>
{
    [SerializeField] private GameObject canvas = null;
    private DialogUI dialogUI = null;

    [Header("Screen Touch")]
    [SerializeField] private Transform screenTouchUI = null;

    [Header("Active UI")]
    [SerializeField] private UIRootOrderByEventTrigger orderByEt = null;
    [SerializeField] private KeyRegister keyRegister = null;
    [SerializeField] private List<UIRoot> activeUI = new List<UIRoot>();


    [Header("HUD")]
    public Transform playerHUD = null;
    public Transform npcHUD = null;
    public ItemInfomationUI itemInfomationUI = null;
    public InteractUI interactUI = null;
    public Transform uiEffectParent = null;
    public DungeonSuccessScoreUI successScoreUI;
    public DungeonFailScoreUI failScoreUI;
    public LevelUpUIContainer levelUpUIContainer;
    public GlobalAppearBossHPUI globalAppearBossHpUI;
    public AppearBossIntro appearBossIntro;
    public EntryPracticeModeUI entryPracticeModeUI;
    public StoreUI storeUI;

    [Header("SaveDatas")]
    public QuickSlotSaveLoadData quickSlotSaveDt = null;

    [Header("Player Reference")]
    public InventoryContainterUI playerInventory = null;
    public EquipmentUI equipmentUI = null;
    public bool isItemInfomationOpen = false;

    [Header("Global UI")]
    [SerializeField] private List<GlobalNotifierContainer> globalNotifierContainers = new List<GlobalNotifierContainer>();

    [Header("Floating Text")]
    [SerializeField] private FloatingDamagedTextContainer floatingTextContainer = null;

    [Header("Global Post")]
    public GlobalPost globalPost = null;

    [Header("Item Gain List")]
    [SerializeField] private float itemGainDelayTime = 0.3f;
    private Queue<ItemContainer> itemOrderList = new Queue<ItemContainer>();
    private bool isExcuteGainList = false;

    [Header("Item")]
    [SerializeField] private ItemCountConfirmationUI itemCountConfirmUI = null;

    public GameObject Canvas => canvas;
    public InteractUI InteractUI => interactUI;
    public LevelUpUIContainer LevelUpUI => levelUpUIContainer;

    public int ActiveUICount => activeUI.Count;
    #region Events
    public delegate void OnScreenTouch();
    public delegate void OnExcuteDialog( DialogFile dialogFile, int excuteId, DialogState state);

    public event OnScreenTouch onExcuteScreenTouch;
    public event OnExcuteDialog onExcuteDialog;
    #endregion


    protected override void Awake()
    {
        if (orderByEt == null) orderByEt = FindObjectOfType<UIRootOrderByEventTrigger>();
        if (keyRegister == null) keyRegister = GetComponent<KeyRegister>();
        if (playerInventory == null) playerInventory = FindObjectOfType<InventoryContainterUI>();
        if (itemInfomationUI == null) itemInfomationUI = FindObjectOfType<ItemInfomationUI>();
        if (interactUI == null) interactUI = FindObjectOfType<InteractUI>();
        if (dialogUI == null) dialogUI = FindObjectOfType<DialogUI>();
        if (entryPracticeModeUI == null) entryPracticeModeUI = FindObjectOfType<EntryPracticeModeUI>();
        if (equipmentUI == null) equipmentUI = FindObjectOfType<EquipmentUI>();
        if (storeUI == null) storeUI = FindObjectOfType<StoreUI>();

        Debug.Log("CommonUIManager Awake!!!!");

        onExcuteScreenTouch += AllCloseActiveUIWindow;
        SetScreenTouch(screenTouchUI.gameObject);
    }

    private void OnDestroy()
    {
        onExcuteScreenTouch -= AllCloseActiveUIWindow;
    }


    #region Floating Damaged Text
    public void ExcuteFloatingDamagedText(int damage, FloatingType floatingType, Transform targetTransform)
        => floatingTextContainer.ExcuteFloatingDamagedText(damage, floatingType, targetTransform);

    #endregion

    #region Global Notifier
    public void ExcuteGlobalNotifer(string notifierString, SoundList sound = SoundList.None)
    {
        GlobalNotifierContainer container = FindNotifierContainer(GlobalNotifierType.NORMAL);
        SoundManager.Instance.PlayEffect(container.NotifierSound);

        GameObject notifer = ObjectPooling.Instance.GetOBP(container.NotifierOBPName);
        RectTransform rectTr = notifer.GetComponent<RectTransform>();
        rectTr.anchoredPosition = Vector3.right * 0f;
        rectTr.localScale = new Vector3(1f, 1f, 1f);
        notifer.GetComponent<GlobalNotifer>().Setting(notifierString);
        SoundManager.Instance.PlayUISound(sound == SoundList.None ? SoundList.UIClick_NeutralButton18_KRST_NONE : sound);
    }

    public void ExcuteGlobalSimpleNotifer(string notifierString, SoundList sound = SoundList.None)
    {
        GlobalNotifierContainer container = FindNotifierContainer(GlobalNotifierType.SIMPLE);
        SoundManager.Instance.PlayEffect(container.NotifierSound);

        GameObject notifer = ObjectPooling.Instance.GetOBP(container.NotifierOBPName);
        RectTransform rectTr = notifer.GetComponent<RectTransform>();
        rectTr.anchoredPosition = Vector3.right * 0f;
        rectTr.localScale = new Vector3(1f, 1f, 1f);
        notifer.GetComponent<GlobalNotifer>().Setting(notifierString);
        SoundManager.Instance.PlayUISound(sound == SoundList.None ? SoundList.UIClick_NeutralButton13_KRST_NONE : sound);

    }

    public void ExcuteGlobalBattleNotifer(string notifierString, float returnTime, float doExitTime = -1f, SoundList sound = SoundList.None)
    {
        GlobalNotifierContainer container = FindNotifierContainer(GlobalNotifierType.BATTLE);
        SoundManager.Instance.PlayEffect(container.NotifierSound);

        GameObject notifer = ObjectPooling.Instance.GetOBP(container.NotifierOBPName);
        RectTransform rectTr = notifer.GetComponent<RectTransform>();
        rectTr.anchoredPosition = Vector3.right * 0f;
        rectTr.localScale = new Vector3(1f, 1f, 1f);
        notifer.GetComponent<GlobalNotifer>().Setting(notifierString);
        notifer.GetComponent<ReturnObjectToObjectPooling>().TimeSetting(returnTime, doExitTime);
        SoundManager.Instance.PlayUISound(sound == SoundList.None ? SoundList.MAGMisc_HubMenuAppears02_KRST_NONE : sound);

    }

    #endregion

    #region Item Gain
    public void ExcuteItemGainNotifier(int amount)
    {
        ItemContainer itemContainer = new ItemContainer(amount);

        itemOrderList.Enqueue(itemContainer);

        if (!isExcuteGainList)
            StartCoroutine(CreateItemGain());
    }

    public void ExcuteItemGainNotifier(Item item, int amount)
    {
        ItemContainer itemContainer = new ItemContainer(item, amount);
        itemOrderList.Enqueue(itemContainer);

        if (!isExcuteGainList)
            StartCoroutine(CreateItemGain());
    }

    public void ExcuteItemGainNotifier(Reward reward)
    {
        ItemContainer itemContainer = new ItemContainer(reward);
        itemOrderList.Enqueue(itemContainer);

        if (!isExcuteGainList)
            StartCoroutine(CreateItemGain());
    }


    private IEnumerator CreateItemGain()
    {
        isExcuteGainList = true;

        while (itemOrderList.Count > 0)
        {
            CreateitemGainNotifier(itemOrderList.Dequeue());
            yield return new WaitForSeconds(itemGainDelayTime);
        }

        if (itemOrderList.Count > 0)
            StartCoroutine(CreateItemGain());

        isExcuteGainList = false;
    }

    private void CreateitemGainNotifier(ItemContainer itemContainer)
    {
        GlobalNotifierContainer container = FindNotifierContainer(GlobalNotifierType.ITEMGAIN);
        GameObject notifier = ObjectPooling.Instance.GetOBP(container.NotifierOBPName);

        notifier.transform.SetAsLastSibling();
        SoundManager.Instance.PlayUISound(UISoundType.ITEM_COLLECT);

        RectTransform rectTr = notifier.GetComponent<RectTransform>();
        rectTr.anchoredPosition = Vector3.right * 0f;
        rectTr.localScale = new Vector3(1f, 1f, 1f);
        if (!itemContainer.IsMoney && itemContainer.Reward == null)
            notifier.GetComponent<ItemGainNotifer>().SettingNotifier(itemContainer.Item, itemContainer.Amount);
        else if (itemContainer.Reward != null)
            notifier.GetComponent<ItemGainNotifer>().SettingNotifier(itemContainer.Reward);
        else
            notifier.GetComponent<ItemGainNotifer>().SettingNotifier(itemContainer.Amount);
    }


    private GlobalNotifierContainer FindNotifierContainer(GlobalNotifierType notifierType)
    {
        foreach (GlobalNotifierContainer container in globalNotifierContainers)
            if (container.NotifierType == notifierType)
                return container;
        return null;
    }


    #endregion

    #region UI Popup

    public void ClearActiveUIs() => activeUI.Clear();
    public int GetActiveUICount() => activeUI.Count;

    public void RegisterPopupUI(UIRoot uiRoot)
    {
        if (!uiRoot.isRegisterPopup)
            return;

        if (!activeUI.Contains(uiRoot))
            activeUI.Add(uiRoot);
    }

    public void RemovePopupUI(UIRoot uIRoot)
    {
        if (activeUI.Contains(uIRoot))
            activeUI.Remove(uIRoot);

        if (activeUI.Count <= 0)
        {
            if (uIRoot.uIType == ContainerType.QUICK || uIRoot.uIType == ContainerType.ITEMUI 
                || uIRoot.uIType == ContainerType.DUNGEON_PORTAL_CHAPTER)
                return;
            GameManager.Instance.canUseCamera = true;
            CursorManager.Instance.CursorLock();
            SettingManager.Instance.IsUnInterruptibleUI = false;
        }
    }

    public void CloseLastPopupWindow()
    {
        if (activeUI.Count <= 0) return;
        activeUI[activeUI.Count - 1].CloseUIWindow();
    }


    public void ExcuteOrder(UIRoot uiRoot)
    {
        orderByEt.FindUIAndExcuteOrderBy(uiRoot);

        if (uiRoot.uIType != ContainerType.ITEMUI)
            SoundManager.Instance.PlayUISound(UISoundType.ORDER_BY_WINDOW);
    }

    /// <summary>
    /// ActiveUI에 존재할 경우  ActiveUI window 가장 앞으로 이동.
    /// </summary>
    public void OrderLast(UIRoot uIRoot)
    {
        if (!activeUI.Contains(uIRoot) || isItemInfomationOpen) return;
        int index = GetActiveUIIndex(uIRoot);
         activeUI.RemoveAt(index);
         activeUI.Add(uIRoot);

    }

    private int GetActiveUIIndex(UIRoot uiRoot)
    {
        for (int i = 0; i < activeUI.Count; i++)
            if (activeUI[i].UIID == uiRoot.UIID)
                return i;

        return -1;
    }
    #endregion

    #region Screen Touch
    public void SetScreenTouch(GameObject screen)
    {
        if (screen.GetComponent<EventTrigger>() == null)
            screen.AddComponent<EventTrigger>();

        UIHelper.AddEventTrigger(screen, EventTriggerType.PointerClick, delegate { ExcuteScreenTouch(); });
    }


    private void ExcuteScreenTouch()
    {
        if (SettingManager.Instance.IsTitle || !SettingManager.Instance.UseScreenTouch || QuestManager.Instance.isDialoging ) return;
        if (SettingManager.Instance.CanExcuteScreenTouch)
            onExcuteScreenTouch?.Invoke();

        GameManager.Instance.canUseCamera = true;
        CursorManager.Instance.CursorLock();
        GameManager.Instance.isWriting = false;
        GameManager.Instance.canUsePlayerState = true;
        SettingManager.Instance.IsUnInterruptibleUI = false;
    }

    public void AllCloseActiveUIWindow()
    {
        foreach (UIRoot ui in activeUI.ToArray())
            ui.CloseUIWindow();
        activeUI.Clear();
        SettingManager.Instance.IsUnInterruptibleUI = false;

    }
    #endregion

    #region Interact UI
    
    public void InteractUIActive(bool isActive) => interactUI.gameObject.SetActive(isActive);
    public void InteractUISettingDescript(string description) => interactUI.SettingDescription(description);
    public void InteractUIRegister(InteractObject interact) => interactUI.RegisterInteractUI(interact);
    public void InteractUIRemove(InteractObject interact) => interactUI.RemoveInteractUI(interact);
    public bool IsHaveCurrentInteract() => interactUI.currentInteract;
    public void ResetInteractUI() => interactUI.RemoveCurrentInteractUI();

    #endregion


    public void ExcuteDialog(DialogFile file, int excuteID, DialogState state)
    {
        Debug.Log("익스큐트 다이어로그!");
        onExcuteDialog?.Invoke(file, excuteID,state);
    }

    public void DropItem(Item item, int amount) => itemCountConfirmUI.SettingDrop(item, amount);
}
