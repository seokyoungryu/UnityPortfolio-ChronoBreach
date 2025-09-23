using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Database/Quest/Quest Database", fileName ="Database")]
public class QuestDatabase : BaseFindobjectDatabase<Quest>
{

    public Quest FindQuestByCodeName(string codeName)
    {
        foreach (Quest quest in database)
        {
            if (quest.CodeName == codeName)
                return quest;
        }

        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("퀘스트 목록 불러오기")]
    public void SetQuestList()
    {
        FindAddDatas();
        SetID();
        SetDirtys();
    }
#endif

    [ContextMenu("업적 불러오기")]
    public void SetAchievementList()
    {

    }



}
