using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SoundClip
{
    public int id = 0;
    public string clipFullPath = string.Empty;
    public string clipPath = string.Empty;
    public string clipName = string.Empty;

    public SoundPlayType playType = SoundPlayType.Effect;
    public SoundPlayTypeBGM playTypeBgm ;
    public SoundPlayTypeEffect playTypeEffect;
    public SoundPlayTypeUI playTypeUI;
    public SoundPlayTypeETC playTypeETC;

    public bool isAwakePlay = false;
    public bool isLoop = false;
    public AudioClip clip;
    public float pitch = 1.0f;
    public float volume = 1f;
    public float spatialBlend = 0f;
    public float minDistance = 10000.0f;
    public float maxDistance = 40000.0f;

    

    public SoundClip() { }
    public SoundClip(int ID, string name)
    {
        id = ID;
        clipName = name;
    }


    public void LoadClipData()
    {
        clipPath = clipPath.Trim();
        clipName = clipName.Trim();
        clipFullPath = clipPath + clipName;
        clip = Resources.Load(clipFullPath) as AudioClip;
    }

}
