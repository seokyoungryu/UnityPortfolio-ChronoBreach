using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ExcuteAfterCollsionType
{
    ONCE =0 ,
    PERHIT = 1,
}


[CreateAssetMenu(menuName = "Data/Attack Data/After Collision RAnge Attack Info")]
public class AfterColisionRangeAttackInfo : ScriptableObject
{
    public ExcuteAfterCollsionType excuteType = ExcuteAfterCollsionType.ONCE;
    [HideInInspector] public EffectList projectileEffect = EffectList.None;
   public int createCount = 1;
   public List<Vector3> createSpawnPosition = new List<Vector3>();
   public List<Vector3> createRotation = new List<Vector3>();
   public float excuteDelay = 0f;
   public float createDelay = 0.5f;
    public bool rotateToTarget = true;
    [HideInInspector] public RangeAttackInfo infos;
    private int hasCreatedCount = 0;

    public ExcuteAfterCollsionType ExcuteType => excuteType;
    public int CreateCount { get { return createCount; } set { createCount = value; } }
    public List<Vector3> CreateSpawnPosition { get { return createSpawnPosition; } set { createSpawnPosition = value; } }
    public List<Vector3> CreateRotation { get { return createRotation; } set { createRotation = value; } }
    public float CreateDelay { get { return createDelay; } set { createDelay = value; } }
    public EffectList ProjectileEffect { get { return projectileEffect; } set { projectileEffect = value; } }
    public float ExcuteDelay { get { return excuteDelay; } set { excuteDelay = value; } }
    public int HasCreatedCount { get { return hasCreatedCount; } set { hasCreatedCount = value; } }
    public bool RotateToTarget { get { return rotateToTarget; } set { rotateToTarget = value; } }

    public AfterColisionRangeAttackInfo Clone()
    {
        AfterColisionRangeAttackInfo clone = new AfterColisionRangeAttackInfo();
        clone.excuteType = excuteType;
        clone.projectileEffect = projectileEffect;
        clone.createCount = createCount;
        clone.createSpawnPosition = new List<Vector3>(createSpawnPosition);
        clone.createRotation = new List<Vector3>(CreateRotation);
        clone.createDelay = createDelay;
        clone.excuteDelay = excuteDelay;
        clone.rotateToTarget = rotateToTarget;
        if (infos != null)
            clone.infos = infos.Clone();

        return clone;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}

