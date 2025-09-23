using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolDataContainer
{
    [SerializeField] private string poolZipName = string.Empty;
    [SerializeField] private List<PoolData> pools = new List<PoolData>();

    public List<PoolData> Pools => pools;
    public string PoolZipName => poolZipName;
}
