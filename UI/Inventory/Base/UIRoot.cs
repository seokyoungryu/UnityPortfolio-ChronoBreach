using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ContainerType
{
    EQUIPMNET,
    INVENTORY,
    QUICK,
    SKILL,
    NPC_STORE,
    DIALOG,
    KEYBOARD,
    REQUIPEDSKILLSETTING,
    STATSINFORMATION,
    DUNGEON_PORTAL,
    DUNGEON_PORTALDETAIL,
    NPC_STORE_CONFIRM,
    DUNGEON_PORTAL_CHAPTER,
    ITEMUI,
    RECONFIRM_DROP,
    QUEST_SELECT_CONATINER,
    
    SCOREUI,
    PRACTICEMODE_ENTRY,
    CONSOLUE_COMMAND_WINDOW,

}


public class UIRoot : MonoBehaviour
{
    [SerializeField, HideInInspector] private int uiID = 0;
    public ContainerType uIType = ContainerType.INVENTORY;
    public bool isRegisterPopup = true;

    protected float mouseStayInIconTimer = 0f;
    protected bool isItemWindowOpen = false;

    public int UIID { get { return uiID; } set { uiID = value; } }
    public bool IsItemWindowOpen => isItemWindowOpen;
  // public SoundList OpenSound { get { return openSound; } set { openSound = value; } }
  // public SoundList CloseSound { get { return closeSound; } set { closeSound = value; } }


    #region Events
    public delegate void OnExcute();

    public event OnExcute onExcuteOpenWindowUI;
    public event OnExcute onExcuteCloseWindowUI;
    #endregion


    protected virtual void Awake()
    {
        UIHelper.AddEventTrigger(this.gameObject, EventTriggerType.PointerEnter, delegate { OnUIRootEnter(this.gameObject); });
        UIHelper.AddEventTrigger(this.gameObject, EventTriggerType.PointerExit, delegate { OnUIRootExit(this.gameObject); });
        UIHelper.AddEventTrigger(this.gameObject, EventTriggerType.PointerDown, delegate { OnPointerClickOrderByWindow(this); });

    }

    protected virtual void Start()
    {
    }

    public virtual void StartResetActive() { }


    public void OnPointerClickOrderByWindow(UIRoot uiRoot)
    {
        if (isItemWindowOpen || CommonUIManager.Instance.isItemInfomationOpen) return;
        if (uIType == ContainerType.QUICK) return;

        CommonUIManager.Instance.ExcuteOrder(uiRoot);
        GameManager.Instance.isWriting = false;
    }


    public virtual void OnUIRootEnter(GameObject go)
    {
        MouseUIData.enterUIRoot = go.GetComponent<UIRoot>();
        Debug.Log("Enter : " + go.name);
    }

    public virtual void OnUIRootExit(GameObject go)
    {
        MouseUIData.enterUIRoot = null;
    }

    #region Open UI Window 

    public virtual void OpenUIWindow()
    {
        if (uIType != ContainerType.DUNGEON_PORTAL && uIType != ContainerType.DUNGEON_PORTAL_CHAPTER
            &&uIType != ContainerType.DUNGEON_PORTAL_CHAPTER && uIType != ContainerType.SCOREUI )
            SoundManager.Instance.PlayUISound(UISoundType.OPEN_WINDOW);


        GameManager.Instance.canUseCamera = false;
        CursorManager.Instance.CursorVisible();
        Debug.Log(gameObject.name + "OpenUIWindow");
        OpenUIWindowData();
    }

    protected void OpenUIWindowData()
    {
        CommonUIManager.Instance.RegisterPopupUI(this);
        this.gameObject.SetActive(true);
        CommonUIManager.Instance.ExcuteOrder(this);
        onExcuteOpenWindowUI?.Invoke();
    }
    protected void OtherActive(bool active, GameObject go)
    {
        GameManager.Instance.canUseCamera = false;
        CursorManager.Instance.CursorVisible();
        Debug.Log(go.name + "OpenUIWindow");
        go.SetActive(active);
    }


    #endregion

    #region Close UI Window

    public virtual void CloseUIWindow()
    {
        if (uIType == ContainerType.QUICK || uIType == ContainerType.QUEST_SELECT_CONATINER) return;
        if (!gameObject.activeInHierarchy)
            return;

        CloseUIWindowData();
        Debug.Log(gameObject.name + "CloseUIWindow");

        if (uIType != ContainerType.DUNGEON_PORTAL_CHAPTER && uIType != ContainerType.SCOREUI)
        {
            SoundManager.Instance.PlayUISound(UISoundType.CLOSE_WINDOW);
            SettingManager.Instance.UseScreenTouch = true;
            SettingManager.Instance.CanExcuteESC = true;
        }
    }

    protected void CloseUIWindowData()
    {
        onExcuteCloseWindowUI?.Invoke();
        CommonUIManager.Instance.RemovePopupUI(this);
        this.gameObject.SetActive(false);
        GameManager.Instance.isWriting = false;

        if (isItemWindowOpen)
            OnItemInformationPointerExit(null);
    }

    #endregion

    public virtual void OnItemInformationPointerEnter(Item item)
    {
        if (item == null || item.itemClip == null || MouseUIData.tempDraggingImage != null || isItemWindowOpen) return;
        StartCoroutine(ItemDetailProcess(item));
    }

    public virtual void OnItemInformationPointerExit(Item item)
    {
        CommonUIManager.Instance.itemInfomationUI.CloseUIWindow();

        if (!isItemWindowOpen) return;

        StopAllCoroutines();
        isItemWindowOpen = false;
        CommonUIManager.Instance.itemInfomationUI.ResetItems();
        CommonUIManager.Instance.itemInfomationUI.CloseUIWindow();
        mouseStayInIconTimer = 0f;
        CommonUIManager.Instance.isItemInfomationOpen = false;

    }



    private IEnumerator ItemDetailProcess(Item item)
    {
        Debug.Log(item.itemClip.uiItemName + "ItemDetail Open");

        isItemWindowOpen = true;
        float informationOpentime = CommonUIManager.Instance.itemInfomationUI.ItemDetailTime;
        while (mouseStayInIconTimer <= informationOpentime)
        {
            mouseStayInIconTimer += Time.deltaTime;
            yield return null;
        }

        CommonUIManager.Instance.itemInfomationUI.OpenUIWindow();
        CommonUIManager.Instance.itemInfomationUI.SettingItem(item, Input.mousePosition, uIType);
        mouseStayInIconTimer = 0f;
        CommonUIManager.Instance.isItemInfomationOpen = true;
        SoundManager.Instance.PlayUISound(UISoundType.ITEMDETAIL_WINDOW);

        //디테일 UI 띄우기. mouseSyat 변수를 기준으로 detailTimer보다 높으면 상세창 띄우고 낮으면 끄기.
    }



}
