using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpContainer
{
    [SerializeField] private string titleName = string.Empty;
    [SerializeField] private int lv = 0;
    [SerializeField] private long requiredExp = 0;

    public int Lv => lv;
    public long RequiredExp => requiredExp;

    public ExpContainer(int lv, long repuiredExp)
    {
        this.lv = lv;
        this.requiredExp = repuiredExp;
        titleName = "Lv." + lv;
    }
}
