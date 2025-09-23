using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMoveType { STAND = 0 , WALK =1 , SPRINT =2 , BREAKLIMIT=3 }

public class MoveState : GenealState
{
    [SerializeField] private bool drawGizmos = true;

    [Header("이동 타입")]
    [SerializeField] private PlayerMoveType moveType = PlayerMoveType.STAND;

    [Header("Reference")]
    private Transform myTransform = null;
    private Transform camTransform = null;
    private Animator anim = null;

    [Header("Move Variables")]
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float sprintSpeed = 2.5f;
    [SerializeField] private float limitBreakSpeed = 3.3f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float needReachSpeedTime = 3f;
    [SerializeField] private float currentReachedTimer = 0f;
    private bool excuteGravity = false;
    private bool canBreakLimit = false;
    private Vector3 moveDirection;


    [Header("Slope")]
    [SerializeField] private LayerMask limitSlopeLayer;
    [SerializeField] private LayerMask slopeLayer;
    [SerializeField] private float limitSlopeAngle = 45f;
    [SerializeField] private float slopeDownDistance = 1.2f;
    [SerializeField] private float limitSlopeDistance = 0.5f;
    private RaycastHit downSlopeHit;
    private float slopeAngle = 0f;
    private float angle;


    [Header("Can Amount")]
    [SerializeField] private LayerMask amountLayer;
    [SerializeField] private float limitAmountHeight = 1f;
    [SerializeField] private float amountHeight = 0.1f;
    [SerializeField] private float amountDistance = 1.5f;
    private float amountAngle = 0f;
    private bool isCanAmount = false;
    private bool isAmounting = false;
    private RaycastHit canAmountHit;
    private RaycastHit limitAmountHit;

    [Header("Stair")]
    [SerializeField] private LayerMask stairLayer;
    [SerializeField] private float stairAngle = 90f;
    private bool isStair = false;
    private float currStairAngle = 0f;
    private RaycastHit frontSlopeHit;
    private RaycastHit stairSlopeHit;

    [Header("CantMoveCollision")]
    [SerializeField] private LayerMask cantMoveUpLayer;
    [SerializeField] private float cantMoveUpHeight = 1.5f;
    [SerializeField] private float cantMoveUpDistance = 0.5f;
    [SerializeField] private float cantMoveUpRadius = 0.5f;
    [SerializeField] private float cantMoveback = 0.4f;

    [SerializeField] private LayerMask cantMoveDownLayer;
    [SerializeField] private float cantMoveDownHeight = 0.1f;
    [SerializeField] private float cantMoveDownDistance = 0.5f;
    [SerializeField] private float cantMoveDownRadius = 0.2f;
    private RaycastHit cantMoveHit;

    [Header("Animator Hash Value")]
    private int groundBoolHash = 0;
    private int speedFloatHash = 0;
    private int moveTypeIntHash = 0;
    Vector3 lastDirection;
    private bool isMoveState = false;

    public RaycastHit GetSlopeHit => downSlopeHit;
    public RaycastHit GetStairHit => stairSlopeHit;
    public float CurrentSpeed => currentSpeed;

    protected override void Awake()
    {
        base.Awake();
        anim = controller.myAnimator;
        myTransform = GetComponent<Transform>();
        speedFloatHash = Animator.StringToHash("Speed");
        moveTypeIntHash = Animator.StringToHash("MoveType");
        controller.AddState(this, ref controller.moveStateHash, this.hashCode);
    }

    private void Start()
    {
        camTransform = controller.cam.transform.parent;
    }


    public override void Enter(PlayerStateController stateController, int enumType = -1)
    {
        isMoveState = true;
        walkSpeed = stateController.playerStats.OriginWalkSpeed;
        sprintSpeed = stateController.playerStats.OriginSprintSpeed;
        limitBreakSpeed = stateController.playerStats.OriginLimitBreakSpeed;
    }

