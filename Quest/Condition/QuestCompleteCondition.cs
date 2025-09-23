using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Conditions/Require Condition/Quest Complete Conditon", fileName = "CompleteCondition_")]
public class QuestCompleteCondition : QuestCondition
{
    [SerializeField] public Quest conditionQuest;

    public override bool IsPass(Quest quest)
    {
        //Debug.Log(quest.CodeName + " üũ : " + QuestManager.Instance.ExistCompleteQuest(quest));
        if (QuestManager.Instance.ExistCompleteQuest(conditionQuest))
            return true;
        else
            return false;

    }

}
