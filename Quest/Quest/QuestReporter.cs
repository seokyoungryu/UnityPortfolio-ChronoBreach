using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestReporterInfo
{
    [SerializeField] private string categoryCode = string.Empty;
    [SerializeField] private QuestCategory category = null;
    [SerializeField] private int successCount = 0;

    public string CategoryCode { get { return categoryCode; } set { categoryCode = value; } }
    public QuestCategory Category => category;
    public int SuccessCount => successCount;

}

public class QuestReporter : MonoBehaviour
{
    [SerializeField] private TaskTarget target;
    [SerializeField] private QuestReporterInfo[] reporterInfos;

    public TaskTarget Target => target;


    public void SetTarget(TaskTarget target)
    {
        this.target = target;
    }

    public void ReceiveReport(string categoryCode)
    {
        if (reporterInfos.Length <= 0) return;
        if (target == null) return;

        for (int i = 0; i < reporterInfos.Length; i++)
        {
            if (reporterInfos[i].CategoryCode == categoryCode)
            {
                QuestManager.Instance.ReceiveReport(reporterInfos[i].Category, target, reporterInfos[i].SuccessCount);
            }
        }
    }


    private void OnValidate()
    {
        if (target == null) return;
        if(reporterInfos.Length > 0)
        {
            for (int i = 0; i < reporterInfos.Length; i++)
                if (reporterInfos[i].Category != null)
                    reporterInfos[i].CategoryCode = reporterInfos[i].Category.CodeName;
        }
    }
}
