using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMapTriggers : MonoBehaviour
{
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private bool allDraw = false;
    [SerializeField] private BaseDungeonTitle title;


    private void OnDrawGizmos()
    {
        if (title != null)
        {
            if (!allDraw)
                title.ExcuteDrawCurrentWave(currentWaveIndex);
            else
                title.ExcuteDrawAllWave();
        }
    }

}