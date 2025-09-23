using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    [SerializeField] private List<TimeInfos> baseTimeDatas = new List<TimeInfos>();
    private float currentTimer = 0f;
    private float loadTimer = 0;

    private int hour;
    private int min;
    private int sec;

    public float currTimeScale = 0f;
    public TimeData currentTimeData = null;

    private float lerpSmooth = 1f;
    private float targetTimeScale = 1f;
    private bool isTimeChange = false;

    public float CurrentTimer => currentTimer;
    public float LoadTimer { get { return loadTimer; } set { loadTimer = value; } }

    private void Update()
    {
       // if (Input.GetKeyDown(KeyCode.Alpha9))
       //     ExcuteTimeData(currentTimeData);

        currTimeScale = Time.timeScale;
        if (targetTimeScale <= 0f) targetTimeScale = 0.01f;
        if (!isTimeChange || Time.timeScale == targetTimeScale) return;

        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * lerpSmooth);

    }

    public void TimeReset()
    {
        isTimeChange = false;
        targetTimeScale = 1f;
        Time.timeScale = 1f;
        currTimeScale = 1f;
    }

    public void AllTimeReset()
    {
        StopAllCoroutines();
        TimeReset();
    }

    public void ExcuteBaseTimeData(TimeInfoType infoType)
    {
        if (infoType == TimeInfoType.ATTACK)
        {
            Debug.Log("공격 아웃");
            return;
        }


        TimeData data = GetBaseTimeData(infoType);

        if (data == null || !CanExcuteStrength(data)) return;

        currentTimeData  = data;
        lerpSmooth = currentTimeData.Smooth;
        ExcuteTimeSlow(currentTimeData);
    }

    [ContextMenu("Test")]
    public void Test()
    {
        Debug.Log("Time : " + Time.time);

    }

    public void ExcuteTimeData(TimeData data)
    {
        if (data == null || !CanExcuteStrength(data)) return;

        currentTimeData = data;
        lerpSmooth = data.Smooth;
        ExcuteTimeSlow(currentTimeData);
    }

    private bool CanExcuteStrength(TimeData data)
    {
        if (isTimeChange)
        {
            if (currentTimeData.Strength == CamStrength.ABSULUTE)
                return false;
            if (data.Strength == CamStrength.NONE)
                return true;
            if (currentTimeData.Strength <= data.Strength)
                return true;
            else if (currentTimeData.Strength > data.Strength)
                return false;
        }
        return true;
    }



    private void ExcuteTimeSlow(TimeData data)
    {
        switch (data.Type)
        {
            case TimeDataType.ADDITVE: TimeSlowAdditive(data); break;
            case TimeDataType.CURVE: TimeSlowCurve(data); break;
            case TimeDataType.STOPMOMENT: TimeSlowStopmoment(data); break;
        }
    }


    private void TimeSlowCurve(TimeData data)
    {
        if (data.AllstopCoAndExcute)
            StopAllCoroutines();
        StartCoroutine(SlowCurve_Co(data));
    }


    private void TimeSlowAdditive(TimeData data)
    {
        if (data.AllstopCoAndExcute)
            StopAllCoroutines();
        StartCoroutine(SlowAdditive_Co(data));
    }

    private void TimeSlowStopmoment(TimeData data)
    {
        if (data.AllstopCoAndExcute)
            StopAllCoroutines();
        StartCoroutine(SlowStopmomonet_Co(data));
    }



    private IEnumerator SlowCurve_Co(TimeData data)
    {
        isTimeChange = true;
        GameManager.Instance.canUseCamera = false;
        GameManager.Instance.canUsePlayerState = false;
        float currentTime = 0f;
        Time.timeScale = 0.01f;
        targetTimeScale = 0.01f;

        while (currentTime < data.SlowCurveDurationTime)
        {
            currentTime += Time.unscaledDeltaTime / data.SlowCurveDurationTime;
            targetTimeScale = data.SlowCurve.Evaluate(currentTime);
            if (targetTimeScale > 1f) targetTimeScale = 1f;
            yield return null;

        }

        isTimeChange = false;
        Time.timeScale = 1f;
        GameManager.Instance.canUseCamera = true;
        GameManager.Instance.canUsePlayerState = true;
    }

    private IEnumerator SlowAdditive_Co(TimeData data)
    {
        bool isReached = false;
        GameManager.Instance.canUseCamera = false;
        GameManager.Instance.canUsePlayerState = false;
        targetTimeScale = data.StartTimeScale;
        Time.timeScale = data.StartTimeScale;
        isTimeChange = true;

        yield return new WaitForSecondsRealtime(data.WaitStartTime);

        while(!isReached)
        {
            yield return new WaitForSecondsRealtime(data.WaitPerSec);

            CalculateAddtive(data);

            if (data.AddtiveType == TimeDataAdditiveType.DEVISION)
                if (targetTimeScale <= data.DevisionMinValue)
                    isReached = true;

            if (targetTimeScale >= 1f)
            {
                targetTimeScale = 1f;
                isReached = true;
            }
            else if(targetTimeScale <= 0f)
            {
                targetTimeScale = 0.01f;
                isReached = true;
            }
        }

        lerpSmooth = data.ResetLerpSmooth;

        if (data.AddtiveType == TimeDataAdditiveType.DEVISION || data.AddtiveType == TimeDataAdditiveType.SUBTRACTION)
        {
            yield return new WaitForSecondsRealtime(data.ResetTime);
            targetTimeScale = 1f;
        }
        else
            targetTimeScale = 1f;


        GameManager.Instance.canUseCamera = true;
        GameManager.Instance.canUsePlayerState = true;

        StartCoroutine(CheckIsReachedTarget());
    }


    //단순 한개의 속도로
    private IEnumerator SlowStopmomonet_Co(TimeData data)
    {
        targetTimeScale = 0f;
        isTimeChange = false;

        Time.timeScale = data.StopValue;

        yield return new WaitForSecondsRealtime(data.StopTime);

        Time.timeScale = 1f;
        GameManager.Instance.canUseCamera = true;
        GameManager.Instance.canUsePlayerState = true;
    }

    private IEnumerator CheckIsReachedTarget()
    {
        bool isReached = false;

        while(!isReached)
        {
            yield return null;

            if (Time.timeScale + 0.1f >= targetTimeScale)
            {
                isReached = true;
                isTimeChange = false;
            }
        }

    }


    private void CalculateAddtive(TimeData data)
    {
        if (data.AddtiveType == TimeDataAdditiveType.ADDITION)
            targetTimeScale += data.PerValue;
        else if (data.AddtiveType == TimeDataAdditiveType.SUBTRACTION)
            targetTimeScale -= data.PerValue;
        else if (data.AddtiveType == TimeDataAdditiveType.MULTIPLICATION)
            targetTimeScale *= data.PerValue;
        else if (data.AddtiveType == TimeDataAdditiveType.DEVISION)
            targetTimeScale /= data.PerValue;
    }

    public TimeData GetBaseTimeData(TimeInfoType  infoType )
    {
        for (int i = 0; i < baseTimeDatas.Count; i++)
            if (baseTimeDatas[i].InfoType == infoType)
                return baseTimeDatas[i].Data;

        return null;
    }

    public void SetCurrTime()
    {
        currentTimer = Time.time;
        (int, int, int) times = TranslateTime((int)(currentTimer + loadTimer));
        hour = times.Item1;
        min = times.Item2;
        sec = times.Item3;
    }
  
    public (int,int,int) TranslateTime(int seconds)
    {                     
        int hours = seconds / 3600;  
        int minutes = (seconds % 3600) / 60;  
        int sec = seconds % 60; 
        return (hours, minutes, sec);
    }
}


[System.Serializable]
public class TimeInfos
{
    [SerializeField] private TimeInfoType infoType;
    [SerializeField] private TimeData data;

    public TimeInfoType InfoType => infoType;
    public TimeData Data => data;
}

public enum TimeInfoType
{
     COUNTER_SUCCESS = 0,
     ATTACK = 1,
     SKILL = 2,
     COUNTER_PARRYING = 3,
     RANGE_SKILL = 4,
}