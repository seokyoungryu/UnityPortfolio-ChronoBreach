using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill Data/Dash Skill Data", fileName = "SD_")]
public class DashSkillClip : BaseSkillClip
{
    [Header("Animation Variables")]
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private AnimationClip animationClip = null;
    [SerializeField] private string animName = string.Empty;
    [SerializeField] private int fullFrame = 0;
    [SerializeField] private int endFrame = 0;
    [SerializeField] private int[] damageTimingFrame;


    [Header("Dash Variables")]
    [SerializeField] private ControllerEffectInfo[] effectInfo;


    [SerializeField] private AttackStrengthType[] attackStrengthType;
    [SerializeField] private float dashDetectRadius = 2f;
    [SerializeField] private Vector2 allowAiDistance = Vector2.zero;
    [SerializeField] private float dashDamageRange = 4f;
    [SerializeField] private float[] dashDamageP;


    [Header("Player용 SKill Upgrade")]
    public DashSkillUpgrade[] upgrades;

    //스킬 업글하면 공격력 증가, 대시 가능 범위 증가. , 데미지 범위 증가. maxcount증가?

    #region Getter

    public float DashDetectRadius => dashDetectRadius;
    public Vector2 AllowAiDistance => allowAiDistance;
    public float DashDamageRange => dashDamageRange;
    public float[] DashDamageP => dashDamageP;

    public float AnimationSpeed => animationSpeed;
    public float EndTime => GetFrameToTime(endFrame, animationClip.frameRate);
    public string AnimationName => animName;
    public AttackStrengthType[] AttackStrengthType => attackStrengthType;
    public EffectInfo[] EffectInfo => effectInfo;
    #endregion


    public DashSkillClip() : base() { }
    public DashSkillClip(BaseSkillClip copyClip) : base(copyClip)
    {
        if (copyClip is DashSkillClip)
        {
            DashSkillClip clone = copyClip as DashSkillClip;
            skillType = SkillType.ATTACK;
            attackStrengthType = clone.attackStrengthType;
            effectInfo = clone.effectInfo;
            animationSpeed = clone.animationSpeed;
            animationClip = clone.animationClip;
            animName = clone.animName;
            fullFrame = clone.fullFrame;
            endFrame = clone.endFrame;
            damageTimingFrame = clone.damageTimingFrame;
            dashDetectRadius = clone.dashDetectRadius;
            allowAiDistance = clone.allowAiDistance;
            dashDamageRange = clone.dashDamageRange;
            dashDamageP = clone.dashDamageP;

             upgrades = clone.upgrades;
        }
    }


    public override void LoadSkill(PlayerStateController controller) 
        => LoadSkill(controller, upgrades);

    public override void UpgradeSkill(PlayerStateController controller, bool isOwnSkill = false)
          => UpgradeSkill(controller, upgrades, isOwnSkill);

    protected override void ApplySkillInfo(PlayerStateController controller, SkillUpgrade skillUpgrade, bool isLoadSkill = false)
    {
        base.ApplySkillInfo(controller, skillUpgrade, isLoadSkill);

        if (!(skillUpgrade is DashSkillUpgrade))
            return;
        DashSkillUpgrade upgrade = skillUpgrade as DashSkillUpgrade;
       this.dashDetectRadius = upgrade.SkillInfo.DashRadius;
       this.allowAiDistance = upgrade.SkillInfo.AllowAiDistance;
       this.dashDamageRange = upgrade.SkillInfo.DashDamageRange;
       this.dashDamageP = upgrade.SkillInfo.DashDamageP;
       this.maxTargetCount = upgrade.SkillInfo.DamageMaxCount;
        Debug.Log("어플라이 대시!");

    }

    public override string[] GetConditionDescriptions() => GetConditionDescriptions(upgrades);
    public override void UpdateUpgradeType() => UpdateUpgradeType(upgrades);

    public override bool CheckCanUpgrade(PlayerStateController controller)
      => CheckCanUpgrade(controller, upgrades);

    public override int GetNextRequireLv() => NextRequireLv(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireMoney() => NextRequireMoney(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireSkillPoint() => NextRequireSkillPoint(upgrades, currentSkillIndex + 1);

    public float[] GetDamageTime()
    {
        List<float> times = new List<float>();
        float rate = (1 / (animationClip.frameRate * animationSpeed));

        for (int i = 0; i < damageTimingFrame.Length; i++)
            times.Add(damageTimingFrame[i] * rate);

        return times.ToArray();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if(animationClip != null)
        {
            fullFrame = (int)(animationClip.length * animationClip.frameRate);
        }

        if (skillType != SkillType.DASH)
            skillType = SkillType.DASH;

        if (upgrades.Length > 0)
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].UpgradeName = "Upgrade " + (i + 1);
                upgrades[i].SkillLevel = (i + 1);
            }
        }


    }
#endif
}
