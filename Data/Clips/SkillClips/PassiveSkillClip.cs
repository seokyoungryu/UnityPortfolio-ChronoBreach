using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill Data/Passive Skill Data", fileName = "SP_")]
public class PassiveSkillClip : BaseSkillClip
{
    [SerializeField] private BaseStatsObject[] statsObject = null;


    [Header("Player¿ë SKill Upgrade")]
    public PassiveSkillUpgrade[] upgrades;


    public PassiveSkillClip() : base() { }
    public PassiveSkillClip(BaseSkillClip clip) : base(clip)
    {
        if(clip is PassiveSkillClip)
        {
            PassiveSkillClip clone = clip as PassiveSkillClip;
            skillType = SkillType.PASSIVE;
            statsObject = clone.statsObject;
            upgrades = clone.upgrades;
        }
    }

    public override void LoadSkill(PlayerStateController controller) => LoadSkill(controller, upgrades);
    public override void UpdateUpgradeType() => UpdateUpgradeType(upgrades);

    public override void UpgradeSkill(PlayerStateController controller, bool isOwnSkill = false)
      => UpgradeSkill(controller, upgrades, isOwnSkill);

    public override string[] GetConditionDescriptions()
    => GetConditionDescriptions(upgrades);

    public override bool CheckCanUpgrade(PlayerStateController controller)
       => CheckCanUpgrade(controller, upgrades);
    public override int GetNextRequireLv() => NextRequireLv(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireMoney() => NextRequireMoney(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireSkillPoint() => NextRequireSkillPoint(upgrades, currentSkillIndex + 1);

    protected override void ApplySkillInfo(PlayerStateController controller, SkillUpgrade skillUpgrade, bool isLoadSkill = false)
    {
        base.ApplySkillInfo(controller, skillUpgrade, isLoadSkill);

        if (!(skillUpgrade is PassiveSkillUpgrade))
            return;
        PassiveSkillUpgrade upgrade = skillUpgrade as PassiveSkillUpgrade;
        statsObject = upgrade.SkillInfo.UpgradeStatsObjects;

        if (!isLoadSkill)
            for (int i = 0; i < statsObject.Length; i++)
                statsObject[i].Apply(controller);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (!Application.isPlaying)
        {
            if (skillType != SkillType.PASSIVE)
                skillType = SkillType.PASSIVE;

        }

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
