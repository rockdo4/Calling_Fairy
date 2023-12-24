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
    public TextMeshProUGUI expText;
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statText;
    public Image expSlider;
    public Button button;


    public void OpenPopup(string title, string level, int exp, int maxExp, string statName, string stat, string buttonText, UnityAction action, bool isBonus)
    {
        OpenPopup(title);

        bonusMessage.SetActive(isBonus);

        levelText.text = level;
        expText.text = exp + " / " + maxExp;
        statNameText.text = statName;
        statText.text = stat;
        expSlider.fillAmount = (float)exp / maxExp;
        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
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
        expText.text = "";
        statText.text = "";
        expSlider.fillAmount = 0;
    }
}
