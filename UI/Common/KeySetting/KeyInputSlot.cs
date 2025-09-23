using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputSlot : MonoBehaviour
{
    [SerializeField] private GameObject window = null;
    [SerializeField] private Item item = null;

    public GameObject Window => window;
    public Item Item => item;

}
