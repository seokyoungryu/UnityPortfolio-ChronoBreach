using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;

public class AIInfoData : BaseData
{
    private string enumListName = "AIInfoList";
    private int allIndex = 0;
    public AIInfoClip[] allEnemyClips = new AIInfoClip[0];
    public AIInfoClip[] commonEnemyClip = new AIInfoClip[0];
    public AIInfoClip[] eliteEnemyClips = new AIInfoClip[0];
    public AIInfoClip[] bossEnemyClips = new AIInfoClip[0];


    [Header("CSV Path")]
    private string csvFilePath = "";    // application.datapath + dataDirectory + csvFolderPath + csvFileName;
    private string csvFolderPath = "/CSV/";
    private string csvFileName = "EnemyInfoData.csv";
    private string dataPath = "Data/CSV/EnemyInfoData";


    public void LoadData()
    {
        csvFilePath = Application.dataPath + dataDirectory + csvFolderPath + csvFileName;
        TextAsset asset = Resources.Load(dataPath) as TextAsset;
        string assetText = string.Empty;
        if (asset == null)
            return;
        else
            assetText = asset.text;

        allEnemyClips = new AIInfoClip[0];

        allIndex = 0;
        int index = 0;
        string[] enterSplit = assetText.Split('\n');
        for (int i = 1; i < enterSplit.Length - 1; i++)
        {
            index = 0;
            string[] splitTab = enterSplit[i].Split(',');
            AIInfoClip enemyClip = new AIInfoClip();
            enemyClip.id = int.Parse(splitTab[index++]);
            enemyClip.aiNameEnum = splitTab[index++];
            enemyClip.characteristicsDisplayName = splitTab[index++];
            enemyClip.originDisplayName = splitTab[index++];
            enemyClip.aiType = (AIType)int.Parse(splitTab[index++]);
            enemyClip.level = int.Parse(splitTab[index++]);
            enemyClip.health = int.Parse(splitTab[index++]);
            enemyClip.healthBarCount = int.Parse(splitTab[index++]);
            enemyClip.atk = float.Parse(splitTab[index++]);
            enemyClip.minAtkPercent = float.Parse(splitTab[index++]);
            enemyClip.atkSpeed = float.Parse(splitTab[index++]);
            enemyClip.defense = float.Parse(splitTab[index++]);
            enemyClip.magicDefense = float.Parse(splitTab[index++]);
            enemyClip.evasion = float.Parse(splitTab[index++]);
            enemyClip.immortality= bool.Parse(splitTab[index++]);
            enemyClip.recycleHpReset= bool.Parse(splitTab[index++]);
            enemyClip.recycleHpResetTime= float.Parse(splitTab[index++]);


            enemyClip.exp = float.Parse(splitTab[index++]);
            enemyClip.ignoreDamageState = bool.Parse(splitTab[index++]);
            enemyClip.useDefense = bool.Parse(splitTab[index++]);
            enemyClip.defensePercent = float.Parse(splitTab[index++]);
            enemyClip.defenseType = (AIDefenseType)int.Parse(splitTab[index++]);
            enemyClip.defenseCount = int.Parse(splitTab[index++]);
            enemyClip.defensingTime = float.Parse(splitTab[index++]);
            enemyClip.defenseCoolTime = float.Parse(splitTab[index++]);  //

            enemyClip.useStanding = bool.Parse(splitTab[index++]);
            enemyClip.standingPercent = float.Parse(splitTab[index++]);
            enemyClip.standingType = (AIStandingType)int.Parse(splitTab[index++]);
            enemyClip.increaseStandingAttackSpeed = float.Parse(splitTab[index++]);
            enemyClip.standingAttackCount = int.Parse(splitTab[index++]);
            enemyClip.standingCoolTime = float.Parse(splitTab[index++]);  //

            int dropitemcount = int.Parse(splitTab[index++]);

            if (dropitemcount > 0)
            {
                for (int j = 0; j < dropitemcount; j++)
                {
                    DropItem dropitem = new DropItem();

                    dropitem.isMoney = bool.Parse(splitTab[index++]);   //13 18 23
                    dropitem.itemList = (ItemList)int.Parse(splitTab[index++]);
                    dropitem.itemCount = int.Parse(splitTab[index++]);
                    dropitem.dropPercent = float.Parse(splitTab[index++]);
                    dropitem.minMoney = int.Parse(splitTab[index++]);
                    dropitem.maxMoney = int.Parse(splitTab[index++]);
                    enemyClip.dropItems = ArrayHelper.Add(dropitem, enemyClip.dropItems);
                }
            }

            allIndex += 1;
            allEnemyClips = ArrayHelper.Add(enemyClip, allEnemyClips);
        }
        UpdateSortType();

    }

