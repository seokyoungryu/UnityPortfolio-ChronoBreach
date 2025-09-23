using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantItemClip : BaseItemClip
{
    [Header("Enchant Stats")]
    public EnchantType enchantType = EnchantType.WEAPON;
    public float successPercent = 0;
    public float minValue = 0f;
    public float maxValue = 0f;
    public float value = 0f;

  
    public EnchantItemClip() { }
    public EnchantItemClip(int index, string name, int enumNum) : base(index, name, enumNum) { }
    public EnchantItemClip(BaseItemClip clip) : base(clip) { }

}


public enum EnchantType
{
    WEAPON =0,
    ARMOR =1,
    ACCESSORY =2,

}
