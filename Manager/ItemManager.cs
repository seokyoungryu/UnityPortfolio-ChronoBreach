using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private ItemData itemData = null;

    protected override void Awake()
    {
        base.Awake();
        if(itemData == null)
        {
            itemData = ScriptableObject.CreateInstance<ItemData>();
            itemData.LoadData();
        }

        Debug.Log("ItemManager Awake!!!!");
    }


    public ItemData GetItemData() => itemData;

    public Item GenerateRandomItem()
    {
        int index = MathHelper.GetRandom(6, GetAllItemCount());
        Item item = GenerateItem(index);
        return item;
    }


    public Item GenerateRandomEquipmentItem() => GenerateRandomItem(ItemCategoryType.EQUIPMENT);
    public Item GenerateRandomConsumableItem() => GenerateRandomItem(ItemCategoryType.CONSUMABLE);
    public Item GenerateRandomMaterialsItem() => GenerateRandomItem(ItemCategoryType.MATERIAL);
    public Item GenerateRandomQuestItem() => GenerateRandomItem(ItemCategoryType.QUESTITEM);

    private Item GenerateRandomItem(ItemCategoryType type)
    {
        int index = 0;
        bool find = false;
        while (!find)
        {
            index = MathHelper.GetRandom(0, GetAllItemCount());
            if (itemData.allItemClips[index].itemCategoryType == type)
                find = true;
        }
        Item item = GenerateItem(index);
        return item;
    }

    public Item GenerateItem(Item item)
    {
        return GenerateItem(item.itemClip.id);
    }

    public Item GenerateItem(int itemID)
    {
        if (itemData.allItemClips[itemID] is WeaponItemClip)
            return CreateItem<WeaponItemClip>(itemData.allItemClips[itemID]);
        else if (itemData.allItemClips[itemID] is ArmorItemClip)
            return CreateItem<ArmorItemClip>(itemData.allItemClips[itemID]);
        else if (itemData.allItemClips[itemID] is AccessoryItemClip)
            return CreateItem<AccessoryItemClip>(itemData.allItemClips[itemID]);
        else if (itemData.allItemClips[itemID] is TitleItemClip)
            return CreateItem<TitleItemClip>(itemData.allItemClips[itemID]);
        else if (itemData.allItemClips[itemID] is PosionItemClip)
            return CreateItem<PosionItemClip>(itemData.allItemClips[itemID]);
        else if (itemData.allItemClips[itemID] is ExtraItemClip)
            return CreateItem<ExtraItemClip>(itemData.allItemClips[itemID]);
        else if (itemData.allItemClips[itemID] is EnchantItemClip)
            return CreateItem<EnchantItemClip>(itemData.allItemClips[itemID]);
        else if (itemData.allItemClips[itemID] is CraftItemClip)
            return CreateItem<CraftItemClip>(itemData.allItemClips[itemID]);
        else if (itemData.allItemClips[itemID] is QuestItemClip)
            return CreateItem<QuestItemClip>(itemData.allItemClips[itemID]);

        return null;
    }


    private Item CreateItem<T>(BaseItemClip item) where T : BaseItemClip
    {
        if (item is T)
        {
            T clip = Instantiate(item as T);
            clip.ReloadResource();
            Item returnItem = new Item(clip);
            returnItem.itemClip.SetInitRandomPotential();
            returnItem.itemClip.InitBaseStats();
            returnItem.itemClip.SetSkinnedMeshContainerName();
            returnItem.itemClip.SetItemInstanceID();
            returnItem.itemClip.SetUseableObject();
            return returnItem;
        }
        else
            return null;
    }


    #region Get Item Clip
    public BaseItemClip GetItemClip(int index) => itemData.GetItemClip(index);
    public WeaponItemClip GetWeaponItemClip(int index) => itemData.GetWeaponItem(index);
    public ArmorItemClip GetArmorItemClip(int index) => itemData.GetArmorItem(index);
    public AccessoryItemClip GetAccessoryItemClip(int index) => itemData.GetAccessoryItem(index);
    public TitleItemClip GetTitleItemClip(int index) => itemData.GetTitleItem(index);
    public PosionItemClip GetPosionItemClip(int index) => itemData.GetPosionItem(index);
    public EnchantItemClip GetEnchantItemClip(int index) => itemData.GetEnchantItem(index);
    public CraftItemClip GetCraftItemClip(int index) => itemData.GetCraftItem(index);
    public ExtraItemClip GetExtraItemClip(int index) => itemData.GetExtraItem(index);
    public QuestItemClip GetQuestItemClip(int index) => itemData.GetQuestItem(index);

    #endregion


    #region Get Item Count
    public int GetAllItemCount() => itemData.GetDataCount(itemData.allItemClips);
    public int GetEquipmentItemCount() => itemData.GetDataCount(itemData.equipmentItemClips);
    public int GetWeaponItemCount() => itemData.GetDataCount(itemData.weaponItemClips);
    public int GetArmorItemCount() => itemData.GetDataCount(itemData.armorItemClips);
    public int GetAccessoryItemCount() => itemData.GetDataCount(itemData.accessoryItemClips);
    public int GetTitleItemCount() => itemData.GetDataCount(itemData.titleItemClips);
    public int GetConsumableItemCount() => itemData.GetDataCount(itemData.consumableItemClips);
    public int GetPosionItemCount() => itemData.GetDataCount(itemData.posionItemClips);
    public int GetEnchantItemCount() => itemData.GetDataCount(itemData.enchantItemClips);
    public int GetMaterialsItemCount() => itemData.GetDataCount(itemData.materialItemClips);

    #endregion




}
