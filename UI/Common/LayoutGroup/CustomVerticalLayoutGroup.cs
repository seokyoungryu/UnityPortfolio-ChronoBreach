using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CustomVerticalLayoutGroup : BaseLayoutGroup
{
    [SerializeField] private float space = 0f;
    // 생성될때, or 삭제될때.
    /// <summary>
    /// 애니메이션 넣거나 위치 수정하고싶을때 -> update 말고 생성될때 호출하기 등으로 실행.
    /// </summary>
    private void Update()
    {
        if (isInGameUpdate)
            Excute();
    }

    [ContextMenu("실행")]
    public void Excute()
    {
        if (uiRect == null)
            uiRect = GetComponent<RectTransform>();

        if (useChildTextSizeFitter)
            FindAndExcuteChildTextSizeFitter();

        StopAllCoroutines();
        FindUnderChildUI(childRects);
        SortLayout(childRects);
    }

    private void SortLayout(List<RectTransform> childList)
    {
        if (childList.Count < 0) return;

        float rectHeight = 0f + paddings.top;
        foreach (RectTransform rect in childList)
        {
            float paddingLR = paddings.left > 0 ? paddings.left : -paddings.right;
            float paddingBottom = paddings.bottom > 0 ? paddings.bottom : 0;
            rect.anchoredPosition = new Vector2(0 + paddingLR,  -rectHeight);
            rectHeight += (rect.rect.height + space + paddingBottom);
        }

        if (useContextSizeFilter && uiRect != null)
            uiRect.sizeDelta = new Vector2(uiRect.sizeDelta.x, rectHeight);

    }

   

    public void QuestCompleteProcess(Quest qeust)
    {
        FindUnderChildUI(childRects);
        StopAllCoroutines();
        QuestCompleteSmoothMove(childRects);
    }

    private void QuestCompleteSmoothMove(List<RectTransform> childList)
    {
        float rectHeight = 0f;

        foreach (RectTransform rect in childList)
        {
            StartCoroutine(QuestCompleteSmoothMove(rect, rectHeight));
            rectHeight += rect.rect.height;
        }

        if (useContextSizeFilter && uiRect != null)
            uiRect.sizeDelta = new Vector2(uiRect.sizeDelta.x, rectHeight);

    }

    IEnumerator QuestCompleteSmoothMove(RectTransform rect, float height)
    {
        Vector2 resultPos = new Vector2(rect.anchoredPosition.x, -height);
        Vector2 resultUIRect = new Vector2(uiRect.sizeDelta.x, height);

        while (Vector2.Distance(rect.anchoredPosition, resultPos) > 0.05f)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, resultPos, Time.deltaTime * 3f);
            yield return null;
        }
        rect.anchoredPosition = resultPos;
    }

}
