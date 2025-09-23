    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class InteractObject : MonoBehaviour
{
    [SerializeField] protected bool autoExcute = false;
    [SerializeField] protected string canInteractTag = TagAndLayerDefine.Tags.Player;
    [SerializeField] protected string interactDescription = string.Empty;
    [SerializeField] protected SoundList interactSound = SoundList.None;
    protected bool canInteract = true;
    protected bool isExcuteInteractUI = false;

    public string InteractDescription => interactDescription;
    public bool AutoExcute => autoExcute;
    public bool IsExcuteInteractUI { get { return isExcuteInteractUI; } set { isExcuteInteractUI = value; } }

    /// <summary>
    /// Interact시 동작할 작업
    /// </summary>
    public virtual void ExcuteInteract()
    {

        if (!canInteract || isExcuteInteractUI) return;
        isExcuteInteractUI = true;

    }

    public virtual void ExitInteract()
    {
        if (IsExcuteInteractUI)
            QuestManager.Instance.isDialoging = false;

        isExcuteInteractUI = false;
    }

    public virtual void UIDescripSetting()
    {
        if (!canInteract)
        {
            CommonUIManager.Instance.InteractUIRemove(this);
        }

        CommonUIManager.Instance.InteractUISettingDescript(interactDescription);
    }

}
