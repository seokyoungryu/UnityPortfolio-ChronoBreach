# 🎮 Unity3D Portfolio RPG Game - Chrono Breach


## 📚 목차
- [UML 클래스 다이어그램](#-uml-클래스-다이어그램)
- [동영상 링크](#-동영상-링크)
- [정보](#-정보)
- [핵심 기술 파트](#-핵심-기술-파트)
  - [Dungeon System](#-dungeon-system)
  - [Dash System](#-dash-system)
- [트러블 슈팅](#-트러블-슈팅)


# ⚙️ UML 클래스 다이어그램
프로젝트의 주요 시스템 구조를 나타내는 UML 다이어그램입니다.
<img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/UML_F4.drawio.png" alt="UML Diagram" width="1000" />



 ### [🧩 **UML 클래스 다이어그램 열기**](https://app.diagrams.net/?url=https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/refs/heads/main/UML_F4.drawio) (ctrl + wheel로 줌 아웃)

# 📹 동영상 링크
**동영상 화질을 4k로 선택하여 시청해주시면 감사합니다.** 
<a href="https://www.youtube.com/watch?v=sTdEx9n8rMI" target="_blank">
  <img src="https://img.youtube.com/vi/sTdEx9n8rMI/maxresdefault.jpg" alt="Unity Portfolio (4K)" style="width:100%;">
</a>

### [🎬 **유튜브 영상 바로 보기**](https://www.youtube.com/watch?v=sTdEx9n8rMI)


---

# 🛠️ 정보

- **Unity Version**: 2021.3.17f1
- **제작 기간**: 1년 2개월
- **게임 장르**: 3D Action RPG
- **타겟 플렛폼**: PC
- **이메일**: whtkrl@gmail.com
- **제작인원**: 1명
---

# ⏳ 핵심 기술 파트

## 📌 Dungeon System
**모듈형 확장 구조의 유연한 던전 UML**🎯
<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/UML_D1.png" width="650"/> </p>


## 🎯 설계 목적
초기에는 단순히 모든 적 처치 시 클리어되는 구조만 구현하려 했으나, 던전별로 고유한 목표와 규칙을 제공하면 게임 플레이의 깊이와 다양성이 크게 향상된다고 판단하여 현재의 모듈형·확장형 구조로 발전시켰습니다.  

설계 과정에서는 다음 두 가지를 특히 중점적으로 고려했습니다.  

- **유지보수성**: 던전별 로직 분리 및 독립 관리  
- **확장성**: 데이터만 교체해 다양한 던전 유형 추가 가능  

---

## 🎯 Dungeon 구성 요소 
아래 구성들은 던전 시스템의 핵심 데이터를 간결하게 표현한 구조입니다.  

## Title
- 던전의 이름, 유형, 설명 등  
- 던전을 식별하고 UI 및 시스템에서 활용되는 기본 정보  
- **( Normal, Rush, Protect, Rescuer, Target )**
<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Title.png" width="650"/> </p>

  
## Category
- 사용하는 Controller 설정.
```csharp
[CreateAssetMenu(menuName ="Map/Dungeon Category/Normal ", fileName ="NormalDungeonCategory")]
public class NormalDungeonCategory : BaseDungeonCateogry
{
    public override PlayerStateController InitControllerSetting(BaseDungeonTitle title)
    {
        PlayerStateController originController = title.OriginController;
        originController.allowStates.Clear();
        originController.allowStates.Add(originController.GetState<MoveState>());
        originController.allowStates.Add(originController.GetState<AttackState>());
        originController.allowStates.Add(originController.GetState<RollState>());
        originController.allowStates.Add(originController.GetState<SkillState>());
        originController.allowStates.Add(originController.GetState<DamagedState>());
        originController.allowStates.Add(originController.GetState<CounterAttackState>());
        originController.allowStates.Add(originController.GetState<DeadState>());
        originController.allowStates.Add(originController.GetState<DashState>());
        return originController;
    }
}
```
- Normal Category에서는 플레이어 컨트롤러가 사용 가능한 State를 세팅합니다.

## Condition
- 던전에 입장하기 위한 조건을 명시  
  - 플레이어 레벨  
  - 필요 아이템  
  - 특정 스테이지 선행 여부  

## Function
- 던전 진행 전체를 담당하는 핵심 프로세스  
  - 라운드 시작  
  - 몬스터/보스 스폰  
  - 클리어 조건 체크  
  - 종료 및 보상 처리  

```csharp
[CreateAssetMenu(menuName = "Map/Dungeon Function/Normal Function ", fileName = "NormalFunction")]
public class NormalDungeonFunction : BaseDungeonFunction<NormalDungeonTitle>
{
    public override void ExcuteProcess(NormalDungeonTitle title)
    {
        SoundManager.Instance.PlayBGM_CrossFade(title.BaseBGM, 4f);
        title.SpawnData.dungeon = title.dungeonCoroutine;
        title.DungeonMapData.ExcuteTeleportMap();

        title.SpawnData.onExcuteBoss += () => { SoundManager.Instance.PlayBGM_CrossFade(title.BossBGM, 3f); };
        ScenesManager.Instance.OnExcuteAfterLoading = () => title.DungeonMapData.ExcuteTeleportController(title.ExcuteController, title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.SettingSpawnPositionList(title.DungeonSpawnPosition);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.SetTarget(title.ExcuteController.gameObject);
        ScenesManager.Instance.OnExcuteAfterLoading += () => GameManager.Instance.Cam.ResetRotation();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.StartWave();
        ScenesManager.Instance.OnExcuteAfterLoading += () => title.SpawnData.CreateExistBarrier();
        ScenesManager.Instance.OnExcuteAfterLoading += () => CommonUIManager.Instance.ExcuteGlobalNotifer(title.InitGlobalNotifier);

        title.SpawnData.onCompleteDungeon += () => QuestManager.Instance.ReceiveReport(QuestCategoryDefines.COMPLETE_DUNGEON, title.TaskTarget, 1);
        GameManager.Instance.Player.playerStats.OnDead_ += () => title?.SpawnData?.ExcuteFailProcess();
    }
}

```

## Reward
- 클리어 보상 정보  
  - 경험치
  - 명성치
  - 스킬포인트
  - 아이템  
  - 골드  
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/R1.png" width="700" style="display:inline-block;"/>
  
## MapData
- 던전에 사용되는 던전 Scene index와 위치, 회전 등의 맵 정보.

## SpawnPosition
- 던전 내 위치 정보 구성 요소  
  - 플레이어 시작 위치  
  - Enemy/Boss 스폰 지점  
  - 트리거 이벤트 위치  

  <p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/화면 캡처1.png" width="700" style="display:inline-block;"/>
-OnValidate() 합수로 밑에있는 Map Position Prefab의 정보를 자동 세팅합니다.

<div align="center">
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/화면 캡처2.png" width="300" style="display:inline-block;"/>
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/화면 캡처3.png" width="300" style="display:inline-block;"/>
</div>
- OnDrawGizmos() 함수로 Root Transform의 child Transform의 위치들을 시각적으로 표시하며 positions 리스트에 자동 추가합니다.

## SpawnData 
- 전투 구성에 필요한 모든 스폰 및 라운드 데이터  
  - Enemy/Boss 스폰 리스트
  - Playable AI 리스트
  - 이동 불가 벽 정보
  - 웨이브/라운드 구성   
- **( Normal, Rush, Protect, Rescuer, Target, TimeAttackRush )**
<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/SpawnD1.png" width="650"/> </p>



## ⚡ Dash System
<p align="center">  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Dash.gif" width="400" style="display:inline-block;"/>
 
- 고속 타격 기반의 지형·적 감지형 대시 시스템 

대시 시스템은 단순한 돌진이 아니라 지형, 적, 장애물, 카메라, 쿨타임 UI로 구성된 전투 시스템으로 설계되었습니다.

아래 두 가지 목표를 중심으로 구현되었습니다.
- **정확성 : 안전하게 이동 가능한 지점만 계산하여 오동작을 최소화**
- **전술성 : 적·지면·장애물 판정을 조합해 전략적으로 대시를 활용 가능**

## ⭐ Dash 설계 핵심 요소
<div align="center">
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/G1.gif" width="450" style="display:inline-block;"/>
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/G2.gif" width="450" style="display:inline-block;"/>
</div>

- 대시는 아래와 같은 구조로 실행됩니다.

## Target Detect
- 대상 위치 계산
  - BaseController 여부에 따라 피격 중심점 또는 Transform 위치를 타깃 좌표로 사용합니다.
- 지면 검출 시 대시 불가
  - CheckDetectGround(targetDir, distance) 에서 지면이 감지되면 해당 위치는 대시 대상에서 제외됩니다.
- 장애물 검출 시 대시 불가
  - CheckDetectObstacle(targetDir, distance) 결과 장애물이 차단하고 있으면 대시할 수 없습니다.
- 근거리 대상 정밀 체크
  - 타깃이 targetingAllowDistance 이내라면, 더 좁은 중앙 스크린 영역(targetingLimitScreenPoint) 안에 있을 때만 대시를 허용합니다.
- 일반 타깃팅 영역 체크
  - 근거리 조건을 충족하지 못하더라도, 넓은 기준 스크린 영역(limitDistance) 안에 위치하면 대시 가능 대상으로 인정합니다.
- 두 조건 모두 벗어나면 대시 불가
  - 스크린 기준점을 벗어나거나 거리 조건을 만족하지 못할 경우 대시는 허용되지 않습니다.
```csharp
  private bool CheckCanDashTarget(Transform targetTr)
    {
        BaseController targetCon = targetTr.GetComponent<BaseController>();
        Vector3 targetPos = targetCon != null ? targetCon.damagedPosition.position : targetTr.position;
        Vector2 point = cam.MainCam.WorldToScreenPoint(targetPos);
        targetDir = (targetPos - centerPosition).normalized;
        float distance = (targetPos - (controller.transform.position + (Vector3)centerOffset)).magnitude;

        gizmoObstacleDir = targetDir;
        gizmoObstacleDistance = distance;

        //땅일경우
        if (CheckDetectGround(targetDir, distance))
        {
            return false;
        }

        ///타겟 위치에 레이어 쏴서 장애물 있나 판단.
        if (CheckDetectObstacle(targetDir, distance))
        {
            return false;
        }

        //타겟팅일 경우 
        if (distance <= targetingAllowDistance)
        {
            if (centerScreenPoint.x + targetingLimitScreenPoint.x >= point.x && centerScreenPoint.x - targetingLimitScreenPoint.x <= point.x &&
            centerScreenPoint.y + targetingLimitScreenPoint.y >= point.y && centerScreenPoint.y - targetingLimitScreenPoint.y <= point.y)
                return true;
        }

        if (centerScreenPoint.x + limitDistance.x >= point.x && centerScreenPoint.x - limitDistance.x <= point.x &&
            centerScreenPoint.y + limitDistance.y >= point.y && centerScreenPoint.y - limitDistance.y <= point.y)
        {
            return true;
        }

        return false;
    }

```

## Ground Check
- 대시 가능한 지점을 찾기 위해 목표점까지의 수평 이동 거리를 기반으로 일정 간격으로 지면을 샘플링합니다.
- 작동 방식
  - 플레이어 → 타깃 방향으로 일정 Interval만큼 전진
  - 각 시점에서 아래로 SphereCast
  - 적이 있는 위치면 Skip
  - 최초로 안전한 지면을 찾으면 그 위치로 이동 확정

🔑 핵심 코드
```csharp
private bool CheckCanDashGround()
{
    sumInterval = 0f;
    targetDirFromDashPos = (dashTargetTr.position - tmpDashPosition);
    groundDistance = targetDirFromDashPos.magnitude;
    groundSumCount = (int)((groundDistance - minDetectTargetDistance) / groundDetectInterval);
    currentTargetHeight = (dashTargetTr.position - controller.transform.position).y;

    if (currentTargetHeight < minDetectHeight || currentTargetHeight > maxDetectHeight)
        return false;

    targetDirFromDashPos.y = 0f;
    targetDirFromDashPos.Normalize();

    for (int i = 0; i < groundSumCount; i++)
    {
        startPos = tmpDashPosition + targetDirFromDashPos * sumInterval + Vector3.up * groundStartYOffset;
        sumInterval += groundDetectInterval;

        if (DetectEnemy(startPos))
            continue;

        if (Physics.SphereCast(startPos, groundDetectRadius, -Vector3.up,out groundCheckRayHit, groundYRange, groundLayer))
        {
            canDashPosition = groundCheckRayHit.point;
            return true;
        }
        else
            canDashPosition = Vector3.zero;
    }
    return false;
}
```

## Enemy Detection
- 대시 경로에 적이 있는지 검사해 충돌 감지 시 Skip 또는 Hit 처리합니다.

🔑 핵심 코드
```csharp
private bool DetectEnemy(Vector3 startPosition)
{
    RaycastHit groundHit;

    // 1) 지면이 없는 경우 - 안전
    if (!Physics.Raycast(startPosition, -Vector3.up, out groundHit, groundYRange, groundLayer))
        return true;

    // 2) 지면까지 SphereCast 시 적 감지
    if (Physics.SphereCast(startPosition, groundDetectRadius, -Vector3.up,
        out groundCheckRayHit, groundHit.point.y, enemyLayer))
    {
        drawEnemyHitPoints.Add(groundCheckRayHit.point);
        return true;
    }

    // 3) 시작 지점에 적이 있는지 검사
    if (Physics.OverlapSphereNonAlloc(startPosition, groundDetectRadius,
        groundEnemyColls, enemyLayer) > 0)
    {
        drawEnemyHitPoints.Add(startPosition);
        return true;
    }

    return false;
}
```

## Obstacle Check
<p align="center">  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Ground.gif" width="400" style="display:inline-block;"/>
 
- 대시 경로에 장애물이 존재하는지 사전 검출합니다.
  - OverlapSphereNonAlloc 기반 충돌 예측 및 최적화
- 장애물과 충돌하면 Target 자동 변경 또는 대시 취소

## Dash Movement + Camera + UI
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/UI1.gif" width="400" style="display:inline-block;"/>

- 타깃 UI로 현재 선택된 대상 시각화 
- 대시 이동이 허용되면 다음 처리가 이루어집니다.
  - 카메라 FOV 변화
  - 원거리/근거리 대시별 SmoothSpeed 자동 조절
  - 성공 카운트 UI 업데이트
  - 대시 스택 기반 쿨타임 회복

```csharp
 private IEnumerator DashMoveProcess_Co()
    {
        if (dashTargetTr == null)
            yield break;
        float endTime = dashClip.EndTime;

        if (CanTeleportToTarget(dashTargetTr))
        {
            StartCoroutine(DashCameraProcess_Co(false));
            yield return new WaitForSeconds(farDelayFOVMoveTime);
            controller.RotateToTarget(dashTargetTr.position);
            controller.myAnimator.CrossFade(dashClip.AnimationName, 0.1f);
            controller.TranslatePosition(canDashPosition);
            controller.StartCoroutine(DashDamageProcess_Co());
        }
        else
        {
            StartCoroutine(DashCameraProcess_Co(true));
            yield return new WaitForSeconds(nearDelayFOVMoveTime);
            controller.RotateToTarget(dashTargetTr.position);
            controller.myAnimator.CrossFade(dashClip.AnimationName, 0.1f);
            controller.StartCoroutine(DashDamageProcess_Co());
        }

        yield return new WaitForSeconds(dashClip.EndTime);
        doneDashState = true;
        dashTargetTr = null;
    }
 

```


# ⏳ 트러블 슈팅
## 🎨 메테리얼 최적화 과정
- NPC나 몬스터를 생성할 때, 캐릭터별로 지정된 색상을 적용하기 위해 메테리얼 컬러 값을 변경하는 기능을 구현하고 있었습니다.
- 즉, 스폰된 캐릭터마다 고유한 색상을 설정하는 과정에서 자연스럽게 메테리얼을 수정하는 로직이 필요했습니다.

## ⚠ 문제 발생
- 메테리얼의 색상을 변경하는 과정에서 기존 메테리얼을 직접 수정하는 것이 아니라, Unity가 내부적으로 새로운 메테리얼 인스턴스를 생성하여 변경을 적용하고 있다는 사실을 확인했습니다.

즉, **공유 메테리얼(Shared Material)** 을 수정하는 것이 아니라, Renderer마다 **고유한 메테리얼 인스턴스(Material Instance)** 를 새로 생성해 적용하는 구조였습니다.

- 이로 인해 NPC나 몬스터가 많아질수록 고유 인스턴스가 기하급수적으로 늘어났고, 그만큼 드로우콜 증가 → 배칭이 깨짐 → 퍼포먼스 저하가 발생했습니다.

## 🔍 원인 분석
- Unity의 메테리얼 구조상 renderer.material을 수정하면 기존 메테리얼은 공유된 상태

수정 순간 Renderer마다 고유한 인스턴스(Material Instance)를 생성
하게 됩니다.

즉, 색상 하나만 바뀌어도 전부 다른 메테리얼로 인식되기 때문에
Static/Dynamic Batching이 적용되지 않고 Draw Call이 불필요하게 확대되는 것이 원인이었습니다.

## ✅ 해결 방법 

- 이를 해결하기 위해 **Material Property Block** 방식을 적용하였습니다.
- Material Property Block는 기존 공유 메테리얼을 유지한 채, Renderer별 색상·파라미터만 개별적으로 변경할 수 있는 기능을 제공합니다.

이로인해 메테리얼 인스턴스를 새로 생성하지 않고, 드로우콜을 증가시키지 않으며, 커스터마이징 색상 적용을 그대로 유지할 수 있었습니다.

```csharp
 public void SetMaterialsColor(Renderer renderer)
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

        propertyBlock.SetColor("_Color_Primary", Color_Primary);
        propertyBlock.SetColor("_Color_Secondary", Color_Secondary);
        propertyBlock.SetColor("_Color_Leather_Primary", Color_Leather_Primary);
        propertyBlock.SetColor("_Color_Metal_Primary", Color_Metal_Primary);
        propertyBlock.SetColor("_Color_Leather_Secondary", Color_Leather_Secondary);
        propertyBlock.SetColor("_Color_Metal_Dark", Color_Metal_Dark);
        propertyBlock.SetColor("_Color_Metal_Secondary", Color_MertalSecondary);
        propertyBlock.SetColor("_Color_Hair", Color_Hair);
        propertyBlock.SetColor("_Color_Skin", Color_Skin);
        propertyBlock.SetColor("_Color_Stubble", Color_Stubble);
        propertyBlock.SetColor("_Color_Scar", Color_Scar);
        propertyBlock.SetColor("_Color_BodyArt", Color_BodyArt);
        propertyBlock.SetColor("_Color_Eyes", Color_Eyes);

        renderer.SetPropertyBlock(propertyBlock);
    }
```



## 🎨Layout group 사용 중 발생한 성능 문제  
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/L1.png" width="500" style="display:inline-block;"/>

- 초기에는 Inventory, 상점 UI, Reward UI 등 다수의 UI 요소가 표시되는 화면에 Unity가 기본 제공하는 Layout Group을 사용하고 있었습니다.
  레이아웃 정렬이 자동으로 이루어져 UI 구성은 편리했지만, 실제 플레이 환경에서는 예상치 못한 성능 저하가 발생했습니다. 


## ⚠ 문제 발견

- UI 요소가 많아질수록 화면 전환 및 스크롤 상황에서 프레임 저하가 눈에 띄게 증가
- Inventory나 상점처럼 자식 UI가 많은 패널에서 Canvas Rebuild가 반복적으로 발생
- 즉, 불필요한 작업이 발생.

  
## ✅ 해결 방법
- Unity 기본 Layout Group 사용을 중단하고, 레이아웃을 필요할 때만 **단일 호출**로 갱신하는 구조로 재설계하였습니다.
- UI 변동 여부에 따라 콘텐츠 크기를 자동 재조정할 수 있도록 **Content Size Filter** 기능을 선택적 옵션으로 제공해 유연하게 활용할 수 있도록 만들었습니다.

BaseLayoutGroup을 부모로, Grid / Horizontal / Vertical의 기능을 만들었습니다.


**1) AnchorSetting()**
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/L2_1.png" width="500" style="display:inline-block;"/>
- 우선 AnchorSettings() 함수를 통해 정렬할 RectTrasnform들의 Anchor위치를 왼쪽 상단으로 세팅합니다.

```csharp
 protected void AnchorSettings()
    {
        if (uiRect == null)
            uiRect = GetComponent<RectTransform>();
        else if (uiRect != null)
            uiRect.pivot = new Vector2(0, 1);

        foreach (RectTransform rect in childRects)
        {
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
        }
    }
```


**1) SortLayout() — UI 배치 계산의 핵심 메서드**
- 아래 메서드는 UI 요소를 Row 또는 Column 기준으로 배치하며, 패딩·간격·슬롯 크기 계산 후 Anchor 기준 위치를 직접 산출합니다.

```csharp
 protected override void SortLayout(List<RectTransform> rectList)
    {
        if (rectList.Count <= 0) 
        {
            uiRect.sizeDelta = new Vector2(0,0);
            return;
        }
        float paddingLR = paddings.left > 0 ? paddings.left : -paddings.right;
        float rectWidth = 0f;
        float rectHeight = 0f + paddings.top;
        int index = 0;

        foreach (RectTransform rect in rectList)
        {
            float paddingBottom = paddings.bottom > 0 ? paddings.bottom : 0;

            if (IsRow())
                RowCalculate(index, ref rectWidth, ref rectHeight, rect);
            else
                ColumeCalculate(index, ref rectWidth, ref rectHeight, rect);

            rect.anchoredPosition = new Vector2(rectWidth + paddingLR, -rectHeight + paddingBottom);
            index++;
        }

        ContentSizeFilter(index, rectWidth, rectHeight);
    }

```

**2) ContentSizeFilter() — UI 콘텐츠 크기 자동 조정 기능**
- UI 항목이 동적으로 증가하는 경우, 전체 슬롯 영역이 UI 내부에 자연스럽게 확장되도록 Row 또는 Column 기준으로 RectTransform의 sizeDelta를 자동 산출합니다.

```csharp
   private void ContentSizeFilter(int index, float rectWidth, float rectHeight)
    {
        if (useContextSizeFilter && uiRect != null)
        {
            if (IsRow())
            {
                float uiRectWidth = childRects[0].rect.width * RoundToInt(index, row);
                float uiRectHeight = childRects[0].rect.height * (row > index ? index : row);
                uiRect.sizeDelta = new Vector2(rectWidth + childRects[0].rect.width, uiRectHeight);
            }
            else
            {
                float uirectWidth = childRects[0].rect.width * (colume > index ? index : colume);
                float uirectHeight = childRects[0].rect.height * RoundToInt(index, colume);
                uiRect.sizeDelta = new Vector2(uirectWidth, rectHeight + childRects[0].rect.height);
            }
        }
    }

```


---
