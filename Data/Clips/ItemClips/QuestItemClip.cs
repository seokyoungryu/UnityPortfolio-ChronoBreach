using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItemClip : BaseItemClip
{

    public QuestItemClip() { }
    public QuestItemClip(int index, string name, int enumNum) : base(index, name, enumNum) { }
    public QuestItemClip(BaseItemClip clip) : base(clip) { }

}
