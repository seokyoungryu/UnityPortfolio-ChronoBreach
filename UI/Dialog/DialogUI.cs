using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogUI : MonoBehaviour
{
    private QuestList questList = null;
    [SerializeField] private QuestContainer currentQuestContainer = null;
    [SerializeField] private bool isFinalDialog = false;
    [Header("NPC Dialog")]
    [SerializeField] private Transform npcDIalog = null;
    [SerializeField] private TMP_Text npcName_Text = null;
    [SerializeField] private TMP_Text npcDialog_Text = null;

    [Header("Player Dialog")]
    [SerializeField] private Transform playerDIalog = null;
    [SerializeField] private TMP_Text playerName_Text = null;
    [SerializeField] private TMP_Text playerDialog_Text = null;

    [Header("Button")]
    [SerializeField] private Transform registerButton_Panel;
    [SerializeField] private Transform forceRegisterButton_Panel;
    [SerializeField] private Transform completeButton_Panel;
    [SerializeField] private Transform progressButton_Panel;
    [SerializeField] private Transform[] containers;
    [SerializeField] private Transform playerContinueImg;
    [SerializeField] private Transform npcContinueImg;

    private AIController currentDialogAiContr = null;
    private bool isForceRegiser = false;


    public QuestList QuestList => questList;
    public QuestContainer CurrentQuestContainer => currentQuestContainer;
    #region Event
    public delegate void OnStartDialog(DialogUI dialogUI);
    public delegate void OnClickDialog();
    public delegate void OnAcceptDialog(DialogUI dialogUI);
    public delegate void OnInteractDialog(DialogUI dialogUI, DialogFile dialogFile, int dialogIndex, DialogState state);

    public event OnStartDialog onStartDialog;
    public event OnStartDialog onEndDialog;
    public event OnStartDialog onProgressDialog;
    public event OnAcceptDialog onAcceptDialog;
    public event OnAcceptDialog onCompleteDialog;
    public event OnClickDialog onClickDialog;
    public event OnClickDialog onInteractReset;
    public event OnInteractDialog onInteractDialog;
    public event OnClickDialog onDialogProcessInit;

    #endregion

    private void Awake()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].gameObject.SetActive(false);


        playerContinueImg.gameObject.SetActive(false);
        npcContinueImg.gameObject.SetActive(false);
    }


    public void InitStart(QuestSelectionViewer questSelectionViewer)
    {
       // Debug.Log("InitStart");
        InitSetting(questSelectionViewer.SelectedQuest, questSelectionViewer.QuestList);
        onStartDialog?.Invoke(this);
    }

    public void InitEnd(QuestSelectionViewer questSelectionViewer)
    {
       // Debug.Log("InitEnd : " + questSelectionViewer.SelectedQuest + "," + questSelectionViewer.QuestList);
        InitSetting(questSelectionViewer.SelectedQuest, questSelectionViewer.QuestList);
        onEndDialog?.Invoke(this);
    }

    public void InitEnd(QuestContainer questContainer, QuestList questList)
    {
       // Debug.Log("InitEnd : " + questContainer + " , "  + questContainer.quest + " , " + questList);
        InitSetting(questContainer, questList);
        onEndDialog?.Invoke(this);
    }

    public void InitProgress(QuestSelectionViewer questSelectionViewer)
    {
       // Debug.Log("InitProgress1");
        InitSetting(questSelectionViewer.SelectedQuest, questSelectionViewer.QuestList);
        onProgressDialog?.Invoke(this);
    }
    public void InitProgress(QuestContainer questContainer, QuestList questList)
    {
       // Debug.Log("InitProgress2");
        InitSetting(questContainer, questList);
        onProgressDialog?.Invoke(this);
    }

    public void InitDialog(QuestContainer questContainer, QuestList questList)
    {
       // Debug.Log("InitDialog1");
        InitSetting(questContainer, questList);
        onProgressDialog?.Invoke(this);
    }

    public void InitInteractDialog(DialogFile dialogFile, int dialogIndex, DialogState state)
    {
      //  Debug.Log("InitInteractDialog1");
        QuestManager.Instance.isDialoging = true;
        InitSetting(dialogFile);
        onInteractDialog?.Invoke(this , dialogFile,dialogIndex, state);
    }

    public void InitForceRegister(QuestSelectionViewer questSelectionViewer, QuestContainer container)
    {
        InitSetting(container, questSelectionViewer.QuestList);
        isForceRegiser = true;
        onStartDialog?.Invoke(this);
    }

    private void InitSetting(QuestContainer questContainer, QuestList questList)
    {
        QuestManager.Instance.isDialoging = true;
        currentQuestContainer = questContainer;
        isFinalDialog = false;
        isForceRegiser = false;
        this.questList = questList;
        UIHelper.AddEventTrigger(npcDIalog.GetChild(0).gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(); });
        UIHelper.AddEventTrigger(playerDIalog.GetChild(0).gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(); });
        ResetPanels();
     //   Debug.Log("Init Setting : " + this.questList + " - " + questList);
    }

    private void InitSetting(DialogFile dialogFile)
    {
        isFinalDialog = false;
        isForceRegiser = false;
        UIHelper.AddEventTrigger(npcDIalog.GetChild(0).gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(); });
        UIHelper.AddEventTrigger(playerDIalog.GetChild(0).gameObject, EventTriggerType.PointerClick, delegate { OnPointerClick(); });
        ResetPanels();
    }

    public void UpdateDialog(bool isPlayer, string name, string sentence, bool isFinalSentence, DialogState state, bool isFinish)
    {
        npcDIalog.gameObject.SetActive(false);
        playerDIalog.gameObject.SetActive(false);
        playerContinueImg.gameObject.SetActive(false);
        npcContinueImg.gameObject.SetActive(false);

        if (isPlayer)
        {
            playerDIalog.gameObject.SetActive(true);
            playerName_Text.text = name;
            playerDialog_Text.text = sentence;

            if (isFinish && !isFinalSentence) playerContinueImg.gameObject.SetActive(true);
        }
        else
        {
            npcDIalog.gameObject.SetActive(true);
            npcName_Text.text = name;
            npcDialog_Text.text = sentence;

            if (isFinish && !isFinalSentence) npcContinueImg.gameObject.SetActive(true);
        }

        if (isFinalSentence)
            SoundManager.Instance.PlayUISound(UISoundType.DONE_DIALOG);

        if (isForceRegiser)
        {
            if (isFinalSentence && state == DialogState.START)
                forceRegisterButton_Panel.gameObject.SetActive(true);
        }
        else
        {
            if (isFinalSentence && state == DialogState.START)
                registerButton_Panel.gameObject.SetActive(true);
            else if (isFinalSentence && state == DialogState.END)
                completeButton_Panel.gameObject.SetActive(true);
            else if (isFinalSentence && state == DialogState.PROGRESS)
                progressButton_Panel.gameObject.SetActive(true);
            else if (isFinalSentence && state == DialogState.INTERACT)
                progressButton_Panel.gameObject.SetActive(true);
            else if (isFinalSentence && state == DialogState.ETC)
                progressButton_Panel.gameObject.SetActive(true);
        }


    }



    public void RegisterAccept_Btn()
    {
        onAcceptDialog?.Invoke(this);
        NpcController contr = QuestManager.Instance.FindNpcController(currentQuestContainer.npcID);
      //  Debug.Log("currentQuestContainer ID : " + currentQuestContainer.npcID);
      //  Debug.Log("NPC ID : " + contr.ID);
        contr?.QuestReporter?.ReceiveReport(QuestCategoryDefines.INTERACT_AFTER_DIALOG);
        SoundManager.Instance.PlayUISound(UISoundType.DIALOG_ACCEPT);
        GameManager.Instance.MainPP.ExcuteAnimate(PPType.VIGENTTE_FADE_OUT);
        QuestManager.Instance.isDialoging = false;
        DisableUI();
    }

    public void RegisterReject_Btn()
    {
        SoundManager.Instance.PlayUISound(UISoundType.DIALOG_REJECT);
        GameManager.Instance.MainPP.ExcuteAnimate(PPType.VIGENTTE_FADE_OUT);
        QuestManager.Instance.isDialoging = false;
        DisableUI();
    }

    public void CompleteAccept_Btn()
    {
        onCompleteDialog?.Invoke(this);
        SoundManager.Instance.PlayUISound(UISoundType.DIALOG_CLOSE);
        GameManager.Instance.MainPP.ExcuteAnimate(PPType.VIGENTTE_FADE_OUT);
        QuestManager.Instance.isDialoging = false;
        DisableUI();
    }
    public void Progress_Btn()
    {
        NpcController contr = CommonUIManager.Instance.InteractUI.currNpcController;
        contr?.QuestReporter?.ReceiveReport(QuestCategoryDefines.INTERACT_AFTER_BASEDIALOG);
        SoundManager.Instance.PlayUISound(UISoundType.DIALOG_CLOSE);
        GameManager.Instance.MainPP.ExcuteAnimate(PPType.VIGENTTE_FADE_OUT);
        QuestManager.Instance.isDialoging = false;
        DisableUI();
    }

    public void CompleteQuest() => onCompleteDialog?.Invoke(this);

    private void OnPointerClick()
    {
        onClickDialog?.Invoke();
    }

    public void DisableUI()
    {
        ResetPanels();
        GameManager.Instance.Player.Conditions.CanMove = true;
        GameManager.Instance.canUseCamera = true;
        SettingManager.Instance.UseScreenTouch = true;
        SettingManager.Instance.CanExcuteESC = true;
        CommonUIManager.Instance.ResetInteractUI();
        QuestManager.Instance.isDialoging = false;
        SettingManager.Instance.IsUnInterruptibleUI = false;
        onDialogProcessInit?.Invoke();

    }

    private void ResetPanels()
    {
        npcDIalog.gameObject.SetActive(false);
        playerDIalog.gameObject.SetActive(false);
        registerButton_Panel.gameObject.SetActive(false);
        completeButton_Panel.gameObject.SetActive(false);
        progressButton_Panel.gameObject.SetActive(false);
        forceRegisterButton_Panel.gameObject.SetActive(false);
        SettingManager.Instance.IsUnInterruptibleUI = false;
    }
}
