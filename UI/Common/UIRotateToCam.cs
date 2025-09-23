using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateToCam : MonoBehaviour
{
    private Camera cam;
    public bool isCheck = true;

    private void Awake()
    {
        cam = GameManager.Instance.Cam?.MainCam;
    }


    private void Update()
    {
        if (cam == null) return;
        Vector3 rot = transform.position + cam.transform.parent.rotation * Vector3.forward;
        Vector3 worldRot = cam.transform.parent.rotation * Vector3.up;

        if (isCheck)
            transform.LookAt(rot, worldRot);
        else if (!isCheck)
            transform.LookAt(rot);
    }

}
