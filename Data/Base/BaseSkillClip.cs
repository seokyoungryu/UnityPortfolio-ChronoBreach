using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum CurrentSkillState
{
    LOCK = 0,
    ACTIVE = 1,
    USE =2,
}

public enum BeforeStartSkillMotionType
{
    NOT_USED  = 0,
    ABSOLUTE_EXCUTE = 1,
    WHEN_TARGET_CLOSE = 2,
    WHEN_TARGET_FAR = 3,
    RANDOM = 4,
}

public abstract class BaseSkillClip : BaseScriptableObject
{
    [Header("생성시 SKill Database에 등록.")]
    [Header("Skill Genaral Settings")]
    public string codeName = string.Empty;
    public string displayName = string.Empty;
    public SkillType skillType = SkillType.NONE;
    public EffectList skill_Prefab = EffectList.None;
    public Sprite icon = null;

    [TextArea(1, 3)]
    public string description = string.Empty;

    [Header("Camera Shake ")]
    [SerializeField] private CameraShakeInfo[] cameraInfo;

    [Header("Skill Lv Settings")]
    public CurrentSkillState skillState = CurrentSkillState.LOCK;
    public SkillUpgradeType currentSkillUpgradeType = SkillUpgradeType.LOCK;
    public int currentSkillIndex = -1;
    public int maxSkillLevel = 0;
    public int currentSkillLevel = 0;

    [Header("Skill Property")]
    public bool startRotateToTarget = true;   //anim 시작할때 타겟한테 회전할지.
    public float skillCoolTime = 0f;
    public float skillStaminaCost = 0f;
    public int maxTargetCount = 0;
    [Header("AI용 ( 거리, 각도 상관없이 실행 )")]
    public bool absoluteExcute = false;
    private float frame = 0f;

    [Header("스킬 실행 조건")]
    [SerializeField] private List<BaseConditionInfo> conditions;

    [Header("스킬 사용전 모션 세팅")]
    public BeforeSkillMotionInfo beforeSkillMotionInfo;

    [Header("스킬 사용시 Voice 랜덤 사운드")]
    public SoundList[] randomVoiceSound;


    public List<BaseConditionInfo> Conditions => conditions;

    public SkillUpgradeType CurrentSkillUpgradeType => currentSkillUpgradeType;
    public CameraShakeInfo[] CameraInfo => cameraInfo;

    public BaseSkillClip() { id = -1; }
    public BaseSkillClip(BaseSkillClip copyClip)
    {
        id = copyClip.id;
        codeName = copyClip.codeName;
        displayName = copyClip.displayName;
        skillType = copyClip.skillType;
        skill_Prefab = copyClip.skill_Prefab;
        icon = copyClip.icon;
        description = copyClip.description;
        startRotateToTarget = copyClip.startRotateToTarget;
        skillStaminaCost = copyClip.skillStaminaCost;
        skillCoolTime = copyClip.skillCoolTime;
        currentSkillIndex = copyClip.currentSkillIndex;
        maxSkillLevel = copyClip.maxSkillLevel;
        currentSkillLevel = copyClip.currentSkillLevel;
        skillState = copyClip.skillState;
        maxTargetCount = copyClip.maxTargetCount;
        currentSkillUpgradeType = copyClip.currentSkillUpgradeType;
        beforeSkillMotionInfo = copyClip.beforeSkillMotionInfo;
        cameraInfo = copyClip.cameraInfo;

        List<BaseConditionInfo> conditionClones = new List<BaseConditionInfo>();
        for (int i = 0; i < copyClip.conditions.Count; i++)
            conditionClones.Add(Instantiate(copyClip.Conditions[i]));

        conditions = conditionClones;

        InitCopyClip(copyClip);
    }

    public virtual void InitCopyClip(BaseSkillClip copyClip) { }

    public abstract int GetNextRequireLv();
    public abstract int GetNextRequireMoney();
    public abstract int GetNextRequireSkillPoint();

    public int NextRequireLv(SkillUpgrade[] upgrades, int index)
    {
        if (index > (upgrades.Length - 1)) return upgrades[upgrades.Length - 1].RequirePlayerLevel;
        return upgrades[index].RequirePlayerLevel;
    }
    public int NextRequireMoney(SkillUpgrade[] upgrades, int index)
    {
        if (index > (upgrades.Length - 1)) return upgrades[upgrades.Length - 1].RequireUpgradeCost;
        return upgrades[index].RequireUpgradeCost;
    }
    public int NextRequireSkillPoint(SkillUpgrade[] upgrades, int index)
    {
        if (index > (upgrades.Length - 1)) return upgrades[upgrades.Length - 1].RequireSkillPoint;
        return upgrades[index].RequireSkillPoint;
    }

