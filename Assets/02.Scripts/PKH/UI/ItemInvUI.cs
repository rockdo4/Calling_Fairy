using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InvMG = InvManager;

public class ItemInvUI :    UI
{
    public GameObject iconPrefab;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NonActiveUI();
        }
    }

    public void ActiveUI()
    {
        Clear();
        SetEquipInventory();
        base.ActiveUI();
    }

    public void NonActiveUI()
    {
        Clear();
        base.NonActiveUI();
    }

    public void SetEquipInventory()
    {
        foreach (var dir in InvMG.equipmentInv.Inven)
        {
            if (dir.Value.Count > 0)
            {
                var go = Instantiate(iconPrefab, contentTrsf);
                var image = go.GetComponent<Image>();
                var text = go.GetComponentInChildren<TextMeshProUGUI>();
                text.text = 'x' + dir.Value.Count.ToString();
            }
        }
    }

    public void Clear()
    {
        for ( int i = contentTrsf.childCount - 1; i >= 0 ; i-- )
        {
            var child = contentTrsf.GetChild( i );
            Destroy( child.gameObject );
        }
    }
}
