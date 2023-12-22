using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Modal : MonoBehaviour
{
    public ModalPanel modalPanel;
    public TextMeshProUGUI title;
    public TextMeshProUGUI message;
    public Button button1;
    public Button button2;

    public void OpenPopup(string title, string message)
    {
        gameObject.SetActive(true);
        modalPanel.OpenModal(transform);
        this.title.text = title;
        this.message.text = message;

        modalPanel.OnCloseModal += ClosePopup;
    }

    //제목, 메시지, 버튼 텍스트, 버튼 이벤트
    public void OpenButtonPopup(string title, string message, string button1, string button2, UnityAction button1Event, UnityAction button2Event)
    {
        gameObject.SetActive(true);
        modalPanel.OpenModal(transform);
        this.title.text = title;
        this.message.text = message;
        this.button1.GetComponentInChildren<TextMeshProUGUI>().text = button1;
        this.button2.GetComponentInChildren<TextMeshProUGUI>().text = button2;

        modalPanel.OnCloseModal += CloseButtonPopup;
        this.button1.onClick.AddListener(button1Event);
        this.button2.onClick.AddListener(button2Event);
    }

    public void CloseButtonPopup()
    {
        gameObject.SetActive(false);
        title.text = "";
        message.text = "";
        button1.GetComponentInChildren<TextMeshProUGUI>().text = "";
        button2.GetComponentInChildren<TextMeshProUGUI>().text = "";
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
        title.text = "";
        message.text = "";
    }
}
