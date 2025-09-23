using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SoundEditor : EditorWindow
{

    Vector2 scrollPosition1;
    Vector2 scrollPostition2;
    int insertIndex = 0;
    int selection = 0;
    private static SoundData soundData;
    TestColor testColor;
    GUIStyle boxRed;
    GUIStyle boxWhite;
    GUIStyle boxBlue;
    GUIStyle boxYellow;
    GUIStyle boxGreen;
    GUIStyle boxPurple;
    GUIStyle boxBrawn;

    private List<AudioClip> dragClips = new List<AudioClip>();
    private bool toggleDrag = false;
    private SoundPlayType dragPlayType = SoundPlayType.ETC;
    private SoundPlayTypeBGM playTypeBgm;
    private SoundPlayTypeEffect playTypeEffect;
    private SoundPlayTypeUI playTypeUI;
    private SoundPlayTypeETC playTypeETC;

    [MenuItem("Tools/Sound Toll")]
    private static void Init()
    {
        soundData = ScriptableObject.CreateInstance<SoundData>();
        soundData.LoadData();

        SoundEditor window = GetWindow<SoundEditor>();
        window.Show();
    }

    private void OnGUI()
    {
        if (soundData == null)
            return;

        if (Event.current.type == EventType.MouseDown)
        {
            GUI.FocusControl(null);
        }

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal("helpbox");   //ADD & COPY & REMOVE 버튼 container begin
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add", GUILayout.Width(200)))
                {
                    soundData.AddData("NewSoundClip");
                    selection = soundData.GetDataCount(soundData.soundClips) -1;
                }
                if (GUILayout.Button("Copy", GUILayout.Width(200)))
                {
                    soundData.Copy(selection);
                    selection = soundData.GetDataCount(soundData.soundClips) - 1;
                }
                if (GUILayout.Button("Remove", GUILayout.Width(200)))
                {
                    soundData.RemoveData(selection);
                    selection = soundData.GetDataCount(soundData.soundClips) - 1;
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();    //ADD & COPY & REMOVE 버튼 container End

            EditorGUILayout.BeginHorizontal("helpbox"); //선 
            {
                EditorGUILayout.Space(0.3f);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            {
                 EditorGUILayout.BeginHorizontal("helpbox"); //선 
                 {
                     toggleDrag = EditorGUILayout.Toggle("Register Drag AudioClips" ,toggleDrag);
                     if(toggleDrag)
                     {
                         EditorGUILayout.BeginVertical();
                         {
                            EditorGUILayout.LabelField("등록된 Audio 수 : " + dragClips.Count);
                            dragPlayType = (SoundPlayType)EditorGUILayout.EnumPopup(dragPlayType);
                            if (dragPlayType == SoundPlayType.BGM)        playTypeBgm = (SoundPlayTypeBGM)EditorGUILayout.EnumPopup("BGM Type", playTypeBgm);
                            else if (dragPlayType == SoundPlayType.Effect)playTypeEffect = (SoundPlayTypeEffect)EditorGUILayout.EnumPopup("Effect Type", playTypeEffect);
                            else if (dragPlayType == SoundPlayType.UI)    playTypeUI = (SoundPlayTypeUI)EditorGUILayout.EnumPopup("UI Type", playTypeUI);
                            else if (dragPlayType == SoundPlayType.ETC)   playTypeETC = (SoundPlayTypeETC)EditorGUILayout.EnumPopup("ETC Type",playTypeETC);
                            
                            DragDropObject();
                            if(GUILayout.Button("등록!"))
                            {
                                for (int i = 0; i < dragClips.Count; i++)
                                {
                                    SoundClip clip = new SoundClip();
                                    clip.playType = dragPlayType;
                                    clip.playTypeBgm = playTypeBgm;
                                    clip.playTypeEffect = playTypeEffect;
                                    clip.playTypeETC = playTypeETC;
                                    clip.playTypeUI = playTypeUI;
                                    clip.clip = dragClips[i];
                                    if (clip.clip != null)
                                    {
                                        string clipName = clip.clip.name;
                                        clipName = clipName.Replace(" ", "");
                                        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(clip.clip), clipName);
                                        clip.clipPath = EditorHelper.GetObjectPath(clip.clip);
                                        clip.clipName = clipName;
                                    }
                                    else
                                    {
                                        clip.clip = null;
                                        clip.clipPath = string.Empty;
                                        clip.clipFullPath = string.Empty;
                                    }

                                    soundData.AddData(clip);
                                }

                                dragClips.Clear();
                            }

                         }
                         EditorGUILayout.EndVertical();
                     }
                 }
                 EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();



            EditorGUILayout.BeginHorizontal("helpbox");   //총 데이터 수 라벨
            {

                EditorHelper.GUIInsertOne(ref soundData.soundClips,ref selection,ref insertIndex);
                GUILayout.FlexibleSpace();

                if (soundData != null)
                    EditorGUILayout.LabelField($"Total Sound Data Count : {soundData.GetDataCount(soundData.soundClips)}", EditorStyles.boldLabel);
                else
                    EditorGUILayout.LabelField($"Total Sound Data Count : Sound Data NULL");

            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("helpbox"); //선 
            {
                EditorGUILayout.Space(0.3f);
            }
            EditorGUILayout.EndHorizontal();

      



            EditorGUILayout.BeginHorizontal();  // 스크롤뷰 container Begin
            {
                scrollPosition1 = EditorGUILayout.BeginScrollView(scrollPosition1, "helpbox", GUILayout.Width(270)); //사운드 Select 스크롤뷰 container Begin
                {
                    if (soundData.GetDataCount(soundData.soundClips) > 0)
                    {
                        int lastSelect = selection;
                        selection = GUILayout.SelectionGrid(selection, soundData.ReturnSoundClipName(soundData.soundClips), 1, GUILayout.Width(250));
                        if (lastSelect != selection)
                        {
                        }
                    }
                    else
                    {
                        string[] tempString = new string[1] { "없음" };
                        selection = GUILayout.SelectionGrid(selection, tempString, 1, GUILayout.Width(250));
                    }
                }
                EditorGUILayout.EndScrollView();     //사운드 Select 스크롤뷰 container End

                if (soundData.GetDataCount(soundData.soundClips) > 0)
                {
                    scrollPostition2 = EditorGUILayout.BeginScrollView(scrollPostition2, "helpbox");
                    {
                        EditorGUILayout.LabelField("ID ", soundData.soundClips[selection].id.ToString());
                        EditorGUILayout.Separator();
                        soundData.soundClips[selection].clipName = EditorGUILayout.TextField("이름 ", soundData.soundClips[selection].clipName);
                        EditorGUILayout.Separator();
                        soundData.soundClips[selection].playType = (SoundPlayType)EditorGUILayout.EnumPopup("Play Type", soundData.soundClips[selection].playType);

                        if(soundData.soundClips[selection].playType == SoundPlayType.BGM)
                            soundData.soundClips[selection].playTypeBgm = (SoundPlayTypeBGM)EditorGUILayout.EnumPopup("BGM Type", soundData.soundClips[selection].playTypeBgm);
                        else if (soundData.soundClips[selection].playType == SoundPlayType.Effect)
                            soundData.soundClips[selection].playTypeEffect = (SoundPlayTypeEffect)EditorGUILayout.EnumPopup("Effect Type", soundData.soundClips[selection].playTypeEffect);
                        else if (soundData.soundClips[selection].playType == SoundPlayType.UI)
                            soundData.soundClips[selection].playTypeUI = (SoundPlayTypeUI)EditorGUILayout.EnumPopup("UI Type", soundData.soundClips[selection].playTypeUI);
                        else if (soundData.soundClips[selection].playType == SoundPlayType.ETC)
                            soundData.soundClips[selection].playTypeETC = (SoundPlayTypeETC)EditorGUILayout.EnumPopup("ETC Type", soundData.soundClips[selection].playTypeETC);


                        EditorGUILayout.Separator();

                        if (soundData.soundClips[selection].clip == null && soundData.soundClips[selection].clipName != string.Empty)
                        {
                            soundData.soundClips[selection].LoadClipData();
                        }
                        soundData.soundClips[selection].clip = (AudioClip)EditorGUILayout.ObjectField("사운드 Clip ", soundData.soundClips[selection].clip, typeof(AudioClip), false);
                        EditorGUILayout.Separator();
                        if (soundData.soundClips[selection].clip != null)
                        {
                            string clipName = soundData.soundClips[selection].clip.name;
                            clipName = clipName.Replace(" ", "");
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(soundData.soundClips[selection].clip), clipName);
                            soundData.soundClips[selection].clipPath = EditorHelper.GetObjectPath(soundData.soundClips[selection].clip);
                            soundData.soundClips[selection].clipName = clipName;
                        }
                        else
                        {
                            soundData.soundClips[selection].clip = null;
                            soundData.soundClips[selection].clipPath = string.Empty;
                            soundData.soundClips[selection].clipFullPath = string.Empty;
                            //soundData.soundClips[selection].clipName = string.Empty;
                        }

                        EditorGUILayout.BeginVertical("helpbox");
                        {
                            soundData.soundClips[selection].isAwakePlay = EditorGUILayout.Toggle("Is Awake Play ", soundData.soundClips[selection].isAwakePlay);
                            EditorGUILayout.Separator();
                            soundData.soundClips[selection].isLoop = EditorGUILayout.Toggle("Is Loop", soundData.soundClips[selection].isLoop);
                            EditorGUILayout.Separator();
                            soundData.soundClips[selection].pitch = EditorGUILayout.Slider("Pitch", soundData.soundClips[selection].pitch, -3.0f, 3.0f);
                            EditorGUILayout.Separator();
                            soundData.soundClips[selection].volume = EditorGUILayout.Slider("Volume", soundData.soundClips[selection].volume, 0f ,1f);
                            EditorGUILayout.Separator();
                            soundData.soundClips[selection].spatialBlend = EditorGUILayout.Slider("Spatial Blend", soundData.soundClips[selection].spatialBlend, 0, 1f);
                            EditorGUILayout.Separator();
                            soundData.soundClips[selection].minDistance = EditorGUILayout.FloatField("Min Distance", soundData.soundClips[selection].minDistance);
                            EditorGUILayout.Separator();
                            soundData.soundClips[selection].maxDistance = EditorGUILayout.FloatField("Max Distance", soundData.soundClips[selection].maxDistance);
                        }
                        EditorGUILayout.EndVertical();


                    }
                    EditorGUILayout.EndScrollView();
                }

            }
            EditorGUILayout.EndHorizontal();    // 스크롤뷰 container End


            EditorGUILayout.BeginHorizontal("helpbox");
            {
                if (GUILayout.Button("Reload Setting", GUILayout.Height(30), GUILayout.ExpandWidth(true)))
                {
                    soundData.LoadData();
                }
                if (GUILayout.Button("Save", GUILayout.Height(30), GUILayout.ExpandWidth(true)))
                {
                    soundData.SaveData();
                    soundData.CreateEnum();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }

            }
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndVertical();


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
                    dragClips.Clear();
                    DragAndDrop.AcceptDrag();   // 드래그앤 드랍을 허용함.
                    foreach (AudioClip go in DragAndDrop.objectReferences)
                    {
                        if (go is AudioClip)
                            dragClips.Add(go);
                    }
                }
                break;
        }
    }

}
