using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum RangeAttackType
{
    SINGLE = 0,
    POINT_EXPLOSION = 1,
    PENETRATE = 2,

}

public enum RangeMoveType
{
    THROWING = 0,
    POINT = 1,   //해당 위치에 생성후 이동x
    TARGETING = 2,
    HOMING = 3,
    WAVE = 4,
}

public enum ProjectileSpawnPosition
{
    WEAPON = 0,
    RANGEATTACK_SHOOT = 1,
    MAGIC_SHOOT = 2,
    TARGET_ZERO = 3,
    TARGET_DAMAGED =4,
    SELF = 5,
    FOR_AFTERATTACK_COLLISION = 6,
    OWNER_TARGET_MIDDLE = 7,
    TARGET_FORWARD = 8,
    TARGET_RIGHT = 9,
}

public enum ProjectileRotationType
{
    ZERO = -1,
    OWNER_ROTATION = 0,
    OWNER_FORWARD = 1,
    OWNER_TO_TARGET_DIRECT = 2,
    OWNER_TO_TARGET_DIRECT_YZERO = 3,
    PROJECTILE_TO_TARGET_ZERO = 4,
    PROJECTILE_FORWARD = 5,
    TARGET_DAMAGED =6,

}

public enum ThowingDirectionType 
{
    TARGET_DAMAGED = 0,
    TARGET_ZERO = 1,
    PROJECTILE_FORWARD = 2,
}

public enum SubjectDirectionType
{
    OWNER_ZERO = 0,
    OWNER_SHOOTPOSITION =1,
    PROJECTILE = 2,
}

public enum ProjectileDetectType
{
    SPHERE = 0,
    CUBE =1, 
}

public enum ProjectileMoveDirectType
{
    FORWARD =0,
    BACK = 1,
    RIGHT = 2,
    LEFT =3,
    UP= 4,
    DOWN = 5,
}

public enum ProjectileCollisionDetectType
{
    OWNER_OBSTACLE = 0,
    SELECT = 1,
}
public enum ProjectileTargetDetectType
{
    OWNER_TARGET = 0,
    OWNER_TARGET_OBSTACLE = 1,
    OWNER_OBSTACLE =2 ,
    OWNER_TARGET_SELECT = 3,
    OWNER_TARGET_OBSTACLE_SELECT =4,
    OWNER_OBSTACLE_SELECT= 5,
    SELECT = 6,
}
                                       
public enum ProjectileTargetingRotateType
{
    LOOK_TARGET = 0,
    CUSTOM_SETTING = 1,
    NOT_ROTATE = 2,
}

//보류?
//public enum IndicatorType
//{
//    NONE = -1,
//    AFTER_ATTACK = 0,
//    IMMEDIATE_ATTACK = 1,
//}

[CreateAssetMenu(menuName = "Data/Attack Data/Range Attack Info")]
public class RangeAttackInfo   :ScriptableObject
{
    private BaseController owner = null;
    public bool isSkill = false;
    public RangeAttackType rangeAttackType = RangeAttackType.SINGLE;
    public RangeMoveType rangeMoveType = RangeMoveType.THROWING;
    public ProjectileMoveDirectType moveDirectType = ProjectileMoveDirectType.FORWARD;
    public ProjectileRotationType rotationType = ProjectileRotationType.ZERO;
    public ProjectileSpawnPosition projectileSpawnPosition = ProjectileSpawnPosition.MAGIC_SHOOT;
    public ProjectileCollisionDetectType collisionDetectType = ProjectileCollisionDetectType.OWNER_OBSTACLE;
    public ProjectileTargetDetectType targetDetectType = ProjectileTargetDetectType.OWNER_TARGET;
    public Vector3 spawnPositionOffset = Vector3.zero;
    public ProjectileDetectType projectileDetectType = ProjectileDetectType.SPHERE;
    public Vector3 projectileDetectCubePosition = Vector3.zero;
    public Vector3 projectileCubeSize = Vector3.zero;
    public LayerMask collisionDetectLayer;
    public LayerMask targetDetectLayer;

