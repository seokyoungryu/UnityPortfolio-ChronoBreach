using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(ReturnObjectToObjectPooling))]
public class RangeAttackProjectile : MonoBehaviour
{
     public RangeAttackInfo rangeAttackInfos = null;
     public AfterColisionRangeAttackInfo afterInfos = null;

    private BaseController owner = null;
    private Transform targetTr = null;
    private BaseController targetContr;
    private ReturnObjectToObjectPooling pooling = null;
    private Collider[] detectColliders = new Collider[10];
    private Collider[] dmgColliders = new Collider[10];
    private Collider[] detectObstacleColl = new Collider[1];
    private Vector3 prevPosition = Vector3.zero;
    private float damageDetectRadius = 0f;
    private float currentSpeed = 0f;
    [SerializeField] private bool canDamage = true;
    private float gizmoExploreRange = 0f;
    private bool isDetectObstacle = false;
    private int detectObstacleCount = 0;
    private Vector3 projectileSpawnPos = Vector3.zero;
    private float projectileDistance = 0f;

    private LayerMask collisionLayer;
    private LayerMask targetLayer;
    private Vector3 collisionPos = Vector3.zero;
    private IEnumerator autoDetectTarget_Co = null;
    public bool canDot = true;

    public RangeAttackInfo Infos { get { return rangeAttackInfos; } set { rangeAttackInfos = value; } }
    public Vector3 CollisionPos { get { return collisionPos; } set { collisionPos = value; } }

    private void Start()
    {
        //음.. Setting 없어도 인스펙터에서 설정한 rangeAttackInfos으로  실행할수있도록하기?.
    }

