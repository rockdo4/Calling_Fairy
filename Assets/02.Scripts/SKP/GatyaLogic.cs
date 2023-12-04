using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatyaLogic : MonoBehaviour
{
    CharacterTable table1;
    SupportCardTable table2;
    int charNumPlusValue = 100001;
    int supNumPlusVallue = 400001;
    int charCount = 0;
    int supCount = 0;
    private void Awake()
    {
        table1 = DataTableMgr.GetTable<CharacterTable>();
        table2 = DataTableMgr.GetTable<SupportCardTable>();
        charCount = table1.dic.Count;
        supCount = table2.dic.Count;

        var gacha = DrawRandomItem(table1.dic);
        Debug.Log(gacha.CharID);
    }
    private void Update()
    {

    }

    public void GetItem(int gachaType)
    {

        switch (gachaType)
        {
            case 1:
                var newFairyCard = new FairyCard(DrawRandomItem(table1.dic).CharID);
                if (!InvManager.fairyInv.Inven.ContainsKey(newFairyCard.ID))
                {
                    InvManager.AddCard(newFairyCard);
                }
                else
                {
                    CharData charData = table1.dic[newFairyCard.ID];

                    var existingCardItem = new Item(10003, charData.CharPiece);
                    InvManager.AddItem(existingCardItem);
                }
                break;
            case 2:
                var newSupportCard = new SupCard(DrawRandomItem(table2.dic).SupportID);
                InvManager.AddCard(newSupportCard);

                break;
            case 3:
                List<CharData> newFairyDatas = DrawTenTimesItems<CharData>(table1.dic);
                foreach (var fairyData in newFairyDatas)
                {
                    var newFairyCards = new FairyCard(fairyData.CharID);
                    if (!InvManager.fairyInv.Inven.ContainsKey(newFairyCards.ID))
                    {
                        InvManager.AddCard(newFairyCards);
                    }
                    else
                    {
                        CharData charData = table1.dic[newFairyCards.ID];

                        var existingCardsItem = new Item(10003, charData.CharPiece);
                        InvManager.AddItem(existingCardsItem);
                    }
                }
                break;
            case 4:
                List<SupportCardData> newSupportDatas = DrawTenTimesItems<SupportCardData>(table2.dic);
                foreach (var supportData in newSupportDatas)
                {
                    var newSupportCards = new SupCard(supportData.SupportID);
                    InvManager.AddCard(newSupportCards);
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

    public List<T> DrawTenTimesItems<T>(Dictionary<int, T> table)
    {
        List<T> result = new List<T>();
        for (int i = 0; i < 10; i++)
        {
            result.Add(DrawRandomItem(table));
            Debug.Log($"{i}");
        }
        return result;
    }

}
