using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeGameActive : MonoBehaviour
{
    [SerializeField] private bool awakeActive = true;
    [SerializeField] private List<GameObject> applyObjs = new List<GameObject>();

    private void Start()
    {
        Debug.Log("Awake Active!");
        for (int i = 0; i < applyObjs.Count; i++)
            applyObjs[i].SetActive(awakeActive);
    }

}
