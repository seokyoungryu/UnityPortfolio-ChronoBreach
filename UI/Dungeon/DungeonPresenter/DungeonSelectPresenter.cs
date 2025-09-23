using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSelectPresenter : MonoBehaviour
{
    [SerializeField] private DungeonPortalUI dungeonPortalUI = null;
    [SerializeField] private DungeonChapterSelectUI dungeonChapterSelectUI = null;
    [SerializeField] private DungeonDetailInfoUI dungeonDetailUI = null;


    private void Awake()
    {
        dungeonChapterSelectUI.onExcuteBack += () => DungeonActive();
        dungeonChapterSelectUI.OnSelectTask_ += SettingChapterTaskSelected;
        dungeonPortalUI.OnSelectChapter_ += OpenChapterSelectUI;
        dungeonPortalUI.OnReceiveChapter_ += dungeonChapterSelectUI.SetCurrentChapter;
        dungeonPortalUI.OnUpdateDetail_ += dungeonDetailUI.SettingInfos;

    }

    private void OnDestroy()
    {
        dungeonChapterSelectUI.onExcuteBack -= () => DungeonActive();
        dungeonChapterSelectUI.OnSelectTask_ -= SettingChapterTaskSelected;
        dungeonPortalUI.OnSelectChapter_ -= OpenChapterSelectUI;
        dungeonPortalUI.OnReceiveChapter_ -= dungeonChapterSelectUI.SetCurrentChapter;
        dungeonPortalUI.OnUpdateDetail_ -= dungeonDetailUI.SettingInfos;
    }


    public void DungeonActive()
    {
        dungeonPortalUI.gameObject.SetActive(true);
    }

    public void OpenChapterSelectUI(DungeonEntryDatabase database )
    {
        dungeonChapterSelectUI.OpenChapterSelect(database);
    }

    public void SettingChapterTaskSelected(List<DungeonChapeterTask> tasks)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].onClickTask += dungeonPortalUI.OpenChapter;
            tasks[i].onClickTask += delegate { dungeonChapterSelectUI.CloseUIWindow(); };
        }
    }
}
