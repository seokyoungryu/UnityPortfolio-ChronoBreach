using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleTask : MonoBehaviour
{
    [SerializeField] private TMP_Text taskText_text = null;
    [SerializeField] private CustomVerticalLayoutGroup customVerticalLayGroup = null;

    public CustomVerticalLayoutGroup VerticalLayGp => customVerticalLayGroup;

    public void SettingTask(string taskText)
    {
        taskText_text.text = taskText;
        customVerticalLayGroup.Excute();
    }

    public void SettingFontColor(Color color)
    {
        taskText_text.color = color;
    }
  
}
