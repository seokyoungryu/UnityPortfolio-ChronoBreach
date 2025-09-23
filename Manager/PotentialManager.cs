using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// potentialArrayList에 접근할수 있는 manager여
/// </summary>
public class PotentialManager : Singleton<PotentialManager>
{
    private PotentialData data = null;
    private PotentialDataContainer database = null;
    private PotentialFunctionsDatabase functionDatabase = null;

    protected override void Awake()
    {
        base.Awake();
        if (data == null)
        {
            data = ScriptableObject.CreateInstance<PotentialData>();
            data.LoadData();
        }

        if (database == null)
            database = data.potentialDatabase;
        if (functionDatabase == null)
            functionDatabase = data.functionDatabase;
    }


    public PotentialDataContainer GetDataBase() => database;
    public PotentialData GetData() => data;

    public PotentialOptionClip GetPotentialClip(int index) => database.allOptions[index];

    public PotentialFunctionsDatabase GetFunctionDatabase() => functionDatabase;
}                                                                                                          
