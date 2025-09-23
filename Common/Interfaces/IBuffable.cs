using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffable 
{
    public void AddBuff(BuffType buffType , float buffValue);
    public void RemoveBuff(BuffType buffType, float buffValue);
}
