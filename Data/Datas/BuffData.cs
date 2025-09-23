using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffData 
{
    public int buffID = -1;
    public string buffName = string.Empty;
    public float durationTime = 0f;
    public float currentTime = 0f;
    public BuffStatsObject buffData = null;
    public bool isCounterStateBuff = false;

    public BuffData(BuffStatsObject buffObject)
    {
        if (buffObject == null) return;

        buffID = buffObject.ID;
        buffName = buffObject.ObjectName;
        buffData = Object.Instantiate(buffObject);
        durationTime = buffObject.Duration;
    }

}

