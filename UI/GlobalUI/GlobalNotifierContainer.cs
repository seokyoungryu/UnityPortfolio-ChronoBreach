using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GlobalNotifierType
{
    NONE = -1,
    NORMAL =0,
    SIMPLE =1,
    BATTLE =2,
    ITEMGAIN =3,
}

[System.Serializable]
public class GlobalNotifierContainer
{
    [SerializeField] private GlobalNotifierType notifierType = GlobalNotifierType.NONE;
    [SerializeField] private string notifierOBPName = string.Empty;
    [SerializeField] private SoundList notifierSound = SoundList.None;

    public GlobalNotifierType NotifierType => notifierType;
    public string NotifierOBPName => notifierOBPName;
    public SoundList NotifierSound => notifierSound;


}
