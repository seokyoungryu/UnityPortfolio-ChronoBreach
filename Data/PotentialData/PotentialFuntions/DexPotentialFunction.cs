using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Dex Plus", fileName = "DexPotentialFunction")]
public class DexPotentialFunction : PotentialFunctionObject
{

    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraDexterity += (int)value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraDexterity -= (int)value;
    }

}
