using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] private AllDungeonTitlesDatabase allDungeonDB = null;
    [SerializeField] private TitleSlotData currentTitleSlotData = null;

    [SerializeField] private TitleSlotData[] titleDatas;
    [SerializeField] private InventoryObject[] inventoryObjects;

    private bool isCanLoad = false;
    private bool isEditSave = false;

    public int SaveSlotIndex => currentTitleSlotData.SaveSlotIndex;
    public bool IsEditSave => isEditSave;



    private void OnApplicationQuit()
    {
        AllSave();
    }

    public void SetCurrTitleData(TitleSlotData data)
    {
        currentTitleSlotData = data;
    }

    public void ChangeSaveSlotIndex(int index)
    {
        currentTitleSlotData.SaveSlotIndex = index;
    }

    public void AllLoad(bool isNewData)
    {
        if (isNewData)
        {
            QuestManager.Instance.DeleteSaveData(SaveSlotIndex);
            QuestLoad();
            PlayerSkillLoad(true);
            InventoryObjsLoad(true);
            CommonUIManager.Instance.quickSlotSaveDt?.ResetLoad();
            CommonUIManager.Instance.playerInventory.ResetInventorys();
            allDungeonDB.ResetClear();
            isCanLoad = false;
            Debug.Log("All 새로운 Data");
        }
        else
        {
            QuestLoad();
            PlayerSkillLoad(false);
            InventoryObjsLoad(false);
            CommonUIManager.Instance.playerInventory.LoadInventorys();
            CommonUIManager.Instance.quickSlotSaveDt?.Load();
            GameManager.Instance.Player.GetComponent<PlayerEquipment>().LoadExcuteEquipmentItem();
            isCanLoad = true;
            Debug.Log("All 기존 Data");
        }
    }

    public void AllSave()
    {
        QuestSave();
        PlayerSkillSave();
        InventoryObjsSave();
        SavePlayerData();
        CommonUIManager.Instance.quickSlotSaveDt?.Save();
        CommonUIManager.Instance.playerInventory.SaveInventorys();
        CommonUIManager.Instance.equipmentUI.SaveEquipmentUI();
    }

    public void EditAllSave()
    {
        isEditSave = true;
        AllSave();
        Debug.Log("EditAllSave");
    }

    public void EditLoad()
    {
        isCanLoad = true;
        QuestLoad();
        PlayerSkillLoad(false);
        InventoryObjsLoad(false);
        CommonUIManager.Instance.playerInventory.LoadInventorys();
        CommonUIManager.Instance.equipmentUI.LoadEquipmentUI();
        CommonUIManager.Instance.quickSlotSaveDt?.Load();
        ExcutePracticeLoadPlayerInfo();
        GameManager.Instance.Player.GetComponent<PlayerEquipment>().LoadExcuteEquipmentItem();
        Debug.Log("EditLoad");
    }

    public void PracticeLoad()
    {
        Debug.Log("PracticeLoad 시작!");
        isCanLoad = true;
        CommonUIManager.Instance.equipmentUI.LoadEquipmentUI();
        PlayerSkillLoad(false);
        InventoryObjsLoad(false);
        ExcutePracticeLoadPlayerInfo();
        GameManager.Instance.Player.GetComponent<PlayerEquipment>().LoadExcuteEquipmentItem();
    }


    //Quest
    public void QuestSave() => QuestManager.Instance.Save();
    public void QuestLoad() => QuestManager.Instance.Load();



    //Player Skill
    public void PlayerSkillSave() => GameManager.Instance.Player?.skillController.SaveSkillDataToExcel();
    public void PlayerSkillLoad(bool isNewData) => GameManager.Instance.Player?.skillController.LoadPlayerSkill(isNewData);

    public void InventoryObjsLoad(bool isNewData)
    {
        for (int i = 0; i < inventoryObjects.Length; i++)
        {
            Debug.Log("인벤토리 로드 : " + inventoryObjects[i].name);
            inventoryObjects[i].Clear();

            if (!isNewData)
            {
                inventoryObjects[i].SaveLoadData.LoadInventoryData(inventoryObjects[i]);
            }

            for (int j = 0; j < inventoryObjects[i].slots.Length; j++)
                inventoryObjects[i].slots[j].UpdateSlot(inventoryObjects[i].slots[j].item, inventoryObjects[i].slots[j].amount);
        }
    }



    public void InventoryObjsSave()
    {
        for (int i = 0; i < inventoryObjects.Length; i++)
            inventoryObjects[i].SaveLoadData.SaveInventoryData(inventoryObjects[i]);
    }

    #region Player Info

    public void SavePlayerData(PlayerStateController controller)
    {
       currentTitleSlotData.SavePlayerInfo(controller);
    }

    [ContextMenu("세이브!")]
    public void SavePlayerData()
    {
        currentTitleSlotData.SavePlayerInfo(GameManager.Instance.Player);
    }
    [ContextMenu("로드!")]
    public void ExcuteInitLoadPlayerInfo()
    {
        PlayerStateController controller = GameManager.Instance.Player;
        currentTitleSlotData.LoadPlayerInfo(controller, isCanLoad, true);
        controller.playerStats.UpdateStats();
        isCanLoad = false;
    }
    public void ExcuteAbsoluteLoadPlayerInfo()
    {
        PlayerStateController controller = GameManager.Instance.Player;
        currentTitleSlotData.LoadPlayerInfo(controller, true, true);
        controller.playerStats.UpdateStats();
        isCanLoad = false;
    }

    public void ExcutePracticeLoadPlayerInfo()
    {
        PlayerStateController controller = GameManager.Instance.Player;
        currentTitleSlotData.LoadPlayerInfo(controller, isCanLoad, false);
        controller.playerStats.UpdateStats();
        isCanLoad = false;

        controller.comboController.SettingClip(controller.skillController);
    }
    #endregion
}
