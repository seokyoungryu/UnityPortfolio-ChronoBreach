using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StoreItem : MonoBehaviour
{
    [SerializeField] private Item item = null;
    [SerializeField] private Image itemImg = null;
    [SerializeField] private TextMeshProUGUI amount_text = null;
    [SerializeField] private TextMeshProUGUI itemName_text = null;
    [SerializeField] private TextMeshProUGUI itemCost_text = null;
    [SerializeField] private int amount = 0;

    public int Amount { get { return amount; } set { amount = value; } }
    public Item GetItem => item;
    public Image ItemImg => itemImg;

    public StoreItem() { }

    public StoreItem(Item item)
    {
        this.item = item;
        amount = 1;
    }

    public StoreItem(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public StoreItem(StoreItem storeItem)
    {
        this.item = storeItem.GetItem;
        amount = storeItem.Amount;
    }

    public void Setting(Item item,int amount ,ItemCountConfirmCategory storeCategory)
    {
        this.item = item;
        this.amount = amount;

        if (storeCategory == ItemCountConfirmCategory.BUY)
            BuyItemInit(item);
        else if (storeCategory == ItemCountConfirmCategory.REPURCHASE)
            RepurchaseItemInit(item);
    }


    public void BuyItemInit(Item item)
    {
        itemImg.sprite = item.itemClip.itemTexture;
        itemName_text.text = item.itemClip.uiItemName;
        itemCost_text.text = "판매가 : " + item.itemClip.buyCost.ToString() + "G";
        amount_text.text = "";
    }

    public void RepurchaseItemInit(Item item)
    {
        itemImg.sprite = item.itemClip.itemTexture;
        itemName_text.text = item.itemClip.uiItemName;
        itemCost_text.text = "구매가 : " + item.itemClip.repurchaseCost.ToString() + "G";
        if (item.itemClip.isOverlap)
            amount_text.text = amount.ToString();
        else
            amount_text.text = "";
    }
}
