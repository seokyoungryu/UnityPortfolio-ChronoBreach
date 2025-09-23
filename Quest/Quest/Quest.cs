using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum QuestState
{
    INACTIVE =0,
    RUNNING=1,
    CANCLED =2,
    COMPLETE =3,
    WAIT_FOR_COMPLETE =4,
}

public enum QuestListSession
{
    NONE =-1,
    ANY = 0,
    MAIN1 = 1,
    MAIN2 = 2,
    MAIN3 = 3,
    MAIN4 = 4,
    MAIN5 =5,
    MAIN6 =6,
}


[CreateAssetMenu(menuName = "Quest/Quest", fileName = "Quest_")]
public class Quest : BaseScriptableObject
{
    [System.Serializable]
    public class QeustRewardGiver
    {
        public Reward reward;
        public bool recallAfterComplete = false;
    }

    [Header("State")]
    [SerializeField] private QuestState questState = QuestState.INACTIVE;
    [SerializeField] private QuestListSession questSession = QuestListSession.NONE;

    [Header("Category")]
    [SerializeField] private QuestCategory category;

    [Header("Data")]
    [SerializeField] private string codeName = string.Empty;
    [SerializeField] private string displayName = string.Empty;
    [SerializeField, TextArea(0, 3)] private string description = string.Empty;
    [SerializeField] private Sprite questIcon = null;

    [Header("Conditions")]
    [SerializeField] private QuestCondition[] registerConditions;
    [SerializeField] private QuestCondition[] cancelConditions;

    [Header("Options")]
    [SerializeField] private bool autoComplete = false;
    [SerializeField] private bool isCancelable = false;

    [Header("TaskGroup")]
    [SerializeField] private TaskGroup[] taskGroups;

    [Header("Task Complete시 실행할 DialogFile")]
    [SerializeField] private DialogFile taskCompleteDialog;

    [Header("Give Quest Item")]
    [SerializeField] private QeustRewardGiver[] questGive;

    [Header("Reward")]
    [SerializeField] private Reward[] rewards;

    [Header("완료시 다음 시즌으로 변경")]
    [SerializeField] private bool excuteNextSession = false;

    private NPCQuestGiver giver = null;
    private QuestContainer questContainer;
    private int npcID = -1;
    private int questContainerIndex = -1;
    private int questListIndex = -1;

    public QuestContainer QuestContainer => questContainer;
    public NPCQuestGiver Giver => giver;

    public TaskGroup[] TaskGroups => taskGroups;
    public QuestState QuestState => questState;
    public QuestListSession QuestListSession { get { return questSession; } set { questSession = value; } }
    public int RewardCount => rewards.Length;
    public int ItemRewardCount => GetItemRewards().Length;
    public int NpcID => npcID;
    public int QuestContainerIndex => questContainerIndex;
    public int QuestListIndex => questListIndex;

    #region Event
    public delegate void OnRegisterQuest(Quest quest);
    public delegate void OnReceiveReportQuest(Quest quest);
    public delegate void OnCompleteQuest(Quest quest);
    public delegate void OnNewTaskGroup(Quest quest);
    public delegate void OnCancelQuest(Quest quest);
    public delegate bool OnCompleteDialog(QuestContainer questContainer, QuestList questList);

    public event OnCompleteDialog onCompleteDialog;
    public event OnRegisterQuest onRegister;
    private OnReceiveReportQuest onReceiveReport;
    public event OnReceiveReportQuest OnReceiveReport
    {
        add
        {
            if (onReceiveReport == null || !onReceiveReport.GetInvocationList().Contains(value))
                onReceiveReport += value;
        }
        remove
        {
            onReceiveReport -= value;
        }
    }

    public event OnNewTaskGroup onNewTaskGroup;
    public event OnNewTaskGroup onEndTaskGroup;

    private event OnCompleteQuest onComplete; 
    public event OnCompleteQuest OnComplete_
    {
        add
        {
            if (onComplete == null || !onComplete.GetInvocationList().Contains(value))
                onComplete += value;
        }
        remove
        {
            onComplete -= value;
        }
    }
    private event OnCancelQuest onCancel;
    public event OnCancelQuest OnCancel_
    {
        add
        {
            if (onCancel == null || !onCancel.GetInvocationList().Contains(value))
                onCancel += value;
        }
        remove
        {
            onCancel -= value;
        }
    }
    #endregion

    public string DisplayName => displayName;
    public QuestCategory Category => category;
    public DialogFile TaskCompleteDialog { get{ return taskCompleteDialog; }set{ taskCompleteDialog = value; } }
    public string CodeName => codeName;
    public string Description => description;
    public int cancelConditionCount => cancelConditions.Length;
    public int DialogID => questContainer.questDialogId;

    public int repeatCount = 0;
    public int currentTaskGroupIndex = 0;
    public TaskGroup currentTaskGroup => taskGroups[currentTaskGroupIndex];

