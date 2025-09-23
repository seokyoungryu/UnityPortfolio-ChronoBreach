using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpwanObject : InteractObject
{
    //여기에 drop 목록을 받아서 Item으로 변환하기.
    [SerializeField] private List<Item> dropItems = new List<Item>();
    [SerializeField] private int dropMoney = 0;
    [SerializeField] private string OBPName = "ItemSpawnObject";
    [SerializeField] private EffectList afterInteractEffect = EffectList.None;
    [SerializeField] private EffectList interactAfterEffect;
    [SerializeField] private float interactAfterEffDelay = 1f;

    public void SetItemInfo(DropItem[] dropItemList)
    {
        for (int i = 0; i < dropItemList.Length; i++)
            AddProbabilityItem(dropItemList[i]);

        if (dropItems.Count <= 0 && dropMoney == 0)
            ResetData();
    }

    public void RemoveDropItem(int index) => dropItems.RemoveAt(index);

    public void ResetData()
    {
        dropItems.Clear();
        dropMoney = 0;
        ObjectPooling.Instance.SetOBP(OBPName, this.gameObject);
    }

    private void AddProbabilityItem(DropItem dropItem)
    {
        float percentage = MathHelper.RandomPercentage0To100();

        if (percentage <= dropItem.dropPercent)
        {
            if (dropItem.isMoney)
                dropMoney += MathHelper.GetRandom(dropItem.minMoney, dropItem.maxMoney);
            else
                dropItems.Add(ItemManager.Instance.GenerateItem((int)dropItem.itemList));
        }
    }


    [ContextMenu("Test Interact")]
    public void TestInteract()
    {
        ReturnObjectToObjectPooling pool = GetComponent<ReturnObjectToObjectPooling>();
        pool.objectPoolName = OBPName;
        pool.DelaySetOBP(interactAfterEffDelay);
        EffectManager.Instance.GetEffectObject(interactAfterEffect, transform.position, Vector3.zero, Vector3.zero);
    }


    public override void ExcuteInteract()
    {
        base.ExcuteInteract();
        Debug.Log("Excute Item : " + gameObject.name);
        List<ItemContainer> itemContainers = new List<ItemContainer>();
        for (int i = 0; i < dropItems.Count; i++)
            itemContainers.Add(new ItemContainer(dropItems[i], 1));

        if (!CommonUIManager.Instance.playerInventory.CheckCanAddItems(itemContainers.ToArray())) return;

        GameManager.Instance.SetPlusOwnMoney(dropMoney);
        CommonUIManager.Instance.playerInventory.AddItem(itemContainers.ToArray());
        CommonUIManager.Instance.InteractUIRemove(this);
        ReturnObjectToObjectPooling pool = GetComponent<ReturnObjectToObjectPooling>();
        pool.objectPoolName = OBPName;
        pool.DelaySetOBP(interactAfterEffDelay);
        EffectManager.Instance.GetEffectObject(interactAfterEffect, transform.position, Vector3.zero, Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("<color=yellow>" + gameObject.name + " : Enter </color>");
            CommonUIManager.Instance.InteractUIRegister(this);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("<color=yellow>" + gameObject.name + " : Exit</color>");
            CommonUIManager.Instance.InteractUIRemove(this);
        }
    }


}
