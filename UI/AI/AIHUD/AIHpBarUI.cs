using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHpBarUI : BaseHpUIBar
{
    [SerializeField] private Image hpBar_Img = null;
    [SerializeField] private Image preHpBa_Img = null;
    [SerializeField] private float gameobjectActiveTime = 2.5f;
    [SerializeField] private float currentActiveTimer = 0f;

    private bool isHide = false;
    private IEnumerator activeCoroutine = null;
    private IEnumerator changeCoroutine = null;
   

    public bool IsHide { get { return isHide; } set { isHide = value; } }

    private void Awake()
    {
        activeCoroutine = ActiveFalse();
    }

    public void ChangedHP(AIStatus aiStatus)
    {
        isDamaged = false;
        if (!aiStatus.gameObject.activeInHierarchy)
        {
            Debug.Log("HP Set False¿”");
            return;
        }

        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);
        activeCoroutine = ActiveFalse();

        currentTimer = 0f;
        currentActiveTimer = 0f;
        hpBar_Img.fillAmount = (float)aiStatus.CurrentHealth / (float)aiStatus.CurrentMaxHealth;

        if (!gameObject.activeSelf) gameObject.SetActive(true);
        if (!isDamaged)
        {
            if (changeCoroutine != null)
                StopCoroutine(changeCoroutine);
            changeCoroutine = SmoothHpReduce(preHpBa_Img, hpBar_Img);
            StartCoroutine(changeCoroutine);
        }

        //Debug.Log("ChangeHP");
        isDamaged = true;
    }

    protected override void ExecutionEndReduce()
    {
        StartCoroutine(activeCoroutine);
    }

    private IEnumerator ActiveFalse()
    {
        currentActiveTimer = 0f;
        while (currentActiveTimer < gameobjectActiveTime)
        {
            if (isHide) gameObject.SetActive(false);
           // else  gameObject.SetActive(true);

            currentActiveTimer += Time.deltaTime;
            yield return null;
        }
        if (changeCoroutine != null)
            StopCoroutine(changeCoroutine);
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);
        gameObject.SetActive(false);
    }


}
