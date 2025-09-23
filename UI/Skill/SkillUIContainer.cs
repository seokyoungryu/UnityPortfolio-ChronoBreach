using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillUIContainer : UIRoot
{
    [SerializeField] private SkillDetailUI skillDetailUI = null;
    [SerializeField] private RequierdSkillSetting requierdSkillSettingUI = null;
    [SerializeField] private TMP_Text skillPoint_Text = null;

    [Header("SKill Category")]
    [Header("[0] :  [1] :  [2] : ")]
    [SerializeField] private Transform[] category = null;
    [SerializeField] private SkillUI[] skillUis = null;

    [SerializeField] private int currentCategoryIndex = 0;
    public SkillUI[] SKillUis => skillUis;
    public RequierdSkillSetting RequierdSkillSettingUI => requierdSkillSettingUI;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < category.Length; i++)
        {
            int index = i;
            UIHelper.AddEventTrigger(category[i].gameObject, EventTriggerType.PointerClick, delegate { OpenCategory(skillUis[index].gameObject, index); });
            skillUis[i].SkillUIIndex = index;
        }

       // UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerEnter, delegate { OnUIRootEnter(gameObject); });
       // UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerExit, delegate { OnUIRootExit(gameObject); });
    }


    protected override void Start()
    {
        base.Start();
    }


    public override void OnUIRootEnter(GameObject go)
    {
        MouseUIData.enterUIRoot = go?.GetComponent<SkillUIContainer>();
        Debug.Log("Enter SkillCon : " + go.name);
    }

    public void UpdateSkillPointText(PlayerStatus playerStatus)
    {
        skillPoint_Text.text = playerStatus.RemainingSkillPoint.ToString();
    }

    public void UpdateSlotInfos()
    {
        for (int i = 0; i < skillUis.Length; i++)
        {
            skillUis[i].UpdateSkillSlotInfo();
        }
    }

    public override void StartResetActive()
    {
        base.StartResetActive();
        CloseAllCategory();
        skillUis[0].gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public override void OpenUIWindow()
    {
        GameManager.Instance.UpdateSkillInfo();
        UpdateSkillPointText(GameManager.Instance.Player.playerStats);
        requierdSkillSettingUI.InitSlotsSetting();
        UpdateSlotInfos();
        SetFirstSkillToDetailUI(0);
        base.OpenUIWindow();
    }

    public override void CloseUIWindow()
    {
        CloseAllCategory();
        // requierdSkillSettingUI.gameObject.SetActive(false);
        skillUis[0].gameObject.SetActive(true);
        GameManager.Instance.UpdateSkillInfo();
        base.CloseUIWindow();
    }

    private void CloseAllCategory()
    {
        for (int i = 0; i < skillUis.Length; i++)
            skillUis[i].gameObject.SetActive(false);
    }

    private void OpenCategory(GameObject go, int categoryIndex)
    {
       // if (currentCategoryIndex == categoryIndex) return;

        currentCategoryIndex = categoryIndex;
        CloseAllCategory();
        go.SetActive(true);
        GameManager.Instance.UpdateSkillInfo();
        SetFirstSkillToDetailUI(categoryIndex);
    }

    private void SetFirstSkillToDetailUI(int categoryIndex)
    {
        SkillData data = skillUis[categoryIndex].GetPlayerSkillData(skillUis[categoryIndex].Skills[0].ID);
        if (data != null)
            skillDetailUI.ClickSetting(data.skillClip);
        else
        {
            Debug.Log("½ºÅ³ NULL : " + skillUis[categoryIndex].Skills[0]);
            skillDetailUI.ClickSetting(skillUis[categoryIndex].Skills[0]);
        }

    }
}
