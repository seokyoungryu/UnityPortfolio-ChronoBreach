using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class NoramlExcutePracticeModeUI : MonoBehaviour
{
    [SerializeField] private BasePracticeModeProcess process = null;
    [SerializeField] private GameObject optionPanel = null;
    [SerializeField] private GameObject optionBackgroundPanel = null;

    [SerializeField] private TMP_Text enemyCount_text = null;
    [SerializeField] private TMP_Text scareCrowCount_text = null;
    [SerializeField] private TMP_Text shooterCount_text = null;
    [SerializeField] private TMP_Text standingBossCount_text = null;
    [SerializeField] private TMP_Text playerLvValue_text = null;
    [SerializeField] private TMP_Text goldValue_text = null;


    [SerializeField] private Animator optionAnim = null;
    [SerializeField] private TMP_Text option_text = null;

    [SerializeField] private bool isActiveOption = false;

    [SerializeField] private List<UISoundSetting> soundSettingInfos = new List<UISoundSetting>();


    private void Awake()
    {
        UIHelper.AddEventTrigger(optionPanel, EventTriggerType.PointerEnter, delegate { MouseEnterToUI(); });
        UIHelper.AddEventTrigger(optionPanel, EventTriggerType.PointerExit, delegate { MouseExitToUI(); });

        UIHelper.AddEventTrigger(optionBackgroundPanel, EventTriggerType.PointerEnter, delegate { MouseEnterToUI(); });
        UIHelper.AddEventTrigger(optionBackgroundPanel, EventTriggerType.PointerExit, delegate { MouseExitToUI(); });

        for (int i = 0; i < soundSettingInfos.Count; i++)
        {
            for (int x = 0; x < soundSettingInfos[i].TargetGos.Length; x++)
            {
                if (soundSettingInfos[i].TargetGos[x] != null)
                {
                    UISoundType type = soundSettingInfos[i].UISoundType;
                    UIHelper.AddEventTrigger(soundSettingInfos[i].TargetGos[x], soundSettingInfos[i].Type, delegate { SoundManager.Instance.PlayUISound(type); });
                }
            }
        }
    }


    public void ActiveOption()
    {
        isActiveOption = !isActiveOption;
        if (isActiveOption)
        {
            option_text.text = "可记 摧扁";
            optionAnim.Play("PracticeOptionOpen");
        }
        else
        {
            option_text.text = "可记 凯扁";
            optionAnim.Play("PracticeOptionClose");
        }
        MouseEnterToUI();
    }


    private void MouseEnterToUI()
    {
        GameManager.Instance.canUseCamera = false;
        GameManager.Instance.Player.Conditions.CanAttack = false;
    }


    private void MouseExitToUI()
    {
        GameManager.Instance.Player.Conditions.CanAttack = true;
    }

    public void UpdateEnemy(BasePracticeModeProcess process)
    {
        enemyCount_text.text = process.CurrentEnemyAiCount.ToString();
    }

    public void UpdateScareCrow(BasePracticeModeProcess process)
    {
        scareCrowCount_text.text = process.CurrentScareCrowsCount.ToString();
    }

    public void UpdateShooter(BasePracticeModeProcess process)
    {
        shooterCount_text.text = process.CurrentShooterCount.ToString();
    }

    public void UpdateLv(BasePracticeModeProcess process)
    {
        playerLvValue_text.text = process.CurrentPlayerLevelUpValue.ToString();
    }
    public void UpdateStandingBoss(BasePracticeModeProcess process)
    {
        standingBossCount_text.text = process.CurrentStandingBossCount.ToString();
    }

    public void UpdateGold(BasePracticeModeProcess process)
    {
        goldValue_text.text = process.CurrentGoldValue.ToString();
    }
    public void GoToMain()
    {
        SoundManager.Instance.PlayBGM_CrossFade(SoundManager.Instance.MainSceneBGM,2.5f);
        process.AllReset();

        CommonUIManager.Instance.playerInventory.LoadInventorys();
        CommonUIManager.Instance.quickSlotSaveDt.Load();

        ScenesManager.Instance.OnExcuteAfterLoading = () => SaveManager.Instance.PracticeLoad();
      //  ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Player.skillController.ResetSkillCoolTime();

        ScenesManager.Instance.ChangeScene(1);
    }
}
