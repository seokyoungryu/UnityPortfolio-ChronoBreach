using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlobalPost : MonoBehaviour
{
    [SerializeField] private RectTransform container = null;
    [SerializeField] private TMP_Text title_Text = null;
    [SerializeField] private TMP_Text description_Text = null;
    [SerializeField] private TMP_Text acceptBtn_Text = null;
    [SerializeField] private List<ItemContainer> items = new List<ItemContainer>();
    [SerializeField] private int money = 0;
    [SerializeField] private Vector3 openPosition = Vector3.zero;
    [SerializeField] private SoundList postSound;
    [SerializeField] private SoundList acceptBtnClickSound;


    private void Start()
    {
        container.gameObject.SetActive(false);
    }

    public void ResetData()
    {
        description_Text.text = "";
        items.Clear();
        money = 0;
    }

    public void ExcutePost()
    {
        GameManager.Instance.canUseCamera = false;
        CursorManager.Instance.CursorVisible();
        GameManager.Instance.Player.Conditions.CanAttack = false;
        SoundManager.Instance.PlayExtraSound(postSound);
        container.gameObject.SetActive(true);
        container.anchoredPosition = openPosition;
    }

    public void SettingDescription(string description)
    {
        description_Text.text = description;
    }
    public void SettingTitle(string titleName)
    {
        title_Text.text = titleName;
    }
    public void SettingAcceptBtn(string acceptBtnName)
    {
        acceptBtn_Text.text = acceptBtnName;
    }

    public void AddItem(Item item, int amount = 1)
    {
        ItemContainer itemContainer = new ItemContainer(item, amount);
        items.Add(itemContainer);
    }

    public void AddMoney(int moneyAmount)
    {
        money += moneyAmount;
    }

    public void Accept()
    {
        if(!CommonUIManager.Instance.playerInventory.CheckCanAddItems(items.ToArray()))
        {
            CommonUIManager.Instance.ExcuteGlobalNotifer("인벤토리 공간이 부족합니다.");
            return;
        }

        for (int i = 0; i < items.Count; i++)
            CommonUIManager.Instance.playerInventory.AddItem(items[i]);
        if (money != 0)
            GameManager.Instance.SetPlusOwnMoney(money);

        SoundManager.Instance.PlayExtraSound(acceptBtnClickSound);
        ResetData();
        container.gameObject.SetActive(false);
    }
    
}
