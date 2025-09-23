using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkinnedMeshInfo : MonoBehaviour
{
    public GameObject model = null;
    public string[] boneNames = null;
    public string rootBoneName = string.Empty;
    public CharacterSkinnedMeshColorInfo colorInfo;

    [ContextMenu("본&색깔 세팅")]
    private void BothSetting()
    {
        SetBoneName();
        SetColor();
    }

    [ContextMenu("본 세팅")]
    private void SetBoneName()
    {
        if (model == null) model = this.gameObject;
        SkinnedMeshRenderer skin = model.GetComponent<SkinnedMeshRenderer>();
        boneNames = new string[skin.bones.Length];
        rootBoneName = skin.rootBone.name;
        for (int i = 0; i < skin.bones.Length; i++)
        {
            boneNames[i] = skin.bones[i].name;
        }
    }

    [ContextMenu("Color Setting")]
    private void SetColor()
    {
        SkinnedMeshRenderer skin = model.GetComponent<SkinnedMeshRenderer>();
        colorInfo.InitMaterialsColor(skin.sharedMaterial);
    }
}
