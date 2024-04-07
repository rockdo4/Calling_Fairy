using System.Collections.Generic;

public abstract class SlotGroup<T> : SlotGroupBase where T : Slot
{
    public List<T> slots = new List<T>();
    public T SelectedSlot { get; set; }

    protected void Awake()
    {
        foreach (var slot in slots)
        {
            slot.SlotGroup = this;
        }
    }

    public void Init()
    {
        foreach (var slot in slots)
        {
            slot.SlotGroup = this;
        }
    }
}
