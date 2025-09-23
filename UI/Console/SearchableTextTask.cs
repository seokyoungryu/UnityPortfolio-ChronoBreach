using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SearchableTextTask : MonoBehaviour
{
    [SerializeField] private TMP_Text searchableText_text = null;
    [SerializeField] private CustomVerticalLayoutGroup layoutGroup = null;

    [SerializeField] private Image background_img= null;
    [SerializeField] private Color originColor;
    [SerializeField] private Color selectedColor;

    public string GetText => searchableText_text.text;

    public void SettingTask(string searchableText)
    {
        searchableText_text.text = searchableText;
        UnSelect();
        layoutGroup.Excute();
    }

    public void Select()
    {
        background_img.color = selectedColor;
    }
    public void UnSelect()
    {
        background_img.color = originColor;
    }
}
