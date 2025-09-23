using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMoveMultiObjects : InteractMoveObject
{
    [Header("Info")]
    [SerializeField] private DoorInfo[] doorInfos;


    private void Start()
    {
        for (int i = 0; i < doorInfos.Length; i++)
            doorInfos[i].SetOriginTargetPos();
    }


    protected override void Open()
    {
        base.Open();
        StopAllCoroutines();

        for (int i = 0; i < doorInfos.Length; i++)
        {
            if (interactType == InteractMoveType.ROTATE)
                ExcuteRotate(doorInfos[i].Target, doorInfos[i].RotateOpenVelocity);
            else if (interactType == InteractMoveType.MOVE)
                ExcuteMovePosition(doorInfos[i], doorInfos[i].Target, doorInfos[i].MoveOpenVelocity);
        }

    }



    protected override void Close()
    {
        base.Close();
        StopAllCoroutines();

        for (int i = 0; i < doorInfos.Length; i++)
        {
            if (interactType == InteractMoveType.ROTATE)
                ExcuteRotate(doorInfos[i].Target, doorInfos[i].RotateCloseVelocity);
            else if (interactType == InteractMoveType.MOVE)
                ExcuteMovePosition(doorInfos[i], doorInfos[i].Target, doorInfos[i].MoveCloseVelocity);
        }
    }

}
