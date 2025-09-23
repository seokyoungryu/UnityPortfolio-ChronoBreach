using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagicSkillUpgrade : SkillUpgrade
{
    [SerializeField] private MagicSkillUpgradeInfo skillInfo;

    public MagicSkillUpgradeInfo SkillInfo => skillInfo;
}

[System.Serializable]
public class MagicSkillUpgradeInfo
{
    [SerializeField] private float spCost = 0f;
    [SerializeField] private float coolTime = 0f;
    [SerializeField] private float canExcuteDistance = 5f;
    [SerializeField] private List<MagicProjectileInfo> projectileCreator = new List<MagicProjectileInfo>();

    public float SpCost { get { return spCost; } set { spCost = value; } }
    public float CoolTime { get { return coolTime; } set { coolTime = value; } }
    public float CanExcuteDistance { get { return canExcuteDistance; } set { canExcuteDistance = value; } }
    public List<MagicProjectileInfo> GetProjectileCreator { get { return projectileCreator; } set { projectileCreator = value; } }
}

