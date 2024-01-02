using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LvUpModal : ModalBase
{
    public GameObject bonusMessage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statText;
    public Gauge gauge;
    public Button button;


    public void OpenPopup(string title, string level, int exp, int maxExp, string statName, string stat, string buttonText, UnityAction action, bool isBonus)
    {
        OpenPopup(title);

        bonusMessage.SetActive(isBonus);

        levelText.text = level;
        statNameText.text = statName;
        statText.text = stat;
        gauge.SetGauge(exp, maxExp);
        button.onClick.AddListener(modalPanel.CloseModal);

        if (action != null)
        {
            button.onClick.AddListener(action);
        }
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
        button.onClick.RemoveAllListeners();
        levelText.text = "";
        gauge.SetGauge(0, 0);
        statText.text = "";
    }
}
