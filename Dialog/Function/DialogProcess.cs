using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogProcess : MonoBehaviour
{
    [SerializeField] private QuestContainer currentQuest = null;
    [SerializeField] private QuestList questList = null;
    [SerializeField] private DialogData dialogData = null;
    [SerializeField] private float oneByOnePrintDelay = 0f;
    private bool isPrintingDialog = false;
    private bool isExcuting = false;

    [SerializeField] private int clickCount = 0; //hide

    public delegate void OnComplateQuest();

    public event OnComplateQuest onCompleteQuest;
  
    public bool IsExcuting { get { return isExcuting; } set { isExcuting = value; } }


    public void ResetData()
    {
        isExcuting = false;
        clickCount = 0;
    }

    public void StartDialog(DialogUI dialogUI)
    {
        if (isExcuting)
            return;
        GameManager.Instance.MainPP.ExcuteAnimate(PPType.VIGNETTE_FADE_IN);

        Init(dialogUI);
        List<DialogEntity> startEntity = dialogData.GetDialogContainer(DialogState.START).dialog;
        StartCoroutine(DialogProcess_Co(dialogUI, startEntity, DialogState.START));
    }

    public void EndDialog(DialogUI dialogUI)
    {
        if (isExcuting)
            return;
        GameManager.Instance.MainPP.ExcuteAnimate(PPType.VIGNETTE_FADE_IN);

        Init(dialogUI);
        List<DialogEntity> endEntity = dialogData.GetDialogContainer(DialogState.END)?.dialog;
        if (endEntity == null || endEntity.Count <= 0)
            onCompleteQuest?.Invoke();
        else
            StartCoroutine(DialogProcess_Co(dialogUI, endEntity, DialogState.END));
    }
    public void ProgressDialog(DialogUI dialogUI)
    {
        if (isExcuting)
            return;
        GameManager.Instance.MainPP.ExcuteAnimate(PPType.VIGNETTE_FADE_IN);

        Init(dialogUI);
        List<DialogEntity> progressEntity = dialogData.GetDialogContainer(DialogState.PROGRESS)?.dialog;
        if (progressEntity == null || progressEntity.Count <= 0)
            return;

        StartCoroutine(DialogProcess_Co(dialogUI, progressEntity, DialogState.PROGRESS));
    }

    public void InteractDialog(DialogUI dialogUI, DialogFile dialogFile, int dialogIndex, DialogState state)
    {
        if (isExcuting)
            return;
        GameManager.Instance.MainPP.ExcuteAnimate(PPType.VIGNETTE_FADE_IN);

        InitJustDialog(dialogFile, dialogIndex);
        List<DialogEntity> interactEntity = dialogData.GetDialogContainer(state)?.dialog;
        if (interactEntity == null || interactEntity.Count <= 0)
            return;
        StartCoroutine(DialogProcess_Co(dialogUI, interactEntity, state));
    }


    private void Init(DialogUI dialogUI)
    {
        questList = dialogUI.QuestList;
        currentQuest = dialogUI.CurrentQuestContainer;

        if (questList == null)
            Debug.Log("널 QUestList");

        dialogData = questList.dialogFile.GetDialogData(currentQuest.questDialogId);
    }

    private void InitJustDialog(DialogFile dialogFile, int id)
    {
        dialogData = dialogFile.GetDialogData(id);
    }


    private IEnumerator DialogProcess_Co(DialogUI dialogUI, List<DialogEntity> entityList,DialogState dialogState)
    {
        SettingManager.Instance.IsUnInterruptibleUI = true;
        SettingManager.Instance.UseScreenTouch = false;
        SettingManager.Instance.CanExcuteESC = false;
        GameManager.Instance.Player.Conditions.CanMove = false;
        GameManager.Instance.Player.StopMove();
        GameManager.Instance.canUseCamera = false;
        CursorManager.Instance.CursorVisible();
        isExcuting = true;

        List<DialogEntity> entity = entityList;
        bool isOneByOne = dialogData.isOneByOnePrint;
        int nextCount = isOneByOne ? 2 : 1;
        clickCount = 0;

        for (int i = 0; i < entity.Count; i++)
        {
            if (i == (entity.Count-1))
                yield return StartCoroutine(NextDialogSentence(dialogUI, entity[i], isOneByOne, nextCount, dialogState, true));
            else
                yield return StartCoroutine(NextDialogSentence(dialogUI, entity[i], isOneByOne, nextCount, dialogState, false));
        }

        isExcuting = false;
    }

    private IEnumerator NextDialogSentence(DialogUI dialogUI, DialogEntity nextSentence, bool isOneByOnePrint, int nextCount, DialogState state, bool isFinalSentence = false)
    {
        //click = 1 -> 문단 완성 click =2 -> 다음 으로. 
        bool isFinish = false;
        string sentence = string.Empty;
        clickCount = 0;
        if (isOneByOnePrint)
        {
            for (int i = 0; i < nextSentence.dialog.Length; i++)
            {
                if (clickCount >= 1)
                {
                    SoundManager.Instance.PlayUISound(UISoundType.PRINT_DIALOG_ALL);
                    break;
                }
                sentence += nextSentence.dialog[i];

                if (!nextSentence.dialog[i].ToString().Equals(" ") && i < nextSentence.dialog.Length - 1)
                    SoundManager.Instance.PlayUISound(UISoundType.PRINT_DIALOG_ONEBYONE);

                dialogUI.UpdateDialog(nextSentence.name == "나", nextSentence.name, sentence, false, state, isFinish);
                yield return new WaitForSeconds(oneByOnePrintDelay);
                if (nextSentence.dialog[i].ToString().Equals(".") && i < nextSentence.dialog.Length + 1)
                    yield return new WaitForSeconds(.175f);
            }

            sentence = nextSentence.dialog;
            clickCount = 1;
            isFinish = true;
            dialogUI.UpdateDialog(nextSentence.name == "나", nextSentence.name, sentence, isFinalSentence, state, isFinish);
        }
        else
        {
            sentence += nextSentence.dialog;
            dialogUI.UpdateDialog(nextSentence.name == "나", nextSentence.name, sentence, isFinalSentence, state, isFinish);
            SoundManager.Instance.PlayUISound(UISoundType.PRINT_DIALOG_ALL);
            clickCount = 0;
            isFinish = true;
        }

        yield return new WaitUntil(() => isFinish && clickCount >= nextCount);
    }


    public void PointerClick()
    {
       clickCount +=1;

    }


}
