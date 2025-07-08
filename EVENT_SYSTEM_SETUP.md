# 이벤트 시스템 설정 가이드 (Event System Setup Guide)

이 문서는 코드 리팩토링 이후, 새로운 이벤트 기반 시스템이 정상적으로 동작하기 위해 필요한 유니티 에디터(Unity Editor) 설정 방법을 안내합니다.

---

### 1단계: GameEvent 에셋 생성

먼저, 데이터 변경 신호를 보낼 통로 역할을 하는 Scriptable Object 에셋을 생성해야 합니다.

1.  유니티 에디터의 **Project** 창에서 `Assets/Scripts/System/Events` 폴더로 이동합니다.
2.  폴더의 빈 공간에서 마우스 오른쪽 클릭 -> **Create** -> **System/Events** -> **Game Event**를 선택합니다.
3.  생성된 새 에셋의 이름을 **`OnPlayerDataModified`** 로 변경합니다. 이 에셋이 모든 '플레이어 데이터 변경' 신호의 중심이 됩니다.

### 2단계: SaveLoadSystem에 리스너(EventListener) 설정

다음으로, `SaveLoadSystem`이 `OnPlayerDataModified` 신호를 받아서 `AutoSave` 함수를 실행하도록 설정합니다.

1.  **Hierarchy** 창에서 `SaveLoadSystem` 게임 오브젝트를 찾아 선택합니다.
2.  **Inspector** 창에서 **Add Component** 버튼을 클릭하고 `GameEventListener`를 검색하여 추가합니다.
3.  새로 추가된 `GameEventListener` 컴포넌트에서 아래와 같이 설정합니다:
    *   **Event 필드:** `Project` 창에 있는 `OnPlayerDataModified` 에셋을 이 `Event` 필드로 드래그 앤 드롭합니다.
    *   **Response () 필드:**
        1.  `+` 버튼을 누릅니다.
        2.  `None (Object)` 라고 되어 있는 슬롯에 `SaveLoadSystem` 자기 자신 게임 오브젝트를 `Hierarchy` 창에서 드래그 앤 드롭합니다.
        3.  오른쪽의 `No Function` 드롭다운 메뉴를 클릭하고, **SaveLoadSystem** -> **AutoSave()** 를 선택합니다.

### 3단계: 데이터 소스(FairyCard)에 이벤트 채널 연결

마지막으로, 데이터가 실제로 변경되는 `FairyCard`가 어떤 이벤트 채널로 신호를 보낼지 알려주어야 합니다.

1.  `FairyCard` 데이터를 생성하거나 관리하는 주체(예: `FairyManager`, `Player` 또는 `FairyCard` 프리팹 등)를 찾습니다.
2.  해당 오브젝트의 Inspector 창에서 `FairyCard` 컴포넌트(또는 스크립트)를 찾습니다.
3.  공개(public) 필드로 노출된 **`On Player Data Modified`** 슬롯을 찾습니다. (스크립트에서 `public GameEvent OnPlayerDataModified;` 로 선언한 부분입니다.)
4.  `Project` 창에 있는 **`OnPlayerDataModified`** 에셋을 이 슬롯으로 드래그 앤 드롭하여 연결합니다.

---

이 설정들을 모두 마치면, `FairyCard`의 레벨업, 랭크업 등이 발생할 때마다 `SaveLoadSystem`이 자동으로 데이터를 저장하게 됩니다. 다른 데이터(인벤토리, 퀘스트 등)의 변경 시에도 3단계와 같이 이벤트 채널만 연결해주면 동일한 자동 저장 시스템을 사용할 수 있습니다.