    public override void UpdateAction(PlayerStateController stateController)
    {
        Handler();
        if (!stateController.Conditions.CanMove )
        {
            currentSpeed = 0f;
            moveType = PlayerMoveType.STAND;
            controller.myAnimator.SetFloat(speedFloatHash, currentSpeed, 0.1f, Time.deltaTime);
            controller.myAnimator.SetInteger(moveTypeIntHash, (int)moveType);
            return;
        }

        isStair = IsStairSlope();
        isCanAmount = IsCanAmount();
        stateController.Conditions.IsSlope = IsSlope();
        stateController.Conditions.IsLimitedSlope = IsLimitSlope();
        stateController.Conditions.IsCantMove = DetectCantMove();

        SpeedControll();
      
        Rotating(stateController.GetH, stateController.GetV);
        MoveTypeSetting();
        CheckBreakLimit();
        UseGravityCheck();
        //ResetVelocity();
        
        //if (stateController.playerConditions.GetIsFiexRotateSkillingBool())
        //    return;
    }

    public override void AlwaysCheckUpdate(PlayerStateController stateController)
    {
        if (!stateController.Conditions.CanMove)
            return;
        
    }

    private void FixedUpdate()
    {
        ExcuteGravity();

        if (!controller.Conditions.CanMove)
            return;
        if (isMoveState && !controller.Conditions.IsRoll)
            MoveRigidBody();
    }


    public override void Exit(PlayerStateController stateController)
    {
        currentSpeed = 0f;
        isMoveState = false;
        stateController.myAnimator.SetFloat(speedFloatHash, currentSpeed);
        stateController.Conditions.ResetConditions();
    }


     public void CanMove() { isMoveState = true; }

