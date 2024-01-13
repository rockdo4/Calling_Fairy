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
    public FairyGrowthUI fairyGrowthUI;
    public TMP_Dropdown dropdown;
    public TabGroup fairyTabGroup;

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
            int index = 336;
            foreach (var option in dropdown.options)
            {
                option.text = GameManager.stringTable[index++].Value;
            }

            dropdown.value = 0;
            dropdown?.onValueChanged?.Invoke(0);
            fairyTabGroup.OnTabSelected(fairyTabGroup.tabButtons[0]);
        }
        else if (mode == Mode.FormationUI)
        {
            InvSort(9);
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
            case 8:
                totalFairyList = totalFairyList.OrderBy(fairyCard => fairyCard.FinalStat.battlePower).ToList();
                break;
            case 9:
                totalFairyList = totalFairyList.OrderByDescending(fairyCard => fairyCard.FinalStat.battlePower).ToList();
                break;
        }

        CategorizeByPosition(totalFairyList);
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

        foreach (var card in list)
        {
            if (mode == Mode.GrowthUI)
            {
                var go = UIManager.Instance.objPoolMgr.GetGo("FairyIcon");
                go.transform.SetParent(transform);
                go.GetComponent<FairyIcon>().Init(card as FairyCard);
                var button = go.GetComponent<Button>();

                button?.onClick.AddListener(() => fairyGrowthUI.Init(card as FairyCard));
                button?.onClick.AddListener(fairyGrowthUI.GetComponent<UI>().ActiveUI);
            }
            else // Mode.FormationUI
            {
                var go = UIManager.Instance.objPoolMgr.GetGo("FairyIcon_250x250");
                go.transform.SetParent(transform);
                var fairyIcon = go.GetComponent<FairyIcon>();
                fairyIcon.Init(card as FairyCard);
                fairyIcon.battlePowerText.gameObject.SetActive(true);
                var button = go.GetComponent<Button>();
                button.interactable = !card.IsUse;

                var InvGo = go.GetComponent<InvGO>();

                button?.onClick.AddListener(() => formationSys.SetAndSortSlots(InvGo.inventoryItem));
                button?.onClick.AddListener(() => button.interactable = !card.IsUse);
                button?.onClick.AddListener(() =>
                {
                    if (formationSys.SelectedGroup.slots[2].SelectedInvenItem != null)
                    {
                        NonActiveUI();
                        formationSys.SelectedGroup = null;
                    }
                });
            }
        }
    }


    public void Clear()
    {
        foreach (var content in contents)
        {
            var poolGos = content.GetComponentsInChildren<PoolAble>();

            foreach (var go in poolGos)
            {
                var button = go.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.enabled = true;

                UIManager.Instance.objPoolMgr.ReturnGo(go.gameObject);
                go.transform.SetParent(UIManager.Instance.objPoolMgr.transform);
            }
        }
    }
}
