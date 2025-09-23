using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearBossData : MonoBehaviour
{
    public AIController target = null;
    public GlobalAppearBossHPUI globalAppearBossHPUI = null;
    public float damage = 10f;

    [SerializeField] private int aiHpBarCount = -1;
    [SerializeField] private float maxHpValue = -1;
    [SerializeField] private int currentHpBarCount = -1;
    [SerializeField] private float currentHpValue = -1;
    [SerializeField] private float oneLineValue = -1;
    [SerializeField] private float currentOneLineValue = -1;


    public int AIHpBarCount => AIHpBarCount;
    public float MaxHpValue => maxHpValue;
    public int CurrentHpBarCount => currentHpBarCount;
    public float CurrentHpValue => currentHpValue;
    public float OneLineValue => oneLineValue;
    public float CurrentOneLineValue => currentOneLineValue;


    public void SettingDatas()
    {
        if (target == null) return;

        AIStatus aiStatus = target.aiStatus;
        aiHpBarCount = aiStatus.OriginHealthBarCount;
        maxHpValue = aiStatus.TotalHealth;
        currentHpValue = aiStatus.CurrentHealth;

        oneLineValue = maxHpValue / aiHpBarCount;        
        currentHpBarCount = 0;
        float tmpCurrentHP = currentHpValue;
        while (tmpCurrentHP > oneLineValue)
        {
            tmpCurrentHP -= oneLineValue;
            currentHpBarCount++;
        }
        currentOneLineValue = tmpCurrentHP;
        currentHpBarCount++;
    
    }
}
