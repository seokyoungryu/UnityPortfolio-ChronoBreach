using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyInfos : MonoBehaviour
{
    [SerializeField] private List<GameObject> dontDestoryList = new List<GameObject>();
    public List<GameObject> DontDestoryList => dontDestoryList;


    public void DestroySameList(string[] names)
    {
        List<GameObject> ret = new List<GameObject>();

        for (int i = 0; i < dontDestoryList.Count; i++)
        {
            Debug.Log("0 - " + dontDestoryList[i].name);

            for (int j = 0; j < names.Length; j++)
            {
                Debug.Log("1 - " + dontDestoryList[i].name + " :" + names[j]);

                if (dontDestoryList[i].name == names[j])
                {
                    Debug.Log("Add " + dontDestoryList[i].name + " :" + names[j]);
                    ret.Add(dontDestoryList[i]);
                }
            }
        }
        for (int i = 0; i < ret.ToArray().Length; i++)
        {
            Debug.Log("2 - " + ret[i].name);
        }

        for (int i = 0; i < ret.ToArray().Length; i++)
        {
            Debug.Log("»èÁ¦ - " + ret[i].name);
            Destroy(ret[i]);
        }
    }
}
