using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LODSettings : MonoBehaviour
{
    public LODGroup[] groups ;

    public float value;


    [ContextMenu("Settings")]
    public void Settings()
    {
        groups = FindObjectsOfType<LODGroup>();

        foreach (LODGroup lodGroup in groups)
        {
            LOD[] lods = lodGroup.GetLODs();

            if (lods.Length < 3) continue; 

            float[] newDistances = { 0.15f, 0.08f, 0.01f }; 

            for (int i = 0; i < lods.Length; i++)
                if (i < newDistances.Length)
                    lods[i].screenRelativeTransitionHeight = newDistances[i];

            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
        }
    }

}
