using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 사용 
/// </summary>
public class SkillState : GenealState
{
    [Header("Reference")]
    [SerializeField] private SkillData currentSkillData = null;
    private SkillController skillController = null;
    private AttackState attackState = null;
    private AttackSkillClip currentAttackClip = null;
    private MagicSkillClip currentMagicClip = null;
    private BuffSkillClip currentBuffClip = null;
    //public Transform magicThrowTransform = null;

    [Header("Variables")]
    public LayerMask detectEnemyLayer = TagAndLayerDefine.LayersIndex.Enemy;
    public SkillType currentSkillType = SkillType.NONE;
    public Collider nearEnemy = null;
    private float[] attackTiming = null;
    private float endFrameTime = 0f;
    private float skillAnimSpeed = 1f;
    //private Vector3 enemyDirection = Vector3.zero;
    //private Vector3 magicAttackPosition = Vector3.zero;
    private Collider[] detectEnemyColliders = new Collider[10];

    protected override void Awake()
    {
        base.Awake();
        controller.AddState(this, ref controller.skillStateHash, hashCode);
        skillController = GetComponentInParent<SkillController>();
    }

    private void Start()
    {
        attackState = GetComponent<AttackState>();
    }

    public override void Enter(PlayerStateController stateController, int skillID)
    {
        if (attackState == null)
            attackState = stateController.GetState<AttackState>();

        StopAllCoroutines();
        InitNullClips();
        InitSkillStartSettings();
        SettingClips(skillID);
        Debug.Log("currentSkillData : " + currentSkillData?.skillClip?.displayName);

        //if (!CheckCanUseSkillState())
        //    stateController.ChangeState(stateController.moveStateHash);

        if (currentSkillType == SkillType.ATTACK)
            StartCoroutine(AttackSkillProcess());
        else if (currentSkillType == SkillType.MAGIC)
            StartCoroutine(MagicSkillProcess());
        else if (currentSkillType == SkillType.BUFF)
            StartCoroutine(BuffSkillProcess());
        else
            stateController.ChangeState(stateController.moveStateHash);


    }

    float time = 0;     //test
    bool isStart = false;    //test
    public override void UpdateAction(PlayerStateController stateController)
    {
        if (isStart)   //test
            time += Time.deltaTime;
    }

    public override void Exit(PlayerStateController stateController)
    {
        InitNullClips();
        InitSkillEndSettings();
        controller.myAnimator.Play("Reset");
    }


    //만약 보스에서 각 보스나다 데미지 실행 프로세스를 다르게 하고싶으면 이런 코루틴 부모를 상속받은걸 실행하면됨.
    IEnumerator AttackSkillProcess()
    {
        if (currentSkillData.skillClip is AttackSkillClip)
            currentAttackClip = currentSkillData.skillClip as AttackSkillClip;

        skillAnimSpeed = currentAttackClip.SkillAnimSpeed;
        controller.myAnimator.SetFloat("SkillSpeed", skillAnimSpeed);
        if (!UseStamina(currentAttackClip))
        {
            controller.ChangeState(controller.moveStateHash);
            yield break;
        }


        isStart = true; //테스트용
        //셋팅
        skillController.AddSkillCoolTimeList(currentSkillData);
        if (currentAttackClip.beforeSkillMotionInfo.beforeSkillMotion != BeforeStartSkillMotionType.NOT_USED)
        {
            attackState.FindNearEnemy(ref nearEnemy, detectEnemyColliders, controller.targetLayer, currentAttackClip.AttackRange[0]);
            yield return controller.StartCoroutine(ExcuteBeforeSkillMotion_Co(currentAttackClip.beforeSkillMotionInfo, controller));
        }

        attackTiming = attackState.SetMultiAttackTimings(currentAttackClip.AnimationAttackTimingFrame.ToArray(),currentAttackClip.AnimationClip ,skillAnimSpeed);
        controller.myAnimator.CrossFade(currentAttackClip.AnimationClipName, 0.1f);

        //회전
        //attackState.FindNearEnemy(ref nearEnemy, detectEnemyColliders, detectEnemyLayer, currentAttackClip.attackRange);
        if (currentAttackClip.startRotateToTarget)
            attackState.RotateAttackDirection(currentAttackClip.AttackRange[0], ref nearEnemy);

        float amountTime = 0f;
        endFrameTime = currentAttackClip.GetFrameToTime(currentAttackClip.AnimationEndFrame,currentAttackClip.AnimationClip.frameRate ,  skillAnimSpeed);
        for (int i = 0; i < currentAttackClip.AnimationAttackTimingFrame.Count; i++)
        {
            yield return new WaitForSeconds(attackTiming[i]);
            amountTime += attackTiming[i];
            Quaternion rot = Quaternion.LookRotation(transform.forward);
            EffectManager.Instance.GetEffectObjectInfo(currentAttackClip.AttackEffects[i], controller.slashPosition.position, rot.eulerAngles, Vector3.zero);
            SoundManager.Instance.PlayEffect(currentAttackClip.AttackEffects[i].effectSound);
            AttackSkillDamage(i);
        }

        yield return new WaitForSeconds(endFrameTime - amountTime);
        Debug.Log("Sk_End : " + time);
        controller.ChangeState(controller.moveStateHash);

    }

