using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsPresenter : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus = null;
    [SerializeField] private PlayerStatsUI playerStatsUI = null;


    private void Awake()
    {
        if (playerStatus == null) playerStatus = GameManager.Instance.Player.playerStats;
        if (playerStatsUI == null) playerStatsUI = FindObjectOfType<PlayerStatsUI>();

        GameManager.Instance.onPlayerStatsUpdate += playerStatsUI.UpdateStatsUIs;
        GameManager.Instance.onPlayerStatsUpdate += playerStatsUI.UpdateStatInformationDatas;
        GameManager.Instance.onPlayerStatsUpdate += playerStatsUI.UpdateDetailStatsInfos;

        playerStatus.OnUpdateFunctionUIs_ += playerStatsUI.UpdateStatsUIs;
        playerStatus.OnUpdateStatInfos_ += playerStatsUI.UpdateStatInformationDatas;
        playerStatus.OnUpdateStatInfos_ += playerStatsUI.UpdateDetailStatsInfos;

        playerStatsUI.UpdateStatInformationDatas(playerStatus);
        playerStatsUI.UpdateDetailStatsInfos(playerStatus);
    }


    private void OnDestroy()
    {
        GameManager.Instance.onPlayerStatsUpdate -= playerStatsUI.UpdateStatsUIs;
        GameManager.Instance.onPlayerStatsUpdate -= playerStatsUI.UpdateStatInformationDatas;
        GameManager.Instance.onPlayerStatsUpdate -= playerStatsUI.UpdateDetailStatsInfos;

        playerStatus.OnUpdateFunctionUIs_ -= playerStatsUI.UpdateStatsUIs;
        playerStatus.OnUpdateStatInfos_ -= playerStatsUI.UpdateStatInformationDatas;
        playerStatus.OnUpdateStatInfos_ -= playerStatsUI.UpdateDetailStatsInfos;
    }
}
