using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaLogic : MonoBehaviour
{
    private CharacterTable table1;
    private SupportCardTable table2;
    //private int charNumPlusValue = 100001;
    //private int supNumPlusVallue = 400001;
    //private int charCount = 0;
    //private int supCount = 0;
    private UI gachaScreen;
    public bool tenTimes = false;
    [SerializeField]
    private GameObject gachaSkipIcon;
    [SerializeField]
    private Sprite gachaSprite;
    [SerializeField]
    private TextMeshProUGUI gachaName;
    [SerializeField]
    private TextMeshProUGUI gachaDescription;
    private int roofTop;
    private void Awake()
    {
        table1 = DataTableMgr.GetTable<CharacterTable>();
        table2 = DataTableMgr.GetTable<SupportCardTable>();
        //charCount = table1.dic.Count;
        //supCount = table2.dic.Count;

        //var gacha = DrawRandomItem(table1.dic);
        //Debug.Log(gacha.CharID);
    }
    
    public void GetItem(int gachaType)
    {
        tenTimes = false;
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
                GachaDirect(newFairyCard.ID);
                break;
            case 2:
                var newSupportCard = new SupCard(DrawRandomItem(table2.dic).SupportID);
                if(!InvManager.supInv.Inven.ContainsKey(newSupportCard.ID))
                {
                    InvManager.AddCard(newSupportCard);
                }
                else
                {
                    SupportCardData supData = table2.dic[newSupportCard.ID];

                    var existingSupItem = new Item(10016, supData.SupportPiece);
                    InvManager.AddItem(existingSupItem);
                }

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
                tenTimes = true;
                foreach(var fairyData in newFairyDatas)
                {
                    var newFairyCards = new FairyCard(fairyData.CharID);
                    GachaDirect(newFairyCards.ID);
                }
                
                break;
            case 4:
                List<SupportCardData> newSupportDatas = DrawTenTimesItems<SupportCardData>(table2.dic);
                foreach (var supportData in newSupportDatas)
                {
                    var newSupportCards = new SupCard(supportData.SupportID);
                    if (!InvManager.supInv.Inven.ContainsKey(newSupportCards.ID))
                    {
                        InvManager.AddCard(newSupportCards);
                    }
                    else
                    {
                        SupportCardData supData = table2.dic[newSupportCards.ID];

                        var existingSupsItem = new Item(10016, supData.SupportPiece);
                        InvManager.AddItem(existingSupsItem);
                    }
                }
                break;
        }
    }
    private void GachaDirect(int ID)
    {
        SkipIconSet();
        //CharData.
        CharIllustSet(ID);
        CharDescriptionSet(ID);
    }

    private void CharDescriptionSet(int iD)
    {
        throw new System.NotImplementedException();
    }

    private void CharIllustSet(int ID)
    {
        gachaSprite = Resources.Load<Sprite>(table1.dic[ID].CharIllust);
    }

    private void SkipIconSet()
    {
        if (tenTimes)
        {
            gachaSkipIcon.SetActive(true);
        }
        else
        {
            gachaSkipIcon.SetActive(false);
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
