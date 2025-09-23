using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/Continue Positive Count", fileName = "ContinuePositiveCountQuestAction")]
public class ContinuePositiveQuestAction : QuestAction
{
    public override int Action(Task task, int currentCount, int getCount)
    {
        return getCount > 0 ? currentCount + getCount : 0;
    }
   
}
