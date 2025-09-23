using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemReward : MonoBehaviour
{
    [SerializeField] private ReturnObjectToObjectPooling returnOBP = null;
    [SerializeField] private Image rewardImg = null;
    [SerializeField] private TMP_Text count_Text = null;
    [SerializeField] private Item item = null;
    [SerializeField] private int itemCount = -1;

    public Item Item => item;
    public int ItemCount => itemCount;

    public void Setting(Sprite sprite, Item item, int itemCount)
    {
        rewardImg.sprite = sprite;
        this.item = item;
        this.itemCount = itemCount;
        count_Text.text = itemCount == 1 ? "" : itemCount.ToString();

    }

    public void SetOBP() => returnOBP.SetOBP();
}
