using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AI/Attack/Range Attack Clip", fileName = "RA_")]
public class AIRangeAttackClip : AIAttackClip
{
    [Header("Range Attack Settings")]
    [SerializeField] private ProjectileCreator creator = null;
    public FollowDirection followDirectionType = FollowDirection.STATIC_DIRECTION;

    public float canExcuteDistance = 10f;
    public float shootTimingFrame = 0f;
    public float animationSpeed = 1f;

    public ProjectileCreator ProjectileCreator => creator;

  
}


public enum RangeAttackShootType
{
    NONE =-1,
    STRAIGHT = 0,
    ROTATE_TO_TARGET = 1,
}

public enum FollowDirection
{
    STATIC_DIRECTION =0,
    FOLLOW_DIRECTION = 1,
}
