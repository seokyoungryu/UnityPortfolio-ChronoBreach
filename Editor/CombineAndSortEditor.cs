using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


public class CombineAndSortEditor : EditorWindow
{
    private static CombineAndSortEditor window;
    private static GameObjectHierarchyTree tree;
    private static SortAndCombineData data;

    private Vector2 sortObjectScroll;
    private Vector2 sortChildPartScroll;
    private Vector2 combineChildPartScroll;

    private List<GameObject> selectObject = new List<GameObject>();
    private bool savePathFoldOut = true;
    private bool editFoldOut = true;
    private bool selectObjectFoldOut = true;
    private bool exceptCombineKeywordFoldOut = true;
    private bool sortchildRootWorksFoldOut = true;
    private bool combinechildRootWorksFoldOut = true;

    [Header("Save Path")]
    private bool haveSceneSavePath = false;
    private string projectSavePath = "";
    private GameObject sceneSaveParent = null;
    private bool saveToOriginObject = false;
    private bool onlyUnderChildSort = false;
    private bool onlyTargetSort = true;

    [Header("Sort Child Works Value")]
    private List<int> sortkeywordCount = new List<int>();
    private List<string> sortchildParentName = new List<string>();
    private List<GameObject> sortchildParent = new List<GameObject>();
    private List<SortKeyWordType> sortSortKeyword = new List<SortKeyWordType>();
    private List<ChildWords> sortchildWorks = new List<ChildWords>();
    private List<bool> sortchildWorkFoldOuts = new List<bool>();
    private List<GameObject> sortchildGameObjects = new List<GameObject>();

    [Header("Combine Child Works Value")]
    private int exceptKeywordCount = 0;
    private SortKeyWordType exceptSortKeywordType = SortKeyWordType.AND;
    private ChildWords exceptChildKeyword = new ChildWords(1);
    private List<string> combinechildParentName = new List<string>();
    private List<GameObject> combinechildParent = new List<GameObject>();
    private List<bool> combinechildWorkFoldOuts = new List<bool>();
    private List<GameObject> combinechildGameObjects = new List<GameObject>();

    private Category currentCategory = Category.OBJECT_SORT;
    private SavePath currentSavePath = SavePath.SCENE;
    private float sortAndCombineScrollHeight = 250;

    [Header("LOD Options")]
    private bool addLODComponent = false;
    private float[] lodScreenTransitions = new float[3];
    private bool lodOptionFoldout = true;

    public enum Category
    {
        OBJECT_SORT = 0,
        OBJECT_COMBINE = 1,
        SORT_AND_COMBINE = 2,
    }

    public enum SavePath
    {
        SCENE = 0,
        PROJECT = 1,
    }

    TestColor testColor;
    GUIStyle boxRed;
    GUIStyle boxWhite;
    GUIStyle boxBlue;
    GUIStyle boxYellow;
    GUIStyle boxGreen;
    GUIStyle boxPurple;
    GUIStyle boxBrawn;

    [MenuItem("Tools/CombineAndSort Tool")]
    private static void Init()
    {
        data = new SortAndCombineData();

        window = CreateWindow<CombineAndSortEditor>();
        window.Show();

        tree = CreateWindow<GameObjectHierarchyTree>();
        tree.Close();
    }



