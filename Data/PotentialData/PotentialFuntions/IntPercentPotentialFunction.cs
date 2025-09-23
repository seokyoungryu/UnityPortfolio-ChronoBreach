using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Int Percentage", fileName = "IntPecentagePotentialFunction")]
public class IntPercentPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.IntPercentage += value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.IntPercentage -= value;
    }

}
