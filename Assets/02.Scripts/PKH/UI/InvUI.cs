using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using InvMG = InvManager;

public class InvUI : UI
{
    public List<Transform> contents = new List<Transform>();
    public List<UnityEvent<Transform>> seters = new List<UnityEvent<Transform>>();
    public GameObject iconPrefab;
    public FairyGrowthSystem fairyGrowthSys;

    public override void ActiveUI()
    {
        base.ActiveUI();
        Clear();
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

    public void SetFairyCards(Transform transform)
    {
        foreach (var dir in InvMG.fairyInv.Inven)
        {
            //var table = DataTableMgr.GetTable<CharacterTable>();
            //if (table.dic[dir.Key.ToString()].CharPosition % 3 != type && type != (int)CardTypes.All)
            //    continue;

            var go = Instantiate(iconPrefab);
            go.transform.SetParent(transform);

            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"ID: {dir.Key}";

            var cr = go.GetComponent<CardIcon>();
            cr.card = dir.Value;

            var button = go.GetComponent<Button>();
            //최적화: UI 기능이랑 GrowthSys랑 분리 고려
            button?.onClick.AddListener(() => fairyGrowthSys.ActiveUI(cr.card as FairyCard));
            button?.onClick.AddListener(fairyGrowthSys.SetRightPanel);
        }
    }
    public void SetSupCards(Transform transform)
    {
        foreach (var dir in InvMG.supInv.Inven)
        {
            var go = Instantiate(iconPrefab, transform);
            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"ID: {dir.Key}";
            var button = go.GetComponent<Button>();
            //SupCardInfoWindow ActiveUI
            //button.onClick.AddListener(cardInfoUI.ActiveUI);
            //var sc = go.AddComponent<SupCard>();
            //sc = dir.Value;
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
