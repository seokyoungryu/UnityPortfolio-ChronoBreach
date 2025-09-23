using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackClip : ScriptableObject
{
    public string animationClipName = "Attack";
    public AnimationClip attackClip = null;

 
    [Tooltip("애니메이션 클립 종료 프레임 지점 ")]
    [SerializeField] private float clipFullFrame = 0;  
    public float attackEndAnimationFrame = 0f;
    public float waitAttackEndTime = 0f;


    [Header("공격전 모션")]
    public BeforeSkillMotionInfo beforeSkillMotionInfo;


    public float GetFrameToTime(float frame, float speed)
    {
        float rate = 1f / (attackClip.frameRate * speed);
        return frame * rate;
    }

    private void OnValidate()
    {
        if (attackClip != null)
        {
            clipFullFrame = attackClip.length * 30f;
            if (attackEndAnimationFrame == 0)
                attackEndAnimationFrame = clipFullFrame;
        }
    }
}
