using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHpPresenter : MonoBehaviour
{
    [SerializeField] private AIStatus aIStatus = null;
    [SerializeField] private AIHpBarUI aIHpBarUI = null;
    [SerializeField] private AIInfoSetting aIInfoSetting = null;

    private void Awake()
    {
        aIStatus.OnChangedHPUI += ChangeHP;
        aIHpBarUI.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        aIStatus.OnChangedHPUI -= ChangeHP;
    }

    public void ChangeHP(AIStatus status)
    {
        if (aIHpBarUI == null)
            aIHpBarUI = FindObjectOfType<AIHpBarUI>();

        aIInfoSetting.InfoSetting(aIStatus);
        aIHpBarUI.ChangedHP(status);
    }
}
