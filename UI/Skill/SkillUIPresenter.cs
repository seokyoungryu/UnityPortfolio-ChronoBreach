using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIPresenter : MonoBehaviour
{
    [SerializeField] private SkillDatabase skillDatabase = null;
    [SerializeField] private PlayerStateController playerController = null;
    [SerializeField] private PlayerSkillController playerSkillController = null;

    [SerializeField] private SkillUIContainer skillUiContainer = null;
    [SerializeField] private SkillDetailUI skillDetailUI = null;
    [SerializeField] private RequierdSkillSetting requierdSkillSettingUI = null;


    private void Awake()
    {
        if (playerController == null) playerController = GameManager.Instance.Player;
        if (playerSkillController == null) playerSkillController = GameManager.Instance.Player.skillController;
        if (skillUiContainer == null) skillUiContainer = MainCanvas.Instance.SkillUIContainer;
        if (skillDetailUI == null) skillDetailUI = MainCanvas.Instance.SkillDetailUI;

        for (int i = 0; i < skillUiContainer.SKillUis.Length; i++)
        {
            skillUiContainer.SKillUis[i].onSkillClick += GetPlayerOwnSkillData;
            skillUiContainer.SKillUis[i].onStartDrag += GetPlayerOwnSkillData;
            skillUiContainer.SKillUis[i].getPlayerController += () => { return playerController; };
            skillUiContainer.SKillUis[i].getRequipedSkillSetting += () => { return skillUiContainer.RequierdSkillSettingUI; };

        }
        //skillDetailUI.onInit += SkillDetailUIInit;
        skillDetailUI.onUpdateSlots += UpdateSlotInfos;
        playerController.playerStats.OnUpdateStatInfos_ += UpdateSkillPointText;
        skillDetailUI.onAcceptBtn += UpgradeSkillAccetp_Btn;
        requierdSkillSettingUI.getSkillController += () => { return playerSkillController; };

        GameManager.Instance.onUpdateSkillInfo += UpdateSkillInfos;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < skillUiContainer.SKillUis.Length; i++)
        {
            skillUiContainer.SKillUis[i].onSkillClick -= GetPlayerOwnSkillData;
            skillUiContainer.SKillUis[i].onStartDrag -= GetPlayerOwnSkillData;
            skillUiContainer.SKillUis[i].getPlayerController -= () => { return playerController; };
            skillUiContainer.SKillUis[i].getRequipedSkillSetting -= () => { return skillUiContainer.RequierdSkillSettingUI; };

        }
        //skillDetailUI.onInit += SkillDetailUIInit;
        skillDetailUI.onUpdateSlots -= UpdateSlotInfos;
        playerController.playerStats.OnUpdateStatInfos_ -= UpdateSkillPointText;
        skillDetailUI.onAcceptBtn -= UpgradeSkillAccetp_Btn;
        requierdSkillSettingUI.getSkillController -= () => { return playerSkillController; };

        GameManager.Instance.onUpdateSkillInfo -= UpdateSkillInfos;

    }


    public void UpdateSlotInfos()
    {
        if (skillUiContainer == null)
            skillUiContainer = MainCanvas.Instance.SkillUIContainer;

        skillUiContainer.UpdateSlotInfos();
    }
    public void UpdateSkillPointText(PlayerStatus stats)
    {
        if (skillUiContainer == null)
            skillUiContainer = MainCanvas.Instance.SkillUIContainer;

        skillUiContainer.UpdateSkillPointText(stats);
    }
    public void UpdateSkillInfos(PlayerStateController controller)
    {
        if (skillUiContainer == null)
            skillUiContainer = MainCanvas.Instance.SkillUIContainer;

        if (skillDetailUI == null)
            skillDetailUI = MainCanvas.Instance.SkillDetailUI;

        skillDetailUI.UpdateSkillInfos(controller);
    }

    public SkillData GetPlayerOwnSkillData(int skillID)
    {
        if (playerController == null) playerController = GameManager.Instance.Player;
        if (playerSkillController == null) playerSkillController = GameManager.Instance.Player.skillController;

        SkillData data;
        if (playerSkillController.GetSkilData(skillID) == null)
        {
            data = new SkillData(playerSkillController.SkillDatabase.GetSkillOrigin(skillID));
            return data;
        }
        else
        {
            data = playerSkillController.GetSkilData(skillID);
            return data;
        }

    }


   // public void SkillDetailUIInit()
   // {
   //     Debug.Log("스킬 체크 시작");
   //     skillDetailUI.StartCoroutine(skillDetailUI.CheckCanSkillUpgrade(playerController));
   // }

   
    public void UpgradeSkillAccetp_Btn(SkillDetailUI detailUI)
    {
        //이부분에서 만약 가능하다면 ownList에 먼저 추가하고 해당 클립을 upgrade하기.
        if (detailUI.SelectedSkillClip.CheckCanUpgrade(playerController))
        {
            if (playerSkillController.GetSkilData(detailUI.SelectedSkillClip.ID)?.skillClip == null)
                playerSkillController.AddSkillToOwnSkillList(detailUI.SelectedSkillClip);

            playerController.playerStats.UseSkillPoint(1);
            detailUI.SelectedSkillClip = playerSkillController.GetSkilData(detailUI.SelectedSkillClip.ID).skillClip;
            detailUI.SelectedSkillClip.UpgradeSkill(playerController);
            detailUI.SelectedSkillClip.UpdateUpgradeType();
            detailUI.SelectedSkillClip.CheckCanUpgrade(playerController);
            CommonUIManager.Instance.ExcuteGlobalNotifer("스킬 업그레이드 성공");
            Debug.Log("00000");

        }
        else
        {
            if (detailUI.SelectedSkillClip.CurrentSkillUpgradeType == SkillUpgradeType.LOCK)
                CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("스킬 잠금해제 불가능.");
            else if (detailUI.SelectedSkillClip.CurrentSkillUpgradeType == SkillUpgradeType.UPGRADE)
                CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("스킬 업그레이드 불가능.");
            else if (detailUI.SelectedSkillClip.CurrentSkillUpgradeType == SkillUpgradeType.DONE)
                CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("스킬 최고 레벨.");

            Debug.Log("111111");
        }
        Debug.Log("222222");

        detailUI.UpdateSkillInfos(playerController);
        skillUiContainer.UpdateSlotInfos();
    }


}
