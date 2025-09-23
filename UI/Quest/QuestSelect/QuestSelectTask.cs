using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSelectTask : MonoBehaviour
{
    [SerializeField] private QuestContainer questContainer;
    [SerializeField] private TMP_Text questName_Text = null;

    public QuestContainer GetQuestContainter => questContainer;
    public Quest GetQuest => questContainer.quest;

    public void Setting(QuestContainer questContainer, int npcID)
    {
        this.questContainer = questContainer;
        questContainer.npcID = npcID;
        questName_Text.text = questContainer.quest.DisplayName;
    }
}
