using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UseableObject : BaseScriptableObject
{
    [SerializeField] protected string objectName = string.Empty;

    public string ObjectName => objectName;


    public abstract void Apply(BaseController controller);
   
}
