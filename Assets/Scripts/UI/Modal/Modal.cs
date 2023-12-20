using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Modal : MonoBehaviour
{
    public ModalPanel modalPanel;
    public TextMeshProUGUI title;
    public TextMeshProUGUI message;

    public void OpenPopup(string title, string message)
    {
        gameObject.SetActive(true);
        modalPanel.OpenModal(transform);
        this.title.text = title;
        this.message.text = message;

        modalPanel.OnCloseModal += ClosePopup;
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
        title.text = "";
        message.text = "";
    }
}
