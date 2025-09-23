using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Buff/HP Duration Heal Object", fileName = "DurationHeal_HP")]
public class HPDurationHealObject : BuffStatsObject
{
    [SerializeField] private float intervalTime = 0f;
    private bool isEndDuration = false;

    public override void Apply(BaseController controller)
    {
        Debug.Log("Duration Heal Start");

        SettingController(controller);
        if (playerController != null && duration > 0)
        {
            playerController.skillController.RegisterBuff(this);
            playerController.skillController.StartCoroutine(DurationProcess());
        }
        else if (aIController != null && duration > 0)
        {
            Debug.Log("Duration Heal");
            aIController.skillController.RegisterBuff(this);
            aIController.skillController.StartCoroutine(DurationProcess());
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
        float currentInterval = intervalTime;
        while (duration > currentTime && !isEndDuration)
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
                else if (aIController != null && isDebuff)
                    SetAIDeBuff(true);
                else if (aIController != null && !isDebuff)
                    SetAIBuff(true);
            }
        
            yield return null;
        }
    }

    protected override void SetPlayerBuff(bool isStart)
    {
        if (isStart)
            playerController.playerStats.AddCurrentHealth((int)value);
    }

    protected override void SetPlayerDeBuff(bool isStart)
    {
        if (isStart)
            playerController.playerStats.UseCurrentHealth((int)value);
    }

    protected override void SetAIBuff(bool isStart)
    {
        Debug.Log("Heal : " + value);
        if (isStart)
            aIController.aiStatus.AddCurrentHealth((int)value);
    }

    protected override void SetAIDeBuff(bool isStart)
    {
        if (isStart)
            aIController.aiStatus.AddCurrentHealth((int)-value);
    }

}