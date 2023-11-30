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
        //List<int> keys = new List<int>(table1.dic.Keys);
        //foreach(var item in keys)
        //{
        //    Debug.Log(item);
        //}
        //int randomKey = keys[Random.Range(0, keys.Count)];
        //Debug.Log(randomKey);
        var gacha = DrawRandomItem(table1.dic);
        Debug.Log(gacha.CharID);
    }
    private void Update()
    {

        //DrawRandomItem(table1.dic);
    }
    public void GetItem(int gachaType)
    {
        int getItem = 0;
        List<CharData> getchars = new();
        List<SupportCardData> supCard = new();
        switch (gachaType)
        {
            case 1:
                var newCard = new FairyCard(DrawRandomItem(table1.dic).CharID);
                if (!InvManager.fairyInv.Inven.ContainsKey(newCard.ID))
                {
                    InvManager.AddCard(newCard);
                }
                else
                {
                }
                break;
            case 2:
                getItem = DrawRandomItem(table2.dic).SupportID;
                break;
            case 3:
                getchars = DrawTenTimesItems(table1.dic);
                break;
                case 4:
                supCard = DrawTenTimesItems(table2.dic);
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
        }
        return result;
    }

}
