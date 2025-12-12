using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[System.Serializable]
public class SearchDictionary
{
    public string key;
    public string transValue;
}

public class SearchableText : MonoBehaviour
{
    [SerializeField] private ScrollUIByKey scroll = null;
    [SerializeField] private Transform rootContainer = null;
    [SerializeField] private Transform taskContainer = null;
    [SerializeField] private ObjectPoolingList taskList;
    [SerializeField] private SoundList selectSound;
    [SerializeField] private SoundList clickSound;
    [SerializeField] private CustomVerticalLayoutGroup layoutGroup = null;

    [Header("Variables")]
    [SerializeField] private bool canMouseSelect = true;
    [SerializeField] private bool useUpdateProcess = false;
    [SerializeField] private bool allowRepeatKeyMove = false;
    [SerializeField] private float repeatInitWaitTime = 1f;
    [SerializeField] private float eachRepeatMovedelayTime = 0.2f;

    [SerializeField] private SearchableTextTask currentSelectTask = null;
    private List<SearchDictionary> filters; 
    private string[] searchableList;
    private List<SearchableTextTask> tasks = new List<SearchableTextTask>();
    private string[] findCommandLists;
    private bool isOpenSearchable = false;
    private int selectIndex = -1;
    private float currRepeatTimer = 0f;
    private bool isExcuteInitRepeat = false;
    private bool isPressing = false;
    private List<RectTransform> scrollTasks = new List<RectTransform>();

    public bool IsOpenSearchable => isOpenSearchable;
    public SearchableTextTask CurrentSelectTask => currentSelectTask;

    #region Events
    public delegate string[] OnGetTextLists();
    public delegate List<SearchDictionary> OnGetFilterLists();
    public delegate void OnSelect(string selectText);
    public event Func<bool> onCanUpdate;


    public event OnGetTextLists onGetSearchableTests;
    public event OnGetFilterLists onGetSearchableFilters;
    public event OnSelect onSelect;
    #endregion

    private void Start()
    {
        searchableList = onGetSearchableTests?.Invoke();
        filters = onGetSearchableFilters?.Invoke();
        rootContainer.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (useUpdateProcess && CanUpdateProcess())
            HandleProcess();
    }


    public bool CanUpdateProcess()
    {
        if (onCanUpdate == null) return true;
        foreach (Func<bool> func in onCanUpdate.GetInvocationList())
            if (!func())
                return false;

        return true;
    }


    public bool FilterExceptionSentence(string sentence, string[] filters)
    {
        string[] split = sentence.ToLower().Split(' ');
        for (int i = 0; i < split.Length; i++)
            if (FilterExceptionWord(split[i], filters))
                return true;

        return false;
    }

    /// <summary>
    /// 매개변수 word가 필터의 단어들중 동일한게 있다면 true.
    /// </summary>
    public bool FilterExceptionWord(string word,string[] filters)
    {
        for (int i = 0; i < filters.Length; i++)
            if (filters[i] == word)
                return true;

        return false;
    }

    public void HandleProcess()
    {
        if (!isOpenSearchable) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            SelectUp();
        else if(Input.GetKey(KeyCode.UpArrow))
        {
            if (!allowRepeatKeyMove) return;
            currRepeatTimer += Time.deltaTime;
            if(!isExcuteInitRepeat && currRepeatTimer >= repeatInitWaitTime)
            {
                SelectUp();
                isExcuteInitRepeat = true;
                currRepeatTimer = 0f;
            }
            else if(isExcuteInitRepeat && currRepeatTimer >= eachRepeatMovedelayTime)
            {
                SelectUp();
                currRepeatTimer = 0f;
            }
            isPressing = true;
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            isExcuteInitRepeat = false;
            currRepeatTimer = 0f;
            isPressing = false;

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
            SelectDown();
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!allowRepeatKeyMove) return;
            currRepeatTimer += Time.deltaTime;
            if (!isExcuteInitRepeat && currRepeatTimer >= repeatInitWaitTime)
            {
                SelectDown();
                isExcuteInitRepeat = true;
                currRepeatTimer = 0f;
            }
            else if (isExcuteInitRepeat && currRepeatTimer >= eachRepeatMovedelayTime)
            {
                SelectDown();
                currRepeatTimer = 0f;
            }
            isPressing = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isExcuteInitRepeat = false;
            currRepeatTimer = 0f;
            isPressing = false;
        }

