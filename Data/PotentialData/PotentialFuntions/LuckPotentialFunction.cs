using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Luck Plus", fileName = "LuckPotentialFunction")]
public class LuckPotentialFunction : PotentialFunctionObject
{

    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraLuck += (int)value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraLuck -= (int)value;
    }

}
