using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillUpgradeType
{
    LOCK,
    UPGRADE,
    DONE,
}


[System.Serializable]
public class SkillUpgrade 
{
    [SerializeField] private string upgradeName = string.Empty;

    [Header("base스킬의 업그레이드 단계")]
    [SerializeField] private int skillLevel = 1;
    [SerializeField] private int requireUpgradeCost = 0;
    [SerializeField] private int requirePlayerLevel = 0;
    [SerializeField] private int requireSkillPoint = 1;
    [SerializeField] private SkillUpgradeContainer[] requiredSkillData;

    [SerializeField, TextArea(0, 10)] private string description = string.Empty;

    public string UpgradeName { get { return upgradeName; } set { upgradeName = value; } }
    public int SkillLevel { get { return skillLevel; } set { skillLevel = value; } }
    public int RequireSkillPoint { get { return requireSkillPoint; } set { requireSkillPoint = value; } }
    public int RequireUpgradeCost { get { return requireUpgradeCost; } set { requireUpgradeCost = value; } }
    public int RequirePlayerLevel { get { return requirePlayerLevel; } set { requirePlayerLevel = value; } }
    public string Description { get { return description; } set { description = value; } }
    public SkillUpgradeContainer[] RequiredSkillData => requiredSkillData;

}


[System.Serializable]
public class SkillUpgradeContainer
{
    [Header("업그레이드 조건")]
    [SerializeField] private int requiredSkillLevel = 0;
    [SerializeField] private string description = string.Empty;
    [SerializeField] private BaseSkillClip conditionSkillClip;

    public int RequiredSkillLevel => requiredSkillLevel;
    public string Description => description;
    public BaseSkillClip ConditionSkillClip => conditionSkillClip;
}