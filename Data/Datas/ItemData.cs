using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

/// <summary>
/// 모든 아이템 데이터 
/// </summary>
public class ItemData : BaseData
{
    public BaseItemClip[] allItemClips = new BaseItemClip[0];  //전체 아이템
    public int allRealInex = 0;
    private string enumListName = "ItemList";

    [Header("Equipment Item")]
    public BaseItemClip[] equipmentItemClips = new BaseItemClip[0]; //장비 분류별 
    public WeaponItemClip[] weaponItemClips = new WeaponItemClip[0];
    public ArmorItemClip[] armorItemClips = new ArmorItemClip[0];
    public AccessoryItemClip[] accessoryItemClips = new AccessoryItemClip[0];
    public TitleItemClip[] titleItemClips = new TitleItemClip[0];

    [Header("Consumable Item")]
    public BaseItemClip[] consumableItemClips = new BaseItemClip[0]; //소비템 분류별 
    public PosionItemClip[] posionItemClips = new PosionItemClip[0];
    public EnchantItemClip[] enchantItemClips = new EnchantItemClip[0];

    [Header("Material Item")]
    public BaseItemClip[] materialItemClips = new BaseItemClip[0]; //재료 분류별 
    public CraftItemClip[] craftItemClips = new CraftItemClip[0];
    public ExtraItemClip[] extraItemClips = new ExtraItemClip[0];

    [Header("Quest Item")]
    public QuestItemClip[] questItemClips = new QuestItemClip[0]; //퀘스트 아이템 분류별 

    [Header("CSV Path")]
    private string csvFilePath = "";    // application.datapath + dataDirectory + csvFolderPath + csvFileName;
    private string csvFolderPath = "/CSV/";
    private string csvFileName = "ItemData.csv";
    private string dataPath = "Data/CSV/ItemData";

    [Header("UseableDatabase Path")]
    private string useableDatabasePath = "Data/FinalScriptableObject/0.Database/0.UseableObjectDatabase/RootUseableObjectDatabase";
    private RootUseableObjectDatabase rootUseableObjectDatabase;

