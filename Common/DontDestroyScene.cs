using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DontDestroyScene : MonoBehaviour
{
    public enum DontDestroyType
    {
        ANYWAY_DONTDESTROY = 0,
        NOT_EXCUTE = 1,
        EXCUTE = 2,
    }

    [SerializeField] private int excuteSceneChangeIndex = -1;
    public string objName = string.Empty;

    public DontDestroyType isExcute =  DontDestroyType.NOT_EXCUTE;
    private void Awake()
    {
        if(isExcute == DontDestroyType.ANYWAY_DONTDESTROY)
            DontDestroyOnLoad(this.gameObject);
    }

    private void OnValidate()
    {

        objName = gameObject.name;
    }

    public void Check()
    {
        if (excuteSceneChangeIndex == ScenesManager.Instance.ChangeCount ||
         excuteSceneChangeIndex == -1)
            isExcute = DontDestroyType.EXCUTE;

        if (isExcute == DontDestroyType.EXCUTE)
            DontDestroyOnLoad(this.gameObject);
        else  if(isExcute == DontDestroyType.NOT_EXCUTE)
        {
            Debug.Log(gameObject.name  + " - ªË¡¶ C : " + excuteSceneChangeIndex + " S :" +ScenesManager.Instance.ChangeCount);
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }
}
