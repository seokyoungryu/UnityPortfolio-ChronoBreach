using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(EventTrigger))]
public class InterfaceMove : MonoBehaviour
{
    [SerializeField] private bool cantMove = false;
    [SerializeField] private bool initPos = false;
    [SerializeField] private GameObject uiWindow = null;
    [SerializeField] private string savePositionXName = string.Empty;
    [SerializeField] private string savePositionYName = string.Empty;

    private Vector3 offset;
    public Vector3 windowResetPosition = Vector3.zero;
    public Vector3 windowPosition = Vector3.zero;


    private void Awake()
    {
        if (uiWindow == null)
            uiWindow = this.gameObject;

        if (!cantMove)
        {
            UIHelper.AddEventTrigger(uiWindow, EventTriggerType.PointerDown, delegate { OnPointerDown(uiWindow); });
            UIHelper.AddEventTrigger(uiWindow, EventTriggerType.Drag, delegate { OnDrag(uiWindow); });
            UIHelper.AddEventTrigger(uiWindow, EventTriggerType.PointerUp, delegate { OnPointerUp(uiWindow); });
        }

        LoadWindowPosition();
    }

    private void OnEnable()
    {
        LoadWindowPosition();
    }

    private void OnDisable()
    {
        SaveWindowPosition();

    }

    private void OnApplicationQuit()
    {
       SaveWindowPosition();
    }

    private void OnPointerDown(GameObject go)
    {
        Debug.Log("move Down");
        offset = uiWindow.transform.position - Input.mousePosition;
    }

    private void OnPointerUp(GameObject go)
    {
        offset = Vector3.zero;
    }

    private void OnDrag(GameObject go)
    {
        Vector3 mousePosition = Input.mousePosition;
        uiWindow.transform.position = mousePosition + offset;
    }


    private void ResetPosition()
    {
        uiWindow.transform.position = windowResetPosition;
    }


    public void SaveWindowPosition()
    {
        int index = SaveManager.Instance.SaveSlotIndex;
        if (savePositionXName + index == string.Empty || savePositionYName + index == string.Empty)
            return;

        PlayerPrefs.SetFloat(savePositionXName + index, uiWindow.transform.position.x);
        PlayerPrefs.SetFloat(savePositionYName + index, uiWindow.transform.position.y);
    }

    public void LoadWindowPosition()
    {
        if (initPos)
        {
            Debug.Log("1");
            ResetPosition();
        }
        else
        {
            int index = SaveManager.Instance.SaveSlotIndex;

            if (PlayerPrefs.HasKey(savePositionXName + index) && PlayerPrefs.HasKey(savePositionYName + index)
                && savePositionXName != string.Empty && savePositionYName != string.Empty)
            {
                Debug.Log("2");
                uiWindow.transform.position = new Vector3(PlayerPrefs.GetFloat(savePositionXName + index), PlayerPrefs.GetFloat(savePositionYName + index), 0);
            }
            else
            {
                Debug.Log("3");
                uiWindow.transform.position = windowResetPosition;
            }
        }
    }

    private void OnDrawGizmos()
    {
        windowPosition = new Vector3(uiWindow.transform.position.x, uiWindow.transform.position.y, 0);
    }

}
