using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class DungeonPortalUI : UIRoot
{
    private DungeonEntryDatabase dungeonDatabase = null;
    [SerializeField] private string dungeonSelectAreaOBP = string.Empty;
    [SerializeField] private string dungeonSelectTaskOBP = string.Empty;
    [SerializeField] private Transform lockImg = null;
    [SerializeField] private Transform chaptersParent = null;
    [SerializeField] private TMP_Text chapterName_Text = null;
    [SerializeField] private ScrollUI[] scrollUIs = null;
    private CustomVerticalLayoutGroup verticalLayoutGroup = null;
    private List<DungeonSelectArea> chaptersList;


    public DungeonEntryDatabase DungeonDatabase => dungeonDatabase;
    #region Events
    public delegate void Excute(DungeonEntryDatabase dungeonDatabase);
    public delegate void ExcuteReciveChapter(DungeonTitleDatabase titleDatabase);
    public delegate void UpdateDetailUI(BaseDungeonTitle title);

    private event UpdateDetailUI onUpdateDetail;
    public event UpdateDetailUI OnUpdateDetail_
    {
        add
        {
            if (onUpdateDetail == null || !onUpdateDetail.GetInvocationList().Contains(value))
                onUpdateDetail += value;
        }
        remove
        {
            onUpdateDetail -= value;
        }
    }
    private event Excute onSelectChapter;
    public event Excute OnSelectChapter_
    {
        add
        {
            if (onSelectChapter == null || !onSelectChapter.GetInvocationList().Contains(value))
                onSelectChapter += value;
        }
        remove
        {
            onSelectChapter -= value;
        }
    }
    private event ExcuteReciveChapter onReceiveChapter;
    public event ExcuteReciveChapter OnReceiveChapter_
    {
        add
        {
            if (onReceiveChapter == null || !onReceiveChapter.GetInvocationList().Contains(value))
                onReceiveChapter += value;
        }
        remove
        {
            onReceiveChapter -= value;
        }
    }
    #endregion


    protected override void Awake()
    {
        base.Awake();
        verticalLayoutGroup = GetComponentInChildren<CustomVerticalLayoutGroup>();
        gameObject.SetActive(false);
    }


    public void SettingData(DungeonEntryDatabase database)
    {
        dungeonDatabase = database;
    }

    public void OpenChapter(StringTaskTarget chapterTarget)
    {
        //this.gameObject.SetActive(true);
        OpenUIWindow();

        foreach (DungeonSelectArea area in chaptersList)
            area.gameObject.SetActive(false);

        foreach (DungeonSelectArea area in chaptersList)
        {
            if (area.TitleDatabse.ChapterName == chapterTarget)
            {
                area.gameObject.SetActive(true);
                dungeonDatabase.CurrentChapterTarget = area.TitleDatabse.ChapterName;
                onReceiveChapter?.Invoke(area.TitleDatabse);
                chapterName_Text.text = area.TitleDatabse.ChapterName.DisplayName;
                foreach (DungeonSelectTask task in area.Titles)
                {
                    if (area.TitleDatabse.CurrentSelectedTitle == null)
                    {
                        Debug.Log("curr Title  NULL");
                        UpdateDetail(null);
                    }
                    else if (area.TitleDatabse.CurrentSelectedTitle != null 
                        && task.Title.TaskTarget == area.TitleDatabse.CurrentSelectedTitle)
                    {
                        Debug.Log("curr Title !" + task.Title.TaskTarget.DisplayName);
                        task.ExcuteSelect();
                    }
                }

            }
        }

        Debug.Log("열림 ");

    }

    public void CreateAreaAndTasks()
    {
        chaptersList = new List<DungeonSelectArea>();
        Debug.Log("던전 : " + dungeonDatabase.Chapters.Count);
        ClearAreas();
        for (int i = 0; i < dungeonDatabase.Chapters.Count; i++)
        {
            if (!dungeonDatabase.Chapters[i].IsLockChapter)
            {
                DungeonSelectArea area = ObjectPooling.Instance.GetOBP(dungeonSelectAreaOBP).GetComponent<DungeonSelectArea>();
                area.transform.SetParent(chaptersParent);
                UIPositionReset(area.GetComponent<RectTransform>());
                area.ClearTitleList();
                area.SettingChapter(dungeonDatabase.Chapters[i]);

                for (int x = 0; x < dungeonDatabase.Chapters[i].Titles.Count; x++)
                {
                    Debug.Log($"던전 {i} Titles.Count : " + dungeonDatabase.Chapters[i].Titles.Count);
                    DungeonSelectTask task = ObjectPooling.Instance.GetOBP(dungeonSelectTaskOBP).GetComponent<DungeonSelectTask>();
                    task.SettingTitle(area.TitleDatabse ,dungeonDatabase.Chapters[i].Titles[x]);
                    task.transform.SetParent(area.transform);
                    task.OnSelectedReset();
                    task.onSelected += delegate { OnPointerClickOrderByWindow(this); };
                    task.onSelected += UpdateDetail;
                    task.onSelected += ResetScrollPosition;
                    
                    UIPositionReset(task.GetComponent<RectTransform>());
                    area.AddTitlePrefabs(task);
                }

                AddImageColorList(area);
                area.ExcuteVerticalLayout();
                chaptersList.Add(area);
                area.gameObject.SetActive(false);
            }
           
        }
    }


    public void ClearAreas()
    {
        Transform[] child = chaptersParent.GetComponentsInChildren<Transform>();

        if (child.Length <= 0) return;
        for (int i = 0; i < child.Length; i++)
        {
            if (child[i] == chaptersParent || child[i] == null) continue;
            child[i].gameObject.SetActive(true);
            child[i].gameObject.GetComponent<ReturnObjectToObjectPooling>()?.SetOBP();
        }

    }

    public void ResetScrollPosition(BaseDungeonTitle title = null)
    {
        for (int i = 0; i < scrollUIs.Length; i++)
            scrollUIs[i].ResetPosition();
    }

    public void UpdateDetail(BaseDungeonTitle title)
    {
        onUpdateDetail?.Invoke(title);
    }

    public void OpenSelectChapterWindow()
    {
        //this.gameObject.SetActive(false);
        CloseUIWindow();
        onSelectChapter?.Invoke(dungeonDatabase);
    }


    private void UIPositionReset(RectTransform rect)
    {
        rect.anchoredPosition = new Vector2(0,0);
        rect.localScale = new Vector2(1,1);
    }


    private void AddImageColorList(DungeonSelectArea area)
    {
        for (int i = 0; i < area.Titles.Count; i++)
        {
            area.Titles[i].ImageList.Clear();
            area.Titles[i].AddToList(area.Titles.ToArray());
        }
    }

    public void ExcuteEntryDungeon()
    {
        gameObject.SetActive(false);
        MapManager.Instance.ExcuteDungeon(MapManager.Instance.CurrentSelectedDungeonTitle);
    }

}
