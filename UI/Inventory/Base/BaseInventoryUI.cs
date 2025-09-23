using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(EventTrigger))]
public abstract class BaseInventoryUI : UIRoot
{
    public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();
    public Vector2 dragImageSize = Vector2.zero;
    protected bool isDraging = false;


    protected override void Awake()
    {
        base.Awake();
        CreateSlotUIs();
        Debug.Log(gameObject.name +"  Awake!");

        // UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerEnter, delegate { OnUIRootEnter(gameObject); });
        // UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerExit, delegate { OnUIRootExit(gameObject); });
    }

    protected override void Start()
    {
        base.Start();
    }


    protected abstract void CreateSlotUIs();

    protected virtual void SlotFunction(GameObject slot)
    {

    }

    public void UpdateInventorySlots(InventoryObject inventory)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            inventory.slots[i].UpdateSlot(inventory.slots[i].item, inventory.slots[i].amount);
        }
    }

    protected virtual void OnPostUpdate(InventorySlot slot)
    {
        if (slot.item.id < 0 )
        {
            slot.slotUI.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            slot.slotUI.transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
            return;
        }

        slot.item.UpdateObjectExist();
        slot.objectName = slot.item.objectName;
        slot.slotUI.transform.GetChild(1).GetComponent<Image>().sprite = slot.item.GetItemImage();
        slot.slotUI.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        slot.slotUI.transform.GetComponentInChildren<TextMeshProUGUI>().text = (slot.item.itemType == SlotAllowType.SKILL || slot.item.itemClip == null || !slot.item.itemClip.isOverlap)
                                                                         ? string.Empty : slot.amount.ToString("n0");
    }

    //밑에 나중에 큇슬롯, 스킬창 까지 구현했을때 중복되면 부모에 선언으로 수정하기.

    public override void OnUIRootEnter(GameObject go)
    {
        if (go.GetComponent<InventoryUI>() == null)
        {
            MouseUIData.enterUIRoot = go.GetComponent<BaseInventoryUI>();
            Debug.Log("Enter BaseInven In! : " + go.name);
        }
    }

   public override void OnUIRootExit(GameObject go)
   {
       if (go.GetComponent<InventoryUI>() == null)
           MouseUIData.enterUIRoot = null;
   }

    protected virtual void OnPointEnter(GameObject go)
    {
        MouseUIData.enterSlot = go;
    }

    protected virtual void OnPointExit(GameObject go)
    {
        MouseUIData.enterSlot = null;
    }

    protected virtual void OnStartDrag(GameObject go)
    {
        if (MouseUIData.enterUIRoot && (MouseUIData.enterUIRoot.uIType == ContainerType.INVENTORY || MouseUIData.enterUIRoot.uIType == ContainerType.EQUIPMNET))
            OnPointerClickOrderByWindow(MouseUIData.enterUIRoot);

        if (go.transform.GetChild(1).GetComponent<Image>()?.sprite == null) return;

        SoundManager.Instance.PlayUISound(UISoundType.START_DRAG);

        GameManager.Instance.canUseCamera = false;
        SettingManager.Instance.CanExcuteScreenTouch = false;

        isDraging = true;
        MouseUIData.tempDraggingImage = CreateDragImage(go);
        MouseUIData.dragSlot = go;
        Debug.Log("OnStartDrag : " + go.name);

    }

    protected virtual void OnDraging(GameObject go)
    {
        if (MouseUIData.tempDraggingImage == null) return;

        isDraging = true;
        MouseUIData.tempDraggingImage.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    protected virtual void OnEndDrag(GameObject go, InventoryObject inventory)
    {
        isDraging = false;
        Debug.Log("OnEndDrag : " + go.name);
        SettingManager.Instance.CanExcuteScreenTouch = true;
        SoundManager.Instance.PlayUISound(UISoundType.END_DRAG);



    }


    protected Vector3 CalculateRectPosition(int index, Vector2 startPosition, Vector2 size, Vector2 space, int colum)
    {
        float x = startPosition.x + ((size.x + space.x) * (index % colum));
        float y = startPosition.y + (-(size.y + space.y) * (index / colum));

        return new Vector3(x, y, 0f);
    }


    protected GameObject CreateDragImage(GameObject go)
    {
        if (slotUIs[go].item.id < 0)
            return null;

        GameObject dragObject = new GameObject();
        RectTransform rect = dragObject.AddComponent<RectTransform>();
        rect.sizeDelta = dragImageSize;
        dragObject.transform.SetParent(MainCanvas.Instance.RootCanvas);

        Image dragImg = dragObject.AddComponent<Image>();
        dragImg.sprite = slotUIs[go].item.GetItemImage();
        dragImg.raycastTarget = false;

        dragObject.name = "Drag Image - " + go.name;

        return dragObject;
    }


    public override void OnItemInformationPointerEnter(Item item)
    {
        if (isDraging || isItemWindowOpen) return;
        base.OnItemInformationPointerEnter(item);
    }

}
