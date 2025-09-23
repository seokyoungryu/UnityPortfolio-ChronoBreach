using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomGridLayoutGroup : BaseLayoutGroup
{
    [Header("Cell Settings")]
    [SerializeField] private float spaceHorizontal = 0;
    [SerializeField] private float spaceVertical = 0;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private bool useChildSameSize_ByCellSize = false;
    [SerializeField] private bool useChildSameSize_ByChildSize = false;
    [SerializeField] private int colume = 0;
    [SerializeField] private int row = 0;


    public void Update()
    {
        if (isInGameUpdate)
            Do();
    }

    [ContextMenu("½ÇÇà")]
    public void Do()
    {
        if (uiRect == null)
            uiRect = GetComponent<RectTransform>();

        FindUnderChildUI(childRects);
        AnchorSettings();
        SameCellSize();
        SortLayout(childRects);
    }

    [ContextMenu("Anchor Setting")]
    public void DoAnchorSetting()
    {
        AnchorSettings();
    }

    public void ResetData()
    {
        childRects.Clear();
    }


    private void SortLayout(List<RectTransform> rectList)
    {
        if (rectList.Count <= 0) 
        {
            uiRect.sizeDelta = new Vector2(0,0);
            return;
        }
        float paddingLR = paddings.left > 0 ? paddings.left : -paddings.right;
        float rectWidth = 0f;
        float rectHeight = 0f + paddings.top;
        int index = 0;

        foreach (RectTransform rect in rectList)
        {
            float paddingBottom = paddings.bottom > 0 ? paddings.bottom : 0;

            if (IsRow())
                RowCalculate(index, ref rectWidth, ref rectHeight, rect);
            else
                ColumeCalculate(index, ref rectWidth, ref rectHeight, rect);

            rect.anchoredPosition = new Vector2(rectWidth + paddingLR, -rectHeight + paddingBottom);
            index++;
        }

        ContentSizeFilter(index, rectWidth, rectHeight);
    }


    #region Colume Row 
    private bool IsRow() => row > 0;
    private bool IsColume() => colume > 0;

    private void ColumeCalculate(int index, ref float rectWidth, ref float rectHeight, RectTransform rect)
    {
        if (index % colume == 0 && index != 0)
        {
            rectWidth = 0f;
            rectHeight += rect.rect.height + spaceVertical;
        }
        else if (index != 0)
            rectWidth += rect.rect.width + spaceHorizontal;


    }

    private void RowCalculate(int index, ref float rectWidth, ref float rectHeight, RectTransform rect)
    {
        if (index % row == 0 && index != 0)
        {
            rectHeight = 0 + paddings.top;
            rectWidth += rect.rect.width + spaceHorizontal;
        }
        else if (index != 0)
            rectHeight += rect.rect.height + spaceVertical;
    }


    private void ContentSizeFilter(int index, float rectWidth, float rectHeight)
    {
        if (useContextSizeFilter && uiRect != null)
        {
            if (IsRow())
            {
                float uiRectWidth = childRects[0].rect.width * RoundToInt(index, row);
                float uiRectHeight = childRects[0].rect.height * (row > index ? index : row);
                uiRect.sizeDelta = new Vector2(rectWidth + childRects[0].rect.width, uiRectHeight);
            }
            else
            {
                float uirectWidth = childRects[0].rect.width * (colume > index ? index : colume);
                float uirectHeight = childRects[0].rect.height * RoundToInt(index, colume);
                uiRect.sizeDelta = new Vector2(uirectWidth, rectHeight + childRects[0].rect.height);
            }
        }
    }

    private int RoundToInt(int index, int columOrRow)
    {
        if (index == 0) return 0;

        float division = (float)index / (float)columOrRow;
        float decimalPoint = division % (int)division;

        if (decimalPoint > 0)
            return (int)division + 1;
        else if (decimalPoint == 0)
            return (int)division;
        else if (columOrRow > index || columOrRow < 0)
            return 1;
        else
            return 1;
    }

    #endregion


    #region Cell Size

    private void SameCellSize()
    {
        if (!useChildSameSize_ByCellSize && !useChildSameSize_ByChildSize) return;

        Vector2 size = Vector2.down;
        if (useChildSameSize_ByChildSize)
            size = new Vector2(childRects[0].sizeDelta.x, childRects[0].sizeDelta.y);
        else if (useChildSameSize_ByCellSize)
            size = new Vector2(cellSize.x, cellSize.y);

        foreach (RectTransform rect in childRects)
            rect.sizeDelta = size;
    }

    #endregion
}
