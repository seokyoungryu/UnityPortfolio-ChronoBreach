using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[CreateAssetMenu(menuName = "Database/Create Define Database/Quest Category", fileName = "QuestCategoryDefineDatabase")]
public class QuestCategoryDefineDatabase : CreateDefineDatabase<QuestCategory>
{



    public QuestCategory GetCategory(string codeName)
    {
        for (int i = 0; i < defineList.Count; i++)
            if (defineList[i].CodeName == codeName)
                return defineList[i];

        return null;
    }

#if UNITY_EDITOR
    protected override StringBuilder WriteDefineDatas()
    {
        StringBuilder builder = new StringBuilder();
        string normalText = "public static string ";

        builder.AppendLine();
        for (int i = 0; i < defineList.Count; i++)
        {
            builder.AppendLine(normalText + defineList[i].CodeName+ " = " +'"' + defineList[i].CodeName + '"'+ ";");
        }

        return builder;
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Define »ý¼º!!")]
    protected override void CreateDefine()
    {
        base.CreateDefine();
    }
#endif

    private void OnValidate()
    {
        if (defineClassName == string.Empty)
            defineClassName = "QuestCategoryDefines";
    }
}