    public float GetFrameToTime(float animationClipFrame,float animFrameRate ,float speed = 1f)
    {
        frame = 1f / (animFrameRate * speed);
        return animationClipFrame * frame;

    }

    public virtual void UseRequipedSkill(PlayerSkillController skillController) { }


    public abstract bool CheckCanUpgrade(PlayerStateController controller);
    public bool CheckCanUpgrade<T>(PlayerStateController controller, T[] upgrades) where T : SkillUpgrade
    {
        Debug.Log("33333");
        if (upgrades == null || upgrades.Length <= 0 || upgrades.Length <= (currentSkillIndex+1) || controller == null)
            return false;
        Debug.Log("44444");

        return CanUpgrade(controller, upgrades[currentSkillIndex+1], upgrades[currentSkillIndex+1].RequiredSkillData);
    }



    public abstract void UpdateUpgradeType();
    public void UpdateUpgradeType(SkillUpgrade[] upgrades)
    {
        if (skillState == CurrentSkillState.LOCK || currentSkillIndex == -1)
            currentSkillUpgradeType = SkillUpgradeType.LOCK;
        else if (upgrades.Length > (currentSkillIndex + 1))  //currentSkillLevel < maxSkillLevel || 
            currentSkillUpgradeType = SkillUpgradeType.UPGRADE;
        else if (upgrades == null || upgrades.Length <= 0 || upgrades.Length == (currentSkillIndex + 1) || currentSkillLevel == maxSkillLevel)
            currentSkillUpgradeType = SkillUpgradeType.DONE;
    }



    public abstract void LoadSkill(PlayerStateController controller);
    protected void LoadSkill<T>(PlayerStateController controller, T[] upgrades) where T : SkillUpgrade
    {
        if (skillState == CurrentSkillState.LOCK || upgrades == null || upgrades.Length < currentSkillIndex) return;

        if (upgrades.Length >= currentSkillIndex && upgrades[currentSkillIndex] != null)
            Debug.Log("있음 : " + displayName);


        if (upgrades.Length >= currentSkillIndex)
            ApplySkillInfo(controller, upgrades[currentSkillIndex], true);
    }



    public abstract void UpgradeSkill(PlayerStateController controller, bool isOwnSkill = false);
    protected void UpgradeSkill<T>(PlayerStateController controller, T[] upgrades, bool isOwnSkill = false) where T : SkillUpgrade
    {
        if (isOwnSkill || skillState == CurrentSkillState.LOCK)
        {
            Upgrade(controller, upgrades[0]);
        }
        else if (!isOwnSkill)
        {
            Upgrade(controller, upgrades[currentSkillIndex + 1]);
        }

    }



    protected void Upgrade<T>(PlayerStateController controller, T upgrade) where T : SkillUpgrade
    {
        if (skillState == CurrentSkillState.LOCK)
        {
            SoundManager.Instance.PlayUISound(UISoundType.SKILL_LOCK_ON);
            skillState = CurrentSkillState.ACTIVE;
        }
        currentSkillIndex++;
        currentSkillLevel = upgrade.SkillLevel;
        SoundManager.Instance.PlayUISound(UISoundType.SKILL_UPGRADE);
        if (!GameManager.Instance.ignoreSkillCondition)
        {
            controller.playerStats.UseSkillPoint(upgrade.RequireSkillPoint);
            GameManager.Instance.SetMinusOwnMoney(upgrade.RequireUpgradeCost);
        }

        ApplySkillInfo(controller, upgrade);
        UpdateUpgradeType();
    }

    protected virtual void ApplySkillInfo(PlayerStateController controller, SkillUpgrade skillUpgrade, bool isLoadSkill = false)
    {
        currentSkillLevel = skillUpgrade.SkillLevel;
        description = skillUpgrade.Description;
    }


