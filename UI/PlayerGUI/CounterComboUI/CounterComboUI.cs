using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterComboUI : MonoBehaviour
{
    [SerializeField] private GameObject container = null;
    [SerializeField] private TMP_Text count_Text = null;
    [SerializeField] private string textChangeAnimationName = string.Empty;
    private Animator animator = null;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        container.SetActive(false);
    }


    public void UpdateText(int count)
    {
        if (count == 0)
            container.SetActive(false);
        else 
            container.SetActive(true);

        animator.Play(textChangeAnimationName);
        count_Text.text = count.ToString();
    }
}
