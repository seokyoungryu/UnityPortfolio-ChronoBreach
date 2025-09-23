using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Luck Percentage", fileName = "LuckPecentagePotentialFunction")]
public class LuckPercentPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.LuckPercentage += value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.LuckPercentage -= value;
    }

}