    private void AttackSkillDamage(int index)
    {
        float angle = attackState.GetExistValue(currentAttackClip.AttackAngle.ToArray(), index);
        float range = attackState.GetExistValue(currentAttackClip.AttackRange.ToArray(), index);
        float damage = attackState.GetExistValue(currentAttackClip.AttackDamage.ToArray(), index);
        RandomEffectInfo effect = (currentAttackClip.HitEffects != null && currentAttackClip.HitEffects.Count > index) ? currentAttackClip.HitEffects[index] : null;
        SoundList sound = (currentAttackClip.HitEffects != null && currentAttackClip.HitEffects.Count > index) ? currentAttackClip.HitEffects[index].effectSound : SoundList.None;
        AttackStrengthType type = (currentAttackClip.StrengthType != null && currentAttackClip.StrengthType.Count > index) ? currentAttackClip.StrengthType[index] : AttackStrengthType.NORMAL;
        CameraShakeInfo camInfo = (currentAttackClip.CameraInfo != null && currentAttackClip.CameraInfo.Length > index) ? currentAttackClip.CameraInfo[index] : null;
        TimeData timeData = (currentAttackClip.TimeDatas != null && currentAttackClip.TimeDatas.Count > index) ? currentAttackClip.TimeDatas[index] : null;

        attackState.FindCanAttackEnemy(angle, range, currentAttackClip.maxTargetCount, ref detectEnemyColliders, detectEnemyLayer);
        attackState.DamageEnemy(effect, sound, type, camInfo, index, damage, timeData, currentAttackClip);
    }


    IEnumerator MagicSkillProcess()  //마법은 위치? 도 설정가능함. (타겟팅인지, 범위인지)
    {
        if (currentSkillData.skillClip is MagicSkillClip)
            currentMagicClip = currentSkillData.skillClip as MagicSkillClip;
        if (!UseStamina(currentMagicClip))
        {
            controller.ChangeState(controller.moveStateHash);
            yield break;
        }

        skillAnimSpeed = currentMagicClip.animationSpeed;
        controller.myAnimator.SetFloat("SkillSpeed", skillAnimSpeed);
        for (int i = 0; i < currentMagicClip.castingSound.Length; i++)
            SoundManager.Instance.PlayEffect(currentMagicClip.castingSound[i]);

        skillController.AddSkillCoolTimeList(currentSkillData);

        if (currentMagicClip.beforeSkillMotionInfo.beforeMotionClip != null)
            yield return controller.StartCoroutine(ExcuteBeforeSkillMotion_Co(currentMagicClip.beforeSkillMotionInfo, controller));

        controller.myAnimator.CrossFade(currentMagicClip.skillAnimationName, 0.1f);

       if (currentMagicClip.startRotateToTarget)
            attackState.FindNearEnemy(ref nearEnemy, detectEnemyColliders, controller.targetLayer, currentMagicClip.canExcuteDistance);
        if (currentMagicClip.startRotateToTarget && nearEnemy != null)
            attackState.RotateAttackDirection(currentMagicClip.canExcuteDistance, ref nearEnemy);
        for (int i = 0; i < currentMagicClip.createProjectileInfos.Count; i++)
            StartCoroutine(CreateMagic_Co(currentMagicClip.createProjectileInfos[i].createFrameTiming, i));

        yield return new WaitForSeconds(currentMagicClip.GetEndTime());

        controller.ChangeState(controller.moveStateHash);
    }



