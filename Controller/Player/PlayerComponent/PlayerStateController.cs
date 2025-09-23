using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;


public class PlayerStateController : BaseController
{
    [Header("상태 등록")]
    public List<GenealState> states = new List<GenealState>();
    public List<GenealState> allowStates = new List<GenealState>();
    [Tooltip("여기 등록된 상태는 AlwayFixedUpdate가 계속 실행됨.")]

    [Header("State Hash Code")]
    public int currentStateHash = 0;
    public int moveStateHash = 0;
    public int attackStateHash = 0;
    public int counterAttackStateHash = 0;
    public int damagedStateHash = 0;
    public int deadStateHash = 0;
    public int skillStateHash = 0;
    public int dashStateHash = 0;
    public int rollStateHash = 0;

    [Header("Reference")]
    public GenealState currentState = null;
    public GenealState lastState = null;
    public ThirdPersonCamera cam = null;
    public PlayerStatus playerStats = null;
    public PlayerConditions Conditions = null;
    public PlayerSkillController skillController = null;
    public PlayerAnimatior playerAnimatior = null;
    public ComboController comboController = null;
    public ItemCheckController itemCheckController = null;
    public PlayerEquipment playerEquipment = null;
    public Rigidbody rb = null;
    public Transform myTrans = null;
    public CapsuleCollider myCollider = null;
    public Animator myAnimator = null;

    [Header("Variables")]
    [SerializeField] private float groundRadius = 1f;
    private float h;
    private float v;
    private int hFloatHash = 0;
    private int vFloatHash = 0;
    private bool isChangeState = false;
    private float groundDistance = 0f;
    private BaseController attacker = null;

    public BaseController Attacker { get { return attacker; } set { attacker = value; } }
    public float GetH { get => h; }
    public float GetV { get => v; }



    private void Awake()
    {
        if (cam == null) cam = FindObjectOfType<ThirdPersonCamera>();
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (Conditions == null) Conditions = GetComponent<PlayerConditions>();
        if (playerStats == null) playerStats = GetComponent<PlayerStatus>();
        if (playerAnimatior == null) playerAnimatior = GetComponent<PlayerAnimatior>();
        if (skillController == null) skillController = GetComponent<PlayerSkillController>();
        if (itemCheckController == null) itemCheckController = GetComponent<ItemCheckController>();
        if (comboController == null) comboController = GetComponent<ComboController>();
        if (myCollider == null) myCollider = GetComponent<CapsuleCollider>();
        if (myAnimator == null) myAnimator = GetComponent<Animator>();
        if (myTrans == null) myTrans = GetComponent<Transform>();
        if (playerEquipment == null) playerEquipment = GetComponent<PlayerEquipment>();
        hFloatHash = Animator.StringToHash(AnimatorKey.Horizontal);
        vFloatHash = Animator.StringToHash(AnimatorKey.Vertical);
        playerStats.UpdateStats();
    }

    private void Start()
    {
        CopyStateToAllowState();
    }

    private void Update()
    {
        skillController?.CheckUpdateSkillCoolTime(Conditions.SkillNoCooltime);
        skillController?.CheckEnableBuffTime(this);
        skillController?.CheckEnableDeBuffTime(this);
        itemCheckController?.CheckItemCoolTime();


        if (GameManager.Instance.isWriting)
            return;

        for (int i = 0; i < allowStates.Count; i++)
            allowStates[i].AlwaysCheckUpdate(this);

        if (currentState == null && states.Count > 0)
            SetDefaultState();
        if (currentState != null)
            currentState.UpdateAction(this);

        if (Conditions.IsRoll) return;
        HandleMove();
    }

    private void FixedUpdate()
    {
        Conditions.IsGround = IsGrounded();
    }

