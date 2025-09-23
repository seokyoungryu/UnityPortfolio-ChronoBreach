using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private float waitTime = 0.02f;
    [Header("Max 1")]
    [SerializeField] private Vector3 rotateDir = Vector3.zero;
    [SerializeField] private float rotatePower = 10f;
    private bool isStart = true;

    private WaitForSeconds waitFSceond;


    void Start()
    {
        waitFSceond = new WaitForSeconds(waitTime);
        StartCoroutine(Rotate_Co());
    }


    private IEnumerator Rotate_Co()
    {
        rotateDir.Normalize();
        rotateDir.x *= rotatePower;
        rotateDir.y *= rotatePower;
        rotateDir.z *= rotatePower;
        Quaternion dir = Quaternion.Euler(rotateDir);

        while (isStart)
        {
            transform.rotation *= dir;
            yield return waitFSceond;
        }

    }



    private void OnValidate()
    {
        if (rotateDir.x > 1) rotateDir.x = 1;
        if (rotateDir.y > 1) rotateDir.y = 1;
        if (rotateDir.z > 1) rotateDir.z = 1;

    }
}
