using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum DialogState
{
    START = 0,
    END = 1,
    PROGRESS = 2,
    INTERACT =3,
    ETC = 4,
}

[CreateAssetMenu(menuName ="CreateDialog", fileName ="Dialog_")]
public class DialogFile : ScriptableObject
{
    public TextAsset dialogCSV = null;
    public List<DialogData> dialogDatas = new List<DialogData>();


    public DialogData[] GetHaveDialogDatasState(DialogState findState)
    {
        List<DialogData> dialogs = new List<DialogData>();

        for (int i = 0; i < dialogDatas.Count; i++)
        {
            for (int j = 0; j < dialogDatas[i].dialog.Count; j++)
            {
                if (dialogDatas[i].dialog[j].dialogStateType == findState)
                    dialogs.Add(dialogDatas[i]);
            }
        }

        return dialogs.ToArray();
    }

    [ContextMenu("Dialog 리셋")]
    public void ResetDialogData()
    {
        dialogDatas.Clear();
    }

    [ContextMenu("모든 다이어로그 OneByOne 체크하기")]
    public void SetAllDialogOneByOne()
    {
        for (int i = 0; i < dialogDatas.Count; i++)
        {
            dialogDatas[i].isOneByOnePrint = true;
        }
    }

    [ContextMenu("다이아로그 생성")]
    public void CreateDialog()
    {
        dialogDatas.Clear();

        string csvToString = dialogCSV.text;
        string[] enterString = csvToString.Split('\n');
        DialogData tmpDialog = new DialogData();
        string currentName = string.Empty;
        DialogState state = DialogState.START;

        for (int i = 1; i < enterString.Length - 1; i++)
        {
            string[] tap = enterString[i].Split(',');
            if (tap.Length <= 0) return;

            DialogEntity entity = new DialogEntity();

            if (i != 1 && tap[0].Trim() != "") //새로운 다이어로그 생성
            {
                dialogDatas.Add(tmpDialog);
                tmpDialog = new DialogData();
                if (tap[2].Trim().ToLower() == DialogState.START.ToString().ToLower())
                    state = DialogState.START;
            }

            if (tap[2].Trim().ToLower() == DialogState.END.ToString().ToLower())
                state = DialogState.END;
            else if (tap[2].Trim().ToLower() == DialogState.PROGRESS.ToString().ToLower())
                state = DialogState.PROGRESS;
            else if (tap[2].Trim().ToLower() == DialogState.INTERACT.ToString().ToLower())
                state = DialogState.INTERACT;
            else if (tap[2].Trim().ToLower() == DialogState.ETC.ToString().ToLower())
                state = DialogState.ETC;

            if (tap[0].Trim() != "")
                tmpDialog.id = int.Parse(tap[0]);
            if (tap[1].Trim() != "")
                tmpDialog.title = tap[1];


            if (tap[3].Trim() != "")
                currentName = tap[3];

            entity.name = currentName;
            entity.dialog = tap[4];


            if (tmpDialog.GetDialogContainer(state) == null)
                tmpDialog.AddDialogState(state);
            tmpDialog.GetDialogContainer(state).dialog.Add(entity);

            if (i == enterString.Length - 2)
                dialogDatas.Add(tmpDialog);
        }


    }


    public DialogData GetDialogData(int id)
    {
        for (int i = 0; i < dialogDatas.Count; i++)
        {
            if (dialogDatas[i].id == id)
                return dialogDatas[i];
        }

        return null;
    }


    private void OnValidate()
    {
        if(dialogDatas != null)
        {

        }
    }
}