    /// <summary>
    /// apply root motion과 rigidbody를 같이 적용하면서 중력적용하기위함.
    /// </summary>                                               
    private void OnAnimatorMove()
    {
        if (Conditions.IsLimitedSlope || Conditions.IsCantMove || !Conditions.CanApplyAnimator)
            return;

        if (Conditions.IsRoll)
        {
            //  Vector3 rootMotion = myAnimator.deltaPosition;
            //  rootMotion *= desireRollDistance / rootMotion.magnitude;
            //  rb.velocity.Normalize();
            //  rb.MovePosition(rb.position + rootMotion);
            rb.MovePosition(rb.position);

        }
        else
        rb.MovePosition(rb.position + myAnimator.deltaPosition);
    }


    public override bool IsDead() => Conditions.IsDead;

    public override void Damaged(float damage,BaseController attacker , bool isCritical, bool isSkill, AttackStrengthType attackStrengthType, bool isForceDmg = false) 
        => playerStats.Damaged(damage, attacker, isCritical, isSkill, attackStrengthType, false);

    public override bool CanDetect()
    {
        return Conditions.CanDetect;
    }
    public override bool CanDamage()
    {
        return Conditions.CanDamaged();
    }

    public void StopMove()
    {
        rb.velocity = Vector3.zero;
        rb.MovePosition(transform.position);
        myAnimator.SetFloat(hFloatHash, 0, 0.1f, Time.deltaTime);
        myAnimator.SetFloat(vFloatHash, 0, 0.1f, Time.deltaTime);
    }

    public override BaseStatus GetBaseStatus() => playerStats;

    public override (bool, float) GetDamageValue(bool isSkill)
    {
        bool isCritical = playerStats.IsCritical(playerStats.TotalCriChance);
        float dmg;

        if (isSkill)
            dmg = isCritical ? playerStats.GetCriticalSkillDamage() : playerStats.GetSkillDamage();
        else
            dmg = isCritical ? playerStats.GetCriticalDamage() : playerStats.GetDamage();

        return (isCritical, dmg);
    }

    public void Resurrection(Vector3 resurrectPosition)
    {
        myAnimator.SetBool("IsDead", false);
        TranslatePosition(resurrectPosition);
        playerStats.Resurrection();
        Conditions.Resurrection();
        SetDefaultState();
    }
    public void Resurrection()
    {
        myAnimator.SetBool("IsDead", false);
        playerStats.Resurrection();
        Conditions.Resurrection();
        SetDefaultState();
    }


    private void HandleMove()
    {
        if (GameManager.Instance.isWriting)
            return;

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        myAnimator.SetFloat(hFloatHash, h, 0.1f, Time.deltaTime);
        myAnimator.SetFloat(vFloatHash, v, 0.1f, Time.deltaTime);
        Conditions.IsMoving = IsMove();

    }

  
    public void TranslatePosition(Vector3 position)
    {
        Conditions.CanMove = false;
        myCollider.enabled = false;
        transform.position = position;
        myCollider.enabled = true;
        Conditions.CanMove = true;
    }


    public void RotateToTarget(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;
        dir.Normalize();
        rb.MoveRotation(Quaternion.LookRotation(dir));
    }

    public void Rotate(Vector3 target)
    {
        Debug.Log("Rotate : " + target);
        rb.MoveRotation(Quaternion.Euler(target));
    }


    public void SetDefaultState()
    {
        foreach (GenealState state in states)
        {
            if (moveStateHash == 0)
                return;

            if (state.GetStateHashCode() == moveStateHash)
            {
                currentState = state;
                currentStateHash = moveStateHash;
                lastState = state;
                currentState.Enter(this, -1);
            }
        }
    }

    public void CopyStateToAllowState()
    {
        allowStates.Clear();
        foreach (GenealState state in states)
            allowStates.Add(state);
    }

    public void ChangeState(int nextState, int enumType = -1)
    {
        if (!CheckExistAllowState(nextState)) return;

        foreach (GenealState state in states)
        {
            if (state.GetStateHashCode() == nextState)
            {
                lastState = currentState;
                currentState.Exit(this);
                currentState = state;
                currentStateHash = nextState;
                currentState.Enter(this, enumType);
            }

        }
    }

