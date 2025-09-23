using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TaskState
{    
    INACTIVE =0,
    RUNNING =1,
    COMPLETE = 2,
}

[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    public TaskState taskState = TaskState.INACTIVE;

    public Quest Owner { get; private set; }
    public TaskGroup parentTaskGroup { get; private set; }


    [Header("Category")]
    [SerializeField] private QuestCategory category;

    [Header("Text")]
    [SerializeField] private string codeName;
    [TextArea(1,5)]
    [SerializeField] private string description;

    [Header("Action")]
    [SerializeField] private QuestAction action;

    [Header("Target")]
    [SerializeField] private TaskTarget[] targets;

    [Header("Settings")]
    [SerializeField] private int needSuccessCount = 0;
    [SerializeField] private int currentSuccessCount = 0;
    [SerializeField] private bool receiveAfterCompletion = false;   //완료후에도 Receive 받을지.
    [SerializeField] private bool isLimitInput;     //최대 input count 제한.
    [SerializeField] private bool deleteItemAfterComplete = false;   //ItemTarget일 경우만 해당.
    [SerializeField] private bool reciveInitGainItemRecive = false;   

    public bool DeleteItemAfterComplete => deleteItemAfterComplete;
    public bool IsComplete => taskState == TaskState.COMPLETE;
    public bool ReciveInitGainItemRecive => reciveInitGainItemRecive;

    public bool ReceiveAfterCompletion => receiveAfterCompletion;
    public int NeedSuccessCount => needSuccessCount;
    public int CurrentSuccessCount
    {
        get
        { return currentSuccessCount; }
        set
        { currentSuccessCount = value; }
    }
    public string CodeName => codeName;
    public string Description => description;

    public TaskState TaskState => taskState;

    public QuestCategory Category => category;
    public TaskTarget[] Targets => targets;

    #region Event
    public delegate void OnCompleted(Quest quest, Task task);
    public delegate void OnReceiveReported(Quest quest, Task task);

    private OnCompleted onComplete;
    public event OnCompleted OnComplete
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

    private OnReceiveReported onReceive;
    public event OnReceiveReported OnReceiveReport
    {
        add
        {
            if (onReceive == null || !onReceive.GetInvocationList().Contains(value))
                onReceive += value;
        }
        remove
        {
            onReceive -= value;
        }

    }

    public OnReceiveReported onUpdateTask;
    #endregion


    public void SetUp(Quest quest)
    {
        Owner = quest;
    }
    public void SetUp(Quest quest, TaskGroup taskGroup)
    {
        Owner = quest;
        parentTaskGroup = taskGroup;
    }
    public void Start()
    {
        taskState = TaskState.RUNNING;
        onReceive?.Invoke(Owner, this); //
    }


    /// <summary>
    /// 종료
    /// </summary>
    public void End()
    {
        taskState = TaskState.COMPLETE;
        Debug.Log("여기기4");
    }


    /// <summary>
    /// 완료
    /// </summary>
    public void Complete()
    {
        currentSuccessCount = needSuccessCount;
        taskState = TaskState.COMPLETE;
        onComplete?.Invoke(Owner, this);
        Debug.Log("여기기3");
    }

    public bool CheckIsComplete()
    {
        if (currentSuccessCount >= needSuccessCount)
        {
            currentSuccessCount = receiveAfterCompletion ? currentSuccessCount : currentSuccessCount = needSuccessCount;
            taskState = TaskState.COMPLETE;
            onComplete?.Invoke(Owner, this);
            Debug.Log("여기기2");
            return true;
        }
        return false;
    }


    public void ReceiveReport(int input)
    {
        Debug.Log("테스크 ReceiveReport0");
        input = isLimitInput ? Mathf.Clamp(input, 0, needSuccessCount) : input;

        currentSuccessCount = action.Action(this, currentSuccessCount, input);
        onReceive?.Invoke(Owner, this);

        if (currentSuccessCount >= needSuccessCount)
        {
            currentSuccessCount = receiveAfterCompletion ? currentSuccessCount : currentSuccessCount = needSuccessCount ;
            taskState = TaskState.COMPLETE;
            onComplete?.Invoke(Owner,this);
            if (Owner.TaskCompleteDialog == null || parentTaskGroup.dialogID <= 0)
            {
                Debug.Log("테스크 ReceiveReport1");
                return;
            }

            Debug.Log("테스크 ReceiveReport2");
            CommonUIManager.Instance.ExcuteDialog(Owner.TaskCompleteDialog, parentTaskGroup.dialogID, DialogState.ETC);

        }
        else
            taskState = TaskState.RUNNING;

    }


    public bool IsTarget(QuestCategory category, object target)
    {
        bool isTarget = false;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].IsEqual(target))
                isTarget = true;
        }

        if (this.category.CompareCategory(category))
            Debug.Log("Compare CompareCategory");
        if (isTarget)
            Debug.Log("Compare IsEqual");
        if ((!IsComplete || IsComplete && receiveAfterCompletion))
            Debug.Log("Compare !IsComplete");



        if (this.category.CompareCategory(category) && isTarget && (!IsComplete || IsComplete && receiveAfterCompletion))
        {
            return true;
        }
        return false;
    }


    public bool IsOnlyTargetCheck(TaskTarget InputTarget)
    {
        foreach (TaskTarget target in targets)
        {
            if (target == InputTarget)
                return true;
        }
        return false;
    }

}
