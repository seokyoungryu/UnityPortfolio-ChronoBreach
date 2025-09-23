using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffSkillUpgrade : SkillUpgrade
{
    [SerializeField] private BuffSkillUpgradeInfo skillInfo;

    public BuffSkillUpgradeInfo SkillInfo => skillInfo;
}

[System.Serializable]
public class BuffSkillUpgradeInfo 
{
    [SerializeField] private BuffStatsObject[] upgradeBuffObjects;
    [SerializeField] private int maxTargetCount = 0;
    [SerializeField] private float buffAngle = 0;
    [SerializeField] private float buffDistance = 0;
    [SerializeField] private float skillSpCost = 0;
    [SerializeField] private float skillCoolTime = 0;

 
    public BuffStatsObject[] UpgradeBuffObjects { get { return upgradeBuffObjects; } set { upgradeBuffObjects = value; } }
    public int MaxTargetCount { get { return maxTargetCount; } set { maxTargetCount = value; } }
    public float BuffAngle { get { return buffAngle; } set { buffAngle = value; } }
    public float BuffDistance { get { return buffDistance; } set { buffDistance = value; } }
    public float SkillSpCost { get { return skillSpCost; } set { skillSpCost = value; } }
    public float SkillCoolTime { get { return skillCoolTime; } set { skillCoolTime = value; } }

}

