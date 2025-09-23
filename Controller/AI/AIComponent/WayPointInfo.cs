using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WayPointInfo
{
    [SerializeField] private int wayPointIndex;
    [SerializeField] private float thisPointWaitTime = 0f;
    private Transform wayPointTr = null;  //이거 private으로 하기.


    public int WayIndex => wayPointIndex;
    public float ThisPointWaitTime => thisPointWaitTime;
    public Transform WayPointTr { get { return wayPointTr; } set { wayPointTr = value; } }
}
