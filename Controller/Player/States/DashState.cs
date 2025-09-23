using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//1. 여기서는 대시 작동을 구현.
//2. 실제 디텍트 부분은 어디서?  
//   -> 여기서 구현하고. DashUI? 부분에서 호출하기.?

public class DashState : GenealState
{
    [SerializeField] private ThirdPersonCamera cam = null;
    [SerializeField] private DashSkillClip dashClip = null;
    [SerializeField] private Transform dashTargetTr = null;
    [SerializeField] private Vector2 limitDistance = new Vector2(100, 100);
    [SerializeField] private Vector2 centerOffset = new Vector2(0f, 3f);                                                                         

    public BaseController targetController = null;  // hide

    [Header("대시 초기화")]
    [SerializeField] private int initCount = 5;
    private bool isActive = false;
    private float currentCoolTimer = 0f;
    private float coolTimePerCount = 0f; //각 카운트 쿨타임.

    [Header("카메라 Stop Distance Far")]
    [SerializeField] private float farCamSmoothSpeed = 10f;
    [SerializeField] private float farChangeFOV = 100;
    [SerializeField] private float farReturnFOVWaitTime = 0.1f;
    [SerializeField] private float farDelayFOVMoveTime = 0.1f;

    [Header("카메라 Stop Distance Near")]
    [SerializeField] private float nearCamSmoothSpeed = 7f;
    [SerializeField] private float nearChangeFOV = 85;
    [SerializeField] private float nearReturnFOVWaitTime = 0.2f;
    [SerializeField] private float nearDelayFOVMoveTime = 0.1f;


    [Header("타겟팅 탐지")]
    [SerializeField] private float targetingAllowAngle = 90f;
    [SerializeField] private float targetingAllowDistance = 3f;
    [SerializeField] private Vector2 targetingLimitScreenPoint = new Vector2(300, 300);
    private Vector3 checkAngleDir = Vector3.zero;
    private Transform gizmosTarget = null;

    [Header("장애물 검사")]
    [SerializeField] private float detectSphereRadius = 3f;
    private RaycastHit[] detectObstacleColls = new RaycastHit[10];
    private Vector3 gizmoObstacleDir;
    private float gizmoObstacleDistance;

