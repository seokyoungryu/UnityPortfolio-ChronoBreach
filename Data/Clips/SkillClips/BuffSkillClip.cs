using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuffOwnType
{
    INCLUDE_OWN = 0,
    EXCLUDE_OWN = 1,
}

public enum BuffTargetType
{
    NONE = -1,
    ONLY_ENEMY = 0,
    ONLY_FRIENDLY = 1,
    JUST_RANGE = 2,
}



[CreateAssetMenu(menuName = "Data/Skill Data/Buff Skill Data", fileName = "SB_")]
public class BuffSkillClip : BaseSkillClip
{
    [Header("Buff Variables")]
    public bool isDebuffSkill = false;
    public BuffOwnType buffOwnType = BuffOwnType.INCLUDE_OWN;
    public BuffTargetType buffTargetType = BuffTargetType.NONE;
    public float detectRange = 0f;
    public float angle = 0f;

    [Header("Buff Object")]
    public BuffStatsObject[] buffObjects = null;

    [Header("Buff Effect")]
    public ControllerEffectInfo[] castingEffect;
    public ControllerEffectInfo[] effectInfos;
    public ParticleLifeType particleTimeType = ParticleLifeType.NONE;
    public float stayTime = 0f;
    public float durationTime = 0f;
    public float returnObpTime = 0f;

    [Header("Animation Settings")]
    public float animationSpeed = 1f;
    public string animationClipName = string.Empty;
    public AnimationClip animationClip = null;
    public int fullFrame = 0;
    public int animationBuffTimingFrame = 0;
    public int animationEndFrame = 0;

    [Header("Player¿ë SKill Upgrade")]
    public BuffSkillUpgrade[] upgrades;

    public BuffSkillClip() : base() { }
    public BuffSkillClip(BaseSkillClip copyClip) : base(copyClip)
    {
         if(copyClip is BuffSkillClip)
        {
            BuffSkillClip clone = copyClip as BuffSkillClip;
            skillType = SkillType.BUFF;
            buffOwnType = clone.buffOwnType;
            buffTargetType = clone.buffTargetType;
            angle = clone.angle;
            detectRange = clone.detectRange;
            isDebuffSkill = clone.isDebuffSkill;
            buffObjects = clone.buffObjects;
            returnObpTime = clone.returnObpTime;
            effectInfos = clone.effectInfos;
            particleTimeType = clone.particleTimeType;
            stayTime = clone.stayTime;
            durationTime = clone.durationTime;
            castingEffect = clone.castingEffect;
           animationClipName = clone.animationClipName;
            animationBuffTimingFrame = clone.animationBuffTimingFrame;
            animationEndFrame = clone.animationEndFrame;
            animationSpeed = clone.animationSpeed;
            upgrades = clone.upgrades;
            fullFrame = clone.fullFrame;
            animationClip = clone.animationClip;
        }
    }


    public float GetBuffTimingTime() => GetFrameToTime(animationBuffTimingFrame, animationClip.frameRate, animationSpeed);
    public float GetBuffEndTime() => GetFrameToTime(animationEndFrame, animationClip.frameRate, animationSpeed);


    public override void LoadSkill(PlayerStateController controller) => LoadSkill(controller, upgrades);

    public override void UpgradeSkill(PlayerStateController controller, bool isOwnSkill = false)
          => UpgradeSkill(controller, upgrades, isOwnSkill);


    public override int GetNextRequireLv() => NextRequireLv(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireMoney() => NextRequireMoney(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireSkillPoint() => NextRequireSkillPoint(upgrades, currentSkillIndex + 1);

    protected override void ApplySkillInfo(PlayerStateController controller, SkillUpgrade skillUpgrade, bool isLoadSkill = false)
    {
        base.ApplySkillInfo(controller, skillUpgrade, isLoadSkill);

        if (!(skillUpgrade is BuffSkillUpgrade))
            return;
        BuffSkillUpgrade upgrade = skillUpgrade as BuffSkillUpgrade;
        this.buffObjects = upgrade.SkillInfo.UpgradeBuffObjects;
        this.angle = upgrade.SkillInfo.BuffAngle;
        this.detectRange = upgrade.SkillInfo.BuffDistance;
        this.skillStaminaCost = upgrade.SkillInfo.SkillSpCost;
        this.skillCoolTime = upgrade.SkillInfo.SkillCoolTime;
        this.maxTargetCount = upgrade.SkillInfo.MaxTargetCount;
    }

    public override string[] GetConditionDescriptions()  => GetConditionDescriptions(upgrades);
    public override void UpdateUpgradeType() => UpdateUpgradeType(upgrades);

    public override bool CheckCanUpgrade(PlayerStateController controller)
      => CheckCanUpgrade(controller, upgrades);


#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        if (animationClip != null)
        {
            fullFrame = (int)(animationClip.length * animationClip.frameRate);
        }

        if (upgrades.Length > 0)
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].UpgradeName = "Upgrade " + (i + 1);
                upgrades[i].SkillLevel = (i + 1);
            }
        }

        if (!Application.isPlaying)
        {
            if (skillType != SkillType.BUFF)
                skillType = SkillType.BUFF;
          
        }
    }
#endif
}
