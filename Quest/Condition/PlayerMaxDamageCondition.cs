using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Conditions/Require Condition/Max Damage Conditon", fileName = "MaxDmgCondition_")]
public class PlayerMaxDamageCondition : QuestCondition
{
    [Header("Value보다 낮을 경우 false")]
    [SerializeField] private int maxDamageValue = 0;
    public override bool IsPass(Quest quest)
    {
        PlayerStatus playerStatus = GameManager.Instance.Player.playerStats;
        if (playerStatus.GetMaxDamage(false) < maxDamageValue)
            return false;
        return true;
    }
}
