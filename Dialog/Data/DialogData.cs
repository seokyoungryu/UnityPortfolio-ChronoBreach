using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class DialogData
{
    public string title = string.Empty;
    public int id = 0;
    public bool isOneByOnePrint = false;
    public List<DialogContainer> dialog = new List<DialogContainer>();

    public void AddDialogState(DialogState state)
    {
        for (int i = 0; i < dialog.Count; i++)
            if (dialog[i].dialogStateType == state)
                return;

        dialog.Add(new DialogContainer(state));
    }
    public DialogContainer GetDialogContainer(DialogState state)
    {
        for (int i = 0; i < dialog.Count; i++)
            if (dialog[i].dialogStateType == state)
                return dialog[i];

        return null;
    }
}


[System.Serializable]
public class DialogEntity
{
    public string name = string.Empty;
    public string dialog = string.Empty;
}

[System.Serializable]
public class DialogContainer
{
    public DialogState dialogStateType = DialogState.START;
    public List<DialogEntity> dialog = new List<DialogEntity>();

    public DialogContainer() { }
    public DialogContainer(DialogState state)
    {
        dialogStateType = state;
        dialog = new List<DialogEntity>();
    }
    public DialogContainer(DialogState state, List<DialogEntity> entity)
    {
        dialogStateType = state;
        dialog = entity;
    }

}


