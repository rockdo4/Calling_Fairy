using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventory<T> where T : Card
{
    private Dictionary<int, T> inventory = new Dictionary<int, T>();

    public Dictionary<int, T> Inven
    {
        get { return inventory; }
        set { inventory = value; }
    }

    public void AddItem(T card)
    {
        if (!inventory.TryGetValue(card.PrivateID, out T value))
        {
            inventory.TryAdd(card.PrivateID, card);
        }
    }


    public void RemoveItem(T card)
    {
        if (inventory.TryGetValue(card.PrivateID, out T value))
        {
            inventory.Remove(card.PrivateID);
        }
    }

}
