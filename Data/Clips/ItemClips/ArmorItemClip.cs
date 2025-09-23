using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItemClip : BaseItemClip
{
    [Header("Armor Stats")]
    public ArmorType armorType = ArmorType.HAND;
    public int requiredLevel = 0;
    public int enchantLevel = 0;
    public int enchantLeftCount = 0;
    public int enchantLimitCount = 0;

    public int minDef, maxDef = 0;
    public int minMagicDef, maxMagicDef = 0;
    public float minHpRegeneration, maxHpRegeneration = 0f;
    public float minStRegeneration, maxStRegeneration = 0f;
    public float minEvasion, maxEvasion = 0f;

    public int defense = 0;
    public int magicDefense = 0;
    public float evasion = 0f;
    public float healthRegeneration = 0f;
    public float staminaRegeneration = 0f;

    public ArmorItemClip() { }
    public ArmorItemClip(int index, string name, int enumNum) : base(index, name, enumNum) { }
    public ArmorItemClip(BaseItemClip clip) : base(clip) { }


    public override void InitBaseStats()
    {
        base.InitBaseStats();
        defense = Random.Range(minDef, maxDef);
        magicDefense = Random.Range(minMagicDef, maxMagicDef);

        float tmpHealthRegeneration = Random.Range(minHpRegeneration, maxHpRegeneration);
        float tmpStaminaRegenaration = Random.Range(minStRegeneration, maxStRegeneration);
        float tmpEvasion = Random.Range(minEvasion, maxEvasion);

        healthRegeneration = Mathf.Round(tmpHealthRegeneration * 100f) / 100f;
        staminaRegeneration = Mathf.Round(tmpStaminaRegenaration * 100f) / 100f;
        evasion = Mathf.Round(tmpEvasion * 100f) / 100f;

        if (healthRegeneration <= 0.99f) healthRegeneration = 0f;
        if (staminaRegeneration <= 0.99f) staminaRegeneration = 0f;
        if (evasion <= 0.09f) evasion = 0f;
    }


    public override void SetInitRandomPotential()
    {
        base.SetInitRandomPotential();
        SetRandomPotentialOption(potentialRank, PotentialManager.Instance.GetDataBase().armorOptions);
    }
    public override void SetPotentialValue()
    {
        base.SetPotentialValue();
        SetRandomPotentialOption(potentialRank, PotentialManager.Instance.GetDataBase().weaponOptions);
    }
    public void SetArmorItemStatus(PlayerStatus playerStatus, bool isEquip)
    {
        if (isEquip)
        {
            SetBaseItemStatus(playerStatus, isEquip);
            playerStatus.ExtraDefense += defense;
            playerStatus.ExtraMagicDefense += magicDefense;
            playerStatus.ExtraHpRegenPerSec += healthRegeneration;
            playerStatus.ExtraStaminaRegenPerSec += staminaRegeneration;
            playerStatus.ExtraEvasion += evasion; 
            if (potentialRank != ItemPotentialRankType.NONE)
                for (int i = 0; i < ownPotential.Length; i++)
                    ownPotential[i].Apply(true, playerStatus);
        }
        else
        {
            SetBaseItemStatus(playerStatus, isEquip);
            playerStatus.ExtraDefense -= defense;
            playerStatus.ExtraMagicDefense -= magicDefense;
            playerStatus.ExtraHpRegenPerSec -= healthRegeneration;
            playerStatus.ExtraStaminaRegenPerSec -= staminaRegeneration;
            playerStatus.ExtraEvasion -= evasion;
            if (potentialRank != ItemPotentialRankType.NONE)
                for (int i = 0; i < ownPotential.Length; i++)
                    ownPotential[i].Apply(false, playerStatus);
        }
    }


}



public enum ArmorType
{
    HEAD = 0,
    UPPER = 1,
    LOWER = 2,
    HAND = 3,
    LEG = 4,
}

