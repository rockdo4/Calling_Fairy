using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageModal : ModalBase
{
    public TextMeshProUGUI message;
    public Button Button;

    private void Awake()
    {
        Button.onClick.AddListener(modalPanel.CloseModal);
    }

    public void OpenPopup(string title, string message)
    {
        OpenPopup(title);
        this.message.text = message;
    }

    public override void ClosePopup()
    {
        message.text = "";
        base.ClosePopup();
    }
}
