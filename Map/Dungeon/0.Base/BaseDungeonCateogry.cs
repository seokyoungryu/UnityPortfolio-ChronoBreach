using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDungeonCateogry : ScriptableObject
{
    public abstract PlayerStateController InitControllerSetting(BaseDungeonTitle title);

    //음 레이싱이나 로봇같은 다른 종류의 controller 같은 경우는..
    //Manager?를 통해서? 음. 일단 레이싱 같은 경우 -> 각자 생성하기. 왜냐면 공격하는 것도있기떄문.
    //Category를 음.. 생성하기. 예를들어  레이싱이면  (해당 자동차의 기능을 가진 프리팹을 해당 위치로 생성. -> 즉 생성만하면 wasd로 조종가능하게함)
    //로봇같은 경우도 일단 생성만 한다면 해당 프리팹에 등록된 기능 사용할수있게한다.
    //또는 SelectInGameCategory -> 이건 게임 안에 들어가서 세팅 UI를 작동시킴. 
    // 기능에서 InitControllerSetting()를 실행하면 해당 장르의 캐릭터를 선택하거나 세팅하는 UI를 실행하는거임
    // 세팅하는 UI에서 선택 버튼을 누르면 해당 선택한 프리팹이 지정위치에 생성되며 -> 기능 실행하는식.

    //즉 정리하자면, 다른 컨트롤러 일 경우 준비물 -> 다른 컨트롤러의 프리팹을 만듬. (생성시 기능(조종) 가능해야함)
    //1. 게임 실행후 변수의  모델 생성
    //2. 캐릭터 select UI를 작동. 다만 InitControllerSetting에서 선택가능한 캐릭터들 세팅할수 있어야함.
    //    ex) 1-2 레이싱에서는 자동차 3개만 사용가능한데 , 2-4에서는 5개 선택 가능하는식. 
    // 음. 선택가능UI에서 -> 종류, 업그레이드? 음 이런거이긴한데.
    // 그냥 선택가능UI는 컨텐츠에서 사용하기. -> 레이싱 컨텐츠 -> 스토리 상태에 따라서 해금가능한 자동차가 달라짐. 실행시 선택창이 나오면서 선택가능.
    // 스토리는 보통 지정된 차만 사용하기.
}
