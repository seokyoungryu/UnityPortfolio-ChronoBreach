using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseSpawnInfos
{
    //던전의 한곳의 생성 정보
    //몬스터 OBP, 위치, isDead 등 여러 변수들, 
    [SerializeField] protected string infoName = string.Empty;
    [SerializeField] protected SpawnProcessType currentProcess = SpawnProcessType.WAIT;
    [SerializeField] protected float waitNextRound = 1f; //라운드 완료후 다음 라운드 시작 대기시간
    [SerializeField] protected string globalNotifier = string.Empty;

    public string InfoName { get { return infoName; } set { infoName = value; } }
    public SpawnProcessType CurrentProcess { get { return currentProcess; } set { currentProcess = value; } }
    public float WaitNextRound => waitNextRound;
    public string GlobalNotifier { get { return globalNotifier; } set { globalNotifier = value; } }

}

public enum SpawnProcessType
{
    NONE = -1,
    WAIT = 0,
    RUNNING = 1,
    COMPLETE = 2,
}

