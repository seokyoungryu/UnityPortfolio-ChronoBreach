using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageColorByCursor : MonoBehaviour
{
    [SerializeField] protected ImageColorSettings colorSettings;
    [SerializeField] protected Image targetImage = null;

    [SerializeField] protected List<ImageColorByCursor> imageList = new List<ImageColorByCursor>();
    protected bool isHlight = false;
    protected bool isPressed = false;
    protected bool isSelected = false;

    public List<ImageColorByCursor> ImageList { get { return imageList; } set { imageList = value; } }
    public Image TargetImage { get { return targetImage; } set { targetImage = value; } }

    protected void Awake()
    {
        UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerEnter, delegate { HlightOn(); });
        UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerExit, delegate { HlightOut(); });
        UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerDown, delegate { Pressed(); });
        UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerUp, delegate { Select(); });
    }


    //에러 있음. 음. 선 경계선?에서 마우스를 놓으면 EndDrag가 안됨.

    [ContextMenu("Set NormalColor from targetImage")]
    private void SetNormalColor()
    {
        colorSettings.SetNormalColor(targetImage.color);
    }

    protected virtual void HlightOn() //OnPointerEnter
    {
        if (isPressed || isSelected) return;

        isHlight = true;
        targetImage.color = colorSettings.HlightColor;

    }

    protected virtual void HlightOut() //OnPointerExit
    {
        if (isPressed || isSelected) return;

        isHlight = false;
        targetImage.color = colorSettings.NormalColor;
    }

    protected virtual void Pressed() //OnPointerDown
    {
        isPressed = true;
        targetImage.color = colorSettings.PressedColor;
    }

    protected virtual void Select()  //OnPointerUp
    {
        AllReset();
        isSelected = true;
        targetImage.color = colorSettings.SelectColor;
    }

    public void ResetState()
    {
        isHlight = false;
        isPressed = false;
        isSelected = false;
    }

    public void AllReset()
    {
        foreach (ImageColorByCursor image in imageList)
        {
            image.TargetImage.color = colorSettings.NormalColor;
            image.ResetState();
        }
    }

    public void AddToList(ImageColorByCursor[] images)
    {
        for (int i = 0; i < images.Length; i++)
            imageList.Add(images[i]);
    }

    public void ChangeSelectedUI()
    {
        AllReset();
        isSelected = true;
        targetImage.color = colorSettings.SelectColor;
    }
}

[System.Serializable]
public class ImageColorSettings
{
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hlightColor = Color.white;
    [SerializeField] private Color pressedColor = Color.white;
    [SerializeField] private Color selectColor = Color.white;
    [SerializeField] private Color disableColor = Color.gray;


    public Color NormalColor => normalColor;
    public Color HlightColor => hlightColor;
    public Color PressedColor => pressedColor;
    public Color SelectColor => selectColor;
    public Color DisableColor => disableColor;


    public void SetNormalColor(Color normalColor) => this.normalColor = normalColor;
}



