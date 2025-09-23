using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSuccessCountUI : MonoBehaviour
{
    private const string completeTrigger = "Complete";

    [SerializeField] private Animator countContainerUIAnim = null;
    [SerializeField] private PlayerStateController controller = null;
    [SerializeField] private float activeTime = 1.5f;
    private float currentActiveTimer = 0f;
    private bool isStartActive = false;

    [Header("Transform")]
    [SerializeField] private Transform countUIsTr = null;
    [SerializeField] private Transform usedUIsTr = null;

    [Header("SuccessCount Infos")]
    [SerializeField] private DashSuccessCountInfo[] animInfos = null;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color completeColor;
    [SerializeField] private Color usedColor;
    [SerializeField] private SoundList baseSuccessSound = SoundList.None;
    [SerializeField] private SoundList completeSuccessSound = SoundList.None;

    private int completeCount = 0;

    private Image[] successCountUIImages;
    private Image[] usedBackgroundUIImages;


    public int i = 0;  //테스트용 지우기.

    [Header("Fade Out")]
    [SerializeField] private float fadeDelay = 0.1f;
    [SerializeField] private int fadePerValue = 1;


    private IEnumerator fadeOut_Co;

    private void Awake()
    {
        if (controller == null)
            controller = GameManager.Instance.Player;

        successCountUIImages = countUIsTr.GetComponentsInChildren<Image>();
        usedBackgroundUIImages = usedUIsTr.GetComponentsInChildren<Image>();
        completeCount = GetCompleteCount();
        Clear();
        Debug.Log("complete Count :" + completeCount);
    }

    private void Start()
    {
        if (controller == null)
            controller = GameManager.Instance.Player;
        controller.Conditions.OnSuccessDashUpdate_ += ExcuteActiveUI;
    }

    private void OnDestroy()
    {
        controller.Conditions.OnSuccessDashUpdate_ -= ExcuteActiveUI;

    }

    private void Update()
    {
      // if(Input.GetKeyDown(KeyCode.Alpha6))      //succesCounter했을때. 1번 실행.
      // {
      //     i++;
      //     ExcuteActiveUI(i);
      // }
      //
      // if (Input.GetKeyDown(KeyCode.Alpha7))
      //     Clear();

       if(isStartActive)
        {
            currentActiveTimer += Time.deltaTime;
            if(currentActiveTimer >= activeTime)
            {
                StopFade();
                StartCoroutine(fadeOut_Co);
            }
        }
    }
     
    
    public void UsedUI()
    {
        Clear();
        ActiveUsedBackgroundUI(true);
        fadeOut_Co = FadeOut(true);
        StopFade();
        FadeOn();
        ChangeImagesColor(usedBackgroundUIImages,usedColor,true);
    }

    public void ExcuteActiveUI(int successCount)
    {
        ActiveUsedBackgroundUI(false);
        if (fadeOut_Co != null)
            StopCoroutine(fadeOut_Co);
        fadeOut_Co = FadeOut(false);

        DashSuccessCountInfo info = FindInfo(successCount);
        if (info != null)
        {
            StopCoroutine(fadeOut_Co);
            info.countGos.SetActive(true);
            ActiveImageAlpha(successCountUIImages);
           
            if (successCount == completeCount)
            {
                ChangeImagesColor(successCountUIImages,completeColor, true);
                StopFade();
                SoundManager.Instance.PlayEffect(completeSuccessSound);
                countContainerUIAnim.SetTrigger(completeTrigger);
            }
            else
            {
                FadeOn();
                ChangeImagesColor(successCountUIImages,baseColor, true);
                SoundManager.Instance.PlayEffect(baseSuccessSound);
            }

        }

    }

    private DashSuccessCountInfo FindInfo(int successCount)
    {
        for (int i = 0; i < animInfos.Length; i++)
            if (animInfos[i].successCount == successCount)
                return animInfos[i];
        return null;
    }

    private void FadeOn()
    {
        currentActiveTimer = 0f;
        isStartActive = true;
    }

    private void StopFade()
    {
        currentActiveTimer = 0f;
        isStartActive = false;
        StopCoroutine(fadeOut_Co);
      
    }

    private int GetCompleteCount()
    {
        int retCount = 0;
        for (int i = 0; i < animInfos.Length; i++)
            if (animInfos[i].successCount > retCount)
                retCount = animInfos[i].successCount;

        return retCount;
    }

    private void Clear()
    {
        i = 0;
        ActiveSuccessCountUI(false);
        ActiveUsedBackgroundUI(false);
    }

    private void ActiveAll()
    {
        i = 0;
        ActiveSuccessCountUI(true);
        ActiveUsedBackgroundUI(true);
    }

    private void ActiveSuccessCountUI(bool active)
    {
        ActiveImageAlpha(successCountUIImages);
        for (int i = 0; i < animInfos.Length; i++)
            if (animInfos[i].countGos != null)
                animInfos[i].countGos.SetActive(active);
    }

    private void ActiveUsedBackgroundUI(bool active)
    {
        ActiveImageAlpha(usedBackgroundUIImages);
        usedUIsTr.gameObject.SetActive(active);
    }


    private void ChangeImagesColor(Image[] images,Color color, bool changeIncludeAlpha)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (changeIncludeAlpha)
                images[i].color = color;
            else
            {
                Color tmpColor = color;
                tmpColor.a = images[i].color.a;
                images[i].color = tmpColor;
            }
        }
    }

    private void ActiveImageAlpha(Image[] images)
    {
        for (int i = 0; i < images.Length; i++)
        {
            Color color = images[i].color;
            color.a = 1f;
            images[i].color = color;
        }    
    }

    private void ChangeImageAlpha(int alpha, Image[] images)
    {
        float percent = (float)alpha / 255f;
        for (int i = 0; i < images.Length; i++)
        {
            Color color = images[i].color;
            color.a = percent;
            images[i].color = color;
        }
    }

    private IEnumerator FadeOut(bool isBackGround )
    {
        int cAlpha = 255;
        while(cAlpha > 0)
        {
            cAlpha -= fadePerValue;
            if (isBackGround)
                ChangeImageAlpha(cAlpha, usedBackgroundUIImages);
            else
                ChangeImageAlpha(cAlpha, successCountUIImages);
            yield return new WaitForSeconds(fadeDelay);
        }
    }
}


[System.Serializable]
public class DashSuccessCountInfo
{
    public int successCount = 0;
    public GameObject countGos = null;

}