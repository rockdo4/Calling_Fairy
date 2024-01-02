using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ItemIcon : InvGO
{
    public Item Item
    {
        get
        {
            return (Item)inventoryItem;
        }
        private set
        {
            inventoryItem = value;
        }
    }

    public TextMeshProUGUI text;
    public Image image;

    public override void Init(InventoryItem invItem)
    {
        Item = invItem as Item;
        image.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<ItemTable>().dic[Item.ID].icon);
        UpdateCount();
    }
    public void SetStockTextTrsf(float parentSize)
    {
        var newSize = parentSize * 0.4f;
        text.rectTransform.sizeDelta = new Vector2(newSize, newSize);

        text.transform.SetLocalPositionAndRotation(new Vector3(parentSize / 2, parentSize / 2), Quaternion.identity);
    }

    public void UpdateCount()
    {
        if (text != null)
            text.text = $"x{Item.Count}";
    }
}
