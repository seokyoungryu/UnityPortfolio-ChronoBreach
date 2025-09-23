using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsolePresenter : MonoBehaviour
{
    [SerializeField] private ConsoleUI consoleUI;
    [SerializeField] private ConsoleProcess consoleProcess;
    [SerializeField] private SearchableText searchable;

    private void Awake()
    {
        consoleUI.InputField.onSubmit.AddListener(OnSummit);

        searchable.onGetSearchableTests += consoleProcess.GetTextList;
        searchable.onGetSearchableFilters += consoleProcess.GetTextFilters;
        consoleProcess.onClear += consoleUI.ClearAllTask;
        searchable.onSelect += consoleUI.SettingInputFieldText;
        consoleUI.onExcuteSummitProcess += consoleProcess.Excute;

        consoleProcess.onExcuteLog += consoleUI.ExcuteSystemLog;
        consoleUI.onCanHistroy += searchable.IsSelectSearchableTest;
        searchable.onCanUpdate += SearchableCanUpdate;
    }

    private void OnDestroy()
    {
        consoleUI.InputField.onSubmit.RemoveListener(OnSummit);

        searchable.onGetSearchableTests -= consoleProcess.GetTextList;
        searchable.onGetSearchableFilters -= consoleProcess.GetTextFilters;
        consoleProcess.onClear -= consoleUI.ClearAllTask;

        searchable.onSelect -= consoleUI.SettingInputFieldText;
        consoleUI.onExcuteSummitProcess -= consoleProcess.Excute;
        consoleProcess.onExcuteLog -= consoleUI.ExcuteSystemLog;
        consoleUI.onCanHistroy -= searchable.IsSelectSearchableTest;
        searchable.onCanUpdate -= SearchableCanUpdate;
    }


    public bool SearchableCanUpdate()
    {
        if (consoleUI.GetIsDoingHistory())
            return false;
        else
            return true;
    }

    public void OnSummit(string text)
    {
        if (searchable.CurrentSelectTask != null) return;
        consoleUI.ExcuteSummit(text);
    }

}
