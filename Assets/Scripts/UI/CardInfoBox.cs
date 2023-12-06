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
        cardName.text = $"�̸�: {table.dic[fairyCard.ID].CharName}";
        level.text = fairyCard.Level.ToString();
        //bp.text set
        pa.text = $"{NumberToProperty(table.dic[fairyCard.ID].CharProperty)} / {NumberToPosition(table.dic[fairyCard.ID].CharPosition)}";
    }

    public string NumberToProperty(int number)
    {
        switch (number)
        {
            case 1: return "�繰��";
            case 2: return "�Ĺ���";
            case 3: return "������";
            default: return "���Ӽ�";
        }
    }

    public string NumberToPosition(int number)
    {
        switch (number)
        {
            case 1: return "ǻ����Ŀ";
            case 2: return "������Ŀ";
            case 3: return "�ǵ���Ŀ";
            case 4: return "��������";
            case 5: return "��������";
            case 6: return "��ŸƯ��";
            case 7: return "���ۼ���";
            case 8: return "���������";
            case 9: return "����";
            default: return "����";
        }
    }

    
}
