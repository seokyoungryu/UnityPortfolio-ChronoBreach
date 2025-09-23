using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSelectionPresenter : MonoBehaviour
{
    [SerializeField] private QuestSelectionViewer questSelectionViewer;
    private List<NpcController> npcControllers = null;
    [SerializeField] private DialogUI dialogUI = null;
    [SerializeField] private DialogProcess dialogProcess = null;

    public QuestSelectionViewer QuestSelectionViewer => questSelectionViewer;

    private void Awake()
    {
        questSelectionViewer.onStartSelectedTask += dialogUI.InitStart;
        questSelectionViewer.onEndSelectedTask += dialogUI.InitEnd;
        questSelectionViewer.onProgressSelectedTask += dialogUI.InitProgress;
        questSelectionViewer.onInteractSelectedTask += dialogUI.InitInteractDialog;
        questSelectionViewer.onForceRegisterQuest += dialogUI.InitForceRegister;

        dialogProcess.onCompleteQuest += dialogUI.CompleteQuest;
        dialogUI.onStartDialog += dialogProcess.StartDialog;
        dialogUI.onEndDialog += dialogProcess.EndDialog;
        dialogUI.onProgressDialog += dialogProcess.ProgressDialog;
        dialogUI.onInteractDialog += dialogProcess.InteractDialog;
        dialogUI.onClickDialog += dialogProcess.PointerClick;
        dialogUI.onAcceptDialog += AcceptDialog;
        dialogUI.onCompleteDialog += CompleteAcceptDialog;
        dialogUI.onDialogProcessInit += dialogProcess.ResetData;
        QuestManager.Instance.onSetNewNpcContrCallback += SetNpcController;
        CommonUIManager.Instance.onExcuteDialog += questSelectionViewer.OnInteractDialogProcess;
      
    }

    private void OnDestroy()
    {
        questSelectionViewer.onStartSelectedTask -= dialogUI.InitStart;
        questSelectionViewer.onEndSelectedTask -= dialogUI.InitEnd;
        questSelectionViewer.onProgressSelectedTask -= dialogUI.InitProgress;
        questSelectionViewer.onInteractSelectedTask -= dialogUI.InitInteractDialog;
        questSelectionViewer.onForceRegisterQuest -= dialogUI.InitForceRegister;

        dialogProcess.onCompleteQuest -= dialogUI.CompleteQuest;
        dialogUI.onStartDialog -= dialogProcess.StartDialog;
        dialogUI.onEndDialog -= dialogProcess.EndDialog;
        dialogUI.onProgressDialog -= dialogProcess.ProgressDialog;
        dialogUI.onInteractDialog -= dialogProcess.InteractDialog;
        dialogUI.onClickDialog -= dialogProcess.PointerClick;
        dialogUI.onAcceptDialog -= AcceptDialog;
        dialogUI.onCompleteDialog -= CompleteAcceptDialog;
        dialogUI.onDialogProcessInit -= dialogProcess.ResetData;
        QuestManager.Instance.onSetNewNpcContrCallback -= SetNpcController;
        CommonUIManager.Instance.onExcuteDialog -= questSelectionViewer.OnInteractDialogProcess;

        for (int i = 0; i < QuestManager.Instance.NpcControllers.Count; i++)
        {
            RemoveNpcController(QuestManager.Instance.NpcControllers[i]);
        }
    }

    private void Start()
    {
        for (int i = 0; i < QuestManager.Instance.NpcControllers.Count; i++)
        {
            SetNpcController(QuestManager.Instance.NpcControllers[i]);
        }
    }




    public void SetNpcController(NpcController contr)
    {
        contr.onRegisterSelectQuest += questSelectionViewer.InitRegisterViewer;
        contr.onCompletedQuest += questSelectionViewer.InitCompleteViewer;
        contr.onProgressSelectQuest += questSelectionViewer.OnProgessTask;
        contr.onInteractDialog += questSelectionViewer.OnInteractDialogProcess;
        contr.onRegisterAutoQuest += questSelectionViewer.OnForceRegisterQuestContainer;

        contr.onExitNpc += dialogUI.DisableUI;
        contr.onExitNpc += questSelectionViewer.DisableUI;
    }

    public void RemoveNpcController(NpcController contr)
    {
        contr.onRegisterSelectQuest -= questSelectionViewer.InitRegisterViewer;
        contr.onCompletedQuest -= questSelectionViewer.InitCompleteViewer;
        contr.onProgressSelectQuest -= questSelectionViewer.OnProgessTask;
        contr.onInteractDialog -= questSelectionViewer.OnInteractDialogProcess;
        contr.onRegisterAutoQuest -= questSelectionViewer.OnForceRegisterQuestContainer;

        contr.onExitNpc -= dialogUI.DisableUI;
        contr.onExitNpc -= questSelectionViewer.DisableUI;
    }


    public void AcceptDialog(DialogUI dialogUI)
    {
        npcControllers = QuestManager.Instance.NpcControllers;
        Debug.Log("AcceptDialog ½ÇÇà ");

        for (int i = 0; i < npcControllers.Count; i++)
        {
            if(dialogUI.CurrentQuestContainer.npcID == npcControllers[i].ID)
            {
               // Debug.Log("AcceptDialog Find Npc");
               // Debug.Log("AcceptDialog Find Npc : " + dialogUI + " ," + dialogUI.CurrentQuestContainer + " , " + dialogUI.CurrentQuestContainer.quest);
               // Debug.Log("AcceptDialog Find Npc : " + npcControllers[i].NpcQuestGiver);

                QuestManager.Instance.Register(dialogUI.CurrentQuestContainer.quest, npcControllers[i].NpcQuestGiver, dialogUI.CurrentQuestContainer);
                npcControllers[i].NpcQuestGiver.SetProgressList(dialogUI.CurrentQuestContainer);
            }
        }
    }

    public void CompleteAcceptDialog(DialogUI dialogUI)
    {
        Quest quest = QuestManager.Instance.FindInActiveQuest(dialogUI.CurrentQuestContainer.quest);
        quest?.Complete();

        npcControllers = QuestManager.Instance.NpcControllers;
        for (int i = 0; i < npcControllers.Count; i++)
            if (dialogUI.CurrentQuestContainer.npcID == npcControllers[i].ID)
                npcControllers[i].NpcQuestGiver.RemoveProgressList(dialogUI.CurrentQuestContainer);

    }
}
