using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalBase : MonoBehaviour
{
    public ModalPanel modalPanel;
    public TextMeshProUGUI title;

    public void OpenPopup(string title)
    {
        gameObject.SetActive(true);
        modalPanel.OpenModal(transform);
        this.title.text = title;

        modalPanel.OnCloseModal += ClosePopup;
    }

    public virtual void ClosePopup()
    {
        title.text = "";
        gameObject.SetActive(false);
    }
}
