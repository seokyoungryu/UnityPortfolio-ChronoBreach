using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveSkillUpgrade : SkillUpgrade
{
    [SerializeField] private PassiveSkillUpgradeInfo skillInfo;

    public PassiveSkillUpgradeInfo SkillInfo => skillInfo;
}

[System.Serializable]
public class PassiveSkillUpgradeInfo
{
    [SerializeField] private BaseStatsObject[] upgradeStatsObjects;

    public BaseStatsObject[] UpgradeStatsObjects { get { return upgradeStatsObjects; } set { upgradeStatsObjects = value; } }

}
