using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill Data/Combo Skill Data", fileName = "SCB_")]
public class ComboSkillClip : BaseSkillClip
{
    [SerializeField] private List<ComboData> comboDatas = new List<ComboData>();
    //콤보 스킬 같은 경우엔, ComboClip에 Upgrade[] 가 구현되있음.
  
    [Header("Player용 SKill Upgrade")]
    public ComboSkillUpgrade[] upgrades;

    public List<ComboData> ComboDatas => comboDatas;

    public ComboSkillClip() : base() { }
    public ComboSkillClip(BaseSkillClip copyClip) : base(copyClip)
    {
        if (copyClip is ComboSkillClip)
        {
            ComboSkillClip clone = copyClip as ComboSkillClip;
            skillType = SkillType.COMBO;
            CloneComboDatas(clone);
            upgrades = clone.upgrades;
        }
    }

    public void CloneComboDatas(ComboSkillClip comboSkillClip)
    {
        for (int i = 0; i < comboSkillClip.comboDatas.Count; i++)
        {
            ComboData data = new ComboData();
            data.inputs = comboSkillClip.comboDatas[i].inputs;
            data.comboInput = comboSkillClip.comboDatas[i].comboInput;
            data.comboClip = Instantiate(comboSkillClip.comboDatas[i].comboClip);
            comboDatas.Add(data);
        }
    }

    public override void LoadSkill(PlayerStateController controller)
    {
        if (skillState == CurrentSkillState.LOCK || upgrades == null || upgrades.Length < currentSkillIndex) return;

        ApplySkillInfo(controller, upgrades[currentSkillIndex]);
    }
    public override void UpgradeSkill(PlayerStateController controller, bool isOwnSkill = false)
            => UpgradeSkill<ComboSkillUpgrade>(controller, upgrades, isOwnSkill);



    public override void UseRequipedSkill(PlayerSkillController skillController)
    {
        if (skillType != SkillType.COMBO) return;
        ComboSkillClip[] clips = skillController.GetOwnSkillTypes<ComboSkillClip>();
        foreach (ComboSkillClip clip in clips)
            if (clip.skillState == CurrentSkillState.USE)
                clip.skillState = CurrentSkillState.ACTIVE;

        this.skillState = CurrentSkillState.USE;
    }
    public override int GetNextRequireLv() => NextRequireLv(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireMoney() => NextRequireMoney(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireSkillPoint() => NextRequireSkillPoint(upgrades, currentSkillIndex + 1);

    protected override void ApplySkillInfo(PlayerStateController controller, SkillUpgrade skillUpgrade, bool isLoadSkill = false)
    {
        base.ApplySkillInfo(controller, skillUpgrade, isLoadSkill);

        if (!(skillUpgrade is ComboSkillUpgrade))
            return;
        ComboSkillUpgrade upgrade = skillUpgrade as ComboSkillUpgrade;

        for (int i = 0; i < comboDatas.Count; i++)
            comboDatas[i].comboClip.Upgrade(currentSkillIndex);
    }

    public override string[] GetConditionDescriptions() => GetConditionDescriptions<ComboSkillUpgrade>(upgrades);
    public override void UpdateUpgradeType() => UpdateUpgradeType(upgrades);
    public override bool CheckCanUpgrade(PlayerStateController controller) => CheckCanUpgrade(controller, upgrades);




#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (skillType != SkillType.COMBO)
            skillType = SkillType.COMBO;

        if (upgrades.Length > 0)
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].UpgradeName = "Upgrade " + (i + 1);
                upgrades[i].SkillLevel = (i + 1);
            }
        }

        for (int i = 0; i < comboDatas.Count; i++)
        {
            if (comboDatas[i].comboClip != null)
            {
                comboDatas[i].inputs = new List<ComboMouseInputType>(comboDatas[i].comboClip.comboInputKey);
                comboDatas[i].comboInput = new List<int>(comboDatas[i].comboClip.GetInputKeyInt());
            }
        }
    }
#endif
}
