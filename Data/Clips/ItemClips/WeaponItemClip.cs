using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemClip : BaseItemClip
{

    [Header("Weapon Stats")]
    public int requiredLevel = 0;
    public int enchantLimitCount = 0;
    public int enchantLeftCount = 0;
    public int enchantLevel = 0;

    public float minAtk, maxAtk = 0f;
    public float minAtkSpeed, maxAtkSpeed = 0f;
    public float minCriChance, maxCriChance = 0f;
    public float minCriDmg, maxCriDmg = 0f;

    public float atkValue = 0f;
    public float atkSpeed = 0f;
    public float criticalChance = 0f; // 0 ~ 100%
    public float criticalDamage = 0f; // 100% ~ 

    public WeaponItemClip() { }
    public WeaponItemClip(int index, string name, int enumNum) : base(index, name, enumNum) { }
    public WeaponItemClip(BaseItemClip clip) : base(clip) { }

    public override void InitBaseStats()
    {
        base.InitBaseStats();
        atkValue = (int)Random.Range(minAtk, maxAtk);
        float tmpAtkSpeed = Random.Range(minAtkSpeed, maxAtkSpeed);
        float tmpCriticalChance = Random.Range(minCriChance, maxCriChance);
        float tmpCriticalDamage = Random.Range(minCriDmg, maxCriDmg);

        atkSpeed = Mathf.Round(tmpAtkSpeed * 100f) / 100f;
        criticalChance = Mathf.Round(tmpCriticalChance * 100f) / 100f;
        criticalDamage = Mathf.Round(tmpCriticalDamage * 100f) / 100f;

        if (atkSpeed <= 0.01f) atkSpeed = 0f;
        if (criticalChance <= 0.09f) criticalChance = 0f;
        if (criticalDamage <= 0.01f) criticalDamage = 0f;

    }


    public override void SetInitRandomPotential()
    {
        base.SetInitRandomPotential();
        SetRandomPotentialOption(potentialRank, PotentialManager.Instance.GetDataBase().weaponOptions);
    }

    public override void SetPotentialValue()
    {
        base.SetPotentialValue();
        SetRandomPotentialOption(potentialRank, PotentialManager.Instance.GetDataBase().weaponOptions);
    }

    public void SetWeaponItemStatus(PlayerStatus playerStatus, bool isEquip)
    {
        if (isEquip)
        {
            SetBaseItemStatus(playerStatus, isEquip);
            playerStatus.ExtraAtk += (int)atkValue;
            playerStatus.ExtraAtkSpeed += atkSpeed;
            playerStatus.ExtraCriticalChance += criticalChance;
            playerStatus.ExtraCriticalDmg += criticalDamage;
            //이부분에 잠재능력 적용?
            if (potentialRank != ItemPotentialRankType.NONE)
                for (int i = 0; i < ownPotential.Length; i++)
                    ownPotential[i].Apply(true, playerStatus);
        }
        else
        {
            SetBaseItemStatus(playerStatus, isEquip);
            playerStatus.ExtraAtk -= (int)atkValue;
            playerStatus.ExtraAtkSpeed -= atkSpeed;
            playerStatus.ExtraCriticalChance -= criticalChance;
            playerStatus.ExtraCriticalDmg -= criticalDamage;
            if (potentialRank != ItemPotentialRankType.NONE)
                for (int i = 0; i < ownPotential.Length; i++)
                    ownPotential[i].Apply(false, playerStatus);
        }
        playerStatus.UpdateStats();
    }

}
