using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Phase/Phase Info", fileName ="Phase_")]
public class PhaseInfo : ScriptableObject
{
    public List<AIPhaseAttackData> phaseInfos;


    private void OnValidate()
    {
        if (phaseInfos.Count <= 0) return;

        for (int i = 0; i < phaseInfos.Count; i++)
        {
            phaseInfos[i].phaseCount = i + 1;
            phaseInfos[i].phaseName = "Phase_" + (i + 1);
            if (phaseInfos[i].animClip != null)
                phaseInfos[i].animFullFrame = phaseInfos[i].animClip.length * phaseInfos[i].animClip.frameRate;
        }
    }

}
