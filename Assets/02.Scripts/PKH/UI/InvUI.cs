using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;
using InvMG = InvManager;

public class InvUI : UI
{
    public List<Transform> contents = new List<Transform>();
    public List<UnityEvent<Transform>> seters = new List<UnityEvent<Transform>>();
    public GameObject iconPrefab;
    public FairyGrowthSystem fairyGrowthSys;

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
            switch((CardTypes)(table.dic[dic.Key.ToString()].CharPosition % 3))
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
        if (list == null)
            return;
        SetSlots(transform, list);
    }

    public void SetSupCards(Transform transform)
    {
        var inven = InvMG.supInv.Inven.Values;
        var list = inven.ToList<InventoryItem>();
        if (list == null)
            return;
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
        if(!contents.Contains(transform))
        {
            contents.Add(transform);
        }

        switch (list[0].GetType())
        {
            case Type type when typeof(Card).IsAssignableFrom(type) :
                foreach (var item in list)
                {
                    var go = Instantiate(iconPrefab);
                    go.transform.SetParent(transform);

                    var slotItem = go.GetComponent<SlotItem>();
                    slotItem.Init(item);

                    if (fairyGrowthSys != null)
                    {
                        var button = go.GetComponent<Button>();
                        //최적화: UI 기능이랑 GrowthSys랑 분리 고려
                        button?.onClick.AddListener(() => fairyGrowthSys.ActiveUI(item as FairyCard));
                        button?.onClick.AddListener(fairyGrowthSys.SetRightPanel);
                    }
                    else
                    {

                    }
                }
            break;
            case Type type when typeof(Item).IsAssignableFrom(type) :
                foreach (var item in list)
                {
                    var go = Instantiate(iconPrefab);
                    go.transform.SetParent(transform);

                    var slotItem = go.GetComponent<SlotItem>();
                    slotItem.Init(item);
                }
            break;
        }
    }

    
    public void SetEquipments(Transform transform)
    {
        foreach (var dir in InvMG.equipmentInv.Inven)
        {
            if (dir.Value.Count > 0)
            {
                var go = Instantiate(iconPrefab, transform);
                var image = go.GetComponent<Image>();
                var text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.text = 'x' + dir.Value.Count.ToString();
            }
        }
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
