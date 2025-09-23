using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootProjectileObstacleDirectType { NONE = -1, NOT_ROTATE = 0, AUTO_ROTATE = 1, TARGETING = 2 }
public enum ShootProjectileObstacleType { NONE = -1, AUTO_SHOOT = 0, DETECT_SHOOT = 1 }


public class ShootProjectileObstacle : BaseController
{
    [Header("Shoot Proectile Obstacle")]
    //public EffectList projectileObpList = EffectList.None;
    //public RangeAttackInfo projectileInfo = null;
    public ProjectileCreator projectileCreatorInfo;
    public Transform rotateTr = null;
    public ShootProjectileObstacleDirectType directType = ShootProjectileObstacleDirectType.NONE;
    public ShootProjectileObstacleType shootType = ShootProjectileObstacleType.NONE;

    [Header("Settings")]
    [SerializeField] private float atkDamage = 5f;
    [SerializeField] private float skillDamage = 10f;
    [SerializeField] private float shootCoolTime = 0f;
    [SerializeField] private float currentShootCoolTimer = 0f; // hide
    [SerializeField] private Vector2 rangeRotate = new Vector2(0, 180f);   // -180 ~ 180
    private float distance = 0f;
    private bool isDead = false;

    [Header("Direct Auto Rotate")]
    [SerializeField] private bool isSmoothRotate = true;
    [SerializeField] private float rotateCycle = 0f;
    [SerializeField] private float perRotateValue;
    [SerializeField] private float reachedWaitTime = 0f;
    [SerializeField] private float autoRotSmooth = 1f;
    private float currentRotateTimer = 0f;
    private float originEulerAngleY;
    private Vector3 originDir;
    private Vector3 minDirEulerY;
    private Vector3 maxDirEulerY;
    private Vector3 targetDir;
    private float targetAngle;
    [SerializeField] private float currentAngle;
    [SerializeField] private float nextRotAngle;
    private Quaternion lookRot;
    private Quaternion smoothLerpRot;
   private Quaternion retRot;
   private bool isReachedRot = true;
   private bool isRangeRot = false;
   private bool isWaitRot = false;
    private float waitTimer = 0f;



    [Header("Direct Targeting")]
    [SerializeField] private bool onlyHorizontalRotateTr = false;
    [SerializeField] private float detectTargetRange = 3f;
    [SerializeField] private float targetingSmooth = 1f;
    private int detectCount = 0;
    private BaseController target = null;
    private Collider[] colls = new Collider[10];

    void Start()
    {
        SetOriginForward();
        currentShootCoolTimer = shootCoolTime;
        isDead = false;
    }

    public void SetOriginForward()
    {
        originDir = transform.forward;
        originEulerAngleY = transform.eulerAngles.y;
        minDirEulerY = GetAngleDirection(rangeRotate.x, originEulerAngleY);
        maxDirEulerY = GetAngleDirection(rangeRotate.y, originEulerAngleY);

    }

    private void Update()
    {
        if (target == null && currentShootCoolTimer < shootCoolTime)
            currentShootCoolTimer += Time.deltaTime;

        if (directType == ShootProjectileObstacleDirectType.TARGETING || shootType == ShootProjectileObstacleType.DETECT_SHOOT)
            DetectTarget();

        ExcuteDirectRotate();
        CheckTargetNull();

        if (shootType == ShootProjectileObstacleType.DETECT_SHOOT && target)
            ShootProcess();
        else if (shootType == ShootProjectileObstacleType.AUTO_SHOOT)
            ShootProcess();

    }

    private bool IsTargetInDetectRange()
    {
        if (target == null) return false;

        distance = (target.transform.position - transform.position).magnitude;
        if (distance <= detectTargetRange)
            return true;
        return false;
    }

    private void DetectTarget()
    {
        detectCount = Physics.OverlapSphereNonAlloc(transform.position, detectTargetRange, colls, targetLayer);

        if (detectCount > 0)
        {
            for (int i = 0; i < detectCount; i++)
            {
                BaseController controller = colls[i].GetComponent<BaseController>();
                if (controller == null || IsDetectObstacle(damagedPosition ,controller.damagedPosition))
                    continue;
                target = controller;
                CheckTargetNull();
            }
        }
    }

    private void CheckTargetNull()
    {
        if (target == null) return;
        if (!IsTargetInDetectRange()) target = null;
        else if (!CheckTargetInAngle()) target = null;
        else if (target.IsDead()) target = null;
        else if (IsDetectObstacle(rangeShootingPosition, target.damagedPosition)) target = null;
    }

