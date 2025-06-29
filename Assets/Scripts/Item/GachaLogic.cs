using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ResultGacha
{
    public FairyCard Kard { get; set; }
    public bool IsNew { get; set; }
}
public class GachaLogic : MonoBehaviour
{
    public CharacterTable charTable;
    //public SupportCardTable supTable;
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
    private int boxAllPercent;
    private int upBoxAllPercent;
    private int usePercent;
    private string gachaBoxName;
    private string boxName = "Box1";
    private string upBoxName = "Box2";
    private bool gotTwoStar = false;
    [SerializeField]
    private GameObject gachaIcon;
    BoxData boxCheck;
    private void Awake()
    {
        gachaTable = DataTableMgr.GetTable<GachaTable>();

        charTable = DataTableMgr.GetTable<CharacterTable>();
        //supTable = DataTableMgr.GetTable<SupportCardTable>();
        GSUI = GetComponentInChildren<GachaSceneUI>(true);
        Test();
    }


    public void GachaPopUp(int type)
    {
        gachaIcon.SetActive(true);
        gachaType = type;

        switch (type)
        {
            case 1:
                gachaExplainText.text = GameManager.stringTable[401].Value;
                payMoney = 150;
                break;
            case 3:
                gachaExplainText.text = GameManager.stringTable[402].Value;
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
                gachaExplainText.text = GameManager.stringTable[401].Value;
                break;
            case 3:
                gachaExplainText.text = GameManager.stringTable[402].Value;
                break;
        }

    }
    public void GetItem()
    {
        gachaIcon.SetActive(false);
        resultGacha.Clear();
        tenTimes = false;
        usePercent = boxAllPercent;
        switch (gachaType)
        {
            case 1:
                //payMoney = 150;
                //int pickNum = Random.Range(0,allPercent);
                gachaBoxName = boxName;
                var newFairyCard = new FairyCard(DrawBoxItem(gachaTable.dic).box_itemID);
                //var newFairyCard = new FairyCard(DrawRandomItem(charTable.dic).CharID);
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
                /*var newSupportCard = new SupCard(DrawRandomItem(supTable.dic).SupportID);
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
                */
                break;
            case 3:
                //payMoney = 1500;
                gotTwoStar = false;
                gachaBoxName = boxName;
                Stack<int> newFairyDatas = DrawTenTimesItems(gachaTable.dic);
                foreach (var fairyData in newFairyDatas)
                {
                    var newFairyCards = new FairyCard(fairyData);
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


                break;
            case 4:
                //Stack<SupportCardData> newSupportDatas = DrawTenTimesItems<SupportCardData>(supTable.dic);
                //foreach (var supportData in newSupportDatas)
                //{
                //    var newSupportCards = new SupCard(supportData.SupportID);
                //    if (!InvManager.supInv.Inven.ContainsKey(newSupportCards.ID))
                //    {
                //        InvManager.AddCard(newSupportCards);
                //    }
                //    else
                //    {
                //        SupportCardData supData = supTable.dic[newSupportCards.ID];

                //        var existingSupsItem = new Item(10016, supData.SupportPiece);
                //        InvManager.AddItem(existingSupsItem);
                //    }
                //}
                break;
        }
    }

    private void Test()
    {
        //var gachaTable.dic[boxName];
        var oneBox = gachaTable.dic[boxName];
        var twoBox = gachaTable.dic[upBoxName];
        boxAllPercent = 0;
        upBoxAllPercent = 0;
        /*
        for (int i = 0; i < vv.Count; i++)
        {
            if (vv[boxName].box_ID == boxName)
            {
                vv = boxCheck[i];
                break;
            }
            if (boxCheck.Count == i + 1)
            {
                if (boxCheck[i].box_ID != boxName)
                {
                    Debug.Log("박스데이터가 없습니다.");
                    return;
                }
            }
        }
        for (int i = 0; i < boxData.boxDetailDatas.Count; i++)
        {
            allPercent += boxData.boxDetailDatas[i].box_itemPercent;
        }
        */
        for (int i = 0; i < oneBox.boxDetailDatas.Count; i++)
        {
            boxAllPercent += oneBox.boxDetailDatas[i].box_itemPercent;
        }
        for (int i = 0; i < twoBox.boxDetailDatas.Count; i++)
        {
            upBoxAllPercent += twoBox.boxDetailDatas[i].box_itemPercent;
        }

    }

    public T DrawRandomItem<T>(Dictionary<int, T> table)
    {
        List<int> keys = new List<int>(table.Keys);
        int randomKey = keys[Random.Range(0, keys.Count)];
        //Debug.Log(table[randomKey]);
        return table[randomKey];
    }
    public DetailBoxData DrawBoxItem(Dictionary<string, BoxData> table)
    {
        var gachaNum = Random.Range(0, usePercent);
        for (int i = 0; i < boxAllPercent; i++)
        {
            if (gachaNum <= table[gachaBoxName].boxDetailDatas[i].box_itemPercent)
            {
                if (table[gachaBoxName].boxDetailDatas[i].box_Tier >= 2)
                {
                    gotTwoStar = true;
                }
                return table[gachaBoxName].boxDetailDatas[i];
            }
            else
            {
                gachaNum -= table[gachaBoxName].boxDetailDatas[i].box_itemPercent;
            }
        }
        return table[gachaBoxName].boxDetailDatas[table[gachaBoxName].boxDetailDatas.Count - 1];
    }

    //public T DrawRandomItem<T>(Dictionary<string, T> table)
    //{

    //    List<string> keys = new List<string>(table.Keys);
    //    var tables = table;
    //    //var tableValue = tables.Values.;
    //    var tablesValue = tables.Values;

    //    return table[keys[0]];
    //}
    public Stack<int> DrawTenTimesItems(Dictionary<string, BoxData> table)
    {
        Stack<int> result = new Stack<int>();

        for (int i = 0; i < 9; i++)
        {
            result.Push(DrawBoxItem(table).box_itemID);
        }

        if (gotTwoStar)
        {
            result.Push(DrawBoxItem(table).box_itemID);
        }
        else
        {
            usePercent = upBoxAllPercent;
            gachaBoxName = upBoxName;
            result.Push(DrawBoxItem(table).box_itemID);
        }
        return result;
    }

    //public Stack<T> DrawTenTimesItems<T>(Dictionary<int, T> table)
    //{
    //    Stack<T> result = new Stack<T>();

    //    for (int i = 0; i < 9; i++)
    //    {
    //        result.Push(DrawRandomItem(table));
    //    }


    //    return result;
    //}

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
        whenPayFailPopUP.transform.GetChild(0).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = GameManager.stringTable[406].Value;
    }
}
