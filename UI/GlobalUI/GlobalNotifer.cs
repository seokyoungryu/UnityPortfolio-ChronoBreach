using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalNotifer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notiferString_Text = null;


    public void Setting(string notiferString)
    {
        notiferString_Text.text = notiferString;
    }
}
