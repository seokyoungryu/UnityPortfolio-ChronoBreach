using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Reward ", fileName = "Reward_")]
public class DungeonReward : ScriptableObject
{
    [SerializeField] private RewardMoney money = null;
    [SerializeField] private RewardExp exp = null;
    [SerializeField] private RewardSkillPoint skillPoint = null;
    [SerializeField] private RewardReputation reputation = null;
    [SerializeField] private RewardItem[] rewardItems;

   public RewardMoney Money => money;
   public RewardExp Exp => exp;
   public RewardSkillPoint SkillPoint => skillPoint;
   public RewardReputation Reputation => reputation;
   public RewardItem[] RewardItems => rewardItems;
}



[System.Serializable]
public class ItemRewardData
{
    [SerializeField] private ItemList item;
    [SerializeField] int itemCount = 1;

    public ItemList Item => item;
    public int ItemCount => itemCount;
}
