using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamagedTextContainer : MonoBehaviour
{
    [SerializeField] private string floatingDamagedTextOBPName = string.Empty;
    private List<FloatingDamagedText> containers = new List<FloatingDamagedText>();



    public void ExcuteFloatingDamagedText(int damage, FloatingType floatingType, Transform targetTransform)
    {
        FloatingDamagedText damagedText = ObjectPooling.Instance.GetOBP(floatingDamagedTextOBPName).GetComponent<FloatingDamagedText>();
        damagedText.SettingText(damage, targetTransform, floatingType);
        containers.Add(damagedText);
        ReturnObjectToObjectPooling obp = damagedText.GetComponent<ReturnObjectToObjectPooling>();
        obp.onResetData += () => { damagedText.enabled = false; };
        obp.TimeSetting(1.3f, 1f);
    }


    public void AllCloseTexts()
    {
        for (int i = 0; i < containers.Count; i++)
        {
            if (containers[i] == null) continue;
            containers[i].GetComponent<ReturnObjectToObjectPooling>().SetOBP();
        }
        containers.Clear();
    }
}
