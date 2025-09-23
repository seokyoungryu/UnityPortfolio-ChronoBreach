using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttackSkillClip))]
public class AttackSkillGUI : Editor
{
    private AttackSkillClip clip = null;
    private GUIStyle dataStyle = new GUIStyle();
    private GUIStyle attackEffectFoldStyle = new GUIStyle();
    private List<bool> foldOutVariables = new List<bool>();

    private List<bool> foldOutAttackEffects = new List<bool>();
    private List<bool> foldOutHitEffects = new List<bool>();
    private bool foldOutAllEffects = false;
    private bool foldOutAllHitEffect = false;
    private bool foldOutAllAttackEffect = false;


    public Animator animator;
    private void OnEnable()
    {
        foldOutVariables = new List<bool>();
    }
    public override void OnInspectorGUI()
    {
        clip = (AttackSkillClip)target;
        base.OnInspectorGUI();

        if (clip == null) return;

        if (foldOutVariables == null)
            foldOutVariables = new List<bool>();

        if (foldOutAttackEffects == null)
            foldOutAttackEffects = new List<bool>(); 
        if (foldOutHitEffects == null)
            foldOutHitEffects = new List<bool>();


        if (clip.AttackDamage != null && clip.AttackDamage.Count > 0)
        {
            for (int i = 0; i < clip.AttackDamage.Count; i++)
            {
                if (foldOutVariables.Count != clip.AttackDamage.Count)
                    foldOutVariables.Add(false);
                if (foldOutAttackEffects.Count != clip.AttackDamage.Count)
                    foldOutAttackEffects.Add(false);
                if (foldOutHitEffects.Count != clip.AttackDamage.Count)
                    foldOutHitEffects.Add(false);
            }
        }

        if (clip.TimeDatas.Count != clip.AttackDamage.Count)
        {
            for (int i = 0; i < clip.AttackDamage.Count; i++)
            {
                clip.TimeDatas.Add(null);
                if (clip.TimeDatas.Count == clip.AttackDamage.Count)
                    break;
            }
        }


        GUILayout.Space(20);

        GUILayout.BeginVertical("HelpBox");
        {
            dataStyle.fontStyle= FontStyle.Bold;
            dataStyle.normal.textColor = Color.white;
            dataStyle.fontSize = 15;
            EditorGUILayout.LabelField("기본 공격 데이터", dataStyle);


            GUILayout.BeginVertical("HelpBox");
            {
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("애니메이션 속도", GUILayout.Width(115));
                    clip.SkillAnimSpeed = EditorGUILayout.FloatField(clip.SkillAnimSpeed);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("애니메이션 클립", GUILayout.Width(115));
                    clip.AnimationClip = (AnimationClip)EditorGUILayout.ObjectField(clip.AnimationClip,typeof(AnimationClip), false);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("애니메이터 상태 이름", GUILayout.Width(115));
                    clip.AnimationClipName = EditorGUILayout.TextField(clip.AnimationClipName, GUILayout.Width(100));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    GUI.enabled = false;
                    if (clip.AnimationClip != null)
                        clip.ClipFullFrame = clip.AnimationClip.length * 30f;
                    EditorGUILayout.LabelField("클립 Full Frame :", GUILayout.Width(115));
                    EditorGUILayout.IntField((int)clip.ClipFullFrame);
                    GUILayout.FlexibleSpace();
                    GUI.enabled = true;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("종료 시점 프레임:", GUILayout.Width(115));
                   clip.AnimationEndFrame =  EditorGUILayout.IntField(clip.AnimationEndFrame);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();



            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("(AI용)Detect Range", GUILayout.Width(200));
                clip.TargetDetectRange = EditorGUILayout.FloatField(clip.TargetDetectRange);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("추가"))
                {
                    foldOutAttackEffects.Add(false);
                    foldOutHitEffects.Add(false);
                    foldOutVariables.Add(true);
                    clip.HitEffects.Add(new RandomEffectInfo());
                    clip.AttackEffects.Add(new ControllerEffectInfo());

                    clip.StrengthType.Add(AttackStrengthType.WEAK);
                    clip.AttackDamage.Add(0f);
                    clip.AttackRange.Add(0f);
                    clip.AttackAngle.Add(0f);
                    clip.TimeDatas.Add(null);
                    clip.AnimationAttackTimingFrame.Add(0);
                    clip.AnimationRotateFrames.Add(0);

                    clip.IsHitRandom.Add(false);
                }
                if (GUILayout.Button("삭제"))
                {
                    if (foldOutAttackEffects.Count > 0)
                        foldOutAttackEffects.RemoveAt(foldOutAttackEffects.Count - 1);
                    if (foldOutHitEffects.Count > 0)
                        foldOutHitEffects.RemoveAt(foldOutHitEffects.Count - 1);
                    if (foldOutVariables.Count > 0)
                        foldOutVariables.RemoveAt(foldOutVariables.Count - 1);
                    if (clip.TimeDatas.Count > 0)
                        clip.TimeDatas.RemoveAt(clip.TimeDatas.Count - 1);
                    if (clip.StrengthType.Count > 0)
                        clip.StrengthType.RemoveAt(clip.StrengthType.Count - 1);
                    if (clip.AttackDamage.Count > 0)
                        clip.AttackDamage.RemoveAt(clip.AttackDamage.Count - 1);
                    if (clip.AttackRange.Count > 0)
                        clip.AttackRange.RemoveAt(clip.AttackRange.Count - 1);
                    if (clip.AttackAngle.Count > 0)
                        clip.AttackAngle.RemoveAt(clip.AttackAngle.Count - 1);
                    if (clip.AnimationAttackTimingFrame.Count > 0)
                        clip.AnimationAttackTimingFrame.RemoveAt(clip.AnimationAttackTimingFrame.Count - 1);
                    if (clip.AnimationRotateFrames.Count > 0)
                        clip.AnimationRotateFrames.RemoveAt(clip.AnimationRotateFrames.Count - 1);

                    if (clip.IsHitRandom.Count > 0)
                        clip.IsHitRandom.RemoveAt(clip.IsHitRandom.Count - 1);
                    if (clip.HitEffects.Count > 0)
                        clip.HitEffects.RemoveAt(clip.HitEffects.Count - 1); 
                    if (clip.AttackEffects.Count > 0)
                        clip.AttackEffects.RemoveAt(clip.AttackEffects.Count - 1);
                }
            }
            GUILayout.EndHorizontal();

            foldOutAllEffects = EditorGUILayout.Foldout(foldOutAllEffects,"Hit & Attack Effect Setting");
            if (foldOutAllEffects)
            {
              
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("All Hit 적용", GUILayout.Width(150), GUILayout.Height(30f)))
                    {
                        for (int i = 0; i < clip.HitEffects.Count; i++)
                        {
                            clip.HitEffects[i].effectSound = clip.allHitEffect.effectSound;
                            clip.HitEffects[i].effect = clip.allHitEffect.effect;
                            clip.HitEffects[i].minPosition = clip.allHitEffect.minPosition;
                            clip.HitEffects[i].maxPosition = clip.allHitEffect.maxPosition;
                            clip.HitEffects[i].minRotation = clip.allHitEffect.minRotation;
                            clip.HitEffects[i].maxRotation = clip.allHitEffect.maxRotation;
                            clip.HitEffects[i].minScale = clip.allHitEffect.minScale;
                            clip.HitEffects[i].maxScale = clip.allHitEffect.maxScale;
                            clip.IsHitRandom[i] = clip.allHitRandom;

                        }
                    }
                    if (GUILayout.Button("All Attack 적용", GUILayout.Width(150), GUILayout.Height(30f)))
                    {
                        for (int i = 0; i < clip.AttackEffects.Count; i++)
                        {
                            clip.AttackEffects[i].effectSound = clip.allAttackEffect.effectSound;
                            clip.AttackEffects[i].effect = clip.allAttackEffect.effect;
                            clip.AttackEffects[i].spawnPosition = clip.allAttackEffect.spawnPosition;
                            clip.AttackEffects[i].spawnRotation = clip.allAttackEffect.spawnRotation;
                            clip.AttackEffects[i].spawnScale = clip.allAttackEffect.spawnScale;
                            clip.StrengthType[i] = clip.allAttackStrength;
                            clip.AttackEffects[i].spawnType = clip.allAttackEffect.spawnType;
                        }
                    }
                    GUILayout.FlexibleSpace();

                }
                GUILayout.EndHorizontal();

