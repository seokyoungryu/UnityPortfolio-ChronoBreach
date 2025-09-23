using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboUpgrade 
{
    [SerializeField] private string name = string.Empty;
    private int comboLevel = 0;
    [SerializeField] private ComboInfo comboInfo = null;

    public string Name { get { return name; } set { name = value; } }
    public int ComboLv { get { return comboLevel; } set { comboLevel = value; } }
    public ComboInfo ComboInfo => comboInfo;
}


[System.Serializable]
public class ComboInfo
{
    [SerializeField] private float[] damage ;
    [SerializeField] private float attackAngle = 0f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private int maxTargetCount = 0;
    [SerializeField] private float spCost = 0f;

    public float[] Damage => damage;
    public float AttackAngle => attackAngle;
    public float AttackRange => attackRange;
    public int MaxTargetCount => maxTargetCount;
    public float SpCost => spCost;
}