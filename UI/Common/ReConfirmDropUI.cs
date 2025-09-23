using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReConfirmDropUI : UIRoot
{
    [SerializeField] private TMP_Text description_Text;

    private Item item = null;
    private int amount = 0;

    public void Setting(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
        description_Text.text = this.item.itemClip.uiItemName + " " + this.amount.ToString() + " 를 버리시겠습니까?";
        OpenUIWindow();
    }

    public override void OpenUIWindow()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        base.OpenUIWindow();
    }

    public override void CloseUIWindow()
    {
        base.CloseUIWindow();
    }

    public void Accept_Btn()
    {
        // if (item.itemClip.isOverlap)
        //     CommonUIManager.Instance.playerInventory.RemoveItem(item, amount);
        // else
        CommonUIManager.Instance.playerInventory.RemoveItemInstanceID(item, amount, item.itemClip.instanceID);
        CloseUIWindow();
    }

    public void Refuse_Btn()
    {
        CloseUIWindow();
    }
}
