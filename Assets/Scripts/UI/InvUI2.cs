using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InvUI2 : UI
{
    public PlayerInfoBox playerInfoBox;
    public TabGroup tabGroup;
    public GameObject elementPrefab;
    public List<UnityEvent<Transform>> seters = new ();

    private List<Transform> contents = new ();

    private List<FairyCard> totalFairyList = new List<FairyCard>();
    private List<Card> tankerList = new List<Card>();
    private List<Card> dealerList = new List<Card>();
    private List<Card> strategistList = new List<Card>();

    public void Init()
    {
        Clear();
        SetList();
        SetInvUI();

        tabGroup.OnTabSelected(tabGroup.tabButtons[0]);
    }

    public void SetList()
    {
        totalFairyList = InvManager.fairyInv.Inven.Values.ToList();
        totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.ID).ToList();
        CategorizeByPosition(totalFairyList);
    }

    public void CategorizeByPosition(List<FairyCard> totalFairyList)
    {
        tankerList.Clear();
        dealerList.Clear();
        strategistList.Clear();

        var table = DataTableMgr.GetTable<CharacterTable>();

        foreach (var fairyCard in totalFairyList)
        {
            switch ((CardTypes)(table.dic[fairyCard.ID].CharPosition / 3))
            {
                case CardTypes.Tanker:
                    tankerList.Add(fairyCard);
                    break;
                case CardTypes.Dealer:
                    dealerList.Add(fairyCard);
                    break;
                case CardTypes.Strategist:
                    strategistList.Add(fairyCard);
                    break;
            }
        }
    }
    public void SetFairyCards(Transform transform)
    {
        List<Card> newTotalFairyList = totalFairyList.Cast<Card>().ToList();
        SetSlots(transform, newTotalFairyList);
    }
    public void SetTankerFairys(Transform transform)
    {
        SetSlots(transform, tankerList);
    }
    public void SetDealerFairys(Transform transform)
    {
        SetSlots(transform, dealerList);
    }
    public void SetBufferFairys(Transform transform)
    {
        SetSlots(transform, strategistList);
    }

    public override void ActiveUI()
    {
        base.ActiveUI();
        Init();
    }

    public override void NonActiveUI()
    {
        Clear();
        base.NonActiveUI();
    }

    public void SetInvUI()
    {
        foreach (var seter in seters)
        {
            seter.Invoke(null);
        }
    }

    public void SetSlots(Transform transform, List<Card> list)
    {
        if (list.Count < 1)
            return;

        if (!contents.Contains(transform))
        {
            contents.Add(transform);
        }

        foreach (var invItem in list)
        {
            var go = UIManager.Instance.objPoolMgr.GetGo("FairyIcon_250x250");
            go.transform.SetParent(transform);

            var fairyIcon = go.GetComponent<FairyIcon>();
            fairyIcon.Init(invItem);
            fairyIcon.battlePowerText.gameObject.SetActive(false);

            var button = go.GetComponent<Button>();
            button?.onClick.AddListener(() => playerInfoBox.SelectMainFairy(invItem.ID));
            button?.onClick.AddListener(NonActiveUI);
        }
    }

    public void Clear()
    {
        foreach (var content in contents)
        {
            var poolGos = content.GetComponentsInChildren<PoolAble>();

            foreach (var poolGo in poolGos)
            {
                poolGo.GetComponent<Button>()?.onClick.RemoveAllListeners();
                UIManager.Instance.objPoolMgr.ReturnGo(poolGo.gameObject);
                poolGo.transform.SetParent(UIManager.Instance.objPoolMgr.transform);
            }
        }
    }
}