    public void LoadData()
    {
        csvFilePath = Application.dataPath + dataDirectory + csvFolderPath + csvFileName;
        rootUseableObjectDatabase = Resources.Load(useableDatabasePath) as RootUseableObjectDatabase;

        TextAsset asset = Resources.Load(dataPath) as TextAsset;
        string allText = string.Empty;
        if (asset != null)
            allText = asset.text;

        allItemClips = new BaseItemClip[0];
        string[] enterSplit = allText.Split('\n');
        for (int i = 1; i < enterSplit.Length -1; i++)
        {
            string[] splitTab = enterSplit[i].Split(',');

            ItemSortType clipType = (ItemSortType)int.Parse(splitTab[2]);
            if (clipType == ItemSortType.WEAPON)
            {
                WeaponItemClip weaponclip = new WeaponItemClip();
                LoadCommonData(weaponclip, splitTab);
                weaponclip.requiredLevel = int.Parse(splitTab[31]);
                weaponclip.enchantLevel = int.Parse(splitTab[32]);
                weaponclip.enchantLeftCount = int.Parse(splitTab[33]);
                weaponclip.enchantLimitCount = int.Parse(splitTab[34]);
                weaponclip.potentialPercent[0] = int.Parse(splitTab[35]);
                weaponclip.potentialPercent[1] = int.Parse(splitTab[36]);
                weaponclip.potentialPercent[2] = int.Parse(splitTab[37]);
                weaponclip.potentialPercent[3] = int.Parse(splitTab[38]);
                weaponclip.potentialPercent[4] = int.Parse(splitTab[39]);

                //수정목록
                weaponclip.minAtk = float.Parse(splitTab[40]);
                weaponclip.maxAtk = float.Parse(splitTab[41]);
                weaponclip.minAtkSpeed = float.Parse(splitTab[42]);
                weaponclip.maxAtkSpeed = float.Parse(splitTab[43]);
                weaponclip.minCriChance = float.Parse(splitTab[44]);
                weaponclip.maxCriChance = float.Parse(splitTab[45]);
                weaponclip.minCriDmg = float.Parse(splitTab[46]);
                weaponclip.maxCriDmg = float.Parse(splitTab[47]);

                allItemClips = ArrayHelper.Add(weaponclip, allItemClips);

            }
            else if (clipType == ItemSortType.ARMOR)
            {
                ArmorItemClip armorClip = new ArmorItemClip();
                LoadCommonData(armorClip, splitTab);
                armorClip.requiredLevel = int.Parse(splitTab[31]);
                armorClip.enchantLevel = int.Parse(splitTab[32]);
                armorClip.enchantLeftCount = int.Parse(splitTab[33]);
                armorClip.enchantLimitCount = int.Parse(splitTab[34]);
                armorClip.potentialPercent[0] = int.Parse(splitTab[35]);
                armorClip.potentialPercent[1] = int.Parse(splitTab[36]);
                armorClip.potentialPercent[2] = int.Parse(splitTab[37]);
                armorClip.potentialPercent[3] = int.Parse(splitTab[38]);
                armorClip.potentialPercent[4] = int.Parse(splitTab[39]);

                armorClip.armorType = (ArmorType)int.Parse(splitTab[48]);
                //수정목록
                armorClip.minDef = int.Parse(splitTab[56]);
                armorClip.maxDef = int.Parse(splitTab[57]);
                armorClip.minMagicDef = int.Parse(splitTab[58]);
                armorClip.maxMagicDef = int.Parse(splitTab[59]);
                armorClip.minEvasion = float.Parse(splitTab[60]);
                armorClip.maxEvasion = float.Parse(splitTab[61]);
                armorClip.minHpRegeneration = float.Parse(splitTab[62]);
                armorClip.maxHpRegeneration = float.Parse(splitTab[63]);
                armorClip.minStRegeneration = float.Parse(splitTab[64]);
                armorClip.maxStRegeneration = float.Parse(splitTab[65]);

                allItemClips = ArrayHelper.Add(armorClip, allItemClips);

            }
            else if (clipType == ItemSortType.ACCESSORY)
            {
                AccessoryItemClip accessoryClip = new AccessoryItemClip();
                LoadCommonData(accessoryClip, splitTab);
                accessoryClip.requiredLevel = int.Parse(splitTab[31]);
                accessoryClip.enchantLevel = int.Parse(splitTab[32]);
                accessoryClip.enchantLeftCount = int.Parse(splitTab[33]);
                accessoryClip.enchantLimitCount = int.Parse(splitTab[34]);
                accessoryClip.potentialPercent[0] = int.Parse(splitTab[35]);
                accessoryClip.potentialPercent[1] = int.Parse(splitTab[36]);
                accessoryClip.potentialPercent[2] = int.Parse(splitTab[37]);
                accessoryClip.potentialPercent[3] = int.Parse(splitTab[38]);
                accessoryClip.potentialPercent[4] = int.Parse(splitTab[39]);

                accessoryClip.minAtk = float.Parse(splitTab[40]);
                accessoryClip.maxAtk = float.Parse(splitTab[41]);
                accessoryClip.minAtkSpeed = float.Parse(splitTab[42]);
                accessoryClip.maxAtkSpeed = float.Parse(splitTab[43]);
                accessoryClip.minCriChance = float.Parse(splitTab[44]);
                accessoryClip.maxCriChance = float.Parse(splitTab[45]);
                accessoryClip.minCriDmg = float.Parse(splitTab[46]);
                accessoryClip.maxCriDmg = float.Parse(splitTab[47]);
                accessoryClip.accessoryType = (AccessoryType)int.Parse(splitTab[49]);
                accessoryClip.minDef = int.Parse(splitTab[56]);
                accessoryClip.maxDef = int.Parse(splitTab[57]);
                accessoryClip.minMagicDef = int.Parse(splitTab[58]);
                accessoryClip.maxMagicDef = int.Parse(splitTab[59]);
                accessoryClip.minEvasion = float.Parse(splitTab[60]);
                accessoryClip.maxEvasion = float.Parse(splitTab[61]);
                accessoryClip.minHpRegeneration = float.Parse(splitTab[62]);
                accessoryClip.maxHpRegeneration = float.Parse(splitTab[63]);
                accessoryClip.minStRegeneration = float.Parse(splitTab[64]);
                accessoryClip.maxStRegeneration = float.Parse(splitTab[65]);
                accessoryClip.minAllStats = int.Parse(splitTab[66]);
                accessoryClip.maxAllStats = int.Parse(splitTab[67]);

                allItemClips = ArrayHelper.Add(accessoryClip, allItemClips);

            }
            else if (clipType == ItemSortType.TITLE)
            {
                TitleItemClip titleClip = new TitleItemClip();
                LoadCommonData(titleClip, splitTab);

                titleClip.minAtk = float.Parse(splitTab[40]);
                titleClip.maxAtk = float.Parse(splitTab[41]);
                titleClip.minAtkSpeed = float.Parse(splitTab[42]);
                titleClip.maxAtkSpeed = float.Parse(splitTab[43]);
                titleClip.minCriChance = float.Parse(splitTab[44]);
                titleClip.maxCriChance = float.Parse(splitTab[45]);
                titleClip.minCriDmg = float.Parse(splitTab[46]);
                titleClip.maxCriDmg = float.Parse(splitTab[47]);
                titleClip.minDef = int.Parse(splitTab[56]);
                titleClip.maxDef = int.Parse(splitTab[57]);
                titleClip.minMagicDef = int.Parse(splitTab[58]);
                titleClip.maxMagicDef = int.Parse(splitTab[59]);
                titleClip.minHpRegeneration = float.Parse(splitTab[62]);
                titleClip.maxHpRegeneration = float.Parse(splitTab[63]);
                titleClip.minStRegeneration = float.Parse(splitTab[64]);
                titleClip.maxStRegeneration = float.Parse(splitTab[65]);
                titleClip.minAllStats = int.Parse(splitTab[66]);
                titleClip.maxAllStats = int.Parse(splitTab[67]);

                allItemClips = ArrayHelper.Add(titleClip, allItemClips);
            }
            else if (clipType == ItemSortType.POSION)
            {
                PosionItemClip postionClip = new PosionItemClip();
                LoadCommonData(postionClip, splitTab);
                //postionClip.posionType = (BuffType)int.Parse(splitTab[48]);
                //postionClip.posionValue = float.Parse(splitTab[53]);
                //postionClip.durationTime = float.Parse(splitTab[66]);

                allItemClips = ArrayHelper.Add(postionClip, allItemClips);
            }
            else if (clipType == ItemSortType.ENCHANT)
            {
                EnchantItemClip enchantClip = new EnchantItemClip();
                LoadCommonData(enchantClip, splitTab);
                enchantClip.enchantType = (EnchantType)int.Parse(splitTab[51]);
                enchantClip.minValue = float.Parse(splitTab[52]);
                enchantClip.maxValue = float.Parse(splitTab[53]);
                enchantClip.successPercent = float.Parse(splitTab[54]);

                allItemClips = ArrayHelper.Add(enchantClip, allItemClips);
            }
            else if(clipType == ItemSortType.CRAFT)
            {
                CraftItemClip craftClip = new CraftItemClip();
                LoadCommonData(craftClip, splitTab);
                allItemClips = ArrayHelper.Add(craftClip, allItemClips);
            }
            else if (clipType == ItemSortType.EXTRA)
            {
                ExtraItemClip extraClip = new ExtraItemClip();
                LoadCommonData(extraClip, splitTab);
                allItemClips = ArrayHelper.Add(extraClip, allItemClips);
            }
            else if (clipType == ItemSortType.QUEST)
            {
                QuestItemClip questClip = new QuestItemClip();
                LoadCommonData(questClip, splitTab);
                allItemClips = ArrayHelper.Add(questClip, allItemClips);
            }
        }
        allRealInex = allItemClips.Length;


    }