    public Quest Clone()
    {
        Quest clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new TaskGroup(x)).ToArray();

        clone.registerConditions = registerConditions;
        clone.TaskCompleteDialog = taskCompleteDialog;
        return clone;
    }

    public RewardItem[] GetItemRewards()
    {
        List<RewardItem> retRewardItem = new List<RewardItem>();
        for (int i = 0; i < rewards.Length; i++)
            if (rewards[i] is RewardItem)
                retRewardItem.Add(rewards[i] as RewardItem);
        return retRewardItem.ToArray();
    }

    public void Register(NPCQuestGiver giver, QuestContainer questContain, bool isLoad = false)
    {
        questState = QuestState.RUNNING;
        this.giver = giver;
        questContainer = questContain;
        foreach (TaskGroup taskGroup in taskGroups)
        {
            taskGroup.onCompleteTaskGroup += DeleteItemAfterComplete;
            taskGroup.SetUp(this);
        }

        onRegister?.Invoke(this);
        currentTaskGroup.Start();

        if (!isLoad)
            for (int i = 0; i < questGive.Length; i++)
                questGive[i].reward.Giver(this);

        //여기서 검사하기. 
        for (int i = 0; i < taskGroups.Length; i++)
        {
            for (int j = 0; j < taskGroups[i].Tasks.Length; j++)
            {
                Task task = taskGroups[i].Tasks[j];
                if (!task.ReciveInitGainItemRecive || task.Category.CodeName != "ITEM_GAIN") continue;
                for (int k = 0; k < task.Targets.Length; k++)
                {
                    if (task.Targets[k] is ItemTaskTarget)
                    {
                        Item item = ItemManager.Instance.GenerateItem((int)(task.Targets[k] as ItemTaskTarget).ItemList);
                        int count = CommonUIManager.Instance.playerInventory.GetHaveItemCount(item);
                        Debug.Log("Task : " + task.Category  + ":" + item.itemClip.uiItemName + " Count : " + count);
                        taskGroups[i].ReceiveReport(task.Category, task.Targets[k], count);
                    }
                }

            }
        }
    }




    public void ReceiveReport(QuestCategory category, object target, int successCount)
    {
        if (questState == QuestState.COMPLETE)
            return;
        if (giver == null || questContainer == null)
        {
            giver = QuestManager.Instance.FindNpcController(npcID)?.NpcQuestGiver;
            if (giver != null)
                questContainer = giver.QuestList.questInfos[questListIndex].questContain[questContainerIndex];
            Debug.Log("여기옴 ! Null Quest Giver || Container");
        }

        currentTaskGroup.ReceiveReport(category, target, successCount);

        if (currentTaskGroup.IsAllCompleteTask())
        {
            if (currentTaskGroupIndex == taskGroups.Length - 1)
            {
                questState = QuestState.WAIT_FOR_COMPLETE;
                if (!giver.WaitForCompleteQuests.Contains(questContainer))
                    giver.WaitForCompleteQuests.Add(questContainer);
                if (autoComplete) //auto 완료일경우 NPC의 완료 대화창 실행후 -> Complete하기.
                {
                    if (onCompleteDialog.Invoke(questContainer, giver.QuestList))
                        Complete();
                }  
            }
            else
            {
                onEndTaskGroup?.Invoke(this);
                currentTaskGroup.End();
                currentTaskGroupIndex++;
                onNewTaskGroup?.Invoke(this);
                currentTaskGroup.Start();
            }
        }
        else
            questState = QuestState.RUNNING;

        onReceiveReport?.Invoke(this);
       
    }


    public void Complete()
    {
        if (questGive != null && questGive.Length > 0)
        {
            for (int i = 0; i < questGive.Length; i++)
            {
                if (questGive[i].recallAfterComplete)
                    questGive[i].reward.Remove();
            }
        }


        questState = QuestState.COMPLETE;
        foreach (TaskGroup group in taskGroups)
            group.Complete();

        foreach (Reward reward in rewards)
            reward.Giver(this);

        giver.WaitForCompleteQuests.Remove(questContainer);
        onComplete?.Invoke(this);
        onComplete = null;
        onReceiveReport = null;

        if (excuteNextSession)
        {
            CommonUIManager.Instance.ExcuteGlobalBattleNotifer($"시즌 {(int)(QuestManager.Instance.currentQuestSession + 1)} 시작!",7f);
            QuestManager.Instance.currentQuestSession = QuestManager.Instance.currentQuestSession + 1;
        }
    }

    /// <summary>
    /// 퀘스트 완료후 아이템 삭제용.
    /// </summary>
    public void DeleteItemAfterComplete(Task[] tasks)
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (!tasks[i].DeleteItemAfterComplete) continue;
            for (int x = 0; x < tasks[i].Targets.Length; x++)
            {
                Item item = ItemManager.Instance.GenerateItem((int)tasks[i].Targets[x].Value);
                CommonUIManager.Instance.playerInventory.RemoveItem(item, tasks[i].NeedSuccessCount);
                Debug.Log("삭제 : " + item.itemClip.itemName + " , " + tasks[i].NeedSuccessCount);
            }
        }
    }

    public void CancelQuest()
    {
        SoundManager.Instance.PlayUISound(UISoundType.QUEST_CANCEL);
        QuestManager.Instance.RemoveActiveQuest(this);
        onCancel?.Invoke(this);
    }

    public void UpdateTask()
    {
        for (int i = 0; i < taskGroups[currentTaskGroupIndex].Tasks.Length; i++)
        {
            taskGroups[currentTaskGroupIndex].Tasks[i].onUpdateTask?.Invoke(this, taskGroups[currentTaskGroupIndex].Tasks[i]);
            taskGroups[currentTaskGroupIndex].Tasks[i].CheckIsComplete();
        }
    }

    public bool CheckIsTargetMarker(QuestCategory _category, TaskTarget target)
    {
        if (_category == null || target == null) return false;

        foreach (Task task in currentTaskGroup.Tasks)
        {
            if (task.Category.CompareCategory(_category) && task.IsOnlyTargetCheck(target)
                && task.TaskState == TaskState.RUNNING && (!task.IsComplete || (task.IsComplete && task.ReceiveAfterCompletion)))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 결과가 True일 경우 Register 가능.
    /// </summary>
    public bool CheckRegisterConditions()
    {
        if (registerConditions.Length <= 0) return true;

       bool canRegister = true;
        for (int i = 0; i < registerConditions.Length; i++)
            if (!registerConditions[i].IsPass(this))
                canRegister = false;

        return canRegister;
    }


    /// <summary>
    /// 결과가 True일 경우 Cancel 가능.
    /// </summary>
    public bool CheckCancelConditions()
    {
        bool isCancel = true;
        for (int i = 0; i < cancelConditions.Length; i++)
            if (!cancelConditions[i].IsPass(this))
                isCancel = false;

        return isCancel;
    }

    public bool CheckIsWaitForCompleted()
    {
        if (giver == null) return false;
        if (questState == QuestState.WAIT_FOR_COMPLETE && !giver.WaitForCompleteQuests.Contains(questContainer))
        {
            giver.WaitForCompleteQuests.Add(questContainer);
            Debug.Log(displayName + " Wait For Complete...");

            return true;
        }
        giver.ProgressQuests.Add(questContainer);
        Debug.Log(displayName + " Progress...");

        return false;
    }

    public bool CheckHaveTargetTask(QuestCategory category, TaskTarget target)
    {
        foreach (Task task in currentTaskGroup.Tasks)
        {
            if (task.Category.CodeName != category.CodeName) continue;
            for (int i = 0; i < task.Targets.Length; i++)
                if (task.Targets[i].Value == target.Value)
                    return true;
        }
        return false;
    }

    public QuestSaveData ToSaveData()
    {
        if(giver == null)
        {
            Debug.Log(displayName + " - NUll");
        }

        int[] questInfoIndex = giver.QuestList.FindQuestInfosIndex(questContainer);

        QuestSaveData data = new QuestSaveData()
        {
            codeName = this.codeName,
            state = questState,
            repeatCount = this.repeatCount,
            npcID = giver.GetComponent<NpcController>().ID,
            questListInfoIndex = questInfoIndex[0],
            questContainerIndex = questInfoIndex[1],
            taskGroupIndex = currentTaskGroupIndex,
            session = (int)questSession,
            taskSuccessCounts = currentTaskGroup.Tasks.Select(x => x.CurrentSuccessCount).ToArray()
 
        };
        return data;
    }

    public void LoadData(QuestSaveData saveData)
    {
        questState = saveData.state;
        currentTaskGroupIndex = saveData.taskGroupIndex;
        Debug.Log("Load Quest : " + QuestManager.Instance.NpcControllers.Count);
        npcID = saveData.npcID;
        questContainerIndex = saveData.questContainerIndex;
        questListIndex = saveData.questListInfoIndex;
        questSession = (QuestListSession)saveData.session;
        giver = QuestManager.Instance.FindNpcController(saveData.npcID)?.NpcQuestGiver;
        if (giver != null)
            questContainer = giver?.QuestList.questInfos[saveData.questListInfoIndex].questContain[saveData.questContainerIndex];

        repeatCount = saveData.repeatCount;

        for (int i = 0; i < saveData.taskGroupIndex; i++)
        {
            taskGroups[i].Start();
            taskGroups[i].Complete();
        }

        for (int i = 0; i < saveData.taskSuccessCounts.Length; i++)
        {
            currentTaskGroup.Start();
            currentTaskGroup.Tasks[i].CurrentSuccessCount = saveData.taskSuccessCounts[i];
        }
    }

}
