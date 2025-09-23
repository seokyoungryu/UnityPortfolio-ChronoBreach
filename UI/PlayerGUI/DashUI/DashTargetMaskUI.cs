using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashTargetMaskUI : MonoBehaviour
{
    [SerializeField] private GameObject targetingAnimTr = null;
    [SerializeField] private GameObject checkAnimTr = null;

    [SerializeField] private RectTransform targetRect = null;
    [SerializeField] private Image currentMarkImg = null;

    [Header("Dash Color")]
    [SerializeField] private DashTargetColorInfo[] dashColorinfos;
    private float currentPercent = 0f;

    private Transform compareTarget = null;
    public Sprite targetingImage = null;

    private DashTargetColorInfo[] currentRange = new DashTargetColorInfo[2];

    private Image[] targetingImgs;
    public RectTransform TargetRect => targetRect;

    private void Awake()
    {
        targetingImgs = targetingAnimTr.GetComponentsInChildren<Image>();
        checkAnimTr.SetActive(false);
        targetingAnimTr.SetActive(false);

    }

    private void Update()
    {
        if (currentPercent < 0 || currentPercent > 1) return;

        if (currentRange[0] != null && currentRange[1] != null)
            for (int i = 0; i < targetingImgs.Length; i++)
                targetingImgs[i].color = Color.Lerp(currentRange[0].overColor, currentRange[1].overColor, currentPercent);
    }

    public void ClearCompareTarget() => compareTarget = null;

    public void ResetMask(Transform target)
    {
       // currentMarkImg.sprite = baseTargetImage;
        if (compareTarget == null || target != compareTarget)
        {
            compareTarget = target;
            EnterMask();
        }
    }

    private void EnterMask()
    {
        checkAnimTr.SetActive(false);
        checkAnimTr.SetActive(true);
        targetingAnimTr.SetActive(false);
    }


    private (float, float) GetCurrentRange(float currentPercent)
    {
        (float, float) retRange = (0f,0f);
        for (int i = 1; i < dashColorinfos.Length; i++)
        {
            if (i >= dashColorinfos.Length) return retRange;
            if (currentPercent <= dashColorinfos[i].overDistancePercentage) // 
            {
                retRange.Item1 = dashColorinfos[i - 1].overDistancePercentage;
                retRange.Item2 = dashColorinfos[i].overDistancePercentage;
                currentRange[0] = dashColorinfos[i - 1];
                currentRange[1] = dashColorinfos[i];
                return retRange;
            }
        }
        return retRange;
    }

    public void SettingTargetMaskColor(Vector2 percent)
    {
        if (percent.x >= percent.y) currentPercent = percent.x;
        else if (percent.x < percent.y) currentPercent = percent.y;

        (float, float) range = GetCurrentRange(currentPercent);
    }

    public void SettingTargetingImage()
    {
        if (targetingAnimTr.activeInHierarchy) return;
        checkAnimTr.SetActive(false);
        targetingAnimTr.SetActive(true);
    }

    public void SettingCheckImage()
    {
        if (checkAnimTr.activeInHierarchy) return;

        targetingAnimTr.SetActive(false);
        checkAnimTr.SetActive(true);
    }

    public void ResetTargetTr()
    {
        targetingAnimTr.SetActive(false);
    }

}

[System.Serializable]
public class DashTargetColorInfo
{
    public float overDistancePercentage ;
    public Color overColor;
}
