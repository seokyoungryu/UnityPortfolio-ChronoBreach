using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/Item Target", fileName = "TargetItem_")]
public class ItemTaskTarget : TaskTarget
{
    [SerializeField] private ItemList itemID = ItemList.None;

    public override object Value => itemID;
    public ItemList ItemList => itemID;

    public override bool IsEqual(object target)
    {
        if (!(target is int) && !(target is ItemList)) return false;

        ItemList targetItem = (ItemList)target;
        if (targetItem == ItemList.None) return false;

        if (targetItem == itemID)
            return true;
        return false;
    }
}
