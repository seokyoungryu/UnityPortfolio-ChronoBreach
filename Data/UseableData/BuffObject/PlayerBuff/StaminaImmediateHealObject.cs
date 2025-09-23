using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/Stamina Immediate Heal Buff Object", fileName = "Buff_StaminaImmediateHeal")]
public class StaminaImmediateHealObject : BuffStatsObject
{
    public override void Apply(BaseController controller)
    {
        SettingController(controller);
        if (playerController != null)
            playerController.playerStats.AddCurrentStamina((int)value);
    }
}
