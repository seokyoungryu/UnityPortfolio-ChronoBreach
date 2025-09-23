using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DashSkillUpgrade : SkillUpgrade
{
    [SerializeField] private DashSkillUpgradeInfo skillInfo;

    public DashSkillUpgradeInfo SkillInfo => skillInfo;
}

[System.Serializable]
public class DashSkillUpgradeInfo
{
    [SerializeField] private float dashRadius = 2f;
    [SerializeField] private Vector2 allowAiDistance = Vector2.zero;
    [SerializeField] private float dashDamageRange = 4f;
    [SerializeField] private float[] dashDamageP ;
    [SerializeField] private int damageMaxCount = 10;

    public float DashRadius => dashRadius;
    public Vector2 AllowAiDistance => allowAiDistance;
    public float DashDamageRange => dashDamageRange;
    public float[] DashDamageP => dashDamageP;
    public int DamageMaxCount => damageMaxCount;

}
