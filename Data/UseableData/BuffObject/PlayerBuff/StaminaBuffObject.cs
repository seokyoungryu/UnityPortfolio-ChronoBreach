using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/Stamina Buff Object", fileName = "Buff_Stamina")]
public class StaminaBuffObject : BuffStatsObject
{
    public override void Apply(BaseController controller)
    {
        SettingController(controller);
        if (playerController != null)
        {
            PlayerStaminaBuff(true);
            playerController.skillController.RegisterBuff(this);
        }
    }

    public override void RemoveBuff(BaseController controller)
    {
        base.RemoveBuff(controller);
        if (playerController != null)
            PlayerStaminaBuff(false);
    }


    private void PlayerStaminaBuff(bool isPlus)
    {
        PlayerStatus playerStats = playerController.playerStats;
        playerStats.UpdateStats();
        float currentStaminaPercentage = playerStats.CurrentStamina / playerStats.TotalStamina;
        playerStats.ExtraStamina += isPlus ? (int)value : (int)-value;
        playerStats.SetCurrentStamina((int)(playerStats.TotalStamina * currentStaminaPercentage));
    }

}
