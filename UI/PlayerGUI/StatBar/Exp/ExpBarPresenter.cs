using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBarPresenter : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus = null;
    [SerializeField] private ExpBarUI expBarUI = null;


    private void Start()
    {
        if (playerStatus == null) playerStatus = FindObjectOfType<PlayerStatus>();
        if (expBarUI == null) expBarUI = FindObjectOfType<ExpBarUI>();

        playerStatus.OnExpUpdate_ += UpdateExpBar;
        playerStatus.OnLevelUpInit_ += InitLevelUpBar;
    }

    private void OnDestroy()
    {
        playerStatus.OnExpUpdate_ -= UpdateExpBar;
        playerStatus.OnLevelUpInit_ -= InitLevelUpBar;
    }

    public void UpdateExpBar(PlayerStatus stats)
    {
        if (expBarUI == null)
            expBarUI = FindObjectOfType<ExpBarUI>();

        expBarUI.UpdateExpBar(stats);
    }
    public void InitLevelUpBar(PlayerStatus stats)
    {
        if (expBarUI == null)
            expBarUI = FindObjectOfType<ExpBarUI>();
        expBarUI.InitLevelUpBar(stats);
    }
}
