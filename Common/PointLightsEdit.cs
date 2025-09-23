using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightsEdit : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    [SerializeField] private float applyPercent = 1f;


    [ContextMenu("Àû¿ë")]
    private void Edit()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity *= applyPercent;
            lights[i].range *= applyPercent;

        }
    }
}
