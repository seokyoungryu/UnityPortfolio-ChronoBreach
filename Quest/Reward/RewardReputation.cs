using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Reputation Reward", fileName = "ReputationReward_")]
public class RewardReputation : Reward
{
    [SerializeField] private int reputationValue = 1;
    public int ReputationValue { get { return reputationValue; } set { reputationValue = value; } }

    public override void Giver(Quest quest)
    {
        GameManager.Instance.AddReputation(reputationValue,this);
    }
    public override int GetIntValue()
    {
        return reputationValue;
    }

    public override void Remove()
    {
    }
}
