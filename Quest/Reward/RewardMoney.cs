using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Money Reward", fileName = "MoneyReward_")]
public class RewardMoney : Reward
{
    [SerializeField] private int money = 0;

    public int Money { get { return money; } set { money = value; } }

    public override void Giver(Quest quest)
    {
        GameManager.Instance.SetPlusOwnMoney(money, this);
    }
    public override int GetIntValue()
    {
        return money;
    }
    public override void Remove()
    {
        GameManager.Instance.SetMinusOwnMoney(money);
    }
}
