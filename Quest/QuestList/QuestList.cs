using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Quest/Quest List", fileName = "QuestList_")]
public class QuestList : BaseScriptableObject
{
    public DialogFile dialogFile;
    public QuestListInfo[] questInfos;



    public int[] FindQuestInfosIndex(QuestContainer questContainer)
    {
        for (int i = 0; i < questInfos.Length; i++)
            for (int x = 0; x < questInfos[i].questContain.Length; x++)
                if (questInfos[i].questContain[x].quest.CodeName == questContainer.quest.CodeName)
                    return new int[] { i, x };
        return new int[] { -1 , -1};
    }
}
