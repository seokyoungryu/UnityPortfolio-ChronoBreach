using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map/Dungeon Spawn Data/Time Rush Spawn Data", fileName = "TimeRushSpawnData_")]
public class TimeRushSpawnData : NormalRushSpawnData
{
    [Header("버터야하는 시간")]
    [SerializeField] private float holdOutTime = 0f;
    private float currentTimer = 0f;



    protected override bool CheckDungeonComplete()
    {
        return false;
    }

    public override void StartRush()
    {
        base.StartRush();
        printEnemyCount = false;
        dungeon.StartCoroutine(TimeCheck());
    }


    private IEnumerator TimeCheck()
    {
        Debug.Log("<color=yellow> Time Check 시작</color>");

        currentTimer = holdOutTime;
        MapManager.Instance.DungeonNotifierUI.SetText("남은 시간 " + GetTimerTranslateText(currentTimer));

        while (!isCompleteDungeon)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0f)
                currentTimer = 0f;

            MapManager.Instance.DungeonNotifierUI.SetText("남은 시간 " + GetTimerTranslateText(currentTimer));
            Debug.Log("<color=yellow> Time Check 중..</color>");
            yield return null;

            if (currentTimer <= 0f )
                ExcuteEndProcess();
        }

    }


    private string GetTimerTranslateText(float time)
    {
        // 500초 : 
        int minute = (int)(time / 60f);

        return minute + ":" + (time % 60).ToString("00");

    }

}
