using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Potential/Critical Chance", fileName = "CriticalChancePotentialFunction")]
public class CriticalPercentPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraCriticalChance += (int)value;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraCriticalChance -= (int)value;
    }

}
