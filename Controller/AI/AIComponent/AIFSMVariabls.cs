using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 FSM Action , Decision의 개인 변수들. (ScriptableObject를 공유하지 않기 위한 클래스)
/// </summary>
public class AIFSMVariabls : MonoBehaviour
{
    private AIConditions aIConditions = null;

    #region Action Variables
    [Space(15f), Header("Idle Action Variables")]
    public float relaxationChangeTime = 0f;
    public float currentIdleTimer = 0f;
    public string[] relaxationAnimationClipName = null;
     public int relaxationCount => relaxationAnimationClipName.Length;
    public float minChangeTime = 0f;
    public float maxChangeTime = 0f;

    [Space(15f), Header("Attack Action Variabls")]
    public AIMeleeAttackClip meleeClip = null;
    public AIRangeAttackClip rangeClip = null;
    public bool isFinalClip = false;
    public float attackWaitTime = 0f;
    public float[] meleeAttackTimingFrame = null;
    public float[] meleeAttackTimingTime = null;
    public float rangeAttackTimingTime = 0f;
    public float attackEndTime = 0f;
    public AIMeleeAttackClip pervMeleeClip = null;
    [HideInInspector] public int attackComboCount = 0;
    public List<BaseController> canDamageEnemy = new List<BaseController>();


    [Space(15f), Header("Skill Action Variabls")]
    public float skillSpeed = 0f;
    public int skillCount = 0;
    public bool isDoneSkillProcess = false;
    public SkillType currentSkillType = SkillType.NONE;
    public SkillData currentSkillData = null;
    public AttackSkillClip currentAttackClip = null;
    public MagicSkillClip currentMagicClip = null;
    public BuffSkillClip currentBuffClip = null;
    public float[] attackTiming = null;
    public float[] attackRotateTiming = null;
    public bool isSkillSmoothRotate = false;
    public Quaternion targetRotation = Quaternion.identity;
    public int maxTargetCount = 0;
    public float detectAttackRadius = 0f;
    public float detectSkillRadius = 0f;
    public List<float> attackDamage = new List<float>();
    public List<float> attackRange = new List<float>();
    public List<float> attackAngle = new List<float>();
    public List<float> skillDamage = new List<float>();
    public List<float> skillRange = new List<float>();
    public List<float> skillAngle = new List<float>();

    [Space(15f), Space(15f), Header("Alert Action Variables")]
    public float maxAlertTime = 0f;
    public bool isReachedMaxAlertTime = false;

    [Space(15f), Space(15f), Header("Interact Action Variables")]
    public Vector3 interactOriginRotation = Vector3.zero;
    public Vector3 interactTargetRotation = Vector3.zero;
    public bool canExitInteractAction = false;
    public float interactRotateSmoothValue = 0f;


    [Space(15f), Header("FollowTarget Action Variables")]
    public float currentFollowTimer = 0f;
    public float delayTime = 0f;
    public ProtectDungeonPlayableMoveType followType;

    [Space(15f), Header("Groggy Action Variables")]
    public float groggyingTime = 0f;

    [Space(15f), Header("MoveToTarget Action Variables")]
    public float distance = 0f;
    public float transitionTime = 0.3f;
    public float currentTransitionTImer = 0f;

    [Space(15f), Header("Phase Action Variables")]
    public int phaseCount = 0;
    public bool isPhaseDone = false;

    [Space(15f), Header("Rest Action Variables")]
    public float currentRestTimer = 0f;

    [Space(15f), Header("ResetPosition Action Variables")]
    public Vector3 resetPos = Vector3.zero;
    public float stayPosOffset = 2f;  //IsStayResetPostion 체크할때 거리 offset값.
    public bool isArrivePosition = false;

    [Space(15f), Header("Wait Action Variables")]
    public float timer = 0f;
    public bool isWaitTime = false;

    [Space(15f), Header("Dash Action Variables")]
    public bool isDoneDash = false;                  //hide
    public int currentTotalDashCount = 0;
    public int currentDashCount = 0;                 //hide
    public float currentMaxDashCountTimer = 0f;     //hide
    public float currentDashCoolTimer = 0f;         //hide
    public RaycastHit dashHit;                       //hide
    public float randomDashDistance = 0f;            //hide
    public float randomDirAngle = 0f;              //hide
    public Vector3 dashDirection;  //hide
    public RaycastHit groundHit;               //hide
    public Vector3 dashPosition = Vector3.zero;   //hide
    public Collider[] limitDashColl = new Collider[5];

    [Space(15f), Header("SetCurrentCombatType Action Variables")]
    public float percentage = 0f;
    public float attackPercentage = 0f;
    public float skillPercentage = 0f;

    [Space(15f), Header("Dead Variables")]
    public string deadAnimataionName = string.Empty;
    public int afterDeadProcessFrame = 0;

    [Space(15f), Header("Damaged Variables")]
    private AttackStrengthType damagedStrengthType = AttackStrengthType.NONE;
    private bool isEndDamagedAnimation = false;
    private float currentDamagedTimer = 0f;
    private float damagedTimer = 0f;

