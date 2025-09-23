using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Map/Dungeon Category/Normal ", fileName ="NormalDungeonCategory")]
public class NormalDungeonCategory : BaseDungeonCateogry
{


    public override PlayerStateController InitControllerSetting(BaseDungeonTitle title)
    {
        PlayerStateController originController = title.OriginController;
        originController.allowStates.Clear();
        originController.allowStates.Add(originController.GetState<MoveState>());
        originController.allowStates.Add(originController.GetState<AttackState>());
        originController.allowStates.Add(originController.GetState<RollState>());
        originController.allowStates.Add(originController.GetState<SkillState>());
        originController.allowStates.Add(originController.GetState<DamagedState>());
        originController.allowStates.Add(originController.GetState<CounterAttackState>());
        originController.allowStates.Add(originController.GetState<DeadState>());
        originController.allowStates.Add(originController.GetState<DashState>());
        return originController;
    }
}
