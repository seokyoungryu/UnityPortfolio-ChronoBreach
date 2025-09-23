using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjCreate : MonoBehaviour
{
    [SerializeField] private GameObject[] gos;

    private void Awake()
    {
        if (ScenesManager.Instance.ChangeCount == 1)
            for (int i = 0; i < gos.Length; i++)
                Instantiate(gos[i]);
    }

}
