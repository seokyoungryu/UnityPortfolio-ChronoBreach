using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolData
{
    public string name = string.Empty;
    public int count = 0;
    public bool includeAllDisable = true;
    public GameObject prefab = null;
    public Transform parent = null;
}

[System.Serializable]
public class EffectPoolData : PoolData
{
    public int idForEffect = 0;
}