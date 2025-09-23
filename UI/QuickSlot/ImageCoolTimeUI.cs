using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageCoolTimeUI : MonoBehaviour
{
    public Image coolTime_Img = null;
    public TMP_Text coolTime_Text = null;

    public void Clear()
    {
        coolTime_Img.fillAmount = 0f;
        coolTime_Text.text = "";
    }

    public void CoolTime(float currentValue, float maxValue)
    {
        if (maxValue <= 0.1f || currentValue <= 0.1f)
        {
            Clear();
            return;
        }
        coolTime_Img.fillAmount = currentValue / maxValue;
        coolTime_Text.text = currentValue.ToString("0.0");
    }


}
