using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Padding
{
    public float left;
    public float right;
    public float top;
    public float bottom;
}



public class BaseLayoutGroup : MonoBehaviour
{
    protected bool isInGameUpdate = false;
    protected RectTransform uiRect;
    [SerializeField] protected Padding paddings;
    [SerializeField] protected List<RectTransform> childRects = new List<RectTransform>();
    [SerializeField] protected bool useContextSizeFilter = false;
    [SerializeField] protected bool useChildTextSizeFitter = false;

    protected TextSizeFitter[] textSizeFitters;

    public void FindChildTestSizeFitter()
    {
        textSizeFitters = GetComponentsInChildren<TextSizeFitter>();
    }

    public void ExcuteChildTextSizeFitter()
    {
        for (int i = 0; i < textSizeFitters.Length; i++)
            if (textSizeFitters[i] != null)
                textSizeFitters[i].ExcuteFitToText();
    }

    public void FindAndExcuteChildTextSizeFitter()
    {
        FindChildTestSizeFitter();
        ExcuteChildTextSizeFitter();
    }


    protected void FindUnderChildUI(List<RectTransform> childList)
    {
        childList.Clear();
        foreach (RectTransform rect in this.transform)
            if (rect.gameObject.activeInHierarchy)
                childList.Add(rect);
    }

    protected void AnchorSettings()
    {
        if (uiRect == null)
            uiRect = GetComponent<RectTransform>();
        else if (uiRect != null)
        {
            //uiRect.anchorMin = new Vector2(0, 0.5f);
            //uiRect.anchorMax = new Vector2(0, 0.5f);
            uiRect.pivot = new Vector2(0, 1);
        }

        foreach (RectTransform rect in childRects)
        {
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
        }
    }


}
