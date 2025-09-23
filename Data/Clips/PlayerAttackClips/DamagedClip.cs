using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Attack Data/Damaged Data")]
public class DamagedClip : ScriptableObject
{
    [SerializeField] private bool rotateToAttacker = false;
    [SerializeField] private AttackStrengthType strengthType = AttackStrengthType.NONE;
    [SerializeField] private AnimationClip damagedClip = null;
    [SerializeField] private string animationName = string.Empty;
    [SerializeField] private int endAnimationFrame = 0;
    [SerializeField] private float clipFullFrame = 0;
    [SerializeField] private CameraShakeInfo cameraShakeInfo = null;

    [Header("FlyDown Settings")]
    [SerializeField] private bool isFlyDown = false;
    [SerializeField] private float riseTime = 0f;   //1이면 다운후 1초뒤 rise 실행.

    [Tooltip("데미지 받을수 있는 프레임. ex) endFrame이 20이고," +
        " canDamagedFrame이 15일 경우 15프레임 부터는 다시 데미지를 받을수 있음. 다만 종료는 20프레임에서.")]
    [SerializeField] private int canDamagedFrame = 0;
    [SerializeField] private float animationPlaySpeed = 1f;

    public bool IsFlyDown => isFlyDown;
    public float RiseTime => riseTime;
    public bool RotateToAttacker => rotateToAttacker;
    public AttackStrengthType StrengthType => strengthType;
    public string AniamtionName => animationName;
    public float AnimationPlaySpeed => animationPlaySpeed;
    public int EndAnimationFrame => endAnimationFrame;
    public CameraShakeInfo CameraShakeInfo => cameraShakeInfo;
    public float EndAnimationFrameToTime() => FrameToTime(endAnimationFrame);
    public float CanDamagedFrameToTime() => FrameToTime(canDamagedFrame);

    public float FrameToTime(int frame)
    {
        float rate = 1 / (30f * animationPlaySpeed);
        return frame * rate;
    }

    private void OnValidate()
    {
        if (damagedClip != null)
            clipFullFrame = (int)(damagedClip.length * 30f);
    }
}
