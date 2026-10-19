# CODEX_CONTEXT.md

## 1. 프로젝트 개요

- 프로젝트명: **BennieSurvivor**
- 엔진: **Unity 6**
- 언어: **C#**
- 장르: **2D 탑다운 Survivors-like 액션**
- 형태: **트릭컬 리바이브 비공식 팬게임**
- 목적: 개인 학습 / 포트폴리오 / 무료 비상업적 배포
- GitHub 저장소: `karle9412/BennieSurvivor`

이 프로젝트는 공식 게임이나 공식 배포물이 아니며, 원작 IP의 권리는 해당 권리자에게 있다.

---

## 2. Codex 작업 원칙

이 프로젝트를 수정할 때 다음 기준을 우선한다.

1. 기존 코드와 프로젝트 구조를 먼저 확인한 뒤 수정한다.
2. 사용자가 직접 작성한 코드를 불필요하게 전면 재작성하지 않는다.
3. 가능한 한 초보자가 이해하기 쉬운 구조와 이름을 유지한다.
4. 한 번에 지나치게 많은 시스템을 추가하지 않는다.
5. 변경 이유를 설명할 때는 Unity/C# 관점에서 구체적으로 설명한다.
6. 큰 구조 변경이나 파일 대량 삭제가 필요하면 먼저 사용자에게 설명한다.
7. Unity 자동 생성 폴더는 Git에 포함하지 않는다.
8. 팬게임 관련 공식 리소스를 직접 추출하거나 복제하는 방향으로 작업하지 않는다.

---

## 3. 현재 플레이어 이동 구조

플레이어 이동은 다음 역할 분리 방식을 사용한다.

- `Update()`
  - 키보드 입력 읽기
  - 이동 방향 계산
  - Animator 파라미터 갱신
- `FixedUpdate()`
  - `Rigidbody2D`를 이용한 실제 이동 처리

현재 입력은 `Input.GetAxisRaw()`를 사용한다.

이유:
- `Input.GetAxis()`는 입력을 부드럽게 보간하기 때문에 키에서 손을 뗀 뒤에도 잠시 입력값이 남을 수 있다.
- 이 게임은 즉각적인 탑다운 이동 반응이 더 적합하므로 `GetAxisRaw()`를 사용한다.

현재 기준 코드 형태:

```csharp
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private float horizontalInput;
    private float verticalInput;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        if (movement.magnitude > 0)
            movement.Normalize();

        _rb.MovePosition(
            _rb.position + movement * moveSpeed * Time.fixedDeltaTime
        );
    }
}
```

향후 Animator 연동 시에는 이동 벡터를 필드로 분리하는 구조를 우선 고려한다.

예:

```csharp
private Vector2 _movement;
private Animator _animator;

private void Awake()
{
    _rb = GetComponent<Rigidbody2D>();
    _animator = GetComponent<Animator>();
}

private void Update()
{
    float horizontalInput = Input.GetAxisRaw("Horizontal");
    float verticalInput = Input.GetAxisRaw("Vertical");

    _movement = new Vector2(horizontalInput, verticalInput).normalized;

    _animator.SetFloat("Speed", _movement.magnitude);
}

private void FixedUpdate()
{
    _rb.MovePosition(
        _rb.position + _movement * moveSpeed * Time.fixedDeltaTime
    );
}
```

---

## 4. Rigidbody2D 설정 방향

탑다운 2D 플레이어 기준:

- `Gravity Scale = 0`
- 캐릭터가 충돌로 회전할 필요가 없으면:
  - `Constraints -> Freeze Rotation Z`

이동은 `Transform.Translate()` 대신 `Rigidbody2D.MovePosition()`을 사용한다.

---

## 5. 현재 애니메이션 구조

현재 준비 중인 상태:

```text
Animator
├─ Idle
└─ Run
```

Animator 파라미터:

```text
Speed : float
```

전환 조건:

```text
Idle -> Run
Speed Greater 0.1
Has Exit Time OFF

Run -> Idle
Speed Less 0.1
Has Exit Time OFF
```

추가 기준:

- `Transition Duration`은 픽셀 애니메이션 특성상 `0` 또는 아주 작은 값부터 시작한다.
- Idle/Run은 입력에 즉시 반응해야 하므로 `Has Exit Time`을 사용하지 않는다.
- Attack, Hit, Death처럼 모션이 끝나야 하는 애니메이션에서는 필요에 따라 `Has Exit Time`을 사용할 수 있다.

---

## 6. 현재 픽셀 애니메이션 규격

현재 Bennie 캐릭터용 Idle/Run 애니메이션은:

- 4프레임
- 1행 4열
- 각 셀 크기: **128 x 128**
- 최종 시트 크기: **512 x 128**
- 투명 배경 PNG
- Unity:
  - `Texture Type = Sprite (2D and UI)`
  - `Sprite Mode = Multiple`
  - `Sprite Editor`
  - `Slice -> Grid By Cell Count`
  - Column = 4
  - Row = 1

애니메이션 샘플 시작값:

- Idle: 약 4~6 Samples
- Run: 약 8~12 Samples

실제 체감에 따라 조절한다.

---

## 7. Bennie 캐릭터 비주얼 기준

현재 주인공은 **베니(Bennie)** 기반 팬아트 캐릭터다.

인지 가능한 특징:

- 크고 둥근 곰 귀
- 짧은 갈색/회갈색 단발
- 앞머리 쪽 어두운 부분과 밝은 포인트
- 둥글고 귀여운 얼굴
- 볼의 홍조
- 갈색/크림 계열 복장
- 작은 앞주머니
- 큰 회색 도끼
- 작고 통통한 치비 실루엣

