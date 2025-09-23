using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractMoveType
{
    NONE = -1,
    MOVE =0,
    ROTATE = 1,
}

public enum InteractMoveDir
{
    GLOBAL = 0,
    LOCAL_FORWARD = 1,
    LOCAL_RIGHT = 2,
    LOCAL_UP = 3,
}


public class InteractMoveObject : InteractTriggerMoveObject
{
    [Header("Type")]
    [SerializeField] protected InteractMoveType interactType = InteractMoveType.NONE;

    [Header("Setting")]
    [SerializeField] protected bool useSmooth = true;
    [SerializeField] protected float smoothSpeed = 1f;



    #region Move

    protected void ExcuteMovePosition(DoorInfo info ,Transform target ,Vector3 velocity)
    {
        if (useSmooth)
        {
            Debug.Log("Move Enter : " + target.position + " Ori : " + info.OriginTargetPos);
            StartCoroutine(SmoothMove(info, info.MoveDirType, velocity));
        }
        else
        {
            Vector3 movePosition = GetMovePosition(info, info.MoveDirType, velocity);
            target.position = movePosition;
        }
    }

    protected Vector3 GetMovePosition(DoorInfo info ,InteractMoveDir type, Vector3 MoveVelocity)
    {
        if (type == InteractMoveDir.GLOBAL)
            return info.OriginTargetPos + MoveVelocity;
        else if (type == InteractMoveDir.LOCAL_FORWARD)
            return info.OriginTargetPos  + info.Target.transform.forward * MoveVelocity.z;
        else if (type == InteractMoveDir.LOCAL_UP)
            return info.OriginTargetPos + info.Target.transform.up * MoveVelocity.y;
        else if (type == InteractMoveDir.LOCAL_RIGHT)
            return info.OriginTargetPos + info.Target.transform.right * MoveVelocity.x;
        return Vector3.zero;
    }

    protected IEnumerator SmoothMove(DoorInfo info, InteractMoveDir type,Vector3 movePosition)
    {
        bool isLoop = true;
        Vector3 targetPos;
        if (interactObject == null) targetPos = movePosition;
        else targetPos = GetMovePosition(info, type, movePosition);
        Debug.Log("Move Excuter : " + targetPos);

        while (isLoop)
        {
            info.Target.position = Vector3.Lerp(info.Target.position, targetPos, smoothSpeed * Time.deltaTime);
            yield return null;

            if (Vector3.Distance(info.Target.position, targetPos) <= 0.1f)
            {
                Debug.Log("브레이크!");
                yield break;
            }
        }
    }

    #endregion

    #region Rotation
    protected void ExcuteRotate(Transform target, Vector3 velocity)
    {
        if (useSmooth)
        {
            StartCoroutine(Rotate(target, velocity));
        }
        else
            target.localRotation = Quaternion.Euler(velocity);
    }


    protected IEnumerator Rotate(Transform target, Vector3 rotate)
    {
        bool isLoop = true;
        Quaternion targetRot;
        if (interactObject == null) targetRot = Quaternion.Euler(rotate);
        else                        targetRot = Quaternion.Euler(-GetDirectOpen(interactObject, rotate));

        while (isLoop)
        {
            target.localRotation = Quaternion.Lerp(target.localRotation, targetRot, smoothSpeed * Time.deltaTime);
            yield return null;

            if (Quaternion.Angle(target.localRotation, targetRot) <= 1f)
                yield break;
        }
    }


    protected Vector3 GetDirectOpen(Transform player, Vector3 rotateVelocity)
    {
        float dot = GetDot(player);
        Vector3 dotRotate = new Vector3(Mathf.Abs(rotateVelocity.x), Mathf.Abs(rotateVelocity.y), Mathf.Abs(rotateVelocity.z));
        if (dot >= 0)
            return -1 * rotateVelocity;
        else if (dot < 0)
            return rotateVelocity;
      
        return Vector3.zero;
    }

    protected float GetDot(Transform player)
    {
        Vector3 dir = player.transform.position - this.transform.position;
        return Vector3.Dot(transform.forward, dir.normalized);
    }

    #endregion
}


[System.Serializable]
public class DoorInfo
{
    [SerializeField] private Transform target = null;
    private Vector3 originTargetPos = Vector3.zero;

    [Header("Move")]
    [SerializeField] private InteractMoveDir moveDirType = InteractMoveDir.GLOBAL;
    [SerializeField] private Vector3 moveOpenVelocity = Vector3.zero;
    [SerializeField] private Vector3 moveCloseVelocity = Vector3.zero;

    [Header("Rotate")]
    [SerializeField] private bool useDirectOpenByPlayer = true;
    [SerializeField] private Vector3 rotateOpenVelocity = Vector3.zero;
    [SerializeField] private Vector3 rotateCloseVelocity = Vector3.zero;


    public Transform Target { get { return target; } set { target = value; } }
    public Vector3 OriginTargetPos { get { return originTargetPos; } set { originTargetPos = value; } }

    public InteractMoveDir MoveDirType => moveDirType;
    public bool UseDirectOpen => useDirectOpenByPlayer;
    public Vector3 MoveOpenVelocity => moveOpenVelocity;
    public Vector3 MoveCloseVelocity => moveCloseVelocity;
    public Vector3 RotateOpenVelocity => rotateOpenVelocity;
    public Vector3 RotateCloseVelocity => rotateCloseVelocity;



    public void SetOriginTargetPos()
    {
        originTargetPos = target.position;
       // Debug.Log("Ori : " + originTargetPos);
    }

}
