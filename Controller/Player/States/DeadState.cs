using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : GenealState
{
    [SerializeField] private Vector3 resurrectionPosition = Vector3.zero;
    [SerializeField] private string deadAnimationName = string.Empty;

    public enum DeadType
    {
        NONE = -1,
        
    }

    protected override void Awake()
    {
        base.Awake();
        controller.AddState(this, ref controller.deadStateHash, this.hashCode);
    }



    public override void Enter(PlayerStateController stateController, int enumType = -1)
    {
        //enum으로 죽는 타입을 받아와서 강공격인지, 그냥 스러지는지 등 애니메이션 실행.
        stateController.myAnimator.CrossFade(deadAnimationName, 0.2f);
        stateController.Conditions.DeadSettings();
        stateController.myAnimator.SetBool("IsDead", true);
        Debug.Log("죽음");

    }

    public override void UpdateAction(PlayerStateController stateController)
    {
        stateController.Conditions.DeadSettings();
        if(Input.GetKeyDown(KeyCode.V))
        {
            stateController.Resurrection(resurrectionPosition);
        }
    }

    public override void Exit(PlayerStateController stateController)
    {
    }

}
