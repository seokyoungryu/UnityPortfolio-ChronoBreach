using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AfterColisionRangeAttackInfo))]
public class AfterRangeAttackInfosGUI : Editor
{
    private AfterColisionRangeAttackInfo clip = null;

    public override void OnInspectorGUI()
    {
        clip = (AfterColisionRangeAttackInfo)target;
        base.OnInspectorGUI();

        if (clip.CreateCount < clip.CreateSpawnPosition.Count)
        {
            for (int i = 0; i < clip.CreateSpawnPosition.Count; i++)
            {
                if (clip.CreateCount != clip.CreateSpawnPosition.Count)
                {
                    clip.CreateSpawnPosition.RemoveAt(clip.CreateSpawnPosition.Count - 1);
                    clip.CreateRotation.RemoveAt(clip.CreateRotation.Count - 1);
                }
            }
        }

        if (clip.CreateCount > clip.CreateSpawnPosition.Count)
        {
            for (int i = 0; i < clip.CreateSpawnPosition.Count; i++)
            {
                if (clip.CreateCount != clip.CreateSpawnPosition.Count)
                {
                    clip.CreateSpawnPosition.Add(new Vector3());
                    clip.CreateRotation.Add(new Vector3());
                }
            }
        }

        GUILayout.BeginVertical("HelpBox");
        {
            EditorGUILayout.LabelField("회전값은 RotateToTarget일경우 projectile.forward를 기준.");
           
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("HelpBox");
        {
            clip.infos = EditorGUILayout.ObjectField("Attack Info So", clip.infos, typeof(RangeAttackInfo), false) as RangeAttackInfo;
        }
        GUILayout.EndVertical();
    }

}