    public void SaveData()
    {
        csvFilePath = Application.dataPath + dataDirectory + csvFolderPath + csvFileName;
        using (StreamWriter sw = new StreamWriter(csvFilePath, false, Encoding.UTF8))
        {
            string line = "ID,EnumName,CharacteristicName,UIName,EnemyType,Level,Health,HealthBarCount,Atk,AtkMin% ," +
                "AtkSpeed,Defense,MagicDefense,Evasion,Immortality,RecycleHpReset,RecycleHpResetTime,EXP,IgnoreDamageState,CanDefense,Defense%,DefenseType,DefenseMaxCount,DefenseMaxTime,DefenseCoolTime," +
                "CanStanding,Standing%,StandingAttakType,StandingAtkSpeed,StandingAttackCount,StandingCoolTime,DropItemsCount,DropItems~";
            sw.WriteLine(line);

            for (int i = 0; i < allEnemyClips.Length; i++)
            {
                line = i + "," + allEnemyClips[i].aiNameEnum + "," + allEnemyClips[i].characteristicsDisplayName + "," + allEnemyClips[i].originDisplayName + "," + (int)allEnemyClips[i].aiType + "," + allEnemyClips[i].level + "," +
                    allEnemyClips[i].health + "," + allEnemyClips[i].healthBarCount + "," + allEnemyClips[i].atk + "," + allEnemyClips[i].minAtkPercent + "," + allEnemyClips[i].atkSpeed + "," +
                    allEnemyClips[i].defense + "," + allEnemyClips[i].magicDefense + "," + allEnemyClips[i].evasion + "," +
                    allEnemyClips[i].immortality + "," + allEnemyClips[i].recycleHpReset + "," + allEnemyClips[i].recycleHpResetTime + "," +
                    allEnemyClips[i].exp + "," + allEnemyClips[i].ignoreDamageState + "," + allEnemyClips[i].useDefense + ","  +
                    allEnemyClips[i].defensePercent +"," + (int)allEnemyClips[i].defenseType +","+allEnemyClips[i].defenseCount+","+allEnemyClips[i].defensingTime +"," + allEnemyClips[i].defenseCoolTime + ","   +
                    allEnemyClips[i].useStanding +"," + allEnemyClips[i].standingPercent+ ","+ (int)allEnemyClips[i].standingType+ "," +allEnemyClips[i].increaseStandingAttackSpeed +"," +
                    allEnemyClips[i].standingAttackCount +"," + allEnemyClips[i].standingCoolTime + "," + allEnemyClips[i].dropItems.Length;

                for (int j = 0; j < allEnemyClips[i].dropItems.Length; j++)
                {
                    line += "," +allEnemyClips[i].dropItems[j].isMoney + "," + (int)allEnemyClips[i].dropItems[j].itemList + "," + allEnemyClips[i].dropItems[j].itemCount + ","+ allEnemyClips[i].dropItems[j].dropPercent
                        + "," + allEnemyClips[i].dropItems[j].minMoney + "," + allEnemyClips[i].dropItems[j].maxMoney;
                }
                sw.WriteLine(line);
            }
        }


    }


    public override int FindIDToIndex(int targetID)
    {
        for (int i = 0; i < allEnemyClips.Length; i++)
            if (allEnemyClips[i].id == targetID)
                return i;

        return 0;
    }
    public override void AddData(string newName, int enumTpye)
    {
        if (allEnemyClips.Length == 0)
        {
            allIndex = 0;
            allEnemyClips = new AIInfoClip[0];
        }

        allEnemyClips = ArrayHelper.Add(new AIInfoClip(newName, allIndex, enumTpye), allEnemyClips);
        allIndex += 1;
        
    }

