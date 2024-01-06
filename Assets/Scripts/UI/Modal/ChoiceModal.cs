using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ������ �г��� �����ϴ� ModalBase�� ��ӹ��� �ʰ� ��� ������ ���������� ����� �� �ֵ��� ���� �г��� �����.
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
        // Set String ������Ʈ�� ����ϸ� GameManager�� �������� ������ �Ǿ� GameManager�� ���� ���¿����� ������ �߻���.
        // ���� ���� �˾��� Ư���� GameManager�� ���� ���¿����� ����� �� �ֵ��� ������ SetString ������Ʈ�� ������� �ʰ� ���� StringTable�� ������.
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
