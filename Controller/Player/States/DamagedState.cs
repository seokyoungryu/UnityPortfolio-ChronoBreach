using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : GenealState
{
    [SerializeField] private BaseController attacker = null;

    [Header("Referense")]
    [Header("[0] : Front ,[1] : Back ,[2] : Right,[3] : Left")]
    public List<DamagedClip> weakDamaged_Direction = new List<DamagedClip>();

    [Space(10) ,Header("[0] : Normal , [1] : Strength ,")]
    public List<DamagedClip> damaged_Strength = new List<DamagedClip>();

    [Header("Rise Clip")]
    public DamagedClip riseClip = null;

    [Header("Variables")]
    private bool canRise = false;
    private string damagedAnimationName = string.Empty;

    [Header("Sounds")]
    [SerializeField] private SoundList[] randomDamagedSound;

    private IEnumerator dmg_Co;

    protected override void Awake()
    {
        base.Awake();
        controller.AddState(this, ref controller.damagedStateHash, hashCode);
    }


    public override void Enter(PlayerStateController stateController, int enumType = -1)
    {
        if (dmg_Co != null)
            StopCoroutine(dmg_Co);

        damagedAnimationName = string.Empty;
        attacker = stateController.Attacker;
        stateController.Attacker = null;
        canRise = false;
        AttackStrengthType attackStrengthType = (AttackStrengthType)enumType;
        DamagedClip clip = null;

        SoundManager.Instance.PlayEffect(randomDamagedSound);
       
        switch (attackStrengthType)
        {
            case AttackStrengthType.WEAK:
                DirectionType nearDirection = stateController.ReturnNear4DirectionType(stateController.transform, attacker?.transform);
                clip = weakDamaged_Direction[(int)nearDirection];
                GameManager.Instance.MainPP.ExcuteAnimate(PPType.MOTIONBLUR_DAMAGED_WEAK);
                GameManager.Instance.MainPP.ExcuteAnimate(PPType.DEPTH_OF_FIELD_WEAK);
                dmg_Co = StandDamagedProcess(clip);
                StartCoroutine(dmg_Co);
                break;
            case AttackStrengthType.NORMAL:
                clip = damaged_Strength[(int)attackStrengthType - 1];
                GameManager.Instance.MainPP.ExcuteAnimate(PPType.MOTIONBLUR_DAMAGED_NORMAL);
                GameManager.Instance.MainPP.ExcuteAnimate(PPType.DEPTH_OF_FIELD_NORMAL);
                dmg_Co = StandDamagedProcess(clip);
                StartCoroutine(dmg_Co);
                break;
            case AttackStrengthType.STRONG:
                clip = damaged_Strength[(int)attackStrengthType - 1];
                GameManager.Instance.MainPP.ExcuteAnimate(PPType.MOTIONBLUR_DAMAGED_STRONG);
                GameManager.Instance.MainPP.ExcuteAnimate(PPType.DEPTH_OF_FIELD_STRONG);

                dmg_Co = StandDamagedProcess(clip);
                StartCoroutine(dmg_Co);
                break;
            case AttackStrengthType.FLYDOWN:
                clip = damaged_Strength[(int)attackStrengthType - 1];
                GameManager.Instance.MainPP.ExcuteAnimate(PPType.MOTIONBLUR_DAMAGED_STRONG);
                GameManager.Instance.MainPP.ExcuteAnimate(PPType.DEPTH_OF_FIELD_STRONG);
                dmg_Co = DownDamagedProcess(clip);
                StartCoroutine(dmg_Co); 
                break;
        }

    }


    public override void UpdateAction(PlayerStateController stateController)
    {
        if(IsDown() && canRise && Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(RiseProcess());

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (stateController.Conditions.CanDash)
                stateController.ChangeState(stateController.dashStateHash);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !IsDown())
        {
            if (stateController.IsMove() == false && stateController.Conditions.CanChangeCountAttackState())
                stateController.ChangeState(stateController.counterAttackStateHash);
            else if (stateController.IsMove() == true && stateController.Conditions.CanRoll)
                stateController.ChangeState(stateController.rollStateHash);
        }

    }

    public override void Exit(PlayerStateController stateController)
    {
        damagedAnimationName = string.Empty;
        canRise = false;
        StopAllCoroutines();
    }

    private IEnumerator StandDamagedProcess(DamagedClip clip)
    {
        if (clip != null)
            GameManager.Instance.Cam.ShakeCamera(clip.CameraShakeInfo);

        if (clip.RotateToAttacker)
            RotateToTarget(attacker.transform);

        controller.playerAnimatior.DamagedAnimationSpeed = clip.AnimationPlaySpeed;
        controller.myAnimator.CrossFade(clip.AniamtionName, 0.1f,2,0f);
        yield return new WaitForSeconds(clip.CanDamagedFrameToTime());

        controller.Conditions.IsDamaged = false;
        yield return new WaitForSeconds(clip.EndAnimationFrameToTime() - clip.CanDamagedFrameToTime());

        controller.ChangeState(controller.moveStateHash, -1);
    }

    private IEnumerator DownDamagedProcess(DamagedClip clip)
    {
        if (clip.RotateToAttacker)
            RotateToTarget(attacker?.transform);

        controller.playerAnimatior.DamagedAnimationSpeed = clip.AnimationPlaySpeed;
        controller.myAnimator.CrossFade(clip.AniamtionName, 0.1f);
        yield return new WaitForSeconds(clip.CanDamagedFrameToTime());

        controller.Conditions.IsDamaged = false;
        controller.Conditions.IsDown = true;
        yield return new WaitForSeconds(clip.EndAnimationFrameToTime() - clip.CanDamagedFrameToTime());

        canRise = true;
    }

    private IEnumerator RiseProcess()
    {
        canRise = false;
        controller.myAnimator.CrossFade(riseClip.AniamtionName, 0.2f);

        yield return new WaitForSeconds(riseClip.EndAnimationFrameToTime());
        controller.Conditions.IsDown = false;

        controller.ChangeState(controller.moveStateHash, -1);
    }


    private void RotateToTarget(Transform targetTr)
    {
        if (targetTr == null)
            return;

        Vector3 dir = targetTr.position - controller.transform.position;
        dir.y = 0f;
        dir.Normalize();
        Quaternion look = Quaternion.LookRotation(dir);
        controller.transform.rotation = look;
    }

    private bool IsDown()
    {
        return controller.Conditions.IsDown;
    }
}