    IEnumerator BuffSkillProcess()   // 애니메이션 재생후 버프클립의 해당 타이밍에 skillcontroller에 등록.
    {
        if (currentSkillData.skillClip is BuffSkillClip)
            currentBuffClip = currentSkillData.skillClip as BuffSkillClip;
        BaseController[] targets = controller.GetBuffTargetList(controller, currentBuffClip);

        skillAnimSpeed = currentBuffClip.animationSpeed;
        controller.myAnimator.SetFloat("SkillSpeed", skillAnimSpeed);

        if (!UseStamina(currentBuffClip))
        {
            controller.ChangeState(controller.moveStateHash);
            yield break;
        }

        skillController.AddSkillCoolTimeList(currentSkillData);
        if (currentBuffClip.animationClipName != string.Empty)
            controller.myAnimator.CrossFade(currentBuffClip.animationClipName, 0.2f);

        for (int i = 0; i < targets.Length; i++)
            CreateBuffEffects(controller, targets[i], currentBuffClip);

        for (int i = 0; i < currentBuffClip.buffObjects.Length; i++)
            currentBuffClip.buffObjects[i].Apply(controller);


        yield return new WaitForSeconds(currentBuffClip.GetFrameToTime(currentBuffClip.animationBuffTimingFrame , currentBuffClip.animationClip.frameRate));
        controller.ChangeState(controller.moveStateHash);

        //버프는 자신한테 하는 함수와 디버플일 경우 적 찾은다음에 적 skillcontroller에 주는 디버프 함수 , 이렇게 2개 만들기.
        //if (!currentBuffClip.isDebuffSkill) //디버프면 셋 디버프로 
        //    skillController.SetBuffDataAndAdd(currentBuffClip);
    }


    private void CreateBuffEffects(PlayerStateController controller, BaseController target, BuffSkillClip buffClip)
    {
        for (int i = 0; i < buffClip.castingEffect.Length; i++)
        {
            //if (buffClip.castingEffect[i].effect == EffectList.None) continue;
            float delayTime = buffClip.GetFrameToTime(buffClip.castingEffect[i].effectFrame, buffClip.animationClip.frameRate, buffClip.animationSpeed);
            controller.StartCoroutine(controller.CreateAttackEffect(delayTime, buffClip.castingEffect[i], target));
        }
        for (int i = 0; i < buffClip.effectInfos.Length; i++)
        {
            // if (buffClip.effectInfos[i].effect == EffectList.None) continue;
            float delayTime = buffClip.GetFrameToTime(buffClip.effectInfos[i].effectFrame, buffClip.animationClip.frameRate, buffClip.animationSpeed);
            controller.StartCoroutine(controller.CreateTimeParticleEffect(delayTime, buffClip.effectInfos[i], target, buffClip.particleTimeType, buffClip.stayTime, buffClip.durationTime, buffClip.returnObpTime));
        }
    }

    private IEnumerator ExcuteBeforeSkillMotion_Co(BeforeSkillMotionInfo info, PlayerStateController controller)
    {
        if (info.useBeforeMRotateToTarget)
            attackState.RotateAttackDirection(info.findNearDistance, ref nearEnemy);

        yield return controller.StartCoroutine(info.ExcuteBeforeMotion_Co(controller.myAnimator, controller.transform, nearEnemy?.transform));
    }


