using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraShakeInfo))]
public class CamShakeGUIEditor : Editor
{
    private CameraShakeInfo clip = null;
    private GUIStyle titleGui = new GUIStyle();


   // public float reduceRate = 0f;
   //
   // [Header("Zoom Z")]
   // public AnimationCurve curveX;
   // public AnimationCurve curveY;
   // public AnimationCurve curveZ;

    public override void OnInspectorGUI()
    {
        clip = (CameraShakeInfo)target;


        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.BeginHorizontal();
            {
                titleGui.fontStyle = FontStyle.Bold;
                titleGui.fontSize = 30;
                titleGui.normal.textColor = Color.white;
                EditorGUILayout.LabelField("Camera Shake Info", titleGui);
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);

            //GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Shake Type", GUILayout.Width(100));
                clip.shakeType = (CamShakeType)EditorGUILayout.EnumPopup(clip.shakeType);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Is Ignore Strength", GUILayout.Width(100));
                clip.isIgnoreStrength = EditorGUILayout.Toggle("", clip.isIgnoreStrength);
            }
            EditorGUILayout.EndHorizontal();

            if (clip.shakeType != CamShakeType.NONE)
            {

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Cam Strength", GUILayout.Width(100));
                    clip.camStrength = (CamStrength)EditorGUILayout.EnumPopup(clip.camStrength);
                }
                EditorGUILayout.EndHorizontal();


                if (clip.shakeType != CamShakeType.SMOOTH_REDUCE_TIME && clip.shakeType != CamShakeType.IMMEDIATE_REDUCE_TIME 
                    && clip.shakeType != CamShakeType.CURVE_VECTOR3 && clip.shakeType != CamShakeType.CURVE_Z)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Min Shake Positon", GUILayout.Width(100));
                        clip.minShakePositon = EditorGUILayout.Vector3Field("", clip.minShakePositon);
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Max Shake Positon", GUILayout.Width(100));
                        clip.maxShakePositon = EditorGUILayout.Vector3Field("", clip.maxShakePositon);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if(clip.shakeType == CamShakeType.SMOOTH_REDUCE_TIME || clip.shakeType == CamShakeType.IMMEDIATE_REDUCE_TIME)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Target Shake Positon", GUILayout.Width(100));
                        clip.targetShakePositon = EditorGUILayout.Vector3Field("", clip.targetShakePositon);
                    }
                    EditorGUILayout.EndHorizontal();
                }


                if (clip.shakeType == CamShakeType.IMMEDIATE_COUNT || clip.shakeType == CamShakeType.SMOOTH_COUNT)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Count", GUILayout.Width(100));
                        clip.count = EditorGUILayout.IntField("", clip.count);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Duration", GUILayout.Width(100));
                        clip.duration = EditorGUILayout.FloatField("", clip.duration);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (clip.shakeType == CamShakeType.IMMEDIATE_COUNT || clip.shakeType == CamShakeType.IMMEDIATE_TIME)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Change Rate", GUILayout.Width(100));
                        clip.changeRate = EditorGUILayout.FloatField("", clip.changeRate);
                    }
                    EditorGUILayout.EndHorizontal();
                }

               if (clip.shakeType != CamShakeType.IMMEDIATE_COUNT && clip.shakeType != CamShakeType.IMMEDIATE_TIME
                    && clip.shakeType != CamShakeType.CURVE_VECTOR3 && clip.shakeType != CamShakeType.CURVE_Z)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Lerp Speed", GUILayout.Width(100));
                        clip.lerpSpeed = EditorGUILayout.FloatField("", clip.lerpSpeed);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (clip.shakeType == CamShakeType.IMMEDIATE_REDUCE_TIME || clip.shakeType == CamShakeType.SMOOTH_REDUCE_TIME)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Reduce Rate", GUILayout.Width(100));
                        clip.reduceRate = EditorGUILayout.FloatField("", clip.reduceRate);
                    }
                    EditorGUILayout.EndHorizontal();
                }


                if (clip.shakeType == CamShakeType.CURVE_VECTOR3)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Curve X", GUILayout.Width(100));
                        clip.curveX = EditorGUILayout.CurveField(clip.curveX);
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Curve Y", GUILayout.Width(100));
                        clip.curveY = EditorGUILayout.CurveField(clip.curveY);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if (clip.shakeType == CamShakeType.CURVE_VECTOR3 || clip.shakeType == CamShakeType.CURVE_Z)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Curve Z", GUILayout.Width(100));
                        clip.curveZ = EditorGUILayout.CurveField(clip.curveZ);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if(clip.shakeType == CamShakeType.CURVE_VECTOR3)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        clip.minShakePositon = EditorGUILayout.Vector3Field("Min Vector3", clip.minShakePositon);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        clip.maxShakePositon = EditorGUILayout.Vector3Field("Max Vector3", clip.maxShakePositon);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

}
