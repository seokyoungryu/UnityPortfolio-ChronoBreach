using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RequipedSkillType
{
    NONE = -1,
    COMBO = 0,
    COUNTER = 1,

}

public class RequipedSkillSlot : MonoBehaviour
{
    [SerializeField] private RequipedSkillType requipedSkillType = RequipedSkillType.NONE;
    [SerializeField] private BaseSkillClip requipedClip = null;


    public RequipedSkillType RequipedSkillType => requipedSkillType;
    public BaseSkillClip RequipedClip { get { return requipedClip; } set { requipedClip = value; } }
}
