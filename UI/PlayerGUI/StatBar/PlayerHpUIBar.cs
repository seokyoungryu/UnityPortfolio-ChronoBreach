using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUIBar : BaseHpUIBar
{
    [SerializeField] private Image hpBar_Img = null;
    [SerializeField] private Image previewHpBar_Img = null;

    public void InitImageFillAmount(PlayerStatus playerStatus)
    {
        float HpValue = (float)playerStatus.CurrentHealth / (float)playerStatus.TotalHealth;

        hpBar_Img.fillAmount = HpValue;
        previewHpBar_Img.fillAmount = HpValue;
    }

    public void ChangeHPUI(PlayerStatus playerStatus)
    {
        if (playerStatus == null) return;

        StopAllCoroutines();
        realTimer = 0f;

        float tmpHpValue = (float)playerStatus.CurrentHealth / (float)playerStatus.TotalHealth;

        if (tmpHpValue >= hpBar_Img.fillAmount)   // tmpHpValue 0.9 >= hpBar_Img 0.6  (힐 받음)
        {
            previewHpBar_Img.fillAmount = tmpHpValue;
            StartCoroutine(SmoothHpIncrease(hpBar_Img, previewHpBar_Img));
        }
        else if (tmpHpValue < hpBar_Img.fillAmount) // tmpHpValue 0.6 < hpBar_Img 0.9 ( 데미지 받음)
        {
            hpBar_Img.fillAmount = tmpHpValue;
            StartCoroutine(SmoothHpReduce(previewHpBar_Img, hpBar_Img));
        }

    }


}
