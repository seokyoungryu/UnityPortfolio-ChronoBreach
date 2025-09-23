using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Database/Potential Functions Database", fileName = "PotentialFunctionsDatabase")]
public class PotentialFunctionsDatabase : BaseFindobjectDatabase<PotentialFunctionObject>
{

    public PotentialFunctionObject GetFunctionObject_Origin(int id)
    {
        return database[id] ;
    }

    public PotentialFunctionObject GetFunctionObject_Clone(int id)
    {
        return Instantiate(database[id]);
    }

#if UNITY_EDITOR

    [ContextMenu("FunctionObjects 찾고 ID 설정")]
    public void FindAndSetID()
    {
        FindAddDatas();
        SetID();
        //  SetDirty();
        for (int i = 0; i < database.Count; i++)
            EditorUtility.SetDirty(database[i]);

        AssetDatabase.SaveAssets();
    }

#endif
}
