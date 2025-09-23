using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleNPCWorldCanvas : MonoBehaviour
{
    [SerializeField] private bool active = true;
    [SerializeField] private TMP_Text name_Text;
    [SerializeField] private Color color = Color.white;


    private void Start()
    {
        gameObject.SetActive(active);
        name_Text.color = color;
    }


    public void SettingName(string name) => name_Text.text = name;
}
