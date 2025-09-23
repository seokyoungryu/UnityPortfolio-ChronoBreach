using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Database/Title Database ", fileName = "TItleDB_")]
public class DungeonTitleDatabase : ScriptableObject
{
    [SerializeField] private StringTaskTarget chapterName = null;

    [Header("각 챕터에 들어갈 던전 타이틀들 세팅")]
    [SerializeField] private List<BaseDungeonTitle> titles;
    [SerializeField] private bool isLockChapter = true;
    [SerializeField] private StringTaskTarget currentSelectedTitle = null;

    public List<BaseDungeonTitle> Titles => titles;
    public StringTaskTarget ChapterName => chapterName;
    public bool IsLockChapter => isLockChapter;
    public StringTaskTarget CurrentSelectedTitle { get { return currentSelectedTitle; } set { currentSelectedTitle = value; } }
}

