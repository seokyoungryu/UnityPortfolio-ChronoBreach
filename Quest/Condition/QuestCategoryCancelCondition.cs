using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Quest/Conditions/Cancel Condition/Quest Category Conditon", fileName = "CategoryCancelCondition_")]
public class QuestCategoryCancelCondition : QuestCondition
{
    [SerializeField] private QuestCategory category;

    public override bool IsPass(Quest quest)
    {
        if (!quest.Category.CompareCategory(category))
            return false;

        return true;
    }
}
