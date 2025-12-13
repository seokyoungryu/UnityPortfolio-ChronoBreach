# 🎮 Unity3D Portfolio RPG Game - Chrono Breach


## 📚 목차
- [UML 클래스 다이어그램](#-uml-클래스-다이어그램)
- [동영상 링크](#-동영상-링크)
- [정보](#-정보)
- [핵심 기술 파트](#-핵심-기술-파트)
  - [Dungeon System](#-dungeon-system)
  - [Dash System](#-dash-system)
  - [Item Editor Tool](#-Item-Editor-Tool)
  - [잠재능력 Editor Tool](#-잠재능력-Editor-Tool)
  - [콘솔(커맨드) 창](#console-command)
    
- [트러블 슈팅](#-트러블-슈팅)
  - [메테리얼 최적화 과정](#-메테리얼-최적화-과정)
  - [Layout group 성능 문제](#-Layout-group-성능-문제)
  - [UI 스크롤 문제](#-UI-스크롤-문제)
  - [Mesh Combine Tool 발생한 빌드 속도 저하](#-Mesh-Combine-Tool-발생한-빌드-속도-저하)
    
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
<br><br>

## 📌 Dungeon System
**모듈형 확장 구조의 유연한 던전 UML**🎯
<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/UML_D1.png" width="650"/> </p>
<br><br>

## 🎯 설계 목적
초기에는 단순히 모든 적 처치 시 클리어되는 구조만 구현하려 했으나, 던전별로 고유한 목표와 규칙을 제공하면 게임 플레이의 깊이와 다양성이 크게 향상된다고 판단하여 현재의 모듈형·확장형 구조로 발전시켰습니다.  

설계 과정에서는 다음 두 가지를 특히 중점적으로 고려했습니다.  

- **유지보수성**: 던전별 로직 분리 및 독립 관리  
- **확장성**: 데이터만 교체해 다양한 던전 유형 추가 가능  

---

<br><br>

## 🎯 Dungeon 구성 요소 
아래 구성들은 던전 시스템의 핵심 데이터를 간결하게 표현한 구조입니다.  

<br><br>

## Title
- 던전의 이름, 유형, 설명 등  
- 던전을 식별하고 UI 및 시스템에서 활용되는 기본 정보  
- **( Normal, Rush, Protect, Rescuer, Target )**
<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Title.png" width="650"/> </p>

  <br><br>
  <hr>
  
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

<br><br>
<hr>

## Condition
- 던전에 입장하기 위한 조건을 명시  
  - 플레이어 레벨  
  - 필요 아이템  
  - 특정 스테이지 선행 여부  

<br><br>
<hr>

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
<br><br>
<hr>

## Reward
- 클리어 보상 정보  
  - 경험치
  - 명성치
  - 스킬포인트
  - 아이템  
  - 골드  
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/R1.png" width="700" style="display:inline-block;"/>

  <br><br>
<hr>

## MapData
- 던전에 사용되는 던전 Scene index와 위치, 회전 등의 맵 정보.

<br><br>
<hr>

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

<br><br>
<hr>

## SpawnData 
- 전투 구성에 필요한 모든 스폰 및 라운드 데이터  
  - Enemy/Boss 스폰 리스트
  - Playable AI 리스트
  - 이동 불가 벽 정보
  - 웨이브/라운드 구성   
- **( Normal, Rush, Protect, Rescuer, Target, TimeAttackRush )**
<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/SpawnD1.png" width="650"/> </p>


<br><br><br><br><br><br>

## ⚡ Dash System
<p align="center">  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Dash.gif" width="400" style="display:inline-block;"/>
 
- 고속 타격 기반의 지형·적 감지형 대시 시스템 

대시 시스템은 단순한 돌진이 아니라 지형, 적, 장애물, 카메라, 쿨타임 UI로 구성된 전투 시스템으로 설계되었습니다.

아래 두 가지 목표를 중심으로 구현되었습니다.
- **정확성 : 안전하게 이동 가능한 지점만 계산하여 오동작을 최소화**
- **전술성 : 적·지면·장애물 판정을 조합해 전략적으로 대시를 활용 가능**
  
<br><br>

## ⭐ Dash 설계 핵심 요소
<div align="center">
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/G1.gif" width="450" style="display:inline-block;"/>
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/G2.gif" width="450" style="display:inline-block;"/>
</div>

- 대시는 아래와 같은 구조로 실행됩니다.
  
<br><br>
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
<br><br>
<hr>

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
<br><br>
<hr>

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

<br><br>
<hr>

## Obstacle Check
<p align="center">  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Ground.gif" width="400" style="display:inline-block;"/>
 
- 대시 경로에 장애물이 존재하는지 사전 검출합니다.
  - OverlapSphereNonAlloc 기반 충돌 예측 및 최적화
- 장애물과 충돌하면 Target 자동 변경 또는 대시 취소
  
<br><br>
<hr>

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


<br><br><br><br><br><br>


## ⚡ Item Editor Tool
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Item1.gif" width="800" style="display:inline-block;"/>

- 아이템 에디터 툴은 게임 내 모든 아이템 데이터를 효율적으로 관리하고, ScriptableObject를 기반으로 무기·장비·소모품 등 다양한 항목을 직관적으로 생성·편집할 수 있도록 설계되었습니다.
- 프로젝트 전체 밸런싱 및 아이템 제작 속도를 극적으로 향상시키는 핵심 개발 도구입니다.
<br><br>


## 🎯 설계 목표

- 아이템 데이터의 종류가 증가할수록 수작업 수정과 관리 비용이 기하급수적으로 늘어났으며, 이를 효율적으로 해결하기 위해 다음 두 가지 목표를 중심으로 개발하였습니다.
  - 단일 창에서 모든 아이템 관리하고 추가 ,수정 ,삭제 구현
  - 즉시 적용 가능한 편집 환경 구축 ScriptableObject 저장



## 📦 아이템 카테고리
<div align="center">
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I3.png" width="150" style="display:inline-block;"/>
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I4.png" width="150" style="display:inline-block;"/>
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I5.png" width="150" style="display:inline-block;"/>
  <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I6.png" width="150" style="display:inline-block;"/>
</div>

- 각 아이템은 명확한 Category 체계로 분류되며, 특성에 따라 개별 속성과 기능이 다르게 적용될 수 있습니다.


```csharp
            EditorGUILayout.BeginHorizontal("helpbox"); 
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("카테고리", GUILayout.Width(300)))
                {
                    categoriWindwow = GetWindow<CategoryEditorWindow>("카테고리 선택창");
                    categoriWindwow.minSize = new Vector2(800, 250);
                    categoriWindwow.maxSize = new Vector2(800, 250);
                    categoriWindwow.position = new Rect(itemwindow.position.x + 550, itemwindow.position.y + 50, itemwindow.position.width, itemwindow.position.height);
                    categoriWindwow.Show();
                }
            }
            EditorGUILayout.EndHorizontal();     
```
- CategoryEditorWindow를 따로 구현하여 해당 Editor Window를 사용하는 식으로 구현했습니다.
- 카테고리 버튼을 클릭하면 window Eidotr가 항상 하단에 열리도록 위치를 고정시켰습니다.


<br><br><br>
<hr>

## 📦 아이템 타입
<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Item2.gif" width="1000" style="display:inline-block;"/>

- 아이템 타입 Enum을 변경하며 해당 타입에 맞는 GUI가 변경되며 값을 설정할 수 있습니다.
  

<br><br><br>
<hr>

## 📦 UseableObject
**Item에 UseableObject ScriptableObject로 다양한 아이템 기능을 구현.**

- 아이템 기능은 UseableObject ScriptableObject 조합 방식으로 정의됩니다.
- 즉, 하나의 아이템이 여러 효과를 동시에 가질 수 있으며, 개발자가 원하는 만큼 기능을 추가하여 확장형 설계가 가능합니다.

>밑에는 아이템 기능 예시입니다.
<div align="center">
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I8.png" width="200" style="display:inline-block;"/>
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I1.png" width="600" style="display:inline-block;"/>
</div>

- 체력을 즉시 900을 회복해주는 포션 아이템입니다.
- 각 아이템 재사용 쿨타임을 설정할 수 있게 만들어서 게임의 흐름을 조절할 수 있게 구현했습니다.
<br>
<hr>

<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I2.png" width="600" style="display:inline-block;"/>

- 마을로 복귀하는 기능을 가진 스크롤 아이템입니다.
<br>
<hr>

<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I11.png" width="600" style="display:inline-block;"/>

- 한 아이템의 UseableObject의 갯수를 늘려서 사용할 수 있습니다.
- 위에는 공격력 버프, 크리티컬 버프, 15초동안 체력 회복. 3가지 기능이 있는 아이템입니다.
<br>
<hr>

<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I12.png" width="600" style="display:inline-block;"/>
  
- Buff UseableObject입니다. 
- 값과 지속시간을 설정할 수 있고 중복 사용 가능 여부과 디버프 옵션을 만들었습니다.
<br>
<hr>



## 📦 잠재능력 확률 설정
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I9.png" width="600" style="display:inline-block;"/>
  
- Equipment 카테고리 (무기, 방어구, 악세사리, 칭호)에만 존재합니다.
- 장비 아이템의 잠재능력 등급 확률 값을 설정합니다.
- 자동 설정을 체크하면 None 30%, Normal 40%, Rare 20%, Unique 6%, Legendary 4% 로 자동 세팅됩니다.

<br>
<hr>
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I10.png" width="600" style="display:inline-block;"/>
  
-자동 체크 해제 시, 개발자가 등급별 확률을 세부 조정할 수 있습니다.

<br>
<hr>

## 📦 저장
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/I13.png" width="1000" style="display:inline-block;"/>
  
- .csv 파일로 Item Editor 데이터를 저장 / 불러오기로 관리합니다.

<br>
<hr>

## enum 분류 

- 아이템은 Category → Type → ItemName의 계층 Enum 구조로 관리되며 트리 구조처럼 정렬된 Inspector UI를 보여줍니다.
- 단일 Enum에 모든 아이템을 나열하는 방식 대신, 카테고리와 타입을 독립된 Enum 파일로 분리하여 아이템 수가 증가해도 검색 과정의 가독성을 유지할 수 있도록 설계했습니다.
- 결과적으로 아이템 선택 과정에서 스크롤 탐색 없이 원하는 타입으로 즉시 접근 가능하며, 대규모 아이템 확장 시에도 관리 구조가 흔들리지 않도록 안정성을 확보하였습니다.

<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Item3.gif" width="500" style="display:inline-block;"/>


```csharp
 private string GetInspectorName(BaseItemClip clip)
    {
        string retName = "[InspectorName(|*|)]";

        switch (clip.itemCategoryType)
        {
            case ItemCategoryType.EQUIPMENT:
                if (clip.equipmentTpye == EquipmentTpye.WEAPON)
                    retName = retName.Replace("*", "Equipment/Weapon/" + clip.itemName);
                else if (clip.equipmentTpye == EquipmentTpye.ARMOR)
                    retName = retName.Replace("*", "Equipment/Armor/" + clip.itemName);
                else if (clip.equipmentTpye == EquipmentTpye.ACCESSORIES)
                    retName = retName.Replace("*", "Equipment/Accessorie/" + clip.itemName);
                else if (clip.equipmentTpye == EquipmentTpye.TITLE)
                    retName = retName.Replace("*", "Equipment/Title/" + clip.itemName);
                break;

            case ItemCategoryType.CONSUMABLE:
                if (clip.consumableType == ConsumableType.ENCHANT)
                    retName = retName.Replace("*", "Consumable/Enchant/" + clip.itemName);
                else if (clip.consumableType == ConsumableType.POSION)
                    retName = retName.Replace("*", "Consumable/Posion/" + clip.itemName);
                break;

            case ItemCategoryType.MATERIAL:
                if (clip.materialType == MaterialType.CRAFT)
                    retName = retName.Replace("*", "Material/Craft/" + clip.itemName);
                else if (clip.materialType == MaterialType.EXTRA)
                    retName = retName.Replace("*", "Material/Extra/" + clip.itemName);
                break;

            case ItemCategoryType.QUESTITEM:
                if (clip.questIType == QuestIType.QUEST)
                    retName = retName.Replace("*", "Quest/" + clip.itemName);
                break;
        }

        return retName.Replace('|', '"');
    }
```



  


<br><br><br><br><br><br>
<hr>




## ⚡ 잠재능력 Editor Tool
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Potential1.gif" width="800" style="display:inline-block;"/>
  
- 장비 기반 아이템(Weapon / Armor / Accessory / Title) 에만 적용되는 잠재능력 시스템을 관리하기 위해 제작된 Unity Editor 확장 툴입니다.
- 등급별 수치 범위, Split 구간, 확률 값을 시각적으로 확인하며 편집할 수 있어, 데이터 밸런싱 및 생산성에 최적화 되게 만들었습니다.

<br>
<hr>

## 잠재능력 Function ScriptableObject
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/p6.png" width="600" style="display:inline-block;"/>
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/p7.png" width="300" style="display:inline-block;"/>


- 잠재능력 효과는 PotentialFunctionObject를 상속하는 ScriptableObject 단위로 독립화되어 있습니다.
- 각 잠재능력은 Apply() / Remove() 메서드를 오버라이드하여 장비 장착,해제 상황에 대응하도록 구현했으며,
  신규 효과 추가 시 클래스만 생성하면 즉시 시스템에 편입될 수 있도록 구조적 확장성을 확보했습니다.

>밑에는 최대 체력 % ScriptableObject 부분입니다.
```csharp
[CreateAssetMenu(menuName = "Potential/Max Hp Percentage", fileName = "MaxHpPecentagePotentialFunction")]
public class MaxHpPercentPotentialFunction : PotentialFunctionObject
{
    public override void Apply(float value, PlayerStatus playerStatus)
    {
        playerStatus.MaxHpPercentage += value;
        float additiveHp = playerStatus.CurrentHealth * value;
        playerStatus.SetCurrentHP(playerStatus.CurrentHealth + (int)additiveHp);
    }

    public override void Remove(float value, PlayerStatus playerStatus)
    {
        playerStatus.MaxHpPercentage -= value;
        float additiveHp = playerStatus.CurrentHealth * value;
        playerStatus.SetCurrentHP(playerStatus.CurrentHealth - (int)additiveHp);
    }

}
```

```csharp
 public void Apply(bool isEquip , PlayerStatus playerStatus)
    {
        if (clipData.potentialFunctionObject == null) return;

        if (isEquip)
            clipData.potentialFunctionObject.Apply(potentialValue, playerStatus);
        else
            clipData.potentialFunctionObject.Remove(potentialValue, playerStatus);
    }
```


<br>
<hr>


## 카테고리
- 잠재능력(Potential) 시스템은 장비 타입별 옵션 특성 및 밸런스를 독립적으로 관리하기 위해 다음 5개의 Category로 세분화하였습니다.

<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/pp6.png" width="600" style="display:inline-block;"/>
<div align="center">
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/pp7.png" width="190" style="display:inline-block;"/>
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/pp8.png" width="190" style="display:inline-block;"/>
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/pp9.png" width="190" style="display:inline-block;"/>
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/pp10.png" width="190" style="display:inline-block;"/>
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/pp11.png" width="190" style="display:inline-block;"/>
</div>

- Common 카테고리는 장비 유형과 관계없이 모든 Potential에 공통 적용되는 기반 옵션을 정의합니다.
- 각 전용 카테고리는 해당 장비에서만 사용 가능한 고유 Potential 옵션을 세팅합니다.

<br>
<hr>

## 세부 설정
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Potential2.gif" width="800" style="display:inline-block;"/>

- 각 등급마다 최소 ~ 최대 값을 입력하고 Split Count를 입력해서 분할 개수를 정합니다. 
- 각 분할 퍼센트는 직접 수정할 수 있으며, 정보의 Split 범위 값은는 자동으로 나눠집니다. 
- Split Count는 최소 2 ~ 최대 5를 넘을 수 없습니다.

<hr>
<br>
>예시)밑에 사진처럼 Normal 등급의 Split Count가 3개이고 각 퍼센트가 60% , 35%, 5% 일경우
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/pp4.png" width="500" style="display:inline-block;"/>

- 해당 아이템의 Normal 등급 잠재능력에서의 수치는 60% 확률로 10 ~ 40 사이 값이되며, 35% 확률로 40 ~70,  5% 확률로 70 ~ 100 값이 됩니다.

<hr>


<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/pp5.png" width="500" style="display:inline-block;"/>

- 각 등급의 값의 범위는 밑에 한눈에 보기 쉽게 나타냈습니다.



<br>
<hr>
## In Game UI
<div align="center">
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/p1.png" width="165" style="display:inline-block;"/>
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/p2.png" width="155" style="display:inline-block;"/>
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/p3.png" width="150" style="display:inline-block;"/>
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/p4.png" width="155" style="display:inline-block;"/>
   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/p5.png" width="165" style="display:inline-block;"/>
</div>
<p align="center">  None -> Normal -> Rare -> Unique -> Legendary 

- In Game에서의 아이템 잠재능력 UI입니다.
- 등급별로 텍스트의 색을 변경하여 한 눈에 구분하기 쉽게 구현했습니다. 



<br><br><br><br><br><br>






<a id="console-command"></a>
## ⚡ 콘솔(커맨드) 창
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Con1.gif" width="600" style="display:inline-block;"/>

프로젝트가 커질수록, 단순 UI 기반 수동 설정만으로는 반복 작업 비용이 크게 증가했습니다.  
아이템 생성, 데이터 필드 수정, 오브젝트 등록, 테스트 명령 입력 등 사소해 보이는 일들이 쌓이고 쌓여 개발 속도는 점점 느려지고 유지보수 난이도는 기하급수적으로 증가했습니다.

- 특히, 다음 문제가 발생했습니다.
  - 모든 기능을 버튼/인스펙터에서 하나씩 눌러야 함
  - 테스트 과정에서 매번 씬을 이동하거나 메뉴 UI를 열었다 닫는 시간이 누적
  - 개발 과정에서 자주 사용하는 명령들이 절차적으로 너무 길음
  - 반복적인 입력은 실수를 유발하고, 수정 테스트 루프가 길어짐

**그래서 저는 개발용 Console UI를 직접 구축하기로 결정했습니다.**


<hr>


## 🎯 설계 목표

**1.명령 한 줄로 기능 실행**
  - 복잡한 인스펙터 조작 없이, 필요한 기능을 즉시 호출할 수 있습니다.

**2.자동 검색·자동 완성 지원**
  - 자주 사용하는 명령을 빠르게 찾고 다시 실행할 수 있어 테스트 반복 비용을 줄입니다.

**3.확장성 중심 구조**
  - 필요한 기능이 있다면 여러개를 만들어서 테스트 가능합니다. 

**4.실행 결과 확인 가능**
  - 명령 실행 기록이 UI에 표시되어 성공/실패 여부를 즉각 검증할 수 있습니다.

**.5테스트 흐름 방해 최소화**
  - 입력 포커스 유지, 마우스 진입 처리 등 사용 간섭을 줄여 개발 집중도를 높입니다.

<br><br><br>
<hr>


## 구조
<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/ConU.drawio.png" width="800" style="display:inline-block;"/>

- 콘솔 시스템은 ConsolePresenter를 중심으로 한 MVP(Model–View–Presenter) 구조로 설계되었습니다.
- 각 컴포넌트(ConsoleUI, ConsoleProcess, SearchableText)는 서로를 직접 참조하지 않으며, 
  delegate 및 이벤트 기반 통신을 통해 상호 작용하도록 구현했습니다.
- 이를 통해 UI, 입력 처리, 검색 및 실행 로직을 명확히 분리하여 낮은 결합도와 높은 유지보수성을 목표로 했습니다.


## 구조 특징

- View(ConsoleUI)
  - 사용자 입력 및 출력 전용 계층으로, 로직을 포함하지 않습니다.
- Presenter(ConsolePresenter)
  - 모든 이벤트를 중계하며, 각 컴포넌트 간 의존성을 제거합니다.
- Model(ConsoleProcess, SearchableText)
  - 커맨드 처리와 검색 기능을 담당하며 UI와 완전히 분리되어 있습니다.

<br>
<hr>

**ConsolePresenter 역할**

- UI 입력 이벤트를 수신하고, 이를 적절한 처리 로직으로 전달하는 중앙 제어자 역할을 수행합니다.
- 검색 요청, 명령 실행, 로그 출력, Task 초기화 등 전체 콘솔 흐름을 조율합니다.
- History 입력 상태에 따라 검색 갱신을 제한하여 입력 충돌을 사전에 방지합니다.

```csharp
public class ConsolePresenter : MonoBehaviour
{
    [SerializeField] private ConsoleUI consoleUI;
    [SerializeField] private ConsoleProcess consoleProcess;
    [SerializeField] private SearchableText searchable;

    private void Awake()
    {
        consoleUI.InputField.onSubmit.AddListener(OnSummit);

        searchable.onGetSearchableTests += consoleProcess.GetTextList;
        searchable.onGetSearchableFilters += consoleProcess.GetTextFilters;
        consoleProcess.onClear += consoleUI.ClearAllTask;
        searchable.onSelect += consoleUI.SettingInputFieldText;
        consoleUI.onExcuteSummitProcess += consoleProcess.Excute;

        consoleProcess.onExcuteLog += consoleUI.ExcuteSystemLog;
        consoleUI.onCanHistroy += searchable.IsSelectSearchableTest;
        searchable.onCanUpdate += SearchableCanUpdate;
    }

    private void OnDestroy()
    {
        consoleUI.InputField.onSubmit.RemoveListener(OnSummit);

        searchable.onGetSearchableTests -= consoleProcess.GetTextList;
        searchable.onGetSearchableFilters -= consoleProcess.GetTextFilters;
        consoleProcess.onClear -= consoleUI.ClearAllTask;

        searchable.onSelect -= consoleUI.SettingInputFieldText;
        consoleUI.onExcuteSummitProcess -= consoleProcess.Excute;
        consoleProcess.onExcuteLog -= consoleUI.ExcuteSystemLog;
        consoleUI.onCanHistroy -= searchable.IsSelectSearchableTest;
        searchable.onCanUpdate -= SearchableCanUpdate;
    }


    public bool SearchableCanUpdate()
    {
        if (consoleUI.GetIsDoingHistory())
            return false;
        else
            return true;
    }

    public void OnSummit(string text)
    {
        if (searchable.CurrentSelectTask != null) return;
        consoleUI.ExcuteSummit(text);
    }

}

```


## 기능 사용  
<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Con2.gif" width="600" style="display:inline-block;"/>

- 콘솔 명령을 통해 인벤토리 내 장비 목록을 조회한 뒤, 해당 목록의 0번 인덱스 장비 아이템의 잠재 능력을 재설정하는 과정을 시연한 예시입니다.

<hr>

<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Con3.gif" width="600" style="display:inline-block;"/>

- 콘솔 명령을 통해 플레이어의 레벨을 55 증가 시키는 기능 시연입니다. 

<hr>

<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Con4.gif" width="600" style="display:inline-block;"/>

- 콘솔 명령을 통해 플레이어의 전체적인 이동속도를 10 증가 시키는 기능 시연입니다.



이런식의 기능들을 사용하며 디버깅 및 밸런싱 작업 시 특정 장비의 옵션을 즉시 수정할 수 있도록 설계되어  
반복되는 수동 작업을 대폭 줄여 개발 효율을 향상시킵니다.




<br><br><br>
<hr>


## History 기능

<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Con5.gif" width="600" style="display:inline-block;"/>

- 편리성을 위해 입력했던 명령어를 자동으로 기록하여, 키보드 ↑/↓ 입력만으로 이전 또는 다음 명령어를 빠르게 탐색할 수 있도록 구성했습니다.  
- 반복 입력이 필요한 디버깅,테스트 환경에서 생산성을 크게 향상시키며, 동일한 명령을 다시 입력할 때 발생할 수 있는 오타도 방지합니다.





<br><br><br>
<hr>

## Searchable 

<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Con6.gif" width="600" style="display:inline-block;"/>

입력한 문자열을 기반으로 자동 검색(Search Suggestion) 목록을 생성하고,  
키보드/마우스 입력으로 검색 결과를 탐색 및 선택할 수 있도록 구성된 UI 검색 시스템입니다.



**입력한 텍스트를 전달받아**
  - 1.검색 조건에 맞는 문자열을 필터링
  - 2.필터 단어(key → transValue) 자동 치환
  - 3.기존 Task 오브젝트 반환(Object Pooling)
  - 4.새 Task 오브젝트 재생성
  - 5.스크롤 및 레이아웃 초기화

검색 결과가 없으면 검색창(rootContainer)은 자동으로 닫힙니다.

<br>
<hr>

- 현재 입력된 문자열을 기준으로 검색 가능한 텍스트 목록을 필터링하여 배열로 반환합니다.
```csharp
 public string[] GetSeachableTexts2(string currrInput)
    {
        List<string> retStr = new List<string>();

        for (int i = 0; i < searchableList.Length; i++)
        {
            if (CheckIsSameSentence(currrInput, searchableList[i]))
                retStr.Add(searchableList[i]);
        }

        return retStr.ToArray();
    }
```

<br><br>
<hr>


- 입력한 문장(input)이 특정 검색 문자열(searchable)과 동일한 구조인지 검사합니다.  
- 단어 단위 및 문자 단위 prefix 비교를 수행하며, 필터에 정의된 값(value) 영역은 비교에서 제외합니다.
```csharp

public bool CheckIsSameSentence(string input, string searchable)
{
    // 입력값이 비어 있으면 비교 불가
    if (input == string.Empty || input == "" || input.Trim() == "")
        return false;

    // 특정 key가 있는 위치(해당 위치는 비교 제외)
    int valueIndex = GetValue(searchable);

    // 단어 단위로 분리
    string[] inputWordSplit = input.ToLower().Split(' ');
    string[] searchWordSplit = searchable.ToLower().Split(' ');

    // 입력한 단어 배열만큼 비교 수행
    for (int i = 0; i < inputWordSplit.Length; i++)
    {
        // valueIndex 위치는 비교에서 제외 (특정 키워드 필터링 목적)
        if (valueIndex != -1 && valueIndex == i)
        {
            Debug.Log(inputWordSplit[i]);
            continue;
        }

        // 검색 대상 단어가 모자라면 불일치
        if (searchWordSplit.Length <= i)
            return false;

        char[] inputCharSplit = inputWordSplit[i].ToCharArray();
        char[] searchCharSplit = searchWordSplit[i].ToCharArray();

        // char 단위 앞에서부터 비교 (prefix 비교)
        for (int x = 0; x < inputCharSplit.Length; x++)
        {
            // 검색 대상 문자열이 더 짧으면 불일치
            if (x >= searchCharSplit.Length)
                return false;

            // 다른 글자가 발견되면 불일치
            if (inputCharSplit[x] != searchCharSplit[x])
                return false;
        }
    }

    // 모든 단어가 prefix 조건을 만족하면 동일 문장으로 판단
    return true;
}

```

<br><br>
<hr>

- 현재 입력된 문자열을 기반으로 검색 결과를 생성하고, UI를 갱신하며, 풀링된 Task 오브젝트를 재사용하여 리스트를 구성합니다.  
- 필터 적용, Task 재생성, 스크롤 초기화 및 마우스 이벤트 등록까지 검색 UI 전체를 관리하는 핵심 메서드입니다.
```csharp
 public void GetText(string currSearchText)
    {
        selectIndex = -1;

    // 현재 입력값으로 검색된 문자열 리스트 반환
    findCommandLists = GetSeachableTexts2(currSearchText);

    // 스크롤용 RectTransform 리스트 초기화
    scrollTasks.Clear();

    // 검색 결과 내에서 key → transValue로 UI 표시용 문자열 치환
    for (int i = 0; i < findCommandLists.Length; i++)
        for (int x = 0; x < filters.Count; x++)
            if (findCommandLists[i].Contains(filters[x].key))
                findCommandLists[i] = findCommandLists[i].Replace(filters[x].key, filters[x].transValue);

    // 검색 결과가 없을 경우 UI 닫기
    if (findCommandLists == null || findCommandLists.Length <= 0)
    {
        isOpenSearchable = false;
        rootContainer.gameObject.SetActive(false);
        return;
    }

    // 검색창 열기
    rootContainer.gameObject.SetActive(true);
    isOpenSearchable = true;

    // 기존 Task 오브젝트들 반환
    for (int i = 0; i < tasks.Count; i++)
        ObjectPooling.Instance.SetOBP(taskList.ToString(), tasks[i].gameObject);
    tasks.Clear();

    // 검색된 문자열을 기반으로 새로운 Task 생성
    for (int i = 0; i < findCommandLists.Length; i++)
    {
        SearchableTextTask task = ObjectPooling.Instance
            .GetOBP(taskList.ToString())
            .GetComponent<SearchableTextTask>();

        task.transform.parent = taskContainer;
        task.transform.SetAsFirstSibling();   // 입력된 순서 역순 배치
        task.SettingTask(findCommandLists[i]);
        tasks.Add(task);

        // 스크롤 이동용 RectTransform 저장
        if (task.GetComponent<RectTransform>())
            scrollTasks.Add(task.GetComponent<RectTransform>());
    }

    // UI 상단 기준 정렬을 위해 리스트 반전
    tasks.Reverse();
    layoutGroup.Excute();
    scrollTasks.Reverse();

    // 스크롤 UI에 Task 목록 전달
    scroll.SettingTask(scrollTasks.ToArray());

    // 마우스 선택 가능할 경우 이벤트 등록
    if (canMouseSelect)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            int index = i;
            UIHelper.AddEventTrigger(tasks[i].gameObject, EventTriggerType.PointerEnter,
                delegate { SelectByMouse(index); });

            UIHelper.AddEventTrigger(tasks[i].gameObject, EventTriggerType.PointerClick,
                delegate { ClickByMouse(index); });
        }
    }
}
```



<br><br><br>
<hr>

## 예외처리
<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/con2.png" width="400" style="display:inline-block;"/>

- 등록되지 않은 명령어를 입력하거나, 필요한 인수 개수가 맞지 않을 때 시스템 오류 메시지를 시각적으로 표시하여 즉시 문제를 인지할 수 있도록 했습니다.
- 예외 케이스는 입력값 검증 단계에서 선제적으로 처리되며, 잘못된 명령어 실행으로 인해 발생할 수 있는 내부 로직 오류를 사전에 방지합니다.

**잘못된 입력이 반복되더라도 시스템이 중단되지 않고 안정적으로 동작하도록 입력 검증 로직을 모듈화해 유지보수성을 향상했습니다.**


<br><br><br>
<hr>

## 등록
<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/con1.png" width="400" style="display:inline-block;"/>

- 인스펙터서 커맨드 기능을 등록합니다.
- 값이 필요한 명령어에는 ( )로 입력 타입을 명시하고, 입력 가능한 범위를 설정합니다. 


<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/con5.png" width="600" style="display:inline-block;"/>

- 위와 같이 커맨드를 원하는 형식으로 등록한 후, 내부 코드에서는 해당 정보를 기준으로 실제 입력 값을 변환하여 실행합니다.
- Key 값으로 등록된 명령어를 빠르게 탐색하여, 사용자가 입력한 문자열을 해당 명령어 로직으로 매핑하는 구조로 설계했습니다.




<br><br><br>
<hr>

## Help Options
<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/con6.png" width="500" style="display:inline-block;"/>
  
- Help Option입니다. (-l, -h) 두개를 구현했습니다. 
  - 사용은 해당 명령어 뒤에 붙여서 사용합니다. 
>ex) Print All Command List -l

**-l Help Option**

<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/con3.png" width="500" style="display:inline-block;"/>

- 이 옵션은 해당 명령어가 성공적으로 실행됬는지 확인하는 옵션입니다.
- 노란색 Text로 표시되며, 명령어마다 출력되는 내용이 다르게 나옵니다.

```csharp
        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Print All Item List", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
```


<hr>
<br>

**-h Help Option**

<p align="center"><img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/con4.png" width="500" style="display:inline-block;"/>

- 이 옵션은 해당 명령어가 무엇인지, 혹은 범위가 어떻게 되는지 등을 확인하는 옵션입니다.


```csharp
       if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Print Item List {itemListCount}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
```



<hr><hr>
<br><br><br><br><br><br><br>

# ⏳ 트러블 슈팅
## 🎨 메테리얼 최적화 과정
- NPC나 몬스터를 생성할 때, 캐릭터별로 지정된 색상을 적용하기 위해 메테리얼 컬러 값을 변경하는 기능을 구현하고 있었습니다.
- 즉, 스폰된 캐릭터마다 고유한 색상을 설정하는 과정에서 자연스럽게 메테리얼을 수정하는 로직이 필요했습니다.


<br><br>

## ⚠ 문제 발생
- 메테리얼의 색상을 변경하는 과정에서 기존 메테리얼을 직접 수정하는 것이 아니라, Unity가 내부적으로 새로운 메테리얼 인스턴스를 생성하여 변경을 적용하고 있다는 사실을 확인했습니다.

즉, **공유 메테리얼(Shared Material)** 을 수정하는 것이 아니라, Renderer마다 **고유한 메테리얼 인스턴스(Material Instance)** 를 새로 생성해 적용하는 구조였습니다.

- 이로 인해 NPC나 몬스터가 많아질수록 고유 인스턴스가 기하급수적으로 늘어났고, 그만큼 드로우콜 증가 → 배칭이 깨짐 → 퍼포먼스 저하가 발생했습니다.


<br><br>


## 🔍 원인 분석
- Unity의 메테리얼 구조상 renderer.material을 수정하면 기존 메테리얼은 공유된 상태

수정 순간 Renderer마다 고유한 인스턴스(Material Instance)를 생성
하게 됩니다.

즉, 색상 하나만 바뀌어도 전부 다른 메테리얼로 인식되기 때문에
Static/Dynamic Batching이 적용되지 않고 Draw Call이 불필요하게 확대되는 것이 원인이었습니다.


<br><br>

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


<br><br><br><br>




## 🎨Layout group 성능 문제  
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/L1.png" width="500" style="display:inline-block;"/>

- 초기에는 Inventory, 상점 UI, Reward UI 등 다수의 UI 요소가 표시되는 화면에 Unity가 기본 제공하는 Layout Group을 사용하고 있었습니다.
  레이아웃 정렬이 자동으로 이루어져 UI 구성은 편리했지만, 실제 플레이 환경에서는 예상치 못한 성능 저하가 발생했습니다. 



<br><br>


## ⚠ 문제 발견

- UI 요소가 많아질수록 화면 전환 및 스크롤 상황에서 프레임 저하가 눈에 띄게 증가
- Inventory나 상점처럼 자식 UI가 많은 패널에서 Canvas Rebuild가 반복적으로 발생
- 즉, 불필요한 작업이 발생.



<br><br>

  
## ✅ 해결 방법
- Unity 기본 Layout Group 사용을 중단하고, 레이아웃을 필요할 때만 **단일 호출**로 갱신하는 구조로 재설계하였습니다.
- UI 변동 여부에 따라 콘텐츠 크기를 자동 재조정할 수 있도록 **Content Size Filter** 기능을 선택적 옵션으로 제공해 유연하게 활용할 수 있도록 만들었습니다.

BaseLayoutGroup을 부모로, Grid / Horizontal / Vertical의 기능을 만들었습니다.


<br><br>


**1) AnchorSetting()**
<p align="center">   <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/L2_1.png" width="500" style="display:inline-block;"/>
  
- 모든 RectTransform의 기준점을 Left-Top 기준(0,1) 으로 고정하여 UI 배치 시 혼동을 제거했습니다.
- 중심점, Anchor 차이로 발생하던 재배치 오류를 방지하고 계산을 단일 좌표 기준으로 수행할 수 있습니다.

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



<br><br><br>

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


<br><br><br>

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

<br><br><br>

## 🎨 UI 스크롤 문제

- Inventory, 상점 Slot, 스킬 UI 등에서는 EventTrigger를 활용해 마우스 인터랙션을 처리하고 있습니다.
- UI 생성 시 아래 메서드를 통해 각 요소에 필요한 이벤트를 동적으로 등록합니다.

```csharp
 public static void AddEventTrigger(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (trigger == null)
          trigger = go.AddComponent<EventTrigger>();

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
```
해당 방식으로 생성된 UI 요소는 버튼이 아니더라도 커서 진입, 클릭, 드래그 등 다양한 사용자 입력을 감지할 수 있습니다.



<br><br><br>


## ⚠ 문제 발견
- EventTrigger가 적용된 UI가 Button 기반이 아니기 때문에, ScrollRect 내에서 스크롤 드래그 우선순위가 정상적으로 전달되지 않는 문제가 확인되었습니다.
- 그 결과 해당 UI 이미지가 스크롤 이벤트를 선점하며, 이미지가 아닌 빈 영역에서만 드래그 스크롤이 가능해졌습니다.

즉, 클릭 및 마우스 오버가 요구되는 UI가 포함될 경우 ScrollRect의 입력 우선순위가 충돌하면서 부드러운 스크롤 체감이 저하되고 UX 일관성이 무너지는 결과가 나타났습니다.


<br><br><br>



## ✅ 해결 방법
- Unity ScrollRect/Scrollbar 컴포넌트를 사용하지 않고 Wheel 입력 + Custom ScrollBar UI를 직접 구현하여 동작을 세밀하게 제어하였습니다.
- Scroll 영역 위에 마우스가 올라왔을 때만 Wheel 입력을 허용하고, 스크롤이 필요 없는 경우에는 ScrollBar를 자동으로 숨기는 방식으로 UI 효율성과 시각적 안정성을 확보하였습니다.
- 드래그, 휠, 스크롤바 UI 이동이 모두 동일한 ScrollValue 기반으로 동기화되도록 설계하였습니다.
  
<br><br>

**1) Awake() 초기화**
```csharp
private void Awake()
{
    UIHelper.AddEventTrigger(rootRect.gameObject, EventTriggerType.PointerEnter, delegate { OnPointerEnter();});
    UIHelper.AddEventTrigger(rootRect.gameObject, EventTriggerType.PointerExit,  delegate { OnPointerExit();  });

    if (scrollbarHandler != null)
    {
        UIHelper.AddEventTrigger(scrollbarHandler.gameObject, EventTriggerType.BeginDrag, delegate { HandlerDragStart(); });
        UIHelper.AddEventTrigger(scrollbarHandler.gameObject, EventTriggerType.Drag,      delegate { HandlerDrag();      });
    }

    barOriginalYSize = (!isScrollHorizontal && scrollbarBackground != null) ? scrollbarBackground.sizeDelta.y : barOriginalYSize;
    barOriginalXSize = ( isScrollHorizontal && scrollbarBackground != null) ? scrollbarBackground.sizeDelta.x : barOriginalXSize;

    limitMaxViewValue = GetMaxScrollValue();

    if (scrollbarHandler != null)
    {
        barMaxPos = scrollbarHandler.transform.position.y + barOriginalYSize;
        barMinPos = scrollbarHandler.transform.position.y;
    }
}

```
- Awake() 단계에서 각종 Pointer/Drag 이벤트를 직접 연결하여 ScrollRect 없이 입력을 제어할 수 있도록 구성하였습니다.
- 스크롤바 크기, 좌표, 최대 이동량 등을 초기 계산하여 Update 구간의 연산량을 최소화하였습니다.


<br><br>

**2) Update() 프레임 루프**
```csharp
private void Update()
{
    UpdateScrollVariables();      // 스크롤 비율 · 마우스 위치 계산
    ProcessScrollbarVisibility(); // 스크롤 가능한 경우에만 바 표시
    if (isMouseEnter) HandleMouseScroll(); // UI 내부일 때만 Wheel 사용
    ApplyScrollToTarget();        // Content 이동 반영
    ApplyScrollbarPosition();     // ScrollBar UI 위치 동기화
}
```
<br><br>

**3) Pointer 잔입 여부**

```csharp
private void OnPointerEnter() => isMouseEnter = true;
private void OnPointerExit()  => isMouseEnter = false;
```

- 마우스가 UI 영역 내부에 들어온 경우에만 휠 입력을 수신하도록 하였습니다.
- 인벤토리, 상점, 보상 UI 등 복수 UI가 띄워질 때도 중복 스크롤이 발생하지 않도록 안정적으로 제어할 수 있습니다.

<br><br>


**4) ScrollValue 계산**
```csharp
private void UpdateScrollVariables()
{
    if (scrollbarHandler == null) return;

    testMousePos = Input.mousePosition.y;
    currBarY     = scrollbarHandler.transform.position.y;
    middlePos    = scrollbarHandler.transform.position.y + (barSize / 2f);
    percent = Mathf.InverseLerp(0f, barOriginalYSize - scrollbarHandler.rect.height, scrollbarHandler.anchoredPosition.y);

    if (isUpdateSize)
        limitMaxViewValue = GetMaxScrollValue();
}
```

- 스크롤 진행률(percent)과 핸들 위치를 기반으로 스크롤 상태를 갱신하도록 구현하였습니다.
- Content 사이즈가 변경될 경우 즉시 limitMaxViewValue를 다시 계산하여 확장형 콘텐츠에도 대응 가능합니다.
  
<br><br>

**5) 스크롤 가능 시에만 Bar UI 자동 활성화**
```csharp
private void ProcessScrollbarVisibility()
{
    if (!scrollOverRootRect) return;

    bool noScroll = limitMaxViewValue <= 0;
    scrollbarHandler?.gameObject.SetActive(!noScroll);
    scrollbarBackground?.gameObject.SetActive(!noScroll);

    if (noScroll) return;
}
```

- 콘텐츠 높이가 View보다 작다면 스크롤바를 자동으로 숨기도록 하였습니다.
- UI를 불필요하게 차지하지 않으며, 스크롤 필요 시에만 표시되는 UX가 가능합니다.

<br><br>

**6) 마우스 휠**
```csharp
private void HandleMouseScroll()
{
    adjustedSensitivity = (limitMaxViewValue == 0) ? sensitivity : sensitivity * (1f / limitMaxViewValue);

    float wheel = Input.GetAxisRaw("Mouse ScrollWheel") * adjustedSensitivity;
    currentScrollValue += reverseWheel ? -wheel : wheel;
    currentScrollValue = Mathf.Clamp(currentScrollValue, minScrollValue, maxScrollValue);
}
```

- 스크롤 민감도는 콘텐츠 길이에 따라 자동 스케일링되며, 빠른·부드러운 스크롤이 모두 가능합니다.
- 휠 방향 반전 옵션도 지원하여 제작자·사용자 경험에 맞게 적용할 수 있습니다.
  
<br><br>

**7) ScrollBar 위치 UI 실시간 반영**
```csharp
private void ApplyScrollbarPosition()
{
    if (scrollbarHandler == null || scrollbarBackground == null) return;

    if (isScrollHorizontal)
    {
        SetScrollBarSizeX();
        float dir = reverseRect ? -1f : 1f;
        scrollbarHandler.anchoredPosition = Vector3.Lerp(scrollbarHandler.anchoredPosition,
                                                         Vector3.left * currentScrollValue * limitMaxBarValue * dir,
                                                         Time.deltaTime * smoothDamp );
    }
    else
    {
        SetScrollBarSizeY();
        float dir = reverseRect ? 1f : -1f;
        scrollbarHandler.anchoredPosition = Vector3.Lerp( scrollbarHandler.anchoredPosition,
                                                          Vector3.up * currentScrollValue * limitMaxBarValue * dir,
                                                          Time.deltaTime * smoothDamp);
    }
}
```

- ScrollValue 값만 바뀌면 Content와 ScrollBar가 동시에 이동하도록 구조를 통일하였습니다.
- UI 표시 상태, 드래그 반응, Wheel 입력이 모두 하나의 변수를 공유하므로 충돌 없이 동작합니다.




<br><br><br><br><br>








## 🎨 Mesh Combine Tool 발생한 빌드 속도 저하

- 프로젝트 중반부, 저는 씬 최적화를 위해 Mesh Combine Tool을 제작하여 여러 개의 모델을 하나의 Mesh로 병합하는 작업을 진행하고 있었습니다.
- 불필요하게 분리된 기하 구조를 통합하고, DrawCall 감소를 기대할 수 있는 작업이였고 실제로 개발 첫 단계에서는 씬 내 게임오브젝트 수가 줄어들며 에디터에서의 Scene 이동 역시 원활해졌습니다.

- **그러나 어느 시점부터 빌드가 이전보다 비정상적으로 느려지기 시작했습니다. 처음에는 단순한 PC 성능 문제라고 생각했지만, 점차 빌드 시간이 몇 분 단위로 증가하더니 결국 프로젝트를 저장하고 다시 여는 데만 수십 분이 소요되는 상황까지 이어졌습니다.**

<br><br>
<hr>

## ⚠  원인 추적 – 예상 외의 범인

- 빌드 속도가 느려진 이유를 파악하기 위해 저는 먼저 Profiler, Frame Debuger  등 여러 요소를 점검했습니다. 그러다 결국 의심해야 할 대상이 명확하게 드러났습니다.

<p align="center"><span style="font-weight: bold;">Scene 파일의 크기가 몇 GB에 달하고 있었습니다.</span></p>

과거 ProBuilder 사용 경험이 떠올랐습니다. 
Mesh를 Scene 내부에 포함한 상태로 방치하면 Binary가 누적되어 파일용량이 기하급수적으로 증가할 수 있다는 점이었습니다.
- 이에 따라 Mesh Combine Tool 내부를 다시 확인했고 Combine할때 Mesh를 Asset으로 저장하지 않은 채 Scene 내부에 그대로 포함시키고 있었습니다.


여러 번의 Combine 작업을 통해 생성된 Mesh 데이터들이 씬 파일 내부에 계속 쌓이고 있었고,
결국 씬을 열 때마다 Unity가 GB 단위 Mesh를 로드할때 초기 로딩 시간이 급증하는, 빌드 속도 저하의 근본 원인이었습니다.


<br><br>
<hr>

## ✅ 해결

문제 해결 방식은 명확했습니다.
Scene에 남아 있는 Combined Mesh를 모두 제거하고 Mesh 데이터를 프리팹 Asset으로 변환하여 외부 저장하도록 Combine Tool을 개선했습니다.

아래는 개선 이후 추가한 코드 방식입니다.

```csharp
 GameObject newGo = new GameObject(parentName);
        newGo.AddComponent<MeshFilter>().sharedMesh = mesh;
        newGo.AddComponent<MeshRenderer>().sharedMaterial = filters[0].GetComponent<MeshRenderer>().sharedMaterial;
        newGo.transform.parent = parent.transform;

        if (!System.IO.Directory.Exists(combineMeshFolderPath))
            System.IO.Directory.CreateDirectory(combineMeshFolderPath);

        int fileIndex = 1;
        string fileName = combineMeshFolderPath + newGo.transform.parent.name + "_" + parentName;
        string meshPath = fileName + ".asset";
        while (System.IO.File.Exists(meshPath))
        {
            fileName += fileIndex;
            meshPath = fileName + ".asset";
            fileIndex++;
        }

        AssetDatabase.CreateAsset(mesh, meshPath);
        AssetDatabase.SaveAssets();
```

- 이후 Scene에서 메시에 해당하는 오브젝트를 정리한 결과
  - 씬 용량 수 GB	-> 수백 MB 이하로 감소
  - 빌드 시간 수 분~수십분	-> 수초~1분 내로 안정화
  - Scene 저장 시 잦은 프리징 -> 안정적인 편집 환경으로 회복


**수 GB 단위의 데이터가 빠져나가자 프로젝트는 다시 쾌적하게 돌아왔습니다.**
**이번 경험을 통해 저는 최적화는 단순히 병합과 제거가 아니라 데이터가 어디에 저장되고 어떻게 관리되는지까지 고려해야 한다는 점을 다시 확실히 배웠습니다.**

---
