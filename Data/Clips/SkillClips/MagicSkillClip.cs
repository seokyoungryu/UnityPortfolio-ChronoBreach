using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagicType
{
    NONE = -1,
    THROWING = 0,
    TARGETING_MULTIPLE = 1,
    TARGETING_SINGLE = 2,
    RUSH = 3,
}

[CreateAssetMenu(menuName = "Data/Skill Data/Magic Skill Data", fileName = "SM_")]
public class MagicSkillClip : BaseSkillClip
{
    [Space(10)]
    public SoundList[] castingSound = null;

    [Header("Property")]
    public float canExcuteDistance = 5f;
    public int rotatingEndFrame = 0;  
    public bool eachTargetEffect = false;
    public TimeData excuteTimeData = null;

    [Header("Animation")]
    public string skillAnimationName = string.Empty;
    public AnimationClip skillAnimationClip;
    public int fullFrame = 0;
    public float endFrame = 0f;

    [Header("Animation Setting")]
    public float animationSpeed = 1f;
     public List<MagicProjectileInfo> createProjectileInfos = new List<MagicProjectileInfo>();

    [Header("Player용 SKill Upgrade")]
    public MagicSkillUpgrade[] upgrades;


    public List<MagicProjectileInfo> Infos => createProjectileInfos;

    public MagicSkillClip() : base() { }
    public MagicSkillClip(BaseSkillClip copyClip) : base(copyClip)
    {
        if (copyClip is MagicSkillClip)
        {
            MagicSkillClip clone = copyClip as MagicSkillClip;
            skillType = SkillType.MAGIC;
            castingSound = clone.castingSound;
            rotatingEndFrame = clone.rotatingEndFrame;
            skillAnimationName = clone.skillAnimationName;
            skillAnimationClip = clone.skillAnimationClip;
            fullFrame = clone.fullFrame;
            endFrame = clone.endFrame;
            animationSpeed = clone.animationSpeed;
            createProjectileInfos = clone.createProjectileInfos;
            canExcuteDistance = clone.canExcuteDistance;
            upgrades = clone.upgrades;
            eachTargetEffect = clone.eachTargetEffect;
            excuteTimeData = clone.excuteTimeData;
        }

    }

    public float GetEndTime()
    {
        if (skillAnimationName == string.Empty)
            return 0;
        else
            return endFrame * (1f / (skillAnimationClip.frameRate * animationSpeed));
    }

    public float GetRotatingWhenCastTime()
    {
        if (rotatingEndFrame <= 0f)
            return 0;
        else
            return rotatingEndFrame * (1f / (skillAnimationClip.frameRate * animationSpeed));
    }


    public override void LoadSkill(PlayerStateController controller) => LoadSkill(controller, upgrades);
    public override string[] GetConditionDescriptions() => GetConditionDescriptions(upgrades);
    public override void UpdateUpgradeType() => UpdateUpgradeType(upgrades);

    public override void UpgradeSkill(PlayerStateController controller, bool isOwnSkill = false)
    => UpgradeSkill(controller, upgrades, isOwnSkill);
    public override int GetNextRequireLv() => NextRequireLv(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireMoney() => NextRequireMoney(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireSkillPoint() => NextRequireSkillPoint(upgrades, currentSkillIndex + 1);

    protected override void ApplySkillInfo(PlayerStateController controller, SkillUpgrade skillUpgrade, bool isLoadSkill = false)
    {
        base.ApplySkillInfo(controller, skillUpgrade, isLoadSkill);

        if (!(skillUpgrade is MagicSkillUpgrade))
            return;
        MagicSkillUpgrade upgrade = skillUpgrade as MagicSkillUpgrade;

        skillStaminaCost = upgrade.SkillInfo.SpCost;
        skillCoolTime = upgrade.SkillInfo.CoolTime;
        canExcuteDistance = upgrade.SkillInfo.CanExcuteDistance;
        createProjectileInfos = new List<MagicProjectileInfo>(upgrade.SkillInfo.GetProjectileCreator);

    }


    public override bool CheckCanUpgrade(PlayerStateController controller)
          => CheckCanUpgrade(controller, upgrades);


#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if(!Application.isPlaying)
        {
            if (skillType != SkillType.MAGIC)
                skillType = SkillType.MAGIC;

            if (skillAnimationName == string.Empty)
                skillAnimationName = "Skill";

            if (skillAnimationClip)
            {
                fullFrame = (int)(skillAnimationClip.length * skillAnimationClip.frameRate);
                if (endFrame == 0f)
                    endFrame = fullFrame;

            }
            else
                fullFrame = 0;


            if (upgrades.Length > 0)
            {
                for (int i = 0; i < upgrades.Length; i++)
                {
                    upgrades[i].UpgradeName = "Upgrade " + (i + 1);
                    upgrades[i].SkillLevel = (i + 1);
                }
            }

        }
    }
#endif
}

