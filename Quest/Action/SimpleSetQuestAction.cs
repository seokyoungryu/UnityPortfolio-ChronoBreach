using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Quest/Task/Action/Simple Set", fileName ="SimpleSetQuestAction")]
public class SimpleSetQuestAction : QuestAction
{
    public override int Action(Task task, int currentCount, int getCount)
    {
        return getCount;
    }

}
