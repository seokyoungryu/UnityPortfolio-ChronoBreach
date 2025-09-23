using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialFunctionObject : BaseScriptableObject
{
   
    [SerializeField] private string functionName = string.Empty;
    [SerializeField,TextArea(1,2)] private string description = string.Empty;

    public string FunctionName => functionName;
    public string Description => description;

    public virtual void Apply(float value , PlayerStatus playerStatus) {}
    public virtual void Remove(float value, PlayerStatus playerStatus) { }
}
