using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoBox : MonoBehaviour, IUIElement
{
    public TextMeshProUGUI level;
    public Image gradeImage;
    public Image propertyImage;
    public Image positionImage;
    public Image expSlider;
    public TextMeshProUGUI expText;

    public void Init(Card card)
    {
        var fairyCard = card as FairyCard;
        var table = DataTableMgr.GetTable<CharacterTable>();
        var expTable = DataTableMgr.GetTable<ExpTable>();

        level.text = $"Lv {fairyCard.Level}";
        SetGradeImage(fairyCard.Grade);
        SetPropertyColor(table.dic[fairyCard.ID].CharProperty);
        SetPositionIcon(table.dic[fairyCard.ID].CharPosition);
        expSlider.fillAmount = (float)fairyCard.Experience / expTable.dic[fairyCard.Level].Exp;
        expText.text = $"{fairyCard.Experience} / {expTable.dic[fairyCard.Level].Exp}";
    }

    public void SetGradeImage(int grade)
    {
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