    private void ShootProcess()
    {
        currentShootCoolTimer += Time.deltaTime;
        if (currentShootCoolTimer >= shootCoolTime)
        {
            currentShootCoolTimer = 0f;
            // GameObject go = EffectManager.Instance.GetEffectObject(projectileObpList, rangeShootingPosition.position, Vector3.zero, Vector3.one);
            // go.transform.rotation = transform.rotation;
            // go.GetComponent<RangeAttackProjectile>()?.Setting(this, GetTarget(), projectileInfo);
            projectileCreatorInfo.ExcuteCreate(this, target.transform, this);
        }
    }

    private void ExcuteDirectRotate()
    {
        if (directType == ShootProjectileObstacleDirectType.NOT_ROTATE) return;

        if (directType == ShootProjectileObstacleDirectType.AUTO_ROTATE)
            RotateAutoRotate();
        else if (directType == ShootProjectileObstacleDirectType.TARGETING)
            RotateTargeting();
    }

    private Transform GetTarget()
    {
        if (directType == ShootProjectileObstacleDirectType.AUTO_ROTATE && shootType == ShootProjectileObstacleType.DETECT_SHOOT)
            return null;
        else if (directType == ShootProjectileObstacleDirectType.NOT_ROTATE && shootType == ShootProjectileObstacleType.DETECT_SHOOT)
            return null;
        else if (target != null)
            return target.transform;

        return null;
    }



    private void RotateAutoRotate()
    {
        currentRotateTimer += Time.deltaTime;
        if (currentRotateTimer >= rotateCycle && isReachedRot)
        {
            Quaternion nextQua = transform.rotation * Quaternion.Euler(Vector3.up * perRotateValue);
            Vector3 nextAn = nextQua * Vector3.forward;
            nextRotAngle = Vector3.SignedAngle(originDir, nextAn, Vector3.up);

            if (nextRotAngle < rangeRotate.x)
            {
                perRotateValue = -perRotateValue;
                retRot = Quaternion.LookRotation(minDirEulerY);
                isRangeRot = true;
            }
            else if (nextRotAngle > rangeRotate.y)
            {
                perRotateValue = -perRotateValue;
                retRot = Quaternion.LookRotation(maxDirEulerY);
                isRangeRot = true;
            }
            else
                retRot = nextQua;

            isReachedRot = false;
        }

        if(!isReachedRot)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, retRot, autoRotSmooth * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, retRot) < 0.5f)
            {
                if (isRangeRot)
                {
                    if (WaitRot())
                    {
                        waitTimer = 0f;
                        isReachedRot = true;
                        isRangeRot = false;
                        currentRotateTimer = 0f;
                    }
                }
                else
                {
                    isReachedRot = true;
                    currentRotateTimer = 0f;
                }

            }
        }
    }

    private bool WaitRot()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= reachedWaitTime)
            return true;
        return false;
    }


    private void RotateTargeting()
    {
        if (target == null) return;
        Vector3 dir = (target.damagedPosition.position - damagedPosition.position);
        if (onlyHorizontalRotateTr) dir.y = 0f;
        dir.Normalize();

        lookRot = Quaternion.LookRotation(dir);
        if (CheckInRangeAngle(dir))
            smoothLerpRot = Quaternion.Lerp(transform.rotation, lookRot, targetingSmooth * Time.deltaTime);

        transform.rotation = smoothLerpRot;

    }

    private bool CheckTargetInAngle()
    {
        if (target == null) return false;
        targetDir = (target.transform.position - transform.position).normalized;
        targetAngle = Vector3.SignedAngle(originDir, targetDir, Vector3.up);
        if (targetAngle > rangeRotate.x && targetAngle < rangeRotate.y)
        {
            return true;
        }
        return false;
    }

    private bool CheckInRangeAngle(Vector3 transformForward)
    {
        currentAngle =  Vector3.SignedAngle(originDir, transformForward, Vector3.up);
        if (currentAngle > rangeRotate.x && currentAngle < rangeRotate.y)
            return true;

        return false;
    }


    public override bool IsDead() => isDead;

    public override void Damaged(float damage, BaseController attacker, bool isCritical, bool isSkill,AttackStrengthType attackStrengthType, bool isForceDmg = false)
    {
    }

    public override (bool, float) GetDamageValue(bool isSkill)
    {
        if (isSkill)
            return (false, skillDamage);
        else
            return (false, atkDamage);

    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectTargetRange);

        if(directType == ShootProjectileObstacleDirectType.TARGETING && target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }


        minDirEulerY = GetAngleDirection(rangeRotate.x, originEulerAngleY);
        maxDirEulerY = GetAngleDirection(rangeRotate.y, originEulerAngleY);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + minDirEulerY * detectTargetRange);
        Gizmos.DrawLine(transform.position, transform.position + maxDirEulerY * detectTargetRange);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * detectTargetRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + originDir * detectTargetRange);

    }


    private Vector3 GetAngleDirection(float angle, float eulerAngle)
    {
        angle += eulerAngle;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}

