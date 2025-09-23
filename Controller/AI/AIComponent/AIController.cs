using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class AIController : BaseController
{
    public AIStateDatabase stateDatabase = null;
    [SerializeField] private bool isPlayableObject = false;
    [SerializeField] private Transform dashTargetMaskTr = null;
    [SerializeField] private Transform modelTr;
    [SerializeField] private Transform skillNotifierEffectTr = null;
    [SerializeField] private EffectInfo skillNotifierEffect;
    [SerializeField] private EffectInfo dashEffect;
    public SoundList[] defenseSound;
    public SoundList[] standingSound;

    [Header("State Database의 startState로 초기화")]
    public State currentState = null;
    public State lastState = null;
    public State remainState = null;

    [Header("Reference")]
    public AIInfoList aiInfoList = AIInfoList.None;
    public AIAttackType aiAttackType = AIAttackType.MELEE;
    [HideInInspector] public AIStatus aiStatus = null;
    [HideInInspector] public AIVariables aIVariables = null;
    [HideInInspector] public AIFSMVariabls aIFSMVariabls = null;
    [HideInInspector] public AISkillController skillController = null;
    [HideInInspector] public AIConditions aiConditions = null;
    [HideInInspector] public DetectCollider detectCollider = null;
    [HideInInspector] public NavMeshAgent nav = null;
    [HideInInspector] public QuestReporter questReporter = null;
    [HideInInspector] public Animator aiAnim = null;
    [HideInInspector] public Rigidbody myRigid = null;
    [HideInInspector] public Collider myColl = null;
    [SerializeField] private HitEffect hitEffect = null;
    [SerializeField] private AIHpBarUI aiHpBarUI = null;
    [SerializeField] private GameObject dungeonTargetMarker = null;
    [HideInInspector] public NpcController npcController = null;
    [SerializeField] private QuestTargetMarker targetMarker = null;
    public GameObject obpGo = null;

    public List<WayPointInfo> wayPointInfos = new List<WayPointInfo>();
    public int currentWaypointIndex = 0;
    private string obpName = string.Empty;

    [Header("Check Collider")]
    public int checkInSightTargetCount = 0;                            //hide
    public Collider[] checkTargetColliders = new Collider[10];        //hide
    public int checkDetectAttackTargetCount = 0;                       //hide
    public Collider[] checkDetectAttackTargetColliders = new Collider[10];  //hide

    [Header("AI Attack Data")]
    public AIAttackClip[] aiAttackClips = null;                      //hide

    [Header("Damaged Clips")]
    public DamagedClip[] damagedClips;
    public DamagedClip riseClip;

    private GUIStyle style = new GUIStyle();

    private int speedHash = 0;                                            //hide
    public Transform test;                                               //hide
    public int testdebugID;                                              //hide

    #region Events
    public delegate void OnDead();
    public event OnDead onExtraDead;
    public event OnDead onConstDead;


    #endregion


    public Transform DashTargetMaskTr => dashTargetMaskTr;
    public HitEffect HitEffect => hitEffect;
    public bool IsPlayableObject { get { return isPlayableObject; } set { isPlayableObject = value; } }
    public AIHpBarUI AiHpBarUI { get { return aiHpBarUI; } set { aiHpBarUI = value; } }

    public string OBPName => obpName;
    public EffectInfo SkillNotifierEffect => skillNotifierEffect;
    public Transform SkillNotifierEffectTr => skillNotifierEffectTr;
    public EffectInfo DashEffect => dashEffect;

    public enum AIAttackType
    {
        MELEE = 0,
        RANGE = 1,
    }

    //테스트용 
    public Transform Test1;
    public bool isGo = false;


    private void Awake()
    {
        questReporter = GetComponent<QuestReporter>();
        nav = GetComponent<NavMeshAgent>();
        aiAnim = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
        aiStatus = GetComponent<AIStatus>();
        skillController = GetComponent<AISkillController>();
        aiConditions = GetComponent<AIConditions>();
        aIVariables = GetComponent<AIVariables>();
        aIFSMVariabls = GetComponent<AIFSMVariabls>();
        if (targetMarker == null) GetComponentInChildren<QuestTargetMarker>();
        if (npcController == null) npcController = GetComponentInChildren<NpcController>();
        if (aiHpBarUI == null) aiHpBarUI = GetComponentInChildren<AIHpBarUI>();
        detectCollider = GetComponentInChildren<DetectCollider>();
        myColl = GetComponent<Collider>();
        speedHash = Animator.StringToHash(AnimatorKey.Speed);
        aiAnim.applyRootMotion = true;
        if (hitEffect == null)
            hitEffect = GetComponentInChildren<HitEffect>();
        testdebugID = Random.Range(0, 100);
    }

    private void OnAnimatorMove()
    {
        if (aiConditions.IsAttacking || aiConditions.IsSkilling || aiConditions.IsDamageState || nav.velocity != Vector3.zero)
        {
            if (!aiConditions.detectedOn)
            {
                Vector3 rootPos = aiAnim.rootPosition;
                rootPos.y = nav.nextPosition.y;
                transform.position = rootPos;
                nav.nextPosition = rootPos;
            }
        }

    }

    private void Start()
    {
        if (stateDatabase != null)
        {
            currentState = stateDatabase.startState;
            currentState.OnEnterInit(this);
        }
        aiStatus.InitInfoData(aiInfoList);
    }

    private void Update()
    {
        skillController.CheckUpdateSkillCoolTime();
        skillController.CheckEnableBuffTime(this);
        skillController.CheckEnableDeBuffTime(this);

        if (aIFSMVariabls.currentDashCount < aIVariables.maxDashCount)
            CheckDashCoolTime();
        else if (aIFSMVariabls.currentDashCount >= aIVariables.maxDashCount)
            CheckMaxDashCoolTime();

        if (currentState != null && aiConditions.CanState)
        {
            currentState.UpdateAction(this, Time.deltaTime);
            currentState.UpdateCheckTransition(this);
        }

        if (aIVariables.target)
        {
            SetCombatMode(true);
            aIVariables.SetIfTargetIsDead();
        }
        else
            SetCombatMode(false);

        aiAnim.SetFloat(speedHash, nav.velocity.magnitude);
    }


    public void SetOBPName(string name) => obpName = name;
    public override bool IsDead() => aiConditions.IsDead;

    public override void Damaged(float damage, BaseController attacker, bool isCritical, bool isSkill, AttackStrengthType attackStrengthType, bool isForceDmg = false)
        => aiStatus.Damaged(damage, attacker, isCritical, isSkill, attackStrengthType, isForceDmg);


    public override bool CanDetect()
    {
        return aiConditions.CanDetect;
    }

    public override bool CanDamage()
    {
        return aiConditions.CanDamaged;
    }
    public override BaseStatus GetBaseStatus() => aiStatus;

    public void ActiveDungeonTargetMarker(bool active) => dungeonTargetMarker.SetActive(active);

    /// <summary>
    /// bool : IsCritical, int : DamageValue
    /// </summary>
    public override (bool, float) GetDamageValue(bool isSkill)
    {
        bool isCritical = aiStatus.IsCritical(aiStatus.CurrentCriChance);
        return (isCritical,aiStatus.GetDamage(isCritical));
    }

    public void SetNavSpeed(float navSpeed)
    {
        aiStatus.CurrentMoveSpeed = navSpeed;
        nav.speed = aiStatus.CurrentMoveSpeed * aiStatus.ExtraMoveSpeed;
    }

    public void UpdateNavSpeed()
    {
        nav.speed = aiStatus.CurrentMoveSpeed * aiStatus.ExtraMoveSpeed;
    }

    public bool IsMove()
    {
        if (nav.velocity.magnitude > Mathf.Epsilon)
            return true;

        return false;
    }


    public void ResetAI()
    {
        myColl.enabled = true;
        myColl.isTrigger = false;
        nav.enabled = true;
        dungeonTargetMarker.SetActive(false);
        currentWaypointIndex = 0;
        lastState = null;
        currentState = stateDatabase.startState;
        aIVariables.target = null;
        aiStatus.InitInfoData(aiInfoList);
        checkDetectAttackTargetCount = 0;
        checkDetectAttackTargetColliders = new Collider[10];
        checkInSightTargetCount = 0;
        checkTargetColliders = new Collider[10];
        aiConditions.ResetCondition();
        aIFSMVariabls.ResetFSMVariables();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        SetCombatMode(false);
        aiAnim.Play("Idle", 5);
        ClearWayPoints();
    }

    public void ClearOnDead() => onExtraDead = null;
    public void Dead()
    {
        onConstDead?.Invoke();
        onExtraDead?.Invoke();
        myColl.enabled = false;
        nav.enabled = false;
        aiStatus.ResetRecycle_Co();
    }

    public void Kill()
    {
        aiStatus.SetCurrentHP(-1);
        Damaged(0, null, false, false , AttackStrengthType.NONE);
    }


    public void ClearWayPoints() => wayPointInfos.Clear();

    public void SetWayPoints(WayPointInfo wayPointInfo, Transform wayPointTr)
    {
        wayPointInfo.WayPointTr = wayPointTr;
        wayPointInfos.Add(wayPointInfo);
    }

    public bool TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            aiConditions.IsChangingState = true;
            currentState.OnExitInit(this);
            lastState = currentState;
            currentState = nextState;
            aiConditions.IsChangingState = false;
            currentState.OnEnterInit(this);
            return true;
        }

        return false;
    }

    public void TranslatePosition(Vector3 position)
    {
        nav.enabled = false;
        transform.position = position;
        nav.enabled = true;
    }

    public void RotateByVector(Vector3 rotation)
    {
        nav.enabled = false;
        transform.rotation = Quaternion.Euler(rotation);
        nav.enabled = true;
    }

    public void RotateByVector(Quaternion rotation)
    {
        nav.enabled = false;
        transform.rotation = rotation;
        nav.enabled = true;
    }

    public void EnemyDetect(bool canDetect)
    {
        if(canDetect)
        {
            myColl.enabled = true;
            modelTr.gameObject.SetActive(true);
            if (aiHpBarUI != null)
                aiHpBarUI.IsHide = false;
            aiConditions.CanDetect = true;
        }
        else
        {
            myColl.enabled = false;
            modelTr.gameObject.SetActive(false);
            if (aiHpBarUI != null)
                aiHpBarUI.IsHide = true;
            aiConditions.CanDetect = false;
            hitEffect.ResetEffect();
        }
    }


    public void CheckDashCoolTime()
    {
        if (aiConditions.CanDash) return;
        aIFSMVariabls.currentDashCoolTimer += Time.deltaTime;
        if (aIFSMVariabls.currentDashCoolTimer >= aIVariables.dashCoolTime)
        {
            aIFSMVariabls.currentDashCoolTimer = 0f;
            aiConditions.CanDash = true;
        }

    }


    public void CheckMaxDashCoolTime()
    {
        if (aiConditions.CanDash) return;
        aIFSMVariabls.currentMaxDashCountTimer += Time.deltaTime;
        if (aIFSMVariabls.currentMaxDashCountTimer >= aIVariables.maxDashCountResetCoolTime)
        {
            aIFSMVariabls.currentDashCount = 0;
            aIFSMVariabls.currentMaxDashCountTimer = 0f;
            aiConditions.CanDash = true;
        }
    }


    public void RotateTarget(Transform target)
    {
        Vector3 dir = (target.position - transform.position);
        dir.y = 0f;
        dir.Normalize();
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = look;
    }

    public void ResetCheckTargetInSight()
    {
        checkInSightTargetCount = 0;
        for (int i = 0; i < checkTargetColliders.Length; i++)
            checkTargetColliders[i] = null;
    }

    public void ResetCheckTargetInAttackDamaged()
    {
        checkDetectAttackTargetCount = 0;
        for (int i = 0; i < checkDetectAttackTargetColliders.Length; i++)
            checkDetectAttackTargetColliders[i] = null;
    }

    public void ResetRigidBodyVelocity()
    {
        if (myRigid.velocity.magnitude > 0)
            myRigid.velocity = Vector3.zero;
    }

    public void SetCombatMode(bool isCombat)
    {
        aiAnim.SetBool("CombatMode", isCombat);
    }




