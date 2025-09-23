using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/Continue Negative Count", fileName = "ContinueNegativeCountQuestAction")]
public class ContinueNegativeQuestAction : QuestAction
{
    public override int Action(Task task, int currentCount, int getCount)
    {
        return currentCount < 0 ? currentCount + getCount : 0;
    }

}