    public void SaveData()
    {
        csvFilePath = Application.dataPath + dataDirectory + csvFolderPath + csvFileName;
        using (StreamWriter sw = new StreamWriter(csvFilePath , false, Encoding.UTF8))
        {
            //수정목록
            string line = "ID,ItemName,SortType,UIItemName,ItemType,ItemRankType,EuipmentType,ConsumableType,MaterialType,QuestType,Is Unbreakable,IsOverlap,HavePrefab,Description," +
                "ItemPath,TexturePath,TextureName,ItemLv,MinStr,MaxStr,MinDex,MaxDex,MinLuc,MaxLuc,MinInt,MaxInt,BuyCost,SellCost,RepurchaseCost ,MaxOverlapCount,IsDefaultPP,RequiredLv,EnchantLv,EnchantLeftCount," +
                "EnchantLimitCount,PotentialPercent[0],[1],[2],[3],[4],MinAtk,MaxAtk,MinAtkSpeed,MaxAtkSpeed,MinCriChance,MaxCriChance,MinCriDmg,MaxCriDmg,ArmorType,AccessoryType,PosionType,EnchantType,MinValue,MaxValue," +
                "SuccesPercent,PosionValue,MinDef,MaxDef,MinMagicDef,MaxMagicDef,MinEvasion,MaxEvasion,MinHpRegeneration,MaxHpRegeneration,MinStaminaRegeneration,MaxStaminaRegeneration,MinAllStats,MaxAllStats,posionDuration," +
                "Item Cool Time ,UseableObject Count,UseableObject ID";

            sw.WriteLine(line);

            for (int i = 0; i < allItemClips.Length; i++)
            {
                //수정목록
                line =   i +"," +                                        //  0
                         allItemClips[i].itemName + "," +                //  1
                         (int)allItemClips[i].sortType +"," +             // 2
                         allItemClips[i].uiItemName + "," +              //  3
                         (int)allItemClips[i].itemCategoryType + "," +           //  4
                         (int)allItemClips[i].itemRankType + "," +        // 5
                         (int)allItemClips[i].equipmentTpye + "," +       // 6
                         (int)allItemClips[i].consumableType + "," +      // 7
                         (int)allItemClips[i].materialType + "," +       //  8
                         (int)allItemClips[i].questIType + "," +         //  9
                         allItemClips[i].isUnbreakable + "," +
                         allItemClips[i].isOverlap + "," +              //   10
                         allItemClips[i].havePrefab + "," +             //   11
                         allItemClips[i].description + "," +             //  12
                         allItemClips[i].itemPath + "," +               //   13
                         allItemClips[i].texturePath + "," +            //   14
                         allItemClips[i].textureName + "," +            //   15
                         allItemClips[i].itemLevel + "," +              //   16
                         allItemClips[i].minStr + "," +                 //   17
                         allItemClips[i].maxStr + "," +                 //   18
                         allItemClips[i].minDex + "," +                 //   19
                         allItemClips[i].maxDex + "," +                 //   20
                         allItemClips[i].minLuc + "," +                 //   21
                         allItemClips[i].maxLuc + "," +                 //   22
                         allItemClips[i].minInt + "," +                 //   23
                         allItemClips[i].maxInt + "," +                 //   24
                         allItemClips[i].buyCost + "," +                //   25
                         allItemClips[i].sellCost + "," +               //   26
                         allItemClips[i].repurchaseCost + "," +         
                         allItemClips[i].maxOverlapCount + "," +        //   27
                         allItemClips[i].isDefaultPotentialPercent + ",";   //  28

                if (allItemClips[i] is WeaponItemClip)
                {
                    //수정목록
                    WeaponItemClip clip = allItemClips[i] as WeaponItemClip;
                    line += clip.requiredLevel + "," +
                            clip.enchantLevel + "," +
                            clip.enchantLeftCount + "," +
                            clip.enchantLimitCount + "," +
                            clip.potentialPercent[0] + "," +
                            clip.potentialPercent[1] + "," +
                            clip.potentialPercent[2] + "," +
                            clip.potentialPercent[3] + "," +
                            clip.potentialPercent[4] + "," +
                            clip.minAtk + "," +
                            clip.maxAtk + "," +
                            clip.minAtkSpeed + "," +
                            clip.maxAtkSpeed + "," +
                            clip.minCriChance + "," +
                            clip.maxCriChance + "," +
                            clip.minCriDmg + "," +
                            clip.maxCriDmg + "," +                //17
                            0 + "," + 0 + "," + 0 + "," + 0 + "," +
                            0 + "," + 0 + "," + 0 + "," + 0 + "," +
                            0 + "," + 0 + "," + 0 + "," + 0 + "," +
                            0 + "," + 0 + "," + 0 + "," + 0 + "," +
                            0 + "," + 0 + "," + 0 + "," + 0 + "," +
                            0 + ",";                 //21
                }
                else if (allItemClips[i] is ArmorItemClip)
                {
                    //수정목록 //27
                    ArmorItemClip clip = allItemClips[i] as ArmorItemClip;
                    line += clip.requiredLevel + "," +
                        clip.enchantLevel + "," +
                        clip.enchantLeftCount + "," +
                        clip.enchantLimitCount + "," +
                        clip.potentialPercent[0] + "," +
                        clip.potentialPercent[1] + "," +
                        clip.potentialPercent[2] + "," +
                        clip.potentialPercent[3] + "," +
                        clip.potentialPercent[4] + "," +  
                        0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 
                        (int)clip.armorType + "," +    
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        clip.minDef + "," +
                        clip.maxDef + "," +
                        clip.minMagicDef + "," +
                        clip.maxMagicDef + "," +
                        clip.minEvasion + "," +
                        clip.maxEvasion + "," +
                        clip.minHpRegeneration + "," +
                        clip.maxHpRegeneration + "," +
                        clip.minStRegeneration + "," +
                        clip.maxStRegeneration + "," +
                        0 + "," + 0 + "," + 0 + ",";
                }
                else if (allItemClips[i] is AccessoryItemClip)   //이거 잘 작동하는지 확인
                {
                    //수정목록
                    AccessoryItemClip clip = allItemClips[i] as AccessoryItemClip;
                    line += clip.requiredLevel + "," +
                        clip.enchantLevel + "," +
                        clip.enchantLeftCount + "," +
                        clip.enchantLimitCount + "," +
                        clip.potentialPercent[0] + "," +
                        clip.potentialPercent[1] + "," +
                        clip.potentialPercent[2] + "," +
                        clip.potentialPercent[3] + "," +
                        clip.potentialPercent[4] + "," +   //9
                        clip.minAtk + "," +
                        clip.maxAtk + "," +
                        clip.minAtkSpeed + "," +
                        clip.maxAtkSpeed + "," +
                        clip.minCriChance + "," +
                        clip.maxCriChance + "," +
                        clip.minCriDmg + "," +
                        clip.maxCriDmg + "," +         //17
                        0 + "," +
                        (int)clip.accessoryType + "," + 
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        clip.minDef + "," +
                        clip.maxDef + "," +
                        clip.minMagicDef + "," +
                        clip.maxMagicDef + "," +
                        clip.minEvasion + "," +
                        clip.maxEvasion + "," +
                        clip.minHpRegeneration + "," +
                        clip.maxHpRegeneration + "," +
                        clip.minStRegeneration + "," +
                        clip.maxStRegeneration + "," +
                        clip.minAllStats + "," +
                        clip.maxAllStats + "," + 0 + ",";
                }
                else if (allItemClips[i] is TitleItemClip)
                {
                    //수정목록
                    TitleItemClip clip = allItemClips[i] as TitleItemClip;
                    line += 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        clip.potentialPercent[0] + "," +
                        clip.potentialPercent[1] + "," +
                        clip.potentialPercent[2] + "," +
                        clip.potentialPercent[3] + "," +
                        clip.potentialPercent[4] + "," +   //9
                        clip.minAtk + "," +
                        clip.maxAtk + "," +
                        clip.minAtkSpeed + "," +
                        clip.maxAtkSpeed + "," +
                        clip.minCriChance + "," +
                        clip.maxCriChance + "," +
                        clip.minCriDmg + "," +
                        clip.maxCriDmg + "," +     //17
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + " ," + 0 + "," +
                        clip.minDef + "," +
                        clip.maxDef + "," +
                        clip.minMagicDef + "," +
                        clip.maxMagicDef + "," +
                        0 + "," + 0 + "," +
                        clip.minHpRegeneration + "," +
                        clip.maxHpRegeneration + "," +
                        clip.minStRegeneration + "," +
                        clip.maxStRegeneration + "," +
                        clip.minAllStats + "," +
                        clip.maxAllStats + "," +
                        0 + ",";
                }
                else if (allItemClips[i] is PosionItemClip)
                {
                    PosionItemClip clip = allItemClips[i] as PosionItemClip;
                    line += 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," +
                        0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," + 
                        0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + ",";
                }
                else if (allItemClips[i] is EnchantItemClip)
                {
                    EnchantItemClip clip = allItemClips[i] as EnchantItemClip;
                    line += 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +    
                        (int)clip.enchantType + "," +
                        clip.minValue + "," +
                        clip.maxValue + "," +
                        clip.successPercent + "," +                                                                          
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +    
                        0 + "," + 0 + "," + 0 + "," + 0 + ",";


                }
                else if (allItemClips[i] is CraftItemClip)
                {
                    CraftItemClip clip = allItemClips[i] as CraftItemClip;
                    line += 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0  +","+
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + ",";
                }
                else if (allItemClips[i] is ExtraItemClip)
                {
                    ExtraItemClip clip = allItemClips[i] as ExtraItemClip;
                    line += 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                        0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + ",";
                }
                else if (allItemClips[i] is QuestItemClip)
                {
                    QuestItemClip clip = allItemClips[i] as QuestItemClip;
                    line += 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                         0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                         0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," +
                         0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + ",";
                }

                line += allItemClips[i].itemCoolTime + "," +
                    allItemClips[i].useableObjectCount + ",";
                for (int x = 0; x < allItemClips[i].useableObjectCount; x++)
                {
                    if (allItemClips[i].useableObject[x] != null)
                    {
                        if (x == allItemClips[i].useableObjectCount - 1)
                            line += allItemClips[i].useableObject[x].ID;
                        else
                            line += allItemClips[i].useableObject[x].ID + ",";
                    }
                    else
                        line += -1 + ",";
                }
                sw.WriteLine(line);
            }
        }
    }