                foldOutAllHitEffect = EditorGUILayout.Foldout(foldOutAllHitEffect, "All Hit Effect");
                if (foldOutAllHitEffect)
                {
                    EditorGUILayout.BeginVertical("HelpBox");

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Hit Sound", GUILayout.Width(80));
                        clip.allHitEffect.effectSound = (SoundList)EditorGUILayout.EnumPopup("", clip.allHitEffect.effectSound, GUILayout.Width(250));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Hit Effect", GUILayout.Width(80));
                        clip.allHitEffect.effect = (EffectList)EditorGUILayout.EnumPopup("", clip.allHitEffect.effect, GUILayout.Width(250));
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginVertical("Box");
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Is Random", GUILayout.Width(100));
                            clip.allHitRandom = EditorGUILayout.Toggle(clip.allHitRandom);
                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();

                        if (clip.allHitRandom)
                        {
                            EditorGUILayout.BeginVertical("HelpBox");
                            {
                                EditorGUILayout.LabelField("Position", GUILayout.Width(150));
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                    clip.allHitEffect.minPosition = EditorGUILayout.Vector3Field("", clip.allHitEffect.minPosition, GUILayout.Width(180));
                                    GUILayout.Label(" - ");
                                    EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                    clip.allHitEffect.maxPosition = EditorGUILayout.Vector3Field("", clip.allHitEffect.maxPosition, GUILayout.Width(180));
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
                                    clip.allHitEffect.minRotation = EditorGUILayout.Vector3Field("", clip.allHitEffect.minRotation, GUILayout.Width(180));
                                    GUILayout.Label(" - ");
                                    EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                    clip.allHitEffect.maxRotation = EditorGUILayout.Vector3Field("", clip.allHitEffect.maxRotation, GUILayout.Width(180));
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
                                    clip.allHitEffect.minScale = EditorGUILayout.Vector3Field("", clip.allHitEffect.minScale, GUILayout.Width(180));
                                    GUILayout.Label(" - ");
                                    EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                    clip.allHitEffect.maxScale = EditorGUILayout.Vector3Field("", clip.allHitEffect.maxScale, GUILayout.Width(180));
                                    GUILayout.FlexibleSpace();
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                        }
                        else
                        {
                            EditorGUILayout.BeginVertical("HelpBox");
                            {
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Position", GUILayout.Width(100));
                                    clip.allHitEffect.minPosition = EditorGUILayout.Vector3Field("", clip.allHitEffect.minPosition, GUILayout.Width(180));
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
                                    clip.allHitEffect.minRotation = EditorGUILayout.Vector3Field("", clip.allHitEffect.minRotation, GUILayout.Width(180));
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
                                    clip.allHitEffect.minScale = EditorGUILayout.Vector3Field("", clip.allHitEffect.minScale, GUILayout.Width(180));
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


                foldOutAllAttackEffect = EditorGUILayout.Foldout(foldOutAllAttackEffect, "All Attack Effect");
                if (foldOutAllAttackEffect)
                {
                    GUILayout.BeginVertical("HelpBox");

                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Attack Sound", GUILayout.Width(80));
                        clip.allAttackEffect.effectSound = (SoundList)EditorGUILayout.EnumPopup("", clip.allAttackEffect.effectSound, GUILayout.Width(250));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Attack Effect", GUILayout.Width(80));
                        clip.allAttackEffect.effect = (EffectList)EditorGUILayout.EnumPopup(clip.allAttackEffect.effect, GUILayout.Width(250));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Attack Effect Position", GUILayout.Width(150));
                        clip.allAttackEffect.spawnPosition = EditorGUILayout.Vector3Field("", clip.allAttackEffect.spawnPosition);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Attack Effect Rotation", GUILayout.Width(150));
                        clip.allAttackEffect.spawnRotation = EditorGUILayout.Vector3Field("", clip.allAttackEffect.spawnRotation);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Attack Effect Scale", GUILayout.Width(150));
                        clip.allAttackEffect.spawnScale = EditorGUILayout.Vector3Field("", clip.allAttackEffect.spawnScale);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Attack Strength Type", GUILayout.Width(100));
                        clip.allAttackStrength = (AttackStrengthType)EditorGUILayout.EnumPopup("", clip.allAttackStrength);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Attack Spawn Type", GUILayout.Width(100));
                        clip.allAttackEffect.spawnType = (ControllerInPosition)EditorGUILayout.EnumPopup("", clip.allAttackEffect.spawnType);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }

            }



            if (foldOutVariables.Count > 0)
            {
                EditorGUILayout.BeginVertical("HelpBox");
                for (int i = 0; i < foldOutVariables.Count; i++)
                {
                    foldOutVariables[i] = EditorGUILayout.Foldout(foldOutVariables[i], i + "번 데이터");
                    GUILayout.BeginVertical("box");
                    if (foldOutVariables[i])
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("애니메이션 공격 프레임", GUILayout.Width(130));
                            clip.AnimationAttackTimingFrame[i] = EditorGUILayout.IntField(clip.AnimationAttackTimingFrame[i], GUILayout.Width(100));
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("애니메이션 회전 프레임", GUILayout.Width(130));
                            clip.AnimationRotateFrames[i] = EditorGUILayout.IntField(clip.AnimationRotateFrames[i], GUILayout.Width(100));
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        {
                      
                            EditorGUILayout.LabelField("Time Data", GUILayout.Width(100));
                            clip.TimeDatas[i] = (TimeData)EditorGUILayout.ObjectField(clip.TimeDatas[i],typeof(TimeData), false);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        foldOutHitEffects[i] = EditorGUILayout.Foldout(foldOutHitEffects[i], "Hit Effect Datas");
                        if (foldOutHitEffects[i])
                        {
                            EditorGUILayout.BeginVertical("HelpBox");

                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Hit Sound", GUILayout.Width(80));
                                clip.HitEffects[i].effectSound = (SoundList)EditorGUILayout.EnumPopup("", clip.HitEffects[i].effectSound, GUILayout.Width(250));
                                GUILayout.FlexibleSpace();
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Hit Effect", GUILayout.Width(80));
                                clip.HitEffects[i].effect = (EffectList)EditorGUILayout.EnumPopup("", clip.HitEffects[i].effect, GUILayout.Width(250));
                                GUILayout.FlexibleSpace();
                            }
                            EditorGUILayout.EndHorizontal();


                            EditorGUILayout.BeginVertical("Box");
                            {
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Is Random", GUILayout.Width(100));
                                    clip.IsHitRandom[i] = EditorGUILayout.Toggle(clip.IsHitRandom[i]);
                                    GUILayout.FlexibleSpace();
                                }
                                EditorGUILayout.EndHorizontal();

                                if (clip.IsHitRandom[i])
                                {
                                    EditorGUILayout.BeginVertical("HelpBox");
                                    {
                                        EditorGUILayout.LabelField("Position", GUILayout.Width(150));
                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                            clip.HitEffects[i].minPosition = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minPosition, GUILayout.Width(180));
                                            GUILayout.Label(" - ");
                                            EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                            clip.HitEffects[i].maxPosition = EditorGUILayout.Vector3Field("", clip.HitEffects[i].maxPosition, GUILayout.Width(180));
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
                                            clip.HitEffects[i].minRotation = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minRotation, GUILayout.Width(180));
                                            GUILayout.Label(" - ");
                                            EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                            clip.HitEffects[i].maxRotation = EditorGUILayout.Vector3Field("", clip.HitEffects[i].maxRotation, GUILayout.Width(180));
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
                                            clip.HitEffects[i].minScale = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minScale, GUILayout.Width(180));
                                            GUILayout.Label(" - ");
                                            EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                            clip.HitEffects[i].maxScale = EditorGUILayout.Vector3Field("", clip.HitEffects[i].maxScale, GUILayout.Width(180));
                                            GUILayout.FlexibleSpace();
                                        }
                                        EditorGUILayout.EndHorizontal();
                                    }
                                    EditorGUILayout.EndVertical();
                                }
                                else
                                {
                                    EditorGUILayout.BeginVertical("HelpBox");
                                    {
                                        EditorGUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField("Position", GUILayout.Width(100));
                                            clip.HitEffects[i].minPosition = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minPosition, GUILayout.Width(180));
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
                                            clip.HitEffects[i].minRotation = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minRotation, GUILayout.Width(180));
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
                                            clip.HitEffects[i].minScale = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minScale, GUILayout.Width(180));
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


                        foldOutAttackEffects[i] = EditorGUILayout.Foldout(foldOutAttackEffects[i],"Attack Effect Datas");
                        if(foldOutAttackEffects[i])
                        {
                            GUILayout.BeginVertical("HelpBox");

                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Attack Sound", GUILayout.Width(80));
                                clip.AttackEffects[i].effectSound = (SoundList)EditorGUILayout.EnumPopup("", clip.AttackEffects[i].effectSound, GUILayout.Width(250));
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Attack Effect", GUILayout.Width(80));
                                clip.AttackEffects[i].effect = (EffectList)EditorGUILayout.EnumPopup(clip.AttackEffects[i].effect, GUILayout.Width(250));
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Attack Effect Position", GUILayout.Width(150));
                                clip.AttackEffects[i].spawnPosition = EditorGUILayout.Vector3Field("", clip.AttackEffects[i].spawnPosition);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Attack Effect Rotation", GUILayout.Width(150));
                                clip.AttackEffects[i].spawnRotation = EditorGUILayout.Vector3Field("", clip.AttackEffects[i].spawnRotation);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Attack Effect Scale", GUILayout.Width(150));
                                clip.AttackEffects[i].spawnScale = EditorGUILayout.Vector3Field("", clip.AttackEffects[i].spawnScale);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Attack Strength Type", GUILayout.Width(100));
                                clip.StrengthType[i] = (AttackStrengthType)EditorGUILayout.EnumPopup("", clip.StrengthType[i]);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Spawn Frame", GUILayout.Width(100));
                                clip.AttackEffects[i].effectFrame = EditorGUILayout.FloatField("", clip.AttackEffects[i].effectFrame);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Attack Spawn Type", GUILayout.Width(100));
                                clip.AttackEffects[i].spawnType = (ControllerInPosition)EditorGUILayout.EnumPopup("", clip.AttackEffects[i].spawnType);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.EndVertical();
                        }


                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("데미지", GUILayout.Width(45));
                            clip.AttackDamage[i] = EditorGUILayout.FloatField(clip.AttackDamage[i], GUILayout.Width(70));

                            EditorGUILayout.LabelField("각도", GUILayout.Width(45));
                            clip.AttackAngle[i] = EditorGUILayout.FloatField(clip.AttackAngle[i], GUILayout.Width(70));

                            EditorGUILayout.LabelField("거리", GUILayout.Width(45));
                            clip.AttackRange[i] = EditorGUILayout.FloatField(clip.AttackRange[i], GUILayout.Width(70));
                            GUILayout.FlexibleSpace();

                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
        }
        GUILayout.EndVertical();

    }

}
