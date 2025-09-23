using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData : ScriptableObject
{
    public const string dataDirectory = "/10.Resources/Resources/Data/";


    public int GetDataCount<T>(T[] array)
    {
        int count = 0;

        if (array != null)
            count = array.Length;

        return count;
    }

    public string[] GetNameList(bool showID, string[] array, string filterWord = "")
    {
        string[] retList = new string[0];

        if (array == null)
            return retList;

        retList = new string[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            if (filterWord != "")
            {
                if (array[i].ToLower().Contains(filterWord.ToLower()) == false)
                    continue;
            }
            if (showID)
            {
                retList[i] = i.ToString() + ":" + array[i];
            }
            else
            {
                retList[i] = array[i];
            }
        }

        return retList;
    }

    public virtual int FindIDToIndex(int targetID)
    {
        return 0;
    }

    public virtual void AddData(string newName)
    {

    }

    public virtual void AddData(string newName, int enumdata)
    {

    }

    public virtual void RemoveData(int index)
    {

    }

    public virtual void Copy(int index)
    {

    }
}
