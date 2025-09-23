using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIInfoSetting : MonoBehaviour
{
    [SerializeField] private TMP_Text lv_Text = null;
    [SerializeField] private TMP_Text name_Text = null;


    public void InfoSetting(AIStatus status)
    {
        lv_Text.text = "Lv." + status.CurrentLevel.ToString();
        name_Text.text = status.AINameUI;
    }
}
