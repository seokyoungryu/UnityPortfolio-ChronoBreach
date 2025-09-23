using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectClip
{
    public int id = 0;
    public string effectFullPath = string.Empty;// effectprefab ºÒ·¯¿È
    public string effectPath = string.Empty;
    public string effectName = string.Empty;

    public bool applyChildScale = false;
    public EffectType effectType = EffectType.NONE;
    public GameObject effectPrefab = null;

    public EffectClip() { }
    public EffectClip(int ID, string name)
    {
        id = ID;
        effectName = name;
    }


    public void LoadEffectPrefab()
    {
        effectName = effectName.Trim();
        effectPath = effectPath.Trim();

        effectFullPath = effectPath + effectName;
        if(effectFullPath != string.Empty && effectPrefab == null )
        {
            effectPrefab = Resources.Load(effectFullPath) as GameObject;
        }
    }
}
