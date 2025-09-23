using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMoveOneObject : InteractMoveObject
{
    [Header("Info")]
    [SerializeField] private DoorInfo doorInfo;



    private void Start()
    {
        doorInfo.SetOriginTargetPos();
    }


    protected override void Open()
    {
        base.Open();
        StopAllCoroutines();

        if (interactType == InteractMoveType.ROTATE)
            ExcuteRotate(doorInfo.Target, doorInfo.RotateOpenVelocity);
        else if (interactType == InteractMoveType.MOVE)
            ExcuteMovePosition( doorInfo, doorInfo.Target,  doorInfo.MoveOpenVelocity);
    }


    protected override void Close()
    {
        base.Close();
        StopAllCoroutines();

        if (interactType == InteractMoveType.ROTATE)
            ExcuteRotate(doorInfo.Target,doorInfo.RotateCloseVelocity);
        else if (interactType == InteractMoveType.MOVE)
            ExcuteMovePosition(doorInfo, doorInfo.Target, doorInfo.MoveCloseVelocity);
    }

}