    #region  Detect
    private IEnumerator DetectObstacle()
    {
        //if (rangeAttackInfos.rangeAttackType == RangeAttackType.PENETRATE) yield break;

        collisionLayer = rangeAttackInfos.collisionDetectType == ProjectileCollisionDetectType.OWNER_OBSTACLE ? rangeAttackInfos.Owner.obstacleLayer
                                                                                                   : rangeAttackInfos.collisionDetectLayer;

        while (!isDetectObstacle)
        {
            detectObstacleCount = Physics.OverlapSphereNonAlloc(transform.position, rangeAttackInfos.detectObstacleRadius, detectObstacleColl, collisionLayer);
            if (detectObstacleCount > 0)
            {
                Debug.Log("Obstacle Detect");
                for (int i = 0; i < detectObstacleColl.Length; i++)
                {
                    if (detectObstacleColl[i] == null) continue;
                    else
                    {
                        Debug.Log("Projectile 장애물 충돌");
                        canDamage = false;
                        EffectManager.Instance.GetEffectObjectRandom(rangeAttackInfos.collisionEffect, transform.position, Vector3.zero, Vector3.zero);
                        SoundManager.Instance.PlayEffect(rangeAttackInfos.collisionEffect.effectSound);
                        pooling.ExcuteCollision(rangeAttackInfos.ReturnTime);
                        isDetectObstacle = true;
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator DetectMaxDistance()
    {
        while (canDamage)
        {
            projectileDistance = (transform.position - projectileSpawnPos).magnitude;
            if(projectileDistance >= rangeAttackInfos.maxDistance)
            {
                canDamage = false;
                EffectManager.Instance.GetEffectObjectRandom(rangeAttackInfos.collisionEffect, transform.position, Vector3.zero, Vector3.zero);
                SoundManager.Instance.PlayEffect(rangeAttackInfos.collisionEffect.effectSound);
                pooling.ExcuteCollision(rangeAttackInfos.ReturnTime);
            }
            yield return new WaitForSeconds(0.05f);
        }

    }
    #endregion

    #region Damage Type Process

    private void StartDmagedProcess(RangeAttackInfo info)
    {
        if (info.RangeMoveType != RangeMoveType.WAVE)
            damageDetectRadius = info.DetectTargetRadius;

        switch (info.RangeAttackType)
        {
            case RangeAttackType.SINGLE:
                StartCoroutine(SingleDamage_Co());
                break;
            case RangeAttackType.PENETRATE:
                StartCoroutine(Penetrate_Co());
                break;
            case RangeAttackType.POINT_EXPLOSION:
                StartCoroutine(PointExplosion_Co());
                break;

        }
    }

    private void ExcuteTimeData(RangeAttackInfo info, int index)
    {
        if (owner is PlayerStateController)
        {
            if (info.onlyTimeDataIndexZero)
            {
                if (info.isRepeatDamage && index == 0 && info.timeDatas.Count > index && info.timeDatas[0] != null)
                    TimeManager.Instance.ExcuteTimeData(info.timeDatas[0]);
            }
            else
            {
                if (info.timeDatas.Count > index && info.timeDatas[index] != null)
                    TimeManager.Instance.ExcuteTimeData(info.timeDatas[index]);
                else
                    TimeManager.Instance.ExcuteBaseTimeData(TimeInfoType.RANGE_SKILL);
            }
        }
    }

    #region Single
    private IEnumerator SingleDamage_Co()
    {
        int count = 0;
        int index = 0;
        while (canDamage)
        {
            count = DetectProjectileCollider(rangeAttackInfos);
            if (count > 0)
            {
                rangeAttackInfos.Owner.SortFindEmenyByNearDistance(transform, ref dmgColliders);
                for (int i = 0; i < rangeAttackInfos.Damage.Count; i++)
                {
                    if (dmgColliders[0] == null) continue;

                    if (index == 0)
                        ExcuteTimeData(rangeAttackInfos, i);

                   BaseController controller = dmgColliders[0].GetComponent<BaseController>();
                    if (controller != null)
                    {
                        canDamage = false;
                        isDetectObstacle = true;
                        SoundManager.Instance.PlayEffect(rangeAttackInfos.collisionEffect.effectSound);
                        pooling.ExcuteCollision(rangeAttackInfos.ReturnTime);
                        EffectManager.Instance.GetEffectObjectRandom(rangeAttackInfos.collisionEffect, transform.position, Vector3.zero, Vector3.zero);
                        yield return new WaitForSeconds(rangeAttackInfos.HitDelay[i]);
                        Damaged(controller, rangeAttackInfos, i);
                    }
                }

                index++;
            }
            yield return null;
        }
    }
    #endregion

    #region Point Explosion
    private IEnumerator PointExplosion_Co()
    {
        int detectCount = 0;
        while (canDamage)
        {
            detectCount = DetectProjectileCollider(rangeAttackInfos);
            if (detectCount > 0 || rangeAttackInfos.rangeMoveType == RangeMoveType.POINT)
            {
                isDetectObstacle = true;
                Debug.Log("Point Detect");
                EffectManager.Instance.GetEffectObjectRandom(rangeAttackInfos.collisionEffect, transform.position, Vector3.zero, Vector3.zero);
                pooling.ExcuteCollision(rangeAttackInfos.ReturnTime);
                SoundManager.Instance.PlayEffect(rangeAttackInfos.collisionEffect.effectSound);
                if (!rangeAttackInfos.DelayExplosionAttack)
                    PointExplosion_Immediate(transform.position);  //한번 충돌하면 data 목록만큼 무조건 데미지.
                else
                    StartCoroutine(PointExplosion_DelayAttack(transform.position));  //data의 hitDelay만큼 확인후 공격.
                canDamage = false;
            }
            yield return null;
        }
    }
    private void PointExplosion_Immediate(Vector3 detectPosition)
    {
        int damageCount = 0;
        int currentDmgedTargetCount = 0;
        damageCount = Physics.OverlapSphereNonAlloc(detectPosition, rangeAttackInfos.DamageRange[0], dmgColliders, rangeAttackInfos.TargetLayer);
        if (damageCount > 0)
        {
            ExcuteTimeData(rangeAttackInfos, 0);
            EffectManager.Instance.GetEffectObjectRandom(rangeAttackInfos.collisionEffect, transform.position, Vector3.zero, Vector3.zero);
            rangeAttackInfos.Owner.SortFindEmenyByNearDistance(transform, ref dmgColliders);
            for (int i = 0; i < damageCount; i++)  //적 수 
            {
                if (currentDmgedTargetCount >= rangeAttackInfos.MaxTargetCount) break;
                BaseController controller = dmgColliders[i].GetComponent<BaseController>();
                if (controller == null || controller.IsDetectObstacle(transform, controller.damagedPosition))
                    continue;
                if (!CheckTargetInAngle(transform, controller.transform, rangeAttackInfos.ExplosionAngle[i]))
                    continue;
                StartCoroutine(DelayHitCountDamage(controller));
                currentDmgedTargetCount++;
            }
        }
    }
    private IEnumerator PointExplosion_DelayAttack(Vector3 detectPosition)
    {
        int damageCount = 0;
        int currentDmgedTargetCount = 0;
        //뒤늦게 폭팔하는거 구현임. 
        for (int i = 0; i < rangeAttackInfos.HitCount; i++)
        {
            yield return new WaitForSeconds(rangeAttackInfos.HitDelay[i]);
            ExcuteTimeData(rangeAttackInfos, i);

            damageCount = Physics.OverlapSphereNonAlloc(transform.position, rangeAttackInfos.DamageRange[i], dmgColliders, rangeAttackInfos.TargetLayer);
            if (damageCount > 0)
            {
                rangeAttackInfos.Owner.SortFindEmenyByNearDistance(transform, ref dmgColliders);
                for (int x = 0; x < damageCount; x++)
                {
                    BaseController controller = dmgColliders[x].GetComponent<BaseController>();
                    if (controller == null || controller.IsDetectObstacle(transform, controller.damagedPosition))
                        continue;
                    if (!CheckTargetInAngle(transform, controller.transform, rangeAttackInfos.ExplosionAngle[i]))
                        continue;
                    Damaged(controller, rangeAttackInfos, i);
                    currentDmgedTargetCount++;
                }
            }
        }
    }
    #endregion

    #region Penetrate
    private IEnumerator Penetrate_Co()
    {
        Dictionary<int, int> trInsIDs = new Dictionary<int, int>();
        List<int> damagedTargetList = new List<int>();
        int count = 0;
        int damagedIndex = 0;

        while (canDamage)
        {
            count = DetectProjectileCollider(rangeAttackInfos);

            if (count > 0)
            {
                if (damagedIndex == 0)
                    ExcuteTimeData(rangeAttackInfos, 0);

                damagedIndex++;

                for (int i = 0; i < count; i++)
                {
                    if (dmgColliders[i] == null)
                        continue;
                    if (trInsIDs.Count >= rangeAttackInfos.MaxTargetCount)
                        yield break;
                       // pooling.ExcuteCollision(rangeAttackInfos.returnTime);

                    int targetId = dmgColliders[i].GetInstanceID();
                    if (damagedTargetList.Contains(targetId)) //이미 데미지 입혔는지 확인.
                        continue;

                    if (!trInsIDs.ContainsKey(targetId))  
                    {
                        if (trInsIDs.Count >= rangeAttackInfos.MaxTargetCount)
                            continue;

                        trInsIDs.Add(targetId, 1);
                        BaseController controller = dmgColliders[i].GetComponent<BaseController>();
                        StartCoroutine(DelayHitCountDamage(controller));
                    }   
                    else if (trInsIDs.ContainsKey(targetId) && !damagedTargetList.Contains(targetId))
                    {
                        if (trInsIDs[targetId] >= rangeAttackInfos.MaxSamePenetrateCount)
                            continue;

                        BaseController controller = dmgColliders[i].GetComponent<BaseController>();
                        StartCoroutine(DelayHitCountDamage(controller));
                        trInsIDs[targetId]++;
                    }

                    if (trInsIDs.ContainsKey(targetId) && !damagedTargetList.Contains(targetId))
                    {
                        if (!rangeAttackInfos.AllowPenetrateSameEnemy && trInsIDs[targetId] >= rangeAttackInfos.MaxSamePenetrateCount)
                            continue;
                        damagedTargetList.Add(targetId);
                        StartCoroutine(RemoveTargetInstanceID(trInsIDs, damagedTargetList, targetId));
                    }
                }
            }
            yield return null;
        }
    }
    private IEnumerator RemoveTargetInstanceID(Dictionary<int, int> targetDictionary , List<int> dmgList, int instanceID)
    {
        yield return new WaitForSeconds(rangeAttackInfos.SamePenetrateDelayTime);
        if (dmgList.Contains(instanceID))
            dmgList.Remove(instanceID);
    }
    #endregion

    private void Damaged(BaseController controller, RangeAttackInfo infos, int index)
    {
        if (controller == null) return;

        (bool, float) dmgValue = rangeAttackInfos.Owner.GetDamageValue(infos.IsSkill);

        if (controller is PlayerStateController)
        {
            PlayerStateController player = controller as PlayerStateController;
            if (player.Conditions.IsCounting || player.Conditions.IsDetectParry)
                player.GetState<CounterAttackState>().CounterSuccess(infos.Owner, CounterAttackType.RANGE);
            else if (player.Conditions.CanDamaged())
                player.playerStats.Damaged(infos.Damage[index] * dmgValue.Item2, infos.Owner, dmgValue.Item1, rangeAttackInfos.isSkill, infos.StrengthType[index]);
        }
        else if (controller is AIController)
        {
            AIController ai = controller as AIController;
            if (!ai.aiConditions.IsDead)
                ai.aiStatus.Damaged(infos.Damage[index] * dmgValue.Item2, infos.Owner, dmgValue.Item1, rangeAttackInfos.isSkill, infos.StrengthType[index]);
        }

        if (infos.HitEffects.Count > index)
        {
            EffectManager.Instance.GetEffectObjectRandom(infos.HitEffects[index], controller.damagedPosition.position, Vector3.zero, Vector3.zero);
            SoundManager.Instance.PlayEffect(infos.HitEffects[index].effectSound);
        }
        ExcuteAfterCollisionRangeAttack(afterInfos, transform.position);

    }

    private IEnumerator DelayHitCountDamage(BaseController controller, float excuteWaitTimer = 0f)
    {
        yield return new WaitForSeconds(excuteWaitTimer);
        for (int i = 0; i < rangeAttackInfos.HitCount; i++)
        {
            yield return new WaitForSeconds(rangeAttackInfos.HitDelay[i]);
          
            Damaged(controller, rangeAttackInfos, i);
        }
    }
    private int DetectProjectileCollider(RangeAttackInfo infos)
    {
        targetLayer = GetTargetLayer(infos);

        if (infos.ProjectileDetectType == ProjectileDetectType.SPHERE)
            return Physics.OverlapSphereNonAlloc(transform.position, damageDetectRadius, dmgColliders, targetLayer);
        else if (infos.ProjectileDetectType == ProjectileDetectType.CUBE)
            return Physics.OverlapBoxNonAlloc(transform.position + transform.TransformDirection(rangeAttackInfos.ProjectileDetectCubePosition),
                                              infos.ProjectileCubeSize / 2f, dmgColliders,
                                              Quaternion.LookRotation(transform.forward),
                                             targetLayer);
        return 0;
    }

    #endregion

    #region Move Type Process

    /// <summary>
    /// 투사체 이동 코루틴 실행.
    /// </summary>
    private void MoveProcess(RangeAttackInfo info)
    {
        currentSpeed = info.Speed;
        if (info.IsAccelerate)
            StartCoroutine(Accelate_Co(info));
        switch (info.RangeMoveType)
        {
            case RangeMoveType.THROWING:
                StartCoroutine(MoveThrowing_Co());
                break;
            case RangeMoveType.TARGETING:
                StartCoroutine(MoveTargeting_Co());
                break;
            case RangeMoveType.POINT:
                StartCoroutine(MovePoint_Co());
                break;
            case RangeMoveType.HOMING:
                if (targetTr != null)
                    StartCoroutine(MoveHoming_Co());
                else
                    StartCoroutine(MoveThrowing_Co());
                break;
            case RangeMoveType.WAVE:
                StartCoroutine(MoveWave_Co());
                break;
        }
    }

    private IEnumerator Accelate_Co(RangeAttackInfo info)
    {
        currentSpeed = info.InitSpeed;

        while (currentSpeed < info.MaxSpeed)
        {
            yield return new WaitForSeconds(info.AdditiveDelayTime);
            currentSpeed += info.AdditiveSpeed ;
        }

        currentSpeed = info.MaxSpeed;
    }
 
     private IEnumerator MoveThrowing_Co()
     {
         Vector3 dir = Vector3.zero;
         //transform.rotation = Quaternion.LookRotation(dir);
    
         while (canDamage)
         {
             if (Infos.moveDirectType == ProjectileMoveDirectType.FORWARD)
                 dir = transform.forward ;
             else if (Infos.moveDirectType == ProjectileMoveDirectType.BACK)
                 dir = -transform.forward ;
             else if (Infos.moveDirectType == ProjectileMoveDirectType.RIGHT)
                 dir = transform.right ;
             else if (Infos.moveDirectType == ProjectileMoveDirectType.LEFT)
                 dir = -transform.right ;
             else if (Infos.moveDirectType == ProjectileMoveDirectType.UP)
                 dir = transform.up;
             else if (Infos.moveDirectType == ProjectileMoveDirectType.DOWN)
                 dir = -transform.up ;
    
             transform.position += dir * currentSpeed * Time.deltaTime;
             yield return null;
         }
    
     }

    private IEnumerator MoveTargeting_Co()
    {
        yield return new WaitForSeconds(rangeAttackInfos.ExcuteTargetingDelay);

        if (autoDetectTarget_Co != null)
        {
            StopCoroutine(autoDetectTarget_Co);
            autoDetectTarget_Co = null;
        }

        float timer = rangeAttackInfos.UpdateTargetingDelay;
        BaseController targetContr = null;
        Vector3 targetPos = Vector3.zero;
        Vector3 movePosition = transform.position;
        Vector3 directionToTarget = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;
        while (canDamage || Infos.stillMoveToTarget)      //이부분 updateDelay는 말그대로 위치를 업데이트 시키는거고. 기본적으로 타겟위치로 이동.
        {
            timer += Time.deltaTime;

            if (targetTr == null && rangeAttackInfos.AutoDetectTarget && autoDetectTarget_Co == null)
            {
                autoDetectTarget_Co = AutoDetectTarget_Co();
                StartCoroutine(autoDetectTarget_Co);
            }
            else if (targetTr != null && timer >= rangeAttackInfos.UpdateTargetingDelay)
            {
                timer = 0;
                targetContr = targetTr.GetComponent<BaseController>();
                targetPos = targetContr != null ? targetContr.damagedPosition.position : targetTr.position;
                directionToTarget = (targetPos + rangeAttackInfos.TargetDirectionOffset - transform.position).normalized;
                if (Infos.targetingRotateType == ProjectileTargetingRotateType.LOOK_TARGET)
                    targetRotation = Quaternion.LookRotation(directionToTarget);
                movePosition = targetTr.position + rangeAttackInfos.TargetDirectionOffset;
            }

            if (Infos.targetingRotateType == ProjectileTargetingRotateType.LOOK_TARGET)
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Infos.SmoothRotateSpeed);
            else  if(Infos.targetingRotateType == ProjectileTargetingRotateType.CUSTOM_SETTING)
                transform.eulerAngles += Infos.customRotate * Infos.customRotateSpeed;

            if (Infos.IsMoveLerp)
                transform.position = Vector3.Lerp(transform.position, movePosition, currentSpeed * Time.deltaTime);
            else
                if (Vector3.Distance(transform.position, movePosition) > 0.5f)
                    transform.position += directionToTarget * currentSpeed * Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator AutoDetectTarget_Co()
    {
        Transform target = null;
        int count = 0;
        while (target == null)
        {
            count = Physics.OverlapSphereNonAlloc(transform.position, rangeAttackInfos.AutoDetectRange, detectColliders, rangeAttackInfos.TargetLayer);
            if (count > 0)
            {
                rangeAttackInfos.Owner.SortFindEmenyByNearDistance(transform, ref detectColliders);
                if (detectColliders[0] != null)
                    target = detectColliders[0].transform;
            }
            yield return new WaitForSeconds(rangeAttackInfos.AutoDetectDelay);
        }
        
        targetTr = target;
        StopCoroutine(autoDetectTarget_Co);
        autoDetectTarget_Co = null;
    }

    private IEnumerator MovePoint_Co()
    {
        transform.position = transform.position;
        yield return null;
    }

    private IEnumerator MoveWave_Co()
    {
        float timer = 0f;
        Vector3 waveScale = Vector3.zero;
        transform.localScale = Vector3.one * rangeAttackInfos.InitWaveRadius;
        yield return new WaitForSeconds(rangeAttackInfos.ExcuteWaveDelay);
        damageDetectRadius = rangeAttackInfos.InitWaveRadius;

        while (canDamage && timer < rangeAttackInfos.WaveTime)
        {
            timer +=  Time.deltaTime;
            waveScale = (Vector3.one * rangeAttackInfos.InitWaveRadius) + (Vector3.one * (rangeAttackInfos.CurveScale.Evaluate(timer / rangeAttackInfos.WaveTime) * rangeAttackInfos.TargetWaveRadius));
            waveScale.x = Mathf.Clamp(waveScale.x, 0f, rangeAttackInfos.TargetWaveRadius);
            waveScale.y = Mathf.Clamp(waveScale.y, 0f, rangeAttackInfos.TargetWaveRadius);
            waveScale.z = Mathf.Clamp(waveScale.z, 0f, rangeAttackInfos.TargetWaveRadius);
            damageDetectRadius = waveScale.z ;
            transform.localScale = waveScale;
            yield return null;
        }

    }

    private IEnumerator MoveHoming_Co()
    {
        bool canHoming = true;
        float timer = 0;
        Vector3 startPos = SpawnPosition(Infos);
        Vector2 noise = new Vector2(Random.Range(rangeAttackInfos.MinNoise.x, rangeAttackInfos.MaxNoise.x)
                                   , Random.Range(rangeAttackInfos.MinNoise.y, rangeAttackInfos.MaxNoise.y));
        Vector3 BulletDirectionVector = new Vector3(targetTr.position.x, targetTr.position.y, targetTr.position.z) - startPos;
        Vector3 HorizontalNoiseVector = Vector3.Cross(BulletDirectionVector, Vector3.up).normalized;
        // Vector3 targetPos = rangeAttackInfos.TargetDirectionOffset + GetTargetDirection();
        Vector3 targetPos = rangeAttackInfos.TargetDirectionOffset + GetAttackPosition(rangeAttackInfos);
        float noisePosition = 0f;
        while (timer < 1f + 0.1f && canHoming)
        {
            targetPos = rangeAttackInfos.TargetDirectionOffset + GetAttackPosition(rangeAttackInfos);
            noisePosition = rangeAttackInfos.NoiseCurve.Evaluate(timer);
            transform.position = Vector3.Lerp(startPos, targetPos
                , rangeAttackInfos.PositionCurve.Evaluate(timer)) + new Vector3(HorizontalNoiseVector.x * noisePosition * noise.x,
                                                                         noisePosition * noise.y,
                                                                         noisePosition * HorizontalNoiseVector.z * noise.x);
            // transform.LookAt(targetPos);
            // transform.rotation = Quaternion.LookRotation(targetPos);
            timer += Time.deltaTime * currentSpeed;


            yield return null;
        }


        canDamage = false;
        EffectManager.Instance.GetEffectObjectRandom(rangeAttackInfos.collisionEffect, transform.position, Vector3.zero, Vector3.zero);
        pooling.ExcuteCollision(rangeAttackInfos.ReturnTime);
    }


    #endregion

    #region Spawn Type Process

    /// <summary>
    /// 투사체 초기 위치 세팅.
    /// </summary>
    private Vector3 SpawnPosition(RangeAttackInfo info)
    {
        pooling.ActiveChild(false);
        Vector3 projectilePos = !info.isProjectileRandom ? info.projectileEffect.minPosition : EffectManager.Instance.GetRandomValues(info.projectileEffect)[0];

        switch (info.ProjectileSpawnPosition)
        {
            case ProjectileSpawnPosition.WEAPON:
                projectileSpawnPos = info.Owner.weaponPosition.position ;
                break;
            case ProjectileSpawnPosition.RANGEATTACK_SHOOT:
                projectileSpawnPos = info.Owner.rangeShootingPosition.position;
                break;
            case ProjectileSpawnPosition.MAGIC_SHOOT:
                projectileSpawnPos = info.Owner.skillShootingPosition.position;
                break;
            case ProjectileSpawnPosition.TARGET_ZERO:
                if (targetTr != null) projectileSpawnPos = targetTr.position ;
                else projectileSpawnPos = info.Owner.transform.position + (info.Owner.transform.forward * info.MaxDistance);
                break;
            case ProjectileSpawnPosition.TARGET_DAMAGED:
                if (targetTr != null && targetTr.GetComponent<BaseController>())
                    projectileSpawnPos = targetTr.GetComponent<BaseController>().damagedPosition.position;
                else if(targetTr != null)
                    projectileSpawnPos = targetTr.position;
                else projectileSpawnPos = info.Owner.transform.position + (info.Owner.transform.forward * info.MaxDistance) ;
                break;
            case ProjectileSpawnPosition.SELF:
                projectileSpawnPos = info.Owner.transform.position;
                break;
            case ProjectileSpawnPosition.FOR_AFTERATTACK_COLLISION:
                projectileSpawnPos = collisionPos;
                break;
            case ProjectileSpawnPosition.TARGET_FORWARD:
                if (targetTr != null) projectileSpawnPos = targetTr.position + targetTr.forward * info.targetSpawnOffset;
                else projectileSpawnPos = info.Owner.transform.position + (info.Owner.transform.forward * info.MaxDistance);
                break;
            case ProjectileSpawnPosition.TARGET_RIGHT:
                if (targetTr != null) projectileSpawnPos = targetTr.position + targetTr.right * info.targetSpawnOffset;
                else projectileSpawnPos = info.Owner.transform.position + (info.Owner.transform.forward * info.MaxDistance);
                break;
            case ProjectileSpawnPosition.OWNER_TARGET_MIDDLE:
                if (targetTr != null)
                {
                    Vector3 dir = (owner.transform.position - targetTr.position).normalized;
                    dir.y = 0f;
                    dir.Normalize();
                    projectileSpawnPos = targetTr.position + dir * info.targetSpawnOffset;
                }
                else projectileSpawnPos = info.Owner.transform.position + (info.Owner.transform.forward * info.MaxDistance);
                break;
        }

        transform.position = projectileSpawnPos + info.SpawnPositionOffset + projectilePos;
        pooling.ActiveChild(true);
        SoundManager.Instance.PlayEffect(rangeAttackInfos.flashEffect.effectSound);

        return projectileSpawnPos + info.SpawnPositionOffset + projectilePos;
    }

    private void SpawnRotation(RangeAttackInfo info)
    {
        Vector3 projectileRot = !info.isProjectileRandom ? info.projectileEffect.minRotation : EffectManager.Instance.GetRandomValues(info.projectileEffect)[1];
        Vector3 retRot = RetRotation(info);
        transform.rotation = Quaternion.LookRotation(retRot);
        transform.eulerAngles += projectileRot;
    }


    private Vector3 GetAttackPosition(RangeAttackInfo info)
    {
        if (targetTr == null)
           return rangeAttackInfos.Owner.transform.forward;
        else if (rangeAttackInfos.DirectTionType == ThowingDirectionType.TARGET_ZERO)
            return targetTr.position;
        else if (rangeAttackInfos.DirectTionType == ThowingDirectionType.TARGET_DAMAGED)
        {
            Transform damagedPosition = targetTr.GetComponent<BaseController>()?.damagedPosition;
            if (damagedPosition == null)
                return  rangeAttackInfos.Owner.transform.forward;
            else
            {
                return damagedPosition.position;
            }
        }
        else if (rangeAttackInfos.DirectTionType == ThowingDirectionType.PROJECTILE_FORWARD)
            return transform.forward;

        return transform.position;
    }
    #endregion


    #region After Collision Range Attack

    private void ExcuteAfterCollisionRangeAttack(AfterColisionRangeAttackInfo info, Vector3 collisionPos)
      => StartCoroutine(AfterCollisionRangeAttackProcess_Co(info, collisionPos));

    private IEnumerator AfterCollisionRangeAttackProcess_Co(AfterColisionRangeAttackInfo info, Vector3 collisionPos)
    {
        if (info == null) yield break;
        yield return new WaitForSeconds(info.ExcuteDelay);

        for (int i = 0; i < info.CreateCount; i++)
        {
            RangeAttackProjectile projectile = EffectManager.Instance.GetEffectObjectRandom(info.infos.projectileEffect, transform.position, Vector3.zero, Vector3.zero).GetComponent<RangeAttackProjectile>();
            if (projectile == null) continue;

            RangeAttackInfo rangeInfo = info.infos.Clone();
            rangeInfo.SpawnPositionOffset += info.CreateSpawnPosition[i];

            projectile.CollisionPos = collisionPos;
            if (info.RotateToTarget && targetTr != null)
            {
                Vector3 dir = (targetTr.position - projectile.transform.position).normalized;
                projectile.transform.rotation = Quaternion.LookRotation(dir);
            }
            else
                projectile.transform.rotation = transform.rotation;

            projectile.transform.rotation *= Quaternion.Euler(info.CreateRotation[i]);
            projectile.Setting(rangeAttackInfos.Owner, targetTr, rangeInfo, null);

            yield return new WaitForSeconds(info.CreateDelay);
        }

    }
    #endregion

    public void Setting(BaseController owner, Transform targetTr, RangeAttackInfo infos, ProjectileCreatorInfo creatorInfo)
    {
        if (pooling == null)
            pooling = GetComponent<ReturnObjectToObjectPooling>();

        if (owner != null) this.owner = owner;
        this.afterInfos = infos.afterInfos != null ? infos.afterInfos.Clone() : null;
        rangeAttackInfos = infos.Clone();
        rangeAttackInfos.Setting(owner);
        this.targetTr = targetTr;
        targetContr = targetTr?.GetComponent<BaseController>();
        damageDetectRadius = 0f;
        projectileDistance = 0f;
        canDamage = true;
        isDetectObstacle = false;
        pooling.TimeSetting(rangeAttackInfos.ReturnTime, 0f);
        if (creatorInfo != null)
            rangeAttackInfos.SpawnPositionOffset = Vector3.zero;

        if (rangeAttackInfos.rangeAttackType == RangeAttackType.SINGLE)
            pooling.onSetPollingData += () =>
            {
                if (canDamage)
                    EffectManager.Instance.GetEffectObjectRandom(rangeAttackInfos.collisionEffect, transform.position, Vector3.zero, Vector3.zero);
            };

        //위치 세팅하기.
        SpawnPosition(rangeAttackInfos);
        //해당 타입에 맞는.코루틴 실행
        SpawnRotation(rangeAttackInfos);
        MoveProcess(rangeAttackInfos);
        //데미지 검출 시작.
        StartDmagedProcess(rangeAttackInfos);
        StartCoroutine(DetectObstacle());
        StartCoroutine(DetectMaxDistance());

        if (rangeAttackInfos.UseDotDamage)
        {
            Debug.Log("Use!");
            StartCoroutine(DotDetectProcess_Co(rangeAttackInfos.DotInfo));
            StartCoroutine(DotRegisterProcess_Co(rangeAttackInfos.DotInfo));
            if (rangeAttackInfos.DotInfo.eachDotCoolTime)
                StartCoroutine(DotCheck(rangeAttackInfos.DotInfo));
            //밑에는 충돌했을때(single), pene(은 음..)
            StartCoroutine(rangeAttackInfos.DotInfo.CheckStayTime());
        }
    }



    public bool CheckTargetInAngle(Transform owner, Transform target, float angle)
    {
        Vector3 dir = target.transform.position - owner.transform.position;
        dir.y = 0f;
        dir.Normalize();

        if (Mathf.Abs(Vector3.Angle(dir, owner.transform.forward)) <= angle / 2f)
            return true;
        return false;
    }

    private LayerMask GetTargetLayer(RangeAttackInfo info)
    {
        LayerMask retTargetLayer = info.Owner.targetLayer;
        switch (info.targetDetectType)
        {
            case ProjectileTargetDetectType.OWNER_OBSTACLE: retTargetLayer = info.Owner.obstacleLayer; break;
            case ProjectileTargetDetectType.OWNER_OBSTACLE_SELECT:  retTargetLayer = info.targetDetectLayer + info.Owner.obstacleLayer; break;
            case ProjectileTargetDetectType.OWNER_TARGET:  retTargetLayer = info.TargetLayer; break;
            case ProjectileTargetDetectType.OWNER_TARGET_OBSTACLE: retTargetLayer = info.TargetLayer + info.Owner.obstacleLayer;  break;
            case ProjectileTargetDetectType.OWNER_TARGET_SELECT: retTargetLayer = info.TargetLayer;  break;
            case ProjectileTargetDetectType.SELECT:  retTargetLayer = info.targetDetectLayer;   break;
            case ProjectileTargetDetectType.OWNER_TARGET_OBSTACLE_SELECT:
                retTargetLayer = info.TargetLayer + info.Owner.obstacleLayer + info.targetDetectLayer;
                break;
        }
        return retTargetLayer;
    }

    private Vector3 RetRotation( RangeAttackInfo info)
    {
        Vector3 retRotation = Vector3.zero;
        switch (info.rotationType)
        {
            case ProjectileRotationType.ZERO:
                retRotation = Vector3.zero;
                break;
            case ProjectileRotationType.OWNER_ROTATION:
                retRotation = owner.transform.rotation.eulerAngles;
                break;
            case ProjectileRotationType.OWNER_FORWARD:
                retRotation = owner.transform.forward;
                break;
            case ProjectileRotationType.OWNER_TO_TARGET_DIRECT:
                if (targetTr == null) retRotation = owner.transform.forward;
                else
                    retRotation = ((targetTr.position + info.targetDirectionOffset) - owner.transform.position).normalized;
                break;
            case ProjectileRotationType.OWNER_TO_TARGET_DIRECT_YZERO:
                if (targetTr == null) retRotation = owner.transform.forward;
                else
                {
                    Vector3 dir = ((targetTr.position + info.targetDirectionOffset) - owner.transform.position);
                    dir.y = 0f;
                    retRotation = dir.normalized;
                }
                break;
            case ProjectileRotationType.PROJECTILE_TO_TARGET_ZERO:
                if (targetTr == null) retRotation = Vector3.zero;
                else
                    retRotation = ((targetTr.position + info.targetDirectionOffset) - transform.position).normalized;
                break;
            case ProjectileRotationType.PROJECTILE_FORWARD:
                retRotation = transform.forward;
                break;
            case ProjectileRotationType.TARGET_DAMAGED:
                BaseController targetDamaged = targetTr?.GetComponentInChildren<BaseController>();
                if (targetTr == null || targetDamaged == null)
                {
                    retRotation = owner.transform.forward;
                }
                else
                {
                    retRotation = ((targetDamaged.damagedPosition.position + info.targetDirectionOffset) - transform.position).normalized;
                }
                break;
        }

        return retRotation;
    }



    #region Dot Process
    private IEnumerator DotDetectProcess_Co(DotAttackInfo info)
    {
        while (canDot)
        {
            if (info.IsDone) yield break;

            if (prevPosition != transform.position)
            {
                prevPosition = transform.position;
                info.AddDotPointPosition(transform.position);
                if(info.DotPointPositions.Count >= 2)
                    info.CreateEffect(info.DotPointPositions.Count - 2, info.DotPointPositions.Count - 1);
                yield return new WaitForSeconds(info.PointDelayTime);
            }
            yield return new WaitForSeconds(info.PointDelayTime);

        }

    }
    private IEnumerator DotRegisterProcess_Co(DotAttackInfo info)
    {
        info.SetTargetLayerMask(rangeAttackInfos.TargetLayer);
        while (canDot)
        {
            if (info.IsDone) yield break;

            info.CheckAndRegisterTargetInDotArea(ref detectColliders);
            info.ExcuteDamage(rangeAttackInfos.Owner);
            yield return new WaitForSeconds(0.1f);
        }

    }
    private IEnumerator DotCheck(DotAttackInfo info)
    {
        while (canDot || info.DotPointPositions.Count > 0)
        {
            info.CheckRegisterTimer();
            info.CheckDotDone();
            yield return null;
        }

    }
    #endregion


    private void CheckMaxDistance(Transform owner, float maxDistance)
    {
        if (owner == null) return;

        float currentDistance = (transform.position - owner.position).magnitude;
        //if(currentDistance >= maxDistance)   오브젝트풀링화하기.
    }


    private void ResetColliders()
    {
        for (int i = 0; i < detectColliders.Length; i++)
            detectColliders[i] = null;
    }


    private void OnDrawGizmos()
    {
        if (rangeAttackInfos == null)
           return;

        if (rangeAttackInfos.RangeMoveType == RangeMoveType.WAVE)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, damageDetectRadius);
        }
        else if (rangeAttackInfos.ProjectileDetectType == ProjectileDetectType.CUBE)
        {
            // Gizmos.color = Color.red;
            // Gizmos.DrawWireCube(transform.position + rangeAttackInfos.ProjectileDetectCubePosition, rangeAttackInfos.ProjectileCubeSize);
            Vector3 boxPosition = transform.position + transform.TransformDirection(rangeAttackInfos.ProjectileDetectCubePosition);
            Quaternion rotation = Quaternion.LookRotation(transform.forward); 
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(boxPosition, rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, rangeAttackInfos.ProjectileCubeSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, rangeAttackInfos.DetectTargetRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, rangeAttackInfos.detectObstacleRadius);
        }

        if (rangeAttackInfos.UseDotDamage && !rangeAttackInfos.DotInfo.IsDone)
        {
            rangeAttackInfos.DotInfo.Draw();
        }

        //폭발 범위.
        if(rangeAttackInfos.RangeAttackType == RangeAttackType.POINT_EXPLOSION)
        {
            for (int i = 0; i < rangeAttackInfos.DamageRange.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, rangeAttackInfos.damageRange[i]);
            }

        }

        if(targetTr != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(targetTr.position, 0.5f);
        }
        if (owner != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(owner.transform.position, 0.5f);
        }


        if (rangeAttackInfos != null && targetTr != null)
        {
            if (targetTr.position == Vector3.zero) return;

            // 외적 벡터 계산
            Vector3 startPos = transform.position;
            Vector3 targetDirection = targetTr.position - startPos;
            Vector3 horizontalNoiseVector = Vector3.Cross(targetDirection, Vector3.up).normalized;

            // 시작 위치 및 타겟 위치 설정
            Vector3 startGizmoPos = startPos;
            Vector3 targetGizmoPos = targetTr.position + rangeAttackInfos.TargetDirectionOffset;

            // 외적 벡터를 사용하여 시작 위치와 타겟 위치 사이에 라인 그리기
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startGizmoPos, targetGizmoPos);

            // 외적 벡터 그리기
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startGizmoPos, startGizmoPos + horizontalNoiseVector);

            // 시작 위치와 타겟 위치의 점 그리기
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(startGizmoPos, 0.1f);
            Gizmos.DrawSphere(targetGizmoPos, 0.1f);
        }
    }
}
