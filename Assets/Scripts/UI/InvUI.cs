using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using InvMG = InvManager;

public class InvUI : UI
{
    public enum Mode
    {
        GrowthUI,
        FormationUI,
    }

    public Mode mode;

    //FairyGrowthUI
    public FairyGrowthUI fairyGrowthSys;
    public TMP_Dropdown dropdown;
    public TabGroup cardTabGroup;
    public TabGroup fairyTabGroup;
    public TabGroup supGroup;

    //FormationSystem
    public FormationSystem formationSys;

    //Conmmon
    public GameObject iconPrefab;
    public List<UnityEvent<Transform>> seters = new List<UnityEvent<Transform>>();
    
    private List<Transform> contents = new List<Transform>();
    private List<FairyCard> totalFairyList = new List<FairyCard>();
    //Fairy Category by Position
    private List<InventoryItem> tankerList = new List<InventoryItem>();
    private List<InventoryItem> dealerList = new List<InventoryItem>();
    private List<InventoryItem> strategistList = new List<InventoryItem>();

    //Fairy Category by Property
    private List<Card> objectFairyList = new List<Card>();
    private List<Card> plantFairyList = new List<Card>();
    private List<Card> animalFairyList = new List<Card>();

    private void Awake()
    {
        dropdown.onValueChanged.AddListener(InvSort);
    }
    public override void ActiveUI()
    {
        base.ActiveUI();
        Clear();
        CategorizeByProperty();
        CategorizeByPosition();
        SetInvUI();
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

    public void InvSort(int num)
    {
        switch (num)
        {
            case 0:
                totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.Level).ToList();
                break;
            case 1:
                totalFairyList = totalFairyList.OrderByDescending(fairyCard => fairyCard.Level).ToList();
                break;
            case 2:
                totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.Name).ToList();
                break;
            case 3:
                totalFairyList = totalFairyList.OrderByDescending(fairyCard => fairyCard.Name).ToList();
                break;
            case 4:
                totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.Grade).ToList();
                break;
            case 5:
                totalFairyList = totalFairyList.OrderByDescending(fairyCard => fairyCard.Grade).ToList();
                break;
            case 6:
                totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.Rank).ToList();
                break;
            case 7:
                totalFairyList = totalFairyList.OrderByDescending(fairyCard => fairyCard.Rank).ToList();
                break;
        }
        Clear();
        SetInvUI();
    }

    public void CategorizeByProperty()
    {
        objectFairyList.Clear();
        plantFairyList.Clear();
        animalFairyList.Clear();
        totalFairyList.Clear();

        totalFairyList = InvMG.fairyInv.Inven.Values.ToList();

        var table = DataTableMgr.GetTable<CharacterTable>();

        foreach (var dic in InvMG.fairyInv.Inven)
        {
            switch (table.dic[dic.Key].CharProperty)
            {
                case 1:
                    objectFairyList.Add(dic.Value);
                    break;
                case 2:
                    plantFairyList.Add(dic.Value);
                    break;
                case 3:
                    animalFairyList.Add(dic.Value);
                    break;
            }
        }   
    }

    public void CategorizeByPosition()
    {
        tankerList.Clear();
        dealerList.Clear();
        strategistList.Clear();
        totalFairyList.Clear();

        totalFairyList = InvMG.fairyInv.Inven.Values.ToList();

        var inven = InvMG.fairyInv.Inven;
        var table = DataTableMgr.GetTable<CharacterTable>();
        var list = new List<InventoryItem>();
        foreach (var dic in inven)
        {
            switch((CardTypes)(table.dic[dic.Key].CharPosition % 3))
            {
                case CardTypes.Tanker:
                    tankerList.Add(dic.Value);
                    break;
                case CardTypes.Dealer:
                    dealerList.Add(dic.Value);
                    break;
                case CardTypes.Strategist:
                    strategistList.Add(dic.Value);
                    break;
            }
        }
    }

    public void SetFairyCards(Transform transform)
    {
        List<Card> newTotalFairyList = totalFairyList.Cast<Card>().ToList();
        SetSlots(transform, newTotalFairyList);
    }

    public void SetSupCards(Transform transform)
    {
        var inven = InvMG.supInv.Inven.Values;
        var list = inven.ToList<Card>();

        SetSlots(transform, list);
    }
    
    public void SetObjectFairys(Transform transform)
    {
        SetSlots(transform, objectFairyList);
    }
    public void SetPlantFairys(Transform transform)
    {
        SetSlots(transform, plantFairyList);
    }
    public void SetAnimalFairys(Transform transform)
    {
        SetSlots(transform, animalFairyList);
    }

    public void SetTankerCards(Transform transform)
    {
        //SetSlots(transform, tankerList);
    }
    public void SetDealerCards(Transform transform)
    {
        //SetSlots(transform, dealerList);
    }
    public void SetStrategistCards(Transform transform)
    {
        //SetSlots(transform, strategistList);
    }

    public void SetSlots(Transform transform, List<Card> list)
    {
        if (list.Count < 1)
            return;

        if(!contents.Contains(transform))
        {
            contents.Add(transform);
        }

        switch (list[0].GetType())
        {
            case Type type when typeof(FairyCard).IsAssignableFrom(type) :
                foreach (var item in list)
                {
                    var cardButton = CreateInvGO(item, transform) as CardButton;
                    var button = cardButton.GetComponent<Button>();
                    if (mode == Mode.GrowthUI)
                    {
                        button?.onClick.AddListener(fairyGrowthSys.GetComponent<UI>().ActiveUI);
                        button?.onClick.AddListener(() => fairyGrowthSys.Init(item as FairyCard));
                    }
                    else if (mode == Mode.FormationUI)
                    {
                        var card = item as Card;
                        if (card.IsUse)
                        {
                            button.enabled = !card.IsUse;
                        }
                        else
                        {
                            button.enabled = !card.IsUse;
                            button?.onClick.AddListener(() => formationSys.SortSetFairy2(cardButton.inventoryItem));
                            button?.onClick.AddListener(NonActiveUI);
                        }
                    }
                }
            break;
            case Type type when typeof(SupCard).IsAssignableFrom(type):
                foreach (var item in list)
                {
                    var slotItem = CreateInvGO(item, transform);
                    var button = slotItem.GetComponent<Button>();
                    if (mode == Mode.GrowthUI)
                    {
                        button?.onClick.AddListener(fairyGrowthSys.GetComponent<UI>().ActiveUI);
                        button?.onClick.AddListener(() => fairyGrowthSys.Init(item as FairyCard));
                    }
                    else if (mode == Mode.FormationUI)
                    {   
                    }
                }
                break;
            //case Type type when typeof(Item).IsAssignableFrom(type) :
            //    foreach (var item in list)
            //    {
            //        var go = CreateSlotItem(item, transform);
            //        var slotItem = go.GetComponent<SlotItem>();
            //        slotItem.Init(item);
            //    }
            //break;
        }
    }

    public InvGO CreateInvGO(InventoryItem item, Transform transform)
    {
        var go = Instantiate(iconPrefab);
        go.transform.SetParent(transform);
        var slotItem = go.GetComponent<InvGO>();
        slotItem.Init(item);
        return slotItem;
    }

    public void Clear()
    {
        foreach (var content in contents)
        {
            for (int i = content.transform.childCount - 1; i >= 0; i--)
            {
                var child = content.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }
}
