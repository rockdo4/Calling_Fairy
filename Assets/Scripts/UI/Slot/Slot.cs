using UnityEngine;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour, IUIElement
{
    public int slotNumber;
    protected Button button;
    public SlotGroupBase SlotGroup { get; set; } = null;
    public InventoryItem SelectedInvenItem { get; private set; } = null;
    public bool IsInitialized { get; protected set; } = false;
   

    public virtual void Init(Card card)
    {
        button = GetComponent<Button>();
        IsInitialized = true;
    }

    public virtual void SetSlot(InventoryItem item)
    {
        SelectedInvenItem = item;
    }

    public virtual void UnsetSlot()
    {       
        SelectedInvenItem = null;
    }
}
