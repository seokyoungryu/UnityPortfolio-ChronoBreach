using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Stat Point Reward", fileName = "StatPointReward_")]
public class RewardStatPoint : Reward
{
    [SerializeField] private int statPoint = 0;
    public int StatPoint { get { return statPoint; } set { statPoint = value; } }

    public override void Giver(Quest quest)
    {
        GameManager.Instance.Player?.playerStats.AddStatsPoint(statPoint);

    }

    public override int GetIntValue()
    {
        return statPoint;
    }

    public override void Remove()
    {
    }
}
