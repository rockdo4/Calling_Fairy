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

    public void OpenPopup()
    {
        gameObject.SetActive(true);
        modalPanel.OpenModal(transform);
        modalPanel.OnCloseModal += ClosePopup;
    }

    //제목, 메시지, 버튼 텍스트, 버튼 이벤트
    public void OpenButtonPopup()
    {
        gameObject.SetActive(true);
        modalPanel.OpenModal(transform);
        modalPanel.OnCloseModal += CloseButtonPopup;
    }

    public void CloseButtonPopup()
    {
        gameObject.SetActive(false);
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
