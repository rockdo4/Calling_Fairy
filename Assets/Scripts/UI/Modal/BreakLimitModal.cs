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

        beforeGradeText.text = beforeGrade;
        afterGradeText.text = afterGrade;
        this.beforeGrade.sprite = Resources.Load<Sprite>($"UIElement/{beforeGrade}star");
        this.afterGrade.sprite = Resources.Load<Sprite>($"UIElement/{afterGrade}star");
        var layoutElement = this.beforeGrade.GetComponent<LayoutElement>();
        layoutElement.minWidth = layoutElement.minHeight * int.Parse(beforeGrade);
        layoutElement = this.afterGrade.GetComponent<LayoutElement>();
        layoutElement.minWidth = layoutElement.minHeight * int.Parse(afterGrade);
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
