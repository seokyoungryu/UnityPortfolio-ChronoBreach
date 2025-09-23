using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DungeonSuccessScoreUI : DungeonScoreUI
{
    [SerializeField] private RectTransform victoryPanel = null;

    [SerializeField] private ObjectPoolingList itemReward;
    [SerializeField] private Transform itemRootTr = null;
    [SerializeField] private Transform itemObpTr = null;
    [SerializeField] private Vector2 itemSizeDelta = new Vector2(100, 100);
    [SerializeField] private Vector2 itemScale = new Vector2(1.2f,1.2f);
    [SerializeField] private CustomHorizontalLayoutGroup layoutGroup;

    [Header("[0] Gold [1] Reputation [2] Exp [3] SkillPoint [4] itemReward")]
    [SerializeField] private Transform[] infoPanels;
    [SerializeField] private TMP_Text[] infoTexts;

    [Header("[0] GetRewards [1] GoToMain")]
    [SerializeField] private Button[] buttons;

    [SerializeField] private float targetReachTime = 3f;
    [SerializeField] private float countingSoundRepeatTime = 0.1f;
    [SerializeField] private float itemRewardDelayTime = 0.2f;
    [SerializeField] private float itemRewardActiveTime = 0.2f;
    private bool[] countingBools;
    private List<ItemReward> itemRewards = new List<ItemReward>();


    protected override void Start()
    {
        UIHelper.AddEventTrigger(victoryPanel.gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(); });
    }


    public override void ExcuteScoreUI(BaseDungeonTitle dungeonTitle)
    {
        if (MapManager.Instance.CurrentScoreUIType != ScoreUIType.NOT_EXCUTE)
            return;
        base.ExcuteScoreUI(dungeonTitle);
        OnPointerClick();

        StopAllCoroutines();
        StartCoroutine(ScoreProcess_Co());
    }


    private void OnPointerClick()
    {
        CursorManager.Instance.CursorVisible();
        GameManager.Instance.canUseCamera = false;

    }

    private IEnumerator ScoreProcess_Co()
    {
        OnPointerClick();
        GetComponent<RectTransform>().anchoredPosition = excutePosition;

        victoryPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(.5f);
        SoundManager.Instance.PlayExtraSound(UISoundType.VICTORY_SCORE_ENTER);

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = false;

        victoryPanel.gameObject.SetActive(true);

        countingBools = new bool[4];
        DungeonReward reward = currDungeonTitle.DungeonReward;
        for (int i = 0; i < infoPanels.Length; i++)
            infoPanels[i].gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        StartCoroutine(ProcessScoreUp_Co(infoPanels[0], infoTexts[0], reward.Money == null ? 0f : reward.Money.GetIntValue(), 1,0));
        StartCoroutine(ProcessScoreUp_Co(infoPanels[1], infoTexts[1], reward.Reputation == null ? 0f : reward.Reputation.GetIntValue(), 2, 1));
        StartCoroutine(ProcessScoreUp_Co(infoPanels[2], infoTexts[2], reward.Exp == null ? 0f : reward.Exp.GetIntValue(), 3, 2));
        StartCoroutine(ProcessScoreUp_Co(infoPanels[3], infoTexts[3], reward.SkillPoint == null ? 0f : reward.SkillPoint.GetIntValue(), 4, 3));
        StartCoroutine(ProcessItemRewards_Co(reward, 5f));
    }


    private IEnumerator CountingSound(int InfoIndex, float delayStartTime)
    {
        yield return new WaitForSeconds(delayStartTime);

        while (!countingBools[InfoIndex])
        {
            SoundManager.Instance.PlayUISound(UISoundType.SCORE_COUNTING);
            yield return new WaitForSeconds(countingSoundRepeatTime);
        }
    }


    private IEnumerator ProcessScoreUp_Co(Transform container, TMP_Text targetText, float targetValue,  float delayStart, int infoIndex)
    {
        StartCoroutine(CountingSound(infoIndex, delayStart));

        int value = 0;
        float elapsedTime = 0f;
        float addtiveValue = targetValue / targetReachTime;
        targetText.text = "0";

        yield return new WaitForSeconds(delayStart); 

        container.gameObject.SetActive(true);
        SoundManager.Instance.PlayExtraSound(UISoundType.SCORE_TASK_ENTER);

        while (elapsedTime < targetReachTime && value < targetValue)
        {
            if (targetValue > 10)
                value = Mathf.FloorToInt(addtiveValue * elapsedTime);
            else
                value += (int)1;

            if (value > targetValue)
                value = Mathf.FloorToInt(targetValue);

            targetText.text = value.ToString();
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        countingBools[infoIndex] = true;
        targetText.text = targetValue.ToString();
    }


    private IEnumerator ProcessItemRewards_Co(DungeonReward rewards, float delayStart)
    {
        yield return new WaitForSeconds(delayStart);
        SoundManager.Instance.PlayExtraSound(UISoundType.SCORE_TASK_ENTER);
        infoPanels[4].gameObject.SetActive(true);
        CreateItemReward(rewards);

        yield return new WaitForSeconds(itemRewardDelayTime);


        for (int i = 0; i < itemRewards.Count; i++)
        {
            if (itemRewards[i] == null) continue;
            yield return new WaitForSeconds(itemRewardActiveTime);
            SoundManager.Instance.PlayExtraSound(UISoundType.SCORE_TASK_ENTER);
            itemRewards[i].gameObject.SetActive(true);
            layoutGroup.Excute();
        }

        buttons[0].interactable = true;
    }

    private void CreateItemReward(DungeonReward rewards)
    {
        if (rewards.RewardItems.Length > 0)
        {
            for (int i = 0; i < itemRewards.Count; i++)
            {
                itemRewards[i].transform.SetParent(itemObpTr);
                itemRewards[i].SetOBP();
            }
        }

        itemRewards.Clear();


        for (int i = 0; i < rewards.RewardItems.Length; i++)
        {
            ItemReward itemUI = ObjectPooling.Instance.GetOBP(itemReward.ToString()).GetComponent<ItemReward>();
            Item item = rewards.RewardItems[i].rewardItem;
            itemUI.GetComponent<RectTransform>().sizeDelta = itemSizeDelta;
            itemUI.GetComponent<RectTransform>().localScale = itemScale;

            item.itemClip.potentialRank = ItemPotentialRankType.NONE;
            itemUI.Setting(item.itemClip.itemTexture, item, rewards.RewardItems[i].Count);
            itemUI.transform.SetParent(itemRootTr);

            UIHelper.AddEventTrigger(itemUI.gameObject, EventTriggerType.PointerEnter, delegate { OnItemInformationPointerEnter(itemUI.Item); });
            UIHelper.AddEventTrigger(itemUI.gameObject, EventTriggerType.PointerExit, delegate { OnItemInformationPointerExit(itemUI.Item); });

            itemRewards.Add(itemUI);
            itemUI.gameObject.SetActive(false);
        }

        layoutGroup.Excute();
    }

    

    public void GetReward_Btn()
    {
        DungeonReward rewards = currDungeonTitle.DungeonReward;
        if (CommonUIManager.Instance.playerInventory.CheckCanAddItems(rewards.RewardItems))
        {
            rewards.Money.Giver(null);
            rewards.Exp.Giver(null);
            rewards.Reputation.Giver(null);
            for (int i = 0; i < rewards.RewardItems.Length; i++)
                rewards.RewardItems[i].Giver(null);

            buttons[0].interactable = false;
            buttons[1].interactable = true;
        }
    }

    protected override void AllActive(bool active)
    {
        base.AllActive(active);
        victoryPanel.gameObject.SetActive(active);
    }
}
