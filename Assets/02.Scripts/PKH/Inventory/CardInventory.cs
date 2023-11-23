using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventory<T> where T : Card
{
    private Dictionary<long, T> inventory = new Dictionary<long, T>();

    public Dictionary<long, T> Inven
    {
        get { return inventory; }
        set { inventory = value; }
    }

    public void AddItem(T card)
    {
        if (!inventory.TryGetValue(card.PrivateID, out T value))
        {
            inventory.Add(card.PrivateID, card);
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
