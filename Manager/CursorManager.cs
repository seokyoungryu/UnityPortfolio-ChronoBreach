using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorMode
{
    HIDE = 0,
    VISIBLE = 1,
    DRAGING = 2,
}

public class CursorManager : Singleton<CursorManager>
{
    public bool useCustomCursor = true;
    public Texture2D customCursor; 
    [SerializeField] private CursorMode currentCursorMode = CursorMode.HIDE;
    [SerializeField] private Vector2 hotSpot;

    public CursorMode CurrentCursorMode => currentCursorMode;

    protected override void Awake()
    {
        base.Awake();
        //Cursor.lockState = CursorLockMode.Locked;
        if (useCustomCursor)
        {
            Vector2 hotSpots = new Vector2(hotSpot.x, hotSpot.y);
            Cursor.SetCursor(customCursor, hotSpots, UnityEngine.CursorMode.Auto);
        }

    }


    [ContextMenu("세팅")]
    private void Setting()
    {
        if (useCustomCursor)
        {
            Vector2 hotSpot = new Vector2(customCursor.width / 2, customCursor.height / 2);
            Cursor.SetCursor(customCursor, hotSpot, UnityEngine.CursorMode.Auto);
        }
    }

    [ContextMenu("세팅2")]
    private void Setting2()
    {
        if (useCustomCursor)
        {
            Vector2 hotSpots = new Vector2(hotSpot.x , hotSpot.y);
            Cursor.SetCursor(customCursor, hotSpots, UnityEngine.CursorMode.Auto);
        }
    }
    public void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentCursorMode = CursorMode.HIDE;
        Cursor.visible = false;
    }


    public void CursorVisible()
    {
        Cursor.lockState = CursorLockMode.None;
        currentCursorMode = CursorMode.VISIBLE;
        Cursor.visible = true;
    }


}
