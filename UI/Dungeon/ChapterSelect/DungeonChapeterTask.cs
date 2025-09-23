using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonChapeterTask : ImageColorByCursor
{
    [SerializeField] private TMP_Text title_Text = null;
    [SerializeField] private DungeonTitleDatabase chapterDatabase = null;

    public DungeonTitleDatabase ChapterDatabase => chapterDatabase;

    #region Events
    public delegate void OnClickTask(StringTaskTarget targetString);
    public delegate void OnClickSetting(DungeonTitleDatabase chapter);

    public event OnClickTask onClickTask;
    public event OnClickSetting onSetChapter;
    #endregion

    public void SettingTitle(DungeonTitleDatabase title)
    {
        chapterDatabase = title;
        title_Text.text = chapterDatabase.ChapterName.DisplayName;
    }


    protected override void Select()
    {
        base.Select();
        onSetChapter?.Invoke(chapterDatabase);
        onClickTask?.Invoke(chapterDatabase.ChapterName);
    }


}
