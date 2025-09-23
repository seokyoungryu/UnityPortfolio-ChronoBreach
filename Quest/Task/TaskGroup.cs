using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TaskGroupState
{
    INACTIVE =0,
    RUNNING =1,
    COMPLETE =2,
}

[System.Serializable]
public class TaskGroup 
{
    public Quest Owner { get; private set; }
    public TaskGroupState state = TaskGroupState.INACTIVE;
    private bool isDeleteItem = false;
    public int dialogID = -1;
    [SerializeField] private Task[] tasks;

    public Task[] Tasks => tasks;

    #region Events
    public delegate void OnCompleteTaskGroup(Task[] tasks);
    public event OnCompleteTaskGroup onCompleteTaskGroup;
    #endregion


    public TaskGroup(TaskGroup copyTaskGroup)
    {
        dialogID = copyTaskGroup.dialogID;
        tasks = copyTaskGroup.Tasks.Select(x=> Object.Instantiate(x)).ToArray();
    }                                                               

    public bool IsAllCompleteTask()
    {
        bool AllComplete = true; 
        foreach (Task task in tasks)
        {
            if (!task.IsComplete)
                AllComplete = false;
        }

        return AllComplete;
    }

    public bool CheckIsTarget(QuestCategory category, TaskTarget target)
    {
        foreach (Task task in tasks)
        {
            if (task.IsTarget(category, target))
                return true;
        }
        return false;
    }

    public bool CheckIsHaveCategory(QuestCategory category)
    {
        return tasks.Any(x => x.Category.CompareCategory(category));
    }

    public void SetUp(Quest quest)
    {
        Owner = quest;

        foreach (Task task in tasks)
            task.SetUp(quest, this);
    }


    public void Start()
    {
        SoundManager.Instance.PlayUISound(UISoundType.QUEST_TASKREGISER);

        state = TaskGroupState.RUNNING;
        foreach (Task task in tasks)
            task.Start();
    }

    public void End()
    {
        if (!isDeleteItem)
        {
            isDeleteItem = true;
            onCompleteTaskGroup?.Invoke(tasks);
        }

        state = TaskGroupState.COMPLETE;
        foreach (Task task in tasks)
            task.End();

       
    }

    public void ReceiveReport(QuestCategory category, object target,int successCount)
    {
       Debug.Log("Enter Recive : " + category + ":" + category.CodeName + " : " + target + " : " + successCount);
        foreach (Task task in tasks)
        {
            if (task.IsTarget(category, target))
            {
                Debug.Log("Task Recive True");
                task.ReceiveReport(successCount);
            }
            else
                Debug.Log("Task Recive fasle");
        }
    }

    public void Complete()
    {
        if (!isDeleteItem)
        {
            isDeleteItem = true;
            onCompleteTaskGroup?.Invoke(tasks);
        }
        state = TaskGroupState.COMPLETE;
        foreach (Task task in tasks)
            task.Complete();

        SoundManager.Instance.PlayUISound(UISoundType.QUEST_TASKCOMPLETE);
        Debug.Log("테스크의의 1?");
    }

}
