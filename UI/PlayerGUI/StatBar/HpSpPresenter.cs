using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSpPresenter : MonoBehaviour
{
    [SerializeField] private PlayerHpUIBar playerHpUIBar = null;
    [SerializeField] private PlayerSpUIBar playerSpUIBar = null;
    [SerializeField] private PlayerStatus playerStatus = null;

    private void Start()
    {
        if (playerHpUIBar == null) playerHpUIBar = FindObjectOfType<PlayerHpUIBar>();
        if (playerSpUIBar == null) playerSpUIBar = FindObjectOfType<PlayerSpUIBar>();
        if (playerStatus == null) playerStatus = GameManager.Instance.Player.playerStats;


        playerStatus.OnInit_ += playerHpUIBar.InitImageFillAmount;
        playerStatus.OnHpChanged_ += ChangeHpUI;
        playerStatus.OnSpChanged_ += ChangeSpUI;
    }

    private void OnDestroy()
    {
        playerStatus.OnInit_ -= playerHpUIBar.InitImageFillAmount;
        playerStatus.OnHpChanged_ -= ChangeHpUI;
        playerStatus.OnSpChanged_ -= ChangeSpUI;
    }

    public void ChangeHpUI(PlayerStatus stats)
    {
        if (playerHpUIBar == null)
            playerHpUIBar = FindObjectOfType<PlayerHpUIBar>();

            playerHpUIBar.ChangeHPUI(stats);
    }
    public void ChangeSpUI(PlayerStatus stats)
    {
        if (playerSpUIBar == null)
            playerSpUIBar = FindObjectOfType<PlayerSpUIBar>();
            playerSpUIBar.OnSpChanged(stats);
    }

}