    public T GetState<T>() where T : GenealState
    {
        foreach (GenealState state in states)
            if (state is T)
                return (state as T);
        return null;
    }

    public void AddState(GenealState state, ref int statehashcode, int initHashCode)
    {
        if (states.Contains(state)) return;
        states.Add(state);
        statehashcode = initHashCode;
    }

    private bool CheckExistAllowState(int hashCode)
    {
        foreach (GenealState state in allowStates)
            if (state.GetStateHashCode() == hashCode)
                return true;
        return false;
    }


   

    public bool IsMove()
    {
        if (Mathf.Abs(h) > Mathf.Epsilon || Mathf.Abs(v) > Mathf.Epsilon)
            return true;

        return false;
    }

    public bool IsSprint()
    {
        if (Conditions.IsSprint)
            return true;

        return false;
    }


    public void RotateToDirection()
    {
        Vector3 direction = GetPlayerDirection();
        Quaternion lookTarget = Quaternion.LookRotation(direction);
        transform.rotation = lookTarget;
    }

    public Vector3 GetPlayerDirection()
    {
        if (IsMove())
        {
            Vector3 direction = cam.transform.parent.forward * v + cam.transform.parent.right * h;
            direction.y = 0f;
            direction.Normalize();
            return direction;
        }
        else
        {
            return transform.forward;
        }
    }

    public DirectionType ReturnNear4DirectionType(Transform origin, Transform target)
    {
        if (target == null)
            return DirectionType.FRNOT;

        Vector3 direction = target.position - origin.position;
        direction.y = 0f;
        direction.Normalize();

        float angle = Vector3.SignedAngle(origin.forward, direction, Vector3.up);
        if (angle < 0f) angle += 360f;
        //Debug.Log(angle);

        if (315 < angle || 0 < angle && angle <= 45)
            return DirectionType.FRNOT;
        else if (45 < angle && angle <= 135)
            return DirectionType.RIGHT;
        else if (135 < angle && angle <= 225)
            return DirectionType.BACK;
        else if (225 < angle && angle <= 315)
            return DirectionType.LEFT;

        return DirectionType.FRNOT;
    }


    //삭제예정. (사유 - 리지드바디 안씀. )
    public void RemoveRigidBodyVertical()
    {
       Vector3 removeVertical = rb.velocity;
       removeVertical.y = 0.0f;
       rb.velocity = removeVertical;
    }

    public bool IsGrounded()
    {
        RaycastHit hitInfo;
        return Physics.SphereCast(myCollider.bounds.center, groundRadius, Vector3.down, out hitInfo, myCollider.bounds.extents.y, groundLayer) ;
    }


    void OnDrawGizmosSelected()
    {
        if (!IsGrounded())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(myCollider.bounds.center, myCollider.bounds.center + Vector3.down * myCollider.bounds.extents.y);
            Gizmos.DrawWireSphere(myCollider.bounds.center + Vector3.down * myCollider.bounds.extents.y, groundRadius);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(myCollider.bounds.center, myCollider.bounds.center + Vector3.down * myCollider.bounds.extents.y);
            Gizmos.DrawWireSphere(myCollider.bounds.center + Vector3.down * myCollider.bounds.extents.y, groundRadius);
        }
    }
}


public abstract class GenealState : MonoBehaviour
{
    protected PlayerStateController controller = null;
    protected int hashCode = 0;


    protected virtual void Awake()
    {
        hashCode = this.GetType().GetHashCode();

        controller = GetComponent<PlayerStateController>();
        if (controller == null)
            controller = GetComponentInParent<PlayerStateController>();

    }

    public int GetStateHashCode()
    {
        return hashCode;
    }

    public int GetTypeHashCode<T>(T type)
    {
        return type.GetHashCode();
    }

    public abstract void Enter(PlayerStateController stateController, int enumType);

    public abstract void UpdateAction(PlayerStateController stateController);

    public virtual void AlwaysCheckUpdate(PlayerStateController stateController) { }

    public abstract void Exit(PlayerStateController stateController);


}

