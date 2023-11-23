using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InvMG = InvManager;

public class ItemInvUI : MonoBehaviour
{
    public GameObject content;
    public GameObject iconPrefab;

    private Transform contentTrsf;
    private void Awake()
    {
        contentTrsf = content.GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Clear();
            gameObject.SetActive(false);
        }
    }

    public void SetEquipInventory()
    {
        foreach (var dir in InvMG.Instance.equipmentInv.Inven)
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
