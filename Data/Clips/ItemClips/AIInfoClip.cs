using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIType
{
    COMMON = 0,
    ELITE = 1,
    BOSS = 2,
}

[System.Serializable]
public class AIInfoClip : BaseEditorClip
{

    [Header("AI Infomation")]
    public string aiNameEnum = string.Empty;
    public string originDisplayName = string.Empty;
    public string characteristicsDisplayName = string.Empty;
    public AIType aiType = AIType.COMMON;
    public int level = 0;
    public int healthBarCount = 0;
    public int health = 0;
    public float atk = 0f;
    public float minAtkPercent = 0f;
    public float atkSpeed = 0f;
    public float defense = 0f;
    public float magicDefense = 0f;
    public float criticalChance = 0;
    public float criticalDmg = 0;
    public float evasion = 0f;

    public float standingCoolTime = 0f;
    public bool ignoreDamageState = false;
    public bool immortality = false;

    public bool recycleHpReset = false;
    public float recycleHpResetTime = 3f;

    public bool useStanding = false;
    public float standingPercent = 0f;
    public AIStandingType standingType = AIStandingType.ATTACK_COUNT;
    public int standingAttackCount = 0;
    public float increaseStandingAttackSpeed = 0f;

    public float defenseCoolTime = 0f;
    public bool useDefense = false;
    public float defensePercent = 0f;
    public AIDefenseType defenseType = AIDefenseType.NONE;
    public int defenseCount = 0;
    public float defensingTime = 0f;
    public float exp = 0f;
    public DropItem[] dropItems = new DropItem[0];

    public AIInfoClip() { }
    public AIInfoClip(string name, int indexID, int enumType)
    {
        id = indexID;
        aiNameEnum = name;
        aiType = (AIType)enumType;
    }

    public void AddDropItem()
    {
        dropItems = ArrayHelper.Add(new DropItem(), dropItems);
    }

    public void RemoveDropItem(int index)
    {
        dropItems = ArrayHelper.Remove(index, dropItems);
    }
}   



