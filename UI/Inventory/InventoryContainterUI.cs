using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class InventoryContainterUI : UIRoot
{
    public InventoryContainerObject inventoryContainerObject = null;
    [Header("[ 0: Equipment, 1: Consumable, 2: Material, 3: QuestItem")]
    public InventoryUI[] containers;
    public GameObject[] categoryUIs;
    public QuestCategory haveItemCategory = null;
    [SerializeField] private TMP_Text ownMoney_Text = null;

    #region Events
    public delegate void OnUpdateQuickSlots(InventoryContainterUI inventoryContainterUI, Item item);

    private event OnUpdateQuickSlots onUpdateQuickSlots;
    public event OnUpdateQuickSlots OnUpdateQuickSlot
    {
        add
        {
            if (onUpdateQuickSlots == null || !onUpdateQuickSlots.GetInvocationList().Contains(value))
                onUpdateQuickSlots += value;
        }
        remove
        {
            onUpdateQuickSlots -= value;
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("Application : " + Application.persistentDataPath);
        GameManager.Instance.onOwnMoneyUpdate += UpdateOwnMoneyText;
    }

    protected override void Start()
    {
        for (int i = 0; i < categoryUIs.Length; i++)
        {
            int index = i;
            UIHelper.AddEventTrigger(categoryUIs[i], EventTriggerType.PointerClick, delegate { UIPointerClick(categoryUIs[index]); });
        }
        UpdateOwnMoneyText();

    // for (int i = 0; i < containers.Length; i++)
    // {
    //     containers[i].inventory.Clear();
    //     containers[i].inventory.SaveLoadData.LoadInventoryData(containers[i].inventory);
    //     containers[i].UpdateInventorySlots(containers[i].inventory);
    // }

        //CloseAllCategory();

    }

    public void Update()
    {
      // if (Input.GetKeyDown(KeyCode.C))
      //     ClearAllInventory();
      //
      // if (Input.GetKeyDown(KeyCode.N))
      // {
      //     ItemContainer container = new ItemContainer(ItemManager.Instance.GenerateRandomItem(), 1);
      //     if (CheckCanAddItems(container))
      //     {
      //         AddItem(container);
      //         UpdateSlots();
      //     }
      //
      // }
    }

    public override void OpenUIWindow()
    {
        base.OpenUIWindow();
        UpdateOwnMoneyText();
    }

    public override void StartResetActive()
    {
        base.StartResetActive();
        CloseAllCategory();
        UpdateOwnMoneyText();
        containers[0].gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public InventoryUI GetInventoryUI(int index)
    {
        if (containers.Length <= 0 || index < 0 || index >= containers.Length)
            return null;

        return containers[index];
    }

    public void SaveInventorys()
    {
        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].inventory.SaveLoadData.SaveInventoryData(containers[i].inventory);
        }

    }
    public void ResetInventorys()
    {
        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].inventory.Clear();
            containers[i].UpdateInventorySlots(containers[i].inventory);
        }
    }

    public void LoadInventorys()
    {
        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].inventory.Clear();
            containers[i].inventory.SaveLoadData.LoadInventoryData(containers[i].inventory);
            containers[i].UpdateInventorySlots(containers[i].inventory);
        }
    }
    public void UpdateOwnMoneyText()
    {
        ownMoney_Text.text = GameManager.Instance.OwnMoney.ToString();
    }


    public void UpdateSlots()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].UpdateInventorySlots(containers[i].inventory);
    }

    public void ClearAllInventory()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].inventory.Clear();
    }

    public bool CheckCanAddItems(RewardItem[] rewardItems)
    {
        ItemContainer[] itemContainers = new ItemContainer[rewardItems.Length];
        for (int i = 0; i < rewardItems.Length; i++)
            itemContainers[i] = new ItemContainer(rewardItems[i]);

        return inventoryContainerObject.CheckCanAddItems(itemContainers);
    }
    public bool CheckCanAddItems(ItemRewardData[] rewardItems)
    {
        ItemContainer[] itemContainers = new ItemContainer[rewardItems.Length];
        for (int i = 0; i < rewardItems.Length; i++)
            itemContainers[i] = new ItemContainer(rewardItems[i]);

        return inventoryContainerObject.CheckCanAddItems(itemContainers);
    }

    public bool CheckCanAddItems(ItemContainer[] itemContainers)
    {
        return inventoryContainerObject.CheckCanAddItems(itemContainers);
    }

    public bool CheckCanAddItems(ItemContainer itemContainers)
    {
        return inventoryContainerObject.CheckCanAddItems(itemContainers);
    }

    public bool CheckCanAddItems(Item item, int amount)
    {
        return inventoryContainerObject.CheckCanAddItems(new ItemContainer(item, amount));
    }



    public void AddItem(ItemContainer itemContainers)
    {
        AddItem(itemContainers.Item, itemContainers.Amount);
    }

    public void AddItem(ItemContainer[] itemContainers)
    {
        for (int i = 0; i < itemContainers.Length; i++)
            AddItem(itemContainers[i].Item, itemContainers[i].Amount);
    }

    public void AddItem(Item item, int amount)
    {
        inventoryContainerObject.AddItem(item, amount);
        onUpdateQuickSlots?.Invoke(this, item);
        QuestManager.Instance.ReceiveReport(haveItemCategory, (int)item.itemClip.id, amount);
        UpdateSlots();
    }

    public void RemoveItemOne(Item item)
    {
        RemoveItem(item, 1);
    }

    public void RemoveItem(Item item, int amount)
    {
        inventoryContainerObject.RemoveItem(item, amount);
        onUpdateQuickSlots?.Invoke(this, item);
        UpdateSlots();
    }

    public void RemoveItemInstanceID(Item item, int amount, int instanceID)
    {
        inventoryContainerObject.RemoveItem(item, amount, instanceID);
        onUpdateQuickSlots?.Invoke(this, item);
        UpdateSlots();
    }

    public void UpdateQuickSlots(Item item) => onUpdateQuickSlots?.Invoke(this, item);

    public int GetRemainingItemCount(Item item) => inventoryContainerObject.GetRemainingItemCount(item);

    public int GetEmptySlotCount(Item item) => inventoryContainerObject.GetEmptySlotCount(item);

    public bool HaveItem(int itemID)
    {
        Item item = ItemManager.Instance.GenerateItem(itemID);
        return HaveItem(item);
    }
    public bool HaveItem(Item item)
    {
        if (GetHaveItemCount(item) <= 0) return false;
        else return true;
    }


    public int GetHaveItemCount(Item item) => inventoryContainerObject.GetHaveItemCount(item);
    public int GetHaveItemCount(int itemID)
    {
        Item item = ItemManager.Instance.GenerateItem(itemID);
        return inventoryContainerObject.GetHaveItemCount(item);
    }

    public InventorySlot FindInstanceIDItem(Item item,int instanceID) => inventoryContainerObject.FindInstanceIDItem(item, instanceID);


    public void RegisterReceiveHaveItemCount(Quest quest, Task task = null)
    {
     
        Task[] tasks = quest.currentTaskGroup.Tasks;
        for (int i = 0; i < tasks.Length; i++)
        {
            for (int x = 0; x < tasks[i].Targets.Length; x++)
            {
                if (!(tasks[i].Targets[x] is ItemTaskTarget) || !HaveItem((int)tasks[i].Targets[x].Value)) continue;
                else
                    QuestManager.Instance.ReceiveReport(haveItemCategory, tasks[i].Targets[x], GetHaveItemCount((int)tasks[i].Targets[x].Value));
            }
        }
    }

    private void CloseAllCategory()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].gameObject.SetActive(false);
    }

    private void UIPointerClick(GameObject ui)
    {
        CloseAllCategory();
        ContainerCategory categori = ui.GetComponent<ContainerCategory>();

        switch (categori.containerType)
        {
            case ItemCategoryType.EQUIPMENT:
                Debug.Log("Open : EQUIPMENT");
                containers[0].gameObject.SetActive(true);
                break;
            case ItemCategoryType.CONSUMABLE:
                Debug.Log("Open : CONSUMABLE");
                containers[1].gameObject.SetActive(true);
                break;
            case ItemCategoryType.MATERIAL:
                Debug.Log("Open : MATERIAL");
                containers[2].gameObject.SetActive(true);
                break;
            case ItemCategoryType.QUESTITEM:
                Debug.Log("Open : QUESTITEM");
                containers[3].gameObject.SetActive(true);
                break;
        }
    }

    public DynamicContainerUI GetCurrentInventoryCategory()
    {
        foreach (DynamicContainerUI inven in containers)
            if (inven.isActiveAndEnabled)
                return inven;
        return null;
    }

}