    [SerializeField, HideInInspector] private List<bool> isHitRandom = new List<bool>();
    [SerializeField, HideInInspector] public List<RandomEffectInfo> hitEffects = new List<RandomEffectInfo>();
    public List<TimeData> timeDatas = new List<TimeData>();
    public bool onlyTimeDataIndexZero = false;
    public bool isCollisionRandom;
    public RandomEffectInfo collisionEffect ;
    public bool isFlashRandom;
    public RandomEffectInfo flashEffect;
    public bool isProjectileRandom;
    public RandomEffectInfo projectileEffect;



    public List<AttackStrengthType> strengthType = new List<AttackStrengthType>();
    public List<float> damage = new List<float>();
    public List<float> explosionAngle = new List<float>();
    public List<float> damageRange = new List<float>();
    public List<float> hitDelay = new List<float>();
    public bool delayExplosionAttack = false;  //여러번 검사후 공격
    public bool allowPenetrateSameEnemy = false; // 관통공격 같은적 허용?
    public float samePenetrateDelayTime = 1f;
    public int maxSamePenetrateCount = 1;
    public float detectObstacleRadius = 1f;  //투사체가 장애물과 충돌할 범위
    public float detectTargetRadius = 2f; //투사체가 적 탐지할 범위
   // public float canShootDistance = 10f;   //투사체 발사 가능 거리.
    public int maxTargetCount = 1;
    public float maxDistance = 0f;
    public float speed = 0f;
    public float returnTime = 5f;
    private bool isCritical = false;
    private LayerMask targetLayer;
    public bool isRepeatDamage = false;
    public int repeatCount = 0;
    public float repeatDelayTime = 0f;
    public float targetSpawnOffset = 0f;
    

    //Throwing || Homing || Targeting(?) 용      타겟팅은 이동할 위치?.
    public SubjectDirectionType subjectDirectionType = SubjectDirectionType.OWNER_ZERO;
    public ThowingDirectionType directTionType = ThowingDirectionType.TARGET_DAMAGED;
    public Vector3 targetDirectionOffset = Vector3.zero;


    [Header("Dot Damage")]
    public bool useDotDamage = false;
    public DotAttackInfo dotInfo = null;

    [Header("Acceleration")]
    public  bool isAccelerate = true; //가속
    public  float initSpeed = 0.5f;
    public  float maxSpeed = 100f;
    public  float additiveSpeed = 1f;
    public float additiveDelayTime = 0.01f;

    [Header("Homing")]
    public AnimationCurve noiseCurve = new AnimationCurve(new Keyframe(0f,0f),
                                                          new Keyframe(0.2f,1f),
                                                          new Keyframe(1f,0f));
    public AnimationCurve positionCurve = new AnimationCurve(new Keyframe(0f,0f),
                                                             new Keyframe(1f,1f));
    public Vector2 minNoise = new Vector2(-0.5f, 0f);
    public Vector2 maxNoise = new Vector2(0.5f, 0f);

    [Header("Targeting")]
    public float excuteTargetingDelay = 0f;
    public  bool isMoveLerp = false;
    public bool autoDetectTarget = false;
    public ProjectileTargetingRotateType targetingRotateType = ProjectileTargetingRotateType.NOT_ROTATE;
    public bool stillMoveToTarget = true;
    public float autoDetectDelay = 0.2f;
    public float autoDetectRange = 5f;
    public  float updateTargetingDelay = 0.2f;
    public  float smoothRotateSpeed = 5f;
    public Vector3 customRotate = Vector3.zero;
    public float customRotateSpeed = 1f;

    [Header("Wave Attack")]
    public float excuteWaveDelay = 0.2f;
    public float waveTime = 2f;
    public float initWaveRadius = 0f;
    public float targetWaveRadius = 5f;
    public AnimationCurve curveScale;

