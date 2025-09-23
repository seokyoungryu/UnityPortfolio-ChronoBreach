using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestListDetailUI : MonoBehaviour
{
    [SerializeField] private Quest currentQuest = null;
    [SerializeField] private TMP_Text title_Text = null;
    [SerializeField] private TMP_Text description_Text = null;
    [SerializeField] private Button cancel_Btn = null;

    public Quest CurrentQuest => currentQuest;

    public void UpdateDetailWindow(Quest quest)
    {
        currentQuest = quest;
        SettingDetailUIs(currentQuest);
    }

    private void SettingDetailUIs(Quest quest)
    {
        title_Text.text = quest.DisplayName;
        description_Text.text = quest.Description;
        if (quest.QuestState == QuestState.RUNNING || quest.QuestState == QuestState.WAIT_FOR_COMPLETE)
            cancel_Btn.gameObject.SetActive(true);
        else
            cancel_Btn.gameObject.SetActive(false);
    }

    public void Cancel_Btn()
    {
        if (currentQuest == null) return;
        
        currentQuest.CancelQuest();
        currentQuest = null;
        title_Text.text = "";
        description_Text.text = "";
        cancel_Btn.gameObject.SetActive(false);
        //Notifierµµ ªË¡¶, 
    }

}
