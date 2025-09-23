using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScriptableObject : ScriptableObject
{
    [SerializeField] protected int id = -1;

    public int ID { get { return id; } set { id = value; } }

}
