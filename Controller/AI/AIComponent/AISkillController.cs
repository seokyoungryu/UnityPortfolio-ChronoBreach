using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkillController : SkillController
{
    [Header("AI 소유 스킬 등록")]
    [SerializeField] private OwnSkillDatabase ownSkillDatabase = null;

    [Header("Enemy 전용")]
    [SerializeField] private PhaseInfo phaseSkills;


    [SerializeField]  private List<SkillData> getRandomSkillList = new List<SkillData>();  //test용 private하기
    private AIController aiController;
    private bool checkCanExcuteSkill = false;

    protected override void Awake()
    {
        base.Awake();
        aiController = GetComponent<AIController>();
    }

    public override void CheckEnableBuffTime(BaseController controller) => base.CheckEnableBuffTime(aiController);
    public override void CheckEnableDeBuffTime(BaseController controller) => base.CheckEnableDeBuffTime(aiController);
    protected override void CreateOwnSkill()
    {
        if (ownSkillDatabase == null || ownSkillDatabase.OwnSkills.Count <= 0) return;

        for (int i = 0; i < ownSkillDatabase.OwnSkills.Count; i++)
            ownSkills.Add(MakeClipToCloneSkillData(ownSkillDatabase.OwnSkills[i]));
    }

    public override SkillData GetNotCoolTimeSkill()
    {
        getRandomSkillList.Clear();
        foreach (SkillData data in ownSkills)
            if (!data.isCoolTime)
                getRandomSkillList.Add(data);

        if (getRandomSkillList.Count <= 0) return null;
        int randomIndex = Random.Range(0, getRandomSkillList.Count-1);
        return getRandomSkillList[randomIndex];
    }


    public SkillData GetCanUseSkillData()
    {
        getRandomSkillList.Clear();
        foreach (SkillData data in ownSkills)
        {
            checkCanExcuteSkill = true;
            if (!data.isCoolTime)
            {
                if (data.skillClip.Conditions.Count > 0)
                    for (int i = 0; i < data.skillClip.Conditions.Count; i++)
                        if (!data.skillClip.Conditions[i].CanExcuteCondition(aiController))
                            checkCanExcuteSkill = false;

                if (checkCanExcuteSkill)
                {
                    Debug.Log("Can : " + data.skillClip.codeName);
                    getRandomSkillList.Add(data);
                }
                else Debug.Log("Cant : " + data.skillClip.codeName);

            }
        }

        if (getRandomSkillList.Count <= 0) return null;
        int randomIndex = Random.Range(0, getRandomSkillList.Count - 1);
        return getRandomSkillList[randomIndex];
    }

    /// <summary>
    /// 페이즈시 스킬 잠금 해제
    /// </summary>
    public void UnlockPhaseSkill(int phaseCount)
    {
        if (phaseSkills == null && phaseSkills.phaseInfos.Count <= 0) return;

        foreach (AIPhaseAttackData phaseData in phaseSkills.phaseInfos)
        {
            if (phaseCount == phaseData.phaseCount)
                foreach (BaseSkillClip clip in phaseData.unlockPhaseSkills)
                    ownSkills.Add(MakeClipToCloneSkillData(clip));
        }
    }

    public void ApplyPhaseUseableObjs(int phaseCount)
    {
        if (phaseSkills == null && phaseSkills.phaseInfos.Count <= 0) return;

        foreach (AIPhaseAttackData phaseData in phaseSkills.phaseInfos)
        {
            if (phaseCount == phaseData.phaseCount)
            {
                for (int i = 0; i < phaseData.applyUseableObjs.Length; i++)
                {
                    phaseData.applyUseableObjs[i].Apply(aiController);
                }
            }
        }
    }

    public AIPhaseAttackData GetPhaseData(int phaseCount)
    {
        if (phaseSkills == null || phaseSkills.phaseInfos.Count <= 0) return null;

        foreach (AIPhaseAttackData phaseData in phaseSkills.phaseInfos)
            if (phaseCount == phaseData.phaseCount)
                return phaseData;
        return null;
    }

}
