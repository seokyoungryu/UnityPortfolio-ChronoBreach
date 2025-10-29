using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CounterSkillBuffTimingType
{
    NONE =-1,
    SUCCESSED_COUNTER = 0,  //카운터 성공 시  ex) hp 100 회복 buffObject(heal)
    COUNTER_ATTACK =1, // 카운터 공격 시
    SUCCESSCOUNT= 2,  //카운터 성공 횟수 시.
}

[CreateAssetMenu(menuName = "Data/Skill Data/Counter Skill Data", fileName = "SCT_")]
public class CounterSkillClip : BaseSkillClip
{

    [Header("Sounds")]
    public SoundList entrySound;

    [Header("Clips")]
    public ComboClip counterAttackClip = null;
    public CounterDefenseClip counterDefenseClip = null;
    public CounterRangeReflectClip counterRangeReflectClip = null;

    [Header("등록 Anim Clip 보기용")]
    public AnimationClip meleeAnimationClip = null;
    public float meleeAnimSpeed = 1f;
    public AnimationClip rangeAnimationClip = null;
    public float rangeAnimSpeed = 1f;

    [Header("Player전용 스킬 잠금해제 조건")]
    public CounterSkillUpgrade condition;
    [Header("Player용 SKill Upgrade")]
    public CounterSkillUpgrade[] upgrades;



    public CounterSkillClip() : base() { }
    public CounterSkillClip(BaseSkillClip copyClip) : base(copyClip)
    {
        if (copyClip is CounterSkillClip)
        {
            CounterSkillClip clone = copyClip as CounterSkillClip;
            skillType = SkillType.COUNTER;
            counterAttackClip = CloneComboDatas(clone.counterAttackClip);
            counterDefenseClip = clone.counterDefenseClip;
            counterRangeReflectClip = CloneReflectClipDatas(clone.counterRangeReflectClip);
            meleeAnimationClip = clone.meleeAnimationClip;
            rangeAnimationClip = clone.rangeAnimationClip;
            meleeAnimSpeed = clone.meleeAnimSpeed;
            rangeAnimSpeed = clone.rangeAnimSpeed;
            condition = clone.condition;
            upgrades = clone.upgrades;
        }
    }

    public ComboClip CloneComboDatas(ComboClip comboClip)
    {
        return Instantiate(comboClip);
    }


    public CounterRangeReflectClip CloneReflectClipDatas(CounterRangeReflectClip reflectClip)
    {
        return new CounterRangeReflectClip(reflectClip);
    }

    public override void LoadSkill(PlayerStateController controller) => LoadSkill(controller, upgrades);
    public override void UpgradeSkill(PlayerStateController controller, bool isOwnSkill = false)
            => UpgradeSkill(controller, upgrades,  isOwnSkill);

    public override string[] GetConditionDescriptions() => GetConditionDescriptions(upgrades);


    public override void UpdateUpgradeType() => UpdateUpgradeType(upgrades);
    public override int GetNextRequireLv() => NextRequireLv(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireMoney() => NextRequireMoney(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireSkillPoint() => NextRequireSkillPoint(upgrades, currentSkillIndex + 1);


    protected override void ApplySkillInfo(PlayerStateController controller, SkillUpgrade skillUpgrade, bool isLoadSkill = false)
    {
        base.ApplySkillInfo(controller, skillUpgrade, isLoadSkill);

        if (!(skillUpgrade is CounterSkillUpgrade))
            return;
        
        CounterSkillUpgrade upgrade = skillUpgrade as CounterSkillUpgrade;

        counterAttackClip.Upgrade(currentSkillIndex);
        counterRangeReflectClip.Upgrade(currentSkillIndex);
    }
    public override void UseRequipedSkill(PlayerSkillController skillController)
    {
        if (skillType != SkillType.COUNTER) return;
        CounterSkillClip[] clips = skillController.GetOwnSkillTypes<CounterSkillClip>();
        foreach (CounterSkillClip clip in clips)
            if (clip.skillState == CurrentSkillState.USE)
                clip.skillState = CurrentSkillState.ACTIVE;

        this.skillState = CurrentSkillState.USE;
    }


    public override bool CheckCanUpgrade(PlayerStateController controller)
       => CheckCanUpgrade(controller, upgrades);


#if UNITY_EDITOR
    private void OnValidate()
    {
        base.OnValidate();
        if (counterAttackClip != null && counterAttackClip.animationClip != null)
            meleeAnimationClip = counterAttackClip.animationClip;
        if (counterRangeReflectClip != null && counterRangeReflectClip.reflectCounterAnim != null)
            rangeAnimationClip = counterRangeReflectClip.reflectCounterAnim;

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
