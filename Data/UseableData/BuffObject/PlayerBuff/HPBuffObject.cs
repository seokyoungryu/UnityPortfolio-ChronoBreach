using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/HP Buff Object", fileName = "Buff_HP")]
public class HPBuffObject : BuffStatsObject
{
    public override void Apply(BaseController controller)
    {
        SettingController(controller);
        if (isDebuff) return;

        if (playerController != null)
        {
            PlayerHPBuff(true);
            playerController.skillController.RegisterBuff(this);
        }
        else
        {
            AIHPBuff(true);
            aIController.skillController.RegisterBuff(this);
        }
    }

    public override void RemoveBuff(BaseController controller)
    {
        base.RemoveBuff(controller);
        if (playerController != null)
            PlayerHPBuff(false);
        else
            AIHPBuff(false);
    }


    private void PlayerHPBuff(bool isPlus)
    {
        PlayerStatus playerStats = playerController.playerStats;
        playerStats.UpdateStats();
        float currentHpPercentage = playerStats.CurrentHealth / playerStats.TotalHealth;
        playerStats.ExtraHealth += isPlus ? (int)value : (int)-value;
        playerStats.SetCurrentHP((int)(playerStats.TotalHealth * currentHpPercentage));
    }

    private void AIHPBuff(bool isPlus)
    {
        AIStatus aiStats = aIController.aiStatus;
        aiStats.UpdateStats();
        float currentHpPercentage = aiStats.CurrentHealth / aiStats.TotalHealth;
        aIController.aiStatus.ExtraHealth += isPlus ? (int)value : (int)-value;
        aiStats.SetCurrentHP((int)(aiStats.TotalHealth * currentHpPercentage));
        aiStats.UpdateStats();
    }
}