    private void Handler()
    {
        //if (!GameManager.Instance.canUseCamera) return;
        if (QuestManager.Instance.isDialoging || !SettingManager.Instance.CanExcuteScreenTouch ||
            CommonUIManager.Instance.GetActiveUICount() > 0 ||CursorManager.Instance.CurrentCursorMode == CursorMode.VISIBLE) return;

        HandleSprint();

        if (controller.Conditions.CanAttack)
        {
            if (Input.GetMouseButtonDown(0) && controller.comboController.HaveFirstLeftAttack())
                controller.ChangeState(controller.attackStateHash, (int)AttackState.AttackInputType.LEFT);
            else if (Input.GetMouseButtonDown(1) && controller.comboController.HaveFirstRightAttack())
                controller.ChangeState(controller.attackStateHash, (int)AttackState.AttackInputType.RIGHT);
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (controller.IsMove() == false && controller.Conditions.CanCounter)
            {
                controller.ChangeState(controller.counterAttackStateHash);
            }

            if (controller.IsMove() == true && controller.Conditions.CanRoll && !isAmounting)
            {
                if (controller.playerStats.CurrentStamina < controller.playerStats.RollSpCost)
                {
                    CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("스태미나가 부족합니다.");
                }
                else
                    controller.ChangeState(controller.rollStateHash);

            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (controller.Conditions.CanDash)
                controller.ChangeState(controller.dashStateHash);
        }



    }

    private void Rotating(float horizontal, float vertical) 
    {
        if (controller.Conditions.IsMoveStop)
            return;

        Vector3 forward = camTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f; 
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
        Vector3 targetDirection = Vector3.zero;
        targetDirection = forward * vertical + right * horizontal;   
       
        if (controller.IsMove())
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion newRotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, Time.deltaTime * 10 );
            controller.transform.rotation = newRotation;
        }
    }


    private void MoveRigidBody()
    {
        if (controller.Conditions.IsMoveStop)
            return;

        moveDirection = controller.cam.transform.parent.forward * Input.GetAxis("Vertical") +
                               controller.cam.transform.parent.right * Input.GetAxis("Horizontal");
        moveDirection.y = 0f; // y 방향은 고정
        moveDirection.Normalize();


        isAmounting = false;

        if (controller.Conditions.IsCantMove)
        {
            controller.Conditions.CanApplyAnimator = controller.Conditions.IsCantMove ? false : true;
            moveDirection = AdjustCollisionProjectPlane(cantMoveHit.normal);
            controller.rb.MovePosition(controller.rb.position + moveDirection * currentSpeed  * Time.fixedDeltaTime);
           // Debug.Log("IsCantMove");
        }
        else if (controller.Conditions.IsLimitedSlope)
        {
            controller.Conditions.CanApplyAnimator = controller.Conditions.IsLimitedSlope ? false : true;
            moveDirection = AdjustCollisionProjectPlane(frontSlopeHit.normal);
            controller.rb.MovePosition(controller.rb.position + moveDirection * currentSpeed  * Time.fixedDeltaTime);
          // Debug.Log("IsLimitedSlope ");
        }
        else if (currStairAngle > stairAngle)
        {
            controller.Conditions.CanApplyAnimator = controller.Conditions.IsLimitedSlope ? false : true;
            moveDirection = AdjustCollisionProjectPlane(stairSlopeHit.normal);
            controller.rb.MovePosition(controller.rb.position + moveDirection * currentSpeed * 0.45f * Time.fixedDeltaTime);
           // Debug.Log("Stair Limit! ");
        }
        else if (isStair && !controller.Conditions.IsCantMove)
        {
            moveDirection = AdjustSlopeDirection(stairSlopeHit.normal);
            controller.Conditions.CanApplyAnimator = false;
            float isSlopeSpeed = 1.8f;
            controller.rb.MovePosition(controller.rb.position + moveDirection * currentSpeed * isSlopeSpeed * Time.fixedDeltaTime);
           // Debug.Log("Stair");
        }
        else if (isCanAmount && amountAngle >= limitSlopeAngle)
        {
            isAmounting = true;
            moveDirection = AdjustAmountDirection();
            controller.rb.MovePosition(controller.rb.position + moveDirection * currentSpeed * 1.5f * Time.fixedDeltaTime);
         //  Debug.Log("Amount");
        }
        else if (controller.Conditions.IsSlope)
        {
            moveDirection =  AdjustSlopeDirection( downSlopeHit.normal);
            controller.Conditions.CanApplyAnimator = controller.Conditions.IsSlope ? false : true;
            float isSlopeSpeed = 1.8f;
            controller.rb.MovePosition(controller.rb.position + moveDirection * currentSpeed * isSlopeSpeed * Time.fixedDeltaTime);
          //  Debug.Log("IsSlope");
        }
        else
        {
            controller.Conditions.CanApplyAnimator = true;
            float isSlopeSpeed = 1f;
            controller.rb.MovePosition(controller.rb.position + moveDirection * currentSpeed * isSlopeSpeed * Time.fixedDeltaTime);
         //  Debug.Log("IsGround");
        }

        controller.myAnimator.SetFloat(speedFloatHash, currentSpeed, 0.1f, Time.deltaTime);
    }



    private void ExcuteGravity()
    {
        excuteGravity = true;

        if (controller.Conditions.IsGround || isStair) 
            excuteGravity = false;
        if (!controller.Conditions.IsGround && isCanAmount && controller.IsMove()) 
            excuteGravity = false;

        if (excuteGravity)
            controller.rb.AddForce(Vector3.down * 30f, ForceMode.Acceleration);
    }


    private void SpeedControll()
    {
        if (controller.Conditions.IsRoll)
        {
            if (controller.Conditions.IsLimitedSlope || controller.Conditions.IsCantMove)
                controller.rb.velocity = Vector3.zero;
            else
                controller.rb.velocity = controller.rb.velocity.normalized ;
        }
        else  if (controller.Conditions.IsSlope)
        {
            if (controller.rb.velocity.magnitude > currentSpeed)
                controller.rb.velocity = controller.rb.velocity.normalized * currentSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(controller.rb.velocity.x, 0f, controller.rb.velocity.z);

            if (flatVel.magnitude > currentSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * currentSpeed;
                controller.rb.velocity = new Vector3(limitedVel.x, controller.rb.velocity.y, limitedVel.z);
            }
        }
    }


    private void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            controller.Conditions.IsSprint = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            controller.Conditions.IsSprint = false;

    }

    private void MoveTypeSetting()
    {
        if (!controller.IsMove()) moveType = PlayerMoveType.STAND;
        else if (canBreakLimit) moveType = PlayerMoveType.BREAKLIMIT;
        else if (controller.Conditions.IsSprint) moveType = PlayerMoveType.SPRINT;
        else if (!controller.Conditions.IsSprint) moveType = PlayerMoveType.WALK;

        controller.myAnimator.SetInteger(moveTypeIntHash, (int)moveType);
        SpeedSetting();
    }

    private void CheckBreakLimit()
    {
        if (CheckResetBreakLimit())
        {
            canBreakLimit = false;
            currentReachedTimer = 0f;
        }

        if (!canBreakLimit && moveType == PlayerMoveType.SPRINT)
        {
            currentReachedTimer += Time.deltaTime;
            if (currentReachedTimer >= needReachSpeedTime)
            {
                canBreakLimit = true;
                currentReachedTimer = 0f;
            }
        }

    }

    private bool CheckResetBreakLimit()
    {
        if (moveType == PlayerMoveType.STAND || moveType == PlayerMoveType.WALK || !controller.Conditions.IsSprint || controller.Conditions.IsRoll
            || controller.Conditions.IsDamaged || controller.Conditions.IsDown || controller.Conditions.IsSkilling)
            return true;

        return false;
    }

    private void SpeedSetting()
    {
        switch (moveType)
        {
            case PlayerMoveType.STAND: currentSpeed = 0; break;
            case PlayerMoveType.WALK: currentSpeed = (walkSpeed + controller.playerStats.EditWalkSpeed + controller.playerStats.EditAllMoveSpeed )* controller.playerStats.ExtraMoveSpeed ; break;
            case PlayerMoveType.SPRINT: currentSpeed = (sprintSpeed + controller.playerStats.EditSprintSpeed + controller.playerStats.EditAllMoveSpeed ) * controller.playerStats.ExtraMoveSpeed; break;
            case PlayerMoveType.BREAKLIMIT: currentSpeed = (limitBreakSpeed + controller.playerStats.EditLimitBreakSpeed + controller.playerStats.EditAllMoveSpeed )*controller.playerStats.ExtraMoveSpeed; break;
        }

        controller.myAnimator.SetFloat(speedFloatHash, currentSpeed, 0.1f, Time.deltaTime);
        controller.myAnimator.SetInteger(moveTypeIntHash, (int)moveType);
    }

    private void UseGravityCheck()
    {
        if (controller.Conditions.IsSlope && !controller.Conditions.IsLimitedSlope)
            controller.rb.useGravity = false;
        else if(isStair || isCanAmount)
            controller.rb.useGravity = false;
        else
            controller.rb.useGravity = true;
    }

    private void ResetVelocity()
    {
        if (!controller.IsMove() && controller.Conditions.IsGround && controller.rb.velocity.magnitude > 0)
            controller.rb.velocity = Vector3.zero;
    }


    #region Slope
    //1. 우선 경사 올라갈수 잇는지 검사. ( 경사 높이와 각도)
    //2. 경사 올라갈때 미끄러지는 (중력) 사용할지 . 사용안하면 경사올라갈때 미끄럼 x
    //3. 경사 내려갈때 통통 튀는것.
    //4. 올라갈때 내려갈때 속도 일정.

    public bool IsSlope()
    {
        if (Physics.Linecast(controller.transform.position + Vector3.up * 1f, controller.transform.position + Vector3.down * slopeDownDistance, out downSlopeHit, slopeLayer))
        {
            //무릎 위쪽으로 레이를 한번더 쏴서 각도 검사.
            //즉, 1차는 잔잔한 장애물들 2차는 실제 ground
            slopeAngle = Vector3.Angle(Vector3.up, downSlopeHit.normal);
          // Debug.Log("Slope angel : " + slopeAngle);
            if (slopeAngle <= limitSlopeAngle && slopeAngle > 0f)
                return true;
        }
        return false;
    }

    private bool IsCanAmount()
    {
        //1차 충돌체 감지시.
        if(Physics.Linecast(controller.transform.position + Vector3.up * amountHeight, controller.transform.position + Vector3.up * amountHeight + controller.transform.forward * amountDistance, out canAmountHit, amountLayer ))
        {
            //2차 허용가능 높이인지 검사.
            if (!IsLimitAmount())
            {
                amountAngle = Vector3.Angle(Vector3.up, canAmountHit.normal);
                //Debug.Log("Amount angel : " + amountAngle);
                return true;
            }
            else
                amountAngle = 0f;
        }
        return false;
    }

    private bool IsLimitAmount()
    {
        // 충돌체 감지시.
        if (Physics.Linecast(controller.transform.position + Vector3.up * limitAmountHeight, controller.transform.position + Vector3.up * limitAmountHeight + controller.transform.forward * amountDistance, out limitAmountHit, amountLayer))
            return true;
        return false;
    }


    public bool IsStairSlope()
    {
        if (Physics.Linecast(controller.transform.position + Vector3.up * 1f, controller.transform.position + Vector3.down * slopeDownDistance, out stairSlopeHit, stairLayer))
        {
            if (Physics.Linecast(controller.transform.position + Vector3.up * 0.5f, controller.transform.position + Vector3.down * 0.04f, controller.groundLayer))
            {
                //Debug.Log("계단 찾는데 땅이 검출됨.");
                return false;
            }

            currStairAngle = Vector3.Angle(Vector3.up, stairSlopeHit.normal);
            //Debug.Log("IsStairSlope : " + angle);

            if (currStairAngle <= stairAngle && currStairAngle > 0f)
            {
               // Debug.Log("계단 찾음");
                return true;
            }

            currStairAngle = 0f;
        }
        return false;
    }


    private bool IsLimitSlope()
    {
        if (Physics.Linecast(controller.transform.position + Vector3.up * 0.05f, controller.transform.position + Vector3.up * 0.05f + controller.transform.forward * limitSlopeDistance, out frontSlopeHit, limitSlopeLayer))
            return true;
 
        return false;
    }



    private bool DetectCantMove()
    {
        if (Physics.SphereCast(controller.transform.position + -transform.forward * cantMoveback + Vector3.up * cantMoveUpHeight, cantMoveUpRadius, transform.forward, out cantMoveHit, cantMoveUpDistance, cantMoveUpLayer))
            return true;
        
        if (Physics.SphereCast(controller.transform.position + Vector3.up * cantMoveDownHeight, cantMoveDownRadius, transform.forward, out cantMoveHit, cantMoveDownDistance, cantMoveDownLayer))
            return true;

        return false;
    }


    public Vector3 AdjustSlopeDirection( Vector3 hitNoraml)
    {
        return Vector3.ProjectOnPlane(transform.forward, hitNoraml).normalized;
    }

    public Vector3 AdjustCollisionProjectPlane(Vector3 hitNormal)
    {
        hitNormal.y = 0f;
        return Vector3.ProjectOnPlane(transform.forward, hitNormal).normalized;
    }

    private Vector3 AdjustAmountDirection()
    {
        Vector3 surfaceUp = Vector3.Cross(Vector3.Cross(canAmountHit.normal, Vector3.up), canAmountHit.normal).normalized;
        return surfaceUp;
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !drawGizmos) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(controller.transform.position + Vector3.up * 1f, controller.transform.position + Vector3.down * slopeDownDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(controller.transform.position + Vector3.up * cantMoveUpHeight, controller.transform.position + Vector3.up * cantMoveUpHeight + controller.transform.forward * cantMoveUpDistance);


        DrawCantMoveGizmos();
        DrawCanAmountGizmos();

        //Slope Direction.
         Gizmos.color = Color.blue;
         if (controller.Conditions.IsSlope)
         {
             Gizmos.DrawLine(controller.transform.position, downSlopeHit.point);
             Gizmos.DrawLine(downSlopeHit.point, downSlopeHit.point + AdjustSlopeDirection(downSlopeHit.normal) * 3f);
         }
        
         //Limit 
         if (controller.Conditions.IsLimitedSlope)
         {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(controller.transform.position + Vector3.up * 0.05f, controller.transform.position + Vector3.up * 0.05f + controller.transform.forward * limitSlopeDistance);

            Gizmos.color = Color.green;
             Gizmos.DrawLine(controller.transform.position + Vector3.up * 0.1f, frontSlopeHit.point);
             Gizmos.DrawLine(frontSlopeHit.point, frontSlopeHit.point + AdjustCollisionProjectPlane(frontSlopeHit.normal) * 2f);
         }
         else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(controller.transform.position + Vector3.up * 0.05f, controller.transform.position + Vector3.up * 0.05f + controller.transform.forward * limitSlopeDistance);
        }

        //CantMove
        if (controller.Conditions.IsCantMove)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(controller.transform.position + Vector3.up * cantMoveUpHeight, cantMoveHit.point);
            Gizmos.DrawLine(cantMoveHit.point, cantMoveHit.point + AdjustCollisionProjectPlane(cantMoveHit.normal) * 1.5f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(controller.transform.position + Vector3.up * cantMoveUpHeight, controller.transform.position + Vector3.up * cantMoveUpHeight + controller.transform.forward * cantMoveUpDistance);
        }


    }


    private void DrawCantMoveGizmos()
    {

        //Down
        if (Physics.SphereCast(controller.transform.position + Vector3.up * cantMoveDownHeight, cantMoveDownRadius, transform.forward, out cantMoveHit, cantMoveDownDistance, cantMoveDownLayer))
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(controller.transform.position + Vector3.up * cantMoveDownHeight, cantMoveHit.point);
            Gizmos.DrawWireSphere(cantMoveHit.point, cantMoveDownRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(controller.transform.position + Vector3.up * cantMoveDownHeight, controller.transform.position + Vector3.up * cantMoveDownHeight + controller.transform.forward * cantMoveDownDistance);
            Gizmos.DrawWireSphere(controller.transform.position + Vector3.up * cantMoveDownHeight + controller.transform.forward * cantMoveDownDistance, cantMoveDownRadius);
        }


        // Up
        if (Physics.SphereCast(controller.transform.position + -transform.forward * cantMoveback + Vector3.up * cantMoveUpHeight, cantMoveUpRadius, transform.forward, out cantMoveHit, cantMoveUpDistance, cantMoveUpLayer))
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(controller.transform.position + -transform.forward * cantMoveback + Vector3.up * cantMoveUpHeight, cantMoveHit.point);
            Gizmos.DrawWireSphere(cantMoveHit.point, cantMoveUpRadius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(controller.transform.position + -transform.forward * cantMoveback + Vector3.up * cantMoveUpHeight, controller.transform.position + -transform.forward * cantMoveback + Vector3.up * cantMoveUpHeight + controller.transform.forward * cantMoveUpDistance);
            Gizmos.DrawWireSphere(controller.transform.position + -transform.forward * cantMoveback + Vector3.up * cantMoveUpHeight + controller.transform.forward * cantMoveUpDistance, cantMoveUpRadius);
        }

    }

    private void DrawCanAmountGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(controller.transform.position + Vector3.up * limitAmountHeight, controller.transform.position + Vector3.up * limitAmountHeight + controller.transform.forward * amountDistance);


        if (isCanAmount)
        {
            Vector3 normal = canAmountHit.normal;
            normal.y = 0f;
            Vector3 dir = Vector3.ProjectOnPlane(controller.transform.forward, normal);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(controller.transform.position + Vector3.up * amountHeight, controller.transform.position + Vector3.up * amountHeight+ canAmountHit.point);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(canAmountHit.point, canAmountHit.point + canAmountHit.normal * 3f);
            Gizmos.DrawSphere(controller.transform.position + canAmountHit.point, 0.1f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(canAmountHit.point, canAmountHit.point + dir.normalized * amountDistance);

            // 표면의 위쪽 벡터 구하기
            Vector3 surfaceUp = Vector3.Cross(Vector3.Cross(canAmountHit.normal, Vector3.up), canAmountHit.normal).normalized;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(canAmountHit.point, canAmountHit.point + surfaceUp * 3f);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine( controller.transform.position + Vector3.up * amountHeight, controller.transform.position + Vector3.up * amountHeight + controller.transform.forward * amountDistance);
        }
    }


}