        if (Input.GetKeyDown(KeyCode.Return))
            Excute();
    }

    private void SelectUp()
    {
        selectIndex--;
        if (selectIndex <= -1)
        {
            selectIndex = -1;
            currentSelectTask?.UnSelect();
            currentSelectTask = null;
            return;
        }
        SettingCurrentSelect(selectIndex);
    }
    private void SelectDown()
    {
        selectIndex++;
        if (selectIndex >= tasks.Count)
            selectIndex = tasks.Count - 1;
        SettingCurrentSelect(selectIndex);
    }
    public bool IsSelectSearchableTest()
    {
        return currentSelectTask == null ? true : false;
    }

    public void ResetSearchable()
    {
        currentSelectTask = null;
    }

    public void SettingCurrentSelect(int selectIndex)
    {
        this.selectIndex = selectIndex;
        if (currentSelectTask != null)
            currentSelectTask.UnSelect();

        if(selectIndex >= 0 && selectIndex < tasks.Count)
        {
            currentSelectTask = tasks[selectIndex];
            currentSelectTask.Select();
            scroll.SelectTaskIndex(selectIndex);
            SoundManager.Instance.PlayExtraSound(selectSound);
        }
    }
    private int GetValue(string sentence)
    {
        string[] split = sentence.ToLower().Split(' ');
        for (int i = 0; i < split.Length; i++)
            for (int j = 0; j < filters.Count; j++)
                if (split[i] == filters[j].key)
                    return i;

        return -1;
    }

    public void GetText(string currSearchText)
    {
        selectIndex = -1;
        findCommandLists = GetSeachableTexts2(currSearchText);
        scrollTasks.Clear();

        for (int i = 0; i < findCommandLists.Length; i++)
            for (int x = 0; x < filters.Count; x++)
                if (findCommandLists[i].Contains(filters[x].key))
                    findCommandLists[i] = findCommandLists[i].Replace(filters[x].key, filters[x].transValue);

        if (findCommandLists == null || findCommandLists.Length <= 0)
        {
            isOpenSearchable = false;
            rootContainer.gameObject.SetActive(false);
            return;
        }

        rootContainer.gameObject.SetActive(true);
        isOpenSearchable = true;
        for (int i = 0; i < tasks.Count; i++)
            ObjectPooling.Instance.SetOBP(taskList.ToString(), tasks[i].gameObject);
        tasks.Clear();

        for (int i = 0; i < findCommandLists.Length; i++)
        {
            SearchableTextTask task = ObjectPooling.Instance.GetOBP(taskList.ToString()).GetComponent<SearchableTextTask>();
            task.transform.parent = taskContainer;
            task.transform.SetAsFirstSibling();
            task.SettingTask(findCommandLists[i]);
            tasks.Add(task);
            if (task.GetComponent<RectTransform>())
                scrollTasks.Add(task.GetComponent<RectTransform>());

        }
        tasks.Reverse();
        layoutGroup.Excute();
        scrollTasks.Reverse();

        scroll.SettingTask(scrollTasks.ToArray());
        if (canMouseSelect)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                int index = i;
                UIHelper.AddEventTrigger(tasks[i].gameObject, EventTriggerType.PointerEnter, delegate { SelectByMouse(index); });
                UIHelper.AddEventTrigger(tasks[i].gameObject, EventTriggerType.PointerClick, delegate { ClickByMouse(index); });
            }
        }


    }

    public void SelectByMouse(int selectIndex)
    {
        if (isPressing)
            return;
        SettingCurrentSelect(selectIndex);
    }
    public void ClickByMouse(int selectIndex)
    {
        if (isPressing)
            return;
        Excute();
    }

    private void Excute()
    {
        if (currentSelectTask == null) return;
        onSelect?.Invoke(currentSelectTask.GetText);
        SoundManager.Instance.PlayExtraSound(clickSound);
        ResetSearchable();
    }


    public string[] GetSeachableTexts2(string currrInput)
    {
        List<string> retStr = new List<string>();

        for (int i = 0; i < searchableList.Length; i++)
        {
            if (CheckIsSameSentence(currrInput, searchableList[i]))
                retStr.Add(searchableList[i]);
        }

        return retStr.ToArray();
    }

    public bool CheckIsSameSentence(string input, string searchable)
    {
        if (input == string.Empty || input == "" || input.Trim() == "")
            return false;

        int valueIndex = GetValue(searchable);
        string[] inputWordSplit = input.ToLower().Split(' ');
        string[] searchWordSplit = searchable.ToLower().Split(' ');

        for (int i = 0; i < inputWordSplit.Length; i++)
        {
            if (valueIndex != -1 && valueIndex == i)  
            {
                Debug.Log(inputWordSplit[i]);
                continue;
            }
            if (searchWordSplit.Length <= i)
                return false;

            char[] inputCharSplit = inputWordSplit[i].ToCharArray();
            char[] searchCharSplit = searchWordSplit[i].ToCharArray();

            for (int x = 0; x < inputCharSplit.Length; x++)
            {
                if (x >= searchCharSplit.Length)
                    return false;
                if (inputCharSplit[x] != searchCharSplit[x])
                    return false;
            }
        }

        return true;
    }

    private string[] GetSearchTexts1(string currSearchText)
    {
        List<string> searchTexts = new List<string>();
        char[] currTextSplit = currSearchText.ToLower().ToCharArray();
        char[] searchListSplit;

        for (int i = 0; i < searchableList.Length; i++)
        {
            searchListSplit = searchableList[i].ToLower().ToCharArray();
            for (int x = 0; x < currTextSplit.Length; x++)
            {
                if(currTextSplit[x] == '★' && x == (currTextSplit.Length -1))
                {
                    searchTexts.Add(searchableList[i]);
                    break;
                }
                if (currTextSplit[x] == '★') continue;   
                if (searchListSplit.Length < currTextSplit.Length || searchListSplit[x] != currTextSplit[x])
                    break;
                if (x == (currTextSplit.Length - 1))
                {
                    if (currTextSplit[x] == searchListSplit[x])
                    {
                        searchTexts.Add(searchableList[i]);
                        break;
                    }
                }
            }
        }

        return searchTexts.ToArray();
    }


}
