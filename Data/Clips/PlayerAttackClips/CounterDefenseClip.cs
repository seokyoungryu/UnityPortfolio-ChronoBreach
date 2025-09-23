using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Attack Data/Counter Defense", fileName ="CounterDefense_")]
public class CounterDefenseClip : ScriptableObject
{
    public string counterName = string.Empty;
    public string animationName = string.Empty;
    public SoundList excuteSound = SoundList.None;
    public SoundList[] successSound;

    public int fullFrame = 0;
    public int endAnimationFrame = 0;
    public int detectOffFrame = 0;

    public AnimationClip animationClip;
    [SerializeField] private TimeData successTimeData = null;
    [SerializeField] private CameraShakeInfo successDefenseShakeInfo;
    [SerializeField] private CameraShakeInfo parryingShakeInfo;


                                                                                 
    public TimeData GetTimeData => successTimeData;
    public CameraShakeInfo SuccessDefenseShakeInfo => successDefenseShakeInfo;
    public CameraShakeInfo ParryingShakeInfo => parryingShakeInfo;


    public float GetDetectOffFrameToTime()
    {
        float frame = 1f / animationClip.frameRate;
        return detectOffFrame * frame;
    }

    public float GetEndAnimationFrameToTime()
    {
        float frame = 1f / animationClip.frameRate ;
        return endAnimationFrame * frame; 
    }


    private void OnValidate()
    {
        if(animationClip != null)
        {
            fullFrame = (int)(animationClip.length * animationClip.frameRate);
            if (endAnimationFrame == 0)
                endAnimationFrame = fullFrame;
        }
    }
}
