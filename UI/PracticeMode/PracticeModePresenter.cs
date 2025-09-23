using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeModePresenter : MonoBehaviour
{
    [SerializeField] private BasePracticeModeProcess process;
    [SerializeField] private NoramlExcutePracticeModeUI ui;


    private void Awake()
    {
        process.onUpdate += ui.UpdateEnemy;
        process.onUpdate += ui.UpdateScareCrow;
        process.onUpdate += ui.UpdateGold;
        process.onUpdate += ui.UpdateLv;
        process.onUpdate += ui.UpdateShooter;
        process.onUpdate += ui.UpdateStandingBoss;
    }

    private void OnDestroy()
    {
        process.onUpdate -= ui.UpdateEnemy;
        process.onUpdate -= ui.UpdateScareCrow;
        process.onUpdate -= ui.UpdateGold;
        process.onUpdate -= ui.UpdateLv;
        process.onUpdate -= ui.UpdateShooter;
        process.onUpdate -= ui.UpdateStandingBoss;

    }
}
