using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    [SerializeField] protected string rewardName = string.Empty;
    [SerializeField] protected string description = string.Empty;
    [SerializeField] protected Sprite icon;

    public string RewardName { get { return rewardName; } set { rewardName = value; } }
    public string Description { get { return description; } set { description = value; } }

    public Sprite Icon { get { return icon; } set { icon = value; } }

    public abstract void Giver(Quest quest);
    public abstract int GetIntValue();

    public abstract void Remove();

}
