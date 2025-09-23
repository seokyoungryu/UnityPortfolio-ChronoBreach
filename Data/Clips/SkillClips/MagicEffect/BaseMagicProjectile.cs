using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  보통과 다른 스킬일 경우 ex) 데미지마다 충격파 를 준다거나.. 머 그런거 있으면 이거 부모로 자식클래스 만듬.
/// </summary>
public class BaseMagicProjectile : MonoBehaviour
{
    [Header("Skill Setting")]
    private BaseController owner = null;
    public float targetingSkill_damageTiming = 0f;
    public EffectList hit_Effect = EffectList.None;
    public GameObject projectile_Prefab = null;
    public float detectRange = 0f;

    [Header("Variabls")]
    //private LayerMask targetLayer;
    private Transform singleTargetPosition = null; //?
    private MagicSkillClip thisMagicClip = null;   //고유
   // private AttackStrengthType[] strengthType;
    private MagicType magicType = MagicType.NONE;
    private Vector3 direction = Vector3.zero;      //?
   // private Collider[] findColliders = new Collider[10];
      // private List<float> damage = new List<float>();
   // private float speed = 5f;
  //  private int maxTargetCount = 0;
  //     private int hitCount = 0;
   // private float hitDelay = 0f;

    private bool isStartDamaged = false;
    private bool haveIndicator = false;
    private bool isIndicatorFollowing = false;
    private bool isDoneIndicator = false;
    

    private void Update()
    {
        if (isStartDamaged) return;

        if(magicType == MagicType.THROWING)
        {
          //  transform.localPosition += direction * speed * Time.deltaTime;
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
    /// <summary>
    /// Throwing 스킬 세팅
    /// </summary>
    public void SetInitVariables(Vector3 dir, MagicSkillClip magicClip, LayerMask targetLayers)
    {
        Debug.Log("엔터!");

        
      // if(magicClip.magicType == MagicType.THROWING)
      // {
      //   //  thisMagicClip = magicClip;
      //   //  magicType = MagicType.THROWING;
      //   //  strengthType = magicClip.strengthType;
      //   //  targetLayer = targetLayers;
      //   //  maxTargetCount = magicClip.maxTargetCount;
      //   //  damage = thisMagicClip.magicDamage;
      //   //
      //   //  dir.y = 0f;
      //   //  dir.Normalize();
      //   //  direction = dir;
      //   //  hitCount = magicClip.hitCount;
      //   //  hitDelay = magicClip.hitPerDelay;
      //     
      // }
       
    }

    /// <summary>
    /// Targeting Multiple 스킬 세팅
    /// </summary>
    public void SetInitVariables( MagicSkillClip magicClip, LayerMask targetLayers)
    {
       // if (magicClip.magicType == MagicType.TARGETING_MULTIPLE)
       // {
       //  //  thisMagicClip = magicClip; 
       //  //  magicType = MagicType.TARGETING_MULTIPLE;
       //  //  isIndicatorFollowing = magicClip.isIndicatorFollowing;
       //  //  targetLayer = targetLayers;
       //  //  maxTargetCount = magicClip.maxTargetCount;
       //  //  damage = thisMagicClip.magicDamage;
       //  //
       //  //  hitCount = magicClip.hitCount;
       //  //  hitDelay = magicClip.hitPerDelay;
       //  //  StartCoroutine(TargetingProcess());
       // }
    }

    /// <summary>
    /// Targeting Single 스킬 세팅
    /// </summary>
    public void SetInitVariables(MagicSkillClip magicClip, Transform singleTarget ,LayerMask targetLayers)
    {
      //  if (magicClip.magicType == MagicType.TARGETING_SINGLE)
      //  {
      //   //  thisMagicClip = magicClip;
      //   //  magicType = MagicType.TARGETING_SINGLE;
      //   //  singleTargetPosition = singleTarget;
      //   //  isIndicatorFollowing = magicClip.isIndicatorFollowing;
      //   //  targetLayer = targetLayers;
      //   //  maxTargetCount = magicClip.maxTargetCount;
      //   //  damage = thisMagicClip.magicDamage;
      //   //
      //   //  hitCount = magicClip.hitCount;
      //   //  hitDelay = magicClip.hitPerDelay;
      //   //  StartCoroutine(TargetingProcess());
      //
      //  }
    }
    #endregion

    private bool DetectThrowingCollider()
    {
       // int detectCount = Physics.OverlapSphereNonAlloc(transform.position, detectRange, findColliders, targetLayer);
       // if (detectCount > 0)
       //     return true;
       //
        return false;
    }

    private void FindColliderAndDamage(int index)
    {
      //  findColliders = new Collider[10];
      //  int detectCount = Physics.OverlapSphereNonAlloc(transform.position, detectRange, findColliders, targetLayer);
      //  int attackCount = 0;
      //  PlayerStatus playerStatus = null;
      //  if (owner is PlayerStateController)
      //      playerStatus = (owner as PlayerStateController).playerStats;
      //
      //  for (int i = 0; i < detectCount; i++)
      //  {
      //      if (maxTargetCount <= attackCount) return;
      //
      //      bool isCritical = playerStatus != null ? playerStatus.IsCriticalAttack() : false;
      //      float totalDamage = isCritical ? playerStatus.GetCriticalSkillDamage() * damage[index] 
      //                                     : playerStatus.GetSkillDamage() * damage[index];
      //      findColliders[i]?.GetComponent<IDamageable>()?.Damaged(totalDamage, owner, isCritical, strengthType[index]);
      //      attackCount++;
      //      Debug.Log($"데미지 ({attackCount})");
      //  }
      //

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

    public IEnumerator TargetDamage()
    {
        yield return new WaitForSeconds(targetingSkill_damageTiming);
      //
      // //여기서 만약 자식객체에서 설정한게 있으면 
      // for (int i = 0; i < hitCount; i++)
      // {
      //     FunctionPerDamage(); //이런식으로 자식 함수 추가해서 저 함수만 수정해주느식으로.
      //     FindColliderAndDamage(i);
      //     yield return new WaitForSeconds(hitDelay);
      // }
    }



    public virtual void FunctionPerDamage() { }
    public virtual void FunctionBeforeDamage() { }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);

    }
}
