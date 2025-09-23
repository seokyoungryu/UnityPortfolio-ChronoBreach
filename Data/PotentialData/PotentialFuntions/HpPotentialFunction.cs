using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potential/Max Hp Plus", fileName = "MaxHpPlusPotentialFunction")]
public class HpPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraHealth += (int)value;
        playerStatus.SetCurrentHP(playerStatus.CurrentHealth + (int)value);

    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.ExtraHealth -= (int)value;
        playerStatus.SetCurrentHP(playerStatus.CurrentHealth - (int)value);
    }

}
