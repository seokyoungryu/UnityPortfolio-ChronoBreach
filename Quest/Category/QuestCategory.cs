using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Quest/Category", fileName ="Category_")]
public class QuestCategory : ScriptableObject
{
    [SerializeField] private string codeName;
    [SerializeField] private string displayName;


    public string CodeName => codeName;
    public string DisplayName => displayName;

    public bool CompareCategory(QuestCategory category)
    {
        if (category == null) return false;

        if (this.codeName == category.CodeName || this.DisplayName == category.DisplayName)
        {
            return true;
        }

        return false;
    }
}
