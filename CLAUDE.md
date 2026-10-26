# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 프로젝트 개요

Unity **6000.3.22f1** (URP 17.3) 기반 3D FPS 게임. 메인 씬은 `Assets/01.Scenes/GameScene.unity`.
직접 작성하는 게임 코드는 모두 `Assets/02.Scripts/`에 있으며, 별도 asmdef 없이 `Assembly-CSharp`로 컴파일된다.
`Assets/FractureTool`, `Assets/BloodDecalsAndEffects`, `Assets/TextMesh Pro`, `Assets/Modules/Submodules/Pools`(Redcode.Pools)는 외부 에셋/라이브러리이므로 특별한 요청이 없으면 수정하지 않는다. `Assets/02.Scripts/Example`은 학습용 예제 코드다.

## 명령어

빌드·실행·테스트는 Unity 에디터에서 진행한다(프로젝트에 작성된 테스트 코드는 없음). CLI에서 사용하는 명령은 다음과 같다.

```bash
# 로컬 도구(Husky.Net) 설치 — 클론 후 최초 1회
dotnet tool restore
dotnet husky install

# 포맷 검사 (pre-commit 훅과 동일)
dotnet format 3DFPSGAME.sln --verify-no-changes --severity warn --include Assets/02.Scripts/

# 포맷 자동 수정
dotnet format 3DFPSGAME.sln --severity warn --include Assets/02.Scripts/
```

`.sln`/`.csproj`는 Unity가 생성하는 파일이므로, 새 스크립트를 추가한 직후에는 에디터가 프로젝트 파일을 재생성해야 `dotnet format`에 반영된다.

## Git 규칙

- **commit-msg 훅**: 커밋 메시지는 `type: 설명` 형식이어야 한다. 허용 type: `asset | test | build | chore | docs | feat | fix | refactor | revert | design` (스코프 `type(scope):`, `!` 허용). 기존 커밋은 영어 소문자 설명을 사용한다 (예: `feat: implement mini map zoom-in and zoom-out`).
- **pre-commit 훅**: 위 `dotnet format --verify-no-changes`를 실행하므로 포맷 위반이 있으면 커밋이 실패한다.
- **CI** (`.github/workflows/gemini-commit-review.yml`): `main`에 `Assets/02.Scripts/**/*.cs` 변경이 push되면 Gemini가 `Docs/CodeReviewGuide.md` 기준으로 커밋을 리뷰하고 커밋 코멘트를 남긴다.

## 코드 리뷰/작성 기준

`Docs/CodeReviewGuide.md`가 이 프로젝트의 리뷰 기준이다. 핵심만 요약하면:
- 의미 있는 매직 넘버는 `const` 또는 `[SerializeField]`로 분리 (0, 1, -1 등 관용적인 값은 예외).
- "묻지 말고 시켜라": 외부에서 상태를 조회해 판단하기보다 객체에 행동을 요청 (`TakeDamage()` 등).
- SOLID는 적용하되 작은 코드에 불필요한 추상화를 강요하지 않는다.
- `Update()` 내 할당/`GetComponent`/`Find` 반복, 이벤트 구독 해제 누락, 풀링 필요성 등 Unity 생명주기 문제를 확인한다.

## 코드 스타일

- private 필드는 `_camelCase`, Inspector 노출은 `[SerializeField] private`, 외부 공개는 읽기 전용 프로퍼티(`public float Health => _currentHealth;`).
- 코루틴 메서드명은 `Xxx_Coroutine` 접미사.
- UI 컴포넌트 클래스는 `UI_` 접두사 (예: `UI_MiniMap`).
- 로그는 `GameLogger.Log()`를 사용한다. `[Conditional("ENABLE_LOG")]`이므로 Standalone 스크립팅 심볼 `ENABLE_LOG`가 없으면 호출 자체가 제거된다 (Warning/Error는 항상 출력).
- 공용 상수는 `GameConfig` 정적 클래스에 둔다.

## 아키텍처

### 싱글턴 매니저
`GameManager`, `UIManager`, `ScreenEffectManager`, `BombPoolManager`는 `Awake()`에서 `_instance`를 설정하는 수동 싱글턴(`Instance` 프로퍼티)이다. `GameManager`와 `ScreenEffectManager`는 `DontDestroyOnLoad`. 다른 클래스는 대부분 `UIManager.Instance?.Refresh...()`처럼 null 조건 연산자로 호출한다.

### 게임 상태 흐름
`GameManager`가 `GameState`(Ready → Playing → GameOver)를 관리한다. `Start()`의 코루틴이 `_startTime` 후 Playing으로 전환하며, **플레이어/좀비/커서 등 대부분의 `Update()`는 `if (!GameManager.Instance.IsPlaying()) return;`으로 시작한다.** 새 게임플레이 컴포넌트도 이 패턴을 따른다. 플레이어 체력이 0이 되면 `Player`가 `GameManager.GameOver()`를 호출한다.

### 데미지 시스템
`IDamageable.TakeDamage(DamageInfo)`가 공통 계약이며 `DamageInfo`는 `Value / HitPoint / HitNormal` 구조체다. 구현체: `Player`, `Zombie`, `Drum`, `Box`. 공격 측(`PlayerGunFire` 레이캐스트, `Bomb` OverlapSphere, `Zombie` 공격)은 `GetComponent<IDamageable>()?.TakeDamage(...)`로 호출한다.

### 플레이어
역할별 컴포넌트로 분리되어 있다: `PlayerMove`(CharacterController 이동/점프/달리기, 스태미나 소모), `PlayerRotate`, `PlayerGunFire`(히트스캔, 재장전 코루틴), `PlayerBombFire`(차지 후 투척, `BombPoolManager` 사용), `PlayerStat`(체력/스태미나 데이터, 변경 시 `UIManager`에 갱신 요청), `Player`(`IDamageable` 진입점).

### 몬스터
`Zombie`는 `ZombieState` enum + `switch` 기반 FSM(Idle/Patrol/Trace/Comeback/Attack/Hit/Death). 상태 전환 시 `Debug.Log($"{_state} -> {next}")`를 남기는 패턴을 사용한다. 이동은 `CharacterController.SimpleMove`, 플레이어는 `"Player"` 태그로 찾는다.

### 오브젝트 풀
`Redcode.Pools`(`Pool.Create(prefab, size, container)`, `Get()`, `Take()`)를 사용한다. 현재 `Bomb`이 풀링되며 충돌 시 `BombPoolManager.Instance.Release(this)`로 반환된다.

### 입력과 커서
입력은 **레거시 Input Manager**(`Input.GetButton("Fire1")`, `"Reload"`, `"Sprint"` 등, Project Settings의 Input Manager 축 이름 사용)로 처리한다. Active Input Handling은 Both이며 `InputSystem_Actions.inputactions`는 현재 사용되지 않는다.
`CursorManager`는 기본적으로 커서를 잠그고, Left Alt를 누르는 동안 해제한다. 사격·회전 등은 `Cursor.visible`이 true일 때 동작하지 않도록 막는다.

### 미니맵
`MiniMapCamera`가 플레이어를 따라가는 탑다운 직교 카메라(RenderTexture 출력)이며 `ZoomIn()/ZoomOut()`으로 `orthographicSize`를 조절한다. `UI_MiniMap`의 버튼 핸들러가 이를 호출한다.
