using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

[System.Serializable]
public class InventorySaveLoad
{
    public enum SaveSlotType { ITEM = 0, SKILL =1,}

    public string fullPath = string.Empty;
    public string fileName = string.Empty;
    public string fileExtension = ".csv";


    #region Save Load - Inventory

    public void SaveInventoryData(InventoryObject inventory)
    {
        fullPath = Application.persistentDataPath + fileName + SaveManager.Instance.SaveSlotIndex + fileExtension;
        Debug.Log("Start-"+inventory.name + " - " + fullPath);

        using (StreamWriter sw = new StreamWriter(fullPath, false, System.Text.Encoding.UTF8))
        {
            string line = "SlotIndex,Type,ItemID,Instance ID,Name,Amount,STR,DEX,LUC,INT,AllStats,DEF,Magic DEF,Atk,AtkSpeed,HPRegen,STRegen,CriChance,CriDmg,Evasion,EnchantLevel,EnchantLeftCount" +
                ", Potential Rank, Potential Count, [ Potential ID, Potential Value],[ Potential ID, Potential Value ], [ Potential ID, Potential Value ]";
            sw.WriteLine(line);

            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.slots[i].item.itemClip)
                {
                    line = i + "," + (int)SaveSlotType.ITEM + "," + 
                           inventory.slots[i].item.id + "," +
                           inventory.slots[i].item.itemClip.instanceID + "," +
                           inventory.slots[i].item.objectName + "," +
                           inventory.slots[i].amount + "," +
                           inventory.slots[i].item.itemClip.strength + "," +
                           inventory.slots[i].item.itemClip.dexterity + "," +
                           inventory.slots[i].item.itemClip.luck + "," +
                           inventory.slots[i].item.itemClip.intelligence + ",";

                    if (inventory.slots[i].item.itemClip is WeaponItemClip)
                    {
                        WeaponItemClip clip = inventory.slots[i].item.itemClip as WeaponItemClip;

                        line += "-" + "," +                        //¿Ã½ºÅÈ
                                "-" + "," +                        //def
                                "-" + "," +                        //magic def
                                clip.atkValue + "," +            //atk
                                clip.atkSpeed + "," +            //atkspeed
                                "-" + "," +                        //hpregen
                                "-" + "," +                        //stregen
                                clip.criticalChance + "," +      //crichance
                                clip.criticalDamage + "," +      //criDmg
                                "-" + "," +                        //Evasion
                                clip.enchantLevel + "," +        //enchantLevel
                                clip.enchantLeftCount + "," +    //enchantLeftCount
                                (int)clip.potentialRank + "," +
                                clip.potentialOptionCount;

                         for (int n = 0; n < clip.potentialOptionCount; n++)
                         {
                            if (clip.potentialRank == ItemPotentialRankType.NONE) continue;
                             line += "," + clip.ownPotential[n].id + "," +
                                   clip.ownPotential[n].potentialValue;
                         }
                    }
                    else if (inventory.slots[i].item.itemClip is ArmorItemClip)
                    {
                        ArmorItemClip clip = inventory.slots[i].item.itemClip as ArmorItemClip;
                        line += "-" + "," +                        //¿Ã½ºÅÈ
                              clip.defense + "," +               //def
                              clip.magicDefense + "," +          //magic def
                              "-" + "," +                          //atk
                              "-" + "," +                          //atkspeed
                              clip.healthRegeneration + "," +    //hpregen
                              clip.staminaRegeneration + "," +   //stregen
                              "-" + "," +                          //crichance
                              "-" + "," +                          //criDmg
                              clip.evasion + "," +               //Evasion
                              clip.enchantLevel + "," +         //enchantLevel
                              clip.enchantLeftCount + "," +     //enchantLeftCount
                              (int)clip.potentialRank + "," +
                              clip.potentialOptionCount;

                         for (int n = 0; n < clip.potentialOptionCount; n++)
                         {
                            if (clip.potentialRank == ItemPotentialRankType.NONE) continue;
                            line += "," + clip.ownPotential[n].id + "," +
                                   clip.ownPotential[n].potentialValue;
                         }
                    }
                    else if (inventory.slots[i].item.itemClip is AccessoryItemClip)
                    {
                        AccessoryItemClip clip = inventory.slots[i].item.itemClip as AccessoryItemClip;
                        line += clip.allStats + "," +              //¿Ã½ºÅÈ
                                clip.defense + "," +               //def
                                clip.magicDefense + "," +          //magic def
                                clip.atk + "," +                   //atk
                                clip.atkSpeed + "," +              //atkspeed
                                clip.healthRegeneration + "," +    //hpregen
                                clip.staminaRegenaration + "," +   //stregen
                                clip.critialChance + "," +         //crichance
                                clip.criticalDamage + "," +        //criDmg
                                clip.evasion + "," +               //Evasion
                                clip.enchantLevel + "," +          //enchantLevel
                                clip.enchantLeftCount + "," +      //enchantLeftCount
                                (int)clip.potentialRank + "," +
                                clip.potentialOptionCount;

                         for (int n = 0; n < clip.potentialOptionCount; n++)
                         {
                            if (clip.potentialRank == ItemPotentialRankType.NONE) continue;
                            line += "," + clip.ownPotential[n].id + "," +
                                   clip.ownPotential[n].potentialValue;
                         }

                    }
                    else if (inventory.slots[i].item.itemClip is TitleItemClip)
                    {
                        TitleItemClip clip = inventory.slots[i].item.itemClip as TitleItemClip;
                        line += clip.allStats + "," +              //¿Ã½ºÅÈ
                                clip.defense + "," +               //def
                                clip.magicDefense + "," +          //magic def
                                clip.atk + "," +                   //atk
                                clip.atkSpeed + "," +              //atkspeed
                                clip.healthRegeneration + "," +    //hpregen
                                clip.staminaRegenaration + "," +   //stregen
                                clip.critialChance + "," +         //crichance
                                clip.criticalDamage + "," +        //criDmg
                                "-" + "," +               //Evasion
                                "-" + "," +          //enchantLevel
                                "-" + "," +      //enchantLeftCount
                                (int)clip.potentialRank + "," +
                                clip.potentialOptionCount ;

                         for (int n = 0; n < clip.potentialOptionCount; n++)
                         {
                            if (clip.potentialRank == ItemPotentialRankType.NONE) continue;
                            line += "," + clip.ownPotential[n].id + "," +
                                   clip.ownPotential[n].potentialValue;
                         }
                    }
                    sw.WriteLine(line);
                }
                else if(inventory.slots[i].item.skillClip)
                {
                    BaseSkillClip clip = inventory.slots[i].item.skillClip;
                    line = i + "," + (int)SaveSlotType.SKILL + "," +
                        + clip.ID + ","
                        + clip.displayName + ","
                        + clip.currentSkillIndex + ","
                        + (int)clip.skillState;
                    sw.WriteLine(line);
                }
               

            }
        }

