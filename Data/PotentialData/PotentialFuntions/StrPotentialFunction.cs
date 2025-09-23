using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Str Plus" , fileName = "StrPotentialFunction")]
public class StrPotentialFunction : PotentialFunctionObject
{

    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraStrength += (int)value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraStrength -= (int)value;
    }

}
