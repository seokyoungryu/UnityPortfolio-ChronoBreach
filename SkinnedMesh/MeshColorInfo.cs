using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColorInfo : MonoBehaviour
{
    [SerializeField] private Color colorSetting;
    private Renderer renderer;
    private MaterialPropertyBlock materialProperty;


    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        materialProperty = new MaterialPropertyBlock();
        materialProperty.SetColor("_Color", colorSetting);
        renderer.SetPropertyBlock(materialProperty);
    }

    [ContextMenu("Color Setting")]
    private void SettingColor()
    {
        colorSetting = GetComponent<MeshRenderer>().sharedMaterial.color;
    }

}
