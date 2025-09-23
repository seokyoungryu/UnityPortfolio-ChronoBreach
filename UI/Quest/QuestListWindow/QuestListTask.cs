using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestListTask : MonoBehaviour
{
    [SerializeField] private Quest quest = null;
    [SerializeField] private TMP_Text task_Text = null;

    public Quest Quest => quest;

    public void SettingTask(Quest quest)
    {
        this.quest = quest;
        task_Text.text = quest.DisplayName;
    }
}