    public T LoadCommonData<T>(T itemData, string[] splitTab) where T : BaseItemClip
    {
        T clip = itemData;
        clip.id = int.Parse(splitTab[0]);
        clip.itemName = splitTab[1];
        clip.sortType = (ItemSortType)int.Parse(splitTab[2]);
        clip.uiItemName = splitTab[3];
        clip.itemCategoryType = (ItemCategoryType)int.Parse(splitTab[4]);
        clip.itemRankType = (ItemRankType)int.Parse(splitTab[5]);
        clip.equipmentTpye = (EquipmentTpye)int.Parse(splitTab[6]);
        clip.consumableType = (ConsumableType)int.Parse(splitTab[7]);
        clip.materialType = (MaterialType)int.Parse(splitTab[8]);
        clip.questIType = (QuestIType)int.Parse(splitTab[9]);
        clip.isUnbreakable = bool.Parse(splitTab[10]);
        clip.isOverlap = bool.Parse(splitTab[11]);
        clip.havePrefab = bool.Parse(splitTab[12]);
        clip.description = splitTab[13];
        clip.itemPath = splitTab[14];
        clip.texturePath = splitTab[15];
        clip.textureName = splitTab[16];
        clip.itemLevel = int.Parse(splitTab[17]);

        //수정목록
        clip.minStr = int.Parse(splitTab[18]);
        clip.maxStr = int.Parse(splitTab[19]);
        clip.minDex = int.Parse(splitTab[20]);
        clip.maxDex = int.Parse(splitTab[21]);
        clip.minLuc = int.Parse(splitTab[22]);
        clip.maxLuc = int.Parse(splitTab[23]);
        clip.minInt = int.Parse(splitTab[24]);
        clip.maxInt = int.Parse(splitTab[25]);

        clip.buyCost = int.Parse(splitTab[26]);
        clip.sellCost = int.Parse(splitTab[27]);
        clip.repurchaseCost = int.Parse(splitTab[28]);
        clip.maxOverlapCount = int.Parse(splitTab[29]);
        clip.isDefaultPotentialPercent = bool.Parse(splitTab[30]);


        if (rootUseableObjectDatabase != null)
        {
            clip.itemCoolTime = float.Parse(splitTab[69]);
            clip.useableObjectCount = int.Parse(splitTab[70]);
            clip.useableObject = new UseableObject[clip.useableObjectCount];

            if (clip.useableObjectCount > 0)
            {
                for (int i = 0; i < clip.useableObjectCount; i++)
                {
                    if (int.Parse(splitTab[71 + i]) == -1)
                        continue;
                    clip.useableObject[i] = rootUseableObjectDatabase.GetUseableObject(int.Parse(splitTab[71 + i]));
                }
            }
        }


        return clip;
    }

