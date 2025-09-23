using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDontDestroys : MonoBehaviour
{
    [SerializeField] private List<DontDestroyScene> lists = new List<DontDestroyScene>();


    private void Awake()
    {
        for (int i = 0; i < lists.Count; i++)
            lists[i].Check();
    }
}
