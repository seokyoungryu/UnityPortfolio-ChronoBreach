using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNotifierView : MonoBehaviour
{
    [SerializeField] private QuestNotifier questNotifierPrefab;
    [SerializeField] private Transform seperateImg = null;
    [SerializeField] private CategoryColor[] categoryColors;

    private List<QuestNotifier> notifers = new List<QuestNotifier>();
    CustomVerticalLayoutGroup verticalLayoutGroup;

    private void Awake()
    {
        verticalLayoutGroup = GetComponent<CustomVerticalLayoutGroup>();
        QuestManager.Instance.onRegister += RegisterQuestNotifier;
        QuestManager.Instance.onRegisterLoadQuest += RegisterLoadQuestNotifier;
        QuestManager.Instance.onDelete += DestroyAll;

    }

    private void OnDestroy()
    {
        QuestManager.Instance.onRegister-= RegisterQuestNotifier;
        QuestManager.Instance.onRegisterLoadQuest -= RegisterLoadQuestNotifier;
        QuestManager.Instance.onDelete -= DestroyAll;

    }

    public void DestroyAll()
    {
        foreach (Transform tr in transform)
        {
            Debug.Log("삭제 - " + tr.name);
            Destroy(tr.gameObject);
        }

        Debug.Log("올 삭제");
    }


    public void RegisterQuestNotifier(Quest quest, Task task)
    {
      //  Debug.Log("RegisterQuestNotifier In");
      //  Debug.Log("퀘스트 : " + quest + " Task : " + task);
        SeperateSet(quest);
        quest.OnComplete_ += SeperateSet;
        Color textColor = GetTextColor(quest);
        QuestNotifier go = Instantiate(questNotifierPrefab, transform);
        go.SetUp(quest, textColor);
        notifers.Add(go);
        verticalLayoutGroup?.Excute();
    }
    public void RegisterLoadQuestNotifier(Quest quest, Task task)
    {
        Color textColor = GetTextColor(quest);
        QuestNotifier go = Instantiate(questNotifierPrefab, transform);
        go.SetUpLoadQuest(quest, textColor);
        notifers.Add(go);
        verticalLayoutGroup?.Excute();
    }

    private Color GetTextColor(Quest quest)
    {
        Color textColor = Color.black;
        foreach (CategoryColor color in categoryColors)
            if (color.category.CodeName == quest.CodeName)
                textColor = color.color;
        return textColor;
    }

    private void SeperateSet(Quest quest)
    {
        if (QuestManager.Instance.activeQuests.Count <= 0)
            seperateImg.gameObject.SetActive(false);
        else if (QuestManager.Instance.activeQuests.Count > 0)
            seperateImg.gameObject.SetActive(true);

    }
}

[System.Serializable]
public class CategoryColor
{
    public QuestCategory category;
    public Color color;
}



