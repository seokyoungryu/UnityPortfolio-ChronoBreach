using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Database/Skill database", fileName = "SkillClipDatabase")]
public class SkillDatabase : BaseFindobjectDatabase<BaseSkillClip>
{

#if UNITY_EDITOR
    [ContextMenu("Skill Clips 데이터 찾기")]
    private void Find()
    {
        FindAddDatas();
        SetID();
        SetDirtys();
    }
#endif


    public BaseSkillClip GetSkillClone(int skillID)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == skillID)
                return Instantiate(database[i]);
        }
        return null;
    }

    public BaseSkillClip GetSkillOrigin(int skillID)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == skillID)
                return database[i] ;
        }
        return null;
    }
}
