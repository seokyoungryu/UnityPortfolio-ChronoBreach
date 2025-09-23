using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CounterSkillUpgrade : SkillUpgrade
{
    [SerializeField] private CounterSkillUpgradeInfo skillInfo;

    public CounterSkillUpgradeInfo SkillInfo => skillInfo;
}

[System.Serializable]
public class CounterSkillUpgradeInfo
{
    [SerializeField] private BuffStatsObject[] buffObject;

    public BuffStatsObject[] BuffObject => buffObject;
}
