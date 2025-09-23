using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private TMP_Text level_Text = null;
    [SerializeField] private Transform container = null;



    public void ContainerActive(bool active)
    {
        container.gameObject.SetActive(active);
    }

    public void SettingText(int beforeLv, int currentLv)
    {
        level_Text.text = $"Lv.{beforeLv}     >>>     <color=red>Lv.{currentLv}</color>";
    }
  
}