    [Header("After Attack Info")]
    public AfterColisionRangeAttackInfo afterInfos;



    #region Getter Setter
    public BaseController Owner => owner;

    public bool IsSkill { get { return isSkill; } set { isSkill = value; } }
    public RangeAttackType RangeAttackType { get { return rangeAttackType; } set { rangeAttackType = value; } }
    public RangeMoveType RangeMoveType { get { return rangeMoveType; } set { rangeMoveType = value; } }
    public ProjectileSpawnPosition ProjectileSpawnPosition { get { return projectileSpawnPosition; } set { projectileSpawnPosition = value; } }
    public ProjectileDetectType ProjectileDetectType { get { return projectileDetectType; } set { projectileDetectType = value; } }
    public Vector3 ProjectileCubeSize { get { return projectileCubeSize; } set { projectileCubeSize = value; } }
    public Vector3 ProjectileDetectCubePosition { get { return projectileDetectCubePosition; } set { projectileDetectCubePosition = value; } }

    public List<bool> IsHitRandom { get { return isHitRandom; } set { isHitRandom = value; } }
    public bool IsCollisionRandom { get { return isCollisionRandom; } set { isCollisionRandom = value; } }

    public RandomEffectInfo CollisionEffects { get { return collisionEffect; } set { collisionEffect = value; } }
    public List<RandomEffectInfo> HitEffects { get { return hitEffects; } set { hitEffects = value; } }
    public bool IsFlashRandom { get { return isFlashRandom; } set { isFlashRandom = value; } }

    public RandomEffectInfo FlashEffect { get { return flashEffect; } set { flashEffect = value; } }
    public bool IsProjectileRandom { get { return isProjectileRandom; } set { isProjectileRandom = value; } }

    public RandomEffectInfo ProjectileEffect { get { return projectileEffect; } set { projectileEffect = value; } }

    public List<AttackStrengthType>StrengthType { get { return strengthType; } set { strengthType = value; } }

    public List<float> Damage { get { return damage; } set { damage = value; } }
    public List<float> ExplosionAngle { get { return explosionAngle; } set { explosionAngle = value; } }
    public List<float> DamageRange { get { return damageRange; } set { damageRange = value; } }
    public List<float> HitDelay { get { return hitDelay; } set { hitDelay = value; } }
    public int HitCount => Damage.Count;

    public Vector3 SpawnPositionOffset { get { return spawnPositionOffset; } set { spawnPositionOffset = value; } }
    //public Vector3 SpawnRotationOffset { get { return spawnRotationOffset; } set { spawnRotationOffset = value; } }
    public float DetectTargetRadius { get { return detectTargetRadius; } set { detectTargetRadius = value; } }
    //public float CanShootDistance { get { return canShootDistance; } set { canShootDistance = value; } }
    public int MaxTargetCount { get { return maxTargetCount; } set { maxTargetCount = value; } }
    public float MaxDistance { get { return maxDistance; } set { maxDistance = value; } }
    public float Speed { get { return speed; } set { speed = value; } }

    public float ReturnTime { get { return returnTime; } set { returnTime = value; } }
    public LayerMask TargetLayer { get { return targetLayer; } set { targetLayer = value; } }