#if UNITY_EDITOR
        for (int i = 0; i < inventory.slots.Length; i++)
            EditorUtility.SetDirty(inventory);
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
#endif

    }


    public void LoadInventoryData(InventoryObject inventory)
    {
       // Debug.Log($"{fileName} LOAD _ " + fullPath);
        fullPath = Application.persistentDataPath + fileName + SaveManager.Instance.SaveSlotIndex + fileExtension;
        if (System.IO.File.Exists(fullPath))
        {
            inventory.Clear();
            using (StreamReader sr = new StreamReader(fullPath))
            {
                string loadData = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    loadData = sr.ReadLine();
                    string[] splitTab = loadData.Split(',');
                    int slotIndex = int.Parse(splitTab[0]);
                    int slotType = int.Parse(splitTab[1]);

                    if (slotType == (int)SaveSlotType.ITEM)
                    {
                        int itemindex = int.Parse(splitTab[2]);
                        int instanceID = int.Parse(splitTab[3]);
                        string name = splitTab[4];
                        int amount = int.Parse(splitTab[5]);
                        int str = int.Parse(splitTab[6]);
                        int dex = int.Parse(splitTab[7]);
                        int luc = int.Parse(splitTab[8]);
                        int Int = int.Parse(splitTab[9]);

                        BaseItemClip itemClip = ItemManager.Instance.GetItemClip(itemindex);

                        if (itemClip is WeaponItemClip)
                        {
                            WeaponItemClip tmpClip = Object.Instantiate(itemClip as WeaponItemClip);
                            tmpClip.instanceID = instanceID;
                            tmpClip.strength = str;
                            tmpClip.dexterity = dex;
                            tmpClip.luck = luc;
                            tmpClip.intelligence = Int;

                            tmpClip.atkValue = float.Parse(splitTab[13]);
                            tmpClip.atkSpeed = float.Parse(splitTab[14]);
                            tmpClip.criticalChance = float.Parse(splitTab[17]);
                            tmpClip.criticalDamage = float.Parse(splitTab[18]);
                            tmpClip.enchantLevel = int.Parse(splitTab[20]);
                            tmpClip.enchantLeftCount = int.Parse(splitTab[21]);
                            tmpClip.potentialRank = (ItemPotentialRankType)int.Parse(splitTab[22]);
                            tmpClip.potentialOptionCount = int.Parse(splitTab[23]);

                            int[] IdIndex = { 24, 26, 28 };
                            int[] ValueIndex = { 25, 27, 29 };
                            tmpClip.ownPotential = new OwnPotential[tmpClip.potentialOptionCount];

                            if (tmpClip.potentialRank != ItemPotentialRankType.NONE && tmpClip.potentialOptionCount != -1 && tmpClip.potentialOptionCount != 0)
                                for (int n = 0; n < tmpClip.potentialOptionCount; n++)
                                {
                                    PotentialOptionClip clip = PotentialManager.Instance.GetPotentialClip(int.Parse(splitTab[IdIndex[n]]));
                                    PotentialOptionClip tmpPotential = new PotentialOptionClip(clip);
                                    tmpPotential.potentialValue = float.Parse(splitTab[ValueIndex[n]]);
                                    tmpClip.ownPotential[n] = new OwnPotential(tmpPotential);
                                }


                            inventory.slots[slotIndex].item = new Item(tmpClip);
                            inventory.slots[slotIndex].amount = amount;
                        }
                        else if (itemClip is ArmorItemClip)
                        {
                            ArmorItemClip tmpClip = Object.Instantiate(itemClip as ArmorItemClip);
                            tmpClip.instanceID = instanceID;
                            tmpClip.strength = str;
                            tmpClip.dexterity = dex;
                            tmpClip.luck = luc;
                            tmpClip.intelligence = Int;

                            tmpClip.defense = int.Parse(splitTab[11]);
                            tmpClip.magicDefense = int.Parse(splitTab[12]);
                            tmpClip.healthRegeneration = float.Parse(splitTab[15]);
                            tmpClip.staminaRegeneration = float.Parse(splitTab[16]);
                            tmpClip.evasion = float.Parse(splitTab[19]);

                            tmpClip.enchantLevel = int.Parse(splitTab[20]);
                            tmpClip.enchantLeftCount = int.Parse(splitTab[21]);
                            tmpClip.potentialRank = (ItemPotentialRankType)int.Parse(splitTab[22]);
                            tmpClip.potentialOptionCount = int.Parse(splitTab[23]);

                            int[] IdIndex = { 24, 26, 28 };
                            int[] ValueIndex = { 25, 27, 29 };
                            tmpClip.ownPotential = new OwnPotential[tmpClip.potentialOptionCount];


                            if (tmpClip.potentialRank != ItemPotentialRankType.NONE && tmpClip.potentialOptionCount != -1 && tmpClip.potentialOptionCount != 0)
                                for (int n = 0; n < tmpClip.potentialOptionCount; n++)
                                {
                                    PotentialOptionClip clip = PotentialManager.Instance.GetPotentialClip(int.Parse(splitTab[IdIndex[n]]));
                                    PotentialOptionClip tmpPotential = new PotentialOptionClip(clip);
                                    tmpPotential.potentialValue = float.Parse(splitTab[ValueIndex[n]]);
                                    tmpClip.ownPotential[n] = new OwnPotential(tmpPotential);
                                }


                            inventory.slots[slotIndex].item = new Item(tmpClip);
                            inventory.slots[slotIndex].amount = amount;
                        }
                        else if (itemClip is AccessoryItemClip)
                        {
                            AccessoryItemClip tmpClip = Object.Instantiate(itemClip as AccessoryItemClip);
                            tmpClip.instanceID = instanceID;
                            tmpClip.strength = str;
                            tmpClip.dexterity = dex;
                            tmpClip.luck = luc;
                            tmpClip.intelligence = Int;

                            tmpClip.allStats = int.Parse(splitTab[10]);
                            tmpClip.defense = int.Parse(splitTab[11]);
                            tmpClip.magicDefense = int.Parse(splitTab[12]);
                            tmpClip.atk = float.Parse(splitTab[13]);
                            tmpClip.atkSpeed = float.Parse(splitTab[14]);
                            tmpClip.healthRegeneration = float.Parse(splitTab[15]);
                            tmpClip.staminaRegenaration = float.Parse(splitTab[16]);
                            tmpClip.critialChance = float.Parse(splitTab[17]);
                            tmpClip.criticalDamage = float.Parse(splitTab[18]);
                            tmpClip.evasion = float.Parse(splitTab[19]);
                            tmpClip.enchantLevel = int.Parse(splitTab[20]);
                            tmpClip.enchantLeftCount = int.Parse(splitTab[21]);
                            tmpClip.potentialRank = (ItemPotentialRankType)int.Parse(splitTab[22]);
                            tmpClip.potentialOptionCount = int.Parse(splitTab[23]);

                            int[] IdIndex = { 24, 26, 28 };
                            int[] ValueIndex = { 25, 27, 29 };
                            tmpClip.ownPotential = new OwnPotential[tmpClip.potentialOptionCount];

                            if (tmpClip.potentialRank != ItemPotentialRankType.NONE && tmpClip.potentialOptionCount != -1 && tmpClip.potentialOptionCount != 0)
                                for (int n = 0; n < tmpClip.potentialOptionCount; n++)
                                {
                                    PotentialOptionClip clip = PotentialManager.Instance.GetPotentialClip(int.Parse(splitTab[IdIndex[n]]));
                                    PotentialOptionClip tmpPotential = new PotentialOptionClip(clip);
                                    tmpPotential.potentialValue = float.Parse(splitTab[ValueIndex[n]]);
                                    tmpClip.ownPotential[n] = new OwnPotential(tmpPotential);
                                }


                            inventory.slots[slotIndex].item = new Item(tmpClip);
                            inventory.slots[slotIndex].amount = amount;
                        }
                        else if (itemClip is TitleItemClip)
                        {
                            TitleItemClip tmpClip = Object.Instantiate(itemClip as TitleItemClip);
                            tmpClip.instanceID = instanceID;
                            tmpClip.strength = str;
                            tmpClip.dexterity = dex;
                            tmpClip.luck = luc;
                            tmpClip.intelligence = Int;

                            tmpClip.allStats = int.Parse(splitTab[10]);
                            tmpClip.defense = int.Parse(splitTab[11]);
                            tmpClip.magicDefense = int.Parse(splitTab[12]);
                            tmpClip.atk = float.Parse(splitTab[13]);
                            tmpClip.atkSpeed = float.Parse(splitTab[14]);
                            tmpClip.healthRegeneration = float.Parse(splitTab[15]);
                            tmpClip.staminaRegenaration = float.Parse(splitTab[16]);
                            tmpClip.critialChance = float.Parse(splitTab[17]);
                            tmpClip.criticalDamage = float.Parse(splitTab[18]);
                            tmpClip.potentialRank = (ItemPotentialRankType)int.Parse(splitTab[22]);
                            tmpClip.potentialOptionCount = int.Parse(splitTab[23]);

                            int[] IdIndex = { 24, 26, 28 };
                            int[] ValueIndex = { 25, 27, 29 };
                            tmpClip.ownPotential = new OwnPotential[tmpClip.potentialOptionCount];

                            if (tmpClip.potentialRank != ItemPotentialRankType.NONE && tmpClip.potentialOptionCount != -1 && tmpClip.potentialOptionCount != 0)
                                for (int n = 0; n < tmpClip.potentialOptionCount; n++)
                                {
                                    PotentialOptionClip clip = PotentialManager.Instance.GetPotentialClip(int.Parse(splitTab[IdIndex[n]]));
                                    PotentialOptionClip tmpPotential = new PotentialOptionClip(clip);
                                    tmpPotential.potentialValue = float.Parse(splitTab[ValueIndex[n]]);
                                    tmpClip.ownPotential[n] = new OwnPotential(tmpPotential);
                                }

                            inventory.slots[slotIndex].item = new Item(tmpClip);
                            inventory.slots[slotIndex].amount = amount;
                        }
                        else if (itemClip is PosionItemClip)
                        {
                            PosionItemClip tmpClip = Object.Instantiate(itemClip as PosionItemClip);
                            tmpClip.instanceID = instanceID;

                            inventory.slots[slotIndex].item = new Item(tmpClip);
                            inventory.slots[slotIndex].amount = amount;
                            Debug.Log(inventory.name + " : Æ÷¼Ç ·Îµå");

                        }
                        else if (itemClip is EnchantItemClip)
                        {
                            EnchantItemClip tmpClip = Object.Instantiate(itemClip as EnchantItemClip);
                            tmpClip.instanceID = instanceID;

                            inventory.slots[slotIndex].item = new Item(tmpClip);
                            inventory.slots[slotIndex].amount = amount;
                        }
                        else if (itemClip is CraftItemClip)
                        {
                            CraftItemClip tmpClip = Object.Instantiate(itemClip as CraftItemClip);
                            tmpClip.instanceID = instanceID;

                            inventory.slots[slotIndex].item = new Item(tmpClip);
                            inventory.slots[slotIndex].amount = amount;
                        }
                        else if (itemClip is ExtraItemClip)
                        {
                            ExtraItemClip tmpClip = Object.Instantiate(itemClip as ExtraItemClip);
                            tmpClip.instanceID = instanceID;

                            inventory.slots[slotIndex].item = new Item(tmpClip);
                            inventory.slots[slotIndex].amount = amount;
                        }
                        else if (itemClip is QuestItemClip)
                        {
                            QuestItemClip tmpClip = Object.Instantiate(itemClip as QuestItemClip);
                            tmpClip.instanceID = instanceID;

                            inventory.slots[slotIndex].item = new Item(tmpClip);
                            inventory.slots[slotIndex].amount = amount;
                        }
                    }
                    else if(slotType == (int)SaveSlotType.SKILL)
                    {
                        Debug.Log(inventory.name + " : ½ºÅ³ ·Îµå");
                    }

                }

            }
        }


    }

    public void DeleteFile()
    {
        if(File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    

#endregion
}
