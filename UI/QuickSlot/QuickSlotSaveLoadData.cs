using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class QuickSlotSaveLoadData : MonoBehaviour
{
    //ÀÌ°Å °ÔÀÓ ²¯´ÙÅ°¸é ½ºÅ©¸³ÅÍºí ÃÊ±âÈ­µÇ´Â°Å°°À½. -> excell·Î ¼öÁ¤.
    [SerializeField] private InventoryObject quickInventory = null;
    [SerializeField] private InventoryContainterUI inventoryContainter = null;
    [SerializeField] private SkillDatabase skillDatabase = null;

    private string dataSavePath;
    private string dataSaveFileName = "QuickSlotSaveFile";
    private string dataLoadPath;

    private void Start()
    {
      //  LoadData(quickInventory);
    }

    private void OnApplicationQuit()
    {
      // SaveData(quickInventory);
    }

    public void Save() => SaveData(quickInventory);
    public void Load() => LoadData(quickInventory);
    public void ResetLoad() => Reset(quickInventory);


    public void Reset(InventoryObject inven)
    {
        Debug.Log("¸®¼Â Äü½½·Ô!");
        inven.Clear();

    }

    public void SaveData(InventoryObject inventory)
    {
        dataSavePath = Application.persistentDataPath + dataSaveFileName + SaveManager.Instance.SaveSlotIndex + ".csv";
        using (StreamWriter sw = new StreamWriter(dataSavePath, false, System.Text.Encoding.UTF8))
        {
            Debug.Log("Save QuickSlot" + dataSavePath);

            string line = "QuickSlot ID,Item Type, Item ID, Item Amount";
            sw.WriteLine(line);

            for (int i = 0; i < inventory.slots.Length; i++)
            {
                line = i + "," +
                        (int)inventory.slots[i].item.itemType + "," +
                        inventory.slots[i].item.id + "," +
                        inventory.slots[i].amount;
                sw.WriteLine(line);
           }
        }
    }

    public void LoadData(InventoryObject inventory)
    {
        dataLoadPath = Application.persistentDataPath + dataSaveFileName + SaveManager.Instance.SaveSlotIndex + ".csv";
        Debug.Log("LoadData QuickSlot " + dataLoadPath);

        if (File.Exists(dataLoadPath))
        {
            Debug.Log("Äü½½·Ô csv ÀÎ!");

            using (StreamReader sr =  new StreamReader(dataLoadPath))
            {
                inventory.Clear();
                string loadData = sr.ReadLine();
                while(!sr.EndOfStream)
                {
                    loadData = sr.ReadLine();
                    string[] tap = loadData.Split(',');
                    int quickID = int.Parse(tap[0]);
                    SlotAllowType type = (SlotAllowType)int.Parse(tap[1]);
                    int itemID = int.Parse(tap[2]);
                    int amount = int.Parse(tap[3]);

                    if (itemID != -1 && type == SlotAllowType.SKILL)
                    {
                        inventory.slots[quickID].UpdateSlot(new Item(Instantiate(skillDatabase.GetSkillClone(itemID))),1);
                        Debug.Log("+++++++Äü½½·Ô Load : ½ºÅ³");

                    }
                    else if (itemID != -1)
                    {
                        Item item = ItemManager.Instance.GenerateItem(itemID);
                        if (inventoryContainter.GetHaveItemCount(item) <= 0)
                        {
                            Debug.Log("+++++++Äü½½·Ô Load : ¾ÆÀÌÅÛ 0");
                            inventory.slots[quickID].UpdateSlot(new Item(), 0);
                        }
                        else
                        {
                            Debug.Log("+++++++Äü½½·Ô Load : ¾ÆÀÌÅÛ " + inventoryContainter.GetHaveItemCount(item));
                            inventory.slots[quickID].UpdateSlot(item, inventoryContainter.GetHaveItemCount(item));
                        }
                    }
                    else if(itemID == -1)
                    {
                        Debug.Log("+++++++Äü½½·Ô Load : ºóÄ­ " );
                        inventory.slots[quickID].UpdateSlot(new Item(), 0);
                    }
                    else
                        inventory.slots[quickID].UpdateSlot(new Item(), 0);

                }
            }
          
        }
        else
        {
            inventory.Clear();
        }
    }

}
