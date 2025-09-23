using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFailScoreUI : DungeonScoreUI
{
    [SerializeField] private RectTransform failPanel = null;

    public override void ExcuteScoreUI(BaseDungeonTitle dungeonTitle)
    {
        if (MapManager.Instance.CurrentScoreUIType != ScoreUIType.NOT_EXCUTE)
            return;

        currDungeonTitle = dungeonTitle;
        StopAllCoroutines();
        StartCoroutine(Process_Co());
    }

    protected override void AllActive(bool active)
    {
        base.AllActive(active);
        failPanel.gameObject.SetActive(active);
    }


    private IEnumerator Process_Co()
    {
        Debug.Log("Fail 점수창 Process_Co 실행!");
        GetComponent<RectTransform>().anchoredPosition = excutePosition;
        failPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlayExtraSound(UISoundType.FAIL_SCORE_ENTER);
        failPanel.gameObject.SetActive(true);
    }

    public void Reply_Btn()
    {
        AllActive(false);
        MapManager.Instance.ExcuteReply();

    }

}
