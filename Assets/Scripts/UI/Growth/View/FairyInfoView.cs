using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyInfoView : GrowthView
{
    public TextMeshProUGUI nameText;
    public Image cardImage;

    public TextMeshProUGUI level;
    public TextMeshProUGUI rankText;
    public Image gradeImage;
    public Image propertyImage;
    public Image positionImage;
    public Image expSlider;
    public TextMeshProUGUI expText;

    public override void UpdateUI()
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        var expTable = DataTableMgr.GetTable<ExpTable>();

        nameText.text = controller.SelectFairy.Name;
        cardImage.sprite = Resources.Load<Sprite>(table.dic[controller.SelectFairy.ID].CharIllust);

        level.text = $"Lv.{controller.SelectFairy.Level}";
        if (rankText != null)
            rankText.text = $"Rank.{controller.SelectFairy.Rank}";
        SetGradeImage(controller.SelectFairy.Grade);
        SetPropertyColor(table.dic[controller.SelectFairy.ID].CharProperty);
        SetPositionIcon(table.dic[controller.SelectFairy.ID].CharPosition);
        expSlider.fillAmount = (float)controller.SelectFairy.Experience / expTable.dic[controller.SelectFairy.Level].Exp;
        expText.text = $"{controller.SelectFairy.Experience} / {expTable.dic[controller.SelectFairy.Level].Exp}";
    }


    public void SetGradeImage(int grade)
    {
        if (gradeImage == null)
            return;
        gradeImage.sprite = Resources.Load<Sprite>($"UIElement/{grade}star");
        gradeImage.rectTransform.sizeDelta = new Vector2(50 * grade, 50);
    }

    public void SetPropertyColor(int number)
    {
        switch (number)
        {
            case 1:
                propertyImage.sprite = Resources.Load<Sprite>("UIElement/Object");
                break;
            case 2:
                propertyImage.sprite = Resources.Load<Sprite>("UIElement/Plant");
                break;
            case 3:
                propertyImage.sprite = Resources.Load<Sprite>("UIElement/Animal");
                break;
        }
    }

    public void SetPositionIcon(int position)
    {
        switch ((position / 3) + 1)
        {
            case 1:
                positionImage.sprite = Resources.Load<Sprite>("UIElement/Tanker");
                break;
            case 2:
                positionImage.sprite = Resources.Load<Sprite>("UIElement/Dealer");
                break;
            case 3:
                positionImage.sprite = Resources.Load<Sprite>("UIElement/Supporter");
                break;
        }
    }
}
