using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InvMG = InvManager;

public class CardInvUI : MonoBehaviour
{
    public Transform fairyContentTrsf;
    public Transform supContentTrsf;

    public GameObject iconPrefab;

    public void ActiveUI()
    {
        gameObject.SetActive(true);
        ClearFairyCardInventory();
        ClearSupCardInventory();
        SetFairyCardInventory();
        SetSupCardInventory();
    }

    public void NonActiveUI()
    {
        ClearFairyCardInventory();
        ClearSupCardInventory();
        gameObject.SetActive(false);
    }

    public void SetFairyCardInventory()
    {
        foreach (var dir in InvMG.Instance.fairyInv.Inven)
        {
            var go = Instantiate(iconPrefab, fairyContentTrsf);
            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"ID: {dir.Key}";
            var fc = go.AddComponent<FairyCard>();
            fc = dir.Value;
        }
    }
    public void SetSupCardInventory()
    {
        foreach (var dir in InvMG.Instance.supInv.Inven)
        {
            var go = Instantiate(iconPrefab, supContentTrsf);
            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"ID: {dir.Key}";
            var sc = go.AddComponent<SupCard>();
            sc = dir.Value;
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