    public override void AddData(string newName, int enumNum)
    {
        if (allItemClips == null)
        {
            allRealInex = 0;
            allItemClips = new BaseItemClip[0];
        }

        if ((ItemSortType)enumNum == ItemSortType.WEAPON)
            allItemClips = ArrayHelper.Add(new WeaponItemClip(allRealInex, newName, enumNum), allItemClips);
        else if ((ItemSortType)enumNum == ItemSortType.ARMOR)
            allItemClips = ArrayHelper.Add(new ArmorItemClip(allRealInex, newName, enumNum), allItemClips);
        else if ((ItemSortType)enumNum == ItemSortType.ACCESSORY)
            allItemClips = ArrayHelper.Add(new AccessoryItemClip(allRealInex, newName, enumNum), allItemClips);
        else if ((ItemSortType)enumNum == ItemSortType.TITLE)
            allItemClips = ArrayHelper.Add(new TitleItemClip(allRealInex, newName, enumNum), allItemClips);
        else if ((ItemSortType)enumNum == ItemSortType.POSION)
            allItemClips = ArrayHelper.Add(new PosionItemClip(allRealInex, newName, enumNum), allItemClips);
        else if ((ItemSortType)enumNum == ItemSortType.ENCHANT)
            allItemClips = ArrayHelper.Add(new EnchantItemClip(allRealInex, newName, enumNum), allItemClips);
        else if ((ItemSortType)enumNum == ItemSortType.CRAFT)
            allItemClips = ArrayHelper.Add(new CraftItemClip(allRealInex, newName, enumNum), allItemClips);
        else if ((ItemSortType)enumNum == ItemSortType.EXTRA)
            allItemClips = ArrayHelper.Add(new ExtraItemClip(allRealInex, newName, enumNum), allItemClips);
        else if ((ItemSortType)enumNum == ItemSortType.QUEST)
            allItemClips = ArrayHelper.Add(new QuestItemClip(allRealInex, newName, enumNum), allItemClips);

        allRealInex += 1;
    }

