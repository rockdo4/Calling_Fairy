using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using UnityEngine;

public class GachaLogic : MonoBehaviour
{
    public CharacterTable charTable;
    public SupportCardTable supTable;
    //private int charNumPlusValue = 100001;
    //private int supNumPlusVallue = 400001;
    //private int charCount = 0;
    //private int supCount = 0;
    [HideInInspector]
    public bool tenTimes = false;
    
    private int roofTop;
    private GachaSceneUI GSUI;
    private void Awake()
    {
        charTable = DataTableMgr.GetTable<CharacterTable>();
        supTable = DataTableMgr.GetTable<SupportCardTable>();
        GSUI = GetComponentInChildren<GachaSceneUI>(true);
    }
    
    public void GetItem(int gachaType)
    {
        tenTimes = false;
        switch (gachaType)
        {
            case 1:
                var newFairyCard = new FairyCard(DrawRandomItem(charTable.dic).CharID);
                if (!InvManager.fairyInv.Inven.ContainsKey(newFairyCard.ID))
                {
                    InvManager.AddCard(newFairyCard);
                }
                else
                {
                    CharData charData = charTable.dic[newFairyCard.ID];

                    var existingCardItem = new Item(10003, charData.CharPiece);
                    InvManager.AddItem(existingCardItem);
                }
                
                GSUI.GachaDirect(newFairyCard.ID);
                break;
            case 2:
                var newSupportCard = new SupCard(DrawRandomItem(supTable.dic).SupportID);
                if(!InvManager.supInv.Inven.ContainsKey(newSupportCard.ID))
                {
                    InvManager.AddCard(newSupportCard);
                }
                else
                {
                    SupportCardData supData = supTable.dic[newSupportCard.ID];

                    var existingSupItem = new Item(10016, supData.SupportPiece);
                    InvManager.AddItem(existingSupItem);
                }

                break;
            case 3:
                Stack<CharData> newFairyDatas = DrawTenTimesItems<CharData>(charTable.dic);
                foreach (var fairyData in newFairyDatas)
                {
                    var newFairyCards = new FairyCard(fairyData.CharID);
                    if (!InvManager.fairyInv.Inven.ContainsKey(newFairyCards.ID))
                    {
                        InvManager.AddCard(newFairyCards);
                    }
                    else
                    {
                        CharData charData = charTable.dic[newFairyCards.ID];

                        var existingCardsItem = new Item(10003, charData.CharPiece);
                        InvManager.AddItem(existingCardsItem);
                    }
                }
                tenTimes = true;
                GSUI.GachaDirect(newFairyDatas);
                foreach(var fairyData in newFairyDatas)
                {
                    var newFairyCards = new FairyCard(fairyData.CharID);
                    GSUI.GachaDirect(newFairyCards.ID);
                }
                
                break;
            case 4:
                Stack<SupportCardData> newSupportDatas = DrawTenTimesItems<SupportCardData>(supTable.dic);
                foreach (var supportData in newSupportDatas)
                {
                    var newSupportCards = new SupCard(supportData.SupportID);
                    if (!InvManager.supInv.Inven.ContainsKey(newSupportCards.ID))
                    {
                        InvManager.AddCard(newSupportCards);
                    }
                    else
                    {
                        SupportCardData supData = supTable.dic[newSupportCards.ID];

                        var existingSupsItem = new Item(10016, supData.SupportPiece);
                        InvManager.AddItem(existingSupsItem);
                    }
                }
                break;
        }
    }

    public T DrawRandomItem<T>(Dictionary<int, T> table)
    {
        List<int> keys = new List<int>(table.Keys);
        int randomKey = keys[Random.Range(0, keys.Count)];
        //Debug.Log(table[randomKey]);
        return table[randomKey];
    }

    public Stack<T> DrawTenTimesItems<T>(Dictionary<int, T> table)
    {
        Stack<T> result = new Stack<T>();
        for (int i = 0; i < 10; i++)
        {
            result.Push(DrawRandomItem(table));
            Debug.Log($"{i}");
        }
        return result;
    }

}
