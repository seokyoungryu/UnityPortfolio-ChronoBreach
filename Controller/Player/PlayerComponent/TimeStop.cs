using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    [SerializeField] private float attackStopTime = 0.05f;
    [SerializeField] private float skillStopTime = 0.05f;

   private bool isStop = false;
   private float timer = 0f;
   private float stopTime = 0f;

    void Update()
    {
      // if (isStop)
      // {
      //     timer += Time.unscaledDeltaTime;
      //     if(timer >= stopTime)
      //     {
      //         Time.timeScale = 1f;
      //         timer = 0f;
      //         stopTime = 0f;
      //         isStop = false;
      //     }
      // }
    }


    public void StopTime()
    {
      //  Time.timeScale = 0f;
      //  timer = 0f;
      //  isStop = true;
    }

    public void StopAttackTime()
    {
        stopTime = attackStopTime;
        StopTime();
    }

    public void StopSkillTime()
    {
        stopTime = skillStopTime;
        StopTime();
    }

    public void StopAttackTime(float stopTime)
    {
        this.stopTime = stopTime;
        StopTime();
    }
}
