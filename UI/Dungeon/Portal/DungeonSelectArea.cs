using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSelectArea : MonoBehaviour
{
    [SerializeField] private DungeonTitleDatabase titleDatabase = null;
    private List<DungeonSelectTask> titles = new List<DungeonSelectTask>();
    private CustomVerticalLayoutGroup verticalLayoutGroup = null;

    public DungeonTitleDatabase TitleDatabse => titleDatabase;
    public List<DungeonSelectTask> Titles => titles;

    public void SettingChapter(DungeonTitleDatabase database)
    {
        titleDatabase = database;
        verticalLayoutGroup = GetComponent<CustomVerticalLayoutGroup>();
    }

    public void AddTitlePrefabs(DungeonSelectTask titleTask)  => titles.Add(titleTask);
    public void ClearTitleList() => titles.Clear();
    public void ExcuteVerticalLayout() => verticalLayoutGroup.Excute();

}
