using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSizeFitter : MonoBehaviour
{
    public enum TextSizeFitterType
    {
        FIT_BOTH,
        FIT_HORIZONTAL,
        FIT_VERTICAL,
    }
    [SerializeField] private Vector2 paddingOffset;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private TextSizeFitterType fitType;
    void Awake()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
        if (tmpText == null)
            tmpText = GetComponent<TMP_Text>();
    }

   


    [ContextMenu("실행")]
    public void ExcuteFitToText()
    {
        tmpText.ForceMeshUpdate(); // 텍스트 정보를 갱신해

        Vector2 textSize = tmpText.GetRenderedValues(false); // 실제 출력된 텍스트 크기
        if (fitType == TextSizeFitterType.FIT_BOTH)
            rectTransform.sizeDelta = new Vector2(textSize.x + paddingOffset.x, textSize.y + paddingOffset.y);
        else if (fitType == TextSizeFitterType.FIT_HORIZONTAL)
            rectTransform.sizeDelta = new Vector2(textSize.x + paddingOffset.x, rectTransform.sizeDelta.y + paddingOffset.y);
        else if (fitType == TextSizeFitterType.FIT_VERTICAL)
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x + paddingOffset.x, textSize.y + paddingOffset.y);
    }
}
