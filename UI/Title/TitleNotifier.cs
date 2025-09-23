using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public enum TitleNotiferType
{
    NONE,
    NEW_START_EMPTY,
    NEW_START_HAVEDATA,
    CONTINUE_START,
}

public class TitleNotifier : MonoBehaviour
{
    [SerializeField] private TitleSlotUI currSlotUI = null;
    [SerializeField] private TitleNotiferType notifierType = TitleNotiferType.NONE;
    [Header("[0] NewStart_Empty  [1] NewStart_Have  [2] ContinueStart  [3] DeleteNewStart")]
    [SerializeField] private Transform[] trs;
    [SerializeField,TextArea(0,3)] private string[] description;
    [SerializeField] private TMP_Text[] descript_Texts;

    [SerializeField] private GameObject[] btns;
    [SerializeField] private GameObject[] backgrounds;


    private void Start()
    {
        SettingSounds();
    }


    private void SettingSounds()
    {
        for (int i = 0; i < btns.Length; i++)
            UIHelper.AddEventTrigger(btns[i], EventTriggerType.PointerEnter, delegate { SoundManager.Instance.PlayUISound(UISoundType.TITLE_POINTER_ENTER3); });

        for (int i = 0; i < backgrounds.Length; i++)
            UIHelper.AddEventTrigger(backgrounds[i], EventTriggerType.PointerClick, delegate { SoundManager.Instance.PlayUISound(UISoundType.CLICK); });

    }


    public void ClickNewStartSlot(TitleSlotUI ui)
    {
        if (ui.Data.CanLoadInfo())
            ExcuteNewStart_HaveDataSlot(ui);
        else
            ExcuteNewStart_EmptySlot(ui);
    }

    public void ClickContinueStartSlot(TitleSlotUI ui)
    {
        ExcuteContinueStart(ui);
    }

    public void ExcuteNewStart_EmptySlot(TitleSlotUI ui)
    {
        Common(ui);
        trs[0].gameObject.SetActive(true);
        descript_Texts[0].text = currSlotUI.SlotIndex + description[0];
        notifierType = TitleNotiferType.NEW_START_EMPTY;
    }

    public void ExcuteNewStart_HaveDataSlot(TitleSlotUI ui)
    {
        Common(ui);
        trs[1].gameObject.SetActive(true);
        descript_Texts[1].text = currSlotUI.SlotIndex + description[1];
        notifierType = TitleNotiferType.NEW_START_HAVEDATA;
    }

    public void ExcuteContinueStart(TitleSlotUI ui)
    {
        Common(ui);
        trs[2].gameObject.SetActive(true);
        descript_Texts[2].text = currSlotUI.SlotIndex + description[2];
        notifierType = TitleNotiferType.CONTINUE_START;

    }

    public void ExcuteDeleteDataAndNewStart()
    {
        trs[3].gameObject.SetActive(true);
        descript_Texts[3].text = currSlotUI.SlotIndex + description[3];

    }

    public void Common(TitleSlotUI uI)
    {
        currSlotUI = uI;
        SoundManager.Instance.PlayUISound(UISoundType.OPEN_WINDOW);

        for (int i = 0; i < trs.Length; i++)
            trs[i].gameObject.SetActive(false);
    }


    public void ExcuteMainScene()
    {
        if (notifierType == TitleNotiferType.NEW_START_HAVEDATA || notifierType == TitleNotiferType.NEW_START_EMPTY)
        {
            currSlotUI.Data.DeleteLoadData();
            ExcuteMainSceneCommon(true);
        }
        else ExcuteMainSceneCommon(false);

    }

    private void ExcuteMainSceneCommon(bool isNewData)
    {
        SettingManager.Instance.IsTitle = false;
        SaveManager.Instance.SetCurrTitleData(currSlotUI.Data);
        ScenesManager.Instance.ChangeScene(1, true);
        SoundManager.Instance.PlayBGM_CrossFade(SoundManager.Instance.MainSceneBGM,5f);

        ScenesManager.Instance.OnExcuteAfterLoading = () => GameManager.Instance.Player.gameObject.SetActive(true);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.SetTarget(GameManager.Instance.Player.gameObject);
        ScenesManager.Instance.OnExcuteAfterLoading += () => SaveManager.Instance.ExcuteAbsoluteLoadPlayerInfo();
        ScenesManager.Instance.OnExcuteAfterLoading += () =>
        {
            if (isNewData)
            {
                Debug.Log("새로운 !");
                SaveManager.Instance.AllLoad(true);
            }
            else
            {
                Debug.Log("기존 !");
                SaveManager.Instance.AllLoad(false);
                QuestManager.Instance.currentQuestSession = currSlotUI.Data.CurrQuestSession;
            }
        };
        ScenesManager.Instance.OnExcuteAfterLoading += () => CursorManager.Instance.CursorLock();
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Player.GetComponent<PlayerEquipment>().LoadExcuteEquipmentItem();
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => Debug.Log("로드완!!!!");
    }


    public void CloseWindow()
    {
        SoundManager.Instance.PlayUISound(UISoundType.CLOSE_WINDOW);

        for (int i = 0; i < trs.Length; i++)
            trs[i].gameObject.SetActive(false);
    }

}

