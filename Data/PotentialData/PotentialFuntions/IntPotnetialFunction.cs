using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Int Plus", fileName = "IntPotentialFunction")]
public class IntPotnetialFunction : PotentialFunctionObject
{

    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraIntelligence += (int)value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraIntelligence -= (int)value;
    }

}