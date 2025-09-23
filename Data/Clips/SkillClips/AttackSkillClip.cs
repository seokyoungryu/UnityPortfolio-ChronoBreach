using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Data/Skill Data/Attack Skill Data", fileName ="SA_")]
public class AttackSkillClip : BaseSkillClip
{
    [Header("EFfect Settings") , Space(10)]
    [SerializeField, HideInInspector] private List<bool> isHitRandom = new List<bool>();
    [SerializeField, HideInInspector] private List<RandomEffectInfo> hitEffects = new List<RandomEffectInfo>();
    [SerializeField, HideInInspector] private List<ControllerEffectInfo> attackEffects = new List<ControllerEffectInfo>();
    [SerializeField, HideInInspector] private List<TimeData> timeDatas = new List<TimeData>();
   [HideInInspector] public RandomEffectInfo allHitEffect = new RandomEffectInfo();
   [HideInInspector] public ControllerEffectInfo allAttackEffect = new ControllerEffectInfo();
   [HideInInspector] public bool allHitRandom = false;
   [HideInInspector] public AttackStrengthType allAttackStrength = AttackStrengthType.WEAK;


    [SerializeField, HideInInspector] private List<AttackStrengthType> strengthType = new List<AttackStrengthType>();

    //밑에 private로만 하기. 시리얼 삭제하기 -> 시험용. 
    [SerializeField, HideInInspector] private float targetDetectRange = 7f;
    [SerializeField, HideInInspector] private List<float> attackDamage = new List<float>();
    [SerializeField, HideInInspector] private List<float> attackRange = new List<float>();
    [SerializeField, HideInInspector] private List<float> attackAngle = new List<float>();


   // [Header("On Trigger Setting")]
   // //점프하거나 할떄 플레이어가 지나갈수 있게 하려고한건데.. 생각해보니
   // // 그러면 데미지 입히는게 불가능하지않나 
   // // 확인해보니 데미지는 들어감. 콜라이더로 출동하는게 아닌, physics로 탐색해서 그런가봄
   // // 다만 문제는 거리가 0 일경우 데미지가 안입힘.
   // // 이부분 음.. 
   // public bool onTriggerDuringAnimation = false;
   // public int triggerStartFrame = 0;
   // public int triggerEndFrame = 0;

    [Header("Animation Settings")]
    private float skillAnimSpeed = 1f;
    [SerializeField, HideInInspector] private AnimationClip animationClip = null;
    [SerializeField, HideInInspector] private string animationClipName = "Skill";
    [SerializeField, HideInInspector] private List<int> animationAttackTimingFrame = new List<int>();
    [Tooltip("AI용 공격하기전 회전 프레임")]
    [SerializeField,HideInInspector] private List<int> animationRotateFrames = new List<int>();
    [SerializeField,HideInInspector] private int animationEndFrame = 0;
    [SerializeField, HideInInspector] private float clipFullFrame = 0;


    [Header("Player용 SKill Upgrade")]
    public AttackSkillUpgrade[] upgrades;


    public List<bool> IsHitRandom { get { return isHitRandom; } set { isHitRandom = value; } }
   
    public List<AttackStrengthType> StrengthType { get { return strengthType; } set { strengthType = value; } }
    public List<RandomEffectInfo> HitEffects { get { return hitEffects; } set { hitEffects = value; } }
    public List<ControllerEffectInfo> AttackEffects { get { return attackEffects; } set { attackEffects = value; } }

    public float TargetDetectRange { get { return targetDetectRange; } set { targetDetectRange = value; } }
    public List<float> AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
    public List<float> AttackRange { get { return attackRange; } set { attackRange = value; } }
    public List<float> AttackAngle { get { return attackAngle; } set { attackAngle = value; } }
    public List<TimeData> TimeDatas { get { return timeDatas; } set { timeDatas = value; } }

    public AnimationClip AnimationClip { get { return animationClip; } set { animationClip = value; } }
    public string AnimationClipName { get { return animationClipName; } set { animationClipName = value; } }
    public List<int> AnimationRotateFrames { get { return animationRotateFrames; } set { animationRotateFrames = value; } }
    public List<int> AnimationAttackTimingFrame { get { return animationAttackTimingFrame; } set { animationAttackTimingFrame = value; } }
    public int AnimationEndFrame { get { return animationEndFrame; } set { animationEndFrame = value; } }
    public float ClipFullFrame { get { return clipFullFrame; } set { clipFullFrame = value; } }
    public float SkillAnimSpeed { get { return skillAnimSpeed; } set { skillAnimSpeed = value; } }

