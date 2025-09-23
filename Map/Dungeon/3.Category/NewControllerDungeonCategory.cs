using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewControllerDungeonCategory : BaseDungeonCateogry
{
    //변수로 ScriptableObject를 받는데, 내용은 다른 컨트롤러가 있는 프리팹을 가지고있음.
    //예시로 레이싱 자동차 종류 , 로봇 종류 등. 
    //이것도 So인데 변수로 SO를 받는 이유는.  전체 오브젝트 누르면 게임프리팹은 엄청 많이나오는데 저렇게 SO로 생성하면 쉽게 서택가능.

    public override PlayerStateController InitControllerSetting(BaseDungeonTitle title)
    {
        //Init시 -> 새로운 오브젝트 생성. // 근데 이 오브젝트를 접근어캐함.?
        //즉 모든 새 컨트롤러도 playerStatecontroller이며, 상태는 state로
        return null; // 생성후 여기에 저장.
    }
}


//타이틀
// -> 원본 PlayerStatecontroller와, 실제 게임에 사용할 ExcuteController 가지고있음.
// 나머지는 원본 컨트롤러 소유? 소유할 필요는 없을듯.