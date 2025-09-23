using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRegister : MonoBehaviour
{
    [SerializeField] private KeyCodeContain[] contains;

    private bool isVisible = false;

    private void Start()
    {
        for (int i = 0; i < contains.Length; i++)
            if (contains[i].startActive)
                contains[i].window.OpenUIWindow();
    }

    void Update()
    {
        if (GameManager.Instance.isWriting) return;
        if (Input.GetKeyDown(KeyCode.Escape) && SettingManager.Instance.CanExcuteESC)
        {
            isVisible = true;
            if (CursorManager.Instance.CurrentCursorMode == CursorMode.VISIBLE)     //마우스 Visible이면 창 닫음.
            {
                CommonUIManager.Instance.CloseLastPopupWindow(); 
                if (CommonUIManager.Instance.GetActiveUICount() <= 0)
                {
                    GameManager.Instance.canUseCamera = true;
                    CursorManager.Instance.CursorLock();
                    isVisible = false;
                }
            }
            if (isVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                GameManager.Instance.canUseCamera = false;
                CursorManager.Instance.CursorVisible();
            }
        }

        for (int i = 0; i < contains.Length; i++)
        {
            if (Input.GetKeyDown(contains[i].keyCode))
            {
                if (contains[i].targetUI != null)
                    Active(i, contains[i].targetUI);
                else
                    Active(i, contains[i].window.gameObject);
            }
        }
    }

    public void Active(int index, GameObject targetUI)
    {
        if (targetUI != null && !targetUI.activeInHierarchy)
        {
            SettingManager.Instance.IsUnInterruptibleUI = true;
            contains[index].window.OpenUIWindow();
        }
        else
        {
            contains[index].window.CloseUIWindow();
            if (CommonUIManager.Instance.ActiveUICount <= 0)
                SettingManager.Instance.IsUnInterruptibleUI = false;
        }
    }

    public void Active_Btn(int index)
    {
        if (contains[index].targetUI != null)
            Active(index, contains[index].targetUI);
        else
            Active(index, contains[index].window.gameObject);
    }


    public KeyCodeContain GetWindow(int uiID)
    {
        for (int i = 0; i < contains.Length; i++)
            if (contains[i].window.UIID == uiID)
                return contains[i];
        return null;
    }


}


[System.Serializable]
public class KeyCodeContain
{
    public KeyCode keyCode;
    public UIRoot window;
    public GameObject targetUI;
    public bool startActive;
}

