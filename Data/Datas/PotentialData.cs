using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class PotentialData : BaseData
{
    [Header("Potential Data")]
    public PotentialDataContainer potentialDatabase;
    const string enumName = "PotentialList";

    [Header("All Potential")]
    public PotentialOptionClip[] allOptions ;
    public int AllDataindex = 0;

    [Header("Common Potential")]
    public PotentialOptionClip[] commonOptions ;

    [Header("Weapon Potential")]
    public PotentialOptionClip[] weaponOptions;

    [Header("Armor Potential")]
    public PotentialOptionClip[] armorOptions ;

    [Header("Accessory Potential")]
    public PotentialOptionClip[] accessoryOptions;

    [Header("Title Potential")]
    public PotentialOptionClip[] titleOptions;

    [Header("Potential Function Database")]
    public PotentialFunctionsDatabase functionDatabase;
    public string databasePath = "Data/FinalScriptableObject/0.Database/5.PotentialDatabase/PotentialFunctionsDatabase";

    [Header("CSV Path")]
    private string csvFilePath = "";    // application.datapath + dataDirectory + csvFolderPath + csvFileName;
    private string csvFolderPath = "/CSV/";
    private string csvFileName = "PotentialData.csv";
    private string dataPath = "Data/CSV/PotentialData";


    public void LoadData()
    {
        //potentialOptions에 배열에 csv  저장 데이터를 불러오고 
        //Onselect.. 함수로 각 업데이트 하기.
        functionDatabase = Resources.Load(databasePath) as PotentialFunctionsDatabase;
        csvFilePath = Application.dataPath + dataDirectory + csvFolderPath + csvFileName;
        string loadAllText;
        TextAsset asset = Resources.Load(dataPath) as TextAsset;
        if (asset == null)
            return;
        else
            loadAllText = asset.text;


        string[] tmp1 = loadAllText.Split('\n');
        allOptions = new PotentialOptionClip[0];
        for (int a = 1; a < tmp1.Length-1; a++)
        {
            string[] line = tmp1[a].Split(',');

            int id = int.Parse(line[0]);
            string name = line[1];
            string nameKorean = line[2];
            int type = int.Parse(line[3]);
            string description = line[4];
            bool isFloatValue = bool.Parse(line[5]);
            string lastWord = line[6];
            int functionID = int.Parse(line[7]);


            // additional processing of the rest of the line
            PotentialOptionClip clip = new PotentialOptionClip();
            clip.id = id;
            clip.potentialName = name;
            clip.potentialNameKorean = nameKorean;
            clip.potentialType = (PotentialSelectType)type;
            clip.description = description;
            clip.isFloatValue = isFloatValue;
            clip.lastWord = lastWord;
            if (functionID != -1)
                clip.potentialFunctionObject = functionDatabase.GetFunctionObject_Origin(functionID);
            
            clip.rankValue = new PotentialRankValue[4]
            {
                new PotentialRankValue(),
                new PotentialRankValue(),
                new PotentialRankValue(),
                new PotentialRankValue()
            };

            for (int i = 0; i < 4; i++)  //rankValue 로드
            {
                int rankType = int.Parse(line[8 + (i * 21)]);   //  5, 26, 47, 68
                float min = float.Parse(line[9 + (i * 21)]);        //  6, 27, 48, 69
                float max = float.Parse(line[10 + (i * 21)]);        //7,28, 49, 70 
                int totalPercentage = int.Parse(line[11 + (i * 21)]); //8 , 29, 50, 71
                bool isSplitBool = bool.Parse(line[12 + (i * 21)]);  //9 ,30, 51, 72
                int splitCount = int.Parse(line[13 + (i * 21)]); //10 , 31, 52, 73

                int[] percentage = new int[5]
                {
                            int.Parse(line[14 + (i *21)]),
                            int.Parse(line[15 + (i *21)]),
                            int.Parse(line[16 + (i *21)]),
                            int.Parse(line[17 + (i *21)]),
                            int.Parse(line[18 + (i *21)])
                };

                float[] splitMin = new float[5]
                {
                          float.Parse(line[19 + (i * 21)]),
                          float.Parse(line[20 + (i * 21)]),
                          float.Parse(line[21 + (i * 21)]),
                          float.Parse(line[22 + (i *21)]),
                          float.Parse(line[23 + (i *21)])
                };

                float[] splitMax = new float[5]
               {
                          float.Parse(line[24 + (i * 21)]),
                          float.Parse(line[25 + (i * 21)]),
                          float.Parse(line[26 + (i * 21)]),
                          float.Parse(line[27 + (i *21)]),
                          float.Parse(line[28 + (i *21)])
               };


                clip.rankValue[i].rank = (ItemPotentialRankType)rankType;
                clip.rankValue[i].minValue = min;
                clip.rankValue[i].maxValue = max;
                clip.rankValue[i].totalPercentage = totalPercentage;
                clip.rankValue[i].isSplitValue = isSplitBool;
                clip.rankValue[i].splitCount = splitCount;
                clip.rankValue[i].percentage = percentage;
                clip.rankValue[i].splitMinValue = splitMin;
                clip.rankValue[i].splitMaxValue = splitMax;
            }
            allOptions = ArrayHelper.Add(clip, allOptions);
            AllDataindex = clip.id;
        }
       
        AllDataindex = AllDataindex + 1;
        OnConfirmButtonClicked();
        UpdateDataToPotentialDataList();
    }



    public void SaveData() 
    {                   
        csvFilePath = Application.dataPath + dataDirectory + csvFolderPath + csvFileName;
        using (StreamWriter sw = new StreamWriter(csvFilePath, false, Encoding.UTF8))
        {
            OnConfirmButtonClicked();

            string line = "ID, Name,KorName , Type, Description, IsFloatValue, LastWord , PotentialFunctionObject ID," +
                "Rank[0], Min[0], Max[0], TotalPercentage, IsSplitBool, SplitCount, P[0], P[1], P[2], P[3], P[4], SMin[0], SMin[1], SMin[2], SMin[3], SMin[4],  SMax[0], SMax[1], SMax[2], SMax[3], SMax[4]," +
                "Rank[1], Min[1], Max[1], TotalPercentage, IsSplitBool, SplitCount, P[0], P[1], P[2], P[3], P[4], SMin[0], SMin[1], SMin[2], SMin[3], SMin[4],  SMax[0], SMax[1], SMax[2], SMax[3], SMax[4]," +
                "Rank[2], Min[2], Max[2], TotalPercentage, IsSplitBool, SplitCount, P[0], P[1], P[2], P[3], P[4], SMin[0], SMin[1], SMin[2], SMin[3], SMin[4],  SMax[0], SMax[1], SMax[2], SMax[3], SMax[4]," +
                "Rank[3], Min[3], Max[3], TotalPercentage, IsSplitBool, SplitCount, P[0], P[1], P[2], P[3], P[4], SMin[0], SMin[1], SMin[2], SMin[3], SMin[4],  SMax[0], SMax[1], SMax[2], SMax[3], SMax[4]";  //맨 첫번째줄 
            sw.WriteLine(line);

            for (int i = 0; i < allOptions.Length; i++)
            {
                PotentialOptionClip clip = allOptions[i];
                line = clip.id.ToString() + "," +
                    clip.potentialName + "," +
                    clip.potentialNameKorean + "," +
                    ((int)clip.potentialType).ToString() + "," +
                    clip.description + "," +
                    clip.isFloatValue + "," +
                    clip.lastWord + ","+
                    (clip.potentialFunctionObject == null ? -1 : clip.potentialFunctionObject.ID);

                 for (int r = 0; r < clip.rankValue.Length; r++)
                {
                    clip.rankValue[r].SetSplitValue();
                    line = line + "," + ((int)clip.rankValue[r].rank).ToString()+ "," + clip.rankValue[r].minValue.ToString() + "," + clip.rankValue[r].maxValue.ToString()
                        + "," + clip.rankValue[r].totalPercentage.ToString() + "," + clip.rankValue[r].isSplitValue + "," + clip.rankValue[r].splitCount.ToString();

                    for (int p = 0; p < clip.rankValue[r].percentage.Length; p++)
                    {
                        line = line + "," + clip.rankValue[r].percentage[p];
                    }
                    for (int a = 0; a < clip.rankValue[r].splitMinValue.Length; a++)
                    {
                        line = line + "," + clip.rankValue[r].splitMinValue[a];
                    }
                    for (int b = 0; b < clip.rankValue[r].splitMaxValue.Length; b++)
                    {
                        line = line + "," + clip.rankValue[r].splitMaxValue[b];
                    }
                }
                sw.WriteLine(line);
            }
            
            UpdateDataToPotentialDataList();
        }
    }

    public override int FindIDToIndex(int targetID)
    {
        for (int i = 0; i < allOptions.Length; i++)
            if (allOptions[i].id == targetID)
                return i;

        return 0;
    }

    public override void AddData(string newName,int enumType = 0)
    {
        if(potentialDatabase == null)
        {
            potentialDatabase = ScriptableObject.CreateInstance<PotentialDataContainer>();
        }

        if (allOptions == null || allOptions.Length ==0)
        {
            allOptions = new PotentialOptionClip[] { new PotentialOptionClip(newName,AllDataindex) };
            AllDataindex += 1;
        }
        else
        {
            allOptions = ArrayHelper.Add(new PotentialOptionClip(newName, AllDataindex, enumType ), allOptions);
            AllDataindex += 1;
        }

       
    }

    public override void RemoveData(int index)
    {
        AllDataindex = 0;
        allOptions = ArrayHelper.Remove(index, allOptions);
        if (allOptions.Length == 0)
            allOptions = null;

        for (int i = 0; i < allOptions.Length; i++)
        {
            allOptions[i].id = AllDataindex;
            AllDataindex += 1;
        }
    }



    public override void Copy(int index)
    {
        if (index < 0 || index >= allOptions.Length)
            return ;

        PotentialOptionClip copyClip = allOptions[index];
        PotentialOptionClip temp = new PotentialOptionClip(copyClip.potentialName, AllDataindex, (int)copyClip.potentialType, copyClip);
        AllDataindex += 1;

        allOptions = ArrayHelper.Add(temp, allOptions);

    }

#if UNITY_EDITOR
    public void CreateEnumList()  // Tool 에서 선언하기.
    {
        OnConfirmButtonClicked();
      
        StringBuilder builder = new StringBuilder();
        builder.AppendLine();

        for (int i = 0; i < allOptions.Length; i++)
        {
           if(allOptions[i].potentialName != string.Empty)
            {
                builder.AppendLine("    " + GetInspectorName(allOptions[i]));
                builder.AppendLine("    " + allOptions[i].potentialName + " = " + allOptions[i].id + ",");
            }
        }

        EditorHelper.CreateEnumList(enumName, builder);
    }

#endif
    private string GetInspectorName(PotentialOptionClip clip)
    {
        string retName = "[InspectorName(|*|)]";

        switch (clip.potentialType)
        {
            case PotentialSelectType.WEAPON: retName = retName.Replace("*", "Weapon/" + clip.potentialName); break;
            case PotentialSelectType.ARMOR: retName = retName.Replace("*", "Armor/" + clip.potentialName); break;
            case PotentialSelectType.ACCESSORY: retName = retName.Replace("*", "Accessory/" + clip.potentialName); break;
            case PotentialSelectType.COMMON: retName = retName.Replace("*", "Common/" + clip.potentialName); break;
            case PotentialSelectType.Title: retName = retName.Replace("*", "Title/" + clip.potentialName); break;
        }

        return retName.Replace('|', '"');
    }

    public void UpdateDataToPotentialDataList()  // 세이브 클릭시 호출
    {
        //새로 생기면 추가

        potentialDatabase = ScriptableObject.CreateInstance<PotentialDataContainer>();
        if (potentialDatabase == null)
            Debug.Log("potentialOptions null");


        if(potentialDatabase != null)
        {
            OnConfirmButtonClicked();
            potentialDatabase.allOptions = allOptions;
            potentialDatabase.commonOptions = commonOptions;
            potentialDatabase.weaponOptions = ArrayHelper.CombineTwoArray(commonOptions, weaponOptions);
            potentialDatabase.armorOptions = ArrayHelper.CombineTwoArray(commonOptions, armorOptions);
            potentialDatabase.accessoryOptions = ArrayHelper.CombineTwoArray(commonOptions, accessoryOptions);
            potentialDatabase.titleOptions = ArrayHelper.CombineTwoArray(commonOptions, titleOptions);
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }


    }

    public void OnConfirmButtonClicked()
    {
        commonOptions = allOptions.Where(clip => clip.potentialType == PotentialSelectType.COMMON).ToArray();
        weaponOptions = allOptions.Where(clip => clip.potentialType == PotentialSelectType.WEAPON).ToArray();
        armorOptions = allOptions.Where(clip => clip.potentialType == PotentialSelectType.ARMOR).ToArray();
        accessoryOptions = allOptions.Where(clip => clip.potentialType == PotentialSelectType.ACCESSORY).ToArray();
        titleOptions = allOptions.Where(clip => clip.potentialType == PotentialSelectType.Title).ToArray();
    }

    public void OnConfirmButtonClickedSelect(ref PotentialOptionClip[] array, PotentialOptionClip[] allArray,PotentialSelectType type)
    {
         array = allArray.Where(clip => clip.potentialType == type).ToArray();
    }

    public PotentialOptionClip[] GetOptionsClip(PotentialEditorCategoryType type)
    {
        if (type == PotentialEditorCategoryType.ALL)
            return allOptions;
        else if (type == PotentialEditorCategoryType.COMMON)
            return commonOptions;
        else if (type == PotentialEditorCategoryType.WEAPON)
            return weaponOptions;
        else if (type == PotentialEditorCategoryType.ARMOR)
            return armorOptions;
        else if (type == PotentialEditorCategoryType.ACCESSORY)
            return accessoryOptions;
        else if (type == PotentialEditorCategoryType.Title)
            return titleOptions;
        else
            return null;

    }
  
}
