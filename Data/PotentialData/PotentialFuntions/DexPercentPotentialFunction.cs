using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Dex Percentage", fileName = "DexPecentagePotentialFunction")]
public class DexPercentPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.DexPercentage += value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.DexPercentage -= value;
    }

}