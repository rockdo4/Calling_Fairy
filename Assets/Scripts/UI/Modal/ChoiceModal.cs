using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 기존의 패널을 공유하는 ModalBase를 상속받지 않고 모든 씬에서 독립적으로 사용할 수 있도록 고유 패널을 사용함.
public class ChoiceModal : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI message;
    [SerializeField]
    private Button yesButton;
    private TextMeshProUGUI yesButtonText;
    [SerializeField]
    private Button noButton;
    private TextMeshProUGUI noButtonText;

    public void Awake()
    {
        // Set String 컴포넌트를 사용하면 GameManager에 의존성을 가지게 되어 GameManager가 없는 상태에서는 에러가 발생함.
        // 게임 종료 팝업의 특성상 GameManager가 없는 상태에서도 사용할 수 있도록 기존의 SetString 컴포넌트를 사용하지 않고 직접 StringTable을 참조함.
        var stringTable = DataTableMgr.GetTable<StringTable>();
        yesButtonText = yesButton.GetComponentInChildren<TextMeshProUGUI>();
        yesButtonText.text = stringTable.dic[3].Value;
        noButtonText = noButton.GetComponentInChildren<TextMeshProUGUI>();
        noButtonText.text = stringTable.dic[4].Value;
    }

    public void OpenPopup(string title, string message, UnityAction yesAction, UnityAction noAction)
    {
        gameObject.SetActive(true);
        this.title.text = title;
        this.message.text = message;

        var stringTable = DataTableMgr.GetTable<StringTable>();
        yesButtonText.text = stringTable.dic[3].Value;
        noButtonText.text = stringTable.dic[4].Value;

        //yesButton.onClick.RemoveAllListeners();
        if (yesAction != null)
        {
            yesButton.onClick.AddListener(yesAction);
        }
        yesButton.onClick.AddListener(ClosePopup);

        //noButton.onClick.RemoveAllListeners();
        if (noAction != null)
        {
            noButton.onClick.AddListener(noAction);
        }
        noButton.onClick.AddListener(ClosePopup);
    }

    public virtual void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
