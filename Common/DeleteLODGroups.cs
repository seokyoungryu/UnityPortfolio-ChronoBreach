using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteLODGroups : MonoBehaviour
{
    public List<GameObject> rootObjs = new List<GameObject>();



    [ContextMenu("삭제하기")]
    public void ExcuteDeleteLODGroups()
    {
        for (int i = 0; i < rootObjs.Count; i++)
            Delete(rootObjs[i]);
    }



    private void Delete(GameObject root)
    {
        LODGroup[] groups = root.GetComponentsInChildren<LODGroup>();

        for (int i = 0; i < groups.Length; i++)
        {
            if (groups[i].gameObject == root)
                Debug.Log("Root : " + groups[i].gameObject.name);

            if (groups[i].gameObject != root)
                DestroyImmediate(groups[i].gameObject);
        }

    }


   
}
