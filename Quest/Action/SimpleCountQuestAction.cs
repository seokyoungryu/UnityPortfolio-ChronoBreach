using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/Simple Count", fileName = "SimpleCountQuestAction")]
public class SimpleCountQuestAction : QuestAction
{
    public override int Action(Task task, int currentCount, int getCount)
    {
        return currentCount + getCount;
    }
}
