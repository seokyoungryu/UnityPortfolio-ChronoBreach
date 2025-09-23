using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTargetMaskUIPresenter : MonoBehaviour
{
    [SerializeField] private DashState dashState = null;
    [SerializeField] private FloatingDamagedTextContainer container = null;


    private void Start()
    {
        if (dashState == null) dashState = GameManager.Instance.Player.GetState<DashState>();

        dashState.OnInitDash_ += container.AllCloseTexts;

    }

    private void OnDestroy()
    {
        dashState.OnInitDash_ -= container.AllCloseTexts;
    }
}
