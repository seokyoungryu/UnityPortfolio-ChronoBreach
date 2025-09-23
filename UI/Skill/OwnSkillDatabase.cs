using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Database/Own Skill Database", fileName ="OwnSkillDB_")]
public class OwnSkillDatabase : ScriptableObject
{
    [SerializeField] private List<BaseSkillClip> ownSkills = new List<BaseSkillClip>();

    public List<BaseSkillClip> OwnSkills => ownSkills;


    public void AddSkill(BaseSkillClip clip)
    {
        if (!ownSkills.Contains(clip))
            ownSkills.Add(clip);
    }

    public T FindSkillType<T>() where T : BaseSkillClip
    {
        foreach (BaseSkillClip clip in ownSkills)
            if (clip is T)
                return clip as T;
        return null;
    }

}
