using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Player Stats Object Database", fileName = "PlayerStatsObjectDatabase")]
public class PlayerStatsObjectDatabase : BaseFindobjectDatabase<BaseStatsObject>
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
