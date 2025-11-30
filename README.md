# 🎮 Unity3D Portfolio RPG Game - Chrono Breach


## ⚙️ UML 클래스 다이어그램
프로젝트의 주요 시스템 구조를 나타내는 UML 다이어그램입니다.
<img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UML_F4.drawio.png" alt="UML Diagram" width="600" />



 ### [🧩 **UML 클래스 다이어그램 열기**](https://app.diagrams.net/?url=https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/refs/heads/main/UML_F4.drawio) (ctrl + wheel로 줌 아웃)

## 📹 동영상 링크
**동영상 화질을 4k로 선택하여 시청해주시면 감사합니다.** 
<a href="https://www.youtube.com/watch?v=sTdEx9n8rMI" target="_blank">
  <img src="https://img.youtube.com/vi/sTdEx9n8rMI/maxresdefault.jpg" alt="Unity Portfolio (4K)" style="width:100%;">
</a>

### [🎬 **유튜브 영상 바로 보기**](https://www.youtube.com/watch?v=sTdEx9n8rMI)


---

## 🛠️ 정보

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


# 🎯 설계 목적
초기에는 단순히 모든 적 처치 시 클리어되는 구조만 구현하려 했으나, 던전별로 고유한 목표와 규칙을 제공하면 게임 플레이의 깊이와 다양성이 크게 향상된다고 판단하여 현재의 모듈형·확장형 구조로 발전시켰습니다.  

설계 과정에서는 다음 두 가지를 특히 중점적으로 고려했습니다.  

- **유지보수성**: 던전별 로직 분리 및 독립 관리  
- **확장성**: 데이터만 교체해 다양한 던전 유형 추가 가능  

---

# 🎯 Dungeon 구성 요소 
아래 구성들은 던전 시스템의 핵심 데이터를 간결하게 표현한 구조입니다.  

## Title
- 던전의 이름, 유형, 설명 등  
- 던전을 식별하고 UI 및 시스템에서 활용되는 기본 정보  
- **( Normal, Rush, Protect, Rescuer, Target )**
<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/Title.png" width="650"/> </p>

  
## Category
- DungeonController에서 관리하는 주요 제어 설정  
- 던전 흐름 제어, 웨이브/라운드 진행 로직, 클리어 판정 방식  

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

## Reward
- 클리어 보상 정보  
  - 경험치
  - 명성치
  - 스킬포인트
  - 아이템  
  - 골드  

## MapData
- 던전에 사용되는 맵 프리팹과 환경 데이터  

## SpawnPosition
- 던전 내 위치 정보 구성 요소  
  - 플레이어 시작 위치  
  - Enemy/Boss 스폰 지점  
  - 트리거 이벤트 위치  

## SpawnData 
- 전투 구성에 필요한 모든 스폰 및 라운드 데이터  
  - Enemy/Boss 스폰 리스트
  - Playable AI 리스트
  - 이동 불가 벽 정보
  - 웨이브/라운드 구성   

<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UI/SpawnD1.png" width="650"/> </p>



## ⏳ 트러블 슈팅



## ⏳ 타임라인

아래는 각 주요 장면과 그에 해당하는 시간입니다.

| **시간**  | **내용**                  |
|:---------:|:--------------------------|
| 0:00:00   | **전투 장면 하이라이트**  |
| 0:02:38   | **타이틀 화면**            |
| 0:03:32   | **커맨드창**               |
| 0:08:45   | **연습모드**               |
| 0:12:18   | **카메라 설명**            |
| 0:15:42   | **플레이어 State**         |
| 0:22:50   | **아이템 획득 UI**         |
| 0:28:21   | **장비창**                 |
| 0:31:06   | **잠재능력**               |
| 0:34:06   | **퀵 슬롯**               |
| 0:34:57   | **스킬창**                 |
| 0:36:56   | **스킬 시연**              |
| 0:55:46   | **퀘스트 / 다이어로그**    |
| 1:04:00   | **퀘스트 목록 창**         |
| 1:06:02   | **시즌 1 진행 / 던전 1-1 (일반)**  |
| 1:08:19   | **던전 엔트리 창**         |
| 1:16:12   | **시즌 2 진행 / 던전 2-1 (인질 구출)** |
| 1:27:42   | **던전 2-2 (건물 보호)**               |
| 1:33:01   | **시즌 3 진행 / 던전 3-1 (섬멸)** |
| 1:39:03   | **던전 3-2 (서바이벌 타임어택)**               |
| 1:45:29   | **시즌 4 진행 / 던전 4-1 (타겟 제거)** |
| 1:54:25   | **시즌 5 진행 / 던전 5-1 (목표보호 follow)** |
| 2:03:15   | **던전 5-2 (목표보호 waypoint)**               |
| 2:14:13   | **EditorTool - AiInfo**    |
| 2:16:08   | **EditorTool - Sort & Combine** |
| 2:20:27   | **EditorTool - Effect**    |
| 2:20:55   | **EditorTool - Item**      |
| 2:24:07   | **EditorTool - Potential** |
| 2:25:47   | **EditorTool - Sound**     |
| 2:26:27   | **Editor - Range Projectile Info** |
| 2:30:13   | **END**                    |

---
