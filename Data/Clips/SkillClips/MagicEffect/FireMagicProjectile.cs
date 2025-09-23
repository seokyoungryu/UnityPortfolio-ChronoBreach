using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMagicProjectile : BaseMagicProjectile
{
    public override void FunctionPerDamage()
    {
        Debug.Log("데미지 전 호출!");

    }
}
