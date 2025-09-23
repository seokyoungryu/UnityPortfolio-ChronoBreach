using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillData
{
    public string skillName_Kor = string.Empty;
    public bool isCoolTime = false;
    public float coolTime = 0f;
    public BaseSkillClip skillClip = null;


    public SkillData(BaseSkillClip skillClip)
    {
        if (skillClip is AttackSkillClip)
        {
            AttackSkillClip clone = new AttackSkillClip(skillClip);
            this.skillClip = clone;
            skillName_Kor = skillClip.displayName;
        }
        else if (skillClip is MagicSkillClip)
        {
            MagicSkillClip clone = new MagicSkillClip(skillClip);
            this.skillClip = clone;
            skillName_Kor = skillClip.displayName;
        }
        else if (skillClip is BuffSkillClip)
        {
            BuffSkillClip clone = new BuffSkillClip(skillClip);
            this.skillClip = clone;
            skillName_Kor = skillClip.displayName;
        }
        else if (skillClip is ComboSkillClip)
        {
            ComboSkillClip clone = new ComboSkillClip(skillClip);
            this.skillClip = clone;
            skillName_Kor = skillClip.displayName;
        }
        else if (skillClip is PassiveSkillClip)
        {
            PassiveSkillClip clone = new PassiveSkillClip(skillClip);
            this.skillClip = clone;
            skillName_Kor = skillClip.displayName;
        }
        else if (skillClip is CounterSkillClip)
        {
            CounterSkillClip clone = new CounterSkillClip(skillClip);
            this.skillClip = clone;
            skillName_Kor = skillClip.displayName;
        }
        else if (skillClip is DashSkillClip)
        {
            DashSkillClip clone = new DashSkillClip(skillClip);
            this.skillClip = clone;
            skillName_Kor = skillClip.displayName;
        }
    }

    public void SetCoolTimeByClip()
    {
        coolTime = skillClip.skillCoolTime;
    }

}

