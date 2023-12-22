using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakLimitModal : ModalBase
{
    public Image beforeGrade;
    public Image afterGrade;
    public TextMeshProUGUI beforeGradeText;
    public TextMeshProUGUI afterGradeText;

    public void OpenPopup(string title, string beforeGrade, string afterGrade)
    {
        OpenPopup(title);

        this.beforeGradeText.text = beforeGrade;
        this.afterGradeText.text = afterGrade;
        this.beforeGrade.sprite = Resources.Load<Sprite>($"UIElement/{beforeGrade}star");
        this.afterGrade.sprite = Resources.Load<Sprite>($"UIElement/{afterGrade}star");
    }

    public override void ClosePopup()
    {
        base.ClosePopup();

        beforeGradeText.text = "";
        afterGradeText.text = "";
        beforeGrade.sprite = null;
        afterGrade.sprite = null;
    }
}
