using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public string objectName = string.Empty;
    public SlotAllowType itemType = SlotAllowType.NONE;
    public int id = -1;
    public BaseItemClip itemClip = null;        // 이거랑 밑에 NON시리얼라이즈?  이유 - 둘다 drag로 붙임 ( 아이템은 instance된 (개별적 아이템) 이 들어오고
    public BaseSkillClip skillClip = null;                                         // 스킬은 플레이어의 스킬데이타 (현 쿨타임등) 이므로.) 즉 외부에서 넣는거 x

    public Item()
    {
        itemType = SlotAllowType.NONE;
        objectName = string.Empty;
        id = -1;
        itemClip = null;
        skillClip = null;
    }
    public Item(Item item)
    {
        itemType = item.itemType;
        itemClip = item.itemClip;
        objectName = itemClip.itemName;

        UpdateObjectExist();
        SettingSlotType();
    }
    public Item(BaseItemClip clip)
    {
        objectName = clip.uiItemName;
        itemClip = clip;
        id = clip.id;
        skillClip = null;

        UpdateObjectExist();
        SettingSlotType();
    }
    public Item(BaseSkillClip clip)
    {
        objectName = clip.displayName;
        itemClip = null;
        id = clip.ID;
        skillClip = clip;

        UpdateObjectExist();
        SettingSlotType();
    }

    public void UpdateObjectExist()
    {
        if (itemClip != null)
        {
            skillClip = null;
            objectName = itemClip.uiItemName;
            id = itemClip.id;
        }
       else if (skillClip != null)
        {
            itemClip = null;
            objectName = skillClip.displayName;
            id = skillClip.ID;
        }
        else if (itemClip == null && skillClip == null)
        {
            itemType = SlotAllowType.NONE;
            objectName = string.Empty;
            id = -1;
        }
    }


    public Sprite GetItemImage()
    {
        if (itemClip != null)
            return itemClip.itemTexture;
        else if (skillClip != null)
            return skillClip.icon != null ? skillClip.icon : null;
        else
            return null;
    }



    public bool HaveItem()
    {
        if (id < 0 || (itemClip == null && skillClip == null))
            return false;

        return true;
    }

    public void UseItem(PlayerStateController controller)
    {
        Debug.Log("Use Item !");
        itemClip.UseItem(controller);
    }

    public void UseSkill(PlayerStateController controller)
    {
        if (itemType != SlotAllowType.SKILL) return;

        Debug.Log("여기옴11 : " + skillClip.displayName);

        if (controller.currentStateHash == controller.moveStateHash && !controller.skillController.GetThisSkillIsCoolTime(skillClip.ID))
            controller.ChangeState(controller.skillStateHash, skillClip.ID);
    }


    private void SettingSlotType()
    {
        if (id < 0) itemType = SlotAllowType.NONE;

        if (skillClip != null) itemType = SlotAllowType.SKILL;
        else if (itemClip.itemCategoryType == ItemCategoryType.EQUIPMENT)
        {
            if (itemClip.equipmentTpye == EquipmentTpye.WEAPON) itemType = SlotAllowType.WEAPON;
            else if (itemClip.equipmentTpye == EquipmentTpye.TITLE) itemType = SlotAllowType.TITLE;
            else if (itemClip.equipmentTpye == EquipmentTpye.ARMOR)
            {
                ArmorItemClip armor = itemClip as ArmorItemClip;
                if (armor.armorType == ArmorType.HEAD) itemType = SlotAllowType.HEAD;
                else if (armor.armorType == ArmorType.UPPER) itemType = SlotAllowType.UPPER;
                else if (armor.armorType == ArmorType.LOWER) itemType = SlotAllowType.LOWER;
                else if (armor.armorType == ArmorType.HAND) itemType = SlotAllowType.HAND;
                else if (armor.armorType == ArmorType.LEG) itemType = SlotAllowType.LEG;
            }
            else if (itemClip.equipmentTpye == EquipmentTpye.ACCESSORIES)
            {
                AccessoryItemClip accessory = itemClip as AccessoryItemClip;
                if (accessory.accessoryType == AccessoryType.RING) itemType = SlotAllowType.RING;
                else if (accessory.accessoryType == AccessoryType.EARING) itemType = SlotAllowType.EARING;
                else if (accessory.accessoryType == AccessoryType.BELT) itemType = SlotAllowType.BELT;
                else if (accessory.accessoryType == AccessoryType.BRACELET) itemType = SlotAllowType.BRACELET;
                else if (accessory.accessoryType == AccessoryType.SOUL) itemType = SlotAllowType.SOUL;
                else if (accessory.accessoryType == AccessoryType.CLOAK) itemType = SlotAllowType.CLOAK;

            }
        }
        else if (itemClip.itemCategoryType == ItemCategoryType.CONSUMABLE)
        {
            if (itemClip.consumableType == ConsumableType.ENCHANT) itemType = SlotAllowType.ENCHANT;
            else if (itemClip.consumableType == ConsumableType.POSION) itemType = SlotAllowType.POSION;
        }
        else if (itemClip.itemCategoryType == ItemCategoryType.MATERIAL)
        {
            if (itemClip.materialType == MaterialType.CRAFT) itemType = SlotAllowType.CRAFT;
            else if (itemClip.materialType == MaterialType.EXTRA) itemType = SlotAllowType.EXTRA;
        }
        else if (itemClip.itemCategoryType == ItemCategoryType.QUESTITEM)
        {
            if (itemClip.questIType == QuestIType.QUEST) itemType = SlotAllowType.QUESTITEM;
        }
    }
}
