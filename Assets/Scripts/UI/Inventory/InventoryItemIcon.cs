using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemIcon : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image image;    
    
    public void SetData(int ID, int count)
    {
        var path = DataTableMgr.GetTable<ItemTable>().dic[ID].icon;
        image.sprite = Resources.Load<Sprite>(path);
        text.text = count.ToString();
    }
}
