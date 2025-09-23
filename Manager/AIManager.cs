using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
    private AIInfoData data = null;
    [SerializeField] private GlobalAppearBossHPUI globalAppearBossHPUI = null;
    [SerializeField] private AppearBossIntro appearBossIntro = null;
    private Queue<AIController> appearBossList = new Queue<AIController>();

    public Queue<AIController> AppearBossList => appearBossList;


    protected override void Awake()
    {
        base.Awake();
        data = ScriptableObject.CreateInstance<AIInfoData>();
        data.LoadData();
    }


    public AIInfoClip GetAIInfoClip(AIInfoList list)
    {
        return data.allEnemyClips[(int)list];
    }


    public AIController CreateAI(string obpName, AIInfoList aiList = AIInfoList.None)
    {
        GameObject go = ObjectPooling.Instance.GetOBP(obpName);
        AIController activeEnemyController = go.GetComponentInChildren<AIController>();
        if (activeEnemyController == null)
        {
            Debug.Log($"<color=red> {obpName} (AIManager) {go} CreateAI1 NULL : {activeEnemyController} </color>");
        }
        activeEnemyController.SetOBPName(obpName);
        activeEnemyController.obpGo = go;
        if (aiList != AIInfoList.None) activeEnemyController.aiInfoList = aiList;
        activeEnemyController.nav.enabled = false;
        activeEnemyController.ResetAI();
        activeEnemyController.nav.enabled = true;
        activeEnemyController.ClearOnDead();
        return activeEnemyController;
    }


    public void ExcuteBossIntro()
    {
        if (globalAppearBossHPUI == null)
            globalAppearBossHPUI = CommonUIManager.Instance.globalAppearBossHpUI;
        if (appearBossIntro == null)
            appearBossIntro = CommonUIManager.Instance.appearBossIntro;

        if (appearBossList == null || appearBossList.Count <= 0 || appearBossIntro.IsIntroPlaying)
            return;
        AIController controller = appearBossList.Dequeue();
        StartCoroutine(ExcuteBossIntroProcess_Co(controller));
    }

    private IEnumerator ExcuteBossIntroProcess_Co(AIController controller)
    {
        yield return StartCoroutine(appearBossIntro.StartAppearBossIntro_Co(controller));
        if (!appearBossIntro.IsIntroPlaying && appearBossList.Count > 0)
            StartCoroutine(ExcuteBossIntroProcess_Co(appearBossList.Dequeue()));
    }

    public void SettingAppearBossEvent(AIController controller)
    {
        if (globalAppearBossHPUI == null)
            globalAppearBossHPUI = CommonUIManager.Instance.globalAppearBossHpUI;
        globalAppearBossHPUI.SettingInfos(controller);
    }

    public void UpdateReduceAppearBossEvent(AIController controller)
    {
        if (globalAppearBossHPUI == null)
            globalAppearBossHPUI = CommonUIManager.Instance.globalAppearBossHpUI;
        globalAppearBossHPUI.ReduceHp(controller);
    }
    public void UpdateIncreaseAppearBossEvent(AIController controller)
    {
        if (globalAppearBossHPUI == null)
            globalAppearBossHPUI = CommonUIManager.Instance.globalAppearBossHpUI;
        globalAppearBossHPUI.IncreaseHp(controller);
    }

}
