using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageModal : ModalBase
{
    public TextMeshProUGUI message;

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