    public override int FindIDToIndex(int targetID)
    {
        for (int i = 0; i < allItemClips.Length; i++)
            if (allItemClips[i].id == targetID)
                return i;

        return 0;
    }

    public override void Copy(int index)
    {
        if (index < 0 || index > allItemClips.Length)
            return;
        ItemSortType itemDataType = allItemClips[index].sortType;

        switch (itemDataType)
        {
            case ItemSortType.WEAPON:
                WeaponItemClip newWeaponClip = Instantiate(allItemClips[index] as WeaponItemClip);
                newWeaponClip.id = allRealInex;
                allItemClips = ArrayHelper.Add(newWeaponClip, allItemClips);
                allRealInex += 1;
                break;
            case ItemSortType.ARMOR:
                ArmorItemClip newArmorClip = Instantiate(allItemClips[index] as ArmorItemClip);
                newArmorClip.id = allRealInex;
                allItemClips = ArrayHelper.Add(newArmorClip, allItemClips);
                allRealInex += 1;
                break;
            case ItemSortType.ACCESSORY:
                AccessoryItemClip newAccessoryClip = Instantiate(allItemClips[index]as AccessoryItemClip);
                newAccessoryClip.id = allRealInex;
                allItemClips = ArrayHelper.Add(newAccessoryClip, allItemClips);
                allRealInex += 1;
                break;
            case ItemSortType.TITLE:
                TitleItemClip newTitleClip = Instantiate(allItemClips[index]as TitleItemClip);
                newTitleClip.id = allRealInex;
                allItemClips = ArrayHelper.Add(newTitleClip, allItemClips);
                allRealInex += 1;
                break;
            case ItemSortType.POSION:
                PosionItemClip newPosionClip = Instantiate(allItemClips[index]as PosionItemClip);
                newPosionClip.id = allRealInex;
                allItemClips = ArrayHelper.Add(newPosionClip, allItemClips);
                allRealInex += 1;
                break;
            case ItemSortType.ENCHANT:
                EnchantItemClip newEnchantClip = Instantiate(allItemClips[index]as EnchantItemClip);
                newEnchantClip.id = allRealInex;
                allItemClips = ArrayHelper.Add(newEnchantClip, allItemClips);
                allRealInex += 1;
                break;
            case ItemSortType.CRAFT:
                CraftItemClip newCraftClip = Instantiate(allItemClips[index]as CraftItemClip);
                newCraftClip.id = allRealInex;
                allItemClips = ArrayHelper.Add(newCraftClip, allItemClips);
                allRealInex += 1;
                break;
            case ItemSortType.EXTRA:
                ExtraItemClip newExtraClip = Instantiate(allItemClips[index]as ExtraItemClip);
                newExtraClip.id = allRealInex;
                allItemClips = ArrayHelper.Add(newExtraClip, allItemClips);
                allRealInex += 1;
                break;
            case ItemSortType.QUEST:
                QuestItemClip newQuestClip = Instantiate(allItemClips[index]as QuestItemClip);
                newQuestClip.id = allRealInex;
                allItemClips = ArrayHelper.Add(newQuestClip, allItemClips);
                allRealInex += 1;
                break;
        }
        UpdateSortClips();

    }

