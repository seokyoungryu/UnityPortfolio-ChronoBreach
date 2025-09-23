using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayHelper : MonoBehaviour
{

    public static T[] Insert<T>(T[] array, int currIndex, int insertIndex)
    {
        if (array == null || array.Length <= 0) return null;
        if (insertIndex < 0 || currIndex < 0 || insertIndex >= array.Length) return array;

        T[] tempArr = array.Clone() as T[];
        T tempValue = tempArr[currIndex];

        if (currIndex < insertIndex)
        {
            for (int i = currIndex; i < insertIndex; i++)
            {
                tempArr[i] = tempArr[i + 1];
            }
        }
        else if( currIndex > insertIndex)
        {
            for (int i = currIndex; i > insertIndex; i--)
            {
                tempArr[i] = tempArr[i - 1];
            }
        }

        tempArr[insertIndex] = tempValue;

        return tempArr; 
    }

    public static T[] Resize<T>(T[] array, int count)
    {
        ArrayList tmpList = new ArrayList();
        for (int i = 0; i < count; i++)
        {
            if (array.Length > i)
                tmpList.Add(array[i]);
            else
                tmpList.Add(array[array.Length - 1]);
        }

        return tmpList.ToArray(typeof(T)) as T[];
    }

    public static T[] Add<T>(T newString, T[] list)
    {
        ArrayList tmpList = new ArrayList();
        foreach (T obj in list)
        {
            tmpList.Add(obj);
        }
        tmpList.Add(newString);
        return tmpList.ToArray(typeof(T)) as T[];
    }

    public static T[] Remove<T>(int index, T[] list)
    {
        ArrayList tmpList = new ArrayList();
        foreach (T obj in list)
        {
            tmpList.Add(obj);
        }
        tmpList.RemoveAt(index);
        return tmpList.ToArray(typeof(T)) as T[];
    }

    public static T[] Copy<T>(T[] list)
    {
        if (list == null) return null;
        ArrayList tmpList = new ArrayList();
        foreach (T obj in list)
        {
            tmpList.Add(obj);
        }
        return tmpList.ToArray(typeof(T)) as T[];
    }


    public static T[] CombineTwoArray<T>(T[] firstArray, T[] lastArray)
    {
        //Debug.Log(firstArray.Length +","+ lastArray.Length);
        if (firstArray == null || lastArray == null) return null;
        ArrayList tmpList = new ArrayList();
        foreach (T item in firstArray)
        {
            tmpList.Add(item);
            
        }
        foreach (T item in lastArray)
        {
            tmpList.Add(item);
        }
        //Debug.Log("รั : " + tmpList.Count);
        return tmpList.ToArray(typeof(T)) as T[];
    }


}
