using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHorizontalLayoutGroup : BaseLayoutGroup
{
    [SerializeField] private float space = 0f;

    [ContextMenu("½ÇÇà")]
    public void Excute()
    {
        if (uiRect == null)
            uiRect = GetComponent<RectTransform>();

        StopAllCoroutines();
        FindUnderChildUI(childRects);
       // AnchorSettings();
        SortLayout(childRects);

    }

    [ContextMenu("Anchor Setting")]
    public void DoAnchorSetting()
    {
        AnchorSettings();
    }

    private void SortLayout(List<RectTransform> childList)
    {
        if (childList.Count < 0) return;

        float paddingBottom = paddings.bottom > 0 ? paddings.bottom : 0;
        float paddingLR = paddings.left > 0 ? paddings.left : -paddings.right;
        float rectWeight = 0f + paddingLR;

        foreach (RectTransform rect in childList)
        {
            rect.anchoredPosition = new Vector2(rectWeight, 0 + paddingBottom);
            rectWeight += (rect.rect.width + space );
        }

        if (useContextSizeFilter && uiRect != null)
            uiRect.sizeDelta = new Vector2(rectWeight, uiRect.sizeDelta.y);

    }

}
