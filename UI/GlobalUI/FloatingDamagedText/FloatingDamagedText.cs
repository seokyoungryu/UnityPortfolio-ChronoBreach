using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum FloatingType
{
    ATTACK = 0,
    CRITICAL = 1,
    MISS = 2,
    BLOCK = 3,
    SKILL =4,
    HEAL = 5,
}

public class FloatingDamagedText : MonoBehaviour
{
    [SerializeField] private TMP_Text damaged_Text = null;
    [SerializeField] private RectTransform thisRectTr = null;
    [SerializeField] private RectTransform containerRectTr = null;
    private Transform targetTransform = null;
    private Transform playerTransform = null;
    public TMP_Text Damaged_Text => damaged_Text;
    private bool enable = false;

    [Header("Floating Type Infos")]
    [SerializeField] private List<FloatingColorInfo> infos;

    [Header("Offset Random Position Settings")]
    [SerializeField] private float offsetMinX = 0.5f;
    [SerializeField] private float offsetMaxX = 1.5f;
    [SerializeField] private float offsetMinY = 0.5f;
    [SerializeField] private float offsetMaxY = 1.5f;

    [Header("Rect Scale Settings")]
    [SerializeField] private float checkDistanceNear;
    [SerializeField] private float checkDistanceNormal;
    [SerializeField] private float checkDistanceFar;
    [SerializeField] private Vector2 rectScaleNear = Vector2.one;
    [SerializeField] private Vector2 rectScaleNormal;
    [SerializeField] private Vector2 rectScaleFar;


    public void SettingText(int damage, Transform targetTransform, FloatingType floatingType )
    {
        if (thisRectTr == null) thisRectTr = GetComponent<RectTransform>();
        if (playerTransform == null) playerTransform = GameManager.Instance.Player.transform;

        StopAllCoroutines();

        enable = true;
        SetFloatingType(damage,floatingType);
        this.targetTransform = targetTransform;
        containerRectTr.localScale = GetRectTransformScale();
        StartCoroutine(UpdateTransfom());
    }

    private IEnumerator UpdateTransfom()
    {
        Vector3 maginPosition = Vector3.zero;
        maginPosition.x = Random.Range(offsetMinX, offsetMaxX);
        maginPosition.y = Random.Range(offsetMinY, offsetMaxY);

        while (enable)
        {
            thisRectTr.transform.position = GameManager.Instance.Cam.MainCam.WorldToScreenPoint(targetTransform.position + maginPosition);
            yield return null;
        }
    }

    private Vector3 GetRectTransformScale()
    {
        float distance = (targetTransform.position - playerTransform.position).magnitude;
        if (distance <= checkDistanceNear)
            return rectScaleNear;
        else if (distance <= checkDistanceNormal)
            return rectScaleNormal;
        else if (distance <= checkDistanceFar || distance >= checkDistanceFar)
            return rectScaleFar;

        return Vector3.zero;
    }
   

    private void SetFloatingType(int dmg, FloatingType type)
    {
        FloatingColorInfo floatingInfo = GetColorInfo(type);
        damaged_Text.color = floatingInfo.textColor;
        damaged_Text.fontSize = floatingInfo.fontSize;
        if (type == FloatingType.ATTACK || type == FloatingType.CRITICAL || type == FloatingType.SKILL)
            damaged_Text.text = dmg.ToString();
        else if (type == FloatingType.MISS)
            damaged_Text.text = "Miss";
        else if (type == FloatingType.BLOCK)
            damaged_Text.text = "Block";
        else if (type == FloatingType.HEAL)
            damaged_Text.text = "+" + dmg.ToString();
    }

    private FloatingColorInfo GetColorInfo(FloatingType type)
    {
        foreach (FloatingColorInfo info in infos)
            if (info.floatingType == type)
                return info;
        return null;
    }

}
                               
[System.Serializable]
public class FloatingColorInfo
{
    public FloatingType floatingType;
    public Color textColor = Color.black;
    public float fontSize = 30f;
}
