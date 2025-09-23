using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Hp Regen Plus", fileName = "HpRegenPlusPotentialFunction")]
public class HpRegenPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraHpRegenPerSec += value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraHpRegenPerSec -= value;
    }

}

