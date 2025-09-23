using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPracticeMode : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CommonUIManager.Instance.entryPracticeModeUI.OpenUIWindow();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CommonUIManager.Instance.entryPracticeModeUI.CloseUIWindow();
        }
    }
}
