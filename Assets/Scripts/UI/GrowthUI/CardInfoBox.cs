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

        //nameText.text = fairyCard.Name;
        //cardImage.sprite = Resources.Load<Sprite>(table.dic[fairyCard.ID].CharIllust);
        level.text = $"Lv {fairyCard.Level}";
        //gradeImage
        SetPropertyColor(table.dic[fairyCard.ID].CharProperty);
        SetPositionIcon(table.dic[fairyCard.ID].CharPosition);
        expSlider.fillAmount = (float)fairyCard.Experience / expTable.dic[fairyCard.Level].Exp;
        expText.text = $"{fairyCard.Experience} / {expTable.dic[fairyCard.Level].Exp}";
    }

    public void SetPropertyColor(int number)
    {
        switch (number)
        {
            case 1:
                propertyImage.color = Color.blue;
                break;
            case 2: 
                propertyImage.color = Color.green;
                break;
            case 3: 
                propertyImage.color = Color.red;
                break;
        }
    }

    public void SetPositionIcon(int position)
    {
        switch ((position % 3) + 1)
        {
            case 1:
                positionImage.sprite = Resources.Load<Sprite>("Sprites/UI/Icon/딜러(아이콘)");
                break;
            case 2:
                positionImage.sprite = Resources.Load<Sprite>("Sprites/UI/Icon/탱커(아이콘)");
                break;
            case 3:
                positionImage.sprite = Resources.Load<Sprite>("Sprites/UI/Icon/버퍼(아이콘)");
                break;
        }
    }

}
