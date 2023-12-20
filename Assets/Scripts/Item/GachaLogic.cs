using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using UnityEngine;

public class ResultGacha
{
    public FairyCard Kard { get; set; }
    public bool IsNew { get; set; }
}
public class GachaLogic : MonoBehaviour
{
    public CharacterTable charTable;
    public SupportCardTable supTable;
    public GachaTable gachaTable;
    public GameObject whenPayFailPopUP;
    public UI gachaMoniter;
    //private int charNumPlusValue = 100001;
    //private int supNumPlusVallue = 400001;
    //private int charCount = 0;
    //private int supCount = 0;
    private int payMoney;
    [HideInInspector]
    public bool tenTimes = false;
    public Queue<ResultGacha> resultGacha = new Queue<ResultGacha>();
    private int roofTop;
    private GachaSceneUI GSUI;
    private int gachaType;
    public TextMeshProUGUI gachaExplainText;
    [SerializeField]
    private GameObject gachaIcon;
    private void Awake()
    {
        gachaTable = DataTableMgr.GetTable<GachaTable>();
        charTable = DataTableMgr.GetTable<CharacterTable>();
        supTable = DataTableMgr.GetTable<SupportCardTable>();
        GSUI = GetComponentInChildren<GachaSceneUI>(true);
    }


    public void GachaPopUp(int type)
    {
        gachaIcon.SetActive(true);
        gachaType = type;

        switch (type)
        {
            case 1:
                gachaExplainText.text = GameManager.stringTable[50].Value;
                payMoney = 150;
                break;
            case 3:
                gachaExplainText.text = GameManager.stringTable[51].Value;
                payMoney = 1500;
                break;
        }

    }
    public void GachaPopUp()
    {
        gachaIcon.SetActive(true);
        //gachaType

        switch (gachaType)
        {
            case 1:
                gachaExplainText.text = GameManager.stringTable[50].Value;
                break;
            case 3:
                gachaExplainText.text = GameManager.stringTable[51].Value;
                break;
        }

    }
    public void GetItem()
    {
        gachaIcon.SetActive(false);
        resultGacha.Clear();
        tenTimes = false;
        switch (gachaType)
        {
            case 1:
                payMoney = 150;
                var newFairyCard = new FairyCard(DrawRandomItem(fairyData.CharID);
                if (!InvManager.fairyInv.Inven.ContainsKey(newFairyCard.ID))
                {
                    InvManager.AddCard(newFairyCard);
                    resultGacha.Enqueue(new ResultGacha { Kard = newFairyCard, IsNew = true });
                }
                else
                {
                    CharData charData = charTable.dic[newFairyCard.ID];

                    var existingCardItem = new Item(10003, charData.CharPiece);
                    InvManager.AddItem(existingCardItem);
                    resultGacha.Enqueue(new ResultGacha { Kard = newFairyCard, IsNew = false });
                }

                GSUI.GachaDirect(newFairyCard.ID);
                break;
            case 2:
                var newSupportCard = new SupCard(DrawRandomItem(supTable.dic).SupportID);
                if (!InvManager.supInv.Inven.ContainsKey(newSupportCard.ID))
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
                        resultGacha.Enqueue(new ResultGacha { Kard = newFairyCards, IsNew = true });
                    }
                    else
                    {
                        CharData charData = charTable.dic[newFairyCards.ID];

                        var existingCardsItem = new Item(10003, charData.CharPiece);
                        InvManager.AddItem(existingCardsItem);
                        resultGacha.Enqueue(new ResultGacha { Kard = newFairyCards, IsNew = false });
                    }
                }
                tenTimes = true;
                GSUI.GachaDirect(newFairyDatas);
                //foreach(var fairyData in newFairyDatas)
                //{
                //    var newFairyCards = new FairyCard(fairyData.CharID);
                //    GSUI.GachaDirect(newFairyCards.ID);
                //}

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

    public T DrawRandomItem<T>(Dictionary<string, T> table)
    {
        
        List<string> keys = new List<string>(table.Keys);
        var tables = table;
        //var tableValue = tables.Values.;
        var tablesValue = tables.Values;
        Debug.Log(table[keys[0]]);
        return table[keys[0]];
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

    public void PaySummonStone()
    {
        if (Player.Instance.UseSummonStone(payMoney))
        {
            GetItem();
            gachaMoniter.ActiveUI();
        }
        else
        {
            FailPopUP();
        }
    }

    private void FailPopUP()
    {
        whenPayFailPopUP.SetActive(true);
        whenPayFailPopUP.transform.GetChild(0).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = GameManager.stringTable[61].Value;
    }
}
