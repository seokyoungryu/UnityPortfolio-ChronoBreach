using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimeData))]
public class TimeDataGUIEditor : Editor
{
    private TimeData data = null;


    public override void OnInspectorGUI()
    {
        data = (TimeData)target;

        EditorGUILayout.BeginVertical("Box");
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Time Type", GUILayout.Width(200));
                data.Type = (TimeDataType)EditorGUILayout.EnumPopup(data.Type, GUILayout.Width(250));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Strengh Type", GUILayout.Width(200));
                data.Strength = (CamStrength)EditorGUILayout.EnumPopup(data.Strength, GUILayout.Width(250));
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("진행중인 Time 초기화후 실행", GUILayout.Width(200));
                data.AllstopCoAndExcute = EditorGUILayout.Toggle(data.AllstopCoAndExcute);
            }
            EditorGUILayout.EndHorizontal();

            if (data.Type != TimeDataType.STOPMOMENT)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Lerp Smooth Value ", GUILayout.Width(200));
                    data.Smooth = EditorGUILayout.FloatField(data.Smooth, GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
            }


            EditorGUILayout.Space(20);

            if (data.Type == TimeDataType.ADDITVE)
            {
                EditorGUILayout.BeginVertical("HelpBox");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Addtive Type", GUILayout.Width(150));
                        data.AddtiveType = (TimeDataAdditiveType)EditorGUILayout.EnumPopup(data.AddtiveType, GUILayout.Width(250));
                    }
                    EditorGUILayout.EndHorizontal();

                    if (data.AddtiveType == TimeDataAdditiveType.DEVISION || data.AddtiveType == TimeDataAdditiveType.SUBTRACTION)
                    {
                        EditorGUILayout.BeginVertical("HelpBox");
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                GUILayout.Label("TimeScale 0일때 Reset Time", GUILayout.Width(200));
                                data.ResetTime = EditorGUILayout.FloatField(data.ResetTime, GUILayout.Width(100));
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            {
                                GUILayout.Label("Reset Lerp Smooth", GUILayout.Width(200));
                                data.ResetLerpSmooth = EditorGUILayout.FloatField(data.ResetLerpSmooth, GUILayout.Width(100));
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.BeginHorizontal();
                            {
                                GUILayout.Label("Devision 최소 리셋 값", GUILayout.Width(200));
                                data.DevisionMinValue = EditorGUILayout.FloatField(data.DevisionMinValue, GUILayout.Width(100));
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.EndVertical();
                    }

                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("Start Time Scale", GUILayout.Width(150));
                            data.StartTimeScale = EditorGUILayout.FloatField(data.StartTimeScale, GUILayout.Width(100));
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("Per Scale Value", GUILayout.Width(150));
                            data.PerValue = EditorGUILayout.FloatField(data.PerValue, GUILayout.Width(100));
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("Start Wait Time", GUILayout.Width(150));
                            data.WaitStartTime = EditorGUILayout.FloatField(data.WaitStartTime, GUILayout.Width(100));
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        {
                            GUILayout.Label("Wait Per Time", GUILayout.Width(150));
                            data.WaitPerSec = EditorGUILayout.FloatField(data.WaitPerSec, GUILayout.Width(100));
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
            }
            else if (data.Type == TimeDataType.CURVE)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Slow Curve", GUILayout.Width(150));
                    data.SlowCurve = EditorGUILayout.CurveField(data.SlowCurve, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Slow Duration Time", GUILayout.Width(150));
                    data.SlowCurveDurationTime = EditorGUILayout.FloatField(data.SlowCurveDurationTime, GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
            }
            else if (data.Type == TimeDataType.STOPMOMENT)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Stop Value", GUILayout.Width(150));
                    data.StopValue = EditorGUILayout.FloatField(data.StopValue, GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Stop Duration Time", GUILayout.Width(150));
                    data.StopTime = EditorGUILayout.FloatField(data.StopTime, GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }


}
