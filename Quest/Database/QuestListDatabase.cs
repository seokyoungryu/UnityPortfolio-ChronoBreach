using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Database/Quest/Quest List Database", fileName = "QuestListDatabase")]
public class QuestListDatabase : BaseFindobjectDatabase<QuestList>
{

#if UNITY_EDITOR
    [ContextMenu("퀘스트리스트 불러오기")]
    private void Find()
    {
        FindAddDatas();
        SetID();
        SetDirtys();
    }
#endif

}
