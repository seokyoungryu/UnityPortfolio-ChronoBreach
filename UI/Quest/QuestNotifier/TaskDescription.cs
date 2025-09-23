using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color completeColor;
    [SerializeField] private Color successCountColor;


    public void UpdateText(string text)
    {
        this.text.fontStyle = FontStyles.Normal;
        this.text.text = text;
    }

    public void UpdateText(Task task)
    {
        text.fontStyle = FontStyles.Normal;
        if (task.IsComplete || task.Owner.QuestState == QuestState.WAIT_FOR_COMPLETE || task.Owner.QuestState == QuestState.COMPLETE)
        {
            string completeColorCode = ColorUtility.ToHtmlStringRGB(completeColor);
            text.text = WriteText(task, completeColorCode, completeColorCode);
        }
        else
            text.text = WriteText(task, ColorUtility.ToHtmlStringRGB(normalColor), ColorUtility.ToHtmlStringRGB(successCountColor));

        Vector2 rectSize = GetComponent<RectTransform>().sizeDelta;
        rectSize.y = text.preferredHeight;
        GetComponent<RectTransform>().sizeDelta = rectSize;
    }



    private string WriteText(Task task, string normalColorCode, string SuccessColorCode)
    {
        return $"<color=#{normalColorCode}> ¢º {task.Description} <color=#{SuccessColorCode}> {task.CurrentSuccessCount} </color>/ {task.NeedSuccessCount} </color>";
    }
}
