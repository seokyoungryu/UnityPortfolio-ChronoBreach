using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PotentialEditor : EditorWindow
{
  private Vector2 scrollPosition1 = Vector2.zero;
  private Vector2 scrollPosition2 = Vector2.zero;
  private static int selection = 0;
  private int realIndex = 0;
  private int lastIndex = 0;
  private bool isShowInfomation = false;
  private bool isCanRemove = false;
  private bool isFoldoutRootSplitInfo = true;
  private bool isFoldoutOpenSplitValues = false;
  private bool[] isFoldoutSplitInfo = new bool[4] {false,false,false,false};
  private bool[] isFoldoutPercentageInfo = new bool[5] { false, false, false, false, false };


    string[] rankString = new string[]
    {
      "Normal",
      "Rare",
      "Unique",
      "Legendary"
    };


    PotentialEditorCategoryType clickCategoryType = PotentialEditorCategoryType.ALL;
    
    private GameObject source;
    private static PotentialData potentialData;
    public static int moveIndexUp, moveIndexDown, targetArrayLength, currentInsertIndex, insertIndex, insertID = 0;


    [Header("Test Layout Color")]
    TestColor testColor;
    GUIStyle boxRed;
    GUIStyle boxWhite;
    GUIStyle boxBlue;
    GUIStyle boxYellow;
    GUIStyle boxGreen;
    GUIStyle boxPurple;
    GUIStyle boxBrawn;

    [MenuItem("Tools/Potential Tool")]
    static void Init()
    {
        selection = 0;
        insertIndex = 0;
        moveIndexUp = 0;
        moveIndexDown = 0;
        targetArrayLength = 0;
        currentInsertIndex = 0;
        insertID = 0;

        potentialData = ScriptableObject.CreateInstance<PotentialData>();
        potentialData.LoadData();


        PotentialEditor window = GetWindow<PotentialEditor>("Potential Tool");
        window.Show();
    }

    private void OnGUI()
    {
        if (potentialData == null)
            return;
        #region color
       // if (testColor == null)
       //     testColor = FindObjectOfType<TestColor>();
       //
       // boxRed = new GUIStyle(EditorStyles.helpBox);
       // boxWhite = new GUIStyle(EditorStyles.helpBox);
       // boxBlue = new GUIStyle(EditorStyles.helpBox);
       // boxYellow = new GUIStyle(EditorStyles.helpBox);
       // boxGreen = new GUIStyle(EditorStyles.helpBox);
       // boxPurple = new GUIStyle(EditorStyles.helpBox);
       // boxBrawn = new GUIStyle(EditorStyles.helpBox);
       //
       // boxRed.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[0]);
       // boxWhite.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[1]);
       // boxBlue.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[2]);
       // boxYellow.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[3]);
       // boxGreen.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[4]);
       // boxPurple.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[5]);
       // boxBrawn.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[6]);
        #endregion

        if (Event.current.type == EventType.MouseDown)
        {
            GUI.FocusControl(null);
        }

       // Debug.Log(this.position.width);

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal("box"); // 카테고리 contain Begin
            {
                GUILayout.FlexibleSpace(); 

                if (GUILayout.Button("All",GUILayout.Width(100), GUILayout.Height(30)))
                {
                    SetClickCategoryButtonAndUpdate(PotentialEditorCategoryType.ALL);
                }
                if (GUILayout.Button("Common", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    SetClickCategoryButtonAndUpdate(PotentialEditorCategoryType.COMMON);
                }
                if (GUILayout.Button("Weapon", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    SetClickCategoryButtonAndUpdate(PotentialEditorCategoryType.WEAPON);
                }
                if (GUILayout.Button("Armor", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    SetClickCategoryButtonAndUpdate(PotentialEditorCategoryType.ARMOR);
                }
                if (GUILayout.Button("Accessory", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    SetClickCategoryButtonAndUpdate(PotentialEditorCategoryType.ACCESSORY);
                }
                if (GUILayout.Button("Title", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    SetClickCategoryButtonAndUpdate(PotentialEditorCategoryType.Title);
                }
                GUILayout.FlexibleSpace();

            }  
            EditorGUILayout.EndHorizontal(); // 카테고리 contain End

            EditorGUILayout.BeginHorizontal("helpbox");
            EditorGUILayout.Space(0.3f);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("box");  // ADD & Remove 버튼 contain begin
            {
                try
                {
                    if (GUILayout.Button("Add", GUILayout.Width(200), GUILayout.Height(20)))
                    {
                        switch (clickCategoryType)
                        {
                            case PotentialEditorCategoryType.ALL:
                                if (potentialData != null)
                                    potentialData.AddData("NEWDATA", (int)PotentialSelectType.COMMON);

                                if (potentialData.GetDataCount(potentialData.allOptions) > 0)
                                    selection = potentialData.GetDataCount(potentialData.allOptions) - 1;

                                break;

                            case PotentialEditorCategoryType.COMMON:
                                if (potentialData != null)
                                    potentialData.AddData("NewCommonData", (int)PotentialSelectType.COMMON);

                                if (potentialData.GetDataCount(potentialData.commonOptions) > 0)
                                    selection = potentialData.GetDataCount(potentialData.commonOptions);
                                break;

                            case PotentialEditorCategoryType.WEAPON:
                                if (potentialData != null)
                                    potentialData.AddData("NewWeaponData", (int)PotentialSelectType.WEAPON);

                                if (potentialData.GetDataCount(potentialData.weaponOptions) > 0)
                                    selection = potentialData.GetDataCount(potentialData.weaponOptions);
                                break;

                            case PotentialEditorCategoryType.ARMOR:
                                if (potentialData != null)
                                    potentialData.AddData("NewArmorData", (int)PotentialSelectType.ARMOR);

                                if (potentialData.GetDataCount(potentialData.armorOptions) > 0)
                                    selection = potentialData.GetDataCount(potentialData.armorOptions);
                                break;

                            case PotentialEditorCategoryType.ACCESSORY:
                                if (potentialData != null)
                                    potentialData.AddData("NewAccessoryData", (int)PotentialSelectType.ACCESSORY);

                                if (potentialData.GetDataCount(potentialData.accessoryOptions) > 0)
                                    selection = potentialData.GetDataCount(potentialData.accessoryOptions);
                                break;

                            case PotentialEditorCategoryType.Title:
                                if (potentialData != null)
                                    potentialData.AddData("NewTitleData", (int)PotentialSelectType.Title);

                                if (potentialData.GetDataCount(potentialData.titleOptions) > 0)
                                    selection = potentialData.GetDataCount(potentialData.titleOptions);
                                break;

                        }
                        potentialData.OnConfirmButtonClicked();
                    }
                    if (GUILayout.Button("Copy", GUILayout.Width(200), GUILayout.Height(20)))
                    {
                        potentialData.Copy(realIndex);
                        potentialData.OnConfirmButtonClicked();
                        if (lastIndex != potentialData.GetOptionsClip(clickCategoryType).Length)    //lastindex에는 전프레임의 카테고리 갯수와 현재 카테로리 갯수가 다르면.
                        {
                            selection = potentialData.GetOptionsClip(clickCategoryType).Length - 1;  //현재 select을 현 카테고리 맨 마지막으로
                        }
                    }
                    if (potentialData.GetOptionsClip(clickCategoryType).Length > 0)
                    {
                        if (GUILayout.Button("Remove", GUILayout.Width(200), GUILayout.Height(20)))
                        {
                            if (potentialData != null)
                                potentialData.RemoveData(realIndex);

                            potentialData.OnConfirmButtonClicked();
                            if (lastIndex != potentialData.GetOptionsClip(clickCategoryType).Length)    //lastindex에는 전프레임의 카테고리 갯수와 현재 카테로리 갯수가 다르면.
                            {
                                selection = potentialData.GetOptionsClip(clickCategoryType).Length - 1;  //현재 select을 현 카테고리 맨 마지막으로
                            }
                        }
                    }

                    EditorGUILayout.Separator();

                    if (GUILayout.Button("Confirm", GUILayout.Width(150), GUILayout.Height(35)))
                    {
                        potentialData.OnConfirmButtonClicked();
                        if (lastIndex != potentialData.GetOptionsClip(clickCategoryType).Length)  // 이부분은 last 인덱스는 
                        {
                            selection = potentialData.GetOptionsClip(clickCategoryType).Length - 1;
                        }
                    }
                }
                catch(System.Exception e)
                {
                    Debug.Log("오류 : " + e);
                }
            }
            EditorGUILayout.EndHorizontal(); // ADD & Remove 버튼 contain End

            EditorGUILayout.BeginHorizontal("helpbox");
            EditorGUILayout.Space(0.3f);
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal();  //총 데이터 갯수 
            {
                if (clickCategoryType == PotentialEditorCategoryType.ALL)
                    Insert(potentialData.allOptions, potentialData.allOptions.Length, selection);
                if (clickCategoryType == PotentialEditorCategoryType.WEAPON)
                    Insert(potentialData.weaponOptions, potentialData.weaponOptions.Length, selection);
                if (clickCategoryType == PotentialEditorCategoryType.ARMOR)
                    Insert(potentialData.armorOptions, potentialData.armorOptions.Length, selection);
                if (clickCategoryType == PotentialEditorCategoryType.ACCESSORY)
                    Insert(potentialData.accessoryOptions, potentialData.accessoryOptions.Length, selection);
                if (clickCategoryType == PotentialEditorCategoryType.COMMON)
                    Insert(potentialData.commonOptions, potentialData.commonOptions.Length, selection);
                if (clickCategoryType == PotentialEditorCategoryType.Title)
                    Insert(potentialData.titleOptions, potentialData.titleOptions.Length, selection);

                potentialData.UpdateDataToPotentialDataList();

                EditorHelper.InsertMulti(ref potentialData.allOptions, targetArrayLength, ref selection, realIndex, moveIndexUp, moveIndexDown, ref insertIndex, insertID);

                GUILayout.FlexibleSpace();

                EditorGUILayout.LabelField("Total Data Count : " + potentialData.GetDataCount(potentialData.allOptions), EditorStyles.boldLabel);
               
                if (clickCategoryType != PotentialEditorCategoryType.ALL)
                    EditorGUILayout.LabelField($"Total {ReturnCategoryName(clickCategoryType)} Count : " + potentialData.GetDataCount(potentialData.GetOptionsClip(clickCategoryType)), EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal(); // Scroll 부분 Begin
            {
                EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(250), GUILayout.Height(755));  // 스크롤1 - Data 리스트 
                {
                    scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1, GUILayout.Width(230), GUILayout.Height(650));
                    {
                        if(potentialData.GetDataCount(potentialData.allOptions) > 0)
                        {
                            int lastSelection = 0;
                            MakeSelectionGridDataList(ref lastSelection, clickCategoryType);
                        }
                      
                    }
                    EditorGUILayout.EndScrollView();

                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("helpbox");  // 스크롤2 - info 리스트 
                {
                    if(isShowInfomation)
                    {
                        scrollPosition2 = EditorGUILayout.BeginScrollView(scrollPosition2,GUILayout.Width(630), GUILayout.Height(750));
                        {
                            if (potentialData.GetDataCount(potentialData.allOptions) > 0)
                            {
                                EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(575));
                                {
                                    EditorGUILayout.LabelField("Selection", selection.ToString(), EditorStyles.boldLabel);
                                    EditorGUILayout.LabelField("RealID", potentialData.allOptions[realIndex].id.ToString(), EditorStyles.boldLabel);

                                    potentialData.allOptions[realIndex].potentialName = EditorGUILayout.TextField("enum 이름", potentialData.allOptions[realIndex].potentialName);
                                    potentialData.allOptions[realIndex].potentialNameKorean = EditorGUILayout.TextField("한국어 이름", potentialData.allOptions[realIndex].potentialNameKorean);
                                    potentialData.allOptions[realIndex].lastWord = EditorGUILayout.TextField("마지막 단어", potentialData.allOptions[realIndex].lastWord);

                                    potentialData.allOptions[realIndex].potentialType = (PotentialSelectType)EditorGUILayout.EnumPopup("카테고리 타입.", potentialData.allOptions[realIndex].potentialType);
                                    potentialData.allOptions[realIndex].description = EditorGUILayout.TextField("설명", potentialData.allOptions[realIndex].description, GUILayout.Height(30));
                                    potentialData.allOptions[realIndex].isFloatValue = EditorGUILayout.Toggle("Is Float Tpye", potentialData.allOptions[realIndex].isFloatValue);
                                    potentialData.allOptions[realIndex].potentialFunctionObject 
                                        = (PotentialFunctionObject)EditorGUILayout.ObjectField(potentialData.allOptions[realIndex].potentialFunctionObject, typeof(PotentialFunctionObject), false);

                                    EditorGUILayout.Separator();

                                    EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(575));    // 스탯 설정 contain Begin
                                    {
                                        isFoldoutRootSplitInfo = EditorGUILayout.Foldout(isFoldoutRootSplitInfo, "스탯 설정");
                                        {
                                            if (isFoldoutRootSplitInfo)
                                            {
                                                for (int i = 0; i < potentialData.allOptions[realIndex].rankValue.Length; i++)
                                                {
                                                    if (potentialData.allOptions[realIndex].rankValue == null)
                                                    {
                                                        potentialData.allOptions[realIndex].rankValue = new PotentialRankValue[4];
                                                    }
                                                    potentialData.allOptions[realIndex].rankValue[i].rank = (ItemPotentialRankType)i;

                                                    EditorGUILayout.BeginVertical("box", GUILayout.Width(575));    // 등급설정 contain Begin
                                                    {
                                                        isFoldoutSplitInfo[i] = EditorGUILayout.Foldout(isFoldoutSplitInfo[i], $"{rankString[i]} 등급");
                                                        {
                                                            if (isFoldoutSplitInfo[i])
                                                            {
                                                                EditorGUILayout.BeginVertical("box");  //각 등급 설정 contain begin
                                                                {
                                                                    potentialData.allOptions[realIndex].rankValue[i].rank = (ItemPotentialRankType)EditorGUILayout.EnumPopup("Rank", potentialData.allOptions[realIndex].rankValue[i].rank);
                                                                    potentialData.allOptions[realIndex].rankValue[i].isSplitValue = EditorGUILayout.Toggle("Is Split Value", potentialData.allOptions[realIndex].rankValue[i].isSplitValue);

                                                                    if (potentialData.allOptions[realIndex].rankValue[i].isSplitValue == true)  // is Split Bool True
                                                                    {

                                                                        EditorGUILayout.BeginHorizontal("helpbox");      // min max 범위 
                                                                        if (potentialData.allOptions[realIndex].isFloatValue == true)
                                                                        {
                                                                            potentialData.allOptions[realIndex].rankValue[i].minValue = EditorGUILayout.FloatField("Min Value : ", potentialData.allOptions[realIndex].rankValue[i].minValue);
                                                                            EditorGUILayout.Separator();
                                                                            potentialData.allOptions[realIndex].rankValue[i].maxValue = EditorGUILayout.FloatField("Max Value : ", potentialData.allOptions[realIndex].rankValue[i].maxValue);
                                                                        }
                                                                        else
                                                                        {
                                                                            potentialData.allOptions[realIndex].rankValue[i].minValue = EditorGUILayout.IntField("Min Value : ", (int)potentialData.allOptions[realIndex].rankValue[i].minValue);
                                                                            EditorGUILayout.Separator();
                                                                            potentialData.allOptions[realIndex].rankValue[i].maxValue = EditorGUILayout.IntField("Max Value : ", (int)potentialData.allOptions[realIndex].rankValue[i].maxValue);
                                                                        }
                                                                        EditorGUILayout.EndHorizontal();
                                                                        
                                                                        potentialData.allOptions[realIndex].rankValue[i].splitCount = EditorGUILayout.IntField("Split Count : ", potentialData.allOptions[realIndex].rankValue[i].splitCount);
                                                                        
                                                                        if (potentialData.allOptions[realIndex].rankValue[i].splitCount > 5)
                                                                            potentialData.allOptions[realIndex].rankValue[i].splitCount = 5;
                                                                        else if (potentialData.allOptions[realIndex].rankValue[i].splitCount < 0)
                                                                            potentialData.allOptions[realIndex].rankValue[i].splitCount = 0;

                                                                        
                                                                        EditorGUILayout.BeginVertical("helpbox");   //밑에 percentage 부분 Begin
                                                                        {
                                                                            isFoldoutPercentageInfo[i] = EditorGUILayout.Foldout(isFoldoutPercentageInfo[i],"Percentage 설정 ( 100%까지 설정 )");
                                                                            {
                                                                                if (isFoldoutPercentageInfo[i])
                                                                                {
                                                                                    // Persentage 설정 부분
                                                                                    EditorGUILayout.BeginVertical("helpbox");
                                                                                    {
                                                                                        if (potentialData.allOptions[realIndex].rankValue[i].splitCount < 2)  //최소 split 수 설정
                                                                                            potentialData.allOptions[realIndex].rankValue[i].splitCount = 2;

                                                                                        EditorGUILayout.LabelField($"총 퍼센트: {potentialData.allOptions[realIndex].rankValue[i].totalPercentage} %", EditorStyles.boldLabel);
                                                                                        potentialData.allOptions[realIndex].rankValue[i].totalPercentage = 0;

                                                                                        for (int p = 0; p < potentialData.allOptions[realIndex].rankValue[i].splitCount; p++)
                                                                                        {
                                                                                            EditorGUILayout.BeginHorizontal();
                                                                                            {
                                                                                                potentialData.allOptions[realIndex].rankValue[i].percentage[p] = EditorGUILayout.IntField($"Split Percent[{p}] : ", potentialData.allOptions[realIndex].rankValue[i].percentage[p], GUILayout.Width(200));
                                                                                                potentialData.allOptions[realIndex].rankValue[i].totalPercentage += potentialData.allOptions[realIndex].rankValue[i].percentage[p];
                                                                                            }
                                                                                            EditorGUILayout.EndHorizontal();
                                                                                        }
                                                                                    }
                                                                                    EditorGUILayout.EndVertical();
                                                                                }
                                                                            }    
                                                                        }
                                                                        EditorGUILayout.EndVertical();     //밑에 percentage 부분 End


                                                                        EditorGUILayout.BeginVertical("helpbox");      // 분할된 범위 정보 contain Begin
                                                                        {
                                                                            isFoldoutOpenSplitValues = EditorGUILayout.Foldout(isFoldoutOpenSplitValues, "정보");
                                                                            if (isFoldoutOpenSplitValues)
                                                                            {
                                                                                EditorGUILayout.BeginVertical("helpbox");
                                                                                {
                                                                                    EditorGUILayout.LabelField("Split 범위", EditorStyles.boldLabel);
                                                                                    potentialData.allOptions[realIndex].rankValue[i].SetSplitValue();
                                                                                    for (int p = 0; p < potentialData.allOptions[realIndex].rankValue[i].splitCount; p++)
                                                                                    {
                                                                                        EditorGUILayout.BeginHorizontal("box");
                                                                                        {
                                                                                            EditorGUILayout.BeginHorizontal( GUILayout.Width(100));
                                                                                            EditorGUILayout.LabelField($"Percent[{p}] : {potentialData.allOptions[realIndex].rankValue[i].percentage[p]} %", GUILayout.Width(100));
                                                                                            EditorGUILayout.EndHorizontal();
                                                                                           
                                                                                            EditorGUILayout.BeginHorizontal("box");
                                                                                            if (potentialData.allOptions[realIndex].isFloatValue)
                                                                                                potentialData.allOptions[realIndex].rankValue[i].splitMinValue[p] = EditorGUILayout.FloatField("Min 값",potentialData.allOptions[realIndex].rankValue[i].splitMinValue[p], GUILayout.Width(200));
                                                                                            else
                                                                                                potentialData.allOptions[realIndex].rankValue[i].splitMinValue[p] = EditorGUILayout.IntField("Min 값", (int)potentialData.allOptions[realIndex].rankValue[i].splitMinValue[p], GUILayout.Width(200));


                                                                                            EditorGUILayout.EndHorizontal();

                                                                                            EditorGUILayout.BeginHorizontal("box");
                                                                                            if (potentialData.allOptions[realIndex].isFloatValue)
                                                                                                potentialData.allOptions[realIndex].rankValue[i].splitMaxValue[p] = EditorGUILayout.FloatField("Max 값", potentialData.allOptions[realIndex].rankValue[i].splitMaxValue[p], GUILayout.Width(200));
                                                                                            else
                                                                                                potentialData.allOptions[realIndex].rankValue[i].splitMaxValue[p] = EditorGUILayout.IntField("Max 값", (int)potentialData.allOptions[realIndex].rankValue[i].splitMaxValue[p], GUILayout.Width(200));

                                                                                            EditorGUILayout.EndHorizontal();
                                                                                        }
                                                                                        EditorGUILayout.EndHorizontal();
                                                                                    }
                                                                                }
                                                                                EditorGUILayout.EndVertical();
                                                                            }
                                                                        }
                                                                        EditorGUILayout.EndVertical();     // 분할된 범위 정보 contain End
                                                                    }
                                                                    else
                                                                    {
                                                                        EditorGUILayout.BeginHorizontal("helpbox");      // min max 범위 
                                                                        if (potentialData.allOptions[realIndex].isFloatValue == true)
                                                                        {
                                                                            potentialData.allOptions[realIndex].rankValue[i].minValue = EditorGUILayout.FloatField("Min Value : ", potentialData.allOptions[realIndex].rankValue[i].minValue);
                                                                            EditorGUILayout.Separator();
                                                                            potentialData.allOptions[realIndex].rankValue[i].maxValue = EditorGUILayout.FloatField("Max Value : ", potentialData.allOptions[realIndex].rankValue[i].maxValue);
                                                                        }
                                                                        else
                                                                        {
                                                                            potentialData.allOptions[realIndex].rankValue[i].minValue = EditorGUILayout.IntField("Min Value : ", (int)potentialData.allOptions[realIndex].rankValue[i].minValue);
                                                                            EditorGUILayout.Separator();
                                                                            potentialData.allOptions[realIndex].rankValue[i].maxValue = EditorGUILayout.IntField("Max Value : ", (int)potentialData.allOptions[realIndex].rankValue[i].maxValue);
                                                                        }
                                                                        EditorGUILayout.EndHorizontal();
                                                                    }

                                                                }
                                                                EditorGUILayout.EndVertical();
                                                            }
                                                        }
                                                    }
                                                    EditorGUILayout.EndVertical();        // 등급설정 contain End
                                                }
                                            }
                                        }
                                    }
                                    EditorGUILayout.EndVertical();         // 스탯 설정 End 
                                    EditorGUILayout.Separator();

                                    EditorGUILayout.BeginVertical("helpbox", GUILayout.Width(575));     // 각 등급 최소 값 최대값 정보 container BEgin
                                    {
                                        EditorGUILayout.LabelField("각 등급 범위 정보", EditorStyles.boldLabel, GUILayout.Width(300));

                                        EditorGUILayout.BeginVertical("helpbox");
                                        {
                                            for (int i = 0; i < potentialData.allOptions[realIndex].rankValue.Length; i++)
                                            {
                                                
                                                EditorGUILayout.BeginHorizontal();
                                                {
                                                    EditorGUILayout.BeginHorizontal("box");
                                                    EditorGUILayout.LabelField($"{ReturnRankName(potentialData.allOptions[realIndex].rankValue[i].rank)} 등급 : ", EditorStyles.boldLabel, GUILayout.Width(100));
                                                    EditorGUILayout.EndHorizontal();
                                                    
                                                    EditorGUILayout.BeginHorizontal("box");
                                                    EditorGUILayout.LabelField($"[ Min : {potentialData.allOptions[realIndex].rankValue[i].minValue} ]", EditorStyles.boldLabel);
                                                    EditorGUILayout.EndHorizontal();

                                                    EditorGUILayout.BeginHorizontal("box");
                                                    EditorGUILayout.LabelField($"[ Max : {potentialData.allOptions[realIndex].rankValue[i].maxValue} ]", EditorStyles.boldLabel);
                                                    EditorGUILayout.EndHorizontal();
                                                }
                                                EditorGUILayout.EndHorizontal();
                                            }
                                        }
                                        EditorGUILayout.EndVertical();
                                    }
                                    EditorGUILayout.EndVertical();      // 각 등급 최소 값 최대값 정보 container End
                                    EditorGUILayout.Separator();
                                    EditorGUILayout.Separator();
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                }
                EditorGUILayout.EndVertical();

              
            }
            EditorGUILayout.EndHorizontal(); // Scroll 부분 End
            
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Reload Setting",GUILayout.Height(30)))
                {
                    potentialData.LoadData();
                    selection = 0;
                }
                if (GUILayout.Button("Save", GUILayout.Height(30)))
                {
                    potentialData.SaveData(); 
                    potentialData.CreateEnumList();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }



    #region Function


    private void Insert(PotentialOptionClip[] array, int length, int selection)
    {
        if (array == null || length <= 0) return;

        EditorHelper.InsertSetting(array, potentialData, length, selection, ref realIndex, ref moveIndexUp, ref moveIndexDown, ref targetArrayLength, ref insertIndex, ref insertID);
    }

    private void SetClickCategoryButtonAndUpdate(PotentialEditorCategoryType categoriType)
    {
        clickCategoryType = categoriType;
        selection = 0;
        Debug.Log(categoriType);

        if (potentialData.GetDataCount(potentialData.allOptions) > 0)
            potentialData.OnConfirmButtonClicked();

    }


    /// <summary>
    /// potential enum매개변수를 받아서 해당 카테고리 데이터 이름 목록을 출력.
    /// </summary>
    private void MakeSelectionGridDataList(ref int lastSelection , PotentialEditorCategoryType type)
    {
        lastIndex = potentialData.GetOptionsClip(type).Length;    //현재 카테고리 갯수 대입. [ 5] 
        if (potentialData.GetOptionsClip(type).Length > 0)
        {
            lastSelection = selection;
            isShowInfomation = true;
            selection = GUILayout.SelectionGrid(selection, ReturnNameList(potentialData, type), 1);
            realIndex = potentialData.FindIDToIndex(potentialData.GetOptionsClip(clickCategoryType)[selection].id);
            if (lastSelection != selection)
            {
                potentialData.OnConfirmButtonClicked();
                if (lastIndex != potentialData.GetOptionsClip(type).Length)    // 만약 수정이 되어서 현재 카테고리 갯수와 다를겅우. 
                {
                    selection = potentialData.GetOptionsClip(type).Length - 1;  // 현재 카테고리의 마지막을 select에 준다.
                }
            }                                                                // 이 다음 프레임에 차피 lastindex는 현재 카테고리 갯수로 다시 업데이트됨.
        }
        else
        {
            isShowInfomation = false;
            string[] tmp = new string[1] { "Empty" };
            lastSelection = selection;
            selection = GUILayout.SelectionGrid(selection, tmp, 1, GUILayout.Width(200));
        }  //데이터 없는 경우

   
    }


    /// <summary>
    /// 매겨변수 타입의 이름 배열을 반환
    /// </summary>
    /// <returns></returns>
    public string[] ReturnNameList(PotentialData data, PotentialEditorCategoryType type)
    {
        string[] array = null;
        switch (type)
        {
            case PotentialEditorCategoryType.ALL:
                array = new string[data.allOptions.Length];
                for (int i = 0; i < data.allOptions.Length; i++)
                {
                    array[i] = data.allOptions[i].potentialName;
                }
                break;
            case PotentialEditorCategoryType.COMMON:
                array = new string[data.commonOptions.Length];
                for (int i = 0; i < data.commonOptions.Length; i++)
                {
                    array[i] = data.commonOptions[i].potentialName;
                }
                break;
            case PotentialEditorCategoryType.WEAPON:
                array = new string[data.weaponOptions.Length];
                for (int i = 0; i < data.weaponOptions.Length; i++)
                {
                    array[i] = data.weaponOptions[i].potentialName;
                }
                break;
            case PotentialEditorCategoryType.ARMOR:
                array = new string[data.armorOptions.Length];
                for (int i = 0; i < data.armorOptions.Length; i++)
                {
                    array[i] = data.armorOptions[i].potentialName;
                }
                break;
            case PotentialEditorCategoryType.ACCESSORY:
                array = new string[data.accessoryOptions.Length];
                for (int i = 0; i < data.accessoryOptions.Length; i++)
                {
                    array[i] = data.accessoryOptions[i].potentialName;
                }
                break;
            case PotentialEditorCategoryType.Title:
                array = new string[data.titleOptions.Length];
                for (int i = 0; i < data.titleOptions.Length; i++)
                {
                    array[i] = data.titleOptions[i].potentialName;
                }
                break;
        }
        return array;

    }


    private string ReturnCategoryName(PotentialEditorCategoryType categoriType)
    {
        if (categoriType == PotentialEditorCategoryType.COMMON)
            return "Common";
        else if (categoriType == PotentialEditorCategoryType.WEAPON)
            return "Weapon";
        else if (categoriType == PotentialEditorCategoryType.ARMOR)
            return "Armor";
        else if (categoriType == PotentialEditorCategoryType.ACCESSORY)
            return "Accessory";
        else if (categoriType == PotentialEditorCategoryType.Title)
            return "Title";
        else
            return "Wrong Category";

    }

    private string ReturnRankName(ItemPotentialRankType rank)
    {
        if (rank == ItemPotentialRankType.NORMAL)
            return "Normal";
        else if (rank == ItemPotentialRankType.RARE)
            return "Rare";
        else if (rank == ItemPotentialRankType.UNIQUE)
            return "Unique";
        else if (rank == ItemPotentialRankType.LEGENDARY)
            return "Legendary";
        else
            return "Wrong Category";

    }

    #endregion   //region Function
}


