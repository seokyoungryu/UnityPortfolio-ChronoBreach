using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Potential/Critical Damage", fileName = "CriticalDamagePotentialFunction")]
public class CriticalDamagePotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        float retValue = value * 0.01f; 
        playerStatus.ExtraCriticalDmg += retValue;
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        float retValue = value * 0.01f;
        playerStatus.ExtraCriticalDmg -= retValue;
    }

}

