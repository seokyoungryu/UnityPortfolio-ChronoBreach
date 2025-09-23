using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject newGameobject = new GameObject(typeof(T).Name, typeof(T));
                    instance = newGameobject.GetComponent<T>();
                    Debug.Log(typeof(T) + "instance 생성 : " + instance);

                }
            }
            return instance;
        }
    }



    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 중복된 인스턴스 삭제
            return;
        }
        instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }

}
