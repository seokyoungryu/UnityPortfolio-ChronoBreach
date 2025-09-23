using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabNameChange : MonoBehaviour
{
    public UnityEngine.Object[] gos;
    public string front = "";
    public string changeTarget = "";
    public string changeTo = "";


#if UNITY_EDITOR
    [ContextMenu("Change!")]
    public void Change()
    {
        for (int i = 0; i < gos.Length; i++)
        {
            string name = "";
            if(gos[i].name.Contains(changeTarget))
            {
                name = gos[i].name;
                name = name.Replace(changeTarget, changeTo);
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(gos[i]), front+name);
            }
        }

    }
#endif


}
