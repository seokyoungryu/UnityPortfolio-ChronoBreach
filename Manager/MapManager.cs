using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreUIType { NOT_EXCUTE = 0, FAIL_EXCUTE =1, SUCCESS_EXCUTE =2}


[RequireComponent(typeof(CoroutineForDungeon))]
public class MapManager : Singleton<MapManager>
{
    [SerializeField] private BaseDungeonTitle currentExcuteDungeonTitle = null;
    [SerializeField] private BaseDungeonTitle currentSelectedDungeonTitle = null;
    [SerializeField] private PlayerStateController controller = null;
    [SerializeField] private DungeonNotifierUI dungeonNotifierUI = null;
    [SerializeField] private ScoreUIType currentScoreUIType = ScoreUIType.NOT_EXCUTE;
    [SerializeField] private bool ignoreEntryConditions = false;
    private CoroutineForDungeon dungeonCoroutine = null;
    public DungeonPortalUI dungeonPortalUI = null;



    public BaseDungeonTitle CurrentDungeonTitle { get { return currentExcuteDungeonTitle; } set { currentExcuteDungeonTitle = value; } }
    public BaseDungeonTitle CurrentSelectedDungeonTitle { get { return currentSelectedDungeonTitle; } set { currentSelectedDungeonTitle = value; } }
    public CoroutineForDungeon DungeonCoroutine { get { return dungeonCoroutine; } set { dungeonCoroutine = value; } }
    public PlayerStateController Controller { get { return controller; } set { controller = value; } }
    public ScoreUIType CurrentScoreUIType { get { return currentScoreUIType; } set { currentScoreUIType = value; } }
    public bool IgnoreEntryConditions { get { return ignoreEntryConditions; } set { ignoreEntryConditions = value; } }

    public DungeonNotifierUI DungeonNotifierUI => dungeonNotifierUI;


    protected override void Awake()
    {
        base.Awake();

        dungeonCoroutine = GetComponent<CoroutineForDungeon>();
        if (dungeonPortalUI == null)
            dungeonPortalUI = FindObjectOfType<DungeonPortalUI>();
        if (controller == null)
            controller = GameManager.Instance.Player;
    }

    public void ExcuteReply()
    {
        if (currentExcuteDungeonTitle == null) return;
        //1. obj들 초기화.
        currentExcuteDungeonTitle.dungeonCoroutine.StopAllCoroutines();
        GameManager.Instance.Player.Resurrection();
        CommonUIManager.Instance.AllCloseActiveUIWindow();
        currentExcuteDungeonTitle.ClearObj();
        ExcuteDungeon(currentSelectedDungeonTitle);
    }

    public void ExcuteDungeon(BaseDungeonTitle title)
    {
        if (controller == null)
            controller = GameManager.Instance.Player;
        if(currentExcuteDungeonTitle != null)
        {
            currentExcuteDungeonTitle.dungeonCoroutine.StopAllCoroutines();
            currentExcuteDungeonTitle = null;
        }

        controller.playerStats.Resurrection();
       dungeonCoroutine.title = title;
       currentExcuteDungeonTitle = title.GetClone();
       currentExcuteDungeonTitle.SetDungeonCoroutine(dungeonCoroutine);
       currentExcuteDungeonTitle.SettingControllers(controller);
       currentExcuteDungeonTitle.Excute();
    }


    public void OpenDungeonSelectionUI(DungeonEntryDatabase database)
    {
        if (dungeonPortalUI == null)
            dungeonPortalUI = FindObjectOfType<DungeonPortalUI>();

        dungeonPortalUI.OpenUIWindow();
        dungeonPortalUI.SettingData(database);
        dungeonPortalUI.CreateAreaAndTasks();

        if (database.CurrentChapterTarget == null)
            database.CurrentChapterTarget = database.Chapters[0].ChapterName;

        dungeonPortalUI.OpenChapter(database.CurrentChapterTarget);

    }

    public void CloseDungeonSelectionUI()
    {
        if (dungeonPortalUI == null)
            dungeonPortalUI = FindObjectOfType<DungeonPortalUI>();

        //여기선 dungeonPortalUI에 Task들을 제거해주고 창 닫기.
        dungeonPortalUI.CloseUIWindow();
    }


    private void OnDrawGizmos()
    {
        if (currentExcuteDungeonTitle != null)
        {
            currentExcuteDungeonTitle.ExcuteDrawCurrentWave(-1);
        }
    }

}
