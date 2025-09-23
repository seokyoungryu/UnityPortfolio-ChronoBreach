using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Exp Data")]
public class ExpData : ScriptableObject
{
    [SerializeField] private TextAsset expCSVFile = null;
    [SerializeField] private List<ExpContainer> exps = new List<ExpContainer>();


    public ExpContainer GetExpContainer(int lv)
    {
        foreach (ExpContainer container in exps)
            if (container.Lv == lv)
                return container;
        return null;
    }

    [ContextMenu("Csv √ﬂ√‚")]
    private void CsvToData()
    {
        exps.Clear();
        string expText = expCSVFile.text;
        string[] expEnter = expText.Split('\n');

        for (int i = 1; i < expEnter.Length-1; i++)
        {
            string[] expTap = expEnter[i].Split(',');
            int lv = int.Parse(expTap[0]);
            long requiredExp = long.Parse(expTap[1]);
            ExpContainer exp = new ExpContainer(lv, requiredExp);
            exps.Add(exp);
        }
    }
}