    public override void RemoveData(int index)
    {
        if (index < 0 || index > allItemClips.Length)
            return;

        allItemClips = ArrayHelper.Remove(index, allItemClips);
        allRealInex = 0;
        for (int i = 0; i < allItemClips.Length; i++)
        {
            allItemClips[i].id = allRealInex;
            allRealInex += 1;
        }

        UpdateSortClips();

    }

    public string[] ReturnTypeNameList(ItemEditorCategoryType type)
    {
        switch (type)
        {
            case ItemEditorCategoryType.ALL:
                return SetNameList(allItemClips);
            case ItemEditorCategoryType.WEAPON:
                return SetNameList(weaponItemClips);
            case ItemEditorCategoryType.ARMOR:
                return SetNameList(armorItemClips);
            case ItemEditorCategoryType.ACCESSORY:
                return SetNameList(accessoryItemClips);
            case ItemEditorCategoryType.TITLE:
                return SetNameList(titleItemClips);
            case ItemEditorCategoryType.POSION:
                return SetNameList(posionItemClips);
            case ItemEditorCategoryType.ENCHANT:
                return SetNameList(enchantItemClips);
            case ItemEditorCategoryType.CRAFT:
                return SetNameList(craftItemClips);
            case ItemEditorCategoryType.EXTRA:
                return SetNameList(extraItemClips);
            case ItemEditorCategoryType.QUEST:
                return SetNameList(questItemClips);
            case ItemEditorCategoryType.EQUIPMENT:
                return SetNameList(equipmentItemClips);
            case ItemEditorCategoryType.CONSUMABLE:
                return SetNameList(consumableItemClips);
            case ItemEditorCategoryType.MATERIAL:
                return SetNameList(materialItemClips);
            default:
                return SetNameList(allItemClips);
        }

    }

    private string[] SetNameList(BaseItemClip[] array)
    {
        string[] retNameList = new string[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            retNameList[i] = array[i].itemName;
        }
        return retNameList;
    }

    public void UpdateSortClips()
    {
        equipmentItemClips = allItemClips.Where(i => i.sortType == ItemSortType.WEAPON || i.sortType == ItemSortType.ARMOR
                                                  || i.sortType == ItemSortType.ACCESSORY || i.sortType == ItemSortType.TITLE).ToArray();
        weaponItemClips = allItemClips.OfType<WeaponItemClip>().ToArray();
        armorItemClips = allItemClips.OfType<ArmorItemClip>().ToArray();
        accessoryItemClips = allItemClips.OfType<AccessoryItemClip>().ToArray();
        titleItemClips = allItemClips.OfType<TitleItemClip>().ToArray();

        consumableItemClips = allItemClips.Where(i => i.sortType == ItemSortType.POSION || i.sortType == ItemSortType.ENCHANT).ToArray();

        posionItemClips = allItemClips.OfType<PosionItemClip>().ToArray();
        enchantItemClips = allItemClips.OfType<EnchantItemClip>().ToArray();

        materialItemClips = allItemClips.Where(i => i.sortType == ItemSortType.CRAFT || i.sortType == ItemSortType.EXTRA).ToArray();

        craftItemClips = allItemClips.OfType<CraftItemClip>().ToArray();
        extraItemClips = allItemClips.OfType<ExtraItemClip>().ToArray();

        questItemClips = allItemClips.OfType<QuestItemClip>().ToArray();
    }

    public BaseItemClip[] GetClipArray(ItemEditorCategoryType type)
    {
        switch (type)
        {
            case ItemEditorCategoryType.WEAPON:
                return weaponItemClips;
            case ItemEditorCategoryType.ARMOR:
                return armorItemClips;
            case ItemEditorCategoryType.ACCESSORY:
                return accessoryItemClips;
            case ItemEditorCategoryType.TITLE:
                return titleItemClips;
            case ItemEditorCategoryType.POSION:
                return posionItemClips;
            case ItemEditorCategoryType.ENCHANT:
                return enchantItemClips;
            case ItemEditorCategoryType.CRAFT:
                return craftItemClips;
            case ItemEditorCategoryType.EXTRA:
                return extraItemClips;
            case ItemEditorCategoryType.QUEST:
                return questItemClips;
            case ItemEditorCategoryType.ALL:
                return allItemClips;
            case ItemEditorCategoryType.EQUIPMENT:
                return equipmentItemClips;
            case ItemEditorCategoryType.CONSUMABLE:
                return consumableItemClips;
            case ItemEditorCategoryType.MATERIAL:
                return materialItemClips;
            default:
                return null;
        }
    }

#if UNITY_EDITOR
    public void CreateEnumList()
    {
        UpdateSortClips();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();

        for (int i = 0; i < allItemClips.Length; i++)
        {
            sb.AppendLine("     " + GetInspectorName(allItemClips[i]));
            sb.AppendLine("     " + allItemClips[i].itemName + "=" + allItemClips[i].id + ",");
        }

        EditorHelper.CreateEnumList(enumListName, sb);

    }
#endif


