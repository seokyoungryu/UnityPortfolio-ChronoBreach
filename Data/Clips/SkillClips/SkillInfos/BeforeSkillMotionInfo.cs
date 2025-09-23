using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BeforeSkillMotionInfo 
{
    public bool useBeforeMRotateToTarget = true;
    public bool allowRootMotion = true; //detectCollider 해제.
    public BeforeStartSkillMotionType beforeSkillMotion = BeforeStartSkillMotionType.NOT_USED;
    public string beforeMotionAnimName = "BeforeSkillMotion1";
    public int beforeMotionFullFrame = 0;
    public AnimationClip beforeMotionClip = null;
    public float animationSpeed = 1f;
    public int beforeMotionEndFrame = 0;
    public float standardCloseDistance = 4f;
    public float standardFarDistance = 10f;

    [Header("Player용 : useBeforeMRotateToTarget True일 경우")]
    public float findNearDistance = 6f;

    /// <summary>
    /// 1. CanExcuteBeforeMotion으로 가능한지 검사.
    /// 2. 가능하다면 IEnumerator ExcuteBeforeMOtion()을 실행. 
    /// </summary>


    public IEnumerator ExcuteBeforeMotion_Co(Animator anim, Transform owner, Transform target)
    {
        if (!CanExcuteBeforeMotion(owner, target)) yield break;

        anim.SetFloat(AnimatorKey.BeforeSkillMotionSpeed, animationSpeed);
        anim.Play(beforeMotionAnimName, 1, 0f);

        yield return new WaitForSeconds(GetEndTime());
    }


    public float GetEndTime()
    {
        return beforeMotionEndFrame * (1f / (beforeMotionClip.frameRate * animationSpeed));
    }


    public bool CanExcuteBeforeMotion(Transform owner, Transform target)
    {
        if (beforeSkillMotion == BeforeStartSkillMotionType.NOT_USED || beforeMotionClip == null) return false;
        if (beforeSkillMotion == BeforeStartSkillMotionType.ABSOLUTE_EXCUTE) return true;

        if (beforeSkillMotion == BeforeStartSkillMotionType.WHEN_TARGET_CLOSE)
            return DecideIsTargetClose(owner, target);
        else if (beforeSkillMotion == BeforeStartSkillMotionType.WHEN_TARGET_FAR)
            return DecideIsTargetFar(owner, target);
        else if (beforeSkillMotion == BeforeStartSkillMotionType.RANDOM)
            return DecideRandom(owner, target);
        return false;
    }


    public bool DecideRandom(Transform owner, Transform target)
    {
        float randomNum = Random.Range(0f, 100f);
        int enumCount = System.Enum.GetValues(typeof(BeforeStartSkillMotionType)).Length - 1;
        float eachValue = 100f / enumCount;

        for (int i = 0; i < enumCount; i++)
        {
            if (randomNum <= eachValue * (i + 1))
            {
                switch (i)
                {
                    case 0:return false;
                    case 1:return true;
                    case 2:return DecideIsTargetClose(owner,target);
                    case 3:return DecideIsTargetFar(owner, target);
                }
                break;
            }
        }

        return false;
    }

    private bool DecideIsTargetClose(Transform owner, Transform target)
    {
        if (owner == null || target == null) return false;

        float distance = (target.position - owner.position).magnitude;

        if (distance <= standardCloseDistance)
            return true;

        return false;
    }

    private bool DecideIsTargetFar(Transform owner, Transform target)
    {
        if (owner == null || target == null) return false;

        float distance = (target.position - owner.position).magnitude;

        if (distance >= standardFarDistance)
            return true;

        return false;
    }

}
