using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    [SerializeField] private bool isTitle = false;

    [Header("Screen Touch")]
    [SerializeField] private bool useScreenTouch = true;
    [SerializeField] private bool canExcuteAllCloseScreenTouch = true;
    [SerializeField] private bool canExcuteESC = true;
    [SerializeField] private bool isUnInterruptibleUI = false;
    public bool IsTitle { get { return isTitle; } set { isTitle = value; } }
    public bool UseScreenTouch { get { return useScreenTouch; } set { useScreenTouch = value; } }
    public bool CanExcuteESC { get { return canExcuteESC; } set { canExcuteESC = value; } }
    public bool IsUnInterruptibleUI { get { return isUnInterruptibleUI; } set { isUnInterruptibleUI = value; } }

    public bool CanExcuteScreenTouch { get { return canExcuteAllCloseScreenTouch; } set { canExcuteAllCloseScreenTouch = value; } }
}
