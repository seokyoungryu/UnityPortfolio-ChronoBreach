using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TimeDataType
{
    ADDITVE = 0,
    CURVE = 1,
    STOPMOMENT = 2,
}

public enum TimeDataAdditiveType
{
    ADDITION = 0,
    SUBTRACTION = 1,
    MULTIPLICATION = 2,
    DEVISION = 3,
}

[CreateAssetMenu(menuName = "Time/Time Data", fileName = "TimeData_")]
public class TimeData : ScriptableObject
{
    [SerializeField] private CamStrength strength;
    [SerializeField] private TimeDataType type = TimeDataType.ADDITVE;
    [SerializeField] private bool allstopCoAndExcute = true; // 실행중인 코루틴 멈추고 실행.
    [SerializeField] private float smooth = 1f;

    [Header("Additive")]
    [SerializeField] private TimeDataAdditiveType addtiveType = TimeDataAdditiveType.ADDITION;
    [SerializeField] private float startTimeScale = 0f;
    [SerializeField] private float perValue = 0.1f;
    [SerializeField] private float waitStartTime = 0.5f;
    [SerializeField] private float waitReduceSec = 0.1f;
    [SerializeField] private float resetTime = 0.5f;
    [SerializeField] private float resetLerpSmooth = 1f;
    [SerializeField] private float devisionMinValue = 0.2f;

    [Header("Curve")]
    [SerializeField] private AnimationCurve slowCurve;
    [SerializeField] private float slowCurveTime = 0.5f;

    [Header("Stopmoment")]
    [SerializeField] private float stopValue = 0f;
    [SerializeField] private float stopTime = 0.02f;


    public TimeDataType Type { get { return type; } set { type = value; } }
    public bool AllstopCoAndExcute { get { return allstopCoAndExcute; } set { allstopCoAndExcute = value; } }
    public CamStrength Strength { get { return strength; } set { strength = value; } }

    public TimeDataAdditiveType AddtiveType { get { return addtiveType; } set { addtiveType = value; } }
    public float StartTimeScale { get { return startTimeScale; } set { startTimeScale = value; } }
    public float PerValue { get { return perValue; } set { perValue = value; } }
    public float WaitStartTime { get { return waitStartTime; } set { waitStartTime = value; } }
    public float WaitPerSec { get { return waitReduceSec; } set { waitReduceSec = value; } }
    public float ResetTime { get { return resetTime; } set { resetTime = value; } }
    public float ResetLerpSmooth { get { return resetLerpSmooth; } set { resetLerpSmooth = value; } }
    public float DevisionMinValue { get { return devisionMinValue; } set { devisionMinValue = value; } }

    public float Smooth { get { return smooth; } set { smooth = value; } }

    public AnimationCurve SlowCurve { get { return slowCurve; } set { slowCurve = value; } }
    public float SlowCurveDurationTime { get { return slowCurveTime; } set { slowCurveTime = value; } }

    public float StopValue { get { return stopValue; } set { stopValue = value; } }
    public float StopTime { get { return stopTime; } set { stopTime = value; } }


#if UNITY_EDITOR
    [ContextMenu("Save")]
    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