    #region Get 함수
    private string GetInspectorName(BaseItemClip clip)
    {
        string retName = "[InspectorName(|*|)]";

        switch (clip.itemCategoryType)
        {
            case ItemCategoryType.EQUIPMENT:
                if (clip.equipmentTpye == EquipmentTpye.WEAPON)
                    retName = retName.Replace("*", "Equipment/Weapon/" + clip.itemName);
                else if (clip.equipmentTpye == EquipmentTpye.ARMOR)
                    retName = retName.Replace("*", "Equipment/Armor/" + clip.itemName);
                else if (clip.equipmentTpye == EquipmentTpye.ACCESSORIES)
                    retName = retName.Replace("*", "Equipment/Accessorie/" + clip.itemName);
                else if (clip.equipmentTpye == EquipmentTpye.TITLE)
                    retName = retName.Replace("*", "Equipment/Title/" + clip.itemName);
                break;

            case ItemCategoryType.CONSUMABLE:
                if (clip.consumableType == ConsumableType.ENCHANT)
                    retName = retName.Replace("*", "Consumable/Enchant/" + clip.itemName);
                else if (clip.consumableType == ConsumableType.POSION)
                    retName = retName.Replace("*", "Consumable/Posion/" + clip.itemName);
                break;

            case ItemCategoryType.MATERIAL:
                if (clip.materialType == MaterialType.CRAFT)
                    retName = retName.Replace("*", "Material/Craft/" + clip.itemName);
                else if (clip.materialType == MaterialType.EXTRA)
                    retName = retName.Replace("*", "Material/Extra/" + clip.itemName);
                break;

            case ItemCategoryType.QUESTITEM:
                if (clip.questIType == QuestIType.QUEST)
                    retName = retName.Replace("*", "Quest/" + clip.itemName);
                break;
        }

        return retName.Replace('|', '"');
    }
    public BaseItemClip GetItemClip(int index)
    {
        if (index < 0 || index >= allItemClips.Length)
            return null;

        allItemClips[index].ReloadResource();
        return allItemClips[index];
    }
          
    public WeaponItemClip GetWeaponItem(int index)
    {
        UpdateSortClips();
        for (int i = 0; i < weaponItemClips.Length; i++)
        {
            if (weaponItemClips[i].id == index)
            {
                WeaponItemClip newWeaponClip = Instantiate(weaponItemClips[i]);

                newWeaponClip.ReloadResource();
                return newWeaponClip;
            }
        }
        return null;
    }

    public ArmorItemClip GetArmorItem(int index)
    {
        UpdateSortClips();
        for (int i = 0; i < armorItemClips.Length; i++)
        {
            if (armorItemClips[i].id == index)
            {
                ArmorItemClip armorClip = armorItemClips[i];
                armorClip.ReloadResource();
                return armorClip;
            }
        }
        return null;
    }

    public AccessoryItemClip GetAccessoryItem(int index)
    {
        UpdateSortClips();
        for (int i = 0; i < accessoryItemClips.Length; i++)
        {
            if (accessoryItemClips[i].id == index)
            {
                AccessoryItemClip accessoryClip = accessoryItemClips[i];
                accessoryClip.ReloadResource();
                return accessoryClip;
            }
        }
        return null;
    }

    public TitleItemClip GetTitleItem(int index)
    {
        UpdateSortClips();
        for (int i = 0; i < titleItemClips.Length; i++)
        {
            if (titleItemClips[i].id == index)
            {
                TitleItemClip titleClip = titleItemClips[i];
                titleClip.ReloadResource();
                return titleClip;
            }
        }
        return null;
    }

    public PosionItemClip GetPosionItem(int index)
    {
        UpdateSortClips();
        for (int i = 0; i < posionItemClips.Length; i++)
        {
            if (posionItemClips[i].id == index)
            {
                PosionItemClip posionClip = posionItemClips[i];
                posionClip.ReloadResource();
                return posionClip;
            }
        }
        return null;
    }

    public EnchantItemClip GetEnchantItem(int index)
    {
        UpdateSortClips();
        for (int i = 0; i < enchantItemClips.Length; i++)
        {
            if (enchantItemClips[i].id == index)
            {
                EnchantItemClip enchantClip = enchantItemClips[i];
                enchantClip.ReloadResource();
                return enchantClip;
            }
        }
        return null;
    }

    public CraftItemClip GetCraftItem(int index)
    {
        UpdateSortClips();
        for (int i = 0; i < craftItemClips.Length; i++)
        {
            if (craftItemClips[i].id == index)
            {
                CraftItemClip craftClip = craftItemClips[i];
                craftClip.ReloadResource();
                return craftClip;
            }
        }
        return null;
    }

    public ExtraItemClip GetExtraItem(int index)
    {
        UpdateSortClips();
        for (int i = 0; i < extraItemClips.Length; i++)
        {
            if(extraItemClips[i].id == index)
            {
                ExtraItemClip extraItem = extraItemClips[i];
                extraItem.ReloadResource();
                return extraItem;
            }
           
        }
        return null;
    }

    public QuestItemClip GetQuestItem(int index)
    {
        UpdateSortClips();
        for (int i = 0; i < questItemClips.Length; i++)
        {
            if(questItemClips[i].id == index)
            {
                QuestItemClip questItem = questItemClips[i];
                questItem.ReloadResource();
                return questItem;
            }
        }
        return null;
    }

    #endregion
}

