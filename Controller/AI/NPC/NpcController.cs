using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private int id = -1;
    private NPCQuestGiver npcQuestGiver;
    private QuestReporter questReporter;
    [SerializeField] private QuestTargetMarker targetMarker;
    private SimpleNPCWorldCanvas simpleNpcWorldCanvas;
    private BaseNpcFunction npcFunction;
    private AIController aiController;

    public AIController AiController => aiController;

    public NPCQuestGiver NpcQuestGiver => npcQuestGiver;
    public QuestReporter QuestReporter => questReporter;
    public QuestTargetMarker TargetMarker => targetMarker;
    public BaseNpcFunction NpcFunction => npcFunction;

    public int ID { get { return id; } set { id = value; } }

    #region Events
    public delegate void OnSelectQuest(NpcController npcController);
    public delegate void OnExitNpc();
    public delegate void OnProgressSelectQuest(QuestContainer container, QuestList questList);
    public delegate void OnInteractDialog(DialogFile dialogFile, int dialogIndex, DialogState state);
    public delegate void OnRegisterQuest(NpcController npcController, QuestContainer container);

    public OnExitNpc onExitNpc;
    public OnSelectQuest onCompletedQuest;
    public OnSelectQuest onRegisterSelectQuest;
    public OnProgressSelectQuest onProgressSelectQuest;
    public OnInteractDialog onInteractDialog;
    public OnRegisterQuest onRegisterAutoQuest;

    #endregion

    private void Awake()
    {
        questReporter = GetComponent<QuestReporter>();
        npcQuestGiver = GetComponent<NPCQuestGiver>();
        npcFunction = GetComponent<BaseNpcFunction>();
        aiController = GetComponentInParent<AIController>();
        if (targetMarker == null)
            targetMarker = GetComponentInChildren<QuestTargetMarker>();
        if (simpleNpcWorldCanvas == null)
            simpleNpcWorldCanvas = GetComponentInChildren<SimpleNPCWorldCanvas>();
    }


    public void RegisterCanRegisterQuest(QuestContainer questContainer)
    {
        if (QuestManager.Instance.CanRegistQuest(questContainer.quest))
            onRegisterAutoQuest?.Invoke(this, questContainer);
    }

}
