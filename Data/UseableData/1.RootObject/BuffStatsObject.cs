using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffStatsObject : UseableObject
{
    [SerializeField] protected float value = 0f;
    [SerializeField] protected float duration = 0f;
    [SerializeField] protected bool allowDuplication = false;
    [SerializeField] protected bool isDebuff = false;
    [SerializeField, TextArea(0, 2)] protected string description = string.Empty;
    protected PlayerStateController playerController = null;
    protected AIController aIController = null;

    public float Value => value;
    public float Duration => duration;
    public bool AllowDuplication => allowDuplication;
    public bool IsDebuff => isDebuff;
    public string Description => description;

    public override void Apply(BaseController controller)
    {
        SettingController(controller);
        Debug.Log("µî·Ï : " + name);

        if (playerController != null && isDebuff)
            SetPlayerDeBuff(true);
        else if (playerController != null && !IsDebuff)
            SetPlayerBuff(true);
        else if (aIController != null && isDebuff)
            SetAIDeBuff(true);
        else if (aIController != null && !IsDebuff)
           SetAIBuff(true);
    }

    public virtual void RemoveBuff(BaseController controller)
    {
        SettingController(controller);

        if (playerController != null && isDebuff)
            SetPlayerDeBuff(false);
        else if (playerController != null && !IsDebuff)
            SetPlayerBuff(false);
        else if (aIController != null && isDebuff)
            SetAIDeBuff(false);
        else if (aIController != null && !IsDebuff)
            SetAIBuff(false);
    }

    protected void SettingController(BaseController controller)
    {
        playerController = null;
        aIController = null;

        if (controller is PlayerStateController)
            playerController = controller as PlayerStateController;
        else
            aIController = controller as AIController;
    }

    protected virtual void SetPlayerBuff(bool isStart) { }
    protected virtual void SetAIBuff(bool isStart) {}

    protected virtual void SetPlayerDeBuff(bool isStart) {}
    protected virtual void SetAIDeBuff(bool isStart) {  }


}
