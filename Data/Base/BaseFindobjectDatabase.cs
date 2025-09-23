using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BaseFindobjectDatabase<T> : ScriptableObject where T : BaseScriptableObject
{
    public List<T> database = new List<T>();


#if UNITY_EDITOR
    [ContextMenu("Setting ID")]
    public void SetID()
    {
        for (int i = 0; i < database.Count; i++)
            database[i].ID = i;
    }

    [ContextMenu("Clear Database")]
    public void Clear()
    {
        database.Clear();
    }

    public void SetDirtys()
    {
        for (int i = 0; i < database.Count; i++)
            EditorUtility.SetDirty(database[i]);
    }
    public void AddData(T data) 
    {
        database.Add(data);
        AssetDatabase.SaveAssets();
    }


    public void FindAddDatas() 
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T functionObject = AssetDatabase.LoadAssetAtPath<T>(path);
            bool isHave = false;

            for (int i = 0; i < database.Count; i++)
                if (database[i].GetInstanceID() == functionObject.GetInstanceID())
                    isHave = true;

            if (!isHave)
                database.Add(functionObject);
        }

        for (int i = 0; i < database.Count; i++)
        {
            if (database[i] == null)
            {
                database.Remove(database[i]);
                i = 0;
            }
        }
        AssetDatabase.SaveAssets();

    }
#endif

}
