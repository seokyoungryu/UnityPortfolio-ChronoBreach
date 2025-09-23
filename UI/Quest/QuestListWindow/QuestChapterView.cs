using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuestChapterView : MonoBehaviour
{
    [SerializeField] private bool isListActive = true;
    [SerializeField] private QuestListSession questSession = QuestListSession.NONE;
    [SerializeField] private Transform listContainer = null;
    [Header("[0] : Line [1] : Arrow")]
    [SerializeField] private Image[] viewFoldout_Image = null;
    [SerializeField] private TMP_Text chapter_Text = null;
    [SerializeField] private QuestListTask questTask_Prefab = null;
    [SerializeField] private List<QuestListTask> tasks = new List<QuestListTask>();

    [Header("[0] : Up_Line, [1] Up_Arrow, [2] : Down_Line, [3] Down_Arrow, ")]
    [SerializeField] private Sprite[] foldImgs;


    public bool IsListActive { get { return isListActive; } set { isListActive = value; } }
    public QuestListSession QuestSession { get { return questSession; } set { questSession = value; } }
    public IReadOnlyList<QuestListTask> Tasks => tasks;

    #region Events
    public delegate void OnRegister(Quest quest);

    public OnRegister onRegister;
    #endregion


    public void AllDelete()
    {
        foreach (QuestListTask task in tasks.ToArray())
        {
            tasks.Remove(task);
            task.gameObject.SetActive(false);
            Destroy(task.gameObject);
        }
    }

    public void SetTaskActive(bool isActive)
    {
        SetFoldImg(isActive);

        for (int i = 0; i < tasks.Count; i++)
            tasks[i].gameObject.SetActive(isActive);

        ExcuteVerticalLayoutGroup();
    }

    public void SetFoldImg(bool isActive)
    {
        if (isActive)
        {
            viewFoldout_Image[0].sprite = foldImgs[2];
            viewFoldout_Image[1].sprite = foldImgs[3];
        }
        else
        {
            viewFoldout_Image[0].sprite = foldImgs[0];
            viewFoldout_Image[1].sprite = foldImgs[1];
        }
    }

    public void Setting(Quest quest)
    {
        chapter_Text.text = quest.QuestListSession.ToString();
    }

    public void AddTask(Quest quest)     //quest 이벤트에 등록. 옵저버 패턴.
    {
        QuestListTask task = Instantiate(questTask_Prefab, listContainer);
        task.SettingTask(quest);
        UIHelper.AddEventTrigger(task.gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(task); });
        tasks.Add(task);
        ExcuteVerticalLayoutGroup();
    }

    public void RemoveTask(Quest quest)
    {
        if (!CheckExistTask(quest)) return;

        foreach (QuestListTask task in tasks.ToArray())
        {
            if (task.Quest.CodeName == quest.CodeName)
            {
                Debug.Log("RemoveTask2 :" + task.Quest.DisplayName);
                tasks.Remove(task);
                task.gameObject.SetActive(false);
                Destroy(task.gameObject);
            }
        }
    }


    private void SortQuest()
    {

    }


    public void ExcuteVerticalLayoutGroup()
    {
        listContainer.GetComponent<CustomVerticalLayoutGroup>()?.Excute();
        GetComponent<CustomVerticalLayoutGroup>()?.Excute();
    }

    public bool CheckExistTask(Quest quest)
    {
        foreach (QuestListTask task in tasks)
            if (task.Quest.CodeName == quest.CodeName)
                return true;
        return false;
    }

    private void OnPointerClick(QuestListTask task)
    {
        onRegister?.Invoke(task.Quest);
    }
}
