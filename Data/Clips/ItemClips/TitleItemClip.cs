using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleItemClip : BaseItemClip
{
    [Header("Title Stats")]

    public int minAllStats, maxAllStats = 0;
    public int minDef ,maxDef  =0;
    public int minMagicDef, maxMagicDef = 0;

    public float minAtk, maxAtk = 0f;
    public float minAtkSpeed, maxAtkSpeed = 0f;
    public float minHpRegeneration, maxHpRegeneration = 0f;
    public float minStRegeneration, maxStRegeneration = 0f;
    public float minCriChance, maxCriChance = 0f;
    public float minCriDmg, maxCriDmg = 0f;

    public int allStats = 0;
    public int defense = 0;
    public int magicDefense = 0;
    public float atk = 0f;
    public float atkSpeed = 0f;
    public float healthRegeneration = 0f;
    public float staminaRegenaration = 0f;
    public float criticalDamage = 0f;
    public float critialChance = 0f;



    public TitleItemClip() { }
    public TitleItemClip(int index, string name, int enumNum) : base(index, name, enumNum) { }
    public TitleItemClip(BaseItemClip clip) : base(clip) { }



    public override void InitBaseStats()
    {
        base.InitBaseStats();
        allStats = Random.Range(minAllStats, maxAllStats);
        atk = Random.Range(minAtk, maxAtk);
        defense = Random.Range(minDef, maxDef);
        magicDefense = Random.Range(minMagicDef, maxMagicDef);

        float tmpAtkSpeed = Random.Range(minAtkSpeed, maxAtkSpeed);
        float tmpCriticalChance = Random.Range(minCriChance, maxCriChance);
        float tmpCriticalDamage = Random.Range(minCriDmg, maxCriDmg);
        float tmpHealthRegeneration = Random.Range(minHpRegeneration, maxHpRegeneration);
        float tmpStaminaRegenaration = Random.Range(minStRegeneration, maxStRegeneration);

        atkSpeed = Mathf.Round(tmpAtkSpeed * 100f) / 100f;
        critialChance = Mathf.Round(tmpCriticalChance * 100f) / 100f;
        criticalDamage = Mathf.Round(tmpCriticalDamage * 100f) / 100f;
        healthRegeneration = Mathf.Round(tmpHealthRegeneration * 100f) / 100f;
        staminaRegenaration = Mathf.Round(tmpStaminaRegenaration * 100f) / 100f;

        if (atkSpeed <= 0.01f) atkSpeed = 0f;
        if (critialChance <= 0.09f) critialChance = 0f;
        if (criticalDamage <= 0.01f) criticalDamage = 0f;
        if (healthRegeneration <= 0.99f) healthRegeneration = 0f;
        if (staminaRegenaration <= 0.99f) staminaRegenaration = 0f;
    }
    public override void SetInitRandomPotential()
    {
        base.SetInitRandomPotential();
        SetRandomPotentialOption(potentialRank, PotentialManager.Instance.GetDataBase().titleOptions);
    }

    public override void SetPotentialValue()
    {
        base.SetPotentialValue();
        SetRandomPotentialOption(potentialRank, PotentialManager.Instance.GetDataBase().weaponOptions);
    }
    public void SetTitleItemStatus(PlayerStatus playerStatus, bool isEquip)
    {
        if (isEquip)
        {
            SetBaseItemStatus(playerStatus, isEquip);
            playerStatus.ExtraAllStats += allStats;
            playerStatus.ExtraAtk += (int)atk;
            playerStatus.ExtraAtkSpeed += atkSpeed;
            playerStatus.ExtraCriticalChance += critialChance;
            playerStatus.ExtraCriticalDmg += criticalDamage;
            playerStatus.ExtraDefense += defense;
            playerStatus.ExtraMagicDefense += magicDefense;
            playerStatus.ExtraHpRegenPerSec += healthRegeneration;
            playerStatus.ExtraStaminaRegenPerSec += staminaRegenaration;
            if (potentialRank != ItemPotentialRankType.NONE)
                for (int i = 0; i < ownPotential.Length; i++)
                    ownPotential[i].Apply(true, playerStatus);
        }
        else
        {
            SetBaseItemStatus(playerStatus, isEquip);
            playerStatus.ExtraAllStats -= allStats;
            playerStatus.ExtraAtk -= (int)atk;
            playerStatus.ExtraAtkSpeed -= atkSpeed;
            playerStatus.ExtraCriticalChance -= critialChance;
            playerStatus.ExtraCriticalDmg -= criticalDamage;
            playerStatus.ExtraDefense -= defense;
            playerStatus.ExtraMagicDefense -= magicDefense;
            playerStatus.ExtraHpRegenPerSec -= healthRegeneration;
            playerStatus.ExtraStaminaRegenPerSec -= staminaRegenaration;
            if (potentialRank != ItemPotentialRankType.NONE)
                for (int i = 0; i < ownPotential.Length; i++)
                    ownPotential[i].Apply(false, playerStatus);
        }
    }


}
