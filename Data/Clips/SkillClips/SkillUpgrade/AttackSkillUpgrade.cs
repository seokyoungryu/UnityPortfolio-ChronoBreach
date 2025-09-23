using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackSkillUpgrade : SkillUpgrade
{
    [SerializeField] private AttackSkillUpgradeInfo skillInfo;

    public AttackSkillUpgradeInfo SkillInfo => skillInfo;
}

[System.Serializable]
public class AttackSkillUpgradeInfo 
{
    [SerializeField] private List<float> attackDamage = new List<float>();
    [SerializeField] private List<float> attackRange = new List<float>();
    [SerializeField] private List<float> attackAngle = new List<float>();
    [SerializeField] private float skillSpCost = 0;
    [SerializeField] private float skillCoolTime = 0;
    [SerializeField] private int maxTargetCount = 0;

    
    public float SkillSpCost { get { return skillSpCost; } set { skillSpCost = value; } }
    public int MaxTargetCount { get { return maxTargetCount; } set { maxTargetCount = value; } }
    public List<float> AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
    public List<float> AttackAngle { get { return attackAngle; } set { attackAngle = value; } }
    public List<float> AttackRange { get { return attackRange; } set { attackRange = value; } }
    public float SkillCoolTime { get { return skillCoolTime; } set { skillCoolTime = value; } }


}
