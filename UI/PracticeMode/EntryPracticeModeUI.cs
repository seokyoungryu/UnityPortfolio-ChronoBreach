using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPracticeModeUI : UIRoot
{
    private RectTransform rect = null;
    [SerializeField] private Vector3 uiResetPos = new Vector3(1280, 720, 0);
    [SerializeField] private Vector3 playerSpawnPos;
    public Vector3 pos;


    protected override void Awake()
    {
        base.Awake();
        rect = GetComponent<RectTransform>();
        rect.transform.position = uiResetPos;
        gameObject.SetActive(false);
    }


    public override void OpenUIWindow()
    {
        base.OpenUIWindow();
        rect.transform.position = uiResetPos;
    }


    private void OnDrawGizmos()
    {
        pos = transform.position;
    }

    public void ExcuteEntryPracticeMode1()
    {
        SaveManager.Instance.AllSave();
        SoundManager.Instance.PlayBGM_CrossFade(SoundManager.Instance.PracticeMode1SceneBgm, 5f);
        ScenesManager.Instance.OnExcuteAfterLoading = () => GameManager.Instance.Player.TranslatePosition(playerSpawnPos);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.ChangeScene(9);

    }
}
