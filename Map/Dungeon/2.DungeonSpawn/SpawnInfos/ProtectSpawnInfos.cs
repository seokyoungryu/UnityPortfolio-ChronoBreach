using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProtectAIState
{
    STAND = 0,
    MOVE_TO_POINT = 1,
    FOLLOW =2,
}

public enum ProtectDungeonPlayableMoveType
{
    NONE = -1,
    ONCE_FOLLOW_PLAYER = 0,
    ONCE_FOLLOW_PROTECTAI = 1,
    ONLY_FOLLOW_PLAYER = 2,
    ONLY_FOLLOW_PROTECTAI = 3,
    ALL_PLAYABLE_F_PLAYER = 4,
    ALL_PLAYABLE_F_PROTECTAI =5,
}



[System.Serializable]
public class ProtectSpawnInfos : TriggerSpawnInfo<ProtectDungeonRound, BaseDungeonEnemyInfo>
{
    [Header("PlayableAI Follow")]
    [SerializeField] private ProtectDungeonPlayableMoveType playableAIFollowType;
    [Header("웨이브 시작시 AI가 이돌할 경로 (TriggerIndex)")]
    [SerializeField] private int aiWayPointIndex = -1;
    private Vector3 aiWayPoint = Vector3.zero;
    [Header("Entry 가능한 LayerMask")]
    [SerializeField] private LayerMask triggerLayermask = (TagAndLayerDefine.LayersIndex.Player | TagAndLayerDefine.LayersIndex.ProtectAI);
    [Header("AI가 Wave Entry시 이동해있을 위치index가 -1 일 경우 트리거 위치에서 대기 ")]
    [Header("(TriggerIndex)")]
    [SerializeField] private int protectAIStopPositionIndex = -1;
    private Vector3 protectAIStopPosition = Vector3.one;
    [SerializeField] private float aiMoveSpeed = -1f;

    [Header("ProtectAI Following용")]
    [SerializeField] private bool isMoveEntryPosition = false;

    public LayerMask TriggerLayerMask => triggerLayermask;
    public int AiEnterWayPointIndex { get { return aiWayPointIndex; } set { aiWayPointIndex = value; } }
    public Vector3 AIWayPoint { get { return aiWayPoint; } set { aiWayPoint = value; } }
    public int ProtectAIEntryStopPositionIndex { get { return protectAIStopPositionIndex; } set { protectAIStopPositionIndex = value; } }
    public Vector3 ProtectAIStopPosition { get { return protectAIStopPosition; } set { protectAIStopPosition = value; } }
    public float AIMoveSpeed { get { return aiMoveSpeed; } set { aiMoveSpeed = value; } }
    public ProtectDungeonPlayableMoveType PlayableAIFollowType { get { return playableAIFollowType; } set { playableAIFollowType = value; } }
    public bool IsMoveEntryPosition { get { return isMoveEntryPosition; } set { isMoveEntryPosition = value; } }


    public bool IsEndRound(BaseDungeonEnemyInfo[] infos)
    {
        BaseDungeonEnemyInfo[] retInfos = infos;
        for (int i = 0; i < infos.Length; i++)
            if (infos[i].EnemyState == EnemyState.ACTIVE)
                return false;
        return true;
    }

}



[System.Serializable]
public class ProtectDungeonRound : TriggerDungeonRound<BaseDungeonEnemyInfo>
{
    public override bool IsCompleteRound() => IsCompleteRound(enemyInfos.ToArray());

}