    [Space(15f), Header("FindNearEnemy Variables")]
    [SerializeField] private float findNearWaitTime = 0f;
    [SerializeField] private float detectTime = 0f;
    [SerializeField] private float detectNearRange = 0f;
    [SerializeField] private float detectNearAngle = 360f;
    [SerializeField] private float minLimitHeight = -3f;
    [SerializeField] private float maxLimitHeight = 3f;
    public Collider[] nearTargetColliders = new Collider[10];
    private float currentNearDetectTimer = 0f;
    [HideInInspector] public int nearDetectCount = 0;
    private bool isEndNearAction = false;
    private bool canFindNearEnemy = false;
    private IEnumerator waitCoroutine = null;

    [Space(15f), Header("Defense Variables")]
    public bool isStartBlockDefense = false;
    public bool isEndBlockDefense = false;
    public float blockDefenseAnimTime = 0f;
    public float currentBlockTimer = 0f;
    private int currentDefenseCount = 0;
    private float currentDefenseTimer = 0f;
    private bool isDefenseCoolTime = false;

    [Space(15f), Header("Standing Variables")]
    private bool isStandingCoolTime = false;
 
    #endregion

    #region Decision Variables
    [Space(15f), Header("CanAttack Decision ")]
    public float targetAngle = 0f;
    public int detectTargetCount = 0;
    public float currentAttackRange = 0f;
    public Collider[] targetColliders = new Collider[10];

    [Space(15f), Header("CanSKill Decision ")]
    public float skillTargetAngle = 0f;
    public int skillDetectTargetCount = 0;
    public Collider[] skillTargetColliders = new Collider[10];

    [Space(15f), Header("PhaseCheck Decision ")]
    public float currentHpPercentage = 0f;
    public AIPhaseAttackData phaseData = null;
    public int currentPhaseCount = 0;

    [Space(15f), Header("Groggy Decision ")]
    public bool isEndGroggy = false;
    public float groggyInAnimFrameTime = 0f;
    public float groggyOutAnimFrameTime = 0f;
    public int currentGroggyingCount = 0;
    public float currentCumulativeGroggyDamage = 0f;
    public float currentCumulativeTimer = 0f;
    public float groggyCumulativeHpValue = 0f;
    public float currentGroggyCoolTimer = 0f;

    [Space(15f), Header("Rest Decision ")]
    public float currentRestAwaitTimer = 0f;

    [Space(15f), Header("Is Detect Obstacle To Target Decision")]
    public float detectObstacleTime = 2.5f;
   public float currentDetectObstacleTimer = 0f;      // [HideInInspector] 
   public bool isDetectObstacleToTarget = false;      // [HideInInspector] 

    [Space(15f), Header("Can ResetPosition ")]
    public float resetMinDistance = 10f;
    public float resetTimer = 0f;
    [HideInInspector] public float currentResetTimer = 0f;
    [HideInInspector] public float targetDistance = 0f;


    #endregion

    #region Getter Setter
    public bool IsEndDamagedAnimation { get { return isEndDamagedAnimation; } set { isEndDamagedAnimation = value; } }
    public float CurrentDamagedTimer { get { return currentDamagedTimer; } set { currentDamagedTimer = value; } }
    public float DamagedTimer { get { return damagedTimer; } set { damagedTimer = value; } }
    public AttackStrengthType DamagedStrengthType { get { return damagedStrengthType; } set { damagedStrengthType = value; } }

    public float DetectNearTime => detectTime;
    public float DetectNearRange => detectNearRange;
    public float DetectNearAngle => detectNearAngle;
    public float MinLimitHeight => minLimitHeight;
    public float MaxLimitHeight => maxLimitHeight;
    public float CurrentNearDetectTimer { get { return currentNearDetectTimer; } set { currentNearDetectTimer = value; } }
    public bool IsEndNearAction { get { return isEndNearAction; } set { isEndNearAction = value; } }
    public bool CanFindNearEnemy { get { return canFindNearEnemy; } set { canFindNearEnemy = value; } }
    public float FindNearWaitTime { get { return findNearWaitTime; } set { findNearWaitTime = value; } }
    public IEnumerator WaitCoroutine { get { return waitCoroutine; } set { waitCoroutine = value; } }
    public int CurrentDefenseCount { get { return currentDefenseCount; } set { currentDefenseCount = value; } }
    public float CurrentDefenseTimer { get { return currentDefenseTimer; } set { currentDefenseTimer = value; } }
    public bool IsStandingCoolTime { get { return isStandingCoolTime; } set { isStandingCoolTime = value; } }
    public bool IsDefenseCoolTime { get { return isDefenseCoolTime; } set { isDefenseCoolTime = value; } }

   #endregion

    private void Start()
    {
        aIConditions = GetComponent<AIConditions>();
    }


    public void ResetFSMVariables()
    {
        currentTotalDashCount = 0;
        currentDashCount = 0;
        currentDashCoolTimer = 0f;
        currentMaxDashCountTimer = 0f;
    }

}

