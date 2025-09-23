using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNpcFunction : InteractObject
{
    protected NpcController npcController;
    protected NPCQuestGiver npcQuestGiver = null;

    public DialogFile interacDialog;
    public int currentDialogIndex = 1;
    protected Vector2 rangeDialog = Vector2.zero;
    public Transform interactByTargetTr = null;
   
    public NpcController NpcController => npcController;

    protected virtual void Awake()
    {
        npcController = GetComponent<NpcController>();
        npcQuestGiver = GetComponent<NPCQuestGiver>();
    }



    protected virtual void OnTriggerEnter(Collider other)
    {
        if (QuestManager.Instance.isDialoging) return;

        if (other.CompareTag(canInteractTag))
        {
            if (npcQuestGiver.canGiveQuestCount <= 0 || npcQuestGiver.WaitForCompleteQuests.Count <= 0)
                    CommonUIManager.Instance.InteractUIRegister(this);

            npcController.QuestReporter?.ReceiveReport(QuestCategoryDefines.INTERACT_JUST_ENTRY);
            interactByTargetTr = other.transform;
        }

    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (isExcuteInteractUI || QuestManager.Instance.isDialoging) return;

        if (other.CompareTag(canInteractTag))
        {
            CommonUIManager.Instance.InteractUIRegister(this);
        }

    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (QuestManager.Instance.isDialoging) return;

        if (other.CompareTag(canInteractTag))
        {
            ExitInteract();
            CommonUIManager.Instance.InteractUIRemove(this);
            interactByTargetTr = null;
        
        }

    }

}
