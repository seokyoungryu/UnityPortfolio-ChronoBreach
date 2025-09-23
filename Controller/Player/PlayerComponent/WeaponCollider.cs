using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private BoxCollider collider = null;
    public GameObject prefab;
    void Start()
    {
        collider = GetComponent<BoxCollider>();    
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Test"))
        {
        }
    }
}
