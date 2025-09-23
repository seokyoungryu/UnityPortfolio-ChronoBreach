using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonScoreUI : UIRoot
{
    [SerializeField] protected Vector2 excutePosition = Vector2.zero;

    [SerializeField] protected CameraShakeInfo enterCamera;
    protected BaseDungeonTitle currDungeonTitle = null;


    [ContextMenu("Test")]
    public void Test()
    {
        GetComponent<RectTransform>().anchoredPosition = excutePosition;
    }

    public virtual void ExcuteScoreUI(BaseDungeonTitle dungeonTitle)
    {
        currDungeonTitle = dungeonTitle;
        GameManager.Instance.Cam.ShakeCamera(enterCamera);
        CursorManager.Instance.CursorVisible();
        GameManager.Instance.canUseCamera = false;
    }


    public void GoToMain_Btn()
    {
        MapManager.Instance.DungeonNotifierUI.SetDisable();
        SoundManager.Instance.PlayBGM_CrossFade(SoundManager.Instance.MainSceneBGM, 4f);
        AllActive(false);
        MapManager.Instance.CurrentDungeonTitle.ClearObj();
        ScenesManager.Instance.OnExcuteAfterLoading = () => GameManager.Instance.Cam.SetTarget(GameManager.Instance.Player.gameObject);
        ScenesManager.Instance.OnExcuteAfterLoading += () =>
        {
            GameManager.Instance.Player.gameObject.SetActive(true);
            GameManager.Instance.Player.Resurrection(ScenesManager.Instance.MainSceneInitPos);
            GameManager.Instance.Player.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
            CursorManager.Instance.CursorLock();
            GameManager.Instance.canUseCamera = true;
        };
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => SaveManager.Instance.ExcuteInitLoadPlayerInfo();
       // ScenesManager.Instance.OnExcuteAfterLoading += () => SaveManager.Instance.QuestLoad();

        ScenesManager.Instance.ChangeScene(1);
    }

    protected virtual void AllActive(bool active)
    {
        MapManager.Instance.CurrentScoreUIType = ScoreUIType.NOT_EXCUTE;
    }



}
