using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Max Stamina Plus", fileName = "MaxStaminaPlusPotentialFunction")]
public class StaminaPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraStamina += (int)value;
        playerStatus.SetCurrentStamina(playerStatus.CurrentStamina + (int)value);
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraStamina -= (int)value;
        playerStatus.SetCurrentStamina(playerStatus.CurrentStamina - (int)value);
    }

}
