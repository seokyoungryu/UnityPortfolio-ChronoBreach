using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action", fileName = "Action_")]
public abstract class QuestAction : ScriptableObject
{
    public abstract int Action(Task task, int currentCount, int getCount);
}
