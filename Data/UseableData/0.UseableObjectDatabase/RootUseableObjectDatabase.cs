using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Database/Root Useable Object Database", fileName = "RootUseableObjectDatabase")]
public class RootUseableObjectDatabase : BaseFindobjectDatabase<UseableObject>
{
   [SerializeField] private BuffObjectDatabase buffDatabases;
    [SerializeField] private PlayerStatsObjectDatabase playerStatsObjectDatabase;
    [SerializeField] private FunctionObjectDatabase functionObjectDatabase;

#if UNITY_EDITOR
    [ContextMenu("UseableObject 데이터 찾기")]
    public void FindUseableData()
    {
        FindAddDatas();
        SetID();
        SetDirtys();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }
#endif



    public UseableObject GetUseableObject(int id)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == id)
            {
                return database[i];
            }
        }
        return null;
    }
}
