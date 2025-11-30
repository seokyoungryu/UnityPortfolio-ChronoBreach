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

## ⏳ 핵심 기술 파트

# 📌던전
** 던전 UML **
<img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UML_D1.png" alt="UML Diagram" width="600" />

**구현의도**
초기에는 모든 적을 처치하면 클리어되는 단순한 구조의 던전만 구현할 계획이었습니다.
그러나 던전별로 고유한 클리어 조건을 제공하면 게임 플레이의 다양성과 확장성이 크게 향상될 것이라고 판단하여, 다양한 클리어 방식을 지원하는 형태로 시스템을 확장하게 되었습니다.
이 과정에서 가장 중요하게 고려한 부분은 유지보수성과 확장성으로, 던전 로직을 유연하고 독립적으로 관리할 수 있도록 설계하는 데 중점을 두었습니다.



📌 Dungeon System
모듈형 확장 구조의 유연한 던전 아키텍처
<p align="center"> <img src="https://raw.githubusercontent.com/seokyoungryu/UnityPortfolio-ChronoBreach/main/UML_D1.png" width="650"/> </p>
🎯 설계 의도

초기에는 모든 적을 처치하면 클리어되는 단순한 구조의 던전만 구현할 계획이었으나 
던전별로 **다양한 클리어 조건을 제공**하면 게임 플레이의 다양성과 확장성이 크게 향상될 것이라고 판단하여
다양한 클리어 방식을 지원하는 형태로 시스템을 확장하게 되었습니다.
이 과정에서 가장 중요하게 고려한 부분은 **유지보수성과 확장성**으로
던전 로직을 유연하고 독립적으로 관리할 수 있도록 설계하는 데 중점을 두었습니다.

🧩 구조 요약
✔️ 1. 데이터 기반(Data-Driven) 구성

각 던전은 여러 ScriptableObject를 조합해 완성됩니다.

DungeonTitle : 던전의 기본 정보

Condition Module : 클리어 방식(전멸, 생존, 보호, 타임어택 등)

Spawn Module : 라운드·웨이브·적 그룹 배치

Function Module : 보상, 문 열림, 컷신 처리 등

Map / Enemy Info : 맵 배치, 적 구성

새로운 던전 추가 시 코드 변경 없이
데이터 조합만으로 제작 가능합니다.

🎮 클리어 방식 모듈

던전별 목표를 독립 클래스로 분리하여 필요에 따라 조합:

전멸(Elimination)

특정 목표 제거(Target)

NPC/오브젝트 보호(Protect)

생존/타임어택(Survival)

지점 도달(Reach Point)

🧨 스폰 구조

스폰 시스템은 세분화되어 있어 다양한 전투 패턴 연출이 가능합니다.

Round → Wave → Group → Enemy

시간 기반, 조건 기반, 랜덤 스폰 등 유연한 구성

데이터만 바꿔도 다른 스테이지 패턴 생성 가능


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
