using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Magic Defense Plus", fileName = "MagicDefensePlusPotentialFunction")]
public class MagicDefensePotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraMagicDefense += value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraMagicDefense -= value;
    }

}