#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (currentState != null)
            currentState.DrawDecisionGizmos(this);

        if (currentState != null)
        {
            Handles.color = Color.blue;
            Handles.Label(transform.position + Vector3.up * 4f, currentState.name);
            Gizmos.color = currentState.stateColor;
            Gizmos.DrawSphere(transform.position + Vector3.up * 3f, 0.5f);

        }
        else
        {
            Handles.color = Color.blue;
            Handles.Label(transform.position + Vector3.up * 4f, "NONE");

            Gizmos.color = Color.clear;
            Gizmos.DrawSphere(transform.position + Vector3.up * 3f, 0.5f);
        }

        if (aIFSMVariabls != null && aIFSMVariabls.currentAttackRange != 0f)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, aIFSMVariabls.currentAttackRange);
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.black;
            Handles.Label(transform.position + Vector3.forward * aIFSMVariabls.currentAttackRange, "AttackRange", style);
        }

        

    }


    private void OnDrawGizmosSelected()
    {
        if(wayPointInfos != null && wayPointInfos.Count > 0)
        {
            for (int i = 0; i < wayPointInfos.Count; i++)
            {
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.magenta;
                Handles.Label(wayPointInfos[i].WayPointTr.position + Vector3.up * 1.5f, "Way " + i, style);
                Gizmos.DrawWireCube(wayPointInfos[i].WayPointTr.position, Vector3.one * 0.5f);
            }

        }

        if (!Application.isPlaying || aIFSMVariabls.detectAttackRadius <= 0f || aIFSMVariabls.detectSkillRadius <= 0f) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, aIFSMVariabls.detectAttackRadius);
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        Handles.Label(transform.position + Vector3.up * 2f + -Vector3.forward * aIFSMVariabls.detectAttackRadius, "Detect Attack Range", style);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, aIFSMVariabls.detectSkillRadius);
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        Handles.Label(transform.position + -Vector3.forward * aIFSMVariabls.detectSkillRadius, "Detect Skill Range", style);

    }
#endif

}