    public bool DelayExplosionAttack { get { return delayExplosionAttack; } set { delayExplosionAttack = value; } }
    public bool UseDotDamage { get { return useDotDamage; } set { useDotDamage = value; } }
    public DotAttackInfo DotInfo { get { return dotInfo; } set { dotInfo = value; } }
    public SubjectDirectionType SubjectDirectionType { get { return subjectDirectionType; } set { subjectDirectionType = value; } }
    public ThowingDirectionType DirectTionType { get { return directTionType; } set { directTionType = value; } }
    public Vector3 TargetDirectionOffset { get { return targetDirectionOffset; } set { targetDirectionOffset = value; } }
    public bool AutoDetectTarget { get { return autoDetectTarget; } set { autoDetectTarget = value; } }
    public float SmoothRotateSpeed { get { return smoothRotateSpeed; } set { smoothRotateSpeed = value; } }
    public bool AllowPenetrateSameEnemy { get { return allowPenetrateSameEnemy; } set { allowPenetrateSameEnemy = value; } }
    public float SamePenetrateDelayTime { get { return samePenetrateDelayTime; } set { samePenetrateDelayTime = value; } }
    public int MaxSamePenetrateCount { get { return maxSamePenetrateCount; } set { maxSamePenetrateCount = value; } }
    
    public bool IsMoveLerp { get { return isMoveLerp; } set { isMoveLerp = value; } }
    public float AutoDetectDelay { get { return autoDetectDelay; } set { autoDetectDelay = value; } }
    public float AutoDetectRange { get { return autoDetectRange; } set { autoDetectRange = value; } }
    public float ExcuteTargetingDelay { get { return excuteTargetingDelay; } set { excuteTargetingDelay = value; } }
    public float UpdateTargetingDelay { get { return updateTargetingDelay; } set { updateTargetingDelay = value; } }
    public float WaveTime { get { return waveTime; } set { waveTime = value; } }
    public float ExcuteWaveDelay { get { return excuteWaveDelay; } set { excuteWaveDelay = value; } }
    public float InitWaveRadius { get { return initWaveRadius; } set { initWaveRadius = value; } }
    public float TargetWaveRadius { get { return targetWaveRadius; } set { targetWaveRadius = value; } }
    public AnimationCurve CurveScale { get { return curveScale; } set { curveScale = value; } }

    public AnimationCurve NoiseCurve { get { return noiseCurve; } set { noiseCurve = value; } }
    public AnimationCurve PositionCurve { get { return positionCurve; } set { positionCurve = value; } }
    public Vector2 MinNoise { get { return minNoise; } set { minNoise = value; } }
    public Vector2 MaxNoise { get { return maxNoise; } set { maxNoise = value; } }

    public bool IsAccelerate { get { return isAccelerate; } set { isAccelerate = value; } }
    public float InitSpeed { get { return initSpeed; } set { initSpeed = value; } }
    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
    public float AdditiveSpeed { get { return additiveSpeed; } set { additiveSpeed = value; } }
    public float AdditiveDelayTime { get { return additiveDelayTime; } set { additiveDelayTime = value; } }
    public AfterColisionRangeAttackInfo AfterAttackInfo { get { return afterInfos; } set { afterInfos = value; } }
    #endregion

    public void Setting(BaseController setOwner)
    {
        if (setOwner == null) return;
        owner = setOwner;
        targetLayer = owner.targetLayer;
    }

    public void Setting(LayerMask targetLayer)
    {
        this.targetLayer = targetLayer;
    }

