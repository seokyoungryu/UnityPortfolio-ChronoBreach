using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatsObject : UseableObject
{
    [SerializeField] protected float value = 0f;
    [SerializeField, TextArea(0, 2)] protected string description = string.Empty;
    [SerializeField] protected bool applyOriginStat = false;

    public float Value => value;
    public string Description => description;
    public bool ApplyOriginStat => applyOriginStat;

    public override void Apply(BaseController controller)
    {
    }

    public virtual void RemoveApplyValue(BaseController controller) { }
}
