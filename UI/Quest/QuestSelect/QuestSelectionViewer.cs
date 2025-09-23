using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestSelectionViewer : UIRoot
{
    [SerializeField] private QuestSelectTask questSelectTask_Prefab = null;
    [SerializeField] private Transform panel = null;
    [SerializeField] private Transform selectsList_Tr = null;
    [SerializeField] private QuestContainer selectedQuest = null; // hide
    [SerializeField] private QuestList questList = null;          //hide insperter        
    [SerializeField] private int npcId = -1;

    public int NpcID => npcId;
    public QuestList QuestList => questList;
    public QuestContainer SelectedQuest => selectedQuest;
    #region Events
    public delegate void OnSelectedTask(QuestSelectionViewer questSelectionViewer);
    public delegate void OnForceRegister(QuestSelectionViewer questSelectionViewer, QuestContainer questContainer);
    public delegate void OnInteractDialogTask(DialogFile dialogFile, int dialogIndex ,DialogState state);

    public event OnSelectedTask onStartSelectedTask;
    public event OnSelectedTask onEndSelectedTask;
    public event OnSelectedTask onProgressSelectedTask;
    public event OnForceRegister onForceRegisterQuest;
    public event OnInteractDialogTask onInteractSelectedTask;

    #endregion


    public void InitRegisterViewer(NpcController npcController)
    {
        if (npcController.NpcQuestGiver.canGiveQuestCount <= 0) return;

        OpenUIWindow();

        SettingTask(npcController);
        foreach (QuestContainer questContainer in npcController.NpcQuestGiver.CanGiveQuests)
        {
            QuestSelectTask task = Instantiate(questSelectTask_Prefab, selectsList_Tr);
            task.Setting(questContainer, npcId);
            UIHelper.AddEventTrigger(task.gameObject, EventTriggerType.PointerClick, delegate { OnStartTaskPointerClick(task); });
        }
        selectsList_Tr.GetComponent<CustomVerticalLayoutGroup>()?.Excute();
    }

    public void InitCompleteViewer(NpcController npcController)
    {
        if (npcController.NpcQuestGiver.WaitForCompleteQuests.Count <= 0) return;
        OpenUIWindow();
        SettingTask(npcController);
        foreach (QuestContainer questContainer in npcController.NpcQuestGiver.WaitForCompleteQuests)
        {
            QuestSelectTask task = Instantiate(questSelectTask_Prefab, selectsList_Tr);
            task.Setting(questContainer, npcId);
            UIHelper.AddEventTrigger(task.gameObject, EventTriggerType.PointerClick, delegate { OnEndTaskPointerClick(task); });
        }
        Debug.Log("InitCompleteViewer !! ");
        selectsList_Tr.GetComponent<CustomVerticalLayoutGroup>()?.Excute();
    }


    private void SettingTask(NpcController npcController)
    {
        foreach (Transform child in selectsList_Tr)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        npcId = npcController.ID;
        questList = npcController.NpcQuestGiver.QuestList;
        panel.gameObject.SetActive(true);
    }


    public void DisableUI()
    {
        panel.gameObject.SetActive(false);
        CloseUIWindow();
    }


    public void OnRegisterQuestContainer(NpcController npcController ,QuestContainer questContainer)
    {
        CommonRegister(npcController, questContainer);
        panel.gameObject.SetActive(false);
        onStartSelectedTask?.Invoke(this);
    }

    public void OnForceRegisterQuestContainer(NpcController npcController, QuestContainer questContainer)
    {
        CommonRegister(npcController, questContainer);
        panel.gameObject.SetActive(false);
        onForceRegisterQuest?.Invoke(this, questContainer);
    }

    private void CommonRegister(NpcController npcController, QuestContainer questContainer)
    {
        selectedQuest = questContainer;
        npcId = npcController.ID;
        questList = npcController.NpcQuestGiver.QuestList;
    }

    private void OnStartTaskPointerClick(QuestSelectTask task)
    {
        selectedQuest = task.GetQuestContainter;
        panel.gameObject.SetActive(false);
        onStartSelectedTask?.Invoke(this);
    }


    private void OnEndTaskPointerClick(QuestSelectTask task)
    {
        if(task.GetQuest.ItemRewardCount > 0 &&!CommonUIManager.Instance.playerInventory.CheckCanAddItems(task.GetQuest.GetItemRewards()))
        {
            panel.gameObject.SetActive(false);
            return;
        }
        Debug.Log("OnEndTaskPointerClick : " + task);

        //보상 검사 후 보상 가능하다면 밑에 프로세스 실행.
        selectedQuest = task.GetQuestContainter;
        panel.gameObject.SetActive(false);
        onEndSelectedTask?.Invoke(this);
    }

    public bool OnEndQuest(QuestContainer questContainer, QuestList questList)
    {
        if (questContainer.quest.ItemRewardCount > 0 && !CommonUIManager.Instance.playerInventory.CheckCanAddItems(questContainer.quest.GetItemRewards()))
        {
            panel.gameObject.SetActive(false);
            return false;
        }
        OpenUIWindow();


        //보상 검사 후 보상 가능하다면 밑에 프로세스 실행.
        selectedQuest = questContainer;
        this.questList = questList;
        Debug.Log("OnEndQuest : " + questContainer + " , " + questContainer.quest + " , " + questList);
      //  Debug.Log("OnEndQuest selectedQuest: " + selectedQuest + " , " + this.questList);

        panel.gameObject.SetActive(false);
        onEndSelectedTask?.Invoke(this);
        return true;
    }

    public void OnProgessTask(QuestContainer quest, QuestList questList)
    {
        OpenUIWindow();
        selectedQuest = quest;
        this.questList = questList;
        panel.gameObject.SetActive(false);
        onProgressSelectedTask?.Invoke(this);
    }

    public void OnInteractDialogProcess(DialogFile file, int dialogIndex, DialogState state)
    {
        OpenUIWindow();
        panel.gameObject.SetActive(false);
        onInteractSelectedTask?.Invoke(file, dialogIndex, state);
    }
}
