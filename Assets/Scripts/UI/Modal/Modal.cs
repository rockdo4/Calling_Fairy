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

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
