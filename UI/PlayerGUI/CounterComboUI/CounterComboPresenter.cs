using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterComboPresenter : MonoBehaviour
{
    [SerializeField] private CounterComboUI counterComboUI;
    [SerializeField] private PlayerConditions playerConditions;


    private void Start()
    {
        if (counterComboUI == null) counterComboUI = FindObjectOfType<CounterComboUI>();
        if (playerConditions == null) playerConditions = GameManager.Instance.Player.Conditions;

        playerConditions.OnSuccessCounterUpdate_ += counterComboUI.UpdateText;
    }


    private void OnDestroy()
    {
        playerConditions.OnSuccessCounterUpdate_ -= counterComboUI.UpdateText;
    }
}
