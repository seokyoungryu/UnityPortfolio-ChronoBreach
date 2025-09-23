using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBarUI : MonoBehaviour
{
    [SerializeField] private Image expBar_Img = null;
    [SerializeField] private TMP_Text lv_Text = null;
    [SerializeField] private TMP_Text exp_Text = null;
    [SerializeField] private float smoothBarSpeed = 0f;


    private void Start()
    {
        if (GameManager.Instance.Player)
            LoadHUD(GameManager.Instance.Player.playerStats);


    }


    public void LoadHUD(PlayerStatus playerStatus)
    {
        lv_Text.text = "Lv." + playerStatus.Level;
        exp_Text.text = playerStatus.CurrentExp + " / " + playerStatus.NextExp.RequiredExp
            + $"( {string.Format("{0:0.00}", (playerStatus.CurrentExp / (float)playerStatus.NextExp.RequiredExp) * 100)}% )";
    }

    public void InitLevelUpBar(PlayerStatus playerStatus)
    {
         expBar_Img.fillAmount = 0;
         lv_Text.text = "Lv." + playerStatus.Level;
         exp_Text.text = playerStatus.CurrentExp + " / " + playerStatus.NextExp.RequiredExp
             + $"( {string.Format("{0:0.00}", (playerStatus.CurrentExp / (float)playerStatus.NextExp.RequiredExp) * 100)}% )";
        StartCoroutine(LevelUpProcess(playerStatus));

    }

    public void UpdateExpBar(PlayerStatus playerStatus)
    {
        StopAllCoroutines();
        lv_Text.text = "Lv." + playerStatus.Level;
        exp_Text.text = playerStatus.CurrentExp + " / " + playerStatus.NextExp.RequiredExp 
            + $"( {string.Format("{0:0.00}", (playerStatus.CurrentExp / (float)playerStatus.NextExp.RequiredExp) * 100)}% )";
        StartCoroutine(ExpProcess(playerStatus));
    }

    private IEnumerator ExpProcess(PlayerStatus playerStatus)
    {
        float targetAmount = playerStatus.CurrentExp / (float)playerStatus.NextExp.RequiredExp;
        while (expBar_Img.fillAmount < targetAmount)
        {
            expBar_Img.fillAmount = Mathf.Lerp(expBar_Img.fillAmount, targetAmount, Time.deltaTime * smoothBarSpeed);
            yield return null;
        }
    }

    private IEnumerator LevelUpProcess(PlayerStatus playerStatus)
    {
        float targetAmount = 1f;
        while (expBar_Img.fillAmount < targetAmount)
        {
            expBar_Img.fillAmount = Mathf.Lerp(expBar_Img.fillAmount, targetAmount, Time.deltaTime * smoothBarSpeed);
            yield return null;
        }
    }
}