    public abstract string[] GetConditionDescriptions();
    public string[] GetConditionDescriptions<T>(T[] upgrades) where T : SkillUpgrade
    {
        if (upgrades == null || upgrades.Length <= 0) return null;

        if (upgrades.Length > (currentSkillIndex+1))
        {
            Debug.Log(displayName + " : " + currentSkillIndex +"(" + (currentSkillIndex+1) + ")" + "/" + upgrades.Length + " : ");
            return ConditionDescription(upgrades[currentSkillIndex+1], upgrades[currentSkillIndex+1].RequiredSkillData);
        }
        else
            return null;
    }



    protected bool CanUpgrade(PlayerStateController controller, SkillUpgrade upgrade, SkillUpgradeContainer[] containers)
    {
        if (upgrade == null || currentSkillLevel >= maxSkillLevel) return false;

        if (GameManager.Instance.ignoreSkillCondition)
            return true;

        if (controller.playerStats.RemainingSkillPoint <= 0)
            return false;

        if (CheckRequiredPlayerLv(controller.playerStats.Level, upgrade.RequirePlayerLevel)
        && CheckRequiredUpgradeCost(GameManager.Instance.OwnMoney, upgrade.RequireUpgradeCost)
        && CheckUpgradeContainer(controller.skillController, containers)
        && CheckRequiredUpgradeSkillPoint(controller.playerStats.RemainingSkillPoint, upgrade.RequireSkillPoint))
        {
            Debug.Log("업글가능 : " + controller.playerStats.RemainingSkillPoint + " / " + upgrade.RequireSkillPoint);
            return true;
        }
        Debug.Log("업글 불가능" + controller.playerStats.RemainingSkillPoint + " / " + upgrade.RequireSkillPoint);
        return false;
    }


    protected string[] ConditionDescription(SkillUpgrade skillUpgrade, SkillUpgradeContainer[] container)
    {
        List<string> description = new List<string>();

        if (skillUpgrade.RequirePlayerLevel > 0)
            description.Add("필요 레벨 " + skillUpgrade.RequirePlayerLevel.ToString());
        if (skillUpgrade.RequireUpgradeCost > 0)
            description.Add("필요 금액 " + skillUpgrade.RequireUpgradeCost.ToString());
        if (skillUpgrade.RequireSkillPoint > 0)
            description.Add("필요 스킬포인트 " + skillUpgrade.RequireSkillPoint.ToString());


        if (container.Length <= 0 || container == null)
            return description.ToArray();

        for (int i = 0; i < container.Length; i++)
            description.Add(container[i].Description);

        return description.ToArray();
    }


    private bool CheckRequiredPlayerLv(int playerLv, int requiredLv)
    {
        if (playerLv >= requiredLv || requiredLv == 0) return true;
        else return false;
    }    

    private bool CheckRequiredUpgradeCost(int ownMoney, int requiredCost)
    {
        if (ownMoney >= requiredCost || requiredCost == 0) return true;
        else return false;
    }

    private bool CheckRequiredUpgradeSkillPoint(int skillPoint, int requiredSkillPoint)
    {
        if (skillPoint >= requiredSkillPoint || requiredSkillPoint == 0) return true;
        else return false;
    }


    private bool CheckUpgradeContainer(PlayerSkillController skillController , SkillUpgradeContainer[] containers)
    {
        if (containers.Length <= 0 || containers == null ) return true;

        bool canUpgrade = true;
        for (int i = 0; i < containers.Length; i++)
        {
            if (containers[i] == null || containers[i].ConditionSkillClip == null) continue;
            BaseSkillClip ownClip = skillController.GetSkilData(containers[i].ConditionSkillClip.id)?.skillClip;
            if (ownClip == null)
                ownClip = skillController.SkillDatabase.GetSkillOrigin(containers[i].ConditionSkillClip.id);

            if (ownClip.currentSkillIndex < containers[i].RequiredSkillLevel)
                canUpgrade = false;
        }
        return canUpgrade;
    }

#if UNITY_EDITOR
    [ContextMenu("Set Dirty")]
    protected void SetDirtySKillClip()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }


    protected virtual void OnValidate()
    {
        EditorUtility.SetDirty(this);
        if(beforeSkillMotionInfo.beforeMotionClip)
        {
            beforeSkillMotionInfo.beforeMotionFullFrame = (int)(beforeSkillMotionInfo.beforeMotionClip.length * beforeSkillMotionInfo.beforeMotionClip.frameRate);
        }

    }
#endif


}


