using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EffectEditor : EditorWindow
{
    private static EffectData effectData;

    int insertIndex = 0;
    static int selection = 0;
    private GameObject effectSource = null;

    Vector2 scrollPosition1 = Vector2.zero;
    Vector2 scrollPostition2 = Vector2.zero;

    TestColor testColor;
    GUIStyle boxRed;
    GUIStyle boxWhite;
    GUIStyle boxBlue;
    GUIStyle boxYellow;
    GUIStyle boxGreen;
    GUIStyle boxPurple;
    GUIStyle boxBrawn;



    [MenuItem("Tools/Effect Tool")]
    private static void Init()
    {
        selection = 0;
        effectData = ScriptableObject.CreateInstance<EffectData>();
        effectData.LoadData();

        EffectEditor window = GetWindow<EffectEditor>();
        window.Show();
    }


    private void OnGUI()
    {
        if (effectData == null)
            return;

        if (Event.current.type == EventType.MouseDown)
        {
            GUI.FocusControl(null);
        }

       //#region color
       //if (testColor == null)
       //    testColor = FindObjectOfType<TestColor>();
       //
       //boxRed = new GUIStyle(EditorStyles.helpBox);
       //boxWhite = new GUIStyle(EditorStyles.helpBox);
       //boxBlue = new GUIStyle(EditorStyles.helpBox);
       //boxYellow = new GUIStyle(EditorStyles.helpBox);
       //boxGreen = new GUIStyle(EditorStyles.helpBox);
       //boxPurple = new GUIStyle(EditorStyles.helpBox);
       //boxBrawn = new GUIStyle(EditorStyles.helpBox);
       //
       //boxRed.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[0]);
       //boxWhite.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[1]);
       //boxBlue.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[2]);
       //boxYellow.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[3]);
       //boxGreen.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[4]);
       //boxPurple.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[5]);
       //boxBrawn.normal.background = EditorHelper.MakeTextureColor(1, 1, testColor.colors[6]);
       //#endregion

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal("helpbox");   //ADD & COPY & REMOVE 버튼 container begin
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add", GUILayout.Width(200)))
                {
                    effectData.AddData("Test");
                    selection = effectData.realIndex -1;
                    effectSource = null;
                }
                if (GUILayout.Button("Copy", GUILayout.Width(200)))
                {
                    effectData.Copy(selection);
                    selection = effectData.realIndex - 1;
                }
                if (GUILayout.Button("Remove", GUILayout.Width(200)))
                {
                    if(effectData.effectClips.Length != 0)
                    {
                        effectData.RemoveData(selection);
                        selection = effectData.realIndex - 1;
                    }

                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();    //ADD & COPY & REMOVE 버튼 container End

            EditorGUILayout.BeginHorizontal("helpbox"); //선 
            {
                EditorGUILayout.Space(0.3f);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal("helpbox");   //총 데이터 수 라벨
            {
                EditorHelper.GUIInsertOne(ref effectData.effectClips, ref selection,ref insertIndex);

                GUILayout.FlexibleSpace();

                if (effectData != null)
                    EditorGUILayout.LabelField($"Total Effect Data Count : {effectData.GetDataCount(effectData.effectClips)}", EditorStyles.boldLabel);
                else
                    EditorGUILayout.LabelField($"Total Effect Data Count : Effect Data NULL");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("helpbox"); //선 
            {
                EditorGUILayout.Space(0.3f);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();  // 스크롤뷰 container Begin
            {
                scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1, "helpbox", GUILayout.Width(200)); //사운드 Select 스크롤뷰 container Begin
                {
                    if (effectData.GetDataCount(effectData.effectClips) > 0)
                    {
                        int lastSelect = selection;
                        selection = GUILayout.SelectionGrid(selection, effectData.ReturnClipsName(effectData.effectClips), 1, GUILayout.Width(180));
                        if (lastSelect != selection)
                        {
                        }
                    }
                    else
                    {
                        string[] tempString = new string[1] { "없음" };
                        selection = GUILayout.SelectionGrid(selection, tempString, 1, GUILayout.Width(180));
                    }
                }
                EditorGUILayout.EndScrollView();     //사운드 Select 스크롤뷰 container End

                if (effectData.GetDataCount(effectData.effectClips) > 0)
                {
                    scrollPostition2 = EditorGUILayout.BeginScrollView(scrollPostition2, "helpbox");
                    {

                        EditorGUILayout.LabelField("ID ", effectData.effectClips[selection].id.ToString());
                        EditorGUILayout.Separator();
                        effectData.effectClips[selection].effectName = EditorGUILayout.TextField("Effect Name", effectData.effectClips[selection].effectName);
                        EditorGUILayout.Separator();
                        effectData.effectClips[selection].effectType = (EffectType)EditorGUILayout.EnumPopup("Effect Type", effectData.effectClips[selection].effectType);
                        EditorGUILayout.Separator();
                        effectData.effectClips[selection].applyChildScale = EditorGUILayout.Toggle("Apply Child Scale",effectData.effectClips[selection].applyChildScale);
                        
                        if (effectData.effectClips[selection].effectPrefab == null && effectData.effectClips[selection].effectName != string.Empty)
                        {
                            effectData.effectClips[selection].LoadEffectPrefab();
                        }

                        effectData.effectClips[selection].effectPrefab = (GameObject)EditorGUILayout.ObjectField("Effect Prefab", effectData.effectClips[selection].effectPrefab, typeof(GameObject),false);
                        EditorGUILayout.Separator();

                        if (effectData.effectClips[selection].effectPrefab != null)
                        {
                            string prefabName = effectData.effectClips[selection].effectPrefab.name;
                            prefabName = prefabName.Replace(" ", "");
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(effectData.effectClips[selection].effectPrefab), prefabName);

                            effectData.effectClips[selection].effectPath = EditorHelper.GetObjectPath(effectData.effectClips[selection].effectPrefab);
                            effectData.effectClips[selection].effectName = prefabName;

                        }
                        else
                        {
                            effectData.effectClips[selection].effectPrefab = null;
                            effectData.effectClips[selection].effectPath = string.Empty;
                        }

                    }
                    EditorGUILayout.EndScrollView();
                }

            }
            EditorGUILayout.EndHorizontal();    // 스크롤뷰 container End


            EditorGUILayout.BeginHorizontal("helpbox");
            {
                if (GUILayout.Button("Reload Setting",GUILayout.Height(30), GUILayout.ExpandWidth(true)))
                {
                    effectData.LoadData();
                    selection = 0;
                }
                if (GUILayout.Button("Save", GUILayout.Height(30), GUILayout.ExpandWidth(true)))
                {
                    effectData.SaveData();
                    effectData.CreateEnum();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }

            }
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndVertical();


    }

}


