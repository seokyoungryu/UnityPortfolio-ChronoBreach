using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DetailConditionType
{
    NONE = -1,
    LV = 0,
    DAMAGED = 1,
    REPUTATION =2 ,
    TITLE = 3,
}

public class DungeonDetailConditionTask : MonoBehaviour
{
    [SerializeField] private DetailConditionType conditionType = DetailConditionType.NONE;
    [SerializeField] private float conditionValue = -1;
    private bool isUnLock = false;

    [Header("Containers")]
    [SerializeField] private Transform twoDataTransform = null;
    [SerializeField] private Transform oneDataTransform = null;
    [Header("For Two Data")]
    [SerializeField] private TMP_Text title_Text = null;
    [SerializeField] private TMP_Text data_text = null;
    [Header("For One Data")]
    [SerializeField] private TMP_Text onlyTitle = null;
    [SerializeField] private Color lockColor = Color.red;
    [SerializeField] private Color unlockColor = Color.white;


    [SerializeField] private ReturnObjectToObjectPooling returnPooling = null;

    private BaseDungeonTitle conditionTitle = null;

    public DetailConditionType ConditionType { get { return conditionType; } set { conditionType = value; } }
    public BaseDungeonTitle ConditionTitle => conditionTitle;
    public float ConditionValue => conditionValue;
    public bool IsUnLock => isUnLock;

    public void Setting(string titleText, int data)
    {
        twoDataTransform.gameObject.SetActive(true);
        oneDataTransform.gameObject.SetActive(false);

        title_Text.text = titleText;
        data_text.text = data.ToString();
        conditionValue = data;
    }

    public void Setting(string titleText, BaseDungeonTitle conditionTitle = null)
    {
        twoDataTransform.gameObject.SetActive(false);
        oneDataTransform.gameObject.SetActive(true);

        onlyTitle.text = titleText;
        if (conditionTitle != null)
            this.conditionTitle = conditionTitle;
    }

    public void SetOBP() => returnPooling.SetOBP();


    public void ChangeLock()
    {
        title_Text.color = lockColor;
        data_text.color = lockColor;
        onlyTitle.color = lockColor;
        isUnLock = false;
    }

    public void ChangeUnLock()
    {
        title_Text.color = unlockColor;
        data_text.color = unlockColor;
        onlyTitle.color = unlockColor;
        isUnLock = true;
    }
}
