# BennieSurvivor

Unity 6와 C#으로 개발하는 2D 탑다운 Survivors-like 액션 팬게임입니다.
개인 학습과 포트폴리오, 무료 비상업적 배포를 목적으로 합니다.

## 실행 방법

1. 저장소를 복제합니다.
2. Unity Hub에서 프로젝트 폴더를 추가합니다.
3. Unity **6000.0.58f1**으로 열고 패키지와 에셋 임포트가 끝날 때까지 기다립니다.
4. `Assets/Scenes/SampleScene.unity`를 열고 Play를 누릅니다.
5. 방향키 또는 WASD로 이동합니다.

## 현재 구현

- Rigidbody2D 기반 탑다운 이동과 대각선 속도 정규화
- 이동 방향에 따른 스프라이트 좌우 반전
- Animator의 `Speed` 파라미터를 통한 Idle/Run 전환
- 4프레임 Idle(6 FPS), Run(12 FPS) 반복 애니메이션
- 플레이어 추적 카메라와 20×20 바닥 9장을 재사용하는 무한 숲 배경
- 누루링 Idle 애니메이션 및 거리 제한 없는 추적
- 공통 Health 컴포넌트와 체력 0에서 오브젝트 비활성화
- 누루링 접촉 중 일정 간격의 피해, 접촉 해제 시 피해 중단
- 화면 바깥에서 1초 간격 몬스터 생성 및 생성 직후 추적 대상 연결
- 도끼 휘두르기 4프레임 이펙트 준비 (공격 연결 전)

## 프로젝트 관리

`Assets`, `Packages`, `ProjectSettings` 및 에셋의 `.meta` 파일을 함께 관리합니다.
`Library`, `Temp`, `Logs`, `UserSettings`와 IDE 자동 생성 파일은 제외합니다.
개발 원칙과 다음 작업은 `CODEX_CONTEXT.md`를 참고하세요.

## 확인할 사항

- 플레이어 피격 무적 시간은 0.5초입니다. 자동 공격과 공격 판정은 아직 구현하지 않았습니다.
- 캐릭터 기본 규격은 프레임당 128×128px, PPU 128, Scale (1, 1, 1)입니다. Point 필터와 무압축을 권장합니다.
- 접촉 중 체력 감소, 접촉 해제 시 피해 중단, 체력 0에서 베니 비활성화는 사용자가 Play 모드에서 확인했습니다. 폴더 정리 후 재임포트와 빌드는 별도 확인이 필요합니다.

## 에셋 폴더 구조

- `Assets/01.Script/`: `Player`, `Enemies`, `Common`, `Camera`, `World` 역할별 스크립트
- `Assets/02.Image/`: `Bennie`, `Nururing`, `Environment`, `Effects` 이미지
- `Assets/03.Animation/`: `Bennie`, `Nururing`, `Effects` 애니메이션과 컨트롤러
- `Assets/04.Prefab/`: `Player`, `Enemies` 프리팹
- `Assets/Scenes/`: 게임 씬
- `Assets/Settings/`: 렌더링 및 프로젝트 에셋 설정

에셋을 이동할 때는 대응하는 `.meta`를 함께 이동해 GUID 참조를 유지합니다.

## 비공식 팬 프로젝트

이 프로젝트는 **트릭컬 리바이브의 비공식 팬게임**이며 공식 게임이나 공식 배포물이 아닙니다.
원작 IP의 권리는 해당 권리자에게 있습니다.
공식 리소스를 추출하거나 복제하지 않고 새로 제작한 팬아트 리소스를 사용하는 것을 제작 원칙으로 합니다.
무료·비상업적 프로젝트이며, 이 저장소는 원작 IP에 대한 사용 권한을 부여하지 않습니다.

