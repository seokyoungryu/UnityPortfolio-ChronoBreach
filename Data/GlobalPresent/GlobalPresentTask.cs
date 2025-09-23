using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Global/GlobalPresent/Global Present Task", fileName = "GlobalPresentTask_Type")]
public class GlobalPresentTask : ScriptableObject
{
    [SerializeField] private string postTitle = string.Empty;
    [SerializeField, TextArea(0, 3)] private string postDescription = string.Empty;
    [SerializeField] private string btnName = string.Empty;
    [SerializeField] private int money = 0;
    [SerializeField] private List<GlobalPresentInfo> items = new List<GlobalPresentInfo>();


    public void Excute()
    {
        GlobalPost post = CommonUIManager.Instance.globalPost;
        post.ResetData();
        for (int i = 0; i < items.Count; i++)
            post.AddItem(ItemManager.Instance.GenerateItem((int)items[i].Item), items[i].Count);
        if (money > 0)
            post.AddMoney(money);

        post.SettingDescription(postDescription);
        post.SettingTitle(postTitle);
        post.SettingAcceptBtn(btnName);
        post.ExcutePost();
    }
}

[System.Serializable]
public class GlobalPresentInfo
{
    [SerializeField] private ItemList item;
    [SerializeField] private int count = 1;

    public ItemList Item => item;
    public int Count => count;
}
