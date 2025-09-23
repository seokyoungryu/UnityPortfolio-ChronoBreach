using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class EditorHelper 
{
#if UNITY_EDITOR
    public static Texture2D MakeTextureColor(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }


    public static string GetObjectPath(UnityEngine.Object objectClip)
    {
        string retPath = string.Empty;
        string[] splitNode = AssetDatabase.GetAssetPath(objectClip).Split("/");
        bool isFindResource = false;
        for (int i = 0; i < splitNode.Length - 1; i++)
        {
            if (isFindResource == false)
            {
                if (splitNode[i] == "Resources")
                {
                    isFindResource = true;
                }
            }
            else
            {
                retPath += splitNode[i] + "/";
            }
        }
        //Debug.Log(AssetDatabase.GetAssetPath(objectClip));


        return retPath;
    }


    public static void GUIEffectRandom(ref RandomEffectInfo info, ref bool isRandom, string text)
    {
        EditorGUILayout.BeginVertical("HelpBox");

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(text + " Sound", GUILayout.Width(80));
            info.effectSound = (SoundList)EditorGUILayout.EnumPopup("", info.effectSound, GUILayout.Width(250));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(text + " Effect", GUILayout.Width(80));
            info.effect = (EffectList)EditorGUILayout.EnumPopup("", info.effect, GUILayout.Width(250));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Is Random", GUILayout.Width(100));
                isRandom = EditorGUILayout.Toggle(isRandom);
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            if (isRandom)
            {
                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.LabelField("Position", GUILayout.Width(150));
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                        info.minPosition = EditorGUILayout.Vector3Field("", info.minPosition, GUILayout.Width(180));
                        GUILayout.Label(" - ");
                        EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                        info.maxPosition = EditorGUILayout.Vector3Field("", info.maxPosition, GUILayout.Width(180));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.LabelField("Rotation", GUILayout.Width(150));
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                        info.minRotation = EditorGUILayout.Vector3Field("", info.minRotation, GUILayout.Width(180));
                        GUILayout.Label(" - ");
                        EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                        info.maxRotation = EditorGUILayout.Vector3Field("", info.maxRotation, GUILayout.Width(180));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.LabelField("Scale", GUILayout.Width(150));
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                        info.minScale = EditorGUILayout.Vector3Field("", info.minScale, GUILayout.Width(180));
                        GUILayout.Label(" - ");
                        EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                        info.maxScale = EditorGUILayout.Vector3Field("", info.maxScale, GUILayout.Width(180));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            else
            {

                info.maxPosition = Vector3.zero;
                info.maxRotation = Vector3.zero;
                info.maxScale = Vector3.zero;
                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Position", GUILayout.Width(100));
                        info.minPosition = EditorGUILayout.Vector3Field("", info.minPosition, GUILayout.Width(180));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Rotation", GUILayout.Width(100));
                        info.minRotation = EditorGUILayout.Vector3Field("", info.minRotation, GUILayout.Width(180));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Scale", GUILayout.Width(100));
                        info.minScale = EditorGUILayout.Vector3Field("", info.minScale, GUILayout.Width(180));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }

        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
    }

#endif

#if UNITY_EDITOR
    public static void GetSpcae(int count)
    {
        for(int i =0; i<count; i++)
        {
            EditorGUILayout.Space();
        }
    }

    public static void CreateEnumList(string enumName, StringBuilder data)
    {
        string getTemplateFilePath = "Assets/10.Resources/Resources/Data/EnumTemplateFolder/EnumTemplate.txt";
        string templateTextData = File.ReadAllText(getTemplateFilePath);

        templateTextData = templateTextData.Replace("$ENUM$", enumName);
        templateTextData = templateTextData.Replace("$DATA$", data.ToString());
        
        string createFolderPath = "Assets/1.Scripts/Data/Datas/EnumList/";
        string filePath = createFolderPath + enumName + ".cs";

        if(Directory.Exists(createFolderPath)== false)
        {
            Directory.CreateDirectory(createFolderPath);
        }

        if(File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        File.WriteAllText(filePath, templateTextData);
        Debug.Log("만들어짐 : " + filePath);

    }

  


    public static void GUIInsertOne<T>(ref T[] array, ref int selection, ref int insertIndex)
    {
        if (GUILayout.Button("↑", GUILayout.Width(75)))
        {
            if ((selection - 1) > -1)
            {
                array = ArrayHelper.Insert(array, selection, selection - 1);
                selection -= 1;
            }
        }
        if (GUILayout.Button("↓", GUILayout.Width(75)))
        {
            if ((selection + 1) < array.Length)
            {
                array = ArrayHelper.Insert(array, selection, selection + 1);
                selection += 1;
            }
        }
        GUILayout.Space(5);

        EditorGUILayout.LabelField("Insert :", GUILayout.Width(45));
        insertIndex = EditorGUILayout.IntField(insertIndex, GUILayout.Width(55));

        if (GUILayout.Button("확인", GUILayout.Width(75)))
        {
            if (insertIndex > array.Length)
                insertIndex = array.Length - 1;
            else if (insertIndex < 0)
                insertIndex = 0;

            if (insertIndex < array.Length || insertIndex > -1)
            {
                array = ArrayHelper.Insert(array, selection, insertIndex);
                selection = insertIndex;
            }
        }
    }


    public static void InsertMulti<T>(ref T[] array, int targetArrayLength, ref int selection, int curIndex, int moveUpIndex, int moveDownIndex, ref int insertIndex, int insertRealIndex)
    {
        MoveUP(ref array, targetArrayLength, ref selection, curIndex, moveUpIndex);
        MoveDown(ref array, targetArrayLength, ref selection, curIndex, moveDownIndex);
        Insert(ref array, targetArrayLength, ref selection, curIndex, ref insertIndex, insertRealIndex);
    }




    public static void MoveUP<T>(ref T[] array, int targetArrayLength, ref int selection,int curIndex, int moveIndex )
    {
        if (GUILayout.Button("↑", GUILayout.Width(75)))
        {
            if (selection > -1 && moveIndex > -1 && targetArrayLength > 0)
            {
                array = ArrayHelper.Insert(array, curIndex, moveIndex);
                selection -= 1;
                if (selection < 0)
                    selection = 0;
            }
        }
    }

    public static void MoveDown<T>(ref T[] array, int targetArrayLength, ref int selection, int curIndex, int moveIndex)
    {
        if (GUILayout.Button("↓", GUILayout.Width(75)))
        {
            if ((selection + 1) < array.Length)
            {
                array = ArrayHelper.Insert(array, curIndex, moveIndex );
                selection += 1;
                if (selection >= targetArrayLength)
                    selection = targetArrayLength - 1;
            }
        }
    }

    public static void Insert<T>(ref T[] array , int targetArrayLength, ref int selection, int curIndex,ref  int moveIndex, int realIndex)
    {
        EditorGUILayout.LabelField("Insert :", GUILayout.Width(45));
        moveIndex = EditorGUILayout.IntField(moveIndex, GUILayout.Width(55));
        if(moveIndex > targetArrayLength)
        {
            moveIndex = targetArrayLength - 1;
            Debug.Log("1");
        }

        if (GUILayout.Button("확인", GUILayout.Width(75)))
        {
            if (moveIndex < array.Length || moveIndex > -1)
            {
                Debug.Log($"Insert!   현 {curIndex}  이동 {moveIndex}");
                array = ArrayHelper.Insert(array, curIndex, realIndex);
                selection = moveIndex;
                if (selection < 0)
                    selection = 0;
                if (selection >= targetArrayLength)
                    selection = targetArrayLength - 1;
            }
        }
    }

     public static void InsertSetting<T>(T[] array, BaseData data , int arrayLength, int selection,ref int curRealIndex , ref int indexUp, ref int indexDown, ref int targetlegnth, ref int insertIndex,ref  int insertID) where T : BaseEditorClip
     {
         if ((selection - 1) > -1) indexUp = data.FindIDToIndex(array[selection - 1].id);
         if ((selection + 1) < array.Length) indexDown = data.FindIDToIndex(array[selection + 1].id);
        targetlegnth = arrayLength;
         if (array.Length > 0)
            curRealIndex = data.FindIDToIndex(array[selection].id);
         if ((selection + 1) < array.Length && (selection - 1) > -1)
             insertID = data.FindIDToIndex(array[insertIndex].id);
     }

#endif
}