    private void SettingClips(int skillID)
    {
        // skil List 를 skillCOntroller에서 clone 에 skilList와 비교해서 있으면 해당 클립 리턴.
        currentSkillData = skillController.GetSkilData(skillID);
        if (currentSkillData == null) return;

        if (currentSkillData.skillClip is AttackSkillClip)
            currentSkillType = SkillType.ATTACK;
        else if (currentSkillData.skillClip is MagicSkillClip)
            currentSkillType = SkillType.MAGIC;
        else if (currentSkillData.skillClip is BuffSkillClip)
            currentSkillType = SkillType.BUFF;
    }

    private bool UseStamina(BaseSkillClip clip)
    {
        if (controller.Conditions.InfinityStamina) return true;

        if (controller.playerStats.CurrentStamina < clip.skillStaminaCost)
        {
            CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("스태미나가 부족합니다.");
            return false;
        }
        else
        {
            controller.playerStats.UseCurrentStamina((int)clip.skillStaminaCost);
            return true;
        }

    }
    private void InitNullClips()
    {
        currentSkillData = null;
        currentAttackClip = null;
        currentMagicClip = null;
        currentBuffClip = null;
    }


    private IEnumerator CreateMagic_Co(float frame, int index)
    {
        float delayTime = frame * (1f / (currentMagicClip.skillAnimationClip.frameRate * currentMagicClip.animationSpeed));
        yield return new WaitForSeconds(delayTime);

        if (index == 0) TimeManager.Instance.ExcuteTimeData(currentMagicClip.excuteTimeData);

        if (!currentMagicClip.eachTargetEffect)
            CreateMagicAndSetting(nearEnemy == null ? null : nearEnemy.transform, index);
        else
        {
            int count = 0;
            for (int i = 0; i < detectEnemyColliders.Length; i++)
            {
                if (count >= currentMagicClip.maxTargetCount) break;

                BaseController target = detectEnemyColliders[i]?.GetComponent<BaseController>();
                if (detectEnemyColliders[i] != null && target != null)
                {
                    if (controller.IsDetectObstacle(controller.damagedPosition, target.damagedPosition))
                        continue;

                    CreateMagicAndSetting(detectEnemyColliders[i].transform, index);
                    count++;
                }
            }
        }
    }

    private void CreateMagicAndSetting(Transform enemy, int index)
    {
        Debug.Log("Projecit! : " + enemy?.name);
        currentMagicClip.createProjectileInfos[index]?.projectileCreator?.ExcuteCreate(controller, enemy, this);
    }


    private Vector3 SetEnemyDirection(Transform enemy)
    {
        Vector3 retDirection = Vector3.zero;
        if (nearEnemy != null)
        {
            retDirection = enemy.position - controller.transform.position;
            retDirection.y = 0f;
            retDirection.Normalize();
        }
        else
        {
            retDirection = controller.transform.forward;
        }

        return retDirection;
    }

    private Vector3 SetMagicAttackPosition(Transform enemy)
    {
        Vector3 retAttackPosition;
        if (enemy != null)
            retAttackPosition = enemy.transform.position;
        else
            retAttackPosition = controller.myTrans.position + controller.myTrans.forward * currentMagicClip.canExcuteDistance;
        return retAttackPosition;
    }

    private bool CheckCanUseSkillState()
    {
        if (currentSkillData.skillClip.skillState == CurrentSkillState.LOCK)
        {
            // ui띄우고
            return false;
        }
        if (currentSkillData.skillClip.skillState != CurrentSkillState.ACTIVE)
        {

            //ui띄우고
            return false;
        }
        return true;
    }



    private void InitSkillStartSettings()
    {
        controller.Conditions.IsSkilling = true;
        controller.Conditions.CanSkill = false;
        currentSkillType = SkillType.NONE;

        attackTiming = null;
        endFrameTime = 0f;
    }


    private void InitSkillEndSettings()
    {
        controller.Conditions.IsSkilling = false;
        controller.Conditions.CanSkill = true;
        currentSkillType = SkillType.NONE;

        attackTiming = null;
        endFrameTime = 0f;
    }
}