    public override void Copy(int index)
    {
        AIInfoClip copyClip = allEnemyClips[index];
        AIInfoClip newClip = new AIInfoClip();
        newClip.id = allIndex;
        newClip.originDisplayName = copyClip.originDisplayName;
        newClip.characteristicsDisplayName = copyClip.characteristicsDisplayName;
        newClip.aiNameEnum = copyClip.aiNameEnum;
        newClip.aiType = copyClip.aiType;
        newClip.level = copyClip.level;
        newClip.healthBarCount = copyClip.healthBarCount;
        newClip.health = copyClip.health;
        newClip.atk = copyClip.atk;
        newClip.minAtkPercent = copyClip.minAtkPercent;
        newClip.atkSpeed = copyClip.atkSpeed;
        newClip.defense = copyClip.defense;
        newClip.magicDefense = copyClip.magicDefense;
        newClip.criticalChance = copyClip.criticalChance;
        newClip.criticalDmg = copyClip.criticalDmg;
        newClip.exp = copyClip.exp;
        newClip.dropItems = copyClip.dropItems;
        newClip.immortality = copyClip.immortality;
        newClip.recycleHpReset = copyClip.recycleHpReset;
        newClip.recycleHpResetTime = copyClip.recycleHpResetTime;

        newClip.useDefense = copyClip.useDefense;
        newClip.defensePercent = copyClip.defensePercent;
        newClip.defenseType = copyClip.defenseType;
        newClip.defenseCount = copyClip.defenseCount;
        newClip.defensingTime = copyClip.defensingTime;

        newClip.useStanding = copyClip.useStanding;
        newClip.standingPercent = copyClip.standingPercent;
        newClip.standingType = copyClip.standingType;
        newClip.standingAttackCount = copyClip.standingAttackCount;
        newClip.increaseStandingAttackSpeed = copyClip.increaseStandingAttackSpeed;


        allEnemyClips = ArrayHelper.Add(newClip, allEnemyClips);
        allIndex += 1;
    }

    public override void RemoveData(int index)
    {
        allEnemyClips = ArrayHelper.Remove(index, allEnemyClips);

        allIndex = 0;
        for (int i = 0; i < allEnemyClips.Length; i++)
        {
            allEnemyClips[i].id = allIndex;
            allIndex += 1;
        }
    }


    public string[] ReturnNameList(AIInfoClip[] clips)
    {
        if (clips.Length <= 0 || clips == null)
            return null;

        string[] listNames = new string[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            listNames[i] = clips[i].aiNameEnum;
        }

        return listNames;
    }


    public AIInfoClip[] ReturnClipArray(AIEditorCategoryType type)
    {
        switch (type)
        {
            case AIEditorCategoryType.ALL:
                return allEnemyClips;
            case AIEditorCategoryType.COMMON:
                return commonEnemyClip;
            case AIEditorCategoryType.ELITE:
                return eliteEnemyClips;
            case AIEditorCategoryType.BOSS:
                return bossEnemyClips;
            default:
                return allEnemyClips;
        }

    }

    public void UpdateSortType()
    {
        commonEnemyClip = allEnemyClips.Where(i => i.aiType == AIType.COMMON).ToArray();
        eliteEnemyClips = allEnemyClips.Where(i => i.aiType == AIType.ELITE).ToArray();
        bossEnemyClips = allEnemyClips.Where(i => i.aiType == AIType.BOSS).ToArray();

    }

#if UNITY_EDITOR
    public void CreateEnumList()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();

        for (int i = 0; i < allEnemyClips.Length; i++)
        {
            sb.AppendLine("    " + GetInspectorName(allEnemyClips[i]));
            sb.AppendLine("    " + allEnemyClips[i].aiNameEnum + " = " + i + ",");
        }

        
        EditorHelper.CreateEnumList(enumListName, sb);
    }

#endif

    private string GetInspectorName(AIInfoClip clip)
    {
        string retName = "[InspectorName(|*|)]";

        switch (clip.aiType)
        {
            case AIType.COMMON: retName = retName.Replace("*", "Common/" + clip.aiNameEnum); break;
            case AIType.BOSS: retName = retName.Replace("*", "Boss/" + clip.aiNameEnum); break;
            case AIType.ELITE: retName = retName.Replace("*", "Elite/" + clip.aiNameEnum); break;
        }

        return retName.Replace('|', '"');
    }


}