    private void OnGUI()
    {
        #region Color Settings Test
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

        EditorGUILayout.BeginVertical("box");
        {
            EditorGUILayout.BeginHorizontal("helpbox");
            {
                GUILayout.FlexibleSpace();
                //카테고리 버튼
                if (GUILayout.Button("Object Sort", GUILayout.Width(200), GUILayout.Height(30)))
                {
                    if (currentCategory != Category.OBJECT_SORT)
                    {
                        currentCategory = Category.OBJECT_SORT;
                        combinechildParent.Clear();
                        combinechildParentName.Clear();
                        combinechildWorkFoldOuts.Clear();
                    }
                }
                GUILayout.Space(50);
                if (GUILayout.Button("Mesh Combine", GUILayout.Width(200), GUILayout.Height(30)))
                {
                    if (currentCategory != Category.OBJECT_COMBINE)
                    {
                        currentCategory = Category.OBJECT_COMBINE;
                        sortchildParent.Clear();
                        sortchildParentName.Clear();
                        sortchildWorkFoldOuts.Clear();
                    }
                }
                GUILayout.Space(50);
                if (GUILayout.Button("Sort And Combine", GUILayout.Width(200), GUILayout.Height(30)))
                {
                    if (currentCategory != Category.SORT_AND_COMBINE)
                    {
                        currentCategory = Category.SORT_AND_COMBINE;
                        sortchildParent.Clear();
                        sortchildParentName.Clear();
                        sortchildWorkFoldOuts.Clear();
                        combinechildParent.Clear();
                        combinechildParentName.Clear();
                        combinechildWorkFoldOuts.Clear();
                    }
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            #region Make Line
            EditorGUILayout.Space(0.3f);
            EditorGUILayout.BeginHorizontal("helpbox");
            EditorGUILayout.LabelField("", GUILayout.Height(6));
            EditorGUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(15);
            EditorGUILayout.BeginVertical("helpbox");
            {
                savePathFoldOut = EditorGUILayout.Foldout(savePathFoldOut, "최종 결과물 저장 위치");
                if (savePathFoldOut)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(20);
                        if (GUILayout.Button("Save In Scene", GUILayout.Width(150), GUILayout.Height(30)))
                        {
                            currentSavePath = SavePath.SCENE;
                        }
                        GUILayout.Space(20);
                        if (GUILayout.Button("Save In Project", GUILayout.Width(150), GUILayout.Height(30)))
                        {
                            currentSavePath = SavePath.PROJECT;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (currentSavePath == SavePath.SCENE)
                    {
                        EditorGUILayout.BeginHorizontal("box");
                        {
                            EditorGUILayout.LabelField("원본 오브젝트에서 수정", GUILayout.Width(150));
                            saveToOriginObject = EditorGUILayout.Toggle(saveToOriginObject);
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();

                        if (!saveToOriginObject)
                        {
                            EditorGUILayout.BeginHorizontal("box");
                            {
                                EditorGUILayout.LabelField("저장할 위치가 있는지", GUILayout.Width(150));
                                haveSceneSavePath = EditorGUILayout.Toggle(haveSceneSavePath);
                                if (haveSceneSavePath)
                                    sceneSaveParent
                                        = (GameObject)EditorGUILayout.ObjectField("저장할 GameObject", sceneSaveParent, typeof(GameObject), true, GUILayout.Width(300));

                                GUILayout.FlexibleSpace();

                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    else if (currentSavePath == SavePath.PROJECT)
                    {
                        EditorGUILayout.BeginHorizontal("box");
                        {
                            GUIContent content = new GUIContent("저장할 프로젝트 위치", "ex)Assets/Resources/..");
                            EditorGUILayout.LabelField(content, GUILayout.Width(150));
                            projectSavePath = EditorGUILayout.TextField("", projectSavePath, GUILayout.Width(400));
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.BeginHorizontal("helpbox");
                    {
                        GUIContent content = new GUIContent("합쳐진 메쉬 저장 위치 ");
                        EditorGUILayout.LabelField(content, GUILayout.Width(120));
                        data.combineMeshFolderPath = EditorGUILayout.TextField(data.combineMeshFolderPath, GUILayout.Width(500));
                        GUILayout.FlexibleSpace();
                                                
                        if(GUILayout.Button("경로 초기화", GUILayout.Width(100)))
                        {
                            data.combineMeshFolderPath = data.originCombineMeshFolderPath;
                        }

                    }
                    EditorGUILayout.EndHorizontal();

                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(20);

            EditorGUILayout.BeginVertical();
            {
                if (currentCategory == Category.OBJECT_SORT)
                {
                    GUI_SortCategory("분류 작업", false);
                }   //Combine 카테고리 GUI
                else if (currentCategory == Category.OBJECT_COMBINE)
                {
                    GUI_CombineCategory("분류 작업", false);
                }
                else if (currentCategory == Category.SORT_AND_COMBINE)
                {
                    GUI_SortCategory("Sort", true);
                    GUI_CombineCategory("Combine", true);
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal("box");
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("작업", GUILayout.Width(250), GUILayout.Height(30)))
                {
                    if (currentCategory == Category.OBJECT_SORT)
                    {
                        bool haveANYKeyword = data.HaveAnyKeyword(sortSortKeyword.ToArray());
                        Debug.Log("ANY HAVE : " + haveANYKeyword);

                        GameObject[] workObject = new GameObject[selectObject.Count];
                        for (int i = 0; i < selectObject.Count; i++)
                        {
                            //if (haveANYKeyword)
                            //    workObject[i] = data.SortGameObject(selectObject[i], sortchildParentName, sortchildWorks, sortSortKeyword, onlyUnderChildSort);
                            //else
                                workObject[i] = data.SortOnlyTarget(selectObject[i], sortchildParentName, sortchildWorks, sortSortKeyword, onlyUnderChildSort);

                            SaveObject(workObject);
                        }
                    }
                    else if (currentCategory == Category.OBJECT_COMBINE)
                    {
                        GameObject[] combineObject = new GameObject[selectObject.Count];

                        for (int i = 0; i < selectObject.Count; i++)
                        {
                            if(combinechildGameObjects.Count > 0)
                                combineObject[i] = data.Combine(selectObject[i], combinechildGameObjects, combinechildParentName, exceptChildKeyword, exceptSortKeywordType);
                            else
                                combineObject[i] = data.Combine(selectObject[i], null, combinechildParentName, exceptChildKeyword, exceptSortKeywordType);
                        }

                        if (addLODComponent)
                            SetLODOption(combineObject);

                        SaveObject(combineObject);
                    }
                    else if (currentCategory == Category.SORT_AND_COMBINE)
                    {
                        //sort부분
                        GameObject[] workObject = new GameObject[selectObject.Count];
                        for (int i = 0; i < selectObject.Count; i++)
                            workObject[i] = data.SortGameObject(selectObject[i], sortchildParentName, sortchildWorks, sortSortKeyword, onlyUnderChildSort);

                        GameObject[] combineObject = new GameObject[workObject.Length];
                        //Combine
                        for (int i = 0; i < workObject.Length; i++)
                        {
                            combineObject[i] = data.Combine(workObject[i], null, combinechildParentName, exceptChildKeyword, exceptSortKeywordType);
                        }

                        if (addLODComponent)
                            SetLODOption(combineObject);

                        SaveObject(workObject);
                    }

                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
          
        }
        EditorGUILayout.EndVertical();

    }


    private void GUI_SortCategory(string foldoutName, bool isSortAndCombineCategory)
    {
        editFoldOut = EditorGUILayout.Foldout(editFoldOut, foldoutName);
        if (editFoldOut)
        {
            EditorGUILayout.BeginHorizontal("helpbox");
            {
                if (GUILayout.Button("Add", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    selectObject.Add(null);
                }
                if (GUILayout.Button("Remove", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    if (selectObject.Count > 0)
                        selectObject.RemoveAt(selectObject.Count - 1);
                }
                if (GUILayout.Button("Clear", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    selectObject.Clear();
                }

                DragDropObject();
                GUILayout.FlexibleSpace();

                if(GUILayout.Button("데이터 기본 세팅", GUILayout.Width(100),GUILayout.Height(30)))
                {
                    DefaultDataSetting();
                }

                if (GUILayout.Button("Tree 보기", GUILayout.Width(100), GUILayout.Height(50)))
                {
                    tree = GetWindow<GameObjectHierarchyTree>();
                    tree.isOpen = !tree.isOpen;
                    if (tree.isOpen)
                    {
                        tree.position = new Rect(window.position.x + 810f, window.position.y + 250f, tree.position.width, tree.position.height);
                        if (selectObject.Count > 0)
                            tree.Setting(selectObject);
                        tree.Show();
                    }
                    else
                        tree.Close();
                }
            }
            EditorGUILayout.EndHorizontal();

        

            if (isSortAndCombineCategory)
            {
                LODGUI();
            }

            EditorGUILayout.BeginVertical("helpbox");
            {
                if (selectObject.Count > 0)
                {
                    sortObjectScroll = EditorGUILayout.BeginScrollView(sortObjectScroll, GUILayout.Height(100));
                    {
                        selectObjectFoldOut = EditorGUILayout.Foldout(selectObjectFoldOut, "선택된 Objects");
                        if (selectObjectFoldOut)
                        {
                            for (int i = 0; i < selectObject.Count; i++)
                            {
                                GUIContent content = new GUIContent("원본 root Object " + i, "분류할 원본 GameObject를 넣으세요");
                                selectObject[i] = (GameObject)EditorGUILayout.ObjectField(content, selectObject[i], typeof(GameObject), true, GUILayout.Width(500));
                                if (!IsInSceneObject(selectObject[i]))
                                {
                                    selectObject[i] = null;
                                }
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                else
                    EditorGUILayout.LabelField("원본 Object를 등록하십시오.");
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("helpbox");
            {
                EditorGUILayout.Space(10);
                sortchildRootWorksFoldOut = EditorGUILayout.Foldout(sortchildRootWorksFoldOut, "작업할 분류 Child Objects");
                if (sortchildRootWorksFoldOut)
                {
                    EditorGUILayout.BeginHorizontal("helpbox");
                    {
                        if (GUILayout.Button("Add", GUILayout.Width(100), GUILayout.Height(30)))
                        {
                            AddSortValues();
                        }
                        if (GUILayout.Button("Remove", GUILayout.Width(100), GUILayout.Height(30)))
                        {

                            RemoveSortValues();
                        }
                        GUILayout.Space(20);

                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Object의 바로 밑 자식들만 분류하기.", GUILayout.Width(250));
                                onlyUnderChildSort = EditorGUILayout.Toggle(onlyUnderChildSort);
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            {
                                if (onlyTargetSort)
                                    EditorGUILayout.LabelField("(현)Target만 뺴서 분류하기.", GUILayout.Width(250));
                                else
                                    EditorGUILayout.LabelField("(현)계층구조 초기화하고 분류하기", GUILayout.Width(250));

                                onlyTargetSort = EditorGUILayout.Toggle(onlyTargetSort);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(5);

                    sortChildPartScroll = EditorGUILayout.BeginScrollView(sortChildPartScroll, GUILayout.Height(!isSortAndCombineCategory ? 600 : sortAndCombineScrollHeight));
                    {
                        for (int i = 0; i < sortchildParent.Count; i++)
                        {
                            sortchildWorkFoldOuts[i] = EditorGUILayout.Foldout(sortchildWorkFoldOuts[i], "작업 " + i);
                            if (sortchildWorkFoldOuts[i])
                            {
                                EditorGUILayout.BeginVertical("helpbox");
                                {
                                    EditorGUILayout.BeginHorizontal("box");
                                    {
                                        EditorGUILayout.LabelField("새로 생성할 Parent의 이름");
                                        sortchildParentName[i] = EditorGUILayout.TextField("", sortchildParentName[i], GUILayout.Width(300));
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginVertical("helpbox");
                                    {
                                        if (sortSortKeyword[i] != SortKeyWordType.ANY)
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("KeyWord Count");
                                                sortkeywordCount[i] = EditorGUILayout.IntField(sortkeywordCount[i], GUILayout.Width(150));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }

                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField("KeyWord 분류");
                                            sortSortKeyword[i] = (SortKeyWordType)EditorGUILayout.EnumPopup(sortSortKeyword[i], GUILayout.Width(150));
                                            GUILayout.FlexibleSpace();
                                        }
                                        EditorGUILayout.EndHorizontal();

                                        EditorGUILayout.BeginVertical();
                                        {
                                            if (sortkeywordCount[i] != sortchildWorks[i].keywords.Length)
                                            {
                                                sortchildWorks[i].keywords = new string[sortkeywordCount[i]];
                                            }

                                            for (int j = 0; j < sortchildWorks[i].keywords.Length; j++)
                                            {
                                                if (sortSortKeyword[i] == SortKeyWordType.ANY)
                                                    continue;

                                                EditorGUILayout.BeginHorizontal("box");
                                                {
                                                    EditorGUILayout.LabelField("Ketword" + j);
                                                    sortchildWorks[i].keywords[j] = EditorGUILayout.TextField("", sortchildWorks[i].keywords[j]);
                                                    GUILayout.FlexibleSpace();
                                                }
                                                EditorGUILayout.EndHorizontal();
                                            }

                                        }
                                        EditorGUILayout.EndVertical();
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndVertical();


        }

    }

    private void GUI_CombineCategory(string foldoutName, bool isSortAndCombineCategory)
    {
        if (!isSortAndCombineCategory)
        {
            editFoldOut = EditorGUILayout.Foldout(editFoldOut, foldoutName);
            if (editFoldOut)
            {
                EditorGUILayout.BeginHorizontal("helpbox");
                {
                    if (GUILayout.Button("Add", GUILayout.Width(100), GUILayout.Height(30)))
                    {
                        selectObject.Add(null);
                    }
                    if (GUILayout.Button("Remove", GUILayout.Width(100), GUILayout.Height(30)))
                    {
                        if (selectObject.Count > 0)
                            selectObject.RemoveAt(selectObject.Count - 1);
                    }
                    if (GUILayout.Button("Clear", GUILayout.Width(100), GUILayout.Height(30)))
                    {
                        if (selectObject.Count > 0)
                            selectObject.Clear();
                    }

                    DragDropObject();

                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Tree 보기", GUILayout.Width(100), GUILayout.Height(50)))
                    {
                        tree = GetWindow<GameObjectHierarchyTree>();
                        tree.isOpen = !tree.isOpen;
                        if (tree.isOpen)
                        {
                            tree.position = new Rect(window.position.x + 810f, window.position.y + 250f, tree.position.width, tree.position.height);
                            if (selectObject.Count > 0)
                                tree.Setting(selectObject);
                            tree.Show();
                        }
                        else
                            tree.Close();
                    }

                }
                EditorGUILayout.EndHorizontal();
            }
        }

        if (!isSortAndCombineCategory)
            LODGUI();

        EditorGUILayout.BeginVertical("helpbox");
        {
            exceptCombineKeywordFoldOut = EditorGUILayout.Foldout(exceptCombineKeywordFoldOut, "제외할 키워드");
            if (exceptCombineKeywordFoldOut)
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    EditorGUILayout.LabelField("키워드 갯수", GUILayout.Width(100));
                    exceptKeywordCount = EditorGUILayout.IntField(exceptKeywordCount, GUILayout.Width(100));
                    if (exceptKeywordCount != exceptChildKeyword.keywords.Length)
                        exceptChildKeyword.keywords = new string[exceptKeywordCount];

                    GUILayout.Space(10);

                    EditorGUILayout.LabelField("키워드 타입", GUILayout.Width(100));
                    exceptSortKeywordType = (SortKeyWordType)EditorGUILayout.EnumPopup(exceptSortKeywordType, GUILayout.Width(100));

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();

                for (int i = 0; i < exceptKeywordCount; i++)
                {
                    EditorGUILayout.BeginHorizontal("helpbox");
                    {
                        GUIContent exceptContent = new GUIContent("제외할 키워드 " + i, "MeshFilter집합이 아닌 작업을 제외할 작업이름 키워드를 입력.");
                        EditorGUILayout.LabelField(exceptContent, GUILayout.Width(110));
                        exceptChildKeyword.keywords[i] = EditorGUILayout.TextField("", exceptChildKeyword.keywords[i], GUILayout.Width(250));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
                EditorGUILayout.LabelField("(동시 작업) 제외할 키워드");
        }
        EditorGUILayout.EndVertical();

        if (!isSortAndCombineCategory)
        {
            EditorGUILayout.BeginVertical("helpbox");
            {
                if (selectObject.Count > 0)
                {
                    selectObjectFoldOut = EditorGUILayout.Foldout(selectObjectFoldOut, "선택된 Objects");
                    if (selectObjectFoldOut)
                    {
                        GUIContent content = new GUIContent("원본 root Object", "분류할 원본 GameObject를 넣으세요");
                        for (int i = 0; i < selectObject.Count; i++)
                        {
                            selectObject[i] = (GameObject)EditorGUILayout.ObjectField(content, selectObject[i], typeof(GameObject), true, GUILayout.Width(500));
                            if (!IsInSceneObject(selectObject[i]))
                                selectObject[i] = null;
                        }
                    }
                }
                else
                    EditorGUILayout.LabelField("원본 Object를 등록하십시오.");
            }
            EditorGUILayout.EndVertical();
        }


        EditorGUILayout.BeginVertical("helpbox");
        {
            EditorGUILayout.Space(10);
            combinechildRootWorksFoldOut = EditorGUILayout.Foldout(combinechildRootWorksFoldOut, "작업할 Combine Child Objects");

            if (combinechildRootWorksFoldOut)
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    if (GUILayout.Button("Add", GUILayout.Width(100), GUILayout.Height(30)))
                    {
                        AddCombineValues();
                    }

                    if (GUILayout.Button("Auto 등록", GUILayout.Width(100), GUILayout.Height(30)))
                    {
                        if (selectObject.Count > 0)
                        {
                            combinechildParent.Clear();
                            combinechildParentName.Clear();
                            combinechildWorkFoldOuts.Clear();
                            combinechildGameObjects.Clear();
                            for (int i = 0; i < selectObject[0].transform.childCount; i++)
                            {
                                combinechildParent.Add(selectObject[0].transform.GetChild(i).gameObject);
                                combinechildWorkFoldOuts.Add(true);
                                combinechildGameObjects.Add(selectObject[0].transform.GetChild(i).gameObject);
                                combinechildParentName.Add(selectObject[0].transform.GetChild(i).name);
                                combinechildWorkFoldOuts.Add(true);
                            }
                        }
                    }
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(5);
                combineChildPartScroll = EditorGUILayout.BeginScrollView(combineChildPartScroll, GUILayout.Height(!isSortAndCombineCategory ? 400 : sortAndCombineScrollHeight));
                {
                    for (int i = 0; i < combinechildParent.Count; i++)
                    {
                        combinechildWorkFoldOuts[i] = EditorGUILayout.Foldout(combinechildWorkFoldOuts[i], "작업 " + i);
                        if (combinechildWorkFoldOuts[i])
                        {
                            EditorGUILayout.BeginVertical("helpbox");
                            {
                                EditorGUILayout.BeginHorizontal("box");
                                {
                                    if (!isSortAndCombineCategory)
                                    {
                                        GUIContent content = new GUIContent("Combine root 오브젝트 ", "");
                                        combinechildGameObjects[i]
                                            = (GameObject)EditorGUILayout.ObjectField(content, combinechildGameObjects[i], typeof(GameObject), true, GUILayout.Width(400));

                                        if (!IsInSceneObject(combinechildGameObjects[i]))
                                            combinechildGameObjects[i] = null;
                                    }
                                    else
                                        EditorGUILayout.LabelField("Sort작업 " + i + "번쨰");

                                    GUILayout.FlexibleSpace();

                                    if (GUILayout.Button("Remove", GUILayout.Width(100)))
                                    {
                                        if (combinechildParent.Count > 0)
                                        {
                                            combinechildParent.RemoveAt(i);
                                            combinechildWorkFoldOuts.RemoveAt(i);
                                            combinechildGameObjects.RemoveAt(i);
                                            combinechildParentName.RemoveAt(i);
                                            break;
                                        }
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal("box");
                                {
                                    GUIContent content = new GUIContent("Combine 이름 ", "");
                                    EditorGUILayout.LabelField(content);
                                    GUILayout.Space(-57);
                                    combinechildParentName[i] = EditorGUILayout.TextField("", combinechildParentName[i], GUILayout.Width(250));
                                    GUILayout.FlexibleSpace();
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }

                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndVertical();
    }


    private void SaveObject(GameObject[] objects)
    {
        for (int x = 0; x < objects.Length; x++)
        {
            if (!saveToOriginObject && objects[x] != null)
            {
                if (currentSavePath == SavePath.SCENE)
                    SceneSave(objects[x]);
                else if (currentSavePath == SavePath.PROJECT)
                    ProjectSave(objects[x]);

                DestroyImmediate(objects[x]);
            }
        }
    }
    private void SceneSave(GameObject go)
    {
        if (haveSceneSavePath && sceneSaveParent != null)
        {
            GameObject clone = Instantiate(go, sceneSaveParent.transform);
            clone.name = go.name;
        }
        else
            Instantiate(go).name = go.name;
    }

    private void ProjectSave(GameObject go)
    {
        if (go == null) return;

        if (!Directory.Exists(projectSavePath))
            Directory.CreateDirectory(projectSavePath);

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, projectSavePath + "/" + go.name + ".prefab");
        Debug.Log("project에 저장! {" + projectSavePath + "/" + go.name);
        EditorUtility.SetDirty(prefab);
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }

    private void DragDropObject()
    {
        Rect dropArea = GUILayoutUtility.GetRect(100, 50);
        GUI.Box(dropArea, "게임오브젝트 Drag&Drop");
        Event currentEvent = Event.current;
        //Debug.Log(currentEvent.type);
        switch (currentEvent.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(currentEvent.mousePosition))    // 마우스 포지션이 박스안에 영역인지 확인.
                    break;
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (currentEvent.type == EventType.DragPerform)   // // 드래그 후 마우스 버튼 업인 상태일때
                {
                    selectObject.Clear();
                    DragAndDrop.AcceptDrag();   // 드래그앤 드랍을 허용함.
                    foreach (UnityEngine.Object go in DragAndDrop.objectReferences)
                    {
                        if (go is GameObject)
                            selectObject.Add(go as GameObject);
                    }
                }
                break;
        }
    }

    private void LODGUI()
    {
        EditorGUILayout.BeginVertical("box");
        {
            lodOptionFoldout = EditorGUILayout.Foldout(lodOptionFoldout, "LOD Options");
            if (lodOptionFoldout)
            {
                EditorGUILayout.BeginHorizontal("helpbox");
                {
                    GUIContent content = new GUIContent("LOD 기능 추가 ");
                    EditorGUILayout.LabelField(content, GUILayout.Width(100));
                    addLODComponent = EditorGUILayout.Toggle(addLODComponent);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();

                if (addLODComponent)
                {
                    EditorGUILayout.BeginHorizontal("helpbox");
                    {
                        GUIContent content = new GUIContent($"(100%기준) LOD 0 [{100 - (lodScreenTransitions[0] * 100)}%]");
                        EditorGUILayout.LabelField(content, GUILayout.Width(150));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal("helpbox");
                    {
                        GUIContent content
                            = new GUIContent($"({(lodScreenTransitions[0] * 100)}%) LOD 1 [{(lodScreenTransitions[0] *100) - (lodScreenTransitions[1] * 100)}]", "0 ~ 1의 값 입력.");
                        EditorGUILayout.LabelField(content, GUILayout.Width(100));
                        lodScreenTransitions[0] = EditorGUILayout.FloatField(lodScreenTransitions[0], GUILayout.Width(60));
                        if (lodScreenTransitions[0] > 1 && 0 > lodScreenTransitions[0])
                            lodScreenTransitions[0] = 0;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal("helpbox");
                    {
                        GUIContent content = new GUIContent($"({(lodScreenTransitions[1] * 100)}%) LOD 2 [{(lodScreenTransitions[1] * 100) - (lodScreenTransitions[2] * 100)}]" ,"Culled 값보다 높아야함.");
                        EditorGUILayout.LabelField(content, GUILayout.Width(100));
                        lodScreenTransitions[1] = EditorGUILayout.FloatField(lodScreenTransitions[1], GUILayout.Width(60));
                        if (lodScreenTransitions[1] > 1 && 0 > lodScreenTransitions[1])
                            lodScreenTransitions[1] = 0;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal("helpbox");
                    {
                        GUIContent content = new GUIContent($"({(lodScreenTransitions[2] * 100)}%) Culled","카메라에서 사라지는 지점 입력.");
                        EditorGUILayout.LabelField(content, GUILayout.Width(100));
                        lodScreenTransitions[2] = EditorGUILayout.FloatField(lodScreenTransitions[2], GUILayout.Width(60));
                        if (lodScreenTransitions[2] > 1 && 0 > lodScreenTransitions[2])
                            lodScreenTransitions[2] = 0;
                        if (lodScreenTransitions[2] != 0 && lodScreenTransitions[1] <= lodScreenTransitions[2])
                            lodScreenTransitions[2] = lodScreenTransitions[1] - 0.01f;
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

        }
        EditorGUILayout.EndVertical();

    }
    private void SetLODOption(GameObject[] combineObject)
    {
    
        List<GameObject> child = new List<GameObject>();
        for (int i = 0; i < combineObject.Length; i++)
        {
            child.Clear();
            LODGroup lod = combineObject[i].AddComponent<LODGroup>();
            foreach (Transform tr in combineObject[i].transform)
            {
                if (tr == combineObject[i].transform) continue;

                if (combinechildParentName.Count > 0 )
                {
                    for (int x = 0; x < combinechildParentName.Count; x++)
                        if (tr.name.Contains(combinechildParentName[x]))
                            child.Add(tr.gameObject);
                }
                if(child.Count < 3)
                {
                    for (int x = 0; x < 3; x++)
                        if (tr.name.Contains("Combine_" + x.ToString()))
                            child.Add(tr.gameObject);
                }

            }
            data.SetLodComponent(lod, child, lodScreenTransitions);
        }
    }


    private bool IsInSceneObject(GameObject go)
    {
        if (go == null)
            return false;
        return go.scene.IsValid() && go.scene.isLoaded;
    }


    private void Clear()
    {
        sortchildParent.Clear();
        sortchildParentName.Clear();
        sortchildWorkFoldOuts.Clear();
        sortchildWorks.Clear();
        sortSortKeyword.Clear();
        sortkeywordCount.Clear();
    }

    private void DefaultDataSetting()
    {
        Clear();

        addLODComponent = true;
        lodOptionFoldout = true;
        lodScreenTransitions[0] = 0.6f;
        lodScreenTransitions[1] = 0.15f;
        lodScreenTransitions[2] = 0.01f;

        for (int i = 0; i < 4; i++)
            AddSortValues();


        sortchildParentName[0] = "LOD0";
        sortchildParentName[1] = "LOD1";
        sortchildParentName[2] = "LOD2";
        sortchildParentName[3] = "ANY";
        sortSortKeyword[3] = SortKeyWordType.ANY;

        sortkeywordCount[0] = 1;
        sortkeywordCount[1] = 1;
        sortkeywordCount[2] = 1;
        sortchildWorks[0].keywords[0] = "LOD0";
        sortchildWorks[1].keywords[0] = "LOD1";
        sortchildWorks[2].keywords[0] = "LOD2";


        exceptKeywordCount = 1;
        if (exceptChildKeyword.keywords.Length <= 0)
            exceptChildKeyword.keywords = new string[1];
        exceptChildKeyword.keywords[0] = "ANY";

        for (int i = 0; i < 3; i++)
            AddCombineValues();

        combinechildParentName[0] = "LOD0";
        combinechildParentName[1] = "LOD1";
        combinechildParentName[2] = "LOD2";

    }

    private void AddSortValues()
    {
        sortchildParent.Add(null);
        sortchildParentName.Add(null);
        sortchildWorkFoldOuts.Add(true);
        sortchildWorks.Add(new ChildWords(1));
        sortSortKeyword.Add(SortKeyWordType.AND);
        sortkeywordCount.Add(0);
    }


    private void RemoveSortValues()
    {
        if (sortchildParent.Count <= 0) return;

        sortchildParent.RemoveAt(sortchildParent.Count - 1);
        sortchildParentName.RemoveAt(sortchildParentName.Count - 1);
        sortchildWorkFoldOuts.RemoveAt(sortchildWorkFoldOuts.Count - 1);
        sortchildWorks.RemoveAt(sortchildWorks.Count - 1);
        sortSortKeyword.RemoveAt(sortSortKeyword.Count - 1);
        sortkeywordCount.RemoveAt(sortkeywordCount.Count - 1);
    }

    private void AddCombineValues()
    {
        combinechildParent.Add(null);
        combinechildWorkFoldOuts.Add(true);
        combinechildGameObjects.Add(null);
        combinechildParentName.Add("");
    }

    private void OnDisable()
    {
        if (tree.isOpen)
            tree.Close();
    }
}



public class GameObjectHierarchyTree : EditorWindow
{
    public bool isOpen = false;
    private int selectObjectIndex = 0;
    private List<GameObject> selectObject = new List<GameObject>();
    //private List<bool> hierarchyFoldout = new List<bool>();

    private Dictionary<GameObject, bool> hierarchyFoldout = new Dictionary<GameObject, bool>();

    bool start = false;

    private Vector2 scroll;
    public void Setting(List<GameObject> goList)
    {
        selectObjectIndex = 0;
        selectObject = goList;
        start = true;
    }

    private void OnGUI()
    {
        if (selectObject.Count <= 0) return;

        EditorGUILayout.BeginHorizontal("box");
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("<", GUILayout.Width(100), GUILayout.Height(20)))
            {
                if (selectObjectIndex > 0)
                {
                    hierarchyFoldout.Clear();
                    selectObjectIndex -= 1;
                }
            }
            GUILayout.Space(40);
            if (GUILayout.Button(">", GUILayout.Width(100), GUILayout.Height(20)))
            {
                if (selectObjectIndex < selectObject.Count - 1)
                {
                    hierarchyFoldout.Clear();
                    selectObjectIndex += 1;
                }
            }

            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        scroll = EditorGUILayout.BeginScrollView(scroll);
        {
            if (!hierarchyFoldout.ContainsKey(selectObject[selectObjectIndex]))
                hierarchyFoldout.Add(selectObject[selectObjectIndex], true);

            hierarchyFoldout[selectObject[selectObjectIndex]] = EditorGUILayout.Foldout(hierarchyFoldout[selectObject[selectObjectIndex]], selectObject[selectObjectIndex].name);
            if (hierarchyFoldout[selectObject[selectObjectIndex]])
                DrawGameObjectHierarchy(selectObject[selectObjectIndex], 0);

        }
        EditorGUILayout.EndScrollView();
    }


    private void DrawGameObjectHierarchy(GameObject go, int index)
    {
        int indexx = index + 1;

        EditorGUILayout.BeginVertical("helpbox");
        {
            GUILayout.Space(3);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    if (hierarchyFoldout[go])
                    {
                        for (int i = 0; i < go.transform.childCount; i++)
                        {
                            if (go.transform.GetChild(i).childCount > 0)
                            {
                                EditorGUILayout.BeginHorizontal();
                                if (!hierarchyFoldout.ContainsKey(go.transform.GetChild(i).gameObject))
                                    hierarchyFoldout.Add(go.transform.GetChild(i).gameObject, true);
                                EditorGUILayout.EndHorizontal();

                                hierarchyFoldout[go.transform.GetChild(i).gameObject] = EditorGUILayout.Foldout(hierarchyFoldout[go.transform.GetChild(i).gameObject], go.transform.GetChild(i).name + "(" + go.transform.GetChild(i).childCount + ")");
                                DrawGameObjectHierarchy(go.transform.GetChild(i).gameObject, indexx);
                            }
                            else
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(go.transform.GetChild(i).name);
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }

                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3);
        }
        EditorGUILayout.EndVertical();

    }


}
