using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    ENTER = 0,
    EXIT = 1,
}

[System.Serializable]
public class QuestReporterTriggerInfo
{
    [SerializeField] private TriggerType excuteTriggerType;
    [SerializeField] private string detectTag = string.Empty;
    [SerializeField] private QuestCategory category = null;
    [SerializeField] private int successCount = 0;

    public TriggerType ExcuteTriggerType => excuteTriggerType;
    public string DetectTag => detectTag;
    public QuestCategory Category => category;
    public int SuccessCount => successCount;
}

public class QuestReporterTrigger : MonoBehaviour
{
    [SerializeField] private TaskTarget target = null;
    [SerializeField] private QuestReporterTriggerInfo[] reporterInfos;
    private List<QuestReporterTriggerInfo> retInfos = new List<QuestReporterTriggerInfo>();


    private QuestReporterTriggerInfo[] FindReporterInfos(TriggerType triggerType)
    {
        retInfos.Clear();
        for (int i = 0; i < reporterInfos.Length; i++)
        {
            if (reporterInfos[i].ExcuteTriggerType == triggerType)
                retInfos.Add(reporterInfos[i]);
        }

        return retInfos.ToArray();
    }


    private void OnTriggerEnter(Collider other)
    {
        QuestReporterTriggerInfo[] infos = FindReporterInfos(TriggerType.ENTER);

        for (int i = 0; i < infos.Length; i++)
            if (other.CompareTag(infos[i].DetectTag))
                QuestManager.Instance.ReceiveReport(infos[i].Category, target, infos[i].SuccessCount);
    }

    private void OnTriggerExit(Collider other)
    {
        QuestReporterTriggerInfo[] infos = FindReporterInfos(TriggerType.EXIT);

        for (int i = 0; i < infos.Length; i++)
            if (other.CompareTag(infos[i].DetectTag))
                QuestManager.Instance.ReceiveReport(infos[i].Category, target, infos[i].SuccessCount);
    }

}
