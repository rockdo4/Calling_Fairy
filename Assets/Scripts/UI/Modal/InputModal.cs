using TMPro;
using UnityEngine.UI;

public class InputModal : ModalBase
{
    public TMP_InputField inputField;
    public TextMeshProUGUI placeholderText;
    public TextMeshProUGUI guidelineText;
    public Button button;

    public void Awake()
    {
        inputField.onValueChanged.AddListener(ValidateInput);
    }

    public void ValidateInput(string inputText)
    {
        button.interactable = IsInputValid(inputText);
    }

    public bool IsInputValid(string input)
    {
        // 정규 표현식으로 문자, 숫자, 밑줄 및 한글만 허용
        // UTF-8에서의 바이트 길이 계산
        int byteCount = System.Text.Encoding.UTF8.GetByteCount(input);
        return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[가-힣A-Za-z0-9_]+$") && byteCount <= 12 && byteCount >= 4;
    }

    public void OpenPopup(string title, string placeholder, string guideline)
    {
        OpenPopup(title);

        placeholderText.text = placeholder;
        guidelineText.text = guideline;

        button.onClick.AddListener(modalPanel.CloseModal);
    }

    public override void ClosePopup()
    {
        Player.Instance.Init(new PlayerSaveData(DataTableMgr.GetTable<PlayerTable>()));
        Player.Instance.Name = inputField.text;
        SaveLoadSystem.SaveData.PlayerData = Player.Instance.SaveData;
        SaveLoadSystem.AutoSave();

        base.ClosePopup();
    }
}
