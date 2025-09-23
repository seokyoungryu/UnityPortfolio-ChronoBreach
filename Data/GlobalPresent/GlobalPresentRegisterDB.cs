using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Global/GlobalPresent/GlobalPresent Register DB", fileName ="GlobalPresentRegisterDB")]
public class GlobalPresentRegisterDB : ScriptableObject
{
    [SerializeField] private List<GlobalPresentTask> lists = new List<GlobalPresentTask>();

    public List<GlobalPresentTask> GetList => lists;
  
}
