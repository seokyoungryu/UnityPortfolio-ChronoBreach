using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName ="Database/Buff Stats Object Database" , fileName ="BuffObjectDatabase")]
public class BuffObjectDatabase : BaseFindobjectDatabase<BuffStatsObject>
{
#if UNITY_EDITOR
    [ContextMenu("Find")]
    public void Find()
    {
        FindAddDatas();
        SetDirtys();
    }
#endif
}
