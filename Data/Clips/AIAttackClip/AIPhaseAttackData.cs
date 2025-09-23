using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIPhaseAttackData
{
    public string phaseName = string.Empty;
    public int phaseCount = 1;
    [Header("전체 체력 100% 중 몇 %에 페이즈가 될지 세팅 (1~100%)")]
    public float phasePercent = 0f;
    [Header("페이즈 단계")]
    public string phaseAnimationClipName = string.Empty;
    public AnimationClip animClip = null;
    public float animFullFrame = 0f;
    public float phaseAnimationEndFrame = 0f;
    public float animationSpeed = 1f;
    public float waitEndTime = 0f;
    [TextArea(0, 2)]
    public string globalNotifier = string.Empty;
    public BaseSkillClip[] unlockPhaseSkills = null;
    public UseableObject[] applyUseableObjs = null;



}


