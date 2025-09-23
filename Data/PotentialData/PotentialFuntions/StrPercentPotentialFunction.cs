using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Str Percentage", fileName = "StrPecentagePotentialFunction")]
public class StrPercentPotentialFunction : PotentialFunctionObject
{

    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.StrPercentage += value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.StrPercentage -= value;
    }

}