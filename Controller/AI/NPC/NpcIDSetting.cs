using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIDSetting : MonoBehaviour
{
    [SerializeField] private NpcController[] npcControllers;

    private void Awake()
    {
        if (npcControllers.Length <= 0)
        {
            npcControllers = FindObjectsOfType<NpcController>();
            SettingNpcID();
        }
    }


    [ContextMenu("Id ¼³Á¤")]
    public void SettingNpcID()
    {
        npcControllers = FindObjectsOfType<NpcController>();
        for (int i = 0; i < npcControllers.Length; i++)
            npcControllers[i].ID = i;
    }
}
