using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/Stamina Duration Heal Buff Object", fileName = "Buff_StaminaDurationHeal")]
public class StaminaDurationHealObject : BuffStatsObject
{
    [SerializeField] private float intervalTime = 0f;
    private bool isEndDuration = false;

    public override void Apply(BaseController controller)
    {
        SettingController(controller);
        if (playerController != null && duration > 0)
        {
            playerController.skillController.RegisterBuff(this);
            playerController.StartCoroutine(DurationProcess());
        }
    }

    public override void RemoveBuff(BaseController controller)
    {
        base.RemoveBuff(controller);
        isEndDuration = true;
    }


    private IEnumerator DurationProcess()
    {
        float currentTime = 0f;
        float currentInterval = 0f;

        while (duration > currentTime || !isEndDuration)
        {
            currentTime += Time.deltaTime;
            currentInterval += Time.deltaTime;
            if (currentInterval >= intervalTime)
            {
                currentInterval = 0f;
                if (playerController != null && isDebuff)
                    SetPlayerDeBuff(true);
                else if (playerController != null && !isDebuff)
                    SetPlayerBuff(true);
            }
            yield return null;
        }
    }

    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
            playerController.playerStats.AddCurrentStamina((int)value);
    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
            playerController.playerStats.UseCurrentStamina((int)value);
    }

}
