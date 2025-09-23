using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public void Damaged(float damage , BaseController attacker ,bool isCritical, bool isSkill , AttackStrengthType attackStrengthType,bool isForceDmg = false);

}
