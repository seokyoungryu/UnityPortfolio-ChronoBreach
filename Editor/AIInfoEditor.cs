using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class AIInfoEditor : EditorWindow
{
    private AIEditorCategoryType clickCategoryType = AIEditorCategoryType.ALL;
    private Vector2 scrollPosition1;
    private Vector2 scrollPosition2;
    private static int selection = 0;
    private int realSelection = 0;
    private int lastIndex = 0;
    private bool isFoldoutDropItem = true;
    public static int moveIndexUp, moveIndexDown, targetArrayLength, currentInsertIndex,insertIndex ,insertID = 0;

    public static AIInfoData aiData;

    [Header("Test Layout Color")]
    TestColor testColor;
    public static GUIStyle boxRed;
    public static GUIStyle boxWhite;
    public static GUIStyle boxBlue;
    public static GUIStyle boxYellow;
    public static GUIStyle boxGreen;
    public static GUIStyle boxPurple;
    public static GUIStyle boxBrawn;



    [MenuItem("Tools/Enemy Infomation Tool")]
    private static void Init()
    {
        selection = 0;
        insertIndex = 0;
        moveIndexUp = 0;
        moveIndexDown = 0;
        targetArrayLength = 0;
        currentInsertIndex = 0;
        insertID = 0;

        aiData = ScriptableObject.CreateInstance<AIInfoData>();
       aiData.LoadData();

        AIInfoEditor window = GetWindow<AIInfoEditor>();
        window.Show();
    }


    private void OnGUI()
    {

        if (aiData == null)
            return;

        if(Event.current.type == EventType.MouseDown)
        {
            GUI.FocusControl(null);
        }

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


        EditorGUILayout.BeginVertical("helpbox");
        {
            EditorGUILayout.BeginHorizontal("helpbox");  // 카테고리 container Begin
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("ALL", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    selection = 0;
                    clickCategoryType = AIEditorCategoryType.ALL;
                    aiData.UpdateSortType();
                }
                if (GUILayout.Button("Common", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    selection = 0;
                    clickCategoryType = AIEditorCategoryType.COMMON;
                    aiData.UpdateSortType();

                }
                if (GUILayout.Button("Elite", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    selection = 0;
                    realSelection = 0;
                    clickCategoryType = AIEditorCategoryType.ELITE;
                    aiData.UpdateSortType();

                }
                if (GUILayout.Button("Boss", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    selection = 0;
                    clickCategoryType = AIEditorCategoryType.BOSS;
                    aiData.UpdateSortType();

                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();    // 카테고리 container End

            #region Make Line
            EditorGUILayout.Space(0.3f);
            EditorGUILayout.BeginHorizontal("helpbox");
            {
                if (clickCategoryType == AIEditorCategoryType.ALL)
                    Insert(aiData.allEnemyClips, aiData.allEnemyClips.Length, selection);
                else if (clickCategoryType == AIEditorCategoryType.COMMON)
                    Insert(aiData.commonEnemyClip, aiData.commonEnemyClip.Length, selection);
                else if (clickCategoryType == AIEditorCategoryType.ELITE)
                    Insert(aiData.eliteEnemyClips, aiData.eliteEnemyClips.Length, selection);
                else if (clickCategoryType == AIEditorCategoryType.BOSS)
                    Insert(aiData.bossEnemyClips, aiData.bossEnemyClips.Length, selection);


                EditorHelper.InsertMulti(ref aiData.allEnemyClips, targetArrayLength, ref selection, realSelection, moveIndexUp, moveIndexDown, ref insertIndex, insertID);
                aiData.UpdateSortType();
                EditorGUILayout.LabelField("", GUILayout.Height(10));

            }
            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.BeginHorizontal("helpbox");  // Add & Copy % Remove container Begin
            {
                if (GUILayout.Button("Add", GUILayout.Width(200)))
                {
                    if (clickCategoryType == AIEditorCategoryType.ALL)
                    {
                        aiData.AddData("NewData", (int)AIType.COMMON);
                    }
                    else if (clickCategoryType == AIEditorCategoryType.COMMON)
                    {
                        aiData.AddData("NewCommonAIData", (int)AIType.COMMON);
                    }
                    else if (clickCategoryType == AIEditorCategoryType.ELITE)
                    {
                        aiData.AddData("NewEliteAIData", (int)AIType.ELITE);
                    }
                    else if(clickCategoryType == AIEditorCategoryType.BOSS)
                    {
                        aiData.AddData("NewBossAIData", (int)AIType.BOSS);
                    }

                    aiData.UpdateSortType();
                    if (lastIndex != aiData.ReturnClipArray(clickCategoryType).Length)
                    {
                        selection = aiData.ReturnClipArray(clickCategoryType).Length - 1;
                    }

                }
                if (aiData.GetDataCount(aiData.allEnemyClips) > 0)
                {
                    if (GUILayout.Button("Copy", GUILayout.Width(200)))
                    {
                        aiData.Copy(realSelection);
                        aiData.UpdateSortType();
                        if(lastIndex != aiData.ReturnClipArray(clickCategoryType).Length)
                        {
                            selection = aiData.ReturnClipArray(clickCategoryType).Length - 1;
                        }
                    }
                    if (GUILayout.Button("Remove", GUILayout.Width(200)))
                    {
                        aiData.RemoveData(realSelection);
                        aiData.UpdateSortType();
                        if (lastIndex != aiData.ReturnClipArray(clickCategoryType).Length)
                        {
                            selection = aiData.ReturnClipArray(clickCategoryType).Length - 1;
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();     // Add & Copy % Remove container End

            #region Make Line
            EditorGUILayout.Space(0.3f);
            EditorGUILayout.BeginHorizontal("helpbox");
            EditorGUILayout.LabelField("", GUILayout.Height(3));
            EditorGUILayout.EndHorizontal();
            #endregion


            EditorGUILayout.BeginVertical("helpbox");  // SelectGrid & 정보 Container Begin
            {

                EditorGUILayout.BeginHorizontal();
                {
                    int lastSelection;
                    scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1, "helpbox", GUILayout.Width(230));  // Select Grid Begin
                    {
                        lastIndex = aiData.ReturnClipArray(clickCategoryType).Length;
                        lastSelection = selection;
                        if (aiData.ReturnClipArray(clickCategoryType).Length > 0)
                        {
                            selection = GUILayout.SelectionGrid(selection, aiData.ReturnNameList(aiData.ReturnClipArray(clickCategoryType)), 1, GUILayout.Width(200));
                            realSelection = aiData.FindIDToIndex(aiData.ReturnClipArray(clickCategoryType)[selection].id);
                            if (lastSelection != selection)
                            {
                                aiData.UpdateSortType();
                                if (lastIndex != aiData.ReturnClipArray(clickCategoryType).Length)
                                {
                                    selection = aiData.ReturnClipArray(clickCategoryType).Length - 1;
                                }
                            }
                        }
                        else
                        {
                            string[] empty = new string[1] { "Empty" };
                            selection = GUILayout.SelectionGrid(selection, empty, 1, GUILayout.Width(150));
                        }

                    }
                    EditorGUILayout.EndScrollView();     // Select Grid End


                    if (aiData.GetDataCount(aiData.ReturnClipArray(clickCategoryType)) > 0)  //정보 Container Begin
                    {
                        scrollPosition2 = EditorGUILayout.BeginScrollView(scrollPosition2, "helpbox");
                        {
                            EditorGUILayout.BeginVertical("box");
                            {
                                EditorGUILayout.LabelField("Selection  " + selection);
                                EditorGUILayout.LabelField("Clip id  " + aiData.allEnemyClips[realSelection].id);
                                EditorGUILayout.Separator();
                                aiData.allEnemyClips[realSelection].aiNameEnum = EditorGUILayout.TextField("enum 이름", aiData.allEnemyClips[realSelection].aiNameEnum);
                                EditorGUILayout.Separator();
                                aiData.allEnemyClips[realSelection].aiType = (AIType)EditorGUILayout.EnumPopup("몬스터 타입",aiData.allEnemyClips[realSelection].aiType);

                                EditorHelper.GetSpcae(3);

                                EditorGUILayout.BeginVertical("helpbox");
                                {
                                    GUIStyle statsSettingGUi = new GUIStyle(EditorStyles.boldLabel);
                                    statsSettingGUi.fontSize = 14;
                                    EditorGUILayout.LabelField("AI Infomation Settings", statsSettingGUi);
                                    EditorHelper.GetSpcae(2);

                                    EditorGUILayout.BeginVertical();
                                    {
                                        if (aiData.allEnemyClips[realSelection].aiType != AIType.COMMON)
                                            aiData.allEnemyClips[realSelection].characteristicsDisplayName = EditorGUILayout.TextField("AI 고유명", aiData.allEnemyClips[realSelection].characteristicsDisplayName);

                                        EditorGUILayout.Separator();
                                        aiData.allEnemyClips[realSelection].originDisplayName = EditorGUILayout.TextField("이름", aiData.allEnemyClips[realSelection].originDisplayName);

                                        if (aiData.allEnemyClips[realSelection].aiType != AIType.COMMON)
                                        {
                                            aiData.allEnemyClips[realSelection].level = EditorGUILayout.IntField("Level ", aiData.allEnemyClips[realSelection].level, GUILayout.Width(200));
                                            EditorGUILayout.BeginHorizontal("box");
                                            {
                                                aiData.allEnemyClips[realSelection].health = EditorGUILayout.IntField("Health", aiData.allEnemyClips[realSelection].health, GUILayout.Width(300));
                                                GUILayout.FlexibleSpace();
                                                aiData.allEnemyClips[realSelection].healthBarCount = EditorGUILayout.IntField("Health Bar Count", aiData.allEnemyClips[realSelection].healthBarCount);
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }  // lv. hp, hpbar 부분
                                        else
                                        {
                                            EditorGUILayout.BeginHorizontal("box");
                                            {
                                                aiData.allEnemyClips[realSelection].level = EditorGUILayout.IntField("Level ", aiData.allEnemyClips[realSelection].level, GUILayout.Width(200));
                                                GUILayout.FlexibleSpace();
                                                aiData.allEnemyClips[realSelection].health = EditorGUILayout.IntField("Health", aiData.allEnemyClips[realSelection].health, GUILayout.Width(300));
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }

                                        EditorGUILayout.Separator();

                                        EditorGUILayout.BeginHorizontal("box"); //공격 부분
                                        {
                                            aiData.allEnemyClips[realSelection].atk = EditorGUILayout.FloatField("ATK", aiData.allEnemyClips[realSelection].atk, GUILayout.Width(200));
                                            GUILayout.FlexibleSpace();
                                            aiData.allEnemyClips[realSelection].atkSpeed = EditorGUILayout.FloatField("ATK Speed", aiData.allEnemyClips[realSelection].atkSpeed, GUILayout.Width(200));

                                        }
                                        EditorGUILayout.EndHorizontal();


                                        EditorGUILayout.BeginHorizontal("box"); //공격 부분
                                        {
                                            aiData.allEnemyClips[realSelection].minAtkPercent = EditorGUILayout.FloatField("Min ATK %", aiData.allEnemyClips[realSelection].minAtkPercent, GUILayout.Width(200));
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal("box"); //방어 부분
                                        {
                                            aiData.allEnemyClips[realSelection].defense = EditorGUILayout.FloatField("DEF", aiData.allEnemyClips[realSelection].defense, GUILayout.Width(200));
                                            GUILayout.FlexibleSpace();
                                            aiData.allEnemyClips[realSelection].magicDefense = EditorGUILayout.FloatField("Magic DEF", aiData.allEnemyClips[realSelection].magicDefense, GUILayout.Width(200));
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal("box"); //크리티컬 부분
                                        {
                                            aiData.allEnemyClips[realSelection].evasion = EditorGUILayout.FloatField("회피율 (최대 100%)", aiData.allEnemyClips[realSelection].evasion, GUILayout.Width(200));
                                            GUILayout.FlexibleSpace();
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal("box"); //크리티컬 부분
                                        {
                                            aiData.allEnemyClips[realSelection].criticalChance = EditorGUILayout.FloatField("크리티컬 확률 (최대 100%)", aiData.allEnemyClips[realSelection].criticalChance, GUILayout.Width(200));
                                            GUILayout.FlexibleSpace();
                                            aiData.allEnemyClips[realSelection].criticalDmg = EditorGUILayout.FloatField("크리티컬 배율 (1 ~ )", aiData.allEnemyClips[realSelection].criticalDmg, GUILayout.Width(200));
                                        }
                                        EditorGUILayout.EndHorizontal();
                                        EditorGUILayout.BeginHorizontal("box"); //Damage 상태 진입 무시 
                                        {
                                            aiData.allEnemyClips[realSelection].ignoreDamageState = EditorGUILayout.Toggle("데미지 State 무시", aiData.allEnemyClips[realSelection].ignoreDamageState, GUILayout.Width(200));
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginHorizontal("box"); //불사
                                        {
                                            aiData.allEnemyClips[realSelection].immortality = EditorGUILayout.Toggle("불사", aiData.allEnemyClips[realSelection].immortality, GUILayout.Width(200));
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginVertical("helpbox"); //디펜스 부분
                                        {
                                            EditorGUILayout.BeginHorizontal("box"); //체력 회복
                                            {
                                                aiData.allEnemyClips[realSelection].recycleHpReset = EditorGUILayout.Toggle("주기적으로 체력 리셋", aiData.allEnemyClips[realSelection].recycleHpReset, GUILayout.Width(200));
                                            }
                                            EditorGUILayout.EndHorizontal();
                                            
                                            if(aiData.allEnemyClips[realSelection].recycleHpReset)
                                            {
                                                EditorGUILayout.BeginHorizontal("box");
                                                {
                                                    aiData.allEnemyClips[realSelection].recycleHpResetTime = EditorGUILayout.FloatField("주기", aiData.allEnemyClips[realSelection].recycleHpResetTime, GUILayout.Width(200));
                                                }
                                                EditorGUILayout.EndHorizontal();
                                            }

                                        }
                                        EditorGUILayout.EndVertical();


                                        EditorGUILayout.BeginVertical("helpbox"); //디펜스 부분
                                        {
                                            aiData.allEnemyClips[realSelection].useDefense = EditorGUILayout.Toggle("방어 기능 사용", aiData.allEnemyClips[realSelection].useDefense, GUILayout.Width(200));
                                            if (aiData.allEnemyClips[realSelection].useDefense)
                                            {
                                                //방어 부분
                                                EditorGUILayout.BeginVertical("box");
                                                {

                                                    EditorGUILayout.BeginHorizontal();
                                                    {
                                                        EditorGUILayout.LabelField("방어 쿨타임");
                                                        aiData.allEnemyClips[realSelection].defenseCoolTime = EditorGUILayout.FloatField(aiData.allEnemyClips[realSelection].defenseCoolTime, GUILayout.Width(200));
                                                        GUILayout.FlexibleSpace();
                                                    }
                                                    EditorGUILayout.EndHorizontal();

                                                    EditorGUILayout.BeginHorizontal(); //방어 부분
                                                    {
                                                        EditorGUILayout.LabelField("방어 성공 확률 (최대 100%)");
                                                        aiData.allEnemyClips[realSelection].defensePercent = EditorGUILayout.FloatField(aiData.allEnemyClips[realSelection].defensePercent, GUILayout.Width(200));
                                                        GUILayout.FlexibleSpace();

                                                    }
                                                    EditorGUILayout.EndHorizontal();
                                                    EditorGUILayout.BeginHorizontal();
                                                    {
                                                        EditorGUILayout.LabelField("방어 타입", GUILayout.Width(100));
                                                        aiData.allEnemyClips[realSelection].defenseType = (AIDefenseType)EditorGUILayout.EnumPopup(aiData.allEnemyClips[realSelection].defenseType, GUILayout.Width(200));
                                                        GUILayout.FlexibleSpace();
                                                    }
                                                    EditorGUILayout.EndHorizontal();

                                                    if (aiData.allEnemyClips[realSelection].defenseType == AIDefenseType.COUNT)
                                                    {
                                                        EditorGUILayout.BeginHorizontal();
                                                        {
                                                            EditorGUILayout.LabelField("방어 최대 성공 횟수");
                                                            aiData.allEnemyClips[realSelection].defenseCount = EditorGUILayout.IntField( aiData.allEnemyClips[realSelection].defenseCount, GUILayout.Width(200));
                                                            GUILayout.FlexibleSpace();

                                                        }
                                                        EditorGUILayout.EndHorizontal();

                                                        EditorGUILayout.BeginHorizontal(); 
                                                        {
                                                            EditorGUILayout.LabelField("방어 최대 시간");
                                                            aiData.allEnemyClips[realSelection].defensingTime = EditorGUILayout.FloatField( aiData.allEnemyClips[realSelection].defensingTime, GUILayout.Width(200));
                                                            GUILayout.FlexibleSpace();

                                                        }
                                                        EditorGUILayout.EndHorizontal();
                                                    }
                                                    else if (aiData.allEnemyClips[realSelection].defenseType == AIDefenseType.TIME)
                                                    {
                                                        EditorGUILayout.BeginHorizontal();
                                                        {
                                                            EditorGUILayout.LabelField("방어 최대 시간");
                                                            aiData.allEnemyClips[realSelection].defensingTime = EditorGUILayout.FloatField(aiData.allEnemyClips[realSelection].defensingTime, GUILayout.Width(200));
                                                            GUILayout.FlexibleSpace();
                                                        }
                                                        EditorGUILayout.EndHorizontal();
                                                    }
                                                }
                                                EditorGUILayout.EndVertical();
                                            }
                                        }
                                        EditorGUILayout.EndVertical();



                                        EditorGUILayout.BeginVertical("helpbox"); //스탠딩 부분
                                        {
                                            aiData.allEnemyClips[realSelection].useStanding = EditorGUILayout.Toggle("스탠딩 기능 사용", aiData.allEnemyClips[realSelection].useStanding, GUILayout.Width(200));
                                            if (aiData.allEnemyClips[realSelection].useStanding)
                                            {
                                                EditorGUILayout.BeginHorizontal();
                                                {
                                                    EditorGUILayout.LabelField("스탠딩 쿨타임");
                                                    aiData.allEnemyClips[realSelection].standingCoolTime = EditorGUILayout.FloatField(aiData.allEnemyClips[realSelection].standingCoolTime, GUILayout.Width(200));
                                                    GUILayout.FlexibleSpace();
                                                }
                                                EditorGUILayout.EndHorizontal();

                                                EditorGUILayout.BeginHorizontal("box");
                                                {
                                                    EditorGUILayout.LabelField("스탠딩 성공 확률 (최대 100%)");
                                                    aiData.allEnemyClips[realSelection].standingPercent = EditorGUILayout.FloatField(aiData.allEnemyClips[realSelection].standingPercent, GUILayout.Width(300));
                                                    GUILayout.FlexibleSpace();
                                                }
                                                EditorGUILayout.EndHorizontal();
                                                EditorGUILayout.BeginHorizontal("box");
                                                {
                                                    EditorGUILayout.LabelField("스탠딩 공격 타입");
                                                    aiData.allEnemyClips[realSelection].standingType = (AIStandingType)EditorGUILayout.EnumPopup(aiData.allEnemyClips[realSelection].standingType, GUILayout.Width(300));
                                                    GUILayout.FlexibleSpace();
                                                }
                                                EditorGUILayout.EndHorizontal();

                                                EditorGUILayout.BeginHorizontal("box");
                                                {
                                                    EditorGUILayout.LabelField("스탠딩시 증가될 공격 속도");
                                                    aiData.allEnemyClips[realSelection].increaseStandingAttackSpeed = EditorGUILayout.FloatField(aiData.allEnemyClips[realSelection].increaseStandingAttackSpeed, GUILayout.Width(300));
                                                    GUILayout.FlexibleSpace();
                                                }
                                                EditorGUILayout.EndHorizontal();

                                                if (aiData.allEnemyClips[realSelection].standingType == AIStandingType.ATTACK_COUNT)
                                                {
                                                    EditorGUILayout.BeginHorizontal("box");
                                                    {
                                                        EditorGUILayout.LabelField("스탠딩 공격 횟수 ");
                                                        aiData.allEnemyClips[realSelection].standingAttackCount = EditorGUILayout.IntField(aiData.allEnemyClips[realSelection].standingAttackCount, GUILayout.Width(300));
                                                        GUILayout.FlexibleSpace();
                                                    }
                                                    EditorGUILayout.EndHorizontal();
                                                }
                                            }
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.Separator();

                                        aiData.allEnemyClips[realSelection].exp = EditorGUILayout.FloatField("EXP", aiData.allEnemyClips[realSelection].exp);
                                    }
                                    EditorGUILayout.EndVertical();


                                    EditorGUILayout.BeginVertical("helpbox"); //Drop item 부분
                                    {
                                        isFoldoutDropItem = EditorGUILayout.Foldout(isFoldoutDropItem, "Drop Items");
                                        if(isFoldoutDropItem)
                                        {
                                            EditorGUILayout.BeginVertical();
                                            {
                                                if(GUILayout.Button("Add Drop Item",GUILayout.Width(150),GUILayout.Height(30)))
                                                {
                                                    aiData.allEnemyClips[realSelection].AddDropItem();
                                                }

                                                if(aiData.allEnemyClips[realSelection].dropItems.Length > 0)
                                                {
                                                    for (int i = 0; i < aiData.allEnemyClips[realSelection].dropItems.Length; i++)
                                                    {
                                                        EditorGUILayout.Separator();

                                                        EditorGUILayout.BeginVertical("helpbox");
                                                        {
                                                            EditorGUILayout.BeginHorizontal("box");
                                                            {
                                                                aiData.allEnemyClips[realSelection].dropItems[i].isMoney = EditorGUILayout.Toggle("Is Money", aiData.allEnemyClips[realSelection].dropItems[i].isMoney);

                                                               GUILayout.FlexibleSpace();

                                                                if (GUILayout.Button("Remove", GUILayout.Width(120)))
                                                                {
                                                                    aiData.allEnemyClips[realSelection].RemoveDropItem(i);
                                                                    return;
                                                                }
                                                            }
                                                            EditorGUILayout.EndHorizontal();

                                                            //Money 일 경우
                                                            if (aiData.allEnemyClips[realSelection].dropItems[i].isMoney)
                                                            {
                                                                EditorGUILayout.Separator();

                                                                EditorGUILayout.BeginHorizontal();
                                                                {
                                                                    aiData.allEnemyClips[realSelection].dropItems[i].dropPercent
                                                                      = EditorGUILayout.FloatField("Drop Percent", aiData.allEnemyClips[realSelection].dropItems[i].dropPercent);
                                                                    EditorGUILayout.LabelField("%");
                                                                    GUILayout.FlexibleSpace();

                                                                }
                                                                EditorGUILayout.EndHorizontal();

                                                                EditorGUILayout.BeginHorizontal();
                                                                {
                                                                    aiData.allEnemyClips[realSelection].dropItems[i].minMoney
                                                                        = EditorGUILayout.IntField("Min Money", aiData.allEnemyClips[realSelection].dropItems[i].minMoney, GUILayout.Width(200));

                                                                    GUILayout.FlexibleSpace();
                                                                    aiData.allEnemyClips[realSelection].dropItems[i].maxMoney
                                                                         = EditorGUILayout.IntField("Max Money", aiData.allEnemyClips[realSelection].dropItems[i].maxMoney, GUILayout.Width(200));

                                                                }
                                                                EditorGUILayout.EndHorizontal();
                                                            }
                                                            else    // item일 경우
                                                            {
                                                                EditorGUILayout.Separator();

                                                                EditorGUILayout.BeginHorizontal();
                                                                {
                                                                    aiData.allEnemyClips[realSelection].dropItems[i].dropPercent
                                                                      = EditorGUILayout.FloatField("Drop Percent", aiData.allEnemyClips[realSelection].dropItems[i].dropPercent);
                                                                    EditorGUILayout.LabelField("%");
                                                                    GUILayout.FlexibleSpace();

                                                                }
                                                                EditorGUILayout.EndHorizontal();
                                                                aiData.allEnemyClips[realSelection].dropItems[i].itemList = (ItemList)EditorGUILayout.EnumPopup("Item List", aiData.allEnemyClips[realSelection].dropItems[i].itemList);
                                                                aiData.allEnemyClips[realSelection].dropItems[i].itemCount = EditorGUILayout.IntField("Item Count", aiData.allEnemyClips[realSelection].dropItems[i].itemCount, GUILayout.Width(300));
                                                            }

                                                        }
                                                        EditorGUILayout.EndVertical();
                                                        EditorGUILayout.Separator();
                                                    }
                                                }
                                                
                                            }
                                            EditorGUILayout.EndVertical();
                                        }

                                    }
                                    EditorGUILayout.EndVertical();
                                }
                                EditorGUILayout.EndVertical();
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndScrollView();
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal("box");
                {
                    if (GUILayout.Button("Reload",GUILayout.Height(30)))
                    {
                        aiData.LoadData();
                        selection = 0;
                    }
                    if(GUILayout.Button("Save",  GUILayout.Height(30)))
                    {
                        aiData.SaveData();
                        aiData.CreateEnumList();
                        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();   // SelectGrid & 정보 Container End

        }
        EditorGUILayout.EndVertical();

    }


    private void Insert(AIInfoClip[] array, int length, int selection)
    {
        EditorHelper.InsertSetting(array, aiData, length, selection, ref realSelection, ref moveIndexUp, ref moveIndexDown, ref targetArrayLength, ref insertIndex, ref insertID);
    }
}
