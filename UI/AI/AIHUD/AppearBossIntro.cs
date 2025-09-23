using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AppearBossIntro : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private Transform[] containers = null;
    [SerializeField] private TMP_Text characteristicsName_Text = null;
    [SerializeField] private TMP_Text originalName_Text = null;
    [SerializeField] private float introTime = 3f;
    private string trigger = "Excute";
    private bool isIntroPlaying = false;

    public bool IsIntroPlaying => isIntroPlaying;


    private void Awake()
    {
        ContainerActive(false);
    }

    public void SettingAndExcute(AIController controller)
    {
        StartCoroutine(StartAppearBossIntro_Co(controller));
    }

    public void SettingBossInfo(AIController controller)
    {
        AIStatus aiStatus = controller.aiStatus;
        characteristicsName_Text.text = aiStatus.AICharacteristicNameUI;
        originalName_Text.text = aiStatus.AINameUI;
    }

    public void ExcuteIntro()
    {
        anim.SetTrigger(trigger);
    }

    public IEnumerator StartAppearBossIntro_Co(AIController controller)
    {
        isIntroPlaying = true;
        ContainerActive(true);
        SettingBossInfo(controller);
        ExcuteIntro();
        Debug.Log("StartAppearBossIntro_Co 시작");

        yield return new WaitForSeconds(introTime);
        Debug.Log("StartAppearBossIntro_Co 종료");

        isIntroPlaying = false;
        ContainerActive(false);
    }


    private void ContainerActive(bool active)
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].gameObject.SetActive(active);
    }
}
