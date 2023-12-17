using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    public FairyGrowthUI fairyGrowthUI;
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
    private List<Card> tankerList = new List<Card>();
    private List<Card> dealerList = new List<Card>();
    private List<Card> strategistList = new List<Card>();

    //Fairy Category by Property
    private List<Card> objectFairyList = new List<Card>();
    private List<Card> plantFairyList = new List<Card>();
    private List<Card> animalFairyList = new List<Card>();

    private void Awake()
    {
        if (mode == Mode.GrowthUI)
        {
            dropdown?.onValueChanged?.AddListener(InvSort);
        }
    }

    public void Init()
    {
        Clear();
        if (mode == Mode.GrowthUI)
        {
            dropdown?.onValueChanged?.Invoke(0);
        }
        else if (mode == Mode.FormationUI)
        {
            InvSort(0);
        }
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

    public void InvSort(int num)
    {
        totalFairyList.Clear();
        totalFairyList = InvMG.fairyInv.Inven.Values.ToList();

        switch (num)
        {
            case 0:
                totalFairyList = totalFairyList.OrderByDescending(fairyCard => fairyCard.Level).ToList();
                break;
            case 1:
                totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.Level).ToList();
                break;
            case 2:
                totalFairyList = totalFairyList.OrderByDescending(fairyCard => fairyCard.Name).ToList();
                break;
            case 3:
                totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.Name).ToList();
                break;
            case 4:
                totalFairyList = totalFairyList.OrderByDescending(fairyCard => fairyCard.Grade).ToList();
                break;
            case 5:
                totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.Grade).ToList();
                break;
            case 6:
                totalFairyList = totalFairyList.OrderByDescending(fairyCard => fairyCard.Rank).ToList();
                break;
            case 7:
                totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.Rank).ToList();
                break;
        }

        if (mode == Mode.GrowthUI)
        {
            CategorizeByProperty(totalFairyList);
        }
        else if (mode == Mode.FormationUI)
        {
            CategorizeByPosition(totalFairyList);
        }
        Clear();
        SetInvUI();
    }

    public void CategorizeByProperty(List<FairyCard> totalFairyList)
    {
        objectFairyList.Clear();
        plantFairyList.Clear();
        animalFairyList.Clear();

        var table = DataTableMgr.GetTable<CharacterTable>();

        foreach (var fairyCard in totalFairyList)
        {
            switch (table.dic[fairyCard.ID].CharProperty)
            {
                case 1:
                    objectFairyList.Add(fairyCard);
                    break;
                case 2:
                    plantFairyList.Add(fairyCard);
                    break;
                case 3:
                    animalFairyList.Add(fairyCard);
                    break;
            }
        }   
    }

    public void CategorizeByPosition(List<FairyCard> totalFairyList)
    {
        tankerList.Clear();
        dealerList.Clear();
        strategistList.Clear();

        var table = DataTableMgr.GetTable<CharacterTable>();

        foreach (var fairyCard in totalFairyList)
        {
            switch((CardTypes)(table.dic[fairyCard.ID].CharPosition / 3))
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
                foreach (var card in list)
                {
                    var fairyIcon = CreateInvGO(card, transform) as FairyIcon;
                    var button = fairyIcon.GetComponent<Button>();
                    if (mode == Mode.GrowthUI)
                    {
                        button?.onClick.AddListener(() => fairyGrowthUI.Init(card as FairyCard));
                        button?.onClick.AddListener(fairyGrowthUI.GetComponent<UI>().ActiveUI);
                    }
                    else
                    {
                        if (card.IsUse)
                        {
                            button.enabled = !card.IsUse;
                        }
                        else
                        {
                            button.enabled = !card.IsUse;
                            button?.onClick.AddListener(() => formationSys.SetAndSortSlots(fairyIcon.inventoryItem));
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
                        button?.onClick.AddListener(fairyGrowthUI.GetComponent<UI>().ActiveUI);
                        button?.onClick.AddListener(() => fairyGrowthUI.Init(item as FairyCard));
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
