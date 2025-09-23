using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : GenealState
{
    [SerializeField] private bool drawGizmos = true;

    [Header("Roll Variables")]
    [SerializeField] private float costSp = 15f;
    [SerializeField] private float excuteSpCost = 5f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float waitReEnterTime = 0.3f;
    [SerializeField] private int rollEndFrame = 0;
    [SerializeField] private float clipSpeed = 1f;
    [SerializeField] private AnimationClip rollAnimationClip = null;
    [SerializeField] private float currentTimer = 0f;      //hide
    [SerializeField] private float endTime = 0f;
    [SerializeField] private bool excuteCoolTime = false;
    [SerializeField] private float currentCoolTimer = 0f;
    private MoveState moveState;

    [Header("Detect Front")]
    [SerializeField] private LayerMask cantRollLayer;
    [SerializeField] private float detectMinHeight = 0.1f;
    [SerializeField] private float detectMiddleHeight = 0.6f;
    [SerializeField] private float detectMaxHeight = 1.2f;
    [SerializeField] private float detectDistance = 0.5f;
    private RaycastHit frontHit;
    public bool isCantMove = false;      //public 임시 -> private으로 하기.
    public bool isSlope = false;         //public 임시 -> private으로 하기.
    public bool isStair = false;
    private Vector3 moveDirect;
  

    protected override void Awake()
    {
        base.Awake();
        controller.AddState(this,ref controller.rollStateHash, hashCode);
    }

    private void Start()
    {
        moveState = controller.GetState<MoveState>();
    }

    public override void Enter(PlayerStateController stateController, int enumType)
    {
        if (moveState == null)
            moveState = stateController.GetState<MoveState>();

        if (!stateController.Conditions.InfinityStamina)
            stateController.playerStats.UseCurrentStamina(stateController.playerStats.RollSpCost);

        StopAllCoroutines();
        SetEndTime();
        stateController.RotateToDirection();
        ExcuteConditions();
        stateController.Conditions.IsRoll= true;
        stateController.Conditions.CanAttack = false;
        stateController.myAnimator.SetFloat(AnimatorKey.RollSpeed, clipSpeed);
        stateController.myAnimator.CrossFade("Roll", 0.1f);
        currentTimer = 0f;
        stateController.rb.useGravity = true;
        stateController.Conditions.CanRoll = false;
    }

    public override void UpdateAction(PlayerStateController stateController)
    {
        ExcuteConditions();

        if (stateController.Conditions.IsRoll)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= endTime)
                stateController.ChangeState(stateController.moveStateHash, 0);
        }
       
    }

    public override void AlwaysCheckUpdate(PlayerStateController stateController)
    {
        if(excuteCoolTime)
            ReEnterCoolTime();
    }

    private void FixedUpdate()
    {
        if (controller.Conditions.IsRoll)
            RollingDirection();
    }



    public override void Exit(PlayerStateController stateController)
    {
        stateController.rb.useGravity = false;
        stateController.rb.velocity = Vector3.zero;
        stateController.Conditions.CanAttack = true;
        stateController.Conditions.IsRoll = false;
        stateController.Conditions.IsDamaged = false;
        controller.Conditions.CanApplyAnimator = true;
        currentCoolTimer = 0f;
        excuteCoolTime = true;
        isCantMove = false;
        isSlope = false;
        isStair = false;
    }


    private bool IsDetectCantMove()
    {
        if (Physics.Linecast(transform.position + Vector3.up * detectMinHeight, transform.position + Vector3.up * detectMinHeight + transform.forward * detectDistance, out frontHit, cantRollLayer))
            return true;
        else if (Physics.Linecast(transform.position + Vector3.up * detectMiddleHeight, transform.position + Vector3.up * detectMiddleHeight + transform.forward * detectDistance, out frontHit, cantRollLayer))
            return true;
        else if (Physics.Linecast(transform.position + Vector3.up * detectMaxHeight, transform.position + Vector3.up * detectMaxHeight + transform.forward * detectDistance, out frontHit, cantRollLayer))
            return true;
        return false;
    }


    private void RollingDirection()
    {
        if (isCantMove)
        {
            controller.Conditions.CanApplyAnimator = false;
            Debug.Log("Cant Move");
            controller.rb.velocity = Vector3.zero;
        }
        else if(isStair)
        {
            controller.Conditions.CanApplyAnimator = true;
            moveDirect = moveState.AdjustSlopeDirection(moveState.GetStairHit.normal);
            controller.rb.MovePosition(controller.rb.position + moveDirect * speed * Time.fixedDeltaTime);
            Debug.Log("Is Stair");
        }
        else if (isSlope)
        {
            controller.Conditions.CanApplyAnimator = true;
            moveDirect = moveState.AdjustSlopeDirection(moveState.GetSlopeHit.normal);
            controller.rb.MovePosition(controller.rb.position + moveDirect * speed * Time.fixedDeltaTime);
            Debug.Log("Is Slope");
        }
        else if(controller.Conditions.IsGround)
        {
            controller.Conditions.CanApplyAnimator = true;
            moveDirect = transform.forward;
            controller.rb.MovePosition(controller.rb.position + moveDirect * speed * Time.fixedDeltaTime);
          // Debug.Log("ground");
        }



    }


    private void ExcuteConditions()
    {
        isCantMove = IsDetectCantMove();
        isSlope = moveState.IsSlope();
        isStair = moveState.IsStairSlope();
    }


    private void SetEndTime()
    {
        endTime = rollEndFrame * (1 / (30f * clipSpeed));
    }

    private void ReEnterCoolTime()
    {
        controller.Conditions.CanRoll = false;
        currentCoolTimer += Time.deltaTime;
        if (currentCoolTimer >= waitReEnterTime)
        {
            controller.Conditions.CanRoll = true;
            currentCoolTimer = 0f;
            excuteCoolTime = false;
        }
    }



    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        
        if(isCantMove)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(controller.transform.position + Vector3.up * detectMinHeight, controller.transform.position + Vector3.up * detectMinHeight + controller.transform.forward * detectDistance);
            Gizmos.DrawLine(controller.transform.position + Vector3.up * detectMiddleHeight, controller.transform.position + Vector3.up * detectMiddleHeight + controller.transform.forward * detectDistance);
            Gizmos.DrawLine(controller.transform.position + Vector3.up * detectMaxHeight, controller.transform.position + Vector3.up * detectMaxHeight + controller.transform.forward * detectDistance);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(controller.transform.position + Vector3.up * detectMinHeight, controller.transform.position + Vector3.up * detectMinHeight + controller.transform.forward * detectDistance);
            Gizmos.DrawLine(controller.transform.position + Vector3.up * detectMiddleHeight, controller.transform.position + Vector3.up * detectMiddleHeight + controller.transform.forward * detectDistance);
            Gizmos.DrawLine(controller.transform.position + Vector3.up * detectMaxHeight, controller.transform.position + Vector3.up * detectMaxHeight + controller.transform.forward * detectDistance);

        }
    }

}
