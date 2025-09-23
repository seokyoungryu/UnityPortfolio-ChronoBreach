using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public enum ConsoleSystemLogType
{
    PLAYER_NORMAL_LOG =0,
    SYSTEM_NORMAL_LOG = 1,
    SYSTEM_ERROR_LOG = 2,
    SYSTEM_POSITIVE_LOG=3,
}

public class ConsoleUI : UIRoot
{
    [SerializeField] private Transform option = null;
    [SerializeField] private Transform container = null;
    [SerializeField] private CustomVerticalLayoutGroup commandListLayGp = null;
    [SerializeField] private ScrollUI scrollUI = null;

    [SerializeField] private Transform taskContainer;
    [SerializeField] private ObjectPoolingList taskList;
    [SerializeField] private SoundList typingSound;
    [SerializeField] private SoundList clearSound;
    [SerializeField] private SoundList summitSound;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Color playerNormalFontColor;
    [SerializeField] private Color systemNormalFontColor;
    [SerializeField] private Color systemPositiveLogFontColor;
    [SerializeField] private Color systemErrorLogFontColor;


    private List<string> previousCommands = new List<string>();
    private List<ConsoleTask> tasks = new List<ConsoleTask>();
    private bool isActive = false;
    private bool isDoingHistory = false;
    private bool isInputHistory = false;
    private int maxPreviousIndex = -1;
    private int currPreviousIndex = -1;

    public TMP_InputField InputField => inputField;

    #region Events
    public delegate void OnSummit(string summitText);
    public event Func<bool> onCanHistroy;

    public event OnSummit onExcuteSummitProcess;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        container.gameObject.SetActive(true);
        inputField.onValueChanged.AddListener(Sound);
        UIHelper.AddEventTrigger(container.gameObject, EventTriggerType.PointerEnter, delegate { MouseEnterUI(); });
        UIHelper.AddEventTrigger(option.gameObject, EventTriggerType.PointerEnter, delegate { MouseEnterUI(); });
        container.gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        inputField.onValueChanged.RemoveListener(Sound);
    }


    private void Update()
    {
        if (inputField.isFocused)
            MouseEnterUI();

        //이부분이.. searchable에서 선택하려고 다운 애로우를 누를때 밑에도 작동함..
        if (OnCanHistoryInvoke())
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currPreviousIndex == -1) currPreviousIndex = maxPreviousIndex;
                else
                {
                    currPreviousIndex--;
                    if (currPreviousIndex < 0)
                        currPreviousIndex = 0;
                }
                SetPreviousCommand(currPreviousIndex);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && isInputHistory)
            {
                if (currPreviousIndex == -1 || maxPreviousIndex == -1 || previousCommands.Count <= 0) return;

                currPreviousIndex++;
                if (currPreviousIndex >= maxPreviousIndex)
                    currPreviousIndex = maxPreviousIndex;
                SetPreviousCommand(currPreviousIndex);
            }
            else if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.DownArrow))
            {   //즉 이렇게 했을때 searchable에서 밑으로 선택 가능하게.. 
                isInputHistory = false;
                isDoingHistory = false;
            }
        }
        
    }


    

    public bool GetIsDoingHistory() => isDoingHistory;

    public bool OnCanHistoryInvoke()
    {
        if (onCanHistroy == null) return true;

        foreach (Func<bool> func in onCanHistroy.GetInvocationList())
            if (!func())
                return false;
        return true;
    }

    private void SetPreviousCommand(int index)
    {
        if (index < 0) index = 0;
        else if (index >= maxPreviousIndex) index = maxPreviousIndex;

        if (index < maxPreviousIndex)
        {
            isInputHistory = true;
            isDoingHistory = true;
        }
        else if (index >= maxPreviousIndex)
        {
            isInputHistory = false;
            isDoingHistory = false;
        }


        if (previousCommands.Count > 0)
            SettingInputFieldText(previousCommands[index]);
    }


    public override void OpenUIWindow()
    {
        OtherActive(true, container.gameObject);
        MouseEnterUI();
        OnFocus();
        isActive = true;
    }


    public override void CloseUIWindow()
    {
        OtherActive(false, container.gameObject);
        isActive = false;
    }

    public void SettingInputFieldText(string text)
    {
        inputField.text = text.TrimEnd();
        OnFocus();
    }

    public void Sound(string text)
    {
        SoundManager.Instance.PlayExtraSound(typingSound);
    }

    public void OnFocus()
    {
        inputField.Select();
        inputField.ActivateInputField();
        inputField.text = inputField.text.TrimEnd();
        inputField.caretPosition = inputField.text.Length;
    }

    public void OptionActive()
    {
        isActive = !isActive;
        if (isActive)
        {
            MouseEnterUI();
            scrollUI.ResetPosition();
            OnFocus();
        }

        container.gameObject.SetActive(isActive);
    }


    private void MouseEnterUI()
    {
        
        GameManager.Instance.isWriting = true;
    }


    private void MouseExitUI()
    {
        GameManager.Instance.isWriting = false;
    }

    public void ExcuteBtn()
    {
        inputField.onSubmit?.Invoke(inputField.text);
    }


    public void ClearAllTask()
    {
        for (int i = 0; i < tasks.Count; i++)
            ObjectPooling.Instance.SetOBP(taskList.ToString(), tasks[i].gameObject);
        SoundManager.Instance.PlayExtraSound(clearSound);
        currPreviousIndex = -1;
    }

    public void ExcuteSummit(string text)
    {
        ConsoleTask task = ObjectPooling.Instance.GetOBP(taskList.ToString()).GetComponent<ConsoleTask>();
        task.transform.parent = taskContainer;
        task.transform.SetAsLastSibling();
        task.SettingFontColor(playerNormalFontColor);
        task.SettingTask("- "+text);
        tasks.Add(task);

        SoundManager.Instance.PlayExtraSound(summitSound);
        previousCommands.Add(text);
        maxPreviousIndex = previousCommands.Count - 1;
        currPreviousIndex = -1;
        isDoingHistory = false;
        //콘솔 프로세스 실행. 
        onExcuteSummitProcess?.Invoke(text);

        inputField.text = string.Empty;
        inputField.ActivateInputField();

        commandListLayGp.Excute();
        scrollUI.ResetPosition();

        Debug.Log("Summit : " + text);
    }

    public void ExcuteSystemLog(string text, ConsoleSystemLogType logType)
    {
        ConsoleTask task = ObjectPooling.Instance.GetOBP(taskList.ToString()).GetComponent<ConsoleTask>();
        task.transform.parent = taskContainer;
        task.transform.SetAsLastSibling();
        task.SettingFontColor(GetLogColor(logType));
        task.SettingTask("System : " + text );
        tasks.Add(task);

        inputField.text = string.Empty;
        inputField.ActivateInputField();
        commandListLayGp.Excute();
    }

    public Color GetLogColor(ConsoleSystemLogType logType)
    {
        switch (logType)
        {
            case ConsoleSystemLogType.PLAYER_NORMAL_LOG:
                return playerNormalFontColor;
            case ConsoleSystemLogType.SYSTEM_ERROR_LOG:
                return systemErrorLogFontColor;
            case ConsoleSystemLogType.SYSTEM_NORMAL_LOG:
                return systemNormalFontColor;
            case ConsoleSystemLogType.SYSTEM_POSITIVE_LOG:
                return systemPositiveLogFontColor;
            default:
                return systemNormalFontColor;
        }
    }


}
