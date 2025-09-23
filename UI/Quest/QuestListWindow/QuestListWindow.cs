using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.EventSystems;

public enum QuestListWindowType
{
    ACTIVE = 0,
    COMPLETE = 1,
    REPEAT =2,
}

public class QuestListWindow : UIRoot
{
    [Header("[0] : Active , [1] : Complete , [2] : Repeat")]
    [SerializeField] private GameObject[] categorys = null;
    [SerializeField] private Transform[] listContainers = null;
    [SerializeField] private QuestChapterView questChapterView_Prefab = null;
    [SerializeField] private QuestListDetailUI questListDetailUI = null;

    [SerializeField] private List<QuestChapterView> activeChapters = new List<QuestChapterView>();
    [SerializeField] private List<QuestChapterView> complateChapters = new List<QuestChapterView>();
    [SerializeField] private List<QuestChapterView> repeatChapters = new List<QuestChapterView>();

    protected override void Awake()
    {
        base.Awake();
        UIHelper.AddEventTrigger(categorys[0], EventTriggerType.PointerClick, delegate { OnPointerClickCategory(QuestListWindowType.ACTIVE); });
        UIHelper.AddEventTrigger(categorys[1], EventTriggerType.PointerClick, delegate { OnPointerClickCategory(QuestListWindowType.COMPLETE); });
        UIHelper.AddEventTrigger(categorys[2], EventTriggerType.PointerClick, delegate { OnPointerClickCategory(QuestListWindowType.REPEAT); });
        QuestManager.Instance.onRegister += RegisterQuestList;
        QuestManager.Instance.onComplete += CompleteQuestList;
        QuestManager.Instance.onRepeat += CompleteRepeatQuestList;
        QuestManager.Instance.onDelete += AllDelete;
        QuestManager.Instance.onLoadTemp += LoadQuestLists;
    }

    protected override void Start()
    {
        Debug.Log("퀘스트 리스트 로드 시작 ");
        LoadQuestLists();
        Debug.Log("퀘스트 리스트 로드 중");
        gameObject.SetActive(false);
        Debug.Log("퀘스트 리스트 로드 완 ");

    }

    private void OnDestroy()
    {
        QuestManager.Instance.onRegister -= RegisterQuestList;
        QuestManager.Instance.onComplete -= CompleteQuestList;
        QuestManager.Instance.onRepeat -= CompleteRepeatQuestList;
        QuestManager.Instance.onDelete -= AllDelete;
        QuestManager.Instance.onLoadTemp -= LoadQuestLists;
    }

    public override void OpenUIWindow()
    {
        base.OpenUIWindow();
        OnPointerClickCategory(QuestListWindowType.ACTIVE);
    }

    [ContextMenu("전부 삭제")]
    public void AllDelete()
    {
        for (int i = 0; i < activeChapters.ToArray().Length; i++)
        {
            activeChapters[i].AllDelete();
            activeChapters[i].gameObject.SetActive(false);
            Destroy(activeChapters[i].gameObject);
            activeChapters.RemoveAt(i);
        }
        for (int i = 0; i < complateChapters.ToArray().Length; i++)
        {
            complateChapters[i].AllDelete();
            complateChapters[i].gameObject.SetActive(false);
            Destroy(complateChapters[i].gameObject);
            complateChapters.RemoveAt(i);
        }
        for (int i = 0; i < repeatChapters.ToArray().Length; i++)
        {
            repeatChapters[i].AllDelete();
            repeatChapters[i].gameObject.SetActive(false);
            Destroy(repeatChapters[i].gameObject);
            repeatChapters.RemoveAt(i);
        }

        DeleteEmptyChapter(activeChapters);
        DeleteEmptyChapter(complateChapters);
        DeleteEmptyChapter(repeatChapters);

        UpdateAllVerticalLayoutGroup();
    }

    public void LoadQuestLists()
    {
        Debug.Log($"<color=blue>LoadQuestLists 시작</color>");

        for (int i = 0; i < QuestManager.Instance.activeQuests.Count; i++)
            RegisterQuestList(QuestManager.Instance.activeQuests[i]);

        Debug.Log($"<color=blue>LoadQuestLists 0</color>");

        for (int i = 0; i < QuestManager.Instance.completedQuests.Count; i++)
            CompleteQuestList(QuestManager.Instance.completedQuests[i]);
        Debug.Log($"<color=blue>LoadQuestLists 1</color>");

        for (int i = 0; i < QuestManager.Instance.repeatQuests.Count; i++)
        {
            if (QuestManager.Instance.repeatQuests[i] != null)
                CompleteRepeatQuestList(QuestManager.Instance.repeatQuests[i]);
        }

        Debug.Log($"<color=blue>LoadQuestLists 종료</color>");

        UpdateAllVerticalLayoutGroup();
    }


    private void RegisterQuestList(Quest quest, Task task = null)
    {
        if (CheckExistQuest(quest, false)) return;

        quest.OnCancel_ += DeleteActiveQuest;
        CreateChapter(quest, activeChapters, listContainers[(int)QuestListWindowType.ACTIVE]);
        UpdateAllVerticalLayoutGroup();
    }

    private void CompleteQuestList(Quest quest)
    {
        QuestChapterView chapter = GetChapterView(quest.QuestListSession, activeChapters);
        chapter?.RemoveTask(quest);
        DeleteEmptyChapter(activeChapters);
        CreateChapter(quest, complateChapters, listContainers[(int)QuestListWindowType.COMPLETE]);
        UpdateAllVerticalLayoutGroup();

    }


