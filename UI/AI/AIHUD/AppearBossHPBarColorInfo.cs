using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AppearBossHPBarColorInfo
{
    [SerializeField] private int lessHpBarCount = -1;
    [SerializeField] private Color currentHpBarColor = Color.white;
    [SerializeField] private Color prevHpBarColor = Color.white;
    [SerializeField] private Color backgroundColor = Color.white;

    public int LessHpBarCount => lessHpBarCount;
    public Color CurrentHpBarColor => currentHpBarColor;
    public Color PrevHpBarColor => prevHpBarColor;
    public Color BackgroundColor => backgroundColor;


    public AppearBossHPBarColorInfo GetLessHpBarCountInfo(AppearBossHPBarColorInfo[] infos , int barCount)
    {
        for (int i = 0; i < infos.Length; i++)
            if (barCount <= infos[i].lessHpBarCount)
                return infos[i];
            else if (barCount >= infos[infos.Length-1].lessHpBarCount)
                return infos[infos.Length - 1];

        return infos[0];
    }

   // public AppearBossHPBarColorInfo GetNextLessHpBarColor(AppearBossHPBarColorInfo[] infos, int barCount)
   // {
   //     for (int i = 0; i < infos.Length; i++)            
   //         if (barCount > infos[i].lessHpBarCount)       
   //             return infos[i];                          
   //
   //     return null;
   // }
}
