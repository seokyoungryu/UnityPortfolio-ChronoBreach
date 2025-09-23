using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterSceneActive : MonoBehaviour
{

    //[SerializeField] private List<GameObject> controllerObjects = new List<GameObject>();


    private void Start()
    {
        ScenesManager.Instance.onExucteInit += () => this.gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        ScenesManager.Instance.onExucteInit -= () => this.gameObject.SetActive(false);
    }

    // void Awake()
    // {
    //     ScenesManager.Instance.OnExcuteInit += () => RegisterObjsSetActive(false);
    //     if (!controllerObjects[0].activeSelf)
    //         RegisterObjsSetActive(true);
    // }
    //
    //
    // private void RegisterObjsSetActive(bool active)
    // {
    //     for (int i = 0; i < controllerObjects.Count; i++)
    //         controllerObjects[i].SetActive(active);
    //
    //     Debug.Log("이거 실행");
    // }
    //
}
