using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterSkinnedMeshColorInfo 
{
    public Color Color_Primary;
    public Color Color_Secondary;
    public Color Color_Leather_Primary;
    public Color Color_Metal_Primary;
    public Color Color_Leather_Secondary;
    public Color Color_Metal_Dark;
    public Color Color_MertalSecondary;
    public Color Color_Hair;
    public Color Color_Skin;
    public Color Color_Stubble;
    public Color Color_Scar;
    public Color Color_BodyArt;
    public Color Color_Eyes;

    public MaterialPropertyBlock block;

    public void InitMaterialsColor(Material material)
    {
        Color_Primary = material.GetColor("_Color_Primary");
        Color_Secondary = material.GetColor("_Color_Secondary");
        Color_Leather_Primary= material.GetColor("_Color_Leather_Primary");
        Color_Metal_Primary= material.GetColor("_Color_Metal_Primary");
        Color_Leather_Secondary= material.GetColor("_Color_Leather_Secondary");
        Color_Metal_Dark= material.GetColor("_Color_Metal_Dark");
        Color_MertalSecondary= material.GetColor("_Color_Metal_Secondary");
        Color_Hair= material.GetColor("_Color_Hair");
        Color_Skin= material.GetColor("_Color_Skin");
        Color_Stubble= material.GetColor("_Color_Stubble");
        Color_Scar= material.GetColor("_Color_Scar");
        Color_BodyArt= material.GetColor("_Color_BodyArt");
        Color_Eyes= material.GetColor("_Color_Eyes");
    }

    public void SetMaterialsColor(Renderer renderer)
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        propertyBlock.SetColor("_Color_Primary", Color_Primary);
        propertyBlock.SetColor("_Color_Secondary", Color_Secondary);
        propertyBlock.SetColor("_Color_Leather_Primary", Color_Leather_Primary);
        propertyBlock.SetColor("_Color_Metal_Primary", Color_Metal_Primary);
        propertyBlock.SetColor("_Color_Leather_Secondary", Color_Leather_Secondary);
        propertyBlock.SetColor("_Color_Metal_Dark", Color_Metal_Dark);
        propertyBlock.SetColor("_Color_Metal_Secondary", Color_MertalSecondary);
        propertyBlock.SetColor("_Color_Hair", Color_Hair);
        propertyBlock.SetColor("_Color_Skin", Color_Skin);
        propertyBlock.SetColor("_Color_Stubble", Color_Stubble);
        propertyBlock.SetColor("_Color_Scar", Color_Scar);
        propertyBlock.SetColor("_Color_BodyArt", Color_BodyArt);
        propertyBlock.SetColor("_Color_Eyes", Color_Eyes);

        renderer.SetPropertyBlock(propertyBlock);
    }


}