    private void CompleteRepeatQuestList(Quest quest)
    {   
        if(quest == null)
            Debug.Log($"<color=red>CompleteRepeatQuestList NULL</color>");

        Debug.Log($"<color=red>CompleteRepeatQuestList 시작</color>");
        if (!CheckExistQuest(quest, true))
            CreateChapter(quest, repeatChapters, listContainers[(int)QuestListWindowType.REPEAT]);
        Debug.Log($"<color=red>CompleteRepeatQuestList 0 </color>");

        QuestChapterView chapter = GetChapterView(quest.QuestListSession, activeChapters);
        Debug.Log($"<color=red>CompleteRepeatQuestList 1 </color>");

        if (chapter != null)
            chapter.RemoveTask(quest);

        Debug.Log($"<color=red>CompleteRepeatQuestList 2 </color>");

        DeleteEmptyChapter(activeChapters);
        Debug.Log($"<color=red>CompleteRepeatQuestList 3 </color>");

        UpdateAllVerticalLayoutGroup();
    }


    private void CreateChapter(Quest quest, List<QuestChapterView> questChapters, Transform parent)
    {
        if(quest == null)
            Debug.Log($"<color=red>Quest: NULL</color>");
        if (questChapters == null)
            Debug.Log($"<color=red>questChapters: NULL</color>");
        if (parent == null)
            Debug.Log($"<color=red>parent: NULL</color>");
        if (questListDetailUI == null)
            Debug.Log($"<color=red>questListDetailUI: NULL</color>");


        QuestChapterView chapter;
        Debug.Log($"Quest:  {quest?.DisplayName} Session {quest?.QuestListSession} 시작");

        if (CheckExistChapter(quest.QuestListSession, questChapters))
        {
            chapter = GetChapterView(quest.QuestListSession, questChapters);
            chapter.AddTask(quest);
        }
        else
        {
            chapter = Instantiate(questChapterView_Prefab, parent);
            UIHelper.AddEventTrigger(chapter.gameObject, EventTriggerType.PointerClick, delegate { OnPointerClickChapter(chapter); });
            chapter.onRegister += questListDetailUI.UpdateDetailWindow;
            chapter.Setting(quest);
            chapter.QuestSession = quest.QuestListSession;
            chapter.AddTask(quest);
            questChapters.Add(chapter);
        }
        Debug.Log($"Quest:  {quest?.DisplayName} Session {quest?.QuestListSession} 완료");

        UpdateAllVerticalLayoutGroup();
    }


    private void DeleteEmptyChapter(List<QuestChapterView> questChapters)
    {
        foreach (QuestChapterView view in questChapters.ToArray())
        {
            if (view ==null || view.Tasks.Count <= 0)
            {
                questChapters.Remove(view);
                view.gameObject.SetActive(false);
                Destroy(view.gameObject);
            }
        }
    }

    public void DeleteActiveQuest(Quest quest)
    {
        foreach (QuestChapterView chapter in activeChapters.ToArray())
            chapter.RemoveTask(quest);

        DeleteEmptyChapter(activeChapters);
        UpdateAllVerticalLayoutGroup();
    }

    private QuestChapterView GetChapterView(QuestListSession questListSession, List<QuestChapterView> chapters)
    {
        foreach (QuestChapterView chapter in chapters)
            if (chapter.QuestSession == questListSession)
                return chapter;

        return null;
    }

    private bool CheckExistQuest(Quest quest, bool isRepeat)
    {
        bool isExist = false;
        if(isRepeat)
        {
            for (int i = 0; i < repeatChapters.Count; i++)
                if (repeatChapters[i].CheckExistTask(quest))
                    isExist = true;
        }
        else
        {
            for (int i = 0; i < activeChapters.Count; i++)
                if (activeChapters[i].CheckExistTask(quest))
                    isExist = true;
            for (int i = 0; i < complateChapters.Count; i++)
                if (complateChapters[i].CheckExistTask(quest))
                    isExist = true;
        }

        return isExist;
    }

    private bool CheckExistChapter(QuestListSession questListSession, List<QuestChapterView> chapters)
    {
        QuestChapterView view = GetChapterView(questListSession, chapters);
        if (view == null)
            return false;
        else
            return true;
    }

    private void UpdateAllVerticalLayoutGroup()
    {
        UpdateVerticalLayoutGroup(activeChapters);
        UpdateVerticalLayoutGroup(complateChapters);
        UpdateVerticalLayoutGroup(repeatChapters);

    }

    private void UpdateVerticalLayoutGroup(List<QuestChapterView> views)
    {
        for (int i = 0; i < views.Count; i++)
            views[i].ExcuteVerticalLayoutGroup();

        for (int i = 0; i < listContainers.Length; i++)
            listContainers[i].GetComponent<CustomVerticalLayoutGroup>()?.Excute();
    }


    private void OnPointerClickChapter(QuestChapterView chapter)
    {
        chapter.IsListActive = !chapter.IsListActive;
        chapter.SetTaskActive(chapter.IsListActive);
        UpdateAllVerticalLayoutGroup();
    }

    private void OnPointerClickCategory(QuestListWindowType type)
    {
        AllCloseCategory();
        switch (type)
        {
            case QuestListWindowType.ACTIVE: listContainers[(int)QuestListWindowType.ACTIVE].gameObject.SetActive(true);  break;
            case QuestListWindowType.COMPLETE: listContainers[(int)QuestListWindowType.COMPLETE].gameObject.SetActive(true); break;
            case QuestListWindowType.REPEAT: listContainers[(int)QuestListWindowType.REPEAT].gameObject.SetActive(true); break;
        }
        UpdateAllVerticalLayoutGroup();
    }

    private void AllCloseCategory()
    {
        for (int i = 0; i < categorys.Length; i++)
            listContainers[i].gameObject.SetActive(false);
    }
}
