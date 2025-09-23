using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActve : MonoBehaviour
{
    [SerializeField] private Transform[] tr;

    private void Awake()
    {
        ScenesManager.Instance.onExucteInit += () => Acitve();
    }

    private void Acitve()
    {
        Debug.Log("A ½ÇÇà");
        for (int i = 0; i < tr.Length; i++)
            tr[i].gameObject.SetActive(false);

    }
}
