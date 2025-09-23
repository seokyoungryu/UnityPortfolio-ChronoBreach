using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshContainer : MonoBehaviour
{
    public SlotAllowType meshType = SlotAllowType.NONE;
    public SkinnedMeshInfo[] meshInfos = null;

    public string itemName = string.Empty;



    public void SetItemName(string name) => itemName = name;
    public string GetItemName() => itemName;

    [ContextMenu("meshInfos ¼³Á¤")]
    private void GetMeshInfos()
    {
        meshInfos = GetComponentsInChildren<SkinnedMeshInfo>();
    }
}
