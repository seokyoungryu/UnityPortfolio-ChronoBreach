using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/HP Heal Object", fileName = "ImmediateHeal_HP")]
public class HPImmediateHealObject : BuffStatsObject
{
    public override void Apply(BaseController controller)
    {
        Debug.Log("HPImmediateHealObject1 실행");

        SettingController(controller);
        if (isDebuff) return;

        if (playerController != null)
            playerController.playerStats.AddCurrentHealth((int)value);
        else if (aIController != null)
        {
            Debug.Log("HPImmediateHealObject2 실행");
            aIController.aiStatus.AddCurrentHealth((int)value);
        }

    }

}