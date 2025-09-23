using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEquipment : MonoBehaviour
{
    public EquipmentUI equipmentContainer = null;

    [Header("기본 장비")]
    [Header("[ 0 : Head, 1 : Upper, 2 : Lower, 3 : Hand, 4 : Leg ]")]
    [SerializeField] private ItemList[] originArmorList;
    [SerializeField] private ItemList originWeaponList;
    [SerializeField] private ItemList originTitleList;
    [SerializeField] private Item[] originArmorItems = null;
    [SerializeField] private Item originWeapon = null;
    [SerializeField] private Item originTitle = null;


    [Header("장착된 장비"), Space(15)]
    [Header("Weapon")]
    [SerializeField] private WeaponItemClip equipWeapon = null;

    [Header("Title")]
    [SerializeField] private string titleName = string.Empty;
    [SerializeField] private TitleItemClip equipTitleClip = null;

    [Header("[ 0 : Head, 1 : Upper, 2 : Lower, 3 : Hand, 4 : Leg ]")]
    [SerializeField] private ArmorItemClip[] equipArmorItems = null;

    [Header("[ 0 ~ 1 : Earing, 2 : Soul, 3 : Belt, 4 ~ 5 : Bracelet, 6 ~ 9 : Ring , 10 : Cloak")]
    [SerializeField] private AccessoryItemClip[] equipAccessoryItems = null;

    [Header("[ 0 : Hair, 1 : Head")]
    [SerializeField] private AccessoryItemClip[] equipassistItem = null;


    [Header("Reference")]
    private PlayerStatus playerStatus;
    private SkinnedMeshController skinnedMeshController;
    [SerializeField] private EquipWeaponModel weaponPosition;

    private void Awake()
    {
        if (equipArmorItems.Length <= 0)
            equipArmorItems = new ArmorItemClip[5];
        if (equipAccessoryItems.Length <= 0)
            equipAccessoryItems = new AccessoryItemClip[11];
        if (equipassistItem.Length <= 0)
            equipassistItem = new AccessoryItemClip[2];

        if (equipmentContainer == null) equipmentContainer = FindObjectOfType<EquipmentUI>();
        playerStatus = GetComponent<PlayerStatus>();
        skinnedMeshController = GetComponent<SkinnedMeshController>();
        weaponPosition = GetComponentInChildren<EquipWeaponModel>();
    }


    public void LoadExcuteEquipmentItem()
    {
        if (equipmentContainer == null)
            equipmentContainer = CommonUIManager.Instance.equipmentUI;

        // equipmentContainer.LoadEquipmentUI();
        AllRemoveEquipItem();
        LoadEquipmentItem();
        SetOriginItem();
        EquipOriginItem();
        playerStatus.AddCurrentHealth(playerStatus.TotalHealth);
    }


    /// <summary>
    /// 장비의 스탯을 적용, Mesh 생성
    /// </summary>
    public void EquipmentItem(Item item, bool isOriginItem = false)
    {
        if (item.id < 0 || item.itemClip == null) return;

        EquipmentTpye type = item.itemClip.equipmentTpye;
        switch (type)
        {
            case EquipmentTpye.WEAPON:
                EquipWeaponClip(item);
                break;
            case EquipmentTpye.ARMOR:
                EquipArmorClip(item, isOriginItem);
                break;
            case EquipmentTpye.ACCESSORIES:
                EquipAccessoryClip(item);
                break;
            case EquipmentTpye.TITLE:
                EquipTitleClip(item);
                break;
        }
        SoundManager.Instance.PlayUISound(UISoundType.EQUIPMENT);
        playerStatus.UpdateStats();
    }

    #region Set Equipment Clip
    private void EquipWeaponClip(Item item)
    {
        WeaponItemClip weapon = item.itemClip as WeaponItemClip;
        if (equipWeapon != null)
            UnEquipItem(item, false);

        Debug.Log("Setting Weapon : " + weapon.uiItemName);
        weapon.SetWeaponItemStatus(playerStatus, true);
        equipWeapon = weapon;
        weaponPosition.EquipModel(equipWeapon.itemPrefab);
        playerStatus.UpdateStats();
    }

    private void EquipArmorClip(Item item, bool isOriginItem)
    {
        ArmorItemClip armorClip = item.itemClip as ArmorItemClip;
        ArmorType armorType = armorClip.armorType;
        switch (armorType)
        {
            case ArmorType.HEAD:
                SetArmorClip(ArmorType.HEAD, item, armorClip);
                skinnedMeshController.CreateSkinnedMesh(item);
                break;
            case ArmorType.UPPER:
                SetArmorClip(ArmorType.UPPER, item, armorClip);
                skinnedMeshController.CreateSkinnedMesh(item);
                break;
            case ArmorType.LOWER:
                SetArmorClip(ArmorType.LOWER, item, armorClip);
                skinnedMeshController.CreateSkinnedMesh(item);
                break;
            case ArmorType.HAND:
                SetArmorClip(ArmorType.HAND, item, armorClip);
                skinnedMeshController.CreateSkinnedMesh(item);
                break;
            case ArmorType.LEG:
                SetArmorClip(ArmorType.LEG, item, armorClip);
                skinnedMeshController.CreateSkinnedMesh(item);
                break;
        }
    }
    private void SetArmorClip(ArmorType type, Item item, ArmorItemClip clip)
    {
        if (equipArmorItems[(int)type] != null)
            UnEquipItem(item, false);
        Debug.Log("Setting Armor : " + item.itemClip.uiItemName);
        clip.SetArmorItemStatus(playerStatus, true);
        equipArmorItems[(int)type] = clip;
    }

    private void EquipAccessoryClip(Item item)
    {
        AccessoryItemClip accessory = item.itemClip as AccessoryItemClip;
        AccessoryType armorType = accessory.accessoryType;
        switch (armorType)
        {
            case AccessoryType.EARING:
                SetAccessoryClip(0, 1, item, accessory);
                break;
            case AccessoryType.RING:
                SetAccessoryClip(6, 9, item, accessory);
                break;
            case AccessoryType.BELT:
                SetAccessoryClip(3, 3, item, accessory);
                break;
            case AccessoryType.BRACELET:
                SetAccessoryClip(4, 5, item, accessory);
                break;
            case AccessoryType.SOUL:
                SetAccessoryClip(2, 2, item, accessory);
                break;
            case AccessoryType.CLOAK:
                SetAccessoryClip(10, 10, item, accessory);
                skinnedMeshController.CreateSkinnedMesh(item);
                break;
        }
    }
    private void SetAccessoryClip(int range1, int range2, Item item, AccessoryItemClip clip)
    {
        bool isEquip = false;
        for (int i = range1; i <= range2; i++)
        {
            if (equipAccessoryItems[i] == null)
            {
                clip.SetAccessoryItemStatus(playerStatus, true);
                equipAccessoryItems[i] = clip;
                Debug.Log("Setting Accessory : " + item.itemClip.uiItemName);
                isEquip = true;
                return;
            }
        }

        if (isEquip == false)
        {

        }
    }

    private void EquipTitleClip(Item item)
    {
        TitleItemClip title = item.itemClip as TitleItemClip;
        if (equipTitleClip != null)
            UnEquipItem(item, false);

        title.SetTitleItemStatus(playerStatus, true);
        equipTitleClip = title;
        titleName = equipTitleClip.uiItemName;
        GameManager.Instance.title = titleName;
    }

    #endregion


    /// <summary>
    /// 아이템 제한 레벨과 플레이어 레벨을 비교
    /// </summary>
    public bool CanEquipment(Item item)
    {
        if (item == null) return false;

        bool canEquipment = false;
        EquipmentTpye type = item.itemClip.equipmentTpye;

        switch (type)
        {
            case EquipmentTpye.WEAPON:
                WeaponItemClip weaponClip = item.itemClip as WeaponItemClip;
                if (weaponClip.requiredLevel <= playerStatus.Level) canEquipment = true;
                break;
            case EquipmentTpye.ARMOR:
                ArmorItemClip armorClip = item.itemClip as ArmorItemClip;
                if (armorClip.requiredLevel <= playerStatus.Level) canEquipment = true;
                break;
            case EquipmentTpye.ACCESSORIES:
                AccessoryItemClip accessoryClip = item.itemClip as AccessoryItemClip;
                if (accessoryClip.requiredLevel <= playerStatus.Level) canEquipment = true;
                break;
            case EquipmentTpye.TITLE:
                TitleItemClip titleClip = item.itemClip as TitleItemClip;
                canEquipment = true;
                break;
        }
        return canEquipment;
    }

    /// <summary>
    /// 장비 스탯 해제, 장비 메쉬 제거
    /// </summary>
    public void UnEquipItem(Item item ,bool isRemoveAll , SlotAllowType slotType = SlotAllowType.NONE)
    {
        SlotAllowType type = item == null ? slotType : item.itemType;
        switch (type)
        {
            case SlotAllowType.WEAPON:
                if (equipWeapon == null) break;
                equipWeapon.SetWeaponItemStatus(playerStatus, false);
                weaponPosition.DeleteModel();
                equipWeapon = null;
                break;
            case SlotAllowType.HEAD:
                if (equipArmorItems[0] == null) break;
                equipArmorItems[0].SetArmorItemStatus(playerStatus, false);
                equipArmorItems[0] = null;
                skinnedMeshController.DeletSkinnedMesh(type);
                break;
            case SlotAllowType.UPPER:
                if (equipArmorItems[1] == null) break;
                equipArmorItems[1].SetArmorItemStatus(playerStatus, false);
                equipArmorItems[1] = null;
                skinnedMeshController.DeletSkinnedMesh(type);
                break;
            case SlotAllowType.LOWER:
                if (equipArmorItems[2] == null) break;
                equipArmorItems[2].SetArmorItemStatus(playerStatus, false);
                equipArmorItems[2] = null;
                skinnedMeshController.DeletSkinnedMesh(type);
                break;
            case SlotAllowType.HAND:
                if (equipArmorItems[3] == null) break;
                equipArmorItems[3].SetArmorItemStatus(playerStatus, false);
                equipArmorItems[3] = null;
                skinnedMeshController.DeletSkinnedMesh(type);
                break;
            case SlotAllowType.LEG:
                if (equipArmorItems[4] == null) break;
                equipArmorItems[4].SetArmorItemStatus(playerStatus, false);
                equipArmorItems[4] = null;
                skinnedMeshController.DeletSkinnedMesh(type);
                break;
            case SlotAllowType.TITLE:
                if (equipTitleClip == null) break;
                equipTitleClip.SetTitleItemStatus(playerStatus, false);
                equipTitleClip = null;
                titleName = "";
                break;
            case SlotAllowType.EARING:
            case SlotAllowType.RING:
            case SlotAllowType.BELT:
            case SlotAllowType.BRACELET:
            case SlotAllowType.SOUL:
                if (isRemoveAll)
                {
                    for (int i = 0; i < equipAccessoryItems.Length; i++)
                    {
                        if (equipAccessoryItems[i] == null) continue;
                            equipAccessoryItems[i].SetAccessoryItemStatus(playerStatus, false);
                            equipAccessoryItems[i] = null;
                    }
                }
                else
                {
                    for (int i = 0; i < equipAccessoryItems.Length; i++)
                    {
                        if (equipAccessoryItems[i] == null) continue;
                        if (equipAccessoryItems[i].instanceID == item.itemClip.instanceID)
                        {
                            equipAccessoryItems[i].SetAccessoryItemStatus(playerStatus, false);
                            equipAccessoryItems[i] = null;
                        }
                    }
                }
                break;
            case SlotAllowType.CLOAK:
                if (equipAccessoryItems[10] == null) break;
                equipAccessoryItems[10].SetAccessoryItemStatus(playerStatus, false);
                equipAccessoryItems[10] = null;
                skinnedMeshController.DeletSkinnedMesh(type);
                break;
        }

        EquipOriginItem();
        playerStatus.UpdateStats();
    }

   
    public void RemoveEquipItem(Item item)
    {
        SlotAllowType type = item.itemType;
        Debug.Log($"RemoveEquipItem : {item.itemClip.uiItemName}  , {type.ToString()}");

        switch (type)
        {
            case SlotAllowType.WEAPON:
                if (equipWeapon.instanceID == item.itemClip.instanceID)
                    UnEquipItem(item,false);
                break;
            case SlotAllowType.HEAD:
                if (equipArmorItems[0].instanceID == item.itemClip.instanceID)
                    UnEquipItem(item, false);
                break;
            case SlotAllowType.UPPER:
                if (equipArmorItems[1].instanceID == item.itemClip.instanceID)
                    UnEquipItem(item, false);
                break;
            case SlotAllowType.LOWER:
                if (equipArmorItems[2].instanceID == item.itemClip.instanceID)
                    UnEquipItem(item, false);
                break;
            case SlotAllowType.HAND:
                if (equipArmorItems[3].instanceID == item.itemClip.instanceID)
                    UnEquipItem(item, false);
                break;
            case SlotAllowType.LEG:
                if (equipArmorItems[4].instanceID == item.itemClip.instanceID)
                    UnEquipItem(item, false);
                break;
            case SlotAllowType.TITLE:
                if (equipTitleClip.instanceID == item.itemClip.instanceID)
                    UnEquipItem(item, false);
                break;
            case SlotAllowType.EARING:
            case SlotAllowType.RING:
            case SlotAllowType.BELT:
            case SlotAllowType.BRACELET:
            case SlotAllowType.SOUL:
                for (int i = 0; i < equipAccessoryItems.Length; i++)
                {
                    if (equipAccessoryItems[i] == null) continue;
                    if (equipAccessoryItems[i].instanceID == item.itemClip.instanceID)
                        UnEquipItem(item, false);
                }
                break;
            case SlotAllowType.CLOAK:
                if (equipAccessoryItems[10].instanceID == item.itemClip.instanceID)
                    UnEquipItem(item, false);
                break;
        }
    }


    public void AllRemoveEquipItem()
    {
        UnEquipItem(null, true, SlotAllowType.WEAPON);
        UnEquipItem(null, true, SlotAllowType.CLOAK);
        UnEquipItem(null, true, SlotAllowType.TITLE);
        UnEquipItem(null, true, SlotAllowType.HEAD);
        UnEquipItem(null, true, SlotAllowType.HAND);
        UnEquipItem(null, true, SlotAllowType.UPPER);
        UnEquipItem(null, true, SlotAllowType.LOWER);
        UnEquipItem(null, true, SlotAllowType.LEG);
        UnEquipItem(null, true, SlotAllowType.RING);
    }


    /// <summary>
    /// 저장된 장비템 불러오는 함수
    /// </summary>
    private void LoadEquipmentItem()
    {
        InventorySlot[] equipSlots = equipmentContainer.GetEquipmentSlots();

        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].item.HaveItem())
                EquipmentItem(equipSlots[i].item);
        }
    }


    /// <summary>
    /// 기본 장비 생성
    /// </summary>
    private void SetOriginItem()
    {
        if (originArmorItems == null)
            originArmorItems = new Item[5];

        if (originArmorItems[0].itemClip == null && originArmorList[0] != ItemList.None)
            originArmorItems[0] = ItemManager.Instance.GenerateItem((int)originArmorList[0]);
        if (originArmorItems[1].itemClip == null && originArmorList[1] != ItemList.None)
            originArmorItems[1] = ItemManager.Instance.GenerateItem((int)originArmorList[1]);
        if (originArmorItems[2].itemClip == null && originArmorList[2] != ItemList.None)
            originArmorItems[2] = ItemManager.Instance.GenerateItem((int)originArmorList[2]);
        if (originArmorItems[3].itemClip == null && originArmorList[3] != ItemList.None)
            originArmorItems[3] = ItemManager.Instance.GenerateItem((int)originArmorList[3]);
        if (originArmorItems[4].itemClip == null && originArmorList[4] != ItemList.None)
            originArmorItems[4] = ItemManager.Instance.GenerateItem((int)originArmorList[4]);

        if (originWeapon.itemClip == null && originWeaponList != ItemList.None)
            originWeapon = ItemManager.Instance.GenerateItem((int)originWeaponList);

        if (originTitle.itemClip == null && originTitleList != ItemList.None)
            originTitle = ItemManager.Instance.GenerateItem((int)originTitleList);

    }


    /// <summary>
    /// 장착된 장비가 없으면 기본 장비 장착
    /// </summary>
    private void EquipOriginItem()
    {
        if (equipArmorItems[0] == null && originArmorItems[0].itemClip != null)
            skinnedMeshController.CreateSkinnedMesh(originArmorItems[0]);
        if (equipArmorItems[1] == null && originArmorItems[1].itemClip != null)
            skinnedMeshController.CreateSkinnedMesh(originArmorItems[1]);
        if (equipArmorItems[2] == null && originArmorItems[2].itemClip != null)
            skinnedMeshController.CreateSkinnedMesh(originArmorItems[2]);
        if (equipArmorItems[3] == null && originArmorItems[3].itemClip != null)
            skinnedMeshController.CreateSkinnedMesh(originArmorItems[3]);
        if (equipArmorItems[4] == null && originArmorItems[4].itemClip != null)
            skinnedMeshController.CreateSkinnedMesh(originArmorItems[4]);

        if (equipWeapon == null && originWeapon.itemClip != null)
            weaponPosition.EquipModel(originWeapon.itemClip.itemPrefab);

        if (equipTitleClip == null && originTitle.itemClip != null)
            EquipTitleClip(originTitle);


        if (equipAccessoryItems[10] == null && skinnedMeshController.equipmentSkinnedMesh[5] != null)
            skinnedMeshController.DeletSkinnedMesh(SlotAllowType.CLOAK);
    }

}
