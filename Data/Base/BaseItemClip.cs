using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseItemClip : BaseEditorClip
{
    [Header("Item ID")]
    public int instanceID = 0; //생성 ID 값
    public ItemSortType sortType = ItemSortType.WEAPON;

    [Header("Item Information")]
    public string uiItemName = string.Empty; // UI에 뜨는 item 이름
    public ItemCategoryType itemCategoryType = ItemCategoryType.EQUIPMENT;           //  EQUIPMENT, CONSUMABLE, MATERIAL, QUESTITEM
    public ItemRankType itemRankType = ItemRankType.NONE;
    public EquipmentTpye equipmentTpye = EquipmentTpye.WEAPON;  //이 종류별로 구분.
    public ConsumableType consumableType = ConsumableType.POSION;
    public MaterialType materialType = MaterialType.CRAFT;
    public QuestIType questIType = QuestIType.QUEST;
    public GameObject itemPrefab = null;
    public Sprite itemTexture = null;
    public string description = string.Empty;
    public bool isOverlap = false;
    public bool havePrefab = true;
    public bool isUnbreakable = false;

    [Header("Item Path")]
    public string itemName = string.Empty;  // string 이름 값 & enum Name
    public string itemPath = string.Empty;
    public string itemFullPath = string.Empty;
    public string texturePath = string.Empty;
    public string textureName = string.Empty;
    public string textureFullPath = string.Empty;

    [Header("Item basic Stats")]
    public int itemLevel = 0;

    public float minStr, maxStr = 0f;
    public float minDex, maxDex = 0f;
    public float minLuc, maxLuc = 0f;
    public float minInt, maxInt = 0f;

    public float strength = 0;
    public float dexterity = 0;
    public float intelligence = 0;
    public float luck = 0;
    public int buyCost = 0;
    public int sellCost = 0;
    public int repurchaseCost = 0;
    public int maxOverlapCount = 0;

    [Header("Potential Options")]
    public bool isDefaultPotentialPercent = true;
    public int potentialOptionCount = 0;
    public int[] potentialPercent = new int[5];  // 등급 확률  [0] = NONE , [4] = LEGENDARY
    public ItemPotentialRankType potentialRank = ItemPotentialRankType.NONE;
    public OwnPotential[] ownPotential;

    [Header("Useable Object")]
    public int useableObjectCount = 0;
    public UseableObject[] useableObject = null;
    public float itemCoolTime = 0f;


    public BaseItemClip() { }
    public BaseItemClip(int id, string name, int enumsortType = 0)
    {
        this.id = id;
        itemName = name;
        if ((ItemSortType)enumsortType == ItemSortType.WEAPON)
        {
            itemCategoryType = ItemCategoryType.EQUIPMENT;
            equipmentTpye = EquipmentTpye.WEAPON;
        }
        else if ((ItemSortType)enumsortType == ItemSortType.ARMOR)
        {
            itemCategoryType = ItemCategoryType.EQUIPMENT;
            equipmentTpye = EquipmentTpye.ARMOR;
        }
        else if ((ItemSortType)enumsortType == ItemSortType.ACCESSORY)
        {
            itemCategoryType = ItemCategoryType.EQUIPMENT;
            equipmentTpye = EquipmentTpye.ACCESSORIES;
        }
        else if ((ItemSortType)enumsortType == ItemSortType.TITLE)
        {
            itemCategoryType = ItemCategoryType.EQUIPMENT;
            equipmentTpye = EquipmentTpye.TITLE;
        }
        else if ((ItemSortType)enumsortType == ItemSortType.POSION)
        {
            itemCategoryType = ItemCategoryType.CONSUMABLE;
            consumableType = ConsumableType.POSION;
        }
        else if ((ItemSortType)enumsortType == ItemSortType.ENCHANT)
        {
            itemCategoryType = ItemCategoryType.CONSUMABLE;
            consumableType = ConsumableType.ENCHANT;
        }
        else if ((ItemSortType)enumsortType == ItemSortType.CRAFT)
        {
            itemCategoryType = ItemCategoryType.MATERIAL;
            materialType = MaterialType.CRAFT;
        }
        else if ((ItemSortType)enumsortType == ItemSortType.EXTRA)
        {
            itemCategoryType = ItemCategoryType.MATERIAL;
            materialType = MaterialType.EXTRA;
        }
        else if ((ItemSortType)enumsortType == ItemSortType.QUEST)
        {
            itemCategoryType = ItemCategoryType.QUESTITEM;
            questIType = QuestIType.QUEST;
        }
        SettingSortType();
    }
    public BaseItemClip(BaseItemClip copyClip)
    {
        id = copyClip.id;
        itemName = copyClip.itemName;
        description = copyClip.description;
        texturePath = copyClip.texturePath;
        textureName = copyClip.textureName;
        itemTexture = copyClip.itemTexture;
        sortType = copyClip.sortType;
        uiItemName = copyClip.uiItemName;
        itemCategoryType = copyClip.itemCategoryType;
        itemRankType = copyClip.itemRankType;
        equipmentTpye = copyClip.equipmentTpye;
        consumableType = copyClip.consumableType;
        materialType = copyClip.materialType;
        questIType = copyClip.questIType;
        itemPrefab = copyClip.itemPrefab;
        isOverlap = copyClip.isOverlap;
        havePrefab = copyClip.havePrefab;
        itemPath = copyClip.itemPath;
        itemLevel = copyClip.itemLevel;
        strength = copyClip.strength;
        dexterity = copyClip.dexterity;
        luck = copyClip.luck;
        intelligence = copyClip.intelligence;
        buyCost = copyClip.buyCost;
        sellCost = copyClip.sellCost;
        maxOverlapCount = copyClip.maxOverlapCount;
        isDefaultPotentialPercent = copyClip.isDefaultPotentialPercent;
        potentialPercent = copyClip.potentialPercent;
        useableObjectCount = copyClip.useableObjectCount;
        useableObject = copyClip.useableObject;
        itemCoolTime = copyClip.itemCoolTime;

        minStr = copyClip.minStr;
        maxStr = copyClip.maxStr;
        minDex = copyClip.minDex;
        maxDex = copyClip.maxDex;
        minLuc = copyClip.minLuc;
        maxLuc = copyClip.maxLuc;
        minInt = copyClip.minInt;
        maxInt = copyClip.maxInt;
    }

    public virtual void UseItem(PlayerStateController controller)
    {
        for (int i = 0; i < useableObject.Length; i++)
        {
            Debug.Log("UseItem : " + useableObject[i].name);
            useableObject[i].Apply(controller);
        }
    }

    /// <summary>
    /// 초기 아이템 잠재능력 설정.
    /// </summary>
    public virtual void SetInitRandomPotential()
    {
        SetPotentialCount();
        SetPotentialRank();
    }

    public virtual void SetPotentialValue()
    {

    }

    public void SetUseableObject()
    {
        for (int i = 0; i < useableObjectCount; i++)
        {
            useableObject[i] = Instantiate(useableObject[i]);
        }
    }

    public void SetPotentialRank(ItemPotentialRankType rankType)
    {
        if (rankType == ItemPotentialRankType.NONE)
        {
            potentialRank = ItemPotentialRankType.NONE;
            potentialOptionCount = 0;
            ownPotential = new OwnPotential[0];
        }
        else
            potentialRank = rankType;
    }

    public void SetPotentialCount(int count)
    {
        if (count < 0 || count > 3)
            return;
        potentialOptionCount = count;
    }

    /// <summary>
    /// 잠재능력 갯수 설정.
    /// </summary>
    protected void SetPotentialCount()
    {
        potentialOptionCount = MathHelper.GetRandom(1, 4); // 0, 1, 2, 3
    }

    /// <summary>
    /// 잠재능력 랭크 설정.
    /// </summary>
    protected void SetPotentialRank()
    {
        if (potentialPercent == null || isDefaultPotentialPercent)
            potentialPercent = new int[5] { 40, 30, 20, 6, 4 };

        int[] finalRange = new int[5];
        for (int i = 0; i < 5; i++)
            finalRange[i] = (i == 0) ? potentialPercent[i] : finalRange[i - 1] + potentialPercent[i];

        //  NONE = 30 , NORMAL = 40 , RARE= 20, UNIQUE = 6, LEGENDARY= 4,  
        float randomPercentage = MathHelper.GetRandom(0, finalRange[4]);
        if (randomPercentage > 0 && randomPercentage <= finalRange[0])
        {
            //NONE
            potentialRank = ItemPotentialRankType.NONE;
            potentialOptionCount = 0;
            ownPotential = new OwnPotential[0];

        }
        else if (randomPercentage > potentialPercent[0] && randomPercentage <= finalRange[1])
            potentialRank = ItemPotentialRankType.NORMAL;
        else if (randomPercentage > potentialPercent[1] && randomPercentage <= finalRange[2])
            potentialRank = ItemPotentialRankType.RARE;
        else if (randomPercentage > potentialPercent[2] && randomPercentage <= finalRange[3])
            potentialRank = ItemPotentialRankType.UNIQUE;
        else if (randomPercentage > potentialPercent[3] && randomPercentage <= finalRange[4])
            potentialRank = ItemPotentialRankType.LEGENDARY;
    }


    /// <summary>
    /// 장착템 잠재옵션을 랜덤으로 설정.  
    /// </summary>                       
    public void SetRandomPotentialOption(ItemPotentialRankType potentialRank, PotentialOptionClip[] potentialCategory)
    {
        ownPotential = new OwnPotential[potentialOptionCount];

        if (potentialRank == ItemPotentialRankType.NONE) return;

        PotentialOptionClip[] clips = potentialCategory;

        for (int i = 0; i < potentialOptionCount; i++)
        {
            if (potentialRank == ItemPotentialRankType.NONE) return;

            int index = MathHelper.GetRandom(0, clips.Length);
            PotentialOptionClip potential = clips[index];
            potential.SetRandomPotentialValue(potentialRank);
            OwnPotential tmpPotential = new OwnPotential(potential);
            ownPotential[i] = tmpPotential;
        }
    }


   
    public virtual void InitBaseStats()
    {
        strength = (int)UnityEngine.Random.Range(minStr, maxStr);
        dexterity = (int)UnityEngine.Random.Range(minDex, maxDex);
        luck = (int)UnityEngine.Random.Range(minLuc, maxLuc);
        intelligence = (int)UnityEngine.Random.Range(minInt, maxInt);
    }


    /// <summary>
    /// Item Tool에서 사용되는 SortType을 설정.
    /// </summary>
    public void SettingSortType()
    {
        if (itemCategoryType == ItemCategoryType.EQUIPMENT && equipmentTpye == EquipmentTpye.WEAPON)
            sortType = ItemSortType.WEAPON;
        else if (itemCategoryType == ItemCategoryType.EQUIPMENT && equipmentTpye == EquipmentTpye.ARMOR)
            sortType = ItemSortType.ARMOR;
        else if (itemCategoryType == ItemCategoryType.EQUIPMENT && equipmentTpye == EquipmentTpye.ACCESSORIES)
            sortType = ItemSortType.ACCESSORY;
        else if (itemCategoryType == ItemCategoryType.EQUIPMENT && equipmentTpye == EquipmentTpye.TITLE)
            sortType = ItemSortType.TITLE;
        else if (itemCategoryType == ItemCategoryType.CONSUMABLE && consumableType == ConsumableType.POSION)
            sortType = ItemSortType.POSION;
        else if (itemCategoryType == ItemCategoryType.CONSUMABLE && consumableType == ConsumableType.ENCHANT)
            sortType = ItemSortType.ENCHANT;
        else if (itemCategoryType == ItemCategoryType.MATERIAL && materialType == MaterialType.CRAFT)
            sortType = ItemSortType.CRAFT;
        else if (itemCategoryType == ItemCategoryType.MATERIAL && materialType == MaterialType.EXTRA)
            sortType = ItemSortType.EXTRA;
        else if (itemCategoryType == ItemCategoryType.QUESTITEM && questIType == QuestIType.QUEST)
            sortType = ItemSortType.QUEST;
    }

    public void ReloadResource()
    {
        texturePath = texturePath.Trim();
        textureName = textureName.Trim();
        textureFullPath = texturePath + textureName;

        itemName = itemName.Trim();
        itemPath = itemPath.Trim();
        itemFullPath = itemPath + itemName;

        if (textureName != string.Empty && itemTexture == null)
            itemTexture = Resources.Load<Sprite>(textureFullPath);

        if (havePrefab && itemName != string.Empty && itemPrefab == null)
            itemPrefab = Resources.Load(itemFullPath) as GameObject;

    }


    public void SetItemInstanceID()
    {
        instanceID = GetHashCode();
    }

    public void SetBaseItemStatus(PlayerStatus playerStatus , bool isEquip)
    {
        if (isEquip)
        {
            playerStatus.ExtraStrength += (int)strength;
            playerStatus.ExtraDexterity += (int)dexterity;
            playerStatus.ExtraLuck += (int)luck;
            playerStatus.ExtraIntelligence += (int)intelligence;
        }
        else
        {
            playerStatus.ExtraStrength -= (int)strength;
            playerStatus.ExtraDexterity -= (int)dexterity;
            playerStatus.ExtraLuck -= (int)luck;
            playerStatus.ExtraIntelligence -= (int)intelligence;
        }
    }


    public void SetSkinnedMeshContainerName()
    {
       // if(havePrefab)
       // {
       //     SkinnedMeshContainer container = itemPrefab.GetComponent<SkinnedMeshContainer>();
       //     container.SetItemName(itemName);
       // }
    }

}

