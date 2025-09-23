using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalNpcFunction : BaseNpcFunction
{

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }


    public override void ExcuteInteract()
    {
        if (SettingManager.Instance.IsUnInterruptibleUI)
            return;

        base.ExcuteInteract();
        SettingManager.Instance.UseScreenTouch = false;
        SettingManager.Instance.CanExcuteESC = false;

        if (npcQuestGiver?.WaitForCompleteQuests.Count > 0)
            npcController.onCompletedQuest?.Invoke(npcController);
        else if (npcQuestGiver?.canGiveQuestCount > 0)
            npcController.onRegisterSelectQuest?.Invoke(npcController);
        else if (npcQuestGiver?.ProgressQuests.Count > 0)
            npcController.onProgressSelectQuest?.Invoke(npcQuestGiver?.ProgressQuests[0], npcQuestGiver?.QuestList);
        else
        {
            DialogData[] datas = interacDialog.GetHaveDialogDatasState(DialogState.INTERACT);
            int randomIndex = Random.Range(0, datas.Length);
            currentDialogIndex = datas[randomIndex].id;
          //  Debug.Log("인터락트 카운트 : " + randomIndex + "/" + datas.Length );
            npcController.onInteractDialog?.Invoke(interacDialog, currentDialogIndex, DialogState.INTERACT);
        }

        Debug.Log("Interact");
        npcController.AiController.aiConditions.IsInteract = true;

    }

    public override void ExitInteract()
    {
        if (!npcController.AiController.aiConditions.IsInteract)
            return;

        base.ExitInteract();
        Debug.Log("NPC Exit");
        StopAllCoroutines();
        npcController.onExitNpc?.Invoke();
        SettingManager.Instance.IsUnInterruptibleUI = false;
        npcController.AiController.aiConditions.IsInteract = false;
       
    }




}
