using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitleSlotUI : MonoBehaviour
{
    [SerializeField] private TitleSlotData data;

    [Header("[0] Have Data  [1] Empty Data")]
    [SerializeField] private Transform[] datas;

    [Header("[0] LvText  [1] PlayTimeText")]
    [SerializeField] private TMP_Text[] infos;

    [SerializeField] private Button slot_Btn;


    public int SlotIndex => data.SaveSlotIndex + 1;
    public TitleSlotData Data => data;


    public void SlotUpdate(int index)
    {
        for (int i = 0; i < datas.Length; i++)
            datas[i].gameObject.SetActive(false);

        if (data.CanLoadInfo())
        {
            datas[0].gameObject.SetActive(true);
            string[] dataInfo = data.LoadPlayerInfoForTitleSlot();

            infos[0].text = "플레이어 Level." + dataInfo[0];
            infos[1].text = "플레이 타임 :" + dataInfo[1];

            if (index == 1)
                slot_Btn.interactable = true;
            Debug.Log("로드완료 : Slot" + data.SaveSlotIndex);
        }
        else
        {
            datas[1].gameObject.SetActive(true);
            if (index == 1)
                slot_Btn.interactable = false;
            Debug.Log("로드불가 : Slot" + data.SaveSlotIndex);
        }
    }

}
