using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardInfoBox : MonoBehaviour, IUIElement
{
    public enum Mode
    {
        A,
        B,
    }

    public Mode mode;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI level;
    public TextMeshProUGUI bp;
    public TextMeshProUGUI pa;
    public TextMeshProUGUI rank;

    public void Init(Card card)
    {
        SetPanel(card as FairyCard);
    }

    public void SetPanel(FairyCard fairyCard)
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        cardName.text = $"이름: {table.dic[fairyCard.ID].CharName}";
        level.text = fairyCard.Level.ToString();
        //bp.text set
        pa.text = $"{NumberToProperty(table.dic[fairyCard.ID].CharProperty)} / {NumberToPosition(table.dic[fairyCard.ID].CharPosition)}";
    }

    public string NumberToProperty(int number)
    {
        switch (number)
        {
            case 1: return "사물형";
            case 2: return "식물형";
            case 3: return "동물형";
            default: return "무속성";
        }
    }

    public string NumberToPosition(int number)
    {
        switch (number)
        {
            case 1: return "퓨어탱커";
            case 2: return "도발탱커";
            case 3: return "실드탱커";
            case 4: return "마법딜러";
            case 5: return "균형딜러";
            case 6: return "평타특딜";
            case 7: return "버퍼서폿";
            case 8: return "디버프서폿";
            case 9: return "힐러";
            default: return "무직";
        }
    }

    
}
