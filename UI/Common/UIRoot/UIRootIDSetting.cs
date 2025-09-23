using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIRootIDSetting : MonoBehaviour
{
    public List<UIRoot> uiRoots = new List<UIRoot>();

#if UNITY_EDITOR
    [ContextMenu("UI ID Setting")]
    public void ExcuteUIRootIDSetting()
    {
        uiRoots.Clear();
        UIRoot[] uis = FindObjectsOfType<UIRoot>();

        for (int i = 0; i < uis.Length; i++)
        {
            uis[i].UIID = i + 1;
            uiRoots.Add(uis[i]);
        }
        for (int i = 0; i < uiRoots.Count; i++)
            EditorUtility.SetDirty(uiRoots[i]);

        EditorUtility.SetDirty(this);
    }
#endif

    [ContextMenu("UI ID Debug")]
    public void ExcuteUIRootIDDebug()
    {
        for (int i = 0; i < uiRoots.Count; i++)
            Debug.Log(uiRoots[i].name + " : " + uiRoots[i].UIID);

    }
}
