using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/StringTarget", fileName ="TargetString_")]
public class StringTaskTarget : TaskTarget
{
    [SerializeField] private string value;
    [SerializeField] private string displayName;

    public override object Value => value;
    public string DisplayName => displayName;

    public override bool IsEqual(object target)
    {
        if(target is StringTaskTarget)
        {
            if ((target as StringTaskTarget).value == value)
            {

                Debug.Log((target as StringTaskTarget).value);
                return true;
            }

        }
        string targetString = target as string;

       if (targetString == null)
           return false;

        return value == targetString;
    }
}
