using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/Positive Count", fileName = "PositiveCountQuestAction")]
public class PositiveQuestAction : QuestAction
{
    public override int Action(Task task, int currentCount, int getCount)
    {
        return getCount > 0 ? currentCount + getCount : currentCount;
    }
}
