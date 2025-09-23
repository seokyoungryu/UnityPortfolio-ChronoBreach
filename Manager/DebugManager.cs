using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugManager : Singleton<DebugManager>
{
    public PlayerStateController stateController;

    public GameObject DebugPanel = null;
    public TMP_Text stateText = null;

    public bool isDebugMode = false;


    protected override void Awake()
    {
        base.Awake();

        if (DebugPanel == null)
            DebugPanel = GameObject.Find("DebugMode");
    }


   private void Update()
   {
       if (stateController == null || stateText == null || stateController.currentState == null)
           return;
  
       if (isDebugMode == true && DebugPanel.activeSelf == false)
           DebugPanel.SetActive(true);
       else if (isDebugMode == false && DebugPanel.activeSelf == true)
           DebugPanel.SetActive(false);
  
       stateText.text = "Current "+stateController.currentState.ToString();
   }
}
