using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonNotifierUI : MonoBehaviour
{
    [SerializeField] private Transform containerPanel = null;
    [SerializeField] private TMP_Text notifier_Text = null;


    private void Start()
    {
        containerPanel.gameObject.SetActive(false);
    }


    public void SetText(string text)
    {
        if (!containerPanel.gameObject.activeSelf)
            containerPanel.gameObject.SetActive(true);

        notifier_Text.text = text;
    }

    public void SetDisable() => containerPanel.gameObject.SetActive(false);

}
