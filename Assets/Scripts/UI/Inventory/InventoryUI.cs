using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Transform InvenContents;
    [SerializeField]
    private GameObject ItemPrefab;

    private void OnEnable()
    {
        UpdateUI();
    }

    private void OnDisable()
    {
        foreach (Transform child in InvenContents)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateUI()
    {
        var itemDic = InvManager.itemInv.Inven;
        foreach (var item in itemDic)
        {
            var obj = Instantiate(ItemPrefab, InvenContents);
            obj.GetComponent<InventoryItemIcon>().SetData(item.Value.ID, item.Value.Count);
        }
        var EquitPieceDic = InvManager.equipPieceInv.Inven;
        foreach (var item in EquitPieceDic)
        {
            var obj = Instantiate(ItemPrefab, InvenContents);
            obj.GetComponent<InventoryItemIcon>().SetData(item.Value.ID, item.Value.Count);
        }
        var SpiritStoneDic = InvManager.spiritStoneInv.Inven;
        foreach (var item in SpiritStoneDic)
        {
            var obj = Instantiate(ItemPrefab, InvenContents);
            obj.GetComponent<InventoryItemIcon>().SetData(item.Value.ID, item.Value.Count);
        }
    }
}
