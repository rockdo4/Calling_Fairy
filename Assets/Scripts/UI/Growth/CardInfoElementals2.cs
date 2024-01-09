using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoElementals2 : MonoBehaviour, IUIElement
{
    public TextMeshProUGUI nameText;
    public Image cardImage;

    public void Init(Card card)
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        var fairyCard = card as FairyCard;
        nameText.text = fairyCard.Name;
        cardImage.sprite = Resources.Load<Sprite>(table.dic[fairyCard.ID].CharIllust);
    }
}
