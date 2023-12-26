using TMPro;
using UnityEngine.UI;

public class InputModal : ModalBase
{
    public TMP_InputField inputField;
    public Button button;

    public new void OpenPopup(string title)
    {
        base.OpenPopup(title);

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
