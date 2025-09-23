using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonChapterSelectUI : UIRoot
{
    [SerializeField] private DungeonEntryDatabase dungeonDatabase = null;
    [SerializeField] private string chapterTaskOBP = string.Empty;
    [SerializeField] private Transform selectArea = null;
    [SerializeField] private Transform originTaskObpTransform = null;
    private CustomVerticalLayoutGroup verticalLayoutGroup = null;
    private List<DungeonChapeterTask> tasks = new List<DungeonChapeterTask>();
    [SerializeField] private DungeonTitleDatabase currentChapter = null;

    #region Events
    public delegate void Excute();
    public delegate void SettingTasksClick(List<DungeonChapeterTask> tasks);

    public event Excute onExcuteBack;
    private event SettingTasksClick onSelectTask;
    public event SettingTasksClick OnSelectTask_
    {
        add
        {
            if (onSelectTask == null || !onSelectTask.GetInvocationList().Contains(value))
                onSelectTask += value;
        }
        remove
        {
            onSelectTask -= value;
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        verticalLayoutGroup = selectArea.GetComponent<CustomVerticalLayoutGroup>();
        gameObject.SetActive(false);
    }

    public void SetCurrentChapter(DungeonTitleDatabase chapter) => currentChapter = chapter;

    public void OpenChapterSelect(DungeonEntryDatabase database)
    {
        //this.gameObject.SetActive(true);
        OpenUIWindow();

        ClearTasks();
        dungeonDatabase = database;
        CreateTasks();
        SetSelectedTaskImage();

        if (verticalLayoutGroup == null) verticalLayoutGroup = selectArea.GetComponent<CustomVerticalLayoutGroup>();
        verticalLayoutGroup.Excute();
    }



    private void CreateTasks()
    {
        for (int i = 0; i < dungeonDatabase.Chapters.Count; i++)
        {
            DungeonChapeterTask task = ObjectPooling.Instance.GetOBP(chapterTaskOBP).GetComponent<DungeonChapeterTask>();
            task.SettingTitle(dungeonDatabase.Chapters[i]);
            task.onSetChapter += SetCurrentChapter;
            task.transform.SetParent(selectArea);
            tasks.Add(task);
        }

        for (int i = 0; i < tasks.Count; i++)
            tasks[i].AddToList(tasks.ToArray());

        onSelectTask?.Invoke(tasks);
    }

    public void GoToBack()
    {
        // this.gameObject.SetActive(false);
        CloseUIWindow();
        onExcuteBack?.Invoke();
    }

    private void ClearTasks()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].GetComponent<ReturnObjectToObjectPooling>().SetOBP();
            tasks[i].transform.SetParent(originTaskObpTransform);
        }
        tasks.Clear();
    }


    private void SetSelectedTaskImage()
    {
       // if (currentChapter == null) currentChapter = dungeonDatabase.Chapters[0];

        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].ChapterDatabase.ChapterName == currentChapter.ChapterName)
                tasks[i].ChangeSelectedUI();
        }
    }
}
