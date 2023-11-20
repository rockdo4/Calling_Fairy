using static UnityEditor.Progress;

public interface IInventory
{
    void AddItem(Item item);
    void RemoveItem(Item item);
}

