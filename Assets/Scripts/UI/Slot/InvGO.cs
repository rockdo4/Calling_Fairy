using UnityEngine;

public abstract class InvGO : MonoBehaviour
{
    public InventoryItem inventoryItem;
    public abstract void Init(InventoryItem item);
}
