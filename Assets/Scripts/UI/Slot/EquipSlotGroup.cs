public class EquipSlotGroup : SlotGroup<EquipSlot>, IUIElement
{
    public void Init(Card card)
    {
        foreach (var slot in slots)
        {
            slot.Init(card);
        }
    }
}
