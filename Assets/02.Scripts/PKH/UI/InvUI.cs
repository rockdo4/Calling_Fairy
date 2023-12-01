using System;
using System.Collections.Generic;
using System.Linq;
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
    public FairyGrowthSystem fairyGrowthSys;
    public FormationSystem formationSys;
    public GameObject iconPrefab;
    public List<UnityEvent<Transform>> seters = new List<UnityEvent<Transform>>();

    private List<Transform> contents = new List<Transform>();
    private List<InventoryItem> tankerList = new List<InventoryItem>();
    private List<InventoryItem> dealerList = new List<InventoryItem>();
    private List<InventoryItem> strategistList = new List<InventoryItem>();

    public override void ActiveUI()
    {
        base.ActiveUI();
        Clear();
        SetList();
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

    public void SetList()
    {
        tankerList.Clear();
        dealerList.Clear();
        strategistList.Clear();

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
        var inven = InvMG.fairyInv.Inven.Values;
        var list = inven.ToList<InventoryItem>();

        SetSlots(transform, list);
    }

    public void SetSupCards(Transform transform)
    {
        var inven = InvMG.supInv.Inven.Values;
        var list = inven.ToList<InventoryItem>();

        SetSlots(transform, list);
    }

    public void SetTankerCards(Transform transform)
    {
        SetSlots(transform, tankerList);
    }
    public void SetDealerCards(Transform transform)
    {
        SetSlots(transform, dealerList);
    }
    public void SetStrategistCards(Transform transform)
    {
        SetSlots(transform, strategistList);
    }

    public void SetSlots(Transform transform, List<InventoryItem> list)
    {
        if (list.Count < 1)
            return;

        if(!contents.Contains(transform))
        {
            contents.Add(transform);
        }

        switch (list[0].GetType())
        {
            case Type type when typeof(Card).IsAssignableFrom(type) :
                foreach (var item in list)
                {
                    var slotItem = CreateSlotItem(item, transform);
                    var button = slotItem.GetComponent<Button>();
                    if (mode == Mode.GrowthUI)
                    {
                        button?.onClick.AddListener(fairyGrowthSys.GetComponent<UI>().ActiveUI);
                        button?.onClick.AddListener(() => fairyGrowthSys.Init(item as FairyCard));
                    }
                    else if (mode == Mode.FormationUI)
                    {
                        button?.onClick.AddListener(() => formationSys.SelectSlot.SetSlot(slotItem));
                        button?.onClick.AddListener(NonActiveUI);
                    }
                }
            break;
            case Type type when typeof(Item).IsAssignableFrom(type) :
                foreach (var item in list)
                {
                    var go = CreateSlotItem(item, transform);
                    var slotItem = go.GetComponent<SlotItem>();
                    slotItem.Init(item);
                }
            break;
        }
    }

    public SlotItem CreateSlotItem(InventoryItem item, Transform transform)
    {
        var go = Instantiate(iconPrefab);
        go.transform.SetParent(transform);
        var slotItem = go.GetComponent<SlotItem>();
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
