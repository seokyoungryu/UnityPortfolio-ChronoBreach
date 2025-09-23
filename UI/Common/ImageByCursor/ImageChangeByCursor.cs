using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageChangeByCursor : MonoBehaviour
{
    [SerializeField] private ImageChangeSettings imageChangeSettings = null;
    [SerializeField] private Image targetImage = null;

    [SerializeField] private List<ImageChangeByCursor> imageList = new List<ImageChangeByCursor>();
    [SerializeField] private bool isHlight = false;
    [SerializeField] private bool isPressed = false;
    [SerializeField] private bool isSelected = false;

    public List<ImageChangeByCursor> ImageList { get { return imageList; } set { imageList = value; } }
    public Image TargetImage { get { return targetImage; } set { targetImage = value; } }

    private void Awake()
    {
        UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerEnter, delegate { HlightOn(); });
        UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerExit, delegate { HlightOut(); });
        UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerDown, delegate { Pressed(); });
        UIHelper.AddEventTrigger(gameObject, EventTriggerType.PointerUp, delegate { Select(); });
    }


    [ContextMenu("Set NormalColor from targetImage")]
    private void SetNormalSprite()
    {
        imageChangeSettings.SetNormalSprite(targetImage.sprite);
    }

    protected virtual void HlightOn() //OnPointerEnter
    {
        if (isPressed || isSelected) return;

        isHlight = true;
        targetImage.sprite = imageChangeSettings.HlightSprite;

    }

    protected virtual void HlightOut() //OnPointerExit
    {
        if (isPressed || isSelected) return;

        isHlight = false;
        targetImage.sprite = imageChangeSettings.NormalSprite;
    }

    protected virtual void Pressed() //OnPointerDown
    {
        isPressed = true;
        targetImage.sprite = imageChangeSettings.PressedSprite;
    }

    protected virtual void Select()  //OnPointerUp
    {
        AllReset();
        isSelected = true;
        targetImage.sprite = imageChangeSettings.SelectSprite;
    }

    public void ResetState()
    {
        isHlight = false;
        isPressed = false;
        isSelected = false;
    }

    public void AllReset()
    {
        foreach (ImageChangeByCursor image in imageList)
        {
            image.TargetImage.sprite = imageChangeSettings.NormalSprite;
            image.ResetState();
        }
    }
}


[System.Serializable]
public class ImageChangeSettings 
{
    [SerializeField] private Sprite normalSprite = null;
    [SerializeField] private Sprite hlightSprite = null;
    [SerializeField] private Sprite pressedSprite = null;
    [SerializeField] private Sprite selectSprite = null;
    [SerializeField] private Sprite disableSprite = null;


    public Sprite NormalSprite => normalSprite;
    public Sprite HlightSprite => hlightSprite;
    public Sprite PressedSprite => pressedSprite;
    public Sprite SelectSprite => selectSprite;
    public Sprite DisableSprite => disableSprite;


    public void SetNormalSprite(Sprite normalColor) => this.normalSprite = normalColor;
}
