using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMaterialColorSetting : MonoBehaviour
{
    public Renderer renderer;
    public CharacterSkinnedMeshColorInfo colorInfo;

    private void Awake()
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        colorInfo.SetMaterialsColor(renderer);
    }



    [ContextMenu("Color Setting")]
    private void ColorSetting()
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();
        colorInfo.InitMaterialsColor(renderer.sharedMaterial);
    }

}
