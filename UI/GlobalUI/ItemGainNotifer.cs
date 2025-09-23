using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemGainNotifer : MonoBehaviour
{
    [SerializeField] private Image item_Img = null;
    [SerializeField] private TMP_Text itemName_Text = null;
    [SerializeField] private TMP_Text itemCount_Text = null;
    [SerializeField] private Sprite baseMoney_Img = null;

    public void SettingNotifier(Item item, int amount)
    {
        item_Img.sprite = item?.itemClip?.itemTexture;
        itemCount_Text.text = amount.ToString();
        itemName_Text.text = item.itemClip.uiItemName;

    }

    public void SettingNotifier(int money)
    {
        item_Img.sprite = baseMoney_Img;
        itemCount_Text.text = "";
        itemName_Text.text = string.Format("{0:#,###}",money);
    }
    public void SettingNotifier(Reward reward)
    {
        item_Img.sprite = reward.Icon;
        itemCount_Text.text = reward.GetIntValue().ToString();
        itemName_Text.text = reward.RewardName;
    }
}
