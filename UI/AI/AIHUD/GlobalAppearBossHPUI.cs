using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AppearBossData))]
public class GlobalAppearBossHPUI : MonoBehaviour
{
    [SerializeField] private Color increasePrevBarColor;
    [SerializeField] private AppearBossHPBarColorInfo[] bossBarColors = null;

    [SerializeField] private GameObject container = null;
    [SerializeField] private Image hpBarBackground_Img = null;
    [SerializeField] private Image currentHPBar_Img = null;    // 먼저 줄어드는 이미지
    [SerializeField] private Image prevHPBar_Img = null;       // 후에 줄어드는 이미지
    [SerializeField] private TMP_Text enemyName_Text = null;
    [SerializeField] private TMP_Text hpValue_Text = null;
    [SerializeField] private TMP_Text hpBarCount_Text = null;
    [SerializeField] private AppearBossData bossData = null;

    [Header("Reduce Bar")]
    [SerializeField] private float reduceSmoothValue = 2f;
    [SerializeField] private float reduecPrevTime = 0.3f;

    [Header("Increase Bar")]
    [SerializeField] private float increaseSmoothValue = 2f;
    [SerializeField] private float increasePrevTime = 0.3f;

    private AppearBossHPBarColorInfo currentBarColorInfo = null;
    private float currentLimitTimer = 0f;
    private bool isDead = false;

    private void Awake()
    {
        if (bossData == null) bossData = GetComponent<AppearBossData>();
        //ScenesManager.Instance.onExucteInit += () => SetActive(false);
        SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        if (container == null)
            Debug.Log("Appear container NULLL" + "    , " + isActive);
        else
            Debug.Log("Appear container : " + container + " , " + isActive);

        container.SetActive(isActive);
    }

    //실제 데미지할때는 매개변수 AIcontroller로 하기?
    public void SettingInfos(AIController controller)
    {
        if (bossData.target == controller) return;
        SetActive(true);

        bossData.target = controller;
        Debug.Log("Boss TARget : " + container.name + " , " + bossData.target);
        bossData.SettingDatas();
        SettingBarColor(false);

        AIStatus aiStats = controller.aiStatus;
        enemyName_Text.text = aiStats.AICharacteristicNameUI + " " + aiStats.AINameUI;
        hpValue_Text.text = aiStats.CurrentHealth.ToString() + " / " + aiStats.TotalHealth;
        hpBarCount_Text.text = bossData.CurrentHpBarCount.ToString();
        currentHPBar_Img.fillAmount = bossData.CurrentOneLineValue / bossData.OneLineValue;
        prevHPBar_Img.fillAmount = currentHPBar_Img.fillAmount;
    }

    public void ReduceHp(AIController controller)
    {
        if (!container.activeInHierarchy)
            container.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ReduceSmoothHp_Co(controller.aiStatus));
    }

    public void IncreaseHp(AIController controller)
    {
        if (!container.activeInHierarchy)
            container.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(IncreaseSmoothHp_Co(controller.aiStatus));
    }

    private IEnumerator ReduceSmoothHp_Co(AIStatus aiStats)
    {
        Debug.Log("<color=red> 리듀스 시작</color>");

        bossData.SettingDatas();
        SettingBarColor(false);
        hpValue_Text.text = aiStats.CurrentHealth.ToString() + " / " + aiStats.TotalHealth;
        hpBarCount_Text.text = bossData.CurrentHpBarCount == 1 ? string.Empty : bossData.CurrentHpBarCount.ToString();
        currentHPBar_Img.fillAmount = bossData.CurrentOneLineValue / bossData.OneLineValue;
       if (prevHPBar_Img.fillAmount < currentHPBar_Img.fillAmount)
           prevHPBar_Img.fillAmount = 1f;

        yield return new WaitForSeconds(reduecPrevTime);

        int loop = 0;
        float epsilon = 0.001f; // 허용 가능한 오차 범위

        while (Mathf.Abs(prevHPBar_Img.fillAmount - currentHPBar_Img.fillAmount) > epsilon)
        {
            prevHPBar_Img.fillAmount = Mathf.Lerp(prevHPBar_Img.fillAmount, currentHPBar_Img.fillAmount, Time.deltaTime * reduceSmoothValue);
            yield return null;
            //Debug.Log("<color=red> 리듀스 중...</color>");

            loop++;
            if (loop >= 1000)
            {
                //Debug.LogError("무한루프");
                break;
            }
        }

        prevHPBar_Img.fillAmount = currentHPBar_Img.fillAmount;

        if (bossData.CurrentHpValue <= 0f)
        {
            Debug.Log("Excute Dead HP!!!!!");
            StartCoroutine(DeadProcess_Co());
        }
        
    }

    private IEnumerator IncreaseSmoothHp_Co(AIStatus aiStats)
    {
        bossData.SettingDatas();
        SettingBarColor(true);
        hpValue_Text.text = aiStats.CurrentHealth.ToString() + " / " + aiStats.TotalHealth;
        hpBarCount_Text.text = bossData.CurrentHpBarCount == 1 ? string.Empty : bossData.CurrentHpBarCount.ToString();
        prevHPBar_Img.fillAmount = bossData.CurrentOneLineValue / bossData.OneLineValue;
        if (prevHPBar_Img.fillAmount < currentHPBar_Img.fillAmount)
            currentHPBar_Img.fillAmount = 0f;

        yield return new WaitForSeconds(increasePrevTime);

        int loop = 0;
        while (currentHPBar_Img.fillAmount < (prevHPBar_Img.fillAmount - 0.01f))
        {
            currentHPBar_Img.fillAmount = Mathf.Lerp(currentHPBar_Img.fillAmount, prevHPBar_Img.fillAmount, Time.deltaTime * increaseSmoothValue);
            yield return null;
            loop++;
            if (loop >= 10000)
            {
                Debug.LogError("무한루프");
                break;
            }
        }

        currentHPBar_Img.fillAmount = prevHPBar_Img.fillAmount;

    }

    private AppearBossHPBarColorInfo GetCurrentBarColorInfo(int barCount)
    {
        return bossBarColors[0].GetLessHpBarCountInfo(bossBarColors, barCount);
    }

    private void SettingBarColor(bool isIncrease)
    {
        currentBarColorInfo = GetCurrentBarColorInfo(bossData.CurrentHpBarCount);
        currentHPBar_Img.color = currentBarColorInfo.CurrentHpBarColor;
        hpBarBackground_Img.color = currentBarColorInfo.BackgroundColor;
        if(isIncrease)
            prevHPBar_Img.color = increasePrevBarColor;
        else
            prevHPBar_Img.color = currentBarColorInfo.PrevHpBarColor;

    }


    private IEnumerator DeadProcess_Co()
    {
        Debug.Log("Excute Dead HP  인");

        yield return new WaitForSeconds(7f);
        Debug.Log("Excute Dead HP  아웃");

        SetActive(false);
    }
  
}
