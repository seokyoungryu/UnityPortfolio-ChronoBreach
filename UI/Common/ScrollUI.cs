using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollUI : MonoBehaviour
{
    public enum ScrollType { ONLY_DRAG, ONLY_WHEEL, BOTH}

    [Header("고정된 rect")]
    [SerializeField] private RectTransform rootRect = null;
    [Header("실제 드래그될 타겟 rect")]
    [SerializeField] private RectTransform targetRect = null;  
    [Header("스크롤바 존재하면 등록")]
    [SerializeField] private RectTransform scrollbarBackground = null;
    [SerializeField] private RectTransform scrollbarHandler = null;

    [Header("Variables")]
    [SerializeField] private bool reverseRect = false;
    [SerializeField] private bool reverseWheel = false;
    [SerializeField] private bool scrollOverRootRect = false;
    [SerializeField] private bool scrollOverRootRect_BarActive = false;
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private float smoothDamp = 2f;
    [SerializeField] private bool isScrollHorizontal = false;
    [SerializeField] private bool isUpdateSize = true;

    public float currentScrollValue = 0f;
    private float minScrollValue = 0f;
    private float maxScrollValue = 1f;
    public float barSize = 0f;
    public float limitMaxViewValue = 0f;
    public float limitMaxBarValue = 0f;
    public float barOriginalYSize = 0f;
    private float barOriginalXSize = 0f;
    private float adjustedSensitivity = 0f;
    private bool isMouseEnter = false;
    private float dragOffsetY = 0f;
    public float currBarY = 0f;
    public float percent = 0f;
    public float mousePer = 0f;

    public float testMousePos = 0f;
    public float barMaxPos = 0f;
    public float barMinPos = 0f;
    public float middlePos = 0f;

    private float dragStartPos = 0f;


    private void Awake()
    {
        UIHelper.AddEventTrigger(rootRect.gameObject, EventTriggerType.PointerEnter, delegate { OnPointerEnter();});
        UIHelper.AddEventTrigger(rootRect.gameObject, EventTriggerType.PointerExit, delegate { OnPointerExit(); });
        if (scrollbarHandler != null)
        {
            UIHelper.AddEventTrigger(scrollbarHandler.gameObject, EventTriggerType.BeginDrag, delegate { HandlerDragStart(); });
            UIHelper.AddEventTrigger(scrollbarHandler.gameObject, EventTriggerType.Drag, delegate { HandlerDrag(); });

        }

        if (scrollbarBackground != null && !isScrollHorizontal)
            barOriginalYSize = scrollbarBackground.sizeDelta.y;
        else if (scrollbarBackground != null && isScrollHorizontal)
            barOriginalXSize = scrollbarBackground.sizeDelta.x;

        limitMaxViewValue = GetMaxScrollValue();

        if (scrollbarHandler != null)
        {
            barMaxPos = scrollbarHandler.transform.position.y + barOriginalYSize;
            barMinPos = scrollbarHandler.transform.position.y;
        }

    }

    private void Update()
    {
        if (scrollbarHandler != null)
        {
            testMousePos = Input.mousePosition.y;
            currBarY = scrollbarHandler.transform.position.y;
            middlePos = scrollbarHandler.transform.position.y + (barSize / 2f);
            percent = Mathf.InverseLerp(0f, barOriginalYSize - scrollbarHandler.rect.height, scrollbarHandler.anchoredPosition.y);
        }

        if (isUpdateSize)
            limitMaxViewValue = GetMaxScrollValue();

        if (scrollOverRootRect && limitMaxViewValue <= 0)
        {
            if (scrollOverRootRect_BarActive)
            {
               if(scrollbarHandler.gameObject.activeInHierarchy) scrollbarHandler.gameObject.SetActive(false);
                if (scrollbarBackground.gameObject.activeInHierarchy) scrollbarBackground.gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            if (scrollOverRootRect_BarActive)
            {
                if (scrollbarHandler != null && !scrollbarHandler.gameObject.activeInHierarchy) scrollbarHandler?.gameObject.SetActive(true);
                if (scrollbarBackground != null && !scrollbarBackground.gameObject.activeInHierarchy) scrollbarBackground?.gameObject.SetActive(true);
            }
        }

        if (isMouseEnter)
        {
            adjustedSensitivity = sensitivity;
            if (limitMaxViewValue != 0)
                adjustedSensitivity = sensitivity * (1f / limitMaxViewValue);

            if (reverseWheel)
                currentScrollValue -= Input.GetAxisRaw("Mouse ScrollWheel") * adjustedSensitivity;
            else
                currentScrollValue += Input.GetAxisRaw("Mouse ScrollWheel") * adjustedSensitivity;

            currentScrollValue = Mathf.Clamp(currentScrollValue, minScrollValue, maxScrollValue);
        }

        if (!isScrollHorizontal)
        {
            if(reverseRect)
                targetRect.anchoredPosition = Vector3.Lerp(targetRect.anchoredPosition, Vector3.down * currentScrollValue * limitMaxViewValue, Time.deltaTime * smoothDamp);
            else
                targetRect.anchoredPosition = Vector3.Lerp(targetRect.anchoredPosition, Vector3.up * currentScrollValue * limitMaxViewValue, Time.deltaTime * smoothDamp);
        }
        else
        {
            if (reverseRect)
                targetRect.anchoredPosition = Vector3.Lerp(targetRect.anchoredPosition, Vector3.right * currentScrollValue * limitMaxViewValue, Time.deltaTime * smoothDamp);
            else
                targetRect.anchoredPosition = Vector3.Lerp(targetRect.anchoredPosition, Vector3.left * currentScrollValue * limitMaxViewValue, Time.deltaTime * smoothDamp);

        }

        if (scrollbarHandler != null && scrollbarBackground != null)
        {
            if (isScrollHorizontal)
            {
                SetScrollBarSizeX();
                if (reverseRect)
                    scrollbarHandler.anchoredPosition = Vector3.Lerp(scrollbarHandler.anchoredPosition, Vector3.left * currentScrollValue * limitMaxBarValue, Time.deltaTime * smoothDamp);
                else
                scrollbarHandler.anchoredPosition = Vector3.Lerp(scrollbarHandler.anchoredPosition, -Vector3.left * currentScrollValue * limitMaxBarValue, Time.deltaTime * smoothDamp);

            }
            else
            {
                SetScrollBarSizeY();
                if (reverseRect)
                    scrollbarHandler.anchoredPosition = Vector3.Lerp(scrollbarHandler.anchoredPosition, Vector3.up * currentScrollValue * limitMaxBarValue, Time.deltaTime * smoothDamp);
                else
                    scrollbarHandler.anchoredPosition = Vector3.Lerp(scrollbarHandler.anchoredPosition, -Vector3.up * currentScrollValue * limitMaxBarValue, Time.deltaTime * smoothDamp);

            }
        }
    }

    public void AddCurrentScrollValue(float value)
    {
        currentScrollValue += value;
    }

    public void ResetPosition()
    {
        currentScrollValue = 0f;
        targetRect.anchoredPosition = Vector3.zero;
        scrollbarHandler.anchoredPosition = Vector3.zero;

        if (isScrollHorizontal) SetScrollBarSizeX();
        else SetScrollBarSizeY();
    }


    private float GetMaxScrollValue()
    {
        if (isScrollHorizontal)
        {
            if (targetRect.rect.width > 0 && rootRect.rect.width > 0)
                return targetRect.rect.width - rootRect.rect.width;
            else
                return 0f;
        }
        else
        {
            if (targetRect.rect.height > 0 && rootRect.rect.height > 0)
                return targetRect.rect.height - rootRect.rect.height;
            else
                return 0f;
        }
    }

    public void OnPointerEnter()
    {
        isMouseEnter = true;
    }
    public void OnPointerExit()
    {
        isMouseEnter = false;
    }


    public void SetScrollBarSizeY()
    {
        if (scrollbarBackground == null || scrollbarHandler == null) return;

        limitMaxBarValue = barOriginalYSize * (GetMaxScrollValue() / targetRect.rect.height);
        limitMaxBarValue = Mathf.Max(limitMaxBarValue, 0f); // limitMaxBarValue가 음수일 경우 0으로 설정

        barSize = barOriginalYSize - limitMaxBarValue;
        barSize = Mathf.Max(barSize, 0f); // barSize가 음수일 경우 0으로 설정
        scrollbarHandler.sizeDelta = new Vector2(scrollbarHandler.sizeDelta.x, barSize);
    }

    public void SetScrollBarSizeX()
    {
        if (scrollbarBackground == null || scrollbarHandler == null) return;

        limitMaxBarValue = barOriginalXSize * (GetMaxScrollValue() / targetRect.rect.width);
        limitMaxBarValue = Mathf.Max(limitMaxBarValue, 0f); // limitMaxBarValue가 음수일 경우 0으로 설정

        barSize = barOriginalXSize - limitMaxBarValue;
        barSize = Mathf.Max(barSize, 0f); // barSize가 음수일 경우 0으로 설정
        scrollbarHandler.sizeDelta = new Vector2(barSize, scrollbarHandler.sizeDelta.y);
    }

    public void HandlerDragStart()
    {
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollbarBackground, Input.mousePosition, null, out localMousePos);
        dragOffsetY = localMousePos.y - scrollbarHandler.anchoredPosition.y;
    }
    public void HandlerDrag()
    {
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollbarBackground, Input.mousePosition, null, out localMousePos);
        Debug.Log("Mouse : " + localMousePos);


        // 핸들을 마우스 위치 - offset만큼 따라가도록
        float handlePosY = localMousePos.y - dragOffsetY;

        // 스크롤 영역 내에서의 비율(0~1) 계산
        float availableHeight = scrollbarBackground.rect.height - scrollbarHandler.rect.height;
        float clampedHandleY = Mathf.Clamp(handlePosY, 0f, availableHeight);

        scrollbarHandler.anchoredPosition = new Vector2(scrollbarHandler.anchoredPosition.x, clampedHandleY);

        // 스크롤 비율 갱신
        currentScrollValue = clampedHandleY / availableHeight;
        currentScrollValue = Mathf.Clamp01(currentScrollValue);
    }
   
}
