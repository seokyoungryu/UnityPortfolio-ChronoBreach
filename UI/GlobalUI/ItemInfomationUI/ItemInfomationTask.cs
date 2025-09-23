using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfomationTask : MonoBehaviour
{
    [SerializeField] private TMP_Text taskText = null;
    [SerializeField] private string obpName = string.Empty;
    [SerializeField] private float taskValue = 0f;

    private bool canShow = false;

    public string OBPName => obpName;
    public float TaskValue => taskValue;

    public void SettingText(string text, float value)
    {
        canShow = true;
        if (taskText == null) taskText = GetComponentInChildren<TMP_Text>();
        if (value != -1 && (value <= 0.0f || value <= 0.01f))
        {
            canShow = false;
            ObjectPooling.Instance.SetOBP(obpName, this.gameObject);
        }

        if (canShow)
        {
            //Debug.Log("Ãâ·Â : " + text + " : " + value);
            taskText.text = text;
            taskValue = value;
        }
    }
}
