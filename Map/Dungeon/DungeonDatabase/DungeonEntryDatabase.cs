using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Database/Dungeon Entry Database ", fileName = "DungeonEntryDB")]
public class DungeonEntryDatabase : ScriptableObject
{
    [Header("등록할 캡터 목록")]
    [SerializeField] private List<DungeonTitleDatabase> chapters;

    [Header("DungeonUI Open시 보여줄 캡터")]
    public StringTaskTarget currentChapterTaskTarget = null;

    public StringTaskTarget CurrentChapterTarget { get { return currentChapterTaskTarget; } set { currentChapterTaskTarget = value; } }
    public List<DungeonTitleDatabase> Chapters => chapters;
}
