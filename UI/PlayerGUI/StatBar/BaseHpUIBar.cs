using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHpUIBar : MonoBehaviour
{
    [SerializeField] protected float maxTimer = 3f;
    [SerializeField] protected float activeFalseCycleTime = 1f;
    [SerializeField] protected float lerpSpeed = 2f;

    protected bool isDamaged = false;

    [SerializeField] protected float currentTimer = 0f;
    [SerializeField] protected float realTimer = 0f;


    protected IEnumerator SmoothHpReduce(Image preHpBar, Image hpBar)
    {
        isDamaged = true;
        while (realTimer <= maxTimer || currentTimer <= activeFalseCycleTime)
        {
            realTimer += Time.deltaTime;
            currentTimer += Time.deltaTime;
            if (realTimer >= maxTimer || currentTimer >= activeFalseCycleTime)
                continue;
            yield return null;
        }

        while (preHpBar.fillAmount >= (hpBar.fillAmount + 0.01f))
        {
            Debug.Log($"<color=red> preHpBar SmoothHpReduce... </color>");
            preHpBar.fillAmount = Mathf.Lerp(preHpBar.fillAmount, hpBar.fillAmount, Time.deltaTime * lerpSpeed);
            yield return null;
        }

        preHpBar.fillAmount = hpBar.fillAmount;
        realTimer = 0f;
        currentTimer = 0f;
        isDamaged = false;
        Debug.Log($"<color=white> SmoothHpReduce  isDamaged {isDamaged} </color>");
        ExecutionEndReduce();
    }

    protected IEnumerator SmoothHpIncrease(Image hpBar, Image preHpBar)
    {
        while (realTimer <= maxTimer)
        {
            realTimer += Time.deltaTime;
            if (realTimer >= maxTimer)
                continue;
            yield return null;
        }

        while (hpBar.fillAmount <= (preHpBar.fillAmount + 0.01f))
        {
            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, preHpBar.fillAmount, Time.deltaTime * lerpSpeed);
            yield return null;
        }

        hpBar.fillAmount = preHpBar.fillAmount;
        realTimer = 0f;
    }
    protected virtual void ExecutionEndReduce() { }




}