    public AttackSkillClip() : base() { }
    public AttackSkillClip(BaseSkillClip copyClip) : base(copyClip)
    {
        if (copyClip is AttackSkillClip)
        {
            AttackSkillClip clone = copyClip as AttackSkillClip;
            skillType = SkillType.ATTACK;
            hitEffects = new List<RandomEffectInfo>(clone.hitEffects); 
            attackEffects = new List<ControllerEffectInfo>(clone.attackEffects);
            attackDamage = new List<float>(clone.attackDamage);
            attackRange = new List<float>(clone.attackRange);
            attackAngle = new List<float>(clone.attackAngle);
            skillAnimSpeed = clone.skillAnimSpeed;
            targetDetectRange = clone.targetDetectRange;
            animationClipName = clone.animationClipName;
            animationClip = clone.animationClip;
            timeDatas = clone.timeDatas;
            clipFullFrame = clone.clipFullFrame;
            animationAttackTimingFrame = new List<int>(clone.animationAttackTimingFrame);
            animationEndFrame = clone.animationEndFrame;
            animationRotateFrames = new List<int>(clone.animationRotateFrames);
            StrengthType = new List<AttackStrengthType>(clone.StrengthType);
            upgrades = clone.upgrades;
        }
    }

    public override void LoadSkill(PlayerStateController controller) => LoadSkill(controller, upgrades);
    public override void UpdateUpgradeType() => UpdateUpgradeType(upgrades);
    public override bool CheckCanUpgrade(PlayerStateController controller)
        => CheckCanUpgrade(controller, upgrades);


    public override void UpgradeSkill(PlayerStateController controller, bool isOwnSkill = false)
        => UpgradeSkill(controller, upgrades, isOwnSkill);

    public override string[] GetConditionDescriptions()
        => GetConditionDescriptions(upgrades);          // null들이 원래 upgrade1 들어가는 자리.


    public override int GetNextRequireLv() => NextRequireLv(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireMoney() => NextRequireMoney(upgrades, currentSkillIndex + 1);
    public override int GetNextRequireSkillPoint() => NextRequireSkillPoint(upgrades, currentSkillIndex + 1);

    protected override void ApplySkillInfo(PlayerStateController controller, SkillUpgrade skillUpgrade, bool isLoadSkill = false)
    {
        base.ApplySkillInfo(controller, skillUpgrade, isLoadSkill);
        if (!(skillUpgrade is AttackSkillUpgrade))
            return;

        AttackSkillUpgrade upgrade = skillUpgrade as AttackSkillUpgrade;
        this.AttackDamage = upgrade.SkillInfo.AttackDamage;
        this.AttackAngle = upgrade.SkillInfo.AttackAngle;
        this.AttackRange = upgrade.SkillInfo.AttackRange;
        this.skillStaminaCost = upgrade.SkillInfo.SkillSpCost;
        this.skillCoolTime = upgrade.SkillInfo.SkillCoolTime;
        this.maxTargetCount = upgrade.SkillInfo.MaxTargetCount;
    }

    [ContextMenu("T")]
    public void Tes()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            for (int x = 0; x < upgrades[i].SkillInfo.AttackRange.Count; x++)
            {
                //upgrades[i].SkillInfo.AttackRange[x] = 10;
            }
        }
    }


#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        if (Application.isPlaying) return;

        if (skillType != SkillType.ATTACK)
            skillType = SkillType.ATTACK;

        if (upgrades.Length == 0)
            upgrades = new AttackSkillUpgrade[1] { new AttackSkillUpgrade() };

        if (animationClip != null)
        {
            clipFullFrame = animationClip.length * animationClip.frameRate;
            if (animationEndFrame <= 0f)
                animationEndFrame = (int)clipFullFrame;
        }

        if(attackEffects.Count > 0)
        {
            for (int i = 0; i < attackEffects.Count; i++)
            {
            }
        }

        if (attackDamage.Count > 0)
        {
            for (int i = 0; i < attackDamage.Count; i++)
            {
                if (isHitRandom.Count != attackDamage.Count)
                    isHitRandom.Add(true);
                if (hitEffects.Count != attackDamage.Count)
                    hitEffects.Add(new RandomEffectInfo()); 
                if (attackEffects.Count != attackDamage.Count)
                    attackEffects.Add(new ControllerEffectInfo());
            }

        }

        if ((maxSkillLevel == 0 | maxSkillLevel == -1) && upgrades.Length > 0)
            maxSkillLevel = 1;


        if (upgrades.Length > 0)
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].UpgradeName = "Upgrade " + (i + 1);
                upgrades[i].SkillLevel = (i + 1);
            }

            if (upgrades[0] == null) return;

            upgrades[0].SkillInfo.AttackDamage = attackDamage;
            upgrades[0].SkillInfo.AttackAngle = attackAngle;
            upgrades[0].SkillInfo.AttackRange = attackRange;

        }
    }
#endif


}

