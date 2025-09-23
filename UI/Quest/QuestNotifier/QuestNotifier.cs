using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestNotifier : MonoBehaviour
{
    //현재 active된 퀘스트목록을 검사해서 ? ㄴㄴ  -> 퀘스트가 active될때
    //타겟 자식에 이 스크립트에 있는 카테고리와 , 타겟이 존재하면 타겟마커 온
    public Quest targetQuest;

    public TextMeshProUGUI questTitleText = null;
    public TaskDescription taskDescriptionPrefab = null;
    public Color waitForCompleteTextColor;
    private CustomVerticalLayoutGroup[] verticalLayoutGroups;
    private int currentTaskGroupIndex = 0;
    [SerializeField] private List<ColorQuestCategory> colorCategory = new List<ColorQuestCategory>();

    private Dictionary<Task, TaskDescription> taskByDescription = new Dictionary<Task, TaskDescription>();

    public void SetUp(Quest quest, Color color)
    {
        SettingData(quest, color);
        UpdateNotifier(quest);
    }

    public void SetUpLoadQuest(Quest quest, Color color)
    {
        SettingData(quest, color);
        UpdateLoadNotifier(quest);
    }

    private void SettingData(Quest quest, Color color)
    {
        targetQuest = quest;
        questTitleText.text = quest.Category == null ? $"[{GetColorQuestCategory(quest.Category, quest.DisplayName)}]" : $"[{GetColorQuestCategory(quest.Category, quest.Category.DisplayName)}] <color=white>{quest.DisplayName}</color>";
        questTitleText.color = color;
        verticalLayoutGroups = GetComponentsInParent<CustomVerticalLayoutGroup>();

        quest.onNewTaskGroup += UpdateNotifier;
        quest.OnComplete_ += SelfDestroy;
        quest.OnCancel_ += SelfDestroy;
        quest.OnCancel_ += verticalLayoutGroups[1].QuestCompleteProcess;
        quest.OnComplete_ += verticalLayoutGroups[1].QuestCompleteProcess;
    }


    public string GetColorQuestCategory(QuestCategory category, string text)
    {
        for (int i = 0; i < colorCategory.Count; i++)
        {
            if (colorCategory[i].questCategory.CompareCategory(category))
            {
                return $"<color=#{ColorUtility.ToHtmlStringRGBA(colorCategory[i].color)}>{text}</color>";
            }
        }
        return text;
    }

    public void UpdateNotifier(Quest quest)
    {
        foreach (Task task in quest.currentTaskGroup.Tasks)
        {
            TaskDescription taskDescription = Instantiate(taskDescriptionPrefab, transform);
            taskDescription.UpdateText(task);
            task.onUpdateTask += UpdateText;
            task.OnReceiveReport += UpdateText;
            task.OnComplete += UpdateText;
            taskByDescription.Add(task, taskDescription);
        }
        for (int i = 0; i < verticalLayoutGroups.Length; i++)
            verticalLayoutGroups[i]?.Excute();
    }

    public void UpdateLoadNotifier(Quest quest)
    {
        for (int i = 0; i <= quest.currentTaskGroupIndex; i++)
        {
            foreach (Task task in quest.TaskGroups[i].Tasks)
            {
                TaskDescription taskDescription = Instantiate(taskDescriptionPrefab, transform);
                taskDescription.UpdateText(task);
                task.onUpdateTask += UpdateText;
                task.OnReceiveReport += UpdateText;
                task.OnComplete += UpdateText;
                taskByDescription.Add(task, taskDescription);
            }
        }
        for (int i = 0; i < verticalLayoutGroups.Length; i++)
            verticalLayoutGroups[i]?.Excute();
    }




    public void UpdateText(Quest quest, Task task)
    {
        if (quest.QuestState == QuestState.WAIT_FOR_COMPLETE)
            questTitleText.color = waitForCompleteTextColor;

        taskByDescription[task].UpdateText(task);
    }

    public void SelfDestroy(Quest quest)
    {
        gameObject.SetActive(false);
        Destroy(gameObject);

        // for (int i = 0; i < verticalLayoutGroups.Length; i++)
        //     verticalLayoutGroups[i].Do();
    }

}


[System.Serializable]
public class ColorQuestCategory
{
    public QuestCategory questCategory;
    public Color color;
}