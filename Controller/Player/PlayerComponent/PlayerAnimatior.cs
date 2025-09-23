using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatior : MonoBehaviour
{
    private const string AnimatorDamagedSpeedName = "DamagedSpeed";

    [SerializeField] private AnimatorOverrideController aoc = null;
    public AnimatorOverrideController Aoc => aoc;

    [SerializeField] private float damagedAnimationSpeed = 0f;

    private Animator myAnim = null;

    public float DamagedAnimationSpeed
    {
        get { return damagedAnimationSpeed; }
        set
        {
            damagedAnimationSpeed = value;
            myAnim.SetFloat(AnimatorDamagedSpeedName, damagedAnimationSpeed);
        }
    }


    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }
}
