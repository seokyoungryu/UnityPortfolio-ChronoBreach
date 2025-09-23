using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(menuName = "Camera/Cam Shake Info", fileName ="CShake_")]
public class CameraShakeInfo  : ScriptableObject
{
    public bool isIgnoreStrength = false;
    public CamStrength camStrength = CamStrength.NONE;
    public CamShakeType shakeType = CamShakeType.NONE;
    public Vector3 minShakePositon = Vector3.zero;
    public Vector3 maxShakePositon = Vector3.zero;
    public Vector3 targetShakePositon = Vector3.zero;

    public int count = 0;
    public float duration = 0f;
    public float changeRate = 0f;
    public float lerpSpeed = 0f;
    public float reduceRate = 0f;

    [Header("Zoom Z")]
    public AnimationCurve curveX;
    public AnimationCurve curveY;
    public AnimationCurve curveZ;


#if UNITY_EDITOR
    [ContextMenu("Save")]
    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif

  
}
