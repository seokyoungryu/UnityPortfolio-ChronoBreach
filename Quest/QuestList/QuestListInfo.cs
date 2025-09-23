using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuestListInfo 
{
    public QuestListSession questSession;
    public QuestContainer[] questContain;
}

[System.Serializable]
public class QuestContainer
{
    [HideInInspector] public int npcID = -1;
    public int questDialogId = 0;
    public Quest quest;
    public bool isRepeatQuest = false;
    private QuestListSession questSession = QuestListSession.NONE;

    public QuestContainer(Quest quest)
    {
        this.quest = quest;
    }

    public void SetQuestSession(QuestListSession session)
        => questSession = session;
}
