using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 하는일 : 1.스킬원본을 따로 복제(이 복제본이 게임상 플레이어 스킬 정보를 가짐)
///          2.버프 스킬 사용 및 스탯에 수정. -> 버프 쿨타임 검사 및 해제.
///          3.스킬 데이터 리턴 , 스킬 쿨타임 검사 및 해제
///          4.스킬 잠금여부와 활성화 여부 확인. 및 수정.
/// </summary>
public class SkillController : MonoBehaviour
{
    [SerializeField] protected SkillDatabase skillDatabase = null;
   
    [Header("스킬 목록")]
    [SerializeField] protected List<SkillData> ownSkills = new List<SkillData>(); // 스킬원본 복제품. 수정가능.
    [Header("스킬 쿨타임 목록")]
    [SerializeField] protected List<SkillData> coolTimeSkillList = new List<SkillData>();


    [Header("버프 목록")]
    [SerializeField] protected List<BuffData> currentEnableBuffs = new List<BuffData>();  
    [Header("디버프 목록")]
    [SerializeField] protected List<BuffData> currentEnableDebuff = new List<BuffData>();

    public SkillDatabase SkillDatabase => skillDatabase;
    private BaseController baseController;

    public int GetCurrCoolTimeCount => coolTimeSkillList.Count;

    protected virtual void Awake()
    {
        CreateOwnSkill();
        baseController = GetComponent<BaseController>();
    }

    protected virtual void CreateOwnSkill()
    {
    }

    protected SkillData MakeClipToCloneSkillData(BaseSkillClip clip)
    {
        SkillData skillData = new SkillData(clip);
        return skillData;
    }


    public void ResetSkillCoolTime()
    {
        if (coolTimeSkillList.Count <= 0)
            return;

        foreach (SkillData skill in coolTimeSkillList)
        {
            skill.isCoolTime = false;
            skill.SetCoolTimeByClip();
        }
        coolTimeSkillList.Clear();
    }

    public void CheckUpdateSkillCoolTime(bool isNoCooltime = false)
    {
        if (coolTimeSkillList.Count <= 0)
            return;

        foreach (SkillData skill in coolTimeSkillList)
        {
            skill.coolTime -= Time.deltaTime;
            if (skill.coolTime <= 0f || isNoCooltime)
            {
                skill.isCoolTime = false;
                skill.SetCoolTimeByClip();
                coolTimeSkillList.Remove(skill);
                return;
            }
        }

    }

    public int GetOwnSkillCount() => ownSkills.Count;

    public bool IsAllSkillCoolTime()
    {
        if (ownSkills.Count <= 0) return true;

        foreach (SkillData data in ownSkills)
        {
            if (data.isCoolTime == false)
                return false;
        }
        return true;
    }

    public bool GetThisSkillIsCoolTime(int skillID)
    {
        if (coolTimeSkillList.Count <= 0) return false;

        SkillData skillData = GetSkilData(skillID);
        if (skillData == null) return false;

        foreach (SkillData data in coolTimeSkillList)
        {
            if (data.skillClip.ID == skillData.skillClip.ID)
            {
                if (data.isCoolTime == true)
                    return true;
                else if (data.isCoolTime == false)
                    return false;
            }
        }
        return false;
    }

    public bool ExistInCoolTimeList(int skillID)
    {
        bool isFind = false;
        foreach (SkillData data in coolTimeSkillList)
        {
            if (data.skillClip.ID == skillID)
                isFind = true;
        }

        return isFind;
    }


    public SkillData GetCoolTimeSkill(int skillID)
    {
        foreach (SkillData data in coolTimeSkillList)
        {
            if (data.skillClip.ID == skillID)
                return data;
        }

        return null;
    }

    /// <summary>
    /// 현재 쿨타임이 아닌 스킬데이터를 리턴.
    /// </summary>
    public virtual SkillData GetNotCoolTimeSkill()
    {
        foreach (SkillData data in ownSkills)
        {
            if (data.isCoolTime)
                continue;
            return data;
        }
        return null;
    }

     /// <summary>
     /// ownSkill 목록에서 스킬데이터를 리턴.
     /// </summary>
    public SkillData GetSkilData(int skillID)
    {
        foreach (SkillData data in ownSkills)
            if (data.skillClip.ID == skillID)
                return data;

        return null;
    }


    /// <summary> 버프 or 디버프 등록.
    public bool RegisterBuff(BuffStatsObject buffObject, bool isCounterStateBuff = false)
    {
        if (!buffObject.IsDebuff && !CheckAllowDuplicationBuff(buffObject)) return false;

        BuffData buffData = new BuffData(buffObject);
        buffData.isCounterStateBuff = isCounterStateBuff;
        //buffData.buffData.Apply(baseController);

        if (buffObject.IsDebuff)
            currentEnableDebuff.Add(buffData);
        else
            currentEnableBuffs.Add(buffData);

        return true;
    }

    /// <summary>
    /// 현재 버프목록에 매개변수 버프가 중첩가능한지 검사.
    /// </summary>
    private bool CheckAllowDuplicationBuff(BuffStatsObject buffObject)
    {
        foreach (BuffData buff in currentEnableBuffs)
        {
            if (buff.buffData.ID == buffObject.ID)
            {
                if (!buffObject.AllowDuplication)
                    return false;
                else if (buffObject.AllowDuplication)
                    return true;
            }
        }
        return true;
    }

    public virtual void CheckEnableBuffTime(BaseController controller)
    {
        if (currentEnableBuffs.Count <= 0) return;

        foreach (BuffData buff in currentEnableBuffs.ToArray())
        {
            buff.currentTime += Time.deltaTime;
            if (buff.currentTime >= buff.durationTime)
            {
                buff.buffData.RemoveBuff(controller);
                currentEnableBuffs.Remove(buff);
            }
        }
    }

    public virtual void CheckEnableDeBuffTime(BaseController controller)
    {
        if (currentEnableDebuff.Count <= 0) return;

        foreach (BuffData deBuff in currentEnableDebuff.ToArray())
        {
            deBuff.currentTime += Time.deltaTime;
            if (deBuff.currentTime >= deBuff.durationTime)
            {
                deBuff.buffData.RemoveBuff(controller);
                currentEnableDebuff.Remove(deBuff);
            }

        }
    }

    public void AddSkillCoolTimeList(SkillData data)
    {
        foreach (SkillData skillData in ownSkills)
        {
            if (data.skillClip.ID == skillData.skillClip.ID)
            {
                if (data.isCoolTime == true)
                    return;

                data.SetCoolTimeByClip();
                data.isCoolTime = true;
                coolTimeSkillList.Add(data);
            }
        }
    }



}

