using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DungeonDetailInfoUI : UIRoot
{
    [SerializeField] private AllDungeonTitlesDatabase dungeonDatabase = null;

    [Header("Item Rewards")]
    [SerializeField] private string itemRewardImageOBP= string.Empty;
    [SerializeField] private Transform itemRewardContainer = null;
    [SerializeField] private Transform itemRewardOBPoriginTr= null;

    [Header("Conditions")]
    [SerializeField] private string detailConditionTask = string.Empty;
    [SerializeField] private Transform detailConditionContainer = null;
    [SerializeField] private Transform detailOBPOritinTr = null;

    [Header("Child Transform")]
    [SerializeField] private Transform noneBackgroundUI = null;
    [SerializeField] private Transform detailUIs = null;

    [Header("[0] : Money, [1] : SkillPoint, [2] : Exp, [3] : Reputation")]
    [SerializeField] private TMP_Text[] rewards_text = null;
    [SerializeField] private Transform[] rewards_Tr = null;
    [SerializeField] private TMP_Text title_Text = null;

    [Header("[0] : Title, [1] : Target,")]
    [SerializeField] private TMP_Text[] detail_Texts = null;

    [Header("Button")]
    [SerializeField] private Button entry_Btn = null;

    [SerializeField] private CustomHorizontalLayoutGroup itemDataLayout = null;
    [SerializeField] private CustomHorizontalLayoutGroup itemRewardLayout = null;
    [SerializeField] private CustomVerticalLayoutGroup conditionLayout = null;

    private bool isOpenDetailWindow = false;
    private BaseDungeonTitle currentTitle = null;
    [SerializeField] private DungeonDetailConditionTask[] conditions;
    [SerializeField] private List<ItemReward> itemRewards = new List<ItemReward>();


    protected override void Awake()
    {
        base.Awake();
        if (itemDataLayout == null)
            itemDataLayout = GetComponentInChildren<CustomHorizontalLayoutGroup>();
        if (conditionLayout == null)
            conditionLayout = GetComponentInChildren<CustomVerticalLayoutGroup>();

    }
    public void SettingInfos(BaseDungeonTitle title)
    {
        if (title == null)
        {
            detailUIs.gameObject.SetActive(false);
            noneBackgroundUI.gameObject.SetActive(true);
            return;
        }
        else
            detailUIs.gameObject.SetActive(true);



        currentTitle = title;
        Debug.Log("이거 실행!!!!!!! : " + currentTitle.name);
        title_Text.text = currentTitle.TaskTarget.DisplayName;
        DungeonReward reward = currentTitle.DungeonReward;

        if (reward.Money != null) SettingReward(rewards_Tr[0], rewards_text[0], reward.Money.GetIntValue());
        else SettingReward(rewards_Tr[0], rewards_text[0], 0);
        if (reward.SkillPoint != null) SettingReward(rewards_Tr[1], rewards_text[1], reward.SkillPoint.GetIntValue());
        else SettingReward(rewards_Tr[1], rewards_text[1], 0);
        if (reward.Exp != null) SettingReward(rewards_Tr[2], rewards_text[2], reward.Exp.GetIntValue());
        else SettingReward(rewards_Tr[2], rewards_text[2], 0);
        if (reward.Reputation != null) SettingReward(rewards_Tr[3], rewards_text[3], reward.Reputation.GetIntValue());
        else SettingReward(rewards_Tr[3], rewards_text[3], 0);

        SettingItemReward(reward);
        SettingConditions();

        detail_Texts[0].text = currentTitle.TaskTarget.DisplayName;
        detail_Texts[1].text = currentTitle.TargetString;

        conditionLayout.Excute();
        itemDataLayout.Excute();
        itemRewardLayout.Excute();
        StartCoroutine(CheckCanEntryDungeon());
    }


    private IEnumerator CheckCanEntryDungeon()
    {
        isOpenDetailWindow = true;

        while (isOpenDetailWindow)
        {
            if (conditions != null || conditions.Length > 0)
                for (int i = 0; i < conditions.Length; i++)
                    ChangeTextColor(conditions[i]);

            CheckAndChangeEntryBtn();
            yield return new WaitForSeconds(0.2f);
        }
    }


    private void ChangeTextColor(DungeonDetailConditionTask task)
    {
        if (MapManager.Instance.IgnoreEntryConditions)
        {
            task.ChangeUnLock();
            return;
        }

        PlayerStatus stats = GameManager.Instance.Player.playerStats;

        switch(task.ConditionType)
        {
            case DetailConditionType.LV:
                if (stats.Level < task.ConditionValue) task.ChangeLock();
                else task.ChangeUnLock();
                break;
            case DetailConditionType.DAMAGED:
                if (stats.GetMinDamage(false) < task.ConditionValue) task.ChangeLock();
                else task.ChangeUnLock();
                break;
            case DetailConditionType.REPUTATION:
                if (GameManager.Instance.Reputation< task.ConditionValue) task.ChangeLock();
                else task.ChangeUnLock();
                break;
            case DetailConditionType.TITLE:
                if (!task.ConditionTitle.IsDungeonClear) task.ChangeLock();
                else task.ChangeUnLock();
                break;
        }
    }

    private bool CheckAndChangeEntryBtn()
    {
        if (MapManager.Instance.IgnoreEntryConditions)
        {
            entry_Btn.interactable = true;
            return true;
        }
        for (int i = 0; i < conditions.Length; i++)
        {
            if (!conditions[i].IsUnLock)
            {
                entry_Btn.interactable = false;
                return false;
            }
        }
        entry_Btn.interactable = true;
        return true;
    }

    private void SettingReward(Transform rewardTr,TMP_Text rewardText, int value)
    {
        if (value <= 0)
            rewardTr.gameObject.SetActive(false);
        else
        {
            rewardTr.gameObject.SetActive(true);
            rewardText.text = value.ToString();
        }
    }
    private void SettingItemReward(DungeonReward rewards)
    {
        if (itemRewards.Count > 0)
        {
            for (int i = 0; i < itemRewards.Count; i++)
            {
                itemRewards[i].transform.SetParent(itemRewardOBPoriginTr);
                itemRewards[i].SetOBP();
            }
        }

        itemRewards.Clear();
        if (rewards.RewardItems == null || rewards.RewardItems.Length <= 0)
            return;

        for (int i = 0; i < rewards.RewardItems.Length; i++)
        {
            if (rewards.RewardItems[i] == null || rewards.RewardItems[i].rewardItem == null) continue;
            ItemReward itemUI = ObjectPooling.Instance.GetOBP(itemRewardImageOBP).GetComponent<ItemReward>();
            Item item = new Item(ItemManager.Instance.GetItemClip((int)rewards.RewardItems[i].ItemList));
            Debug.Log("던전 아이템 : " + item.itemClip.name + " - " + rewards.RewardItems[i].ItemList.ToString());
            item.itemClip.potentialRank = ItemPotentialRankType.NONE;
            itemUI.Setting(item.GetItemImage(), item, rewards.RewardItems[i].Count);
            itemUI.transform.SetParent(itemRewardContainer);

            UIHelper.AddEventTrigger(itemUI.gameObject, EventTriggerType.PointerEnter, delegate { OnItemInformationPointerEnter(itemUI.Item); });
            UIHelper.AddEventTrigger(itemUI.gameObject, EventTriggerType.PointerExit, delegate { OnItemInformationPointerExit(itemUI.Item); });

            itemRewards.Add(itemUI);
        }
    }
    private void SettingConditions()
    {
        if (conditions != null && conditions.Length > 0)
        {
            foreach (DungeonDetailConditionTask task in conditions)
            {
                task.transform.SetParent(detailOBPOritinTr);
                task.SetOBP();
            }
        }

        conditions = currentTitle.DungeonCondition.CreateDungeonConditionUIs(detailConditionTask);
        if (conditions == null || conditions.Length <= 0) return;

        foreach (DungeonDetailConditionTask task in conditions)
            task.transform.SetParent(detailConditionContainer);


        detailConditionContainer.GetComponentInChildren<CustomVerticalLayoutGroup>()?.Excute();
        
    }


}
