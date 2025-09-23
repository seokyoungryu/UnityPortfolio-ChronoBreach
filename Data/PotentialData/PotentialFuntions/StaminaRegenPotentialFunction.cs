using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Stamina Regen Plus", fileName = "StaminaRegenPlusPotentialFunction")]
public class StaminaRegenPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraStaminaRegenPerSec += value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraStaminaRegenPerSec -= value;
    }

}