    [Header("지면 검사")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float minDetectHeight = -10f;
    [SerializeField] private float maxDetectHeight = 10f;
    [SerializeField] private float groundStartYOffset = 2f;
    [SerializeField] private float groundYRange = 4f;   // range만큼 위쪽 타겟 가능값.
    [SerializeField] private float minDetectTargetDistance = 1f;
    [SerializeField] private float groundDetectRadius = 1f;
    [SerializeField] private float groundDetectInterval = 0.5f;
    private float currentTargetHeight = 0f;
    private int groundDetectCount = 0;
    private int groundEnemyCount = 0;
    private Collider[] groundEnemyColls = new Collider[2];
    private Vector3 targetDirFromDashPos = Vector3.zero;
    private float groundDistance = 0f;
    private float sumInterval = 0f;
    private Vector3 startPos = Vector3.zero;
    private RaycastHit groundCheckRayHit;
    private int groundSumCount = 0;
    private List<Vector3> drawEnemyHitPoints = new List<Vector3>();


    [Header("이동")]
    [SerializeField] private float dashStopDistance = 4f; //적 텔포 거리.
    private Vector3 tmpDashPosition = Vector3.zero;   //임시 이동할 위치
    private Vector3 canDashPosition = Vector3.zero;  //이동 가능한 위치.

   // [Header("공격")]
    private int damageDetectCount = 0;
    private Collider[] damageColls = new Collider[10];


    private Collider[] retDetectTargets = new Collider[20];
    private int detectCount = 0;
    private Vector3 centerPosition = Vector3.zero;
    private Vector2 centerScreenPoint = Vector2.zero;
    private Transform tmpDashTargetTr = null;
    private bool doneDashState = true;
    private int obstacleCount = 0;

    public DashTargetMaskUI dashTargetCheckUI;
    public DashSuccessCountUI dashSuccessCountUI;

    private Vector3 targetDir;


    #region Events
    public delegate void OnInitDash();
    private event OnInitDash onInitDash;
    public event OnInitDash OnInitDash_
    {
        add
        {
            if (onInitDash == null || !onInitDash.GetInvocationList().Contains(value))
                onInitDash += value;
        }
        remove
        {
            onInitDash -= value;
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        controller.AddState(this, ref controller.dashStateHash, hashCode);
    }

    private void Start()
    {
        //stateController.playerConditions.CurrentNeedDashCount = initCount;
        //isActive = true;
        isActive = false;
        if (cam == null) cam = GameManager.Instance.Cam;
        coolTimePerCount = dashClip.skillCoolTime / initCount;

        if (controller.skillController.GetOwnSkillTypes<DashSkillClip>().Length > 0)
            dashClip = controller.skillController.GetOwnSkillTypes<DashSkillClip>()[0];
    }

    private void OnEnable()
    {
        if (dashTargetCheckUI == null)
            dashTargetCheckUI = FindObjectOfType<DashTargetMaskUI>();
        if (dashSuccessCountUI == null)
            dashSuccessCountUI = FindObjectOfType<DashSuccessCountUI>();
    }


    public override void Enter(PlayerStateController stateController, int enumType)
    {
        if (dashClip == null)
        {
            coolTimePerCount = dashClip.skillCoolTime / initCount;
            if (controller.skillController.GetOwnSkillTypes<DashSkillClip>().Length > 0)
                dashClip = controller.skillController.GetOwnSkillTypes<DashSkillClip>()[0];
        }

        StopAllCoroutines(); 
        onInitDash?.Invoke();
        doneDashState = false;
        isActive = false;
        stateController.myAnimator.SetFloat(AnimatorKey.DashSpeed, dashClip.AnimationSpeed);

        currentCoolTimer = 0f;
        stateController.Conditions.SuccessCountClear();
        dashTargetCheckUI.ClearCompareTarget();
        dashSuccessCountUI.UsedUI();
        StartCoroutine(DashMoveProcess_Co());


    }

    public override void UpdateAction(PlayerStateController stateController)
    {
        if (doneDashState)
            stateController.ChangeState(stateController.moveStateHash);
    }

    public override void AlwaysCheckUpdate(PlayerStateController stateController)
    {
        if (dashSuccessCountUI == null || dashTargetCheckUI == null) return;
        
        drawEnemyHitPoints.Clear();
        CheckDashInitCoolTime();
        if (dashTargetTr == null)
            targetController = null;

        if (!CheckCanDashCondition())
        {
            ResetDash();
            return;
        }

        centerPosition = stateController.transform.position + (Vector3)centerOffset;
        centerScreenPoint = cam.MainCam.WorldToScreenPoint(centerPosition);

        DetectEnemyColliders();
        ExcuteSettingTarget(); // 여기서 dashTargetTr 세팅.

        if (dashTargetTr != null)
        {
            SettingTmpDashPosition(dashTargetTr);  //realPosition 세팅. 
            if (!CheckCanDashGround())
                dashTargetTr = null;
            else if (dashTargetTr.GetComponentInParent<AIConditions>() && !dashTargetTr.GetComponentInParent<AIConditions>().CanDetect)
                dashTargetTr = null;
            else if (dashTargetTr.gameObject.activeInHierarchy && dashTargetTr.GetComponentInParent<AIController>().IsDead())
                dashTargetTr = null;
            else if (!dashTargetTr.gameObject.activeInHierarchy)
                dashTargetTr = null;
        }

        SetActiveTargetMask();
    }


    public override void Exit(PlayerStateController stateController)
    {
        StopAllCoroutines();
        doneDashState = true;
        if (cam.GetCurrentFOV != cam.GetOriginFOV)
            cam.ResetCameraFOV();
    }



    #region Detect Target
    private void DetectEnemyColliders()
    {
        detectCount = Physics.OverlapSphereNonAlloc(controller.transform.position, dashClip.DashDetectRadius, retDetectTargets, controller.targetLayer);

    }



    private void SetActiveTargetMask()
    {
        if (dashTargetTr != null)
        {
            dashTargetCheckUI.TargetRect.gameObject.SetActive(true);
            dashTargetCheckUI.TargetRect.transform.position = cam.MainCam.WorldToScreenPoint(targetController != null 
                                                                                             ? targetController.damagedPosition.position 
                                                                                             : dashTargetTr.position);
            controller.Conditions.CanDash =true;
        }
        else
        {
            dashTargetCheckUI.TargetRect.gameObject.SetActive(false);
            controller.Conditions.CanDash = false;
        }
    }


    private void ExcuteSettingTarget()
    {
        Transform dashPosition = null;
        tmpDashTargetTr = null;
        SortEnemy(retDetectTargets);
        if (CheckIsLockTargetProcess())
            return;

        for (int i = 0; i < retDetectTargets.Length; i++)
        {
            if (retDetectTargets[i] == null || tmpDashTargetTr != null)
                continue;
            if (!CheckInAngle(retDetectTargets[i].transform))
                continue;
            if (retDetectTargets[i].GetComponent<AIConditions>() && !retDetectTargets[i].GetComponent<AIConditions>().CanDetect)
                continue;

            dashPosition = retDetectTargets[i]?.GetComponent<AIController>()?.DashTargetMaskTr;
            if (dashPosition == null)
                dashPosition = retDetectTargets[i].transform;

            if (CheckCanDashTarget(dashPosition))
            {
                tmpDashTargetTr = dashPosition;
                dashTargetCheckUI.ResetMask(tmpDashTargetTr);
            }
            
        }

        if (tmpDashTargetTr != dashTargetTr)
        {
            dashTargetCheckUI.ClearCompareTarget();
            dashTargetTr = tmpDashTargetTr;
            gizmosTarget = tmpDashTargetTr;
            targetController = dashTargetTr?.GetComponent<BaseController>();
        }
    }

    private bool CheckIsLockTargetProcess()
    {
        if (dashTargetTr != null)
        {
          
            float distance = (dashTargetTr.position - controller.transform.position).magnitude;
            if (distance <= targetingAllowDistance && CheckCanDashTarget(dashTargetTr))
            {
                if (dashTargetCheckUI.TargetRect != null)
                {
                    Vector2 curentPoint = ((Vector2)dashTargetCheckUI.TargetRect.position - centerScreenPoint);
                    curentPoint.x = Mathf.Abs(curentPoint.x);
                    curentPoint.y = Mathf.Abs(curentPoint.y);
                    dashTargetCheckUI.SettingTargetMaskColor(curentPoint / targetingLimitScreenPoint);
                    dashTargetCheckUI.SettingTargetingImage();
                }
                return true;
            }
            else if (CheckCanDashTarget(dashTargetTr))
            {
                dashTargetCheckUI.SettingCheckImage();
            }
        }
        dashTargetCheckUI.ResetTargetTr();

        return false;
    }

    private bool CheckCanDashTarget(Transform targetTr)
    {
        BaseController targetCon = targetTr.GetComponent<BaseController>();
        Vector3 targetPos = targetCon != null ? targetCon.damagedPosition.position : targetTr.position;
        Vector2 point = cam.MainCam.WorldToScreenPoint(targetPos);
        targetDir = (targetPos - centerPosition).normalized;
        float distance = (targetPos - (controller.transform.position + (Vector3)centerOffset)).magnitude;

        gizmoObstacleDir = targetDir;
        gizmoObstacleDistance = distance;

        //땅일경우
        if (CheckDetectGround(targetDir, distance))
        {
            return false;
        }

        /// 이부분에 해당 타겟에 레이어 쏴서 장애물 있나 판단하기.
        if (CheckDetectObstacle(targetDir, distance))
        {
            return false;
        }

        //타겟팅일 경우 
        if (distance <= targetingAllowDistance)
        {
            if (centerScreenPoint.x + targetingLimitScreenPoint.x >= point.x && centerScreenPoint.x - targetingLimitScreenPoint.x <= point.x &&
            centerScreenPoint.y + targetingLimitScreenPoint.y >= point.y && centerScreenPoint.y - targetingLimitScreenPoint.y <= point.y)
                return true;
        }

        if (centerScreenPoint.x + limitDistance.x >= point.x && centerScreenPoint.x - limitDistance.x <= point.x &&
            centerScreenPoint.y + limitDistance.y >= point.y && centerScreenPoint.y - limitDistance.y <= point.y)
        {
            return true;
        }

        return false;
    }

    private bool CheckDetectGround(Vector3 dir, float distance)
    {
        // Raycast를 쏴서, 충돌된 오브젝트에 대한 정보를 얻는다.
        if (Physics.Raycast(centerPosition, dir, distance, groundLayer))
        {
            return true;
        }

        return false;
    }

    private bool CheckDetectObstacle(Vector3 dir, float distance)
    {
        obstacleCount = Physics.SphereCastNonAlloc(centerPosition, detectSphereRadius, dir, detectObstacleColls, distance, controller.obstacleLayer);
       
        if (obstacleCount > 0)
            return true;
        else
            return false;

    }

    private bool CheckInAngle(Transform target)
    {
        checkAngleDir = (target.position - cam.transform.parent.position);
        checkAngleDir.y = 0f;
        checkAngleDir.Normalize();

        if (Vector3.Angle(cam.transform.parent.forward, checkAngleDir) <= targetingAllowAngle)
            return true;
        return false;
    }


    private void SortEnemy(Collider[] coll)
    {
        Transform iDashTr = null;
        Transform jDashTr = null;

        Vector2 enemyIPoint = Vector3.zero;
        Vector2 enemyJPoint = Vector3.zero;
        float iDistance = 0f;
        float jDistance = 0f;
      

        for (int i = 0; i < coll.Length; i++)
        {
            if (coll[i] == null) continue;
            iDashTr = coll[i]?.GetComponent<AIController>()?.DashTargetMaskTr;
            if (iDashTr == null)
                iDashTr = coll[i].transform;
            enemyIPoint = cam.MainCam.WorldToScreenPoint(iDashTr.position);
            iDistance = Vector3.Distance(centerScreenPoint, enemyIPoint);

       
            for (int j = 0; j < coll.Length; j++)
            {
                if (coll[i] == coll[j] || coll[j] == null) continue;

                jDashTr = coll[j]?.GetComponent<AIController>()?.DashTargetMaskTr;
                if (jDashTr == null)
                    jDashTr = coll[j].transform;

                enemyJPoint = cam.MainCam.WorldToScreenPoint(jDashTr.position);
                jDistance = Vector3.Distance(centerScreenPoint, enemyJPoint);

                if (iDistance < jDistance)
                {
                    Collider temp = coll[j];
                    coll[j] = coll[i];
                    coll[i] = temp;
                }

            }
        }

    }

    #endregion


    #region Check Ground


    private bool CheckCanDashGround()
    {
        targetDirFromDashPos = (dashTargetTr.position - tmpDashPosition);
        groundDistance = targetDirFromDashPos.magnitude;
        sumInterval = 0f;
        groundSumCount = 0;
        currentTargetHeight = (dashTargetTr.position - controller.transform.position).y;
        targetDirFromDashPos.y = 0f;
        targetDirFromDashPos.Normalize();
        canDashPosition = Vector3.zero;
    
        groundSumCount = (int)((groundDistance - minDetectTargetDistance) / groundDetectInterval);

        if (currentTargetHeight < minDetectHeight || currentTargetHeight > maxDetectHeight)
            return false;
        for (int i = 0; i < groundSumCount; i++)
        {
            startPos = tmpDashPosition + targetDirFromDashPos * sumInterval + Vector3.up * groundStartYOffset;
            sumInterval += groundDetectInterval;

            if (DetectEnemy(startPos))
                continue;
            if (Physics.SphereCast(startPos, groundDetectRadius, -Vector3.up, out groundCheckRayHit, groundYRange, groundLayer))
            {
                canDashPosition = groundCheckRayHit.point;
                return true;
            }
            else
            {
                canDashPosition = Vector3.zero;
            }
        }
        return false;
    }


    private bool DetectEnemy(Vector3 startPosition)
    {
        RaycastHit groundHit;

        //땅 발견 못하면.
        if (!Physics.Raycast(startPosition, -Vector3.up, out groundHit, groundYRange, groundLayer))
        {
         //   Debug.Log("DetectEnemy - 땅 발견 못함");
            return true;
        }



        //땅 위치까지 물체가 있는지 검사. (시작 radius에 검출안됨)
        if (Physics.SphereCast(startPosition, groundDetectRadius, -Vector3.up, out groundCheckRayHit, groundHit.point.y, enemyLayer))
        {
            drawEnemyHitPoints.Add(groundCheckRayHit.point);
          //  Debug.Log("DetectEnemy - 땅 위치까지 물체가 있는지 검사. 발견 (시작 radius에 검출안됨)");
            return true;
        }



        //시작 radius에 물체가 있는지 검사
        if (Physics.OverlapSphereNonAlloc(startPosition, groundDetectRadius, groundEnemyColls, enemyLayer) > 0)
         {
            drawEnemyHitPoints.Add(startPosition);
           // Debug.Log("DetectEnemy - 시작 radius에 물체가 있음");
            return true;
         }


        return false;
    }

    #endregion


    #region Dash Process

    private IEnumerator DashMoveProcess_Co()
    {
        if (dashTargetTr == null) yield break;
        float endTime = dashClip.EndTime;


        //현재 위치와 타겟위치의 거리가 stopDistance보다 작으면 이동 x하고 해당 자리에서 데미지및 애니메이션 실행.
        if (CanTeleportToTarget(dashTargetTr))
        {
            StartCoroutine(DashCameraProcess_Co(false));
            yield return new WaitForSeconds(farDelayFOVMoveTime);
            controller.RotateToTarget(dashTargetTr.position);
            controller.myAnimator.CrossFade(dashClip.AnimationName, 0.1f);
            controller.TranslatePosition(canDashPosition);
            controller.StartCoroutine(DashDamageProcess_Co());
        }
        else
        {
            StartCoroutine(DashCameraProcess_Co(true));
            yield return new WaitForSeconds(nearDelayFOVMoveTime);
            controller.RotateToTarget(dashTargetTr.position);
            controller.myAnimator.CrossFade(dashClip.AnimationName, 0.1f);
            controller.StartCoroutine(DashDamageProcess_Co());
        }


        yield return new WaitForSeconds(dashClip.EndTime);
        doneDashState = true;
        dashTargetTr = null;
    }

    

    private IEnumerator DashCameraProcess_Co(bool isNear)
    {
        if (!isNear)
            cam.SetCamFOV(isNear ? nearChangeFOV : farChangeFOV, isNear ? nearCamSmoothSpeed : farCamSmoothSpeed);

        GameManager.Instance.MainPP.ExcuteAnimate(PPType.CHROMATIC_ABERRATION);
        yield return new WaitForSeconds(isNear? nearReturnFOVWaitTime : farReturnFOVWaitTime);
        cam.ShakeCamera(dashClip.CameraInfo[0]);

        if (!isNear)
            cam.SetCamFOV(60, isNear ? nearCamSmoothSpeed : farCamSmoothSpeed);
    }

    private IEnumerator DashDamageProcess_Co()
    {
        float[] damageTimings = dashClip.GetDamageTime();

        for (int i = 0; i < damageTimings.Length; i++)
        {
            yield return new WaitForSeconds(damageTimings[i]);

            EffectManager.Instance.GetEffectObjectInfo(dashClip.EffectInfo[i], transform.position, Vector3.zero, Vector3.zero);
            DashDamage(dashClip,i);
        }
    }


    private void DashDamage(DashSkillClip clip, int index)
    {
        damageDetectCount = Physics.OverlapSphereNonAlloc(transform.position, clip.DashDamageRange, damageColls, enemyLayer);
        controller.SortFindEmenyByNearDistance(controller.transform,ref damageColls);
        int damageCount = 0;

        //Debug.Log("damageDetectCount : " + damageDetectCount);

        for (int i = 0; i < damageDetectCount; i++)
        {
            if (damageCount >= clip.maxTargetCount)
                break;                       
            if (damageColls.Length <= i || damageColls[i] == null)
                continue;
          
            AIController target = damageColls[i].GetComponent<AIController>();
            if (target == null)
                continue;
            if (controller.IsDetectObstacle(controller.damagedPosition, target.damagedPosition))
                continue;
            if (damageColls[i] != null && target.aiConditions.IsDead)
                continue;
            SoundManager.Instance.PlayEffect(clip.EffectInfo[index].effectSound);

            (bool, float) dmgValue = controller.GetDamageValue(true);
            target.Damaged(clip.DashDamageP[index] * dmgValue.Item2, controller, dmgValue.Item1, true, clip.AttackStrengthType[index], true);
            damageCount++;
        }

    }


    private void SettingTmpDashPosition(Transform targetTr)
    {
        if (targetTr == null)
        {
            tmpDashPosition = -Vector3.one;
            return;
        }
        Vector3 tmpDashPos = targetTr.position - centerPosition;
        tmpDashPos.y = 0f;
        tmpDashPos.Normalize();
        tmpDashPosition = targetTr.position + -tmpDashPos * dashStopDistance;
    }

    private bool CanTeleportToTarget(Transform targetTr)
    {
        if (Vector3.Distance(controller.transform.position, targetTr.position) > dashStopDistance + 1f)
            return true;
        return false;

    }

    #endregion


    #region Check Active Dash Skill

    private bool CheckCanDashCondition()
    {
        if (isActive) return true;

        if (controller.Conditions.CurrentNeedDashCount < initCount)
            return false;

        return true;
    }

    private void ResetDash()
    {
        dashTargetCheckUI.TargetRect.gameObject.SetActive(false);
        controller.Conditions.CanDash = false;
    }

    private void CheckDashInitCoolTime()
    {
        if (isActive) return;

        if (controller.Conditions.CurrentNeedDashCount < initCount)
        {
            currentCoolTimer += Time.deltaTime;
            if(currentCoolTimer >= coolTimePerCount)
            {
                currentCoolTimer = 0f;
                controller.Conditions.AddSuccessDashCount(1);
                if (controller.Conditions.CurrentNeedDashCount >= initCount)
                    isActive = true;
            }
        }
    }
    #endregion
    
    private void DrawGizmoCheckGround()
    {
        if (gizmosTarget != null && tmpDashPosition != -Vector3.one)
        {
            for (int i = 0; i < groundSumCount; i++)
            {
                bool isEnemyFind = false;
                Vector3 drawPos = tmpDashPosition + targetDirFromDashPos * (i * groundDetectInterval) + Vector3.up * groundStartYOffset;

                if (Physics.SphereCast(drawPos, groundDetectRadius, -Vector3.up, out groundCheckRayHit, groundYRange, enemyLayer))
                    isEnemyFind = true;
                if (Physics.OverlapSphere(drawPos, groundDetectRadius, enemyLayer).Length > 0)
                    isEnemyFind = true;
                if (!Physics.Raycast(drawPos, -Vector3.up, groundYRange, groundLayer))
                    isEnemyFind = true;

                if (gizmosTarget != null && dashTargetTr == null) Gizmos.color = Color.black;
                else if (isEnemyFind) Gizmos.color = Color.red;
                else Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(drawPos, groundDetectRadius);

                if (!isEnemyFind && Physics.SphereCast(drawPos, groundDetectRadius, -Vector3.up, out groundCheckRayHit, groundYRange, groundLayer))
                {
                    Gizmos.DrawLine(drawPos, groundCheckRayHit.point);
                    Gizmos.DrawWireSphere(groundCheckRayHit.point, groundDetectRadius);

                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(groundCheckRayHit.point, 0.1f);
                }

            }

        }
    }


    private void OnDrawGizmos()
    {
        if(retDetectTargets.Length > 0)
        {
            for (int i = 0; i < retDetectTargets.Length; i++)
            {
                if (retDetectTargets[i] == null) continue;

                BaseController targetCon = retDetectTargets[i].GetComponent<BaseController>();
                Vector3 targetPosition = targetCon != null ? targetCon.damagedPosition.position : retDetectTargets[i].transform.position;
                Ray ray = new Ray(centerPosition, targetPosition - (controller.transform.position + (Vector3)centerOffset)); // 현재 객체에서 타겟으로 레이를 쏨
                float distance = (targetPosition - centerPosition).magnitude;
                RaycastHit hit;

                // 레이가 장애물에 맞았을 경우
                if (Physics.Raycast(ray, out hit, distance,controller.groundLayer + controller.obstacleLayer))
                {
                    // 레이가 장애물에 맞으면 그 충돌 지점에 빨간색 구를 그리기
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(hit.point, 1f); // 충돌 지점에 구 그리기
                    Gizmos.DrawLine(centerPosition, targetPosition);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(centerPosition, targetPosition); // 타겟으로 향하는 선 그리기
                }
            }
        }

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(centerPosition + (Vector3)centerOffset, 0.4f);
        Gizmos.DrawLine(centerPosition, centerPosition + gizmoObstacleDir * gizmoObstacleDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(centerPosition, detectSphereRadius);
        Gizmos.DrawWireSphere(centerPosition + gizmoObstacleDir * gizmoObstacleDistance, detectSphereRadius);
        Gizmos.DrawLine(centerPosition, centerPosition + gizmoObstacleDir * gizmoObstacleDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(tmpDashPosition, 0.8f);


        for (int i = 0; i < drawEnemyHitPoints.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(drawEnemyHitPoints[i], groundDetectRadius - 0.1f);
        }

        if (canDashPosition != Vector3.zero)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(canDashPosition, 0.3f);
        }

        if (obstacleCount > 0 || gizmosTarget == null) return;

        DrawGizmoCheckGround();
    }

    private void OnValidate()
    {
        if (groundDetectRadius <= 0) groundDetectRadius = 0.01f;
        if (groundDetectInterval <= 0) groundDetectInterval = 0.01f;
        if (groundDetectInterval >= groundDetectRadius) groundDetectInterval = groundDetectRadius;
    }
}
