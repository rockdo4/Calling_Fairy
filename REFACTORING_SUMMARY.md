# 리팩토링 요약 (Refactoring Summary)

이 문서는 최근 진행된 주요 코드 리팩토링 내용을 요약합니다. 각 변경 사항은 코드의 유지보수성, 확장성 및 성능 개선을 목표로 합니다.

---

## 1. UI 스택 구조 리팩토링

기존의 부모-자식 참조 기반 UI 관리 방식을 스택 기반으로 변경하여 UI 네비게이션 로직을 단순화하고 안정성을 높였습니다.

-   **`UI.cs`**: `parentWindow` 및 `childrenWindow` 필드를 제거하고, `ActiveUI()` 및 `NonActiveUI()` 메소드가 `UIManager`의 `PushUI()` 및 `PopUI()`를 호출하도록 변경되었습니다.
-   **`UIManager.cs`**: `CurrentUI` 프로퍼티를 `Stack<UI>` 기반으로 변경하고, `PushUI(UI ui)` 및 `PopUI()` 메소드를 추가하여 UI 스택을 관리하도록 구현되었습니다. 기존 `DirectOpenUI` 및 `ReturnHome` 메소드도 스택 기반으로 수정되었습니다.

---

## 2. `FairyCard` 스탯 계산 방식 리팩토링 (Dirty Flag 패턴)

`FairyCard`의 `FinalStat` 계산이 필요할 때마다 수행되도록 "Dirty Flag" 패턴을 도입하여, 스탯 갱신 로직의 정확성을 높이고 불필요한 재계산을 방지하여 성능을 최적화했습니다.

-   **`FairyCard.cs`**: 
    -   `private bool isStatDirty` 플래그와 `private Stat _finalStat` 필드가 추가되었습니다.
    -   `FinalStat` 프로퍼티의 `get` 접근자는 `isStatDirty`가 `true`일 때만 스탯을 재계산하고 캐시하도록 변경되었습니다.
    -   기존 `SetStat()` 메소드의 이름이 `RecalculateAndCacheStats()`로 변경되고 `private`으로 선언되었습니다.
    -   스탯 변경을 유발하는 모든 메소드(`LevelUp`, `RankUp`, `SetEquip` 등)에서 `SaveLoadSystem.AutoSave()` 호출 대신 `isStatDirty = true;`를 설정하도록 변경되었습니다.

---

## 3. 저장/로드 시스템을 위한 이벤트 시스템 도입

데이터 객체가 `SaveLoadSystem`에 직접 의존하는 대신, 이벤트 시스템을 통해 데이터 변경을 알리도록 하여 결합도를 낮추고 유지보수성을 향상시켰습니다.

-   **`GameEvent.cs`**: 범용 스크립터블 오브젝트 기반 이벤트 채널 스크립트가 생성되었습니다. (`Assets/Scripts/System/Events/GameEvent.cs`)
-   **`GameEventListener.cs`**: `GameEvent`를 구독하고 `UnityEvent`를 실행하는 범용 리스너 컴포넌트가 생성되었습니다. (`Assets/Scripts/System/Events/GameEventListener.cs`)
-   **`FairyCard.cs`**: `SaveLoadSystem.AutoSave()`를 호출하던 모든 부분이 `OnPlayerDataModified.Raise();` (새로 추가된 `public GameEvent OnPlayerDataModified` 필드를 통해)를 호출하도록 변경되었습니다.
-   **유니티 에디터 설정 필요**: `OnPlayerDataModified`라는 `GameEvent` 에셋을 생성하고, `SaveLoadSystem` 게임 오브젝트에 `GameEventListener` 컴포넌트를 추가하여 `OnPlayerDataModified` 이벤트를 구독하고 `SaveLoadSystem.AutoSave()` 메소드를 연결해야 합니다.

---

## 4. UI 데이터 바인딩 시스템 리팩토링

UI 요소 하나당 하나의 스크립트가 존재하던 비효율적인 구조를 범용 데이터 바인딩 시스템으로 대체하여 코드 중복을 제거하고 UI 관리의 유연성을 높였습니다.

-   **`UIBindingType.cs`**: UI에 표시될 다양한 데이터 유형을 정의하는 열거형(enum)이 생성되었습니다. (`Assets/Scripts/UI/System/UIBindingType.cs`)
-   **`DataBinder.cs`**: `UIBindingType`에 따라 `TextMeshProUGUI`, `Image`, `Slider` 등 다양한 UI 컴포넌트에 데이터를 바인딩하는 범용 스크립트가 생성되었습니다. (`Assets/Scripts/UI/System/DataBinder.cs`)
-   **`UIView.cs`**: `DataBinder` 컴포넌트들을 관리하고, `FairyCard` 또는 `Equipment` 모델을 받아 모든 `DataBinder`에게 UI 갱신을 지시하는 뷰 컨트롤러 스크립트가 생성되었습니다. (`Assets/Scripts/UI/System/UIView.cs`)
-   **`FairyGrowthUI.cs`**: 
    -   기존의 직접적인 UI 컴포넌트 제어 로직이 제거되고, `UIView` 컴포넌트를 통해 데이터 바인딩을 요청하는 방식으로 대폭 단순화되었습니다.
    -   시뮬레이션 및 복잡한 계산 로직과 관련된 필드 및 메소드는 현재 단계에서는 유지되었습니다.
-   **기존 UI 스크립트 삭제**: 다음 스크립트들이 새로운 데이터 바인딩 시스템으로 대체되어 삭제되었습니다:
    -   `CardInfoBox.cs`
    -   `CardInfoElementals.cs`
    -   `CardInfoElementals2.cs`
    -   `EquipInfoBox.cs`
    -   `EquipInfoBox2.cs`
    -   `StatInfoBox.cs`
-   **유니티 에디터 설정 필요**: 
    -   `FairyGrowthUI_Refactored.cs` 파일의 이름을 `FairyGrowthUI.cs`로 변경하고, 기존 `FairyGrowthUI.cs` (만약 남아있다면)는 삭제하고 `.meta` 파일도 삭제해야 합니다.
    -   `FairyGrowthUI` 게임 오브젝트의 Inspector에서 `UIView` 타입의 public 필드들(예: `leftCardView`, `statInfoView` 등)에 해당 UI 패널 게임 오브젝트들을 드래그 앤 드롭하여 연결해야 합니다.
    -   삭제된 스크립트들이 붙어있던 각 UI 요소 게임 오브젝트에 `DataBinder` 컴포넌트를 추가하고, Inspector에서 `bindingType`을 설정하며 해당 UI 컴포넌트(Text, Image, Slider)를 연결해야 합니다.

---

이 문서에 설명된 변경 사항들은 프로젝트의 코드 품질을 크게 향상시켰습니다. 추가적인 문의사항이나 도움이 필요하시면 언제든지 알려주십시오.
