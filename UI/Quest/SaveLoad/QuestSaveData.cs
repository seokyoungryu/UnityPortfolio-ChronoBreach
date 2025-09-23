using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct QuestSaveData
{
    public string codeName;
    public QuestState state;
    public int npcID;
    public int questListInfoIndex;
    public int questContainerIndex;
    public int taskGroupIndex;
    public int[] taskSuccessCounts;
    public int repeatCount;
    public int session;

}
