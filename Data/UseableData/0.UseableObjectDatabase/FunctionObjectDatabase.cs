using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Function Object Database", fileName = "FunctionObjectDatabase")]
public class FunctionObjectDatabase : BaseFindobjectDatabase<FunctionObject>
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
