using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleName_text = null;
    [SerializeField] private Transform container = null;
    [Header("[0] New Start [1] Continue")]
    [SerializeField] private Transform[] tr = null;
    [SerializeField] private string[] titleName;

    [SerializeField] private GameObject[] titleBtns;

    [SerializeField] private GameObject[] slots;
    [SerializeField] private GameObject[] notifiers;
    [SerializeField] private GameObject[] panels;
    [Header("Title BGM")]
    public SoundList titleBGM;


    private void Start()
    {
        SoundSetting();
        SoundManager.Instance.PlayBGM_CrossFade(titleBGM,0.1f);
    }


    private void SoundSetting()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            UIHelper.AddEventTrigger(slots[i], EventTriggerType.PointerEnter, delegate { SoundManager.Instance.PlayUISound(UISoundType.TITLE_POINTER_ENTER); });
        }

        for (int i = 0; i < titleBtns.Length; i++)
        {
            UIHelper.AddEventTrigger(titleBtns[i], EventTriggerType.PointerEnter, delegate { SoundManager.Instance.PlayUISound(UISoundType.TITLE_POINTER_ENTER2); });
        }

        for (int i = 0; i < panels.Length; i++)
        {
            UIHelper.AddEventTrigger(panels[i], EventTriggerType.PointerClick, delegate { SoundManager.Instance.PlayUISound(UISoundType.TITLE_POINTER_ENTER2); });
        }
    }



    public void SelectWindowOpen_Btn(int index)
    {
        SoundManager.Instance.PlayUISound(UISoundType.OPEN_WINDOW);

        for (int i = 0; i < notifiers.Length; i++)
            notifiers[i].SetActive(false);

        container.gameObject.SetActive(true);
        titleName_text.text = titleName[index];
        for (int i = 0; i < tr.Length; i++)
            tr[i].gameObject.SetActive(false);
        tr[index].gameObject.SetActive(true);

        TitleSlotUI[] uis = tr[index].GetComponentsInChildren<TitleSlotUI>();
        for (int i = 0; i < uis.Length; i++)
            uis[i].SlotUpdate(index);
    }

    public void SelectWindowClose_Btn()
    {
        SoundManager.Instance.PlayUISound(UISoundType.CLOSE_WINDOW);

        for (int i = 0; i < tr.Length; i++)
            tr[i].gameObject.SetActive(false);
        container.gameObject.SetActive(false);
    }
}