Idle 기준:

- 오른쪽을 바라봄
- 도끼를 어깨에 걸침
- 편안하고 밝은 표정
- 중앙의 어색한 삐침머리/cowlick은 사용하지 않음

Run 기준:

- 오른쪽을 바라봄
- 도끼를 뒤/어깨 쪽에 들고 달림
- **활짝 웃는 표정 금지**
- 중립적이거나 약간 집중한 귀여운 표정
- 화난 인상도 피함

---

## 8. 픽셀아트 제작 기준

이 프로젝트의 캐릭터 스프라이트는 단순한 “픽셀풍 일러스트”가 아니라 실제 게임용 픽셀 스프라이트를 목표로 한다.

필수 기준:

- 큰 사각 픽셀이 눈에 보일 정도의 뚜렷한 도트감
- 안티앨리어싱 금지
- 부드러운 회화식 그라데이션 금지
- 제한된 색상 팔레트
- 단순한 2~3단계 명암
- 형태를 단순화하고 실루엣이 쉽게 읽히게 제작
- 프레임별 캐릭터 크기와 중심점이 크게 흔들리지 않게 유지
- 발 위치의 기준선 통일
- 셀 경계를 침범하지 않게 충분한 투명 여백 확보
- 리사이즈가 필요하면 Nearest Neighbor 사용

Sprite Sheet 제작 시 이미지 생성 결과의 해상도를 그대로 신뢰하지 말고 실제 파일 크기를 확인한다.

필요하면 각 프레임을 후처리해서 정확히 `128 x 128` 셀에 배치한다.

---

## 9. 팬게임 / IP 관련 제작 원칙

트릭컬 기반 팬게임 에셋 작업 시:

1. 공식 스프라이트를 직접 추출하거나 그대로 사용하지 않는다.
2. 공식 리소스의 정확한 픽셀 배열이나 포즈를 그대로 복제하지 않는다.
3. 캐릭터의 식별 가능한 특징은 유지하되 새로운 구도와 형태로 제작한다.
4. 공식 로고를 사용하지 않는다.
5. 공식 작품으로 오인될 수 있는 표현을 피한다.
6. 공식 BGM, 음성, UI, 배경 등을 추출해 사용하지 않는다.
7. 새로 제작된 팬아트 리소스를 사용한다.
8. 무료 / 비상업적 프로젝트 방향을 유지한다.

README나 배포 페이지에는 비공식 팬 프로젝트임을 명확히 표시한다.

---

## 10. Git / GitHub 기준

GitHub 저장소:

```text
karle9412/BennieSurvivor
```

추적할 주요 Unity 폴더:

```text
Assets/
Packages/
ProjectSettings/
```

Git에서 제외할 대표 폴더:

```text
Library/
Temp/
Logs/
obj/
Build/
Builds/
UserSettings/
```

또한 IDE 자동 생성 파일도 보통 제외한다.

예:

```text
.vs/
*.csproj
*.sln
```

---

## 11. 권장 프로젝트 루트 구조

```text
BennieSurvivor/
├─ Assets/
├─ Packages/
├─ ProjectSettings/
├─ CODEX_CONTEXT.md
├─ README.md
└─ .gitignore
```

---

## 12. 향후 개발 방향

다음 시스템을 순차적으로 구현하는 방향이 적합하다.

- Player 이동 완성
- Idle / Run 애니메이션 연동
- 방향 전환 처리
- 몬스터 이동
- 자동 타게팅
- 자동 공격
- 투사체
- 피해 / 체력
- 몬스터 스폰
- 경험치 / 레벨업
- 무기 및 스킬 성장
- 오브젝트 풀링
- UI
- 게임 오버
- 사운드

한 번에 모든 시스템을 만들기보다 현재 기능을 완성하고 테스트한 뒤 다음 단계로 진행한다.

---

## 13. 현재 우선순위

Codex가 프로젝트를 처음 열었을 때 우선 확인할 것:

1. Unity 프로젝트가 정상적으로 열리는지
2. `Player.cs` 현재 코드 확인
3. Player 오브젝트에 `Rigidbody2D`와 `Animator`가 연결되어 있는지
4. Idle / Run Animation Clip 존재 여부
5. Animator의 `Speed` 파라미터 존재 여부
6. Idle ↔ Run Transition 설정 확인
7. Bennie Sprite Sheet Import / Slice 상태 확인
8. 씬 실행 후 이동과 애니메이션이 정상 동작하는지 확인

현재 단계에서는 새로운 시스템 추가보다 **플레이어 이동 + 애니메이션을 안정적으로 완성하는 것**이 우선이다.

## 14. 공통 캐릭터 이미지 규격 (사용자 지정)

- 모든 신규 캐릭터 기본 PNG는 실제 128 x 128 픽셀로 제작한다.
- 모든 애니메이션은 프레임당 128 x 128, 4프레임이면 512 x 128로 통일한다.
- Pixels Per Unit은 128, Transform Scale은 (1, 1, 1)이다. 큰 생성 이미지를 서로 다른 PPU로 보정해 납품하지 않는다.
- 베니 Idle 첫 프레임의 불투명 영역은 약 94 x 95 픽셀, 하단 기준선은 이미지 위에서 y=116이다. 기본 캐릭터는 이 영역에 비례 축소하고 같은 하단 기준선에 배치한다. 넓고 낮은 캐릭터의 종횡비는 유지한다.
- 크기 정규화 시 Nearest Neighbor와 투명 여백을 사용하며 실제 파일 해상도를 검증한다.
- 누루링 정규화 파일: Assets/02.Image/Nururing_Base_128.png 및 Nururing_Idle_128.png. 기존 대형 파일과 구분한다.
