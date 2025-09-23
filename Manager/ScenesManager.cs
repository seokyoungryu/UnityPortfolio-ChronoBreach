using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Linq;

public class ScenesManager : Singleton<ScenesManager>
{
    [SerializeField] private GameObject tempLoadingUI = null;
    private AsyncOperation asyncOp = null;
    [SerializeField] private int currentSceneIndex = 0;

    [SerializeField] private int changeCount = 0;

    [SerializeField] private Vector3 mainSceneInitPos;
    [SerializeField] private Vector3 mainSceneInitRot;

    // public List<Transform> loadingActiveFalse = new List<Transform>();

    [SerializeField] private List<string> dontDestroyNames = new List<string>();
    [SerializeField] private string changeDontDestroyName = "Actived";
    public int CurrentSceneIndex { get { return currentSceneIndex; } set { currentSceneIndex = value; } }

    public Vector3 MainSceneInitPos => mainSceneInitPos; 
    public Vector3 MainSceneInitRot => mainSceneInitRot;

    public string ChangeDontDestroyName => changeDontDestroyName;
    public List<string> DontDestroyNames => dontDestroyNames;
    public int ChangeCount => changeCount;

    #region Events
    public UnityAction OnExcuteAfterLoading;
    public UnityAction onAbsoluteExcuteAfterLoading;
    public UnityAction onExucteInit;
    private UnityAction onAbsoluteExucteInit;

    public event UnityAction OnAbsoluteExucteInit
    {
        add
        {
            if(!onAbsoluteExucteInit.GetInvocationList().Contains(value))
            {
                onAbsoluteExucteInit += value;
            }
        }
        remove
        {
            onAbsoluteExucteInit -= value;
        }
    
    }
    #endregion


    public void ActiveTempLoadingUI(bool active) => tempLoadingUI.SetActive(active);

    public void ChangeScene(string changeSceneIndex, bool isTitle = false)
    {
        ChangeSceneSetting(isTitle);
        int index = SceneUtility.GetBuildIndexByScenePath(changeSceneIndex);
        currentSceneIndex = index;
        Debug.Log("Get :  " + changeSceneIndex + " -> " + index);
        LoadingScene(index);

    }

    public void ChangeScene(int changeSceneIndex, bool isTitle = false)
    {
        ChangeSceneSetting(isTitle);
        currentSceneIndex = changeSceneIndex;
        LoadingScene(changeSceneIndex);
    }


    private void ChangeSceneSetting(bool isTitle)
    {
        GameManager.Instance.canUseCamera = false;
        if (!isTitle)
        {
            CommonUIManager.Instance.AllCloseActiveUIWindow();
            CommonUIManager.Instance.ResetInteractUI();
            CommonUIManager.Instance.globalAppearBossHpUI.SetActive(false);
            SettingManager.Instance.IsUnInterruptibleUI = false;
        }

        tempLoadingUI.SetActive(true);
        onAbsoluteExucteInit?.Invoke();
        onExucteInit?.Invoke();
       if(!isTitle)
           ObjectPooling.Instance.DisableAllActive();
        onExucteInit = () => { };
    }

    public bool IsCurrentScene(int currentIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex == currentIndex)
            return true;

        return false;
    }


    public void LoadingScene(int nextSceneIndex)
    {
        changeCount++;
        ProgressLoadingBar.LoadingSetting(nextSceneIndex);
    }



    public void OnExcuteDoneLoading()
    {
        GameManager.Instance.isWriting = false;

        Debug.Log("¾À ¹Ù²î°í ½ÇÇà.");
        onAbsoluteExcuteAfterLoading?.Invoke();
        OnExcuteAfterLoading?.Invoke();

        OnExcuteAfterLoading = () => { };
    }


}
