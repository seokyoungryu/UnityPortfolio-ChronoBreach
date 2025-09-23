using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Exp Reward", fileName = "ExpReward_")]
public class RewardExp : Reward
{
    [SerializeField] private int expValue = 0;


    public int ExpValue { get { return expValue; } set { expValue = value; } }

    public override void Giver(Quest quest)
    {
        GameManager.Instance.Player.playerStats.AddExp(expValue, this);
    }
    public override int GetIntValue()
    {
        return expValue;
    }

    public override void Remove()
    {
    }
}
