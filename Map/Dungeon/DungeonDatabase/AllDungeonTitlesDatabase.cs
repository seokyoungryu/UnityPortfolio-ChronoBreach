using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Map/Dungeon Database/Dungeon Database ", fileName = "DungeonDB")]
public class AllDungeonTitlesDatabase : BaseFindobjectDatabase<BaseDungeonTitle>
{
#if UNITY_EDITOR
    [ContextMenu("Find Titles")]
    public void FindTitles()
    {
        FindAddDatas();
        SetID();
        SetDirtys();
    }
#endif


    public void ResetClear()
    {
        for (int i = 0; i < database.Count; i++)
            database[i].DungeonReset();
    }

}
