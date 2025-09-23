using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonSelectTask : ImageColorByCursor
{
    [SerializeField] private Image task_Image = null;
    [SerializeField] private TMP_Text titleName_Text = null;
    [SerializeField] private DungeonTitleDatabase chapterDatabase = null;
    [SerializeField] private BaseDungeonTitle title = null;
    [SerializeField] private Color lockColor = Color.red;
    [SerializeField] private Color unlockColor = Color.white;

    #region Events
    public delegate void OnSelected(BaseDungeonTitle title);
 
    public event OnSelected onSelected;
#endregion

    public DungeonTitleDatabase ChapterDatabase => chapterDatabase;
    public BaseDungeonTitle Title => title;

    public void SettingTitle(DungeonTitleDatabase chapterDatabase, BaseDungeonTitle title)
    {
        this.chapterDatabase = chapterDatabase;
        this.title = title;
        titleName_Text.text = title.TaskTarget.DisplayName;
    }


    protected override void Select()
    {
        base.Select();
        onSelected?.Invoke(title);
        chapterDatabase.CurrentSelectedTitle = title.TaskTarget;
        MapManager.Instance.CurrentSelectedDungeonTitle = title;
    }

    public void ExcuteSelect() => Select();
    public void OnSelectedReset() => onSelected = null;
}
