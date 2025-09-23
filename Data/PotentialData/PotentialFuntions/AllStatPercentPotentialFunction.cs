using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/All Stat Percent", fileName = "AllStatPercentPotentialFunction")]
public class AllStatPercentPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraAllStats += (int)value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraAllStats -= (int)value;
    }

}
