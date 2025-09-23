using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectObjectsInHierarchy : MonoBehaviour
{
    public List<GameObject> gos = new List<GameObject>();


#if UNITY_EDITOR
    [ContextMenu("하이어라키에서 선택")]
    public void Select()
    {
        List<GameObject> selectedGos = new List<GameObject>();

        for (int i = 0; i < gos.Count; i++)
            selectedGos.Add(gos[i].transform.parent.gameObject);

        Selection.objects = selectedGos.ToArray();
    }
#endif
}
