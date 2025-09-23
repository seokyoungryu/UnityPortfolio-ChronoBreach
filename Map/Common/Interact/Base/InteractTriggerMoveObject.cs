using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractTriggerObjectType
{
    NONE = -1,
    OPEN = 0,
    CLOSE = 1,
    OPEN_CLOSE = 2,

}

public class InteractTriggerMoveObject : InteractObject
{
    [SerializeField] private InteractTriggerObjectType interactTriggerType;
    [SerializeField] protected InteractTriggerObjectType currentInteractType = InteractTriggerObjectType.NONE;
    [SerializeField] protected List<InteractTriggerMoveInfo> infos = new List<InteractTriggerMoveInfo>();
    protected Transform interactObject = null;

    public override void ExcuteInteract()
    {
        base.ExcuteInteract();
        if (currentInteractType == InteractTriggerObjectType.OPEN)
            Close();
        else if (currentInteractType == InteractTriggerObjectType.CLOSE)
            Open();
        else if(interactTriggerType == InteractTriggerObjectType.OPEN_CLOSE)
        {
            if (currentInteractType == InteractTriggerObjectType.OPEN)
                Close();
            else if (currentInteractType == InteractTriggerObjectType.CLOSE)
                Open();
        }

        if (interactTriggerType == InteractTriggerObjectType.OPEN_CLOSE)
            UIDescripSetting();
        if (!canInteract)
            CommonUIManager.Instance.InteractUIRemove(this);
    }

    public override void UIDescripSetting()
    {
        if (!canInteract)
        {
            CommonUIManager.Instance.InteractUIRemove(this);
            return;
        }

        CommonUIManager.Instance.InteractUISettingDescript(GetCurrentDescript(currentInteractType));
    }


    private string GetCurrentDescript(InteractTriggerObjectType currType)
    {
        for (int i = 0; i < infos.Count; i++)
            if (infos[i].CurrentInteractType == currType)
                return infos[i].Descript;

        return interactDescription;
    }

    protected virtual void Open()
    {
        currentInteractType = InteractTriggerObjectType.OPEN;
        if (interactTriggerType == InteractTriggerObjectType.OPEN)
            canInteract = false;

        SoundManager.Instance.PlayEffect(GetInfo(InteractTriggerObjectType.OPEN).MoveSound);
        
    }

    protected virtual void Close()
    {
        currentInteractType = InteractTriggerObjectType.CLOSE; 
        if (interactTriggerType == InteractTriggerObjectType.CLOSE)
            canInteract = false;

        SoundManager.Instance.PlayEffect(GetInfo(InteractTriggerObjectType.CLOSE).MoveSound);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(canInteractTag))
        {
            if (!canInteract) return;

            interactObject = other.transform;
            Debug.Log("트리거 엔터 : " + other.gameObject.tag);
            CommonUIManager.Instance.InteractUIRegister(this);
        }

        
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(canInteractTag))
        {
            Debug.Log("트리거 익시트");
            interactObject = null;
            CommonUIManager.Instance.InteractUIRemove(this);
        }

    }

    protected InteractTriggerMoveInfo GetInfo(InteractTriggerObjectType type)
    {
        for (int i = 0; i < infos.Count; i++)
            if (infos[i].CurrentInteractType == type)
                return infos[i];

        return null;
    }

}

[System.Serializable]
public class InteractTriggerMoveInfo
{
    [SerializeField] private InteractTriggerObjectType currentInteractType;
    [SerializeField] private string descript = string.Empty;
    [SerializeField] private SoundList moveSound;

    public InteractTriggerObjectType CurrentInteractType => currentInteractType;
    public string Descript => descript;
    public SoundList MoveSound => moveSound;
}