    public RangeAttackInfo Clone()
    {
        RangeAttackInfo clone = new RangeAttackInfo();
        clone.owner = owner;
        clone.isSkill = isSkill;
        clone.rangeAttackType = rangeAttackType;
        clone.rangeMoveType = rangeMoveType;
        clone.projectileSpawnPosition = projectileSpawnPosition;
        clone.spawnPositionOffset = spawnPositionOffset;
        clone.collisionDetectLayer = collisionDetectLayer;
        clone.targetLayer = targetLayer;
        clone.targetDetectType = targetDetectType;
        clone.rotationType = rotationType;
        clone.moveDirectType = moveDirectType;
        clone.projectileDetectType = projectileDetectType;
        clone.projectileCubeSize = projectileCubeSize;
        clone.projectileDetectCubePosition = projectileDetectCubePosition;
        clone.projectileEffect = projectileEffect;
        clone.isCollisionRandom = isCollisionRandom;
        clone.collisionDetectType = collisionDetectType;
        clone.isHitRandom = new List<bool>(isHitRandom);
        clone.collisionEffect = collisionEffect;
        clone.hitEffects = new List<RandomEffectInfo>(hitEffects);
        clone.isFlashRandom = isFlashRandom;
        clone.targetSpawnOffset = targetSpawnOffset;
        clone.targetDetectLayer =  targetDetectLayer;
        clone.targetingRotateType = targetingRotateType;
        clone.flashEffect = flashEffect;
        clone.stillMoveToTarget = stillMoveToTarget;
        clone.excuteTargetingDelay = excuteTargetingDelay;
        clone.customRotate = customRotate;
        clone.customRotateSpeed = customRotateSpeed;
        clone.isRepeatDamage = isRepeatDamage;
        clone.repeatCount = repeatCount;
        clone.repeatDelayTime = repeatDelayTime;
        clone.timeDatas = new List<TimeData>(timeDatas);
        clone.onlyTimeDataIndexZero = onlyTimeDataIndexZero;

        clone.strengthType = new List<AttackStrengthType>(strengthType);
        clone.subjectDirectionType = subjectDirectionType;
        clone.directTionType = directTionType;
        clone.targetDirectionOffset = targetDirectionOffset;
        clone.damage = new List<float>(damage);
        clone.explosionAngle = new List<float>(explosionAngle);
        clone.damageRange = new List<float>(damageRange);
        clone.hitDelay = new List<float>(hitDelay);
        clone.delayExplosionAttack = delayExplosionAttack;
        clone.detectTargetRadius = detectTargetRadius;
        clone.detectObstacleRadius = detectObstacleRadius;
        clone.allowPenetrateSameEnemy = allowPenetrateSameEnemy;
        clone.samePenetrateDelayTime = samePenetrateDelayTime;
        clone.maxSamePenetrateCount = maxSamePenetrateCount;
       // clone.canShootDistance = canShootDistance;
        clone.maxTargetCount = maxTargetCount;
        clone.maxDistance = maxDistance;
        clone.speed = speed;
        clone.isCritical = isCritical;
        clone.targetLayer = targetLayer;
        clone.useDotDamage = useDotDamage;
        if(clone.useDotDamage)
            clone.dotInfo = dotInfo.Clone();
        clone.isAccelerate = isAccelerate;
        clone.initSpeed = initSpeed;
        clone.returnTime = returnTime;
        clone.maxSpeed = maxSpeed;
        clone.additiveSpeed = additiveSpeed;
        clone.additiveDelayTime = additiveDelayTime;
        clone.noiseCurve = noiseCurve;
        clone.positionCurve = positionCurve;
        clone.minNoise = minNoise;
        clone.maxNoise = maxNoise;
        clone.updateTargetingDelay = updateTargetingDelay;
        clone.autoDetectTarget = autoDetectTarget;
        clone.autoDetectDelay = autoDetectDelay;
        clone.isMoveLerp = isMoveLerp;
        clone.autoDetectRange = autoDetectRange;
        clone.smoothRotateSpeed = smoothRotateSpeed;
        clone.excuteWaveDelay = excuteWaveDelay;
        clone.waveTime = waveTime;
        clone.initWaveRadius = initWaveRadius;
        clone.targetWaveRadius = targetWaveRadius;
        clone.curveScale = curveScale;
        if (afterInfos != null)
            clone.afterInfos = afterInfos.Clone();

        return clone;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);

        if (damage.Count > 0 )
        {
            for (int i = 0; i < damage.Count; i++)
            {
                if (isHitRandom.Count != damage.Count)
                    isHitRandom.Add(true);
                if (hitEffects.Count != damage.Count)
                    hitEffects.Add(new RandomEffectInfo());
                if (explosionAngle.Count != damage.Count)
                    explosionAngle.Add(0f);
                if (timeDatas.Count != damage.Count)
                    timeDatas.Add(null);
            }
        }
    }
#endif
}


