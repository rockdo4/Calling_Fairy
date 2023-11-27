using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InvMG = InvManager;

public class CardInvUI : UI
{
    public Transform fairyContentTrsf;
    public Transform supContentTrsf;
    public GameObject iconPrefab;
    public FairyInfoUI cardInfoUI;

    public void ActiveUI()
    {
        base.ActiveUI();
        ClearFairyCardInventory();
        ClearSupCardInventory();
        SetFairyCardInventory();
        SetSupCardInventory();
    }

    public void NonActiveUI()
    {
        ClearFairyCardInventory();
        ClearSupCardInventory();
        base.NonActiveUI();
    }

    public void SetFairyCardInventory()
    {
        foreach (var dir in InvMG.fairyInv.Inven)
        {
            var go = Instantiate(iconPrefab, fairyContentTrsf);
            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"ID: {dir.Key}";
            var cr = go.GetComponent<CardIcon>();
            cr.card = dir.Value;
            var button = go.GetComponent<Button>();
            button.onClick.AddListener(cardInfoUI.ActiveUI);
            button.onClick.AddListener(() => cardInfoUI.SetRightPanel(cr.card));
        }
    }
    public void SetSupCardInventory()
    {
        foreach (var dir in InvMG.supInv.Inven)
        {
            var go = Instantiate(iconPrefab, supContentTrsf);
            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"ID: {dir.Key}";
            var button = go.GetComponent<Button>();
            //SupCardInfoWindow ActiveUI
            //button.onClick.AddListener(cardInfoUI.ActiveUI);
            //var sc = go.AddComponent<SupCard>();
            //sc = dir.Value;
        }
    }

    public void ClearFairyCardInventory()
    {
        for (int i = fairyContentTrsf.childCount - 1; i >= 0; i--)
        {
            var child = fairyContentTrsf.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void ClearSupCardInventory()
    {
        for (int i = supContentTrsf.childCount - 1; i >= 0; i--)
        {
            var child = supContentTrsf.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
