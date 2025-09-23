using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Skill Point Reward", fileName = "SkillPointReward_")]
public class RewardSkillPoint : Reward
{
    [SerializeField] private int skillPoint = 0;
    public int SkillPoint { get { return skillPoint; }  set{ skillPoint = value; } }

    public override void Giver(Quest quest)
    {
        GameManager.Instance.Player.playerStats.AddSkillPoint(skillPoint, this);
    }
    public override int GetIntValue()
    {
        return skillPoint;
    }

    public override void Remove()
    {
    }
}
