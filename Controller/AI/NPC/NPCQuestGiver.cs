using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuestGiver : MonoBehaviour
{
    private QuestListSession currentQuestListSession = QuestListSession.MAIN1;
    public QuestListSession CurrentQuestListSession
    {
        set
        {
            if (currentQuestListSession != value)
            {
                canGiveQuests.Clear();
                currentQuestListSession = value;
                CheckStart();
            }
        }
        get { return currentQuestListSession; }
    }

    [SerializeField] private bool autoRegisterQuest = false;
    private bool endAuto = false;
    private NpcController npcController = null;
    private bool checkSession = true;

    [SerializeField] private Renderer renderer;
    [SerializeField] private QuestList questList;
    [SerializeField] private bool doCheckConditions = false;
    [SerializeField] private float conditionCycleRepeatTime = 0.2f;
    [SerializeField] private List<QuestContainer> waitForCompleteQuests = new List<QuestContainer>();
    [SerializeField] private List<QuestContainer> canGiveQuests = new List<QuestContainer>();
    [SerializeField] private List<QuestContainer> progressQuests = new List<QuestContainer>();

    [Header("순서순 [0] Main, [1] Sub")]
    [SerializeField] private QuestTargetMaterial[] notifierMatertials;
    [SerializeField] private Material notifierWaitForCompleted;
    [SerializeField] private Material notifierProgressing;
    public List<QuestContainer> WaitForCompleteQuests => waitForCompleteQuests;
    public List<QuestContainer> ProgressQuests => progressQuests;

    public IReadOnlyList<QuestContainer> CanGiveQuests => canGiveQuests;
    public int canGiveQuestCount => canGiveQuests.Count;
    public QuestList QuestList { get{ return questList; }set { questList = value; } }

    private void Awake()
    {
        npcController = GetComponent<NpcController>();
        if (renderer == null)
            renderer = GetComponentInChildren<Renderer>();
        if (renderer != null) renderer.enabled = false;
    }

    private void Start()
    {
        if (questList == null) return;
        SettingQuestContainers();
        CheckStart();
        if (autoRegisterQuest)
            StartCoroutine(AutoRegisterQuest());

        StartCoroutine(CheckQuestSession());
    }

    private void OnEnable()
    {
        if (questList == null) return;
        SettingQuestContainers();
        CheckStart();
        if (autoRegisterQuest)
            StartCoroutine(AutoRegisterQuest());

        StartCoroutine(CheckQuestSession());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private IEnumerator CheckQuestSession()
    {
        while (checkSession)
        {
            if (currentQuestListSession != QuestManager.Instance.currentQuestSession)
                CurrentQuestListSession = QuestManager.Instance.currentQuestSession;
            yield return new WaitForSeconds(1f);
        }

    }

    private IEnumerator AutoRegisterQuest()
    {
        while (!endAuto)
        {
            if (canGiveQuests.Count > 0)
            {
                if (!QuestManager.Instance.isDialoging)
                {
                    npcController.RegisterCanRegisterQuest(canGiveQuests[0]);
                }
                yield return new WaitForSeconds(1f);
            }

            yield return new WaitForSeconds(1f);
           // Debug.Log(gameObject.name + "- Auto Quests : " + canGiveQuests.Count);
        }

    }


    //테스트용
  // private void Update()
  // {
  //     if (Input.GetKeyDown(KeyCode.X))
  //     {
  //         CheckStart();
  //     }
  //
  //     if (Input.GetKeyDown(KeyCode.Z))
  //         CurrentQuestListSession = QuestListSession.MAIN2;
  //
  //     if(Input.GetKeyDown(KeyCode.C))
  //     {
  //         if (canGiveQuests.Count > 0)
  //         {
  //             foreach (QuestContainer contain in canGiveQuests)
  //             {
  //                 if (contain.isRepeatQuest && QuestManager.Instance.ExistCompleteQuest(contain.quest))
  //                 {
  //                     QuestManager.Instance.RemoveInCompletedQuests(contain.quest);
  //                     QuestManager.Instance.Register(contain.quest, this, contain);
  //                 }
  //                 else
  //                     QuestManager.Instance.Register(contain.quest, this, contain);
  //
  //             }
  //         }
  //     }
  // }


    public void CheckStart()
    {
        if (questList == null) return;
        StopAllCoroutines();
        CheckRegisterCondition(CurrentQuestListSession);
        CheckRegisterCondition(QuestListSession.ANY);
        if (renderer != null && !autoRegisterQuest)
            StartCoroutine(ShowNotifier(canGiveQuests));

        StartCoroutine(CheckCanCancelList(canGiveQuests));
    }


    private void SettingQuestContainers()
    {
        if (questList == null) return;

        foreach (QuestListInfo info in questList.questInfos)
            foreach (QuestContainer container in info.questContain)
                container.SetQuestSession(info.questSession);
    }

    /// <summary>
    /// 매개변수 QuestSession의 Register Condition 체크
    /// </summary>
    private void CheckRegisterCondition(QuestListSession questSession)
    {
        QuestListInfo questInfo = FindCurrentSessionQuestListInfo(questSession);

        if (questInfo != null && questInfo.questContain != null && questInfo.questContain.Length > 0)
            for (int i = 0; i < questInfo.questContain.Length; i++)
                questInfo.questContain[i].npcID = npcController.ID;



        if (questInfo != null)
            StartCoroutine(RegisterConditions(questInfo, canGiveQuests));

    }

    private QuestListInfo FindCurrentSessionQuestListInfo(QuestListSession questSession)
    {
        QuestListInfo questInfo = null;
        for (int i = 0; i < questList.questInfos.Length; i++)
            if (questList.questInfos[i].questSession == questSession)
                questInfo = questList.questInfos[i];

        return questInfo;
    }

    private IEnumerator RegisterConditions(QuestListInfo questInfo, List<QuestContainer> questList)
    {
        if (questInfo == null) yield break;
 
        while (doCheckConditions && questInfo != null)
        {
            for (int i = 0; i < questInfo.questContain.Length; i++)
            {
                if (!questList.Contains(questInfo.questContain[i]))  //canGiver에 없을경우
                {
                    if (questInfo.questContain[i].quest.CheckRegisterConditions()) //등록 조건 true면
                    {
                        if (!questInfo.questContain[i].isRepeatQuest && QuestManager.Instance.CanRegistQuest(questInfo.questContain[i].quest))
                        {
                          //  Debug.Log(gameObject?.transform.parent?.parent?.name + " Add : " + questInfo.questContain[i].quest.DisplayName);
                            questList.Add(questInfo.questContain[i]);
                        }

                        if (questInfo.questContain[i].isRepeatQuest && !QuestManager.Instance.ExistActiveQuest(questInfo.questContain[i].quest))
                        {
                          //  Debug.Log(gameObject?.transform.parent?.parent?.name + " Add : " + questInfo.questContain[i].quest.DisplayName);
                            questList.Add(questInfo.questContain[i]);
                        }
                    }
                }
                else      //canGiver에 있을경우
                {

                    if (questInfo.questContain[i].isRepeatQuest && QuestManager.Instance.ExistActiveQuest(questInfo.questContain[i].quest))
                    {
                       // Debug.Log(gameObject?.transform.parent?.parent?.name + " Remove1 : " + questInfo.questContain[i].quest.DisplayName);
                        questList.Remove(questInfo.questContain[i]);
                    }
                    else if (!QuestManager.Instance.CanRegistQuest(questInfo.questContain[i].quest))
                    {
                       // Debug.Log(gameObject?.transform.parent?.parent?.name + " Remove2 : " + questInfo.questContain[i].quest.DisplayName);
                        questList.Remove(questInfo.questContain[i]);
                    }
                }
            }

            // Debug.Log(gameObject?.transform.parent?.parent?.name + " : Can Give Quest : " + canGiveQuests?.Count);
            yield return new WaitForSeconds(conditionCycleRepeatTime);
        }
    }


    public void SetProgressList(QuestContainer questContainer)
    {
        progressQuests.Add(questContainer);
    }
    public void RemoveProgressList(QuestContainer questContainer)
    {
        foreach (QuestContainer questCon in progressQuests.ToArray())
        {
            if (questCon.quest.ID == questContainer.quest.ID)
                progressQuests.Remove(questCon);
        }
    }
    /// <summary>
    /// 현재 퀘스트 알림이 존재하는지 알려준다.
    /// </summary>
    private IEnumerator ShowNotifier(List<QuestContainer> questList)
    {
        if (autoRegisterQuest) yield break;

        //메인 퀘스트가 먼자 알림이뜨고 메인 없으면 서브퀘 알림이 뜸.
        //메인, 서브 관련 sprite는 class 만들어서 타겟마크 처럼.
        // 생각해보니 targetMaterials[] 의 순서를 0번쨰가 가장 우선으로 타겟하는 for문을 하면됨
        // 즉, if(quest.Category == material[0]) 이런식. 
        while (doCheckConditions)
        {
            if (waitForCompleteQuests.Count > 0)
            {
                if (notifierWaitForCompleted != null)
                {
                    renderer.material = notifierWaitForCompleted;
                    renderer.enabled = true;
                }
            }
            else if (questList.Count > 0)
            {
                //함수 만들어서 리스트를 정렬하거나, Main을 먼저 찾고 없으면 sub를 찾는 함수 만듬.
                QuestTargetMaterial findRenderer = FindTargetNotifier(canGiveQuests);
                if (findRenderer == null)
                    renderer.enabled = false;
                else
                {
                    renderer.material = findRenderer.material;
                    renderer.enabled = true;
                }
            }
            else if(progressQuests.Count >0)
            {
                if (notifierProgressing != null)
                {
                    renderer.material = notifierProgressing;
                    renderer.enabled = true;
                }
            }
            else
                renderer.enabled = false;

            yield return new WaitForSeconds(conditionCycleRepeatTime);
        }

    }

    private QuestTargetMaterial FindTargetNotifier(List<QuestContainer> questList)
    {
        for (int i = 0; i < notifierMatertials.Length; i++)
        {
            for (int j = 0; j < questList.Count; j++)
            {
                if (questList[j].quest.Category.CodeName == notifierMatertials[i].category.CodeName)
                    return notifierMatertials[i];
            }
        }
        return null;
    }



    private IEnumerator CheckCanCancelList(List<QuestContainer> questList)
    {
        while (doCheckConditions)
        {
            if (questList.Count > 0)
            {
                for (int i = 0; i < questList.Count; i++)
                    if (questList[i].quest.CheckCancelConditions() && questList[i].quest.cancelConditionCount > 0)
                        questList.Remove(questList[i]);
            }
            yield return new WaitForSeconds(conditionCycleRepeatTime);
        }

    }

}
