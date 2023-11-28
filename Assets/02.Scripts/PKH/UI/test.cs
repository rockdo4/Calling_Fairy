using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InvMG = InvManager;

public class test : UI
{
    public Transform contentTrsf;
    public enum Mode
    {
        Item,
        Card
    }

    public Mode mode;
    public GameObject iconPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NonActiveUI();
        }
    }

    public override void ActiveUI()
    {
        Clear();
        if (mode == Mode.Item)
        {
            SetEquipInv();
        }
        else
        {

        }
        base.ActiveUI();
    }

    public override void NonActiveUI()
    {
        Clear();
        base.NonActiveUI();
    }

    public void SetEquipInv()
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

    public void SetFairyCardInv()
    {
        foreach (var dir in InvMG.fairyInv.Inven)
        {
            var go = Instantiate(iconPrefab, contentTrsf);
            var button = go.GetComponent<Button>();
            var text = go.GetComponentInChildren<TextMeshProUGUI>();

            //button.onClick.AddListener();
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
