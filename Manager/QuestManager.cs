using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class QuestManager : Singleton<QuestManager>
{
    public QuestListSession currentQuestSession = QuestListSession.MAIN1;

    public Quest currentClickQuest = null;

    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();
    public List<Quest> repeatQuests = new List<Quest>();
    public QuestDatabase questDatabase;
    public QuestListDatabase questListDatabase;
    public QuestCategoryDefineDatabase categoryDefineDatabase;
    public QuestSelectionPresenter questSelectionPresenter;


    public bool isDialoging = false;

    private List<NpcController> npcControllers = new List<NpcController>();
    private List<StoreNpcFunction> npcStoreFunction = new List<StoreNpcFunction>();
    public List<NpcController> NpcControllers => npcControllers;
    public List<StoreNpcFunction> NpcStoreFunction => npcStoreFunction;
    #region SavePath
    public const string activeQuestSavePath = "ActiveQuest";
    public const string completeQuestSavePath = "CompleteQuest";
    public const string repeatQuestSavePath = "RepeatQuest";
    public const string rootSavePath = "QuestManager";

    #endregion

    #region Event
    public delegate void OnRegisterQuest(Quest quest, Task task = null);
    public delegate void OnCompleteQuest(Quest quest);
    public delegate void OnRepeatQuest(Quest quest);
    public delegate void OnNewNpcControllerCallback(NpcController contr);
    public delegate void OnNewNpcStoreFuncCallback(StoreNpcFunction func);
    public delegate void OnDelete();


    public event OnRegisterQuest onRegisterLoadQuest;
    public event OnRegisterQuest onRegister;
    public event OnDelete onDelete;
    public event OnDelete onLoadTemp;


    public event OnCompleteQuest onComplete;
    public event OnRepeatQuest onRepeat;
    public event OnNewNpcControllerCallback onSetNewNpcContrCallback;
    public event OnNewNpcStoreFuncCallback onSetNewNpcStoreFuncCallback;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        npcControllers = new List<NpcController>(FindObjectsOfType<NpcController>());
    }


    public void SetNewNpcController(NpcController npcContr)
    {
        if (npcContr == null) return;
        if (npcControllers == null) npcControllers = new List<NpcController>();
        if (questSelectionPresenter == null) questSelectionPresenter = FindObjectOfType<QuestSelectionPresenter>();

        npcControllers.Add(npcContr);
       // Debug.Log("NPC 등록!");

        // onSetNewNpcContrCallback?.Invoke(npcContr);
        questSelectionPresenter.SetNpcController(npcContr);

    }

    public void SetNewNpcStoreFunction(StoreNpcFunction npcStoreFunc)
    {
        if (npcStoreFunc == null) return;
        if (npcStoreFunction == null) npcStoreFunction = new List<StoreNpcFunction>();

        npcStoreFunction.Add(npcStoreFunc);
          onSetNewNpcStoreFuncCallback?.Invoke(npcStoreFunc);
    }

    public void RemoveActiveQuest(Quest quest)
    {
        foreach (Quest q in activeQuests.ToArray())
            if (q.CodeName == quest.CodeName)
                activeQuests.Remove(quest);
    }

    public NpcController FindNpcController(int npcID)
    {
        foreach (NpcController npc in npcControllers)
        {
            if (npc.ID == npcID)
                return npc;
        }
        return null;
    }

    public Quest Register(Quest quest, NPCQuestGiver giver,QuestContainer questContain, bool isLoadQuest = false)
    {
        if (questSelectionPresenter == null) questSelectionPresenter = FindObjectOfType<QuestSelectionPresenter>();

        if (!CanRegistQuest(quest))
            return null;

        Quest clone = quest.Clone();
        activeQuests.Add(clone);
        clone.OnComplete_ += OnComplete;
        clone.QuestListSession = giver != null ?  giver.CurrentQuestListSession : QuestListSession.MAIN1;
        clone.onCompleteDialog += questSelectionPresenter.QuestSelectionViewer.OnEndQuest;
        clone.Register(giver, questContain, isLoadQuest);
        SoundManager.Instance.PlayUISound(UISoundType.QUEST_REGISTER);
        //Debug.Log("등록1 : " + giver?.GetComponent<NpcController>()?.ID);

        if (!isLoadQuest)
            onRegister?.Invoke(clone);
       // Debug.Log("등록2 : " + giver?.GetComponent<NpcController>()?.ID);

        //여기서 퀘스트창  active에 등록.
        return clone;
    }


    public void ReceiveReport(string questCategoryDefine, object target, int successCount)
    {
        QuestCategory category = categoryDefineDatabase.GetCategory(questCategoryDefine);
        ReceiveReport(activeQuests, category, target, successCount);
        //Debug.Log("Recive :" + category.CodeName + " : " + target + " : " + successCount);

    }

    public void ReceiveReport(QuestCategory category, object target, int successCount)
    {
        //Debug.Log("Recive! : " + category.CodeName + " : " + target.ToString());
        ReceiveReport(activeQuests, category, target, successCount);
    }

    public void ReceiveReport(QuestCategory category, TaskTarget target, int successCount)
        => ReceiveReport(category, target.Value, successCount);


    public void ReceiveReport(List<Quest> questList,QuestCategory category, object target, int successCount)
    {
        foreach (Quest quest in questList.ToArray())
            quest.ReceiveReport(category, target, successCount);

    }

    public bool ExistActiveQuest(Quest quest)
    {
        foreach (Quest q in activeQuests.ToArray())
            if (quest.CodeName == q.CodeName)
                return true;

        return false;
    }

    public bool ExistCompleteQuest(Quest quest)
    {
        if (quest == null) return false;

        foreach (Quest q in completedQuests.ToArray())
            if (quest.CodeName == q.CodeName)
                return true;

        return false;
    }

    public bool ExistRepeatQuest(Quest quest)
    {
        foreach (Quest q in repeatQuests.ToArray())
            if (quest.CodeName == q.CodeName)
                return true;

        return false;
    }

    public void RemoveInCompletedQuests(Quest quest)
    {
        foreach (Quest q in completedQuests.ToArray())
        {
            if (quest.CodeName == q.CodeName)
            {
                completedQuests.Remove(q);
                return;
            }
        }
    }

    public bool CanRegistQuest(Quest quest)
    {
        bool canRegist = true;
        foreach (Quest activeQuest in activeQuests)
            if (activeQuest.CodeName == quest.CodeName)
                canRegist = false;

        foreach (Quest completeQuest in completedQuests)
            if (completeQuest.CodeName == quest.CodeName)
                canRegist = false;

       // Debug.Log("Cangive " + canRegist + " : " + quest.CodeName);
        return canRegist;
    }


    public Quest FindInActiveQuest(Quest quest)
    {
        foreach (Quest active in activeQuests)
            if (active.CodeName == quest.CodeName)
                return active;
        return null;
    }

    public bool TargetTaskInActiveQuests(QuestCategory category, TaskTarget target)
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.CheckHaveTargetTask(category, target))
                return true;
        }
        return false;
    }


    #region Save Load

    public void Clear()
    {
      //  if (questNotifierView == null)
      //      questNotifierView = FindObjectOfType<QuestNotifierView>();
      //
      //  questNotifierView.DestroyAll();
        activeQuests.Clear();
        completedQuests.Clear();
        repeatQuests.Clear();
    }

    public void Save()
    {
        JObject root = new JObject();
        int index = SaveManager.Instance.SaveSlotIndex;
        root.Add(activeQuestSavePath + index, CreateQuestSaveDataToJsonFile(activeQuests));
        root.Add(completeQuestSavePath + index, CreateQuestSaveDataToJsonFile(completedQuests));
        root.Add(repeatQuestSavePath + index, CreateQuestSaveDataToJsonFile(repeatQuests));

        PlayerPrefs.SetString(rootSavePath + index, root.ToString());
        PlayerPrefs.Save();
        Debug.Log("퀘스트 세이브");

    }

    public void SaveTemp(int tempIndex)
    {
        JObject root = new JObject();
        int index = SaveManager.Instance.SaveSlotIndex;
        root.Add("TempActiveQuest" + tempIndex, CreateQuestSaveDataToJsonFile(activeQuests));
        root.Add("TempCompleteQuest" + tempIndex, CreateQuestSaveDataToJsonFile(completedQuests));
        root.Add("TempRepeatQuest" + tempIndex, CreateQuestSaveDataToJsonFile(repeatQuests));

        PlayerPrefs.SetString("TempRootQuest" + tempIndex, root.ToString());
        PlayerPrefs.Save();
    }

    public bool LoadTemp(int tempIndex)
    {
        if (PlayerPrefs.HasKey("TempRootQuest" + tempIndex))
        {
            onDelete?.Invoke();
            Clear();
            JObject jObject = JObject.Parse(PlayerPrefs.GetString("TempRootQuest" + tempIndex));
            LoadSaveData(jObject["TempActiveQuest" + tempIndex], questDatabase, LoadActiveQuests);
            LoadSaveData(jObject["TempCompleteQuest" + tempIndex], questDatabase, LoadCompleteQuests);
            LoadSaveData(jObject["TempRepeatQuest" + tempIndex], questDatabase, LoadRepeatQuests);
            onLoadTemp?.Invoke();
            return true;
        }
        else
            return false;
    }


    public bool Load()
    {
        int index = SaveManager.Instance.SaveSlotIndex;

        if (PlayerPrefs.HasKey(rootSavePath + index))
        {
            JObject jObject = JObject.Parse(PlayerPrefs.GetString(rootSavePath + index));
            LoadSaveData(jObject[activeQuestSavePath + index], questDatabase, LoadActiveQuests);
            LoadSaveData(jObject[completeQuestSavePath + index], questDatabase, LoadCompleteQuests);
            LoadSaveData(jObject[repeatQuestSavePath + index], questDatabase, LoadRepeatQuests);
            Debug.Log("퀘스트 로드");
            return true;
        }
        else
        {
            Debug.Log("퀘스트 로드 안뎀.");
            return false;
        }
    }

    public void DeleteSaveData(int saveSlotIndex)
    {
        bool isHave = false;
        if (PlayerPrefs.HasKey(rootSavePath + saveSlotIndex))
        {
            isHave = true;
            PlayerPrefs.DeleteKey(rootSavePath + saveSlotIndex);
            
        }
        if (PlayerPrefs.HasKey(activeQuestSavePath + saveSlotIndex))
            PlayerPrefs.DeleteKey(activeQuestSavePath + saveSlotIndex);
        if (PlayerPrefs.HasKey(completeQuestSavePath + saveSlotIndex))
            PlayerPrefs.DeleteKey(completeQuestSavePath + saveSlotIndex);
        if (PlayerPrefs.HasKey(repeatQuestSavePath + saveSlotIndex))
            PlayerPrefs.DeleteKey(repeatQuestSavePath + saveSlotIndex);
        if (isHave)
            PlayerPrefs.Save();
    }

    public JArray CreateQuestSaveDataToJsonFile(List<Quest> quests)
    {
        JArray jArray = new JArray();

        foreach (var quest in quests)
        {
            jArray.Add(JObject.FromObject(quest.ToSaveData()));
        }
        return jArray;
    }

    public void LoadSaveData(JToken datasToken, QuestDatabase database, System.Action<QuestSaveData, Quest> onSucess)
    {
        JArray jArray = datasToken as JArray;
        foreach (var data in jArray)
        {
            QuestSaveData saveData = data.ToObject<QuestSaveData>();
            Quest quest = database.FindQuestByCodeName(saveData.codeName);
            onSucess.Invoke(saveData, quest);
        }

    }

    private void LoadActiveQuests(QuestSaveData saveData, Quest quest)
    {
        Quest activeQuest;
        if (!activeQuests.Contains(quest)) activeQuest = Register(quest, quest.Giver, quest.QuestContainer, true);
        else activeQuest = FindInActiveQuest(quest);

        if (activeQuest == null) return;

        activeQuest.LoadData(saveData);
        onRegisterLoadQuest?.Invoke(activeQuest);
        activeQuest.UpdateTask();
        activeQuest.CheckIsWaitForCompleted();
        Debug.Log("로드 퀘스트 : " + activeQuest.DisplayName);
       // onRegister?.Invoke(activeQuest);
    }

    public void UpdateQuests()
    {
        for (int i = 0; i < activeQuests.Count; i++)
        {

        }
    }

    private void LoadCompleteQuests(QuestSaveData saveData, Quest quest)
    {
        if (!completedQuests.Contains(quest))
        {
            var newQuest = quest.Clone();
            newQuest.LoadData(saveData);
            completedQuests.Add(newQuest);
        }
    }

    private void LoadRepeatQuests(QuestSaveData saveData, Quest quest)
    {
        var newQuest = quest.Clone();
        newQuest.LoadData(saveData);
        repeatQuests.Add(newQuest);
    }

    public QuestList FindQuestList(int listID)
    {
        for (int i = 0; i < questListDatabase.database.Count; i++)
        {
            if (questListDatabase.database[i].ID == listID)
                return questListDatabase.database[i] as QuestList;
        }
        return null;
    }

    #endregion


    #region Callback

    private void OnComplete(Quest quest)
    {
        activeQuests.Remove(quest);
        SoundManager.Instance.PlayUISound(UISoundType.QUEST_COMPLETE);

        if (quest.QuestContainer.isRepeatQuest)
        {
            if (!ExistRepeatQuest(quest))
                repeatQuests.Add(quest);
            else
                foreach (Quest q in repeatQuests)
                    if (q.CodeName == quest.CodeName)
                        q.repeatCount += 1;
            onRepeat?.Invoke(quest);
        }
        else if (!quest.QuestContainer.isRepeatQuest)
        {
            completedQuests.Add(quest);
            onComplete?.Invoke(quest);
        }

    }
    #endregion

}
