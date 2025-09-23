using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Atk Percent", fileName = "AtkPercentPotentialFunction")]
public class AtkPercentPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.AtkPercentage += value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.AtkPercentage -= value;
    }

}
