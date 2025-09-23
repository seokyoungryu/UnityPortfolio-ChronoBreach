using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InteractAnimObject : InteractTriggerMoveObject
{
    [SerializeField] private Animator anim;
    [SerializeField] private string openAnimName = string.Empty;
    [SerializeField] private string closeAnimName = string.Empty;


    protected override void Open()
    {
        Debug.Log("InterAnimOpen");

    }

    protected override void Close()
    {
        Debug.Log("InterAnimCLose");

    }



}



