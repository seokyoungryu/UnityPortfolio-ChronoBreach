using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMagicProjectile : MonoBehaviour
{
    [Header("Skill Setting")]
    private BaseController owner = null;
    public float targetingSkill_damageTiming = 0f;
    public EffectList hit_Effect = EffectList.None;
    public GameObject projectile_Prefab = null;
    public float detectRange = 0f;

    [Header("Variabls")]
    private Transform singleTargetPosition = null; 
    private MagicSkillClip thisMagicClip = null;  
    private MagicType magicType = MagicType.NONE;
    private Vector3 direction = Vector3.zero;      
    private bool isStartDamaged = false;
    private bool haveIndicator = false;
    private bool isIndicatorFollowing = false;
    private bool isDoneIndicator = false;
    

    private void Update()
    {
        if (isStartDamaged) return;

        if(magicType == MagicType.THROWING)
        {
            if(DetectThrowingCollider())
            {
                StartCoroutine(ThrowingProcess());
            }
        }
        else if(magicType == MagicType.TARGETING_SINGLE && isIndicatorFollowing)
        {
            transform.position = singleTargetPosition.position;
        }


    }


    #region Init Variables
   


    private bool DetectThrowingCollider()
    {
        return false;
    }



    #region Throwing

    private IEnumerator ThrowingProcess()
    {
        isStartDamaged = true;
        projectile_Prefab.SetActive(false);
        yield return null;
        StartCoroutine(TargetDamage());

    }

    #endregion

    #region Targeting

    private IEnumerator TargetingProcess()
    {
        if(haveIndicator)
        {
            yield return StartCoroutine(IndicatorPlay());
        }

        StartCoroutine(TargetDamage());

        yield return null;
    }

    private IEnumerator IndicatorPlay()
    {

        yield return new WaitForSeconds(1f);

    }

    private void SingleTargetingDamage()
    {

    }

    #endregion


    public virtual void FunctionPerDamage() { }
    public virtual void FunctionBeforeDamage() { }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);

    }
}
